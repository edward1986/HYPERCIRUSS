

namespace coreApp.Areas.Procurement.Enums
{
    public enum Status
    {
        Draft = 0,
        Returned = 1,
        Submitted = 2,
        ReSubmitted = 3,
        Denied = 4,
        Approved = 5
    }

    public enum ModeOfProcurement
    {
        SVP = 0,
        Shopping = 1,
        PublicBidding = 2,
        CB = 3,
        DC = 4,
        RO = 5,
        TFB = 6,
        EC = 7,
        TOC = 8,
        AOC = 9,
        ATA = 10,
        SSA = 11,
        HTC = 12,
        DCA = 13,
        LRPV = 14,
        NGO = 15,
        CP = 16,
        UN = 17,
        FFP = 18
    }

    public enum MOPType
    {
        OfficePR = 0,
        AgencyPR = 1,
        ConsolidatedPR = 2
    }

    public enum VAT
    {
        VAT = 0,
        NonVAT = 1,
        Exempted = 2
    }
    
    public enum MFO
    {
        MOOE = 0,
        CO = 1
    }
}
