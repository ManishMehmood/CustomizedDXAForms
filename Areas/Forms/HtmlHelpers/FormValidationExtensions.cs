using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using System.Web.Routing;

namespace DXA.Modules.Forms.Areas.Forms.HtmlHelpers
{
    public static class FormValidationExtensions
    {

        public static MvcHtmlString ValidationMessageCustom(this HtmlHelper htmlHelper, string modelName)
        {
            //<span class="text-danger field-validation-error" data-valmsg-for="@field.Name" data-valmsg-replace="true">
            //@if(!ViewData.ModelState.IsValid && ViewData.ModelState[field.Name] != null && ViewData.ModelState[field.Name].Errors.Count > 0)
            //{
            //    <span for= "@field.Name" class="">@ViewData.ModelState[field.Name].Errors.First().ErrorMessage</span>
            //}
            //</span>

            TagBuilder builder = new TagBuilder("span");
            builder.AddCssClass("text-danger");
            builder.AddCssClass("field-validation-error");

            builder.Attributes.Add("data-valmsg-for", modelName);
            builder.Attributes.Add("data-valmsg-replace", "true");

            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
            ModelError modelError = (((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors.FirstOrDefault(m => !String.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrors[0]);

            if (htmlHelper.ViewData.ModelState.IsValid && modelError != null)
            {
                TagBuilder errorMessageSpan = new TagBuilder("span");
                errorMessageSpan.Attributes.Add("for", modelName);
                errorMessageSpan.SetInnerText(modelError.ErrorMessage);
                builder.InnerHtml = errorMessageSpan.ToString();
            }
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));

        }
    }
}