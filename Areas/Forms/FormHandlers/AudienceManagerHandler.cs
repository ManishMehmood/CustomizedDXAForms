using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sdl.Web.Common.Logging;
using Navient.Modules.Forms.Areas.Forms.Models;

namespace Navient.Modules.Forms.Areas.Forms.FormHandlers
{
    public class AudienceManagerHandler : IFormHandler
    {
        public string Name
        {
            get
            {
                return "AudienceManagerHandler";
            }
        }

        public void ProcessForm(NameValueCollection formData, List<FormFieldModel> fields, BaseFormPostActionModel model)
        {
            AudienceManagerPostActionModel amPostActionModel = model as AudienceManagerPostActionModel;

            try
            {
                if (formData[amPostActionModel.IdentificationKeyConfig] == null)
                {
                    throw new Exception("Audience Manager Identification Key Config is invalid! Cannot find such field.");
                }

                string identificationKey = formData[amPostActionModel.IdentificationKeyConfig];

                Repositories.AudienceManagerRepository rep = new Repositories.AudienceManagerRepository(identificationKey, amPostActionModel.IdentificationSource, amPostActionModel.EmailConfirmationIdentifier, amPostActionModel.AddressBookId);
                rep.CreateContact(fields);
            }
            catch (Exception ex)
            {
                Log.Debug(string.Format("Error while saving data to Audience Manager: {0}", ex.Message));
                throw ex;
            }
        }
    }
}