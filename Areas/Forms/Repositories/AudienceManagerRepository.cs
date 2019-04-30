using Navient.Modules.Forms.Areas.Forms.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Tridion.OutboundEmail.ContentDelivery;
using Tridion.OutboundEmail.ContentDelivery.Profile;
using Tridion.OutboundEmail.ContentDelivery.Utilities;

namespace Navient.Modules.Forms.Areas.Forms.Repositories
{
    public class AudienceManagerRepository
    {
        public AudienceManagerRepository(string identificationKey, string identificationSource, string emailConfirmationIdentifier, int addressBookId)
        {
            IdentificationKey = identificationKey;
            IdentificationSource = identificationSource;
            EmailConfirmationIdentifier = emailConfirmationIdentifier;
            AddressBookId = addressBookId;
            
        }



        public string IdentificationKey { get; set; }
        public string IdentificationSource { get; set; }
        public string EmailConfirmationIdentifier { get; set; }
        public int AddressBookId { get; set; }

        public string CreateContact(List<FormFieldModel> fields)
        {
            Contact contact = new Contact();

            // Define default subscription status
            contact.SubscriptionStatus = SubscriptionStatus.Subscribed;

            contact.ExtendedDetails["IDENTIFICATION_KEY"].Value = IdentificationKey;
            contact.ExtendedDetails["IDENTIFICATION_SOURCE"].Value = IdentificationSource;

            if (AddressBookId > 0)
                contact.AddressBookId = AddressBookId;


            foreach (var field in fields)
            {
                if (field.Purpose != null)
                {
                    switch (field.PurposeType)
                    {
                        case FieldPurposeType.Salutation:
                            contact.ExtendedDetails["SALUTATION"].Value = field.Value;
                            break;
                        case FieldPurposeType.Prefix:
                            contact.ExtendedDetails["PREFIX"].Value = field.Value;
                            break;
                        case FieldPurposeType.FirstName:
                            contact.ExtendedDetails["NAME"].Value = field.Value;
                            break;
                        case FieldPurposeType.LastName:
                            contact.ExtendedDetails["SURNAME"].Value = field.Value;
                            break;
                        case FieldPurposeType.Email:
                            contact.EmailAddress = field.Value;
                            break;
                        case FieldPurposeType.Password:
                            contact.ExtendedDetails["PASSWORD"].Value = Digests.DigestPassword(field.Value);
                            break;
                        case FieldPurposeType.Company:
                            contact.ExtendedDetails["COMPANY"].Value = field.Value;
                            break;
                        case FieldPurposeType.PhoneNumber:
                            contact.ExtendedDetails["TELEPHONE"].Value = field.Value;
                            break;
                        case FieldPurposeType.City:
                            contact.ExtendedDetails["CITY"].Value = field.Value;
                            break;
                        case FieldPurposeType.State:
                            contact.ExtendedDetails["STATE"].Value = field.Value;
                            break;
                        case FieldPurposeType.BirthDate:
                            contact.ExtendedDetails["BIRTH_DATE"].Value = field.Value;
                            break;
                        case FieldPurposeType.Age:
                            contact.ExtendedDetails["AGE"].Value = field.Value;
                            break;
                        case FieldPurposeType.WorkingYears:
                            contact.ExtendedDetails["WORKING_YEARS"].Value = field.Value;
                            break;


                        default:
                            break;
                    }
                }
            }
            
            //// Newsletters (keywords)
            ////if (ReceiveNewsletters.Checked && !string.IsNullOrEmpty(KeywordNewsletter))
            ////{
            ////    contact.Keywords.Add(KeywordNewsletter);
            ////}

            ////if (ReceiveOffers.Checked && !string.IsNullOrEmpty(KeywordOffers))
            ////{
            ////    contact.Keywords.Add(KeywordOffers);
            ////}

            try
            {
                // Pass in an identifier used to look up the confirmation e-mail to send, or empty string if no confirmation should be sent.
                contact.Save(EmailConfirmationIdentifier ?? String.Empty);
                
                // Verify that the Contact has been saved
                Contact.GetFromContactIdentificatonKeys(contact.EmailAddress, IdentificationSource);

            }
            catch (ContactAlreadyExistsException ex)
            {
                throw ex;
                //ShowErrorMessage("The e-mail address you entered is already subscribed.");
            }
            catch (Exception ex)
            {
                throw ex;
                //ShowErrorMessage("There was an error while saving your data.");
            }


            return "Subscribed";
        }

    }
}