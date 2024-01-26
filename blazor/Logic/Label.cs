using System.Drawing;

namespace gpxSlopeCalculator.Logic;

public record Label(string Value, int Order, Color Color);

public static class LabelExtensions
{
    private static readonly Label Minus40Label = new("<-40", -40, Color.FromArgb(72, 143, 49));
    private static readonly Label Minus35Label = new("<-35", -35, Color.FromArgb(63, 150, 104));
    private static readonly Label Minus25Label = new("<-25", -25, Color.FromArgb(102, 164, 100));
    private static readonly Label Minus15Label = new("<-15", -15, Color.FromArgb(140, 177, 97));
    private static readonly Label Minus5Label = new("<-5", -5, Color.FromArgb(179, 189, 97));
    private static readonly Label ZeroLabel = new("<0", 0, Color.FromArgb(219, 198, 103));
    private static readonly Label Plus5Label = new("<5", 5, Color.FromArgb(226, 175, 85));
    private static readonly Label Plus15Label = new("<15", 15, Color.FromArgb(230, 150, 75));
    private static readonly Label Plus25Label = new("<25", 25, Color.FromArgb(232, 123, 74));
    private static readonly Label Plus35Label = new("<35", 35, Color.FromArgb(230, 95, 80));
    private static readonly Label PlusInfLabel = new(">35", 10000, Color.FromArgb(222, 66, 91));

    public static Label GetLabel(double slope)
    {
        return slope switch
        {
            _ when slope < -40 => Minus40Label,
            _ when slope < -35 => Minus35Label,
            _ when slope < -25 => Minus25Label,
            _ when slope < -15 => Minus15Label,
            _ when slope < -5 => Minus5Label,
            _ when slope < 0 => ZeroLabel,
            _ when slope < 5 => Plus5Label,
            _ when slope < 15 => Plus15Label,
            _ when slope < 25 => Plus25Label,
            _ when slope < 35 => Plus35Label,
            _ => PlusInfLabel,
        };
    }
    
    public static string SlopeColorPerGainHex(Label label)
    {
        var color = label.Color;
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


}