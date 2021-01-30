using System.Web.Mvc;

namespace coreApp.Areas.SAM
{
    public class SAMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SAM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Issuances",
                "SAM/Issuances/{action}/{id}",
                new { controller = "Issuances", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Issuances_Cont",
                "SAM/Issuances_Cont/{action}/{id}",
                new { controller = "Issuances_Cont", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "StockItems",
                "SAM/Stock/Items/{action}/{itemId}/{startDate}/{endDate}",
                new { controller = "StockItems", action = "Index", itemId = UrlParameter.Optional, startDate = UrlParameter.Optional, endDate = UrlParameter.Optional }
            );

            context.MapRoute(
                 "Stocks",
                 "SAM/Stocks/{action}/{id}",
                 new { controller = "Stocks", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
               "RISItems",
               "SAM/RIS/Items/{action}/{risId}/{id}",
               new { controller = "RISItems", action = "Index", risId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                 "RIS",
                 "SAM/RIS/{action}/{year}/{id}",
                 new { controller = "RIS", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
               "AREItems",
               "SAM/ARE/Items/{action}/{areId}/{id}",
               new { controller = "AREItems", action = "Index", areId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                 "ARE",
                 "SAM/ARE/{action}/{year}/{id}",
                 new { controller = "ARE", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
               "PTRItems",
               "SAM/PTR/Items/{action}/{ptrId}/{id}",
               new { controller = "PTRItems", action = "Index", ptrId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                 "PTR",
                 "SAM/PTR/{action}/{year}/{id}",
                 new { controller = "PTR", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
               "ReturnItems",
               "SAM/Return/Items/{action}/{returnId}/{id}",
               new { controller = "ReturnItems", action = "Index", returnId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                 "Return",
                 "SAM/Return/{action}/{year}/{id}",
                 new { controller = "Return", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
               "InventoryInspectionItems",
               "SAM/Inventory/Inspection/Items/{action}/{inspectionId}/{id}",
               new { controller = "InventoryInspectionItems", action = "Index", inspectionId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
              "InventoryInspection",
              "SAM/Inventory/Inspection/{action}/{year}/{id}",
              new { controller = "InventoryInspection", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
          );


            context.MapRoute(
               "InventoryIssuance",
               "SAM/Inventory/Issuance/{action}/{inventoryId}/{id}",
               new { controller = "InventoryIssuance", action = "Index", inventoryId = UrlParameter.Optional, id = UrlParameter.Optional }
           );

            context.MapRoute(
                 "TaggingEntry",
                 "SAM/Tagging/Entry/{action}/{year}/{receiptItemId}/{id}",
                 new { controller = "TaggingEntry", action = "Index", year = UrlParameter.Optional, receiptItemId = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Tagging",
                 "SAM/Tagging/{action}/{year}/{id}",
                 new { controller = "Tagging", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                "SAMPOItems",
                "SAM/PO/Items/{action}/{poId}/{id}",
                new { controller = "POItems", action = "Index", poId = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                 "SAMPO",
                 "SAM/PO/{action}/{year}/{id}",
                 new { controller = "PO", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );



            context.MapRoute(
               "ReceivingItems",
               "SAM/Receiving/Items/{action}/{receiptId}",
               new { controller = "ReceivingItems", action = "Index", receiptId = UrlParameter.Optional }
           );

            context.MapRoute(
                 "Receiving",
                 "SAM/Receiving/{action}/{year}/{id}",
                 new { controller = "Receiving", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );
            
            context.MapRoute(
               "InspectionItems",
               "SAM/Inspection/Items/{action}/{receiptId}",
               new { controller = "InspectionItems", action = "Index", receiptId = UrlParameter.Optional }
           );

            context.MapRoute(
                 "Inspection",
                 "SAM/Inspection/{action}/{year}/{id}",
                 new { controller = "Inspection", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "ItemLife",
                 "SAM/ItemLife/{action}/{year}/{id}",
                 new { controller = "ItemLife", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
             );

            context.MapRoute(
                "SAMItems",
                "SAM/Items/{action}/{year}/{id}",
                new { controller = "Items", action = "Index", year = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            context.MapRoute(
                "SAM_default",
                "SAM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}