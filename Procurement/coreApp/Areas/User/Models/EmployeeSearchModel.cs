using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Models
{

    public class EmployeeSearchModel
    {
        public string Title { get; set; } = "Search";
        public string DataUrl { get; set; }
        public bool MinimalView { get; set; }
        public bool MultiSelect { get; set; } = false;
        public bool ExcludeNoCareer { get; set; } = false;
        public bool ExcludeNoOffice { get; set; } = false;
        public string AltSource { get; set; } = null;
        public bool EnableSearch { get; set; } = true;
    }
}