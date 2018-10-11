using LibraryData.Models;
using System;
using System.Collections.Generic;

namespace LibraryServices
{
    public class DataHelpers
    {
        public static List<string> BusinessHoursToDateString(IEnumerable<BranchHours> branchHours)
        {
            var hours = new List<string>();

            foreach (var time in branchHours)
            {
                var day = DayToString(time.DayOfWeek);
                var openTime = TimeToString(time.OpenTime);
                var closeTime = TimeToString(time.CloseTime);

                var period = $"{day}: {openTime} to {closeTime}";
                hours.Add(period);
            }

            return hours;
        }

        public static string TimeToString(int time)
        {
            /* My solution.
            string[] hours = new string[] {
                "00", "01", "02", "03",
                "04", "05", "06", "07",
                "08", "09", "10", "11",
                "12", "13", "14", "15",
                "16", "17", "18", "19",
                "20", "21", "22", "23",
            };

            return hours[time];
            */
            // Yet simplier one. MM - Month, mm - minutes.
            return TimeSpan.FromHours(time).ToString(@"hh\:mm");
        }

        public static string DayToString(int dayOfWeek)
        {
            return Enum.GetName(typeof(DayOfWeek), dayOfWeek);
        }
    }
}
