using gpxSlopeCalculator.Logic;
using gpxSlopeCalculator.Shared.Components;

string? content = File.ReadAllText(args[0]);

if (content == null)
{
    throw new ArgumentException("First choose file");
}
