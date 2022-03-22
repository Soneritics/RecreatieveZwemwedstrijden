namespace Models;

public class GeneratedProgram
{
    public string ProgramId { get; set; }

    public Dictionary<int, int> NrOfSwimmersPerSeries { get; set; }

    public List<string> SwimmersOrdered { get; set; }

    public Dictionary<int, Dictionary<int, string>> GetSeries()
    {
        // Order of the fastest swimmer in the series
        var laneOrder = new [] { 3, 4, 2, 5, 1, 6 };

        // the result is a dictionary of series, lane, swimmer id
        var result = new Dictionary<int, Dictionary<int, string>>();

        // Get the swimmers as a list
        var swimmers = SwimmersOrdered.ToList();

        // Create all the necessary objects in the correct order
        foreach (var series in NrOfSwimmersPerSeries)
        {
            result[series.Key] = new Dictionary<int, string>();
        }

        // Loop the series and place the swimmers
        foreach (var series in NrOfSwimmersPerSeries.OrderByDescending(x => x.Key))
        {
            var seriesNumber = series.Key;
            var swimmersInSeries = series.Value;

            for (var i = 0; i < swimmersInSeries; i++)
            {
                var lane = laneOrder[i];
                var swimmerId = swimmers.First();

                result[series.Key][lane] = swimmerId;

                swimmers.RemoveAt(0);
            }
        }

        // Make sure there is at least one program
        if (!result.Any())
        {
            result[1] = new Dictionary<int, string>();
        }

        return result;
    }
}