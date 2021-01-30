using System.ComponentModel.DataAnnotations;

namespace coreApp.Models
{

    public class EmployeeAccountModel
    {
        public int EmployeeId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }
    }

    //public class EnableDisableAccountModel
    //{
    //    public int EmployeeId { get; set; }
    //    public string Id { get; set; }
    //}
}