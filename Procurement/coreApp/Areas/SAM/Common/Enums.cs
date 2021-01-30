namespace coreApp.Areas.SAM.Enums
{
    public enum POStatus
    {
        Undelivered = 0,
        Partial = 1,
        Complete = 2
    }

    public enum InventoryItemStatus
    {
        Usable = 0,
        Unusable = 1,
        Unchecked = 2,
        UnAccounted = 3
    }

    public enum CategoryType
    {
        Supply = 0,
        Equipment = 1
    }

    public enum ReceiptStatus
    {
        Draft = 0,
        Returned = 1,
        Submitted = 2,
        ReSubmitted = 3,
        Inspected = 4
    }

    public enum iType
    {
        Employee = 0,
        Contractor = 1
    }

    public enum AREType
    {
        PAR = 0,
        PTR = 1,
        Return = 2
    }
}
