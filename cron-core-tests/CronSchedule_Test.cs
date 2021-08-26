using System;
using System.Collections.Generic;
using cron_core;
using Xunit;

namespace cron_core_tests
{
    public class CronSchedule_Test
    {
        [Theory]
        [InlineData("*/2", true)]
        [InlineData("* * * * *", true)]
        [InlineData("0 * * * *", true)]
        [InlineData("0,1,2 * * * *", true)]
        [InlineData("*/2 * * * *", true)]
        [InlineData("1-4 * * * *", true)]
        [InlineData("1-55/3 * * * *", true)]
        [InlineData("1,10,20 * * * *", true)]
        [InlineData("* 1,10,20 * * *", true)]
        [InlineData("", false)]
        public void is_valid_Scheduler(string schedule, bool expected)
        {
            var cron_schedule = new CronSchedule();
            Assert.Equal(cron_schedule.isValid(schedule), expected);
        }

        [Fact]
        public static void divided_array_test()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.minutes.GetRange(0, 5);//("*/2", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public static void range_array_test()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);//();
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var cs = new CronSchedule("1-10/3 20-45/4 * * *");
            results = cs.minutes;
            Assert.Equal(results.ToArray(), new int[] { 3, 6, 9 });
        }

        [Fact]
        public void wild_array_test()
        {
            var cron_schedule = new CronSchedule("*");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);//("*", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Fact]
        public void list_array_test()
        {
            var cron_schedule = new CronSchedule("1,2,3,4,5,6,7,8,9,10");
            List<int> results = cron_schedule.minutes;
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void generate_values_divided_test()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.minutes.GetRange(0,5);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public void generate_values_range_test()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.minutes.GetRange(0,10);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void generate_minutes_test()
        {
            var cron_schedule = new CronSchedule("1,2,3 * * * *");
            Assert.Equal(cron_schedule.minutes.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_hours_test()
        {
            var cron_schedule = new CronSchedule("* 1,2,3 * * *");
            Assert.Equal(cron_schedule.hours.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_days_of_month_test()
        {
            var cron_schedule = new CronSchedule("* * 1,2,3 * *");
            Assert.Equal(cron_schedule.days_of_month.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_months_test()
        {
            var cron_schedule = new CronSchedule("* * * 1,2,3 *");
            Assert.Equal(cron_schedule.months.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void generate_days_of_weeks()
        {
            var cron_schedule = new CronSchedule("* * * * 1,2,3 ");
            Assert.Equal(cron_schedule.days_of_week.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void is_time_minute_test()
        {
            var cron_schedule = new CronSchedule("0 * * * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("8:00 am")));
            Assert.False(cron_schedule.isTime(DateTime.Parse("8:01 am")));

            cron_schedule = new CronSchedule("0-10 * * * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("8:00 am")));
            Assert.True(cron_schedule.isTime(DateTime.Parse("8:03 am")));

            cron_schedule = new CronSchedule("*/2 * * * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("8:00 am")));
            Assert.True(cron_schedule.isTime(DateTime.Parse("8:02 am")));
            Assert.False(cron_schedule.isTime(DateTime.Parse("8:03 am")));
        }

        [Fact]
        public void is_time_hour_test()
        {
            var cron_schedule = new CronSchedule("* 0 * * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("12:00 am")));

            cron_schedule = new CronSchedule("* 0,12 * * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("12:00 am")));
            Assert.True(cron_schedule.isTime(DateTime.Parse("12:00 pm")));
        }

        [Fact]
        public void is_time_day_of_month_test()
        {
            var cron_schedule = new CronSchedule("* * 1 * *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("2010/08/01")));
        }

        [Fact]
        public void is_time_month_test()
        {
            var cron_schedule = new CronSchedule("* * * 1 *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * 12 *");
            Assert.False(cron_schedule.isTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * */3 *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("3/1/2008")));
            Assert.True(cron_schedule.isTime(DateTime.Parse("6/1/2008")));
        }

        [Fact]
        public void is_time_day_of_week_test()
        {
            var cron_schedule = new CronSchedule("* * * * 0");
            Assert.True(cron_schedule.isTime(DateTime.Parse("10/12/2008")));
            Assert.False(cron_schedule.isTime(DateTime.Parse("10/13/2008")));

            cron_schedule = new CronSchedule("* * * * */2");
            Assert.True(cron_schedule.isTime(DateTime.Parse("10/14/2008")));
        }

        [Fact]
        public void is_time_test()
        {
            var cron_schedule = new CronSchedule("0 0 12 10 *");
            Assert.True(cron_schedule.isTime(DateTime.Parse("12:00:00 am 10/12/2008")));
            Assert.False(cron_schedule.isTime(DateTime.Parse("12:01:00 am 10/12/2008")));
        }
        
    }
}
