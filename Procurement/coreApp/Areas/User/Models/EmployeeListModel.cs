using Module.DB;
using System.Collections.Generic;

namespace coreApp.Models
{

    public class EmployeeListModel
    {
        public List<tblEmployee> List { get; set; }
        public string DataUrl { get; set; }
    }
}