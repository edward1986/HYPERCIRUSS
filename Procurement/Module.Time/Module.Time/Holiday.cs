using Module.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Time
{
    public class Holidays
    {
        DateTime startDate;
        DateTime endDate;
        List<tblHoliday> data;

        public List<tblHoliday> tblHolidays = new List<tblHoliday>();

        public Holidays(DateTime startDate, DateTime endDate, List<tblHoliday> data)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.data = data;

            generate();
        }

        public void generate()
        {
            int year = startDate.Year;
            while (year <= endDate.Year)
            {
                tblHolidays.AddRange(
                    data.Where(x => x.Year == -1).Select(x => new tblHoliday
                    {
                        Holiday = x.Holiday,
                        Type = x.Type,
                        Day = x.Day,
                        Month = x.Month,
                        Year = year,
                        Remarks = x.Remarks
                    })
                );

                tblHolidays.AddRange(
                    data.Where(x => x.Year == year)
                );

                year++;
            }
        }
    }
}