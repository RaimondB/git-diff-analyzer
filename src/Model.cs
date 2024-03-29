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

    public bool HasSignificantChanges(int threshold, IEnumerable<string> fileTypesToScan)
    {
        bool isScannedFileType = true;

        if(fileTypesToScan != null && fileTypesToScan.Any())
        {
            var extension = Path.GetExtension(this.FileName);

            isScannedFileType = fileTypesToScan.Contains(extension);
        }

        if(!isScannedFileType)
        {
            // Always mention significant changes for files that are not part of the scanned file types
            return true;
        }
        else
        {
            // For scanned filetypes, check the threshold
            return ChangeBlocks != null ? ChangeBlocks.Any(cb => cb.Distance > threshold) : false;
        }
    }

    private bool HasChangeBlocks => ChangeBlocks != null && ChangeBlocks.Any();

    public int MaxDistance() => HasChangeBlocks ? ChangeBlocks.Max(cb => cb.Distance ) : 0;

    public int TotalAdded => HasChangeBlocks ? ChangeBlocks.Sum(cb => cb.AddedLines.Count) : 0;
    public int TotalRemoved => HasChangeBlocks ? ChangeBlocks.Sum(cb => cb.RemovedLines.Count) : 0;

    public int NrOfChangeBlocks => ChangeBlocks != null ? ChangeBlocks.Count : 0;

    public ICollection<ChangeBlock> ChangeBlocks = new List<ChangeBlock>();
}
