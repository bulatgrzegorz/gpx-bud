namespace gpxSlopeCalculator.Logic;

public static class Haversine
{
    public static double Calculate(double lat1, double lon1, double lat2, double lon2) {
        const int r = 6378100;
        var dLat = MathHelper.AngleToRadians(lat2 - lat1);
        var dLon = MathHelper.AngleToRadians(lon2 - lon1);
        lat1 = MathHelper.AngleToRadians(lat1);
        lat2 = MathHelper.AngleToRadians(lat2);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
        var c = 2 * Math.Asin(Math.Sqrt(a));
        return r * c;
    }
}