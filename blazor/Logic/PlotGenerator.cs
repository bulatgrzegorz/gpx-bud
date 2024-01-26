using System.Drawing;

namespace gpxSlopeCalculator.Logic;

public static class PlotGenerator
{
    public static double CalculateSlopeGrade(double rise, double run, SlopeGradeType slopeGradeType)
    {
        return slopeGradeType switch
        {
            SlopeGradeType.Percent => 100 * rise / run,
            SlopeGradeType.Angle => MathHelper.RadiansToAngle(Math.Atan(rise / run)),
            _ => throw new NotSupportedException($"Slope grade type: {slopeGradeType} is not supported")
        };
    }
}