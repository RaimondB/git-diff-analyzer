public class ChangeBlock
{
    public int Distance => Evaluate();
    public ICollection<string> RemovedLines = new List<string>();
    public ICollection<string> AddedLines = new List<string>();

    public int Evaluate ()
    {
        // Step 5: Compare the two strings and calculate the Levenshtein distance
        var removedLines = string.Join("", RemovedLines);
        var addedLines = string.Join("", AddedLines);
        
        int distance = Fastenshtein.Levenshtein.Distance(removedLines, addedLines);
        
        return distance;
    }
}

public class FileChange
{
    public string FileName { get; set; }

    public bool HasSignificantChanges(int threshold) => ChangeBlocks != null ? ChangeBlocks.Any(cb => cb.Distance > threshold) : false;

    public int MaxDistance() => ChangeBlocks != null ? ChangeBlocks.Max(cb => cb.Distance) : 0;

    public int TotalAdded => ChangeBlocks != null ? ChangeBlocks.Sum(cb => cb.AddedLines.Count) : 0;
    public int TotalRemoved => ChangeBlocks != null ? ChangeBlocks.Sum(cb => cb.RemovedLines.Count) : 0;

    public int NrOfChangeBlocks => ChangeBlocks != null ? ChangeBlocks.Count : 0;

    public ICollection<ChangeBlock> ChangeBlocks = new List<ChangeBlock>();
}
