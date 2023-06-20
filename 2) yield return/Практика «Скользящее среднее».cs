using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var windowPoints = new List<double>();
            var result = 0.0;
            foreach (var dataItem in data)
            {
                windowPoints.Add(dataItem.OriginalY);
                result += dataItem.OriginalY;
                if (windowPoints.Count > windowWidth)
                {
                    result -= windowPoints[0];
                    windowPoints.RemoveAt(0);
                }
                yield return dataItem.WithAvgSmoothedY(result / windowPoints.Count);
            }
        }
    }
}
