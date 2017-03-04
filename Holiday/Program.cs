using System;
using System.IO;

namespace Holiday
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			//50年の日付と休日かどうかの情報を出力します
			var start = new DateTime(1984, 11, 15);
			var end = start.AddYears(50);
			var span = end - start;
			using (StreamWriter sw = new StreamWriter("holiday.csv"))
			{
				sw.WriteLine("year,month,day,holiday,title");
				for (var i = 0; i <= span.TotalDays; i++)
				{
					var day = start.AddDays(i);
					var holiday = HolidayUtils.GetHoliday(day);
					sw.WriteLine($"{day.Year},{day.Month},{day.Day},{(!string.IsNullOrEmpty(holiday)).ToString()},{holiday}");
				}
			}
			using (StreamWriter sw = new StreamWriter("holiday-with-dayofweek.csv"))
			{
				sw.WriteLine("year,month,day,dayofweek,week,holiday,title");
				for (var i = 0; i <= span.TotalDays; i++)
				{
					var day = start.AddDays(i);
					var holiday = HolidayUtils.GetHoliday(day);
					sw.WriteLine($"{day.Year},{day.Month},{day.Day},{day.DayOfWeek.ToString()},{day.Day / 7 + 1},{(!string.IsNullOrEmpty(holiday)).ToString()},{holiday}");
				}
			}
		}
	}
}
