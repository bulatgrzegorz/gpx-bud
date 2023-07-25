namespace gpxSlopeCalculator.Logic;

public static class SlopeDerivativeCalculator
{
    public static double[] GetSecondSlopeDerivative(double[] elevations)
    {
        var firstSlopeDerivative = new double[elevations.Length];
        firstSlopeDerivative[0] = 0;
        for (var l = 1; l < elevations.Length - 1; l++)
        {
            firstSlopeDerivative[l] = elevations[l] - elevations[l - 1];
        }

        var secondSlopeDerivative = new double[firstSlopeDerivative.Length];
        secondSlopeDerivative[0] = 0;
        for (var l = 1; l < firstSlopeDerivative.Length - 1; l++)
        {
            secondSlopeDerivative[l] = firstSlopeDerivative[l] - firstSlopeDerivative[l - 1];
        }

        return secondSlopeDerivative;
    }
}