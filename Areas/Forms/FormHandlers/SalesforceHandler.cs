using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Logging;
using DXA.Modules.Forms.Areas.Forms.Models;
using System.Net;
using System.Text;

namespace DXA.Modules.Forms.Areas.Forms.FormHandlers
{
    public class SalesforceHandler : IFormHandler
    {
        public string Name
        {
            get
            {
                return "SalesforceHandler";
            }
        }

        public void ProcessForm(NameValueCollection formData, List<FormFieldModel> fields, BaseFormPostActionModel model)
        {
            SalesforcePostActionModel sfPostActionModel = model as SalesforcePostActionModel;

            try
            {

                string salesforceUrl = "https://www.salesforce.com/servlet/servlet.WebToLead?encoding=UTF-8";

                NameValueCollection webToLeadFormData = new NameValueCollection();
                webToLeadFormData.Add("oid", sfPostActionModel.OrganizationId);
                webToLeadFormData.Add("submit", "Submit");
                //webToLeadFormData.Add("", "");

                foreach (var field in fields)
                {
                    if (field.Purpose != null)
                    {
                        switch (field.PurposeType)
                        {
                            case FieldPurposeType.Salutation:
                                webToLeadFormData.Add("salutation", field.Value);
                                break;
                            case FieldPurposeType.FirstName:
                                webToLeadFormData.Add("first_name", field.Value);
                                break;
                            case FieldPurposeType.LastName:
                                webToLeadFormData.Add("last_name", field.Value);
                                break;
                            case FieldPurposeType.Email:
                                webToLeadFormData.Add("email", field.Value);
                                break;
                            case FieldPurposeType.Company:
                                webToLeadFormData.Add("company", field.Value);
                                break;
                            case FieldPurposeType.PhoneNumber:
                                webToLeadFormData.Add("mobile", field.Value);
                                break;
                            case FieldPurposeType.City:
                                webToLeadFormData.Add("city", field.Value);
                                break;
                            case FieldPurposeType.State:
                                webToLeadFormData.Add("state", field.Value);
                                break;
                            case FieldPurposeType.Address:
                                webToLeadFormData.Add("street", field.Value);
                                break;

                            default:
                                break;
                        }
                    }
                }


                WebClient webclient = new WebClient();
                webclient.Encoding = Encoding.UTF8;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                byte[] responseArray = webclient.UploadValues(salesforceUrl, "POST", webToLeadFormData);
                Log.Debug("Form data was successufully submited to SalesForce. Response: ", Encoding.ASCII.GetString(responseArray));
                    

            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error while posting data to SalesForce: {0}", ex.Message));
                throw ex;
            }
        }


        
    }
}