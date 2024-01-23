namespace gpxSlopeCalculator.Logic;

public static class PercentileCalculator
{
    /// <summary>
    /// Calculates value for given percentile about given sequence.
    /// </summary>
    /// <param name="sequence"></param>
    /// <param name="percentile"></param>
    /// <returns></returns>
    public static double Percentile(double[] sequence, double percentile)
    {
        var sortedSequence = sequence.Order().ToArray();

        var sequenceLength = sequence.Length;
        var n = (sequenceLength - 1) * percentile + 1;
        if (Math.Abs(n - 1d) < 0.01) return sortedSequence[0];
        if (Math.Abs(n - sequenceLength) < 0.01) return sortedSequence[sequenceLength - 1];
        var k = (int)n;
        var d = n - k;
        return sortedSequence[k - 1] + d * (sortedSequence[k] - sortedSequence[k - 1]);
    }
}