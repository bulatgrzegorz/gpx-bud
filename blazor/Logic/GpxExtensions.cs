namespace gpxSlopeCalculator.Logic;

public static class GpxExtensions
{
    private static Point ToPoint(this Trkpt value) => new Point(value.Lat, value.Lon, value.Ele);

    //Missing elevation is being set as -1 
    private static bool IsValidForCalculations(this Trkpt value) => Math.Abs(value.Ele + 1) > 0.01;

    public static List<Point> PrepareGpxPoints(this Gpx gpx, double smoothingWindowThreshold)
    {
        var points = gpx.Trk.Trkseg.Trkpt.Where(x => x.IsValidForCalculations()).Select(x => x.ToPoint()).ToList();

        //|\                      |
        //| \    |\               |
        //|  \ --  \        /\  / |
        //|   V     \  ^   /  \/  |
        //|          \/ \_-       |
        //|---------------------- |
        //|<---100m---*---100m--->|
        // we are calculating sliding window of given length (by distance) for each point
        // then we are using cumulative elevation of each point in this window and calculate mean of it - which became smoothed elevation of point  
        var windowStartIndex = 0;
        var windowsEndIndex = -1;
        var windowElevation = 0d;
        for (var i = 0; i < points.Count; i++)
        {
            while (windowStartIndex < i && points[windowStartIndex].Distance(points[i]) > smoothingWindowThreshold)
            {
                windowElevation -= points[windowStartIndex].Elevation;
                windowStartIndex++;
            }

            while (windowsEndIndex + 1 < points.Count &&
                   points[i].Distance(points[windowsEndIndex + 1]) <= smoothingWindowThreshold)
            {
                windowElevation += points[windowsEndIndex + 1].Elevation;
                windowsEndIndex++;
            }

            points[i].SmoothedElevation = windowElevation / (windowsEndIndex - windowStartIndex + 1);
        }

        if (points.Count > 0)
        {
            points[0].SmoothedElevation = points[0].Elevation;
            points[^1].SmoothedElevation = points[^1].Elevation;
        }

        var result = new List<Point>();
        var totalDistance = 0d;
        var lastValid = points[0];
        result.Add(lastValid);
        for (var i = 1; i < points.Count; i++)
        {
            var currentPoint = points[i];
            var pointDistance = lastValid.Distance(currentPoint);

            totalDistance += pointDistance;

            currentPoint.TotalDistance = Math.Round(totalDistance, 2);
            currentPoint.PointDistance = pointDistance;
            result.Add(currentPoint);

            lastValid = currentPoint;
        }

        return result;
    }
    
    public static double CalculateElevationGain(this List<Point> points)
    {
        double result = 0;
    
        var elevationLastValid = points[0];
        for (var i = 1; i < points.Count; i++)
        {
            var currentPoint = points[i];
            var difference = currentPoint.SmoothedElevation - elevationLastValid.SmoothedElevation;
            if (difference > 0)
            {
                result += difference;
            }

            elevationLastValid = currentPoint;
        }

        return result;
    }
}