using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Emit;
using Newtonsoft.Json;

namespace CZE.Web.Util.Helper
{
    public sealed class Alert
    {
        public String Title { get; set; }
        public String Summary { get; set; }
        public String Details { get; set; }
        public string Severity { get; set; }
        public String AppendToId { get; set; }
        public bool Dismissible { get; set; }
        public bool AutoDismiss { get; set; }

        public Alert(Severity severity, string details,
            string sumarry = "", bool? autoDismiss=null , bool? dismissible = null, string title = "",
            string appendToId = "pageMessages")
        {
            this.Severity = severity.ToString();
            this.Details = details;
            this.Summary = sumarry;
            this.AppendToId = appendToId;

            string titleDef = null;
            bool autoDissmissDef=true,dissmissableDef=true;
                switch (severity)
                {
                    case Helper.Severity.success:
                        titleDef = string.IsNullOrEmpty(title) ? "Uspjeh!" : title;
                        autoDissmissDef = true;dissmissableDef = true;
                    break;
                    case Helper.Severity.info: titleDef = string.IsNullOrEmpty(title) ? "Informacija!" : title;
                        autoDissmissDef = true; dissmissableDef = true;
                    break;
                    case Helper.Severity.warning: titleDef = string.IsNullOrEmpty(title) ? "Upozorenje!" : title;
                        autoDissmissDef = true; dissmissableDef = true;
                    break;
                    case Helper.Severity.danger: titleDef = string.IsNullOrEmpty(title) ? "Oprez!" : title;
                        autoDissmissDef = false; dissmissableDef = true;
                    break;
                }

            this.Title = string.IsNullOrEmpty(title) ? titleDef : title;
            this.AutoDismiss =autoDismiss??autoDissmissDef ;
            this.Dismissible =dismissible??dissmissableDef;
            
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    

    public enum Severity { success, info, warning, danger };
}
