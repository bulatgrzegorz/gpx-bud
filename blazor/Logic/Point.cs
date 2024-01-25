namespace gpxSlopeCalculator.Logic;

public record Point(double Lat, double Lon, double Elevation)
{
    public double PointDistance { get; set; }
    public double TotalDistance { get; set; }
    public double SmoothedElevation { get; set; }

    public double Distance(Point other) => Haversine.Calculate(Lat, Lon, other.Lat, other.Lon);
};