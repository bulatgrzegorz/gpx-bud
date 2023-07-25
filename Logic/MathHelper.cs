namespace gpxSlopeCalculator.Logic;

public static class MathHelper
{    
    private const double ConstPiBy180 = Math.PI / 180.0;
    private const double Const180ByPi = 180.0 / Math.PI ;
    public static double RadiansToAngle(double radians) => radians * Const180ByPi;
    public static double AngleToRadians(double angle) => angle * ConstPiBy180;
    
    public static bool IsLocalMaximum(double[] values, int currentIndex, int toCompare)
    {
        var isLeftSideMaximum =
            currentIndex switch
            {
                0 => true,
                1 => values[currentIndex] >= values[currentIndex - 1],
                _ => values[currentIndex] >= values[Math.Max(currentIndex - toCompare - 1, 0)..(currentIndex - 1)].Max()
            };

        var isRightSideMaximum = 
            currentIndex switch
            {
                _ when currentIndex == values.Length - 1 => true,
                _ when currentIndex == values.Length - 2 => values[currentIndex] >= values[currentIndex + 1],
                _ => values[currentIndex] >= values[(currentIndex + 1)..Math.Min(currentIndex + toCompare + 1, values.Length - 1)].Max()
            };

        return isLeftSideMaximum && isRightSideMaximum;
    }

    public static bool IsLocalMinimum(double[] values, int currentIndex, int toCompare)
    {
        var isLeftSideMinimum =
            currentIndex switch
            {
                0 => true,
                1 => values[currentIndex] <= values[currentIndex - 1],
                _ => values[currentIndex] <= values[Math.Max(currentIndex - toCompare - 1, 0)..(currentIndex - 1)].Min()
            };

        var isRightSideMinimum = 
            currentIndex switch
            {
                _ when currentIndex == values.Length - 1 => true,
                _ when currentIndex == values.Length - 2 => values[currentIndex] <= values[currentIndex + 1],
                _ => values[currentIndex] <= values[(currentIndex + 1)..Math.Min(currentIndex + toCompare + 1, values.Length - 1)].Min()
            };

        return isLeftSideMinimum && isRightSideMinimum;
    }
}