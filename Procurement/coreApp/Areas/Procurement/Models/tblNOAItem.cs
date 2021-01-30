using coreApp.Areas.Procurement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace coreApp.Areas.Procurement.DAL
{
	public partial class tblNOAItemMetaData
	{


	}
	[MetadataType(typeof(tblNOAItemMetaData))]
	public partial class tblNOAItem
	{

		public tblNOA GetNOA()
		{
			tblNOA ret = null;

			using (procurementDataContext context = new procurementDataContext())
			{
				var tmp = context.tblNOAs.Where(x => x.Id == NOAId).SingleOrDefault();
				if (tmp != null)
				{
					ret = tmp;
				}
			}

			return ret;

		}
		
		public tblConsolidatedPR ConsolidatedPR()
		{
			tblConsolidatedPR ret = null;

			using (procurementDataContext context = new procurementDataContext())
			{
				var tmp = context.tblConsolidatedPRs.Where(x => x.Id == ConsolidatedPRId).SingleOrDefault();
				if (tmp != null)
				{
					ret = tmp;
				}
			}

			return ret;

		}
	}
		
	
}