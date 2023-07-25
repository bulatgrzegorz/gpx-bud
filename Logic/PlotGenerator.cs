using System.Drawing;

namespace gpxSlopeCalculator.Logic;

public static class PlotGenerator
{
    public static string SlopeColorPerGainHex(double slopeAngle)
    {
        var color = SlopeColorPerGain(slopeAngle);
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
    
    public static string GetAngleLabel(double slopeAngle)
    {
        return slopeAngle switch
        {
            _ when slopeAngle < -40 => "<-40",
            _ when slopeAngle < -35 => "<-35",
            _ when slopeAngle < -25 => "<-25",
            _ when slopeAngle < -15 => "<-15",
            _ when slopeAngle < -5 => "<-5",
            _ when slopeAngle < 0 => "<0",
            _ when slopeAngle < 5 => "<5",
            _ when slopeAngle < 15 => "<15",
            _ when slopeAngle < 25 => "<25",
            _ when slopeAngle < 35 => "<35",
            _ => ">35",
        };
    }

    private static Color SlopeColorPerGain(double slopeAngle)
    {
        return slopeAngle switch
        {
            _ when slopeAngle < -40 => Color.FromArgb(72, 143, 49),
            _ when slopeAngle < -35 => Color.FromArgb(63, 150, 104),
            _ when slopeAngle < -25 => Color.FromArgb(102, 164, 100),
            _ when slopeAngle < -15 => Color.FromArgb(140, 177, 97),
            _ when slopeAngle < -5 => Color.FromArgb(179, 189, 97),
            _ when slopeAngle < 0 => Color.FromArgb(219, 198, 103),
            _ when slopeAngle < 5 => Color.FromArgb(226, 175, 85),
            _ when slopeAngle < 15 => Color.FromArgb(230, 150, 75),
            _ when slopeAngle < 25 => Color.FromArgb(232, 123, 74),
            _ when slopeAngle < 35 => Color.FromArgb(230, 95, 80),
            _ => Color.FromArgb(222, 66, 91),
        };
    }
    
    public static object Config => new
    {
        Type = "line",
        options = new
        {
            maintainAspectRatio = false,
        },
        Data = new
        {
            Datasets = new[]
            {
                new {
                    label = "My First dataset",
                    backgroundColor = "rgb(255, 99, 132)",
                    borderColor = "rgb(255, 99, 132)",
                    data = new [] { 0, 10, 5, 2, 20, 30, 45 },
                }
                            
            },
            Labels = new[] 
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June"
            }
        }
    };
}