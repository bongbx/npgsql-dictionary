using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace npgsql_dictionary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IEnumerable<PricingAutoSetting> pricingAutoSettings = new List<PricingAutoSetting>
            {
                new PricingAutoSetting
                {
                    SortedOrder = 2,
                    VehicleTypeCode = "100"
                },
                 new PricingAutoSetting
                {
                    SortedOrder = 3,
                    VehicleTypeCode = "100"
                },
                new PricingAutoSetting
                {
                    RouteSettingId = 3,
                    SortedOrder = 2,
                    VehicleTypeCode = "100"
                },
                new PricingAutoSetting
                {
                    RouteSettingId = 3,
                    SortedOrder = 1,
                    VehicleTypeCode = "100"
                },
                new PricingAutoSetting
                {
                    RouteSettingId = 3,
                    SortedOrder = 3,
                    VehicleTypeCode = "100"
                },
                new PricingAutoSetting
                {
                    SortedOrder = 1,
                    VehicleTypeCode = "100"
                },
                new PricingAutoSetting
                {
                    SortedOrder = 3,
                },
                new PricingAutoSetting
                {
                    SortedOrder = 1,
                },
                new PricingAutoSetting
                {
                    SortedOrder = 2,
                },
            };

            pricingAutoSettings = SortedPricingAutoSettings(pricingAutoSettings);


            var orderPricingAutoSettings = Enumerable.Range(1, 10).Select(x => new OrderPricingAutoSetting
            {
                IntervalTime = x * 60,
                Repeat = x,
                Value = x
            }).ToList();

            var startAt = DateTimeOffset.UtcNow;

            var result = orderPricingAutoSettings.Select((x, i) => new
            {
                x.Value,
                x.IntervalTime,
                x.Repeat,
                StartTime = GetStartTime(orderPricingAutoSettings, startAt, i),
                Index = i
            });

            CreateHostBuilder(args).Build().Run();
        }

        private static IEnumerable<PricingAutoSetting> SortedPricingAutoSettings(IEnumerable<PricingAutoSetting> pricingAutoSettings)
        {
            var result = new List<PricingAutoSetting>();

            var firstGroupSetting = pricingAutoSettings
               .Where(x => x.RouteSettingId.HasValue && !string.IsNullOrEmpty(x.VehicleTypeCode))
               .OrderBy(x => x.SortedOrder);

            var secondGroupSetting = pricingAutoSettings
                .Where(x => x.RouteSettingId.HasValue && string.IsNullOrEmpty(x.VehicleTypeCode))
                .OrderBy(x => x.SortedOrder);

            var thirdGroupSetting = pricingAutoSettings
               .Where(x => !x.RouteSettingId.HasValue && !string.IsNullOrEmpty(x.VehicleTypeCode))
               .OrderBy(x => x.SortedOrder);

            var fourthGrouSetting = pricingAutoSettings
               .Where(x => !x.RouteSettingId.HasValue && string.IsNullOrEmpty(x.VehicleTypeCode))
               .OrderBy(x => x.SortedOrder);

            result.AddRange(firstGroupSetting);
            result.AddRange(secondGroupSetting);
            result.AddRange(thirdGroupSetting);
            result.AddRange(fourthGrouSetting);
            return result;
        }


        private static DateTimeOffset GetStartTime(List<OrderPricingAutoSetting> orderPricingAutoSettings, DateTimeOffset now, int index)
        {
            long timeRun = 0;
            for (int i = 0; i < index; i++)
            {
                timeRun += orderPricingAutoSettings[i].IntervalTime * orderPricingAutoSettings[i].Repeat;
            }
            return now.AddSeconds(timeRun);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public class OrderPricingAutoSetting
        {
            public decimal Value { get; set; }
            public int IntervalTime { get; set; }
            public int Repeat { get; set; }
        }

        public class PricingAutoSetting
        {
            public int SortedOrder { get; set; }
            public long? RouteSettingId { get; set; }
            public string VehicleTypeCode { get; set; }
        }
    }
}
