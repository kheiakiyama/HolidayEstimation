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
			//var start = new DateTime(2016, 11, 15);
			//var end = start.AddYears(1);
			var span = end - start;
			var filename = "holiday.csv";
			using (StreamWriter sw = new StreamWriter(filename))
			{
				sw.WriteLine("date,holiday,title");
				for (var i = 0; i <= span.TotalDays; i++)
				{
					var day = start.AddDays(i);
					var holiday = HolidayUtils.GetHoliday(day);
					sw.WriteLine($"{day.ToLongDateString()},{(!string.IsNullOrEmpty(holiday)).ToString()},{holiday}");
				}
			}
		}
	}
}
