using coreApp.DAL;
using Module.DB;

namespace coreApp.Interfaces
{
    public interface IMyController
    {
        tblEmployee employee { get; set; }
    }

    public interface IEmployeeController
    {
        tblEmployee employee { get; set; }
    }

    public interface IOfficeController
    {
       tblOffice office { get; set; }
    }
     public interface ICampusController
    {
        tblCampus campus { get; set; }
    }

    public interface IAllowanceController
    {
       TblPayrollAllowance allowance { get; set; }
    }
    public interface IDeductionController
    {
       TblPayrollDeduction deduction { get; set; }
    }
    public interface IGroupController
    {
        tblGroup group { get; set; }
    }

   
    public interface IPositionController
    {
        tblPosition position { get; set; }
    }

    public interface IEffectivityController
    {
        string effectivity { get; set; }
    }

    public interface IEffectivity_SupplementalController
    {
        int officeId { get; set; }
    }
}