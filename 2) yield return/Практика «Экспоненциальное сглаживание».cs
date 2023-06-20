using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        var smoothedValues = new List<double>();
        foreach (var point in data)
        {
            var smoothedY = smoothedValues.Count > 0 
                ? alpha * point.OriginalY + (1 - alpha) * smoothedValues[smoothedValues.Count - 1]
                : point.OriginalY;
            
            smoothedValues.Add(smoothedY);
            yield return point.WithExpSmoothedY(smoothedY);
        }
    }
}
