using coreApp.Areas.Procurement.DAL;
using coreApp.DAL;
using Module.DB;

namespace coreApp.Areas.Procurement.Interfaces
{
    public interface IYearController
    {
        int Year { get; set; }
    }

    public interface IOfficeController
    {
        tblOffice Office { get; set; }
    }

    public interface IDepartmentController
    {
        tblDepartment  Department { get; set; }
    }

    public interface IPPMPController
    {
        tblPPMP PPMP { get; set; }
    }

    public interface IAPPController
    {
        tblAPP APP { get; set; }
    }

    public interface ICPRController
    {
        tblConsolidatedPR CPR { get; set; }
    }

    public interface IRFQController
    {
        tblRFQ RFQ { get; set; }
    }

    public interface IAPRController
    {
        tblAPR APR { get; set; }
    }

    public interface ICompPRController
    {
        tblCompanyPR PR { get; set; }
    }

    public interface IPRController
    {
        tblPR PR { get; set; }
    }

    public interface IAOBController
    {
        tblAOB AOB { get; set; }
    }

    public interface IPOController
    {
        tblPO PO { get; set; }
    }
    
    public interface INOAController
    {
        tblNOA NOA { get; set; }
    }
}