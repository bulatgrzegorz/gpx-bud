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
    
    public static string SlopeColorPerGainHex(double slopeAngle)
    {
        var color = SlopeColorPerGain(slopeAngle);
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static string GetSlopeTypeSuffix(SlopeGradeType type)
    {
        return type switch
        {
            SlopeGradeType.Angle => "°", 
            SlopeGradeType.Percent => "%", 
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public static string GetSlopeLabel(double slope)
    {
        return slope switch
        {
            _ when slope < -40 => "<-40",
            _ when slope < -35 => "<-35",
            _ when slope < -25 => "<-25",
            _ when slope < -15 => "<-15",
            _ when slope < -5 => "<-5",
            _ when slope < 0 => "<0",
            _ when slope < 5 => "<5",
            _ when slope < 15 => "<15",
            _ when slope < 25 => "<25",
            _ when slope < 35 => "<35",
            _ => ">35",
        };
    }

    private static Color SlopeColorPerGain(double slope)
    {
        return slope switch
        {
            _ when slope < -40 => Color.FromArgb(72, 143, 49),
            _ when slope < -35 => Color.FromArgb(63, 150, 104),
            _ when slope < -25 => Color.FromArgb(102, 164, 100),
            _ when slope < -15 => Color.FromArgb(140, 177, 97),
            _ when slope < -5 => Color.FromArgb(179, 189, 97),
            _ when slope < 0 => Color.FromArgb(219, 198, 103),
            _ when slope < 5 => Color.FromArgb(226, 175, 85),
            _ when slope < 15 => Color.FromArgb(230, 150, 75),
            _ when slope < 25 => Color.FromArgb(232, 123, 74),
            _ when slope < 35 => Color.FromArgb(230, 95, 80),
            _ => Color.FromArgb(222, 66, 91),
        };
    }
}