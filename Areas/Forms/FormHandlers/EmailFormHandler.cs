using DXA.Modules.Forms.Areas.Forms.Models;
using Sdl.Web.Common.Logging;
using Sdl.Web.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace DXA.Modules.Forms.Areas.Forms.FormHandlers
{
    public class EmailFormHandler : IFormHandler
    {
        public string Name
        {
            get
            {
                return "EmailHandler";
            }
        }

        public void ProcessForm(NameValueCollection formData, List<FormFieldModel> fields, BaseFormPostActionModel model)
        {
            try
            {
                EmailPostActionModel emailPostActionModel = model as EmailPostActionModel;
                List<string> emailTo = new List<string>();
                string emailFrom = ConfigurationManager.AppSettings["EmailFrom_Gen_Contact"];
                string emailRecipientSeparator = ConfigurationManager.AppSettings["EmailRecipientsSeparator"] ??";";
                string EmailAppName = ConfigurationManager.AppSettings["EmailManager_AppName"]; 
                List<string> emailCC = new List<string>();
                foreach (var field in fields.Where(i => i.FieldType == FieldType.DropDown))
                {
                    if (!string.IsNullOrEmpty(field.GetEmailIds()))
                    {
                        emailTo.Add(field.GetEmailIds());
                    }
                }
                if (emailPostActionModel.To != null)
                {
                    foreach (var recipient in emailPostActionModel.To)
                    {
                        emailTo.Add(recipient);
                    }
                }
                if (emailPostActionModel.Cc != null)
                {
                    foreach (var recipient in emailPostActionModel.Cc)
                    {
                        emailCC.Add(recipient);
                    }
                }
                string emailBody = GenerateEmailBody(fields, emailPostActionModel.EmailBody);
                Log.Debug("The list of recipients CC {0}", emailBody);
                string toEmail = emailTo.Count>0? string.Join(emailRecipientSeparator, emailTo):string.Empty;
                Log.Debug("The list of recipients To {0}", toEmail);
                string ccEmail = emailCC.Count > 0?string.Join(emailRecipientSeparator, emailCC):string.Empty;
                Log.Debug("The list of recipients CC {0}", ccEmail);
               // Write down the code to send email
               // SendMail.SendEMail(toEmail, emailFrom, string.Empty, ccEmail, string.Empty, emailPostActionModel.Subject, emailBody, EmailAppName, emailRecipientSeparator);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error while sending email: {0}", ex.Message));
              
            }

        }

        public string GenerateEmailBody(List<FormFieldModel> fields, RichText emailBody)
        {

            StringBuilder mailBuilder = new StringBuilder();

            foreach (var field in fields)
            {
                //Replace the email body field name with field values
                if (emailBody != null)
                {
                    emailBody = emailBody.ToString().Replace(string.Format("[{0}]", field.Name), field.PrintValues());
                }
                else
                {
                    mailBuilder.AppendLine(string.Format("{0}: {1}", field.Name, field.PrintValues()));
                }

            }

            return emailBody != null ? emailBody.ToString() : mailBuilder.ToString();
        }
    }
}