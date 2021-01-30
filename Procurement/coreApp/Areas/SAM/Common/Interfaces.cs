
using System;
using System.ComponentModel.DataAnnotations;

namespace coreApp.Areas.SAM.Interfaces
{
    public interface IPOController
    {
        tblPO PO { get; set; }
    }

    public interface IReceiptController
    {
        tblReceipt Receipt { get; set; }
    }

    public interface IReceiptItemController
    {
        tblReceiptItem ReceiptItem { get; set; }
    }

    public interface IInventoryItemController
    {
        tblInventoryItem InventoryItem { get; set; }
    }

    public interface IInventoryInspectionController
    {
        tblInventoryInspection InventoryInspection { get; set; }
    }

    public interface IAREController
    {
        tblARE ARE { get; set; }
    }

    public interface IRISController
    {
        tblRI RIS { get; set; }
    }

    public interface ISAMController
    {
        tblWarehouse warehouse { get; set; }
    }
}