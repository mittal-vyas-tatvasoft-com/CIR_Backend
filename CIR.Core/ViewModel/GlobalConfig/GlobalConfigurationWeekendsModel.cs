using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.ViewModel.GlobalConfig
{
    public class GlobalConfigurationWeekendsModel
    {
        public List<WeekendModel> WeekendsList { get; set; } = new();

        public int Count { get; set; }

     }
    public class WeekendModel
    {
        public long Id { get; set; }
        public long CountryId { get; set; }

        public long DayOfWeekId
        {
            get; set;
        }
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string DayOfWeek { get; set; }

    }
}
