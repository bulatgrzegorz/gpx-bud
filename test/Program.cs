using gpxSlopeCalculator.Logic;
using gpxSlopeCalculator.Shared.Components;

string? content = File.ReadAllText(args[0]);
List<Chart.Dataset> _datasetData = new();

if (content == null)
{
    throw new ArgumentException("First choose file");
}

_datasetData.Clear();
double _totalDistance = 0;
var gpxContent = GpxSerializer.Deserialize(content);
var _sumElevationGain = CalculateElevationGain(gpxContent);
(_totalDistance, var elevationPerDistance) = CalculateTotalDistanceAndElevations(gpxContent);

var elevetions = elevationPerDistance.Select(x => x.elevetion).ToArray();

var secondSlopeDerivative = SlopeDerivativeCalculator.GetSecondSlopeDerivative(elevetions);
var slopePercentile = PercentileCalculator.Percentile(secondSlopeDerivative, 0.95);

var k = 0;
double slopeDistance = 0;
double? previousSlope = null;
var firstElevation = elevationPerDistance.First().elevetion;
var previousSlopeElevation = firstElevation;

var iterationElevations = new List<(double elevetion, double totalDistance)>();
for (var i = 0; i < elevationPerDistance.Count - 1; i++)
{
    var e = elevationPerDistance[i];

    iterationElevations.Add((e.elevetion, e.totalDistance));
    slopeDistance += e.pointDistance;

    if (slopeDistance < 100)
    {
        k++;
        continue;
    }

    var isHighSlope = secondSlopeDerivative[k] >= slopePercentile;
    var isLocalOrMaximum = MathHelper.IsLocalMaximum(elevetions, k, 3) || MathHelper.IsLocalMinimum(elevetions, k, 3);
    if (!isHighSlope && !isLocalOrMaximum)
    {
        k++;
        continue;
    }
    
    var slope = PlotGenerator.CalculateSlopeGrade(e.elevetion - previousSlopeElevation, slopeDistance, SlopeGradeType.Percent);
    //this mean that we do have new slope, but actual one should be merged with previous one. We will recreate it
    if (previousSlope.HasValue && Math.Abs(previousSlope.Value - slope) < 2)
    {
        var lastIterationDataset = _datasetData.Last();
        var previousIterationElevations = lastIterationDataset.Data;
        var previousIterationBeginningElevation = previousIterationElevations[0].Elevetion;
        var combinedSlopeDistance = e.totalDistance - previousIterationElevations[0].TotalDistance;
        slope = PlotGenerator.CalculateSlopeGrade(e.elevetion - previousIterationBeginningElevation, combinedSlopeDistance, SlopeGradeType.Percent);

        iterationElevations = previousIterationElevations.Concat(iterationElevations).ToList();

        _datasetData.Remove(lastIterationDataset);
    }

    if (i != elevationPerDistance.Count - 1)
    {
        var nextIterationValue = elevationPerDistance[i + 1]; 
        iterationElevations.Add((nextIterationValue.elevetion, nextIterationValue.totalDistance));
    }

    var dataset = new Chart.Dataset() { Data = iterationElevations.ToArray() };

    if (slope is Double.PositiveInfinity or Double.NegativeInfinity)
    {
        Console.WriteLine();
    }
    _datasetData.Add(dataset);
    
    iterationElevations.Clear();
    previousSlopeElevation = e.elevetion;
    slopeDistance = 0;
    previousSlope = slope;
    k++;
}

(double totalDistance, List<(double elevetion, double totalDistance, double pointDistance)> elevationsPerDistance) CalculateTotalDistanceAndElevations(Gpx gpxContent)
{
    var elevationPerDistance = new List<(double elevetion, double totalDistance, double pointDistance)>();
    double totalDistance = 0;

    var lastValid = gpxContent.Trk.Trkseg.Trkpt[0];
    for (var i = 1; i < gpxContent.Trk.Trkseg.Trkpt.Count; i++)
    {
        var currentPoint = gpxContent.Trk.Trkseg.Trkpt[i];
        var pointDistance = Haversine.Calculate(lastValid.Lat, lastValid.Lon, currentPoint.Lat, currentPoint.Lon);
        if (pointDistance < 10)
        {
            continue;
        }

        totalDistance += pointDistance;

        elevationPerDistance.Add((currentPoint.Ele, Math.Round(totalDistance, 2), pointDistance));

        lastValid = currentPoint;
    }

    return (totalDistance, elevationPerDistance);
}

double CalculateElevationGain(Gpx gpxContent)
{
    double result = 0;

    var elevationLastValid = gpxContent.Trk.Trkseg.Trkpt[0];
    for (var i = 1; i < gpxContent.Trk.Trkseg.Trkpt.Count; i++)
    {
        var currentPoint = gpxContent.Trk.Trkseg.Trkpt[i];
        
        var difference = currentPoint.Ele - elevationLastValid.Ele;
        if (!(Math.Abs(difference) > 10)) continue;
        
        if (difference > 0)
        {
            result += difference;
        }

        elevationLastValid = currentPoint;
    }

    return result;
}