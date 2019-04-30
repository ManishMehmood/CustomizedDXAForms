using Sdl.Web.Common.Logging;
using Sdl.Web.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Tridion.ContentDelivery.Meta;

namespace DXA.Modules.Forms.Areas.Forms.Models
{
    public class FormFieldModel : EntityModel
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public Tag Type { get; set; }
        
        public FieldType FieldType {
            get {
                try
                {
                    FieldType testEnumValue = (FieldType)Enum.Parse(typeof(FieldType), Type.Key, true);
                    return testEnumValue;
                }
                catch (Exception ex)
                {
                    Log.Debug("Failed to cast '{0}' to {1}. Error message: {2}", Type.Key, nameof(FieldType), ex.Message);
                    return FieldType.Text;
                }
                
            }
        }

        public Tag Purpose { get; set; }

        public FieldPurposeType PurposeType
        {
            get
            {
                try
                {
                    FieldPurposeType testEnumValue = (FieldPurposeType)Enum.Parse(typeof(FieldPurposeType), Purpose.Key, true);
                    return testEnumValue;
                }
                catch (Exception ex)
                {
                    Log.Debug("Failed to cast '{0}' to {1}. Error message: {2}", Type.Key, nameof(FieldPurposeType), ex.Message);
                    return FieldPurposeType.Company;
                }

            }
        }

        public string Required { get; set; }
        public string RequiredError { get; set; }
        public string ValidationRegex { get; set; }
        public string ValidationError { get; set; }
        [SemanticProperty("options")]
        public List<string> Options { get; set; }

        public string OptionsCategory { get; set; }

        [SemanticProperty(IgnoreMapping = true)]
        public List<OptionModel> OptionsCategoryList { get; set; } = new List<OptionModel>();
        public string Width { get; set; }

        [SemanticProperty(IgnoreMapping = true)]
        

        [SemanticProperty(IgnoreMapping = true)]
        public List<string> Values { get; set; } = new List<string>();

        public string PrintValues()
        {
            List<string> displayTextValues = new List<string>();

            if (!string.IsNullOrEmpty(this.OptionsCategory) && this.OptionsCategoryList != null)
            {
                foreach (string value in Values)
                { 

                    OptionModel option = OptionsCategoryList.SingleOrDefault(o => o.Value == value);
                    if (option != null)
                    {
                        displayTextValues.Add(option.DisplayText);
                    }
                }

                return string.Join(", ", displayTextValues.ToArray());
            }
            else
            {
                return string.Join(", ", this.Values.ToArray());
            }
        }
        public string GetEmailIds()
        {


            if (!string.IsNullOrEmpty(this.OptionsCategory) && this.OptionsCategoryList != null)
            {
                foreach (string value in Values)
                {

                    OptionModel option = OptionsCategoryList.SingleOrDefault(o => o.Value == value);
                    if (option != null && option.EmailIds!=null)
                    {
                        foreach (DictionaryEntry email in option.EmailIds)
                        {
                            return ((NameValuePair)email.Value).Value.ToString();
                        }
                    }
                }


            }
            return string.Empty;
        }

        public string Value
        {
            get
            {
                if (Values.Count > 0)
                    return Values.First();
                else
                    return string.Empty;
            }
        }
    }

    public enum FieldType
    {
        Text,           
        TextArea,
        Date, 
        DropDown,       
        CheckBox,     
        RadioButton,   
        Hidden           
    }

    public enum FieldPurposeType
    {
        Salutation,
        Prefix,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        Address,
        City,
        State,
        Zip,
        Password,
        Company,
        Age,
        BirthDate,
        WorkingYears
    }

    public class OptionModel
    {
        public  string Value { get; set; }
        public  string DisplayText { get; set; }
        public string Key { get; set; }
        public IDictionary EmailIds { get; set; }
     
    }
}