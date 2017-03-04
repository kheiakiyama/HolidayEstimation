using System;
using System.Collections.Generic;

namespace Holiday
{
	public static class HolidayUtils
	{
		public static string GetHoliday(DateTime date)
		{
			var res = GetPatternHoliday(date);
			if (!string.IsNullOrEmpty(res))
				return res;
			else
				return GetSpecialHoliday(date);
		}

		private static string GetPatternHoliday(DateTime date)
		{
			foreach (var item in HolidayDefinitions)
				if (item.IsHoliday(date))
					return item.Title;
			return "";
		}

		private static string GetSpecialHoliday(DateTime date)
		{
			//春分の日
			if (!Shunbuns.ContainsKey(date.Year))
				Shunbuns.Add(date.Year, GetShunbun(date.Year));
			if (Shunbuns[date.Year].Month == date.Month &&
				Shunbuns[date.Year].Day == date.Day)
				return "春分の日";
			//秋分の日
			if (!Shubuns.ContainsKey(date.Year))
				Shubuns.Add(date.Year, GetShubun(date.Year));
			if (Shubuns[date.Year].Month == date.Month &&
				Shubuns[date.Year].Day == date.Day)
				return "秋分の日";
			return "";
		}

		//http://www.atmarkit.co.jp/ait/articles/1507/22/news024.html
		private static DateTime GetShunbun(int year)
		{
			// 1. 2000年の太陽の春分点通過日
  			double 基準日 = 20.69115;

			// 2. 春分点通過日の移動量＝（西暦年－2000年）×0.242194
			double 移動量 = (year - 2000) * 0.242194;

			// 3. 閏年によるリセット量＝INT｛（西暦年－2000年）/ 4｝
			int 閏年補正 = (int)((year - 2000) / 4.0);

			// 求める年の春分日＝INT｛（1）＋（2）－（3）｝
			int 春分日 = (int)(基準日 + 移動量 - 閏年補正);

			return new DateTime(year, 3, 春分日);
		}

		private static DateTime GetShubun(int year)
		{ 
			// 1. 2000年の太陽の秋分点通過日
			double 基準日 = 23.09; // 秋分点の揺らぎ補正済みの値

			// 2. 秋分点通過日の移動量＝（西暦年－2000年）×0.242194
			double 移動量 = (year - 2000) * 0.242194;

			// 3. 閏年によるリセット量＝INT｛（西暦年－2000年）/ 4｝
			int 閏年補正 = (int)((year - 2000) / 4.0);

			// 求める年の秋分日＝INT｛（1）＋（2）－（3）｝
			int 秋分日 = (int)(基準日 + 移動量 - 閏年補正);

			return new DateTime(year, 9, 秋分日);
		}

		private static Dictionary<int, DateTime> Shunbuns = new Dictionary<int, DateTime>();
		private static Dictionary<int, DateTime> Shubuns = new Dictionary<int, DateTime>();

		static HolidayUtils()
		{
			HolidayDefinitions = new HolidayDefinition[] {
				new HolidayDefinition() { Title = "元日", Month = 1, Day = 1 },
				new HolidayDefinition() { Title = "成人の日", Month = 1, Day = 15, EndYear = 1999 },
				new HolidayDefinition() { Title = "成人の日", Month = 1, HappyMondayWeek = 2, StartYear = 2000 },
				new HolidayDefinition() { Title = "建国記念の日", Month = 2, Day = 11 , StartYear = 1967 },
				new HolidayDefinition() { Title = "天皇誕生日", Month = 4, Day = 29 , EndYear = 1988 },
				new HolidayDefinition() { Title = "みどりの日", Month = 4, Day = 29 , StartYear = 1989, EndYear = 2006 },
				new HolidayDefinition() { Title = "昭和の日", Month = 4, Day = 29 , StartYear = 2007 },
				new HolidayDefinition() { Title = "憲法記念日", Month = 5, Day = 3 },
				new HolidayDefinition() { Title = "みどりの日", Month = 5, Day = 4, StartYear = 2007 },
				new HolidayDefinition() { Title = "こどもの日", Month = 5, Day = 5 },
				new HolidayDefinition() { Title = "海の日", Month = 7, Day = 20, StartYear = 1996, EndYear = 2002 },
				new HolidayDefinition() { Title = "海の日", Month = 7, HappyMondayWeek = 3, StartYear = 2003 },
				new HolidayDefinition() { Title = "山の日", Month = 8, Day = 11, StartYear = 2016 },
				new HolidayDefinition() { Title = "敬老の日", Month = 9, Day = 15, StartYear = 1966, EndYear = 2002 },
				new HolidayDefinition() { Title = "敬老の日", Month = 9, HappyMondayWeek = 3, StartYear = 2003 },
				new HolidayDefinition() { Title = "体育の日", Month = 10, Day = 10, EndYear = 1999 },
				new HolidayDefinition() { Title = "体育の日", Month = 10, HappyMondayWeek = 2, StartYear = 2000 },
				new HolidayDefinition() { Title = "文化の日", Month = 11, Day = 3 },
				new HolidayDefinition() { Title = "勤労感謝の日", Month = 11, Day = 23 },
				new HolidayDefinition() { Title = "天皇誕生日", Month = 12, Day = 23, StartYear = 1989 },
			};
		}

		private static HolidayDefinition[] HolidayDefinitions;

		private class HolidayDefinition
		{
			internal string Title { get; set; }
			internal int Month { get; set; }
			internal int Day { get; set; }
			internal int? StartYear { get; set; }
			internal int? EndYear { get; set; }
			internal int? HappyMondayWeek { get; set; }
			private Dictionary<int, DateTime> TargetDays; 

			internal HolidayDefinition()
			{
				TargetDays = new Dictionary<int, DateTime>();
			}

			private DateTime GetHappyDay(int year)
			{
				DateTime? target = null;
				var happyCount = 0;
				do
				{
					target = target.HasValue ? target.Value.AddDays(1) : new DateTime(year, Month, 1);
					if (target.Value.DayOfWeek == DayOfWeek.Monday)
						happyCount++;
				} while (happyCount < HappyMondayWeek.Value);
				return target.Value;
			}

			internal bool IsHoliday(DateTime date)
			{
				if (StartYear.HasValue &&
					StartYear.Value > date.Year)
					return false;
				if (EndYear.HasValue &&
					EndYear.Value < date.Year)
					return false;
				if (HappyMondayWeek.HasValue)
				{
					if (!TargetDays.ContainsKey(date.Year))
						TargetDays.Add(date.Year, GetHappyDay(date.Year));

					var day = TargetDays[date.Year];
					return day.Month == date.Month &&
							  day.Day == date.Day;
				}
				else if (Month == date.Month &&
					Day == date.Day)
				{
					return true;
				}
				return false;
			}
		}
	}
}
