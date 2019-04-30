using DXA.Modules.Forms.Areas.Forms.Models;
using Sdl.Web.Common.Configuration;
using Sdl.Web.Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tridion.ContentDelivery.Taxonomies;
using Tridion.ContentDelivery.Web.Utilities;

namespace DXA.Modules.Forms.Areas.Forms.Providers
{
    public class TaxonomyProvider
    {

        //WebRequestContext.Localization


        //public static List<string> LoadOptionsFromCategory(string categoryName, Localization localization)
        //{
        //    var list = new List<string>();
        //    TaxonomyFactory taxonomyFactory = new TaxonomyFactory();

        //    string[] taxonomyIds = taxonomyFactory.GetTaxonomies(GetPublicationTcmUri(localization));

        //    foreach (string taxonomyId in taxonomyIds)
        //    {
        //        Keyword taxonomyRoot = taxonomyFactory.GetTaxonomyKeywords(taxonomyId);
        //        if (taxonomyRoot.KeywordName.Equals(categoryName))
        //        {
        //            list.AddRange(taxonomyRoot.KeywordChildren.Cast<Keyword>().Select(k => k.KeywordName));
        //            return list;
        //        }
        //    }
        //    return list;

        //}

        public static List<OptionModel> LoadOptionsFromCategory(string categoryName, Localization localization)
        {
            var list = new List<OptionModel>();
            TaxonomyFactory taxonomyFactory = new TaxonomyFactory();

            string[] taxonomyIds = taxonomyFactory.GetTaxonomies(GetPublicationTcmUri(localization));

            foreach (string taxonomyId in taxonomyIds)
            {
                Keyword taxonomyRoot = taxonomyFactory.GetTaxonomyKeywords(taxonomyId);
                if (taxonomyRoot.KeywordName.Equals(categoryName))
                {
                    // list.AddRange(taxonomyRoot.KeywordChildren.Cast<Keyword>().Select(k => new OptionModel { Value = new TcmUri(k.KeywordUri).ItemId.ToString(), DisplayText = k.KeywordName }));
                    list.AddRange(taxonomyRoot.KeywordChildren.Cast<Keyword>().Select(k => new OptionModel { Key = k.KeywordKey,Value = k.KeywordName, DisplayText = k.KeywordDescription,EmailIds=k.KeywordMeta.NameValues}));
                    return list;
                }
            }
            
            return list;

        }

        private static string GetPublicationTcmUri(Localization localization)
        {
            return string.Format("tcm:0-{0}-1", localization.LocalizationId);
        }

        //private static string ResolveNavigationTaxonomyUri(Localization localization)
        //{
        //    using (new Tracer(localization))
        //    {
        //        TaxonomyFactory taxonomyFactory = new TaxonomyFactory();
        //        string[] taxonomyIds = taxonomyFactory.GetTaxonomies(GetPublicationTcmUri(localization));

        //        Keyword navTaxonomyRoot = taxonomyIds.Select(id => taxonomyFactory.GetTaxonomyKeyword(id)).FirstOrDefault(tax => tax.KeywordName.Contains(TaxonomyNavigationMarker));
        //        if (navTaxonomyRoot == null)
        //        {
        //            Log.Warn("No Navigation Taxonomy Found in Localization [{0}]. Ensure a Taxonomy with '{1}' in its title is published.", localization, TaxonomyNavigationMarker);
        //            return string.Empty;
        //        }

        //        Log.Debug("Resolved Navigation Taxonomy: {0} ('{1}')", navTaxonomyRoot.TaxonomyUri, navTaxonomyRoot.KeywordName);

        //        return navTaxonomyRoot.TaxonomyUri;
        //    }
        //}



    }
}