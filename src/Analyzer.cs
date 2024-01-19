using System.Runtime.CompilerServices;

namespace GitDiffAnalyzer;

public class Analyzer
{
    private string _filePath { get; set;}
    private int _threshold { get; set;}

    private enum State
    {
        FileSection,
        FileName,
        ChangeBlock,
        RemovedLines,
        AddedLines
    }

    public Analyzer(string filePath, int threshold)
    {
        _filePath = Path.GetFullPath(filePath);
    }

    public async Task<ICollection<FileChange>> AnalyzeSignificantChangesAsync()
    {
        int numSignificantChanges = 0;

        var state = State.FileSection;
        var currentFileName = string.Empty;
        var newFileName = string.Empty;

        var fileChanges = new List<FileChange>();

        FileChange? curFileChange = null;
        ChangeBlock? curChangeBlock = null;

        var currentRemovedLines = new List<string>();
        var currentAddedLines = new List<string>();



        // Step 0: Read the file line by line
        await foreach( string line in File.ReadLinesAsync(_filePath))
        {
            newFileName = ExtractFileName(line);
            if(newFileName != null)
            {
                curFileChange = new FileChange();
                curFileChange.FileName = newFileName;
                   
                fileChanges.Add(curFileChange);                   

                state = State.FileName;
            }

            if(state == State.FileName)
            {
                //Skip lines until @@ is found
                if(line.StartsWith("@@"))
                {
                    state = State.ChangeBlock;
                }
            }

            if(state == State.ChangeBlock)
            { 
                // Step 2: Look for a line that starts with @@ (this is the line that tells us the line numbers that have changed)
                if (line.StartsWith("@@"))
                {
                    curChangeBlock = new ChangeBlock();
                    if(curFileChange == null) throw new Exception("curFileChange is null"); 

                    curFileChange.ChangeBlocks.Add(curChangeBlock);    
                }
                if(line.StartsWith("-"))
                {
                    if(curChangeBlock == null) throw new Exception("curChangeBlock is null"); 

                    // Step 4: Look for lines that start with - (this is the old version of the line) and combine this into a string
                    curChangeBlock.RemovedLines.Add(line[1..]);
                }
                if(line.StartsWith("+"))
                {
                    if(curChangeBlock == null) throw new Exception("curChangeBlock is null"); 

                    // Step 3: Look for lines that start with + (this is the new version of the line) and combine this into a string
                    curChangeBlock.AddedLines.Add(line[1..]);
                }
            }
        }
                    
        return fileChanges;
    }

    private string? ExtractFileName(string line)
    {
        if(line.StartsWith("diff --git"))
        {
            var fileName = line.Split(" ")[2];
            return fileName;
        }
        else
        {
            return null;
        }
    }

    
}
