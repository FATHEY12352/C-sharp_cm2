using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            return visits.DefaultIfEmpty()
                .GroupBy(visit => visit?.UserId)
                .Select(group => group.OrderBy(visit => visit?.DateTime))
                .Select(orderedVisit => ExtensionsTask.Bigrams(orderedVisit))
                .SelectMany(bigrams => bigrams
                    .Where(b => b.First.SlideType == slideType)
                    .Select(b => (b.Second.DateTime - b.First.DateTime).TotalMinutes))
                .Where(time => time >= 1 && time <= 120)
                .DefaultIfEmpty(0)
                .Median();
        }
    }
}
