using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Helpers;

using DXA.Modules.Forms.Areas.Forms.FormHandlers;
using DXA.Modules.Forms.Areas.Forms.Models;
using DXA.Modules.Forms.Areas.Forms.Providers;
using Newtonsoft.Json.Linq;

using Sdl.Web.Common.Logging;
using Sdl.Web.Common.Models;
using Sdl.Web.Mvc.Configuration;
using Sdl.Web.Mvc.Controllers;

namespace DXA.Modules.Forms.Areas.Forms.Controllers
{
    //
    public class FormsController : EntityController
    {
        protected override ViewModel EnrichModel(ViewModel model)
        {


            FormCollectionModel baseFormCollection = base.EnrichModel(model) as FormCollectionModel;

            if (baseFormCollection != null)
            {
                if (baseFormCollection.Forms != null)
                {
                    if (Request.HttpMethod == "POST")
                    {
                        bool status = true;

                        // MapRequestFormData validates model as well
                        MapRequestFormData(baseFormCollection.Forms);

                        if (string.Compare(baseFormCollection.EnableRecaptcha, "Enable") == 0)
                        {

                            var response = Request["g-recaptcha-response"];
                            string secretKey = WebRequestContext.Localization.GetConfigValue("core.recaptchaSecretKey");

                            var client = new WebClient();
                            var result = client.DownloadString(string.Format(WebRequestContext.Localization.GetConfigValue("core.recaptchaValidateUrl"), secretKey, response));
                            var obj = JObject.Parse(result);
                            status = (bool)obj.SelectToken("success");
                            ViewBag.Message = status ? "" : WebRequestContext.Localization.GetConfigValue("core.recaptchaErrorMessage");
                        }
                        if (ModelState.IsValid && status)
                        {

                            try
                            {
                                // Loading the OptionCategory list from TemData to avoid broker calls
                                foreach (var form in baseFormCollection.Forms)
                                {
                                    if (form.FormFields != null)
                                    {


                                        foreach (var field in form.FormFields.Where(f => !string.IsNullOrEmpty(f.OptionsCategory)))
                                        {
                                            if (field.FieldType == FieldType.DropDown || field.FieldType == FieldType.CheckBox || field.FieldType == FieldType.RadioButton)
                                            {

                                                field.OptionsCategoryList = TempData[field.Name] as List<OptionModel>;


                                            }
                                        }


                                    }
                                }
                                //Add logic to save form data here
                                foreach (var postAction in baseFormCollection.FormPostActions)
                                {
                                    string formHandlerName = string.Empty;

                                    try
                                    {
                                        formHandlerName = postAction.FormHandler;

                                        var formHandler = FormHandlerRegistry.Get(formHandlerName);
                                        if (formHandler != null)
                                        {
                                            formHandler.ProcessForm(Request.Form, baseFormCollection.Forms.Where(form => form.FormFields != null).SelectMany(d => d.FormFields).ToList(), postAction);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!string.IsNullOrEmpty(baseFormCollection.ErrorRedirect.Url))
                                        {
                                            Log.Error(ex, "Error occured while processing form data with form handler: {0}. Error message: {1}", formHandlerName, ex.Message);
                                            return new RedirectModel(baseFormCollection.ErrorRedirect.Url);
                                        }
                                        else
                                        {
                                            throw ex;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(baseFormCollection.SuccessRedirect.Url))
                                    return new RedirectModel(baseFormCollection.SuccessRedirect.Url);
                                else
                                    return new RedirectModel(WebRequestContext.Localization.Path);
                            }
                            catch (Exception ex)
                            {
                                if (!string.IsNullOrEmpty(baseFormCollection.ErrorRedirect.Url))
                                {
                                    Log.Error(ex, "Error occured while saving form data.");
                                    return new RedirectModel(baseFormCollection.ErrorRedirect.Url);
                                }
                                else
                                    throw ex;
                            }


                        }
                    }
                    else
                    {
                        // Load options from category
                        foreach (var form in baseFormCollection.Forms)
                        {
                            if (form.FormFields != null)
                            {


                                foreach (var field in form.FormFields.Where(f => !string.IsNullOrEmpty(f.OptionsCategory)))
                                {
                                    if (field.FieldType == FieldType.DropDown || field.FieldType == FieldType.CheckBox || field.FieldType == FieldType.RadioButton)
                                    {

                                        List<OptionModel> options = TaxonomyProvider.LoadOptionsFromCategory(field.OptionsCategory, WebRequestContext.Localization);
                                        field.OptionsCategoryList = options.OrderBy(x => x.Key).ThenBy(x => x.DisplayText).ToList(); 

                                    }
                                }


                            }
                        }
                    }

                }

            }

            return baseFormCollection;

        }


        protected bool MapRequestFormData(List<FormModel> model)
        {
            if (Request.HttpMethod != "POST")
            {
                return false;
            }
            // CSRF protection: If the anti CSRF cookie is present, a matching token must be in the form data too.
            const string antiCsrfToken = "__RequestVerificationToken";
            if (Request.Cookies[antiCsrfToken] != null)
            {
                AntiForgery.Validate();
            }


            foreach (var form in model)
            {
                if (form.FormFields != null)
                {

                    foreach (string formField in Request.Form)
                    {
                        if (formField == antiCsrfToken)
                        {
                            // This is not a form field, but the anti CSRF token (already validated above).
                            continue;
                        }



                        FormFieldModel fieldModel = form.FormFields.FirstOrDefault(f => f.Name == formField);
                        if (fieldModel == null)
                        {
                            Log.Debug("Form [{0}] has no defined field for form field '{1}'", form.Id, formField);
                            continue;
                        }


                        List<string> formFieldValues = Request.Form.GetValues(formField).Where(f => f != "false").ToList();
                        try
                        {
                            fieldModel.Values = formFieldValues;
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("Failed to set Model [{0}] property '{1}' to value obtained from form data: '{2}'. {3}", form.Id, fieldModel.Name, formFieldValues, ex.Message);
                            ModelState.AddModelError(fieldModel.Name, ex.Message);
                        }

                        FormFieldValidator validator = new FormFieldValidator(fieldModel);
                        string validationMessage = "Field Validation Failed";
                        if (!validator.Validate(formFieldValues, ref validationMessage))
                        {
                            if (validationMessage != null)
                            {
                                Log.Debug("Validation of property '{0}' failed: {1}", fieldModel.Name, validationMessage);
                                ModelState.AddModelError(fieldModel.Name, validationMessage);
                                continue;
                            }
                        }


                    }
                }
            }

            return true;
        }


    }

    public class FormFieldValidator
    {
        FormFieldModel field;

        public FormFieldValidator(FormFieldModel fieldDefinition)
        {
            field = fieldDefinition;
        }

        public bool Validate(List<string> values, ref string validationMessage)
        {

            string required = field.Required;

            string regexPattern = field.ValidationRegex;



            if ((string.Compare(required, "Yes") == 0))
            {
                bool fieldValueProvided = false;
                if (values.Count > 0 && !string.IsNullOrEmpty(values.First()))
                    fieldValueProvided = true;

                if (!fieldValueProvided)
                {
                    if (!string.IsNullOrEmpty(field.RequiredError))
                    {
                        validationMessage = field.RequiredError;
                    }
                    else
                    {
                        validationMessage = "Field is required";
                    }
                    return false;
                }
            }

            if (regexPattern != null)
            {

                try
                {
                    Regex rgx = new Regex(regexPattern);
                    if (!rgx.IsMatch(values.First()))
                    {
                        if (!string.IsNullOrEmpty(field.ValidationError))
                        {
                            validationMessage = field.ValidationError;
                        }
                        else
                        {
                            validationMessage = "Field validation failed";
                        }
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //TODO: add log message
                    return false;
                }
            }


            return true;
        }

    }


}