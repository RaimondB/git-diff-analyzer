using System;
using CommandLine;

namespace GitDiffAnalyzer;

class Program
{
    public class Options
    {
        [Option('f', "filePath", Required = true, HelpText = "File path of git diff file to analyze.")]
        public string FilePath { get; set; }

        [Option('o', "outputFilePath", Required = true, HelpText = "File path of the output file containing the results.")]
        public string OutputFilePath { get; set; }

        [Option('t', "threshold", Required = false, Default = 10, HelpText = "Threshold of Levenshtein distance to use to mark a significant change.")]
        public int Threshold { get; set; }

        [Option('v', "verbose", Required = false, Default = false, HelpText = "Adds details about the change blocks in each file to the output.")]
        public bool Verbose { get; set; }

        [Option('e', "filetypes", Required = false, Default = "md", HelpText = "Defines the filetypes that will be analyzed. Non-matching files will result in a considerable impact level directly.")]
        public IEnumerable<string> FileTypeToScan { get; set; }

    }

    public static async Task<int> Main(string[] args)
    {
        var retValue = await Parser.Default.ParseArguments<Options>(args)
                .MapResult(RunAsync, _ => Task.FromResult(1));

        Console.WriteLine($"Exit code= {retValue}");
        return retValue;
    }

  static async Task<int> RunAsync(Options options)
  {
        Console.WriteLine($"Evaluate Diff file: '{options.FilePath}'");
        Console.WriteLine($"Output Filepath: '{options.OutputFilePath}'");
        Console.WriteLine($"Threshold: '{options.Threshold}'");

        Console.WriteLine($"Verbose: '{options.Verbose}'");
        Console.WriteLine($"Filetypes part of analysis: '{String.Join(",", options.FileTypeToScan)}'");

        var analyzer = new Analyzer(options.FilePath, 10);

        var results = await analyzer.AnalyzeSignificantChangesAsync();

        bool diffHasSignificantChanges = false;

        foreach(var result in results)
        {
            // Validate if the Levenshtein distance is larger than the configured threshold, if so the change is considered significant
    
            var fileHasSignificantChanges = result.HasSignificantChanges(options.Threshold, options.FileTypeToScan);
            diffHasSignificantChanges |= fileHasSignificantChanges;

            var maxDistance = result.MaxDistance();

            Console.WriteLine();
            Console.WriteLine($"File: {result.FileName} | Blocks: {result.NrOfChangeBlocks} | Added Lines: {result.TotalAdded} | Removed Lines: {result.TotalRemoved} | Max Distance: {maxDistance} | Significant changes: {fileHasSignificantChanges}");

            if(options.Verbose)
            {
                foreach(var cb in result.ChangeBlocks)
                {
                    Console.WriteLine($"  ChangeBlock: distance {cb.Distance}");
                    foreach(var line in cb.RemovedLines)
                    {
                        Console.WriteLine($"    - {line}");
                    }
                    foreach(var line in cb.AddedLines)
                    {
                        Console.WriteLine($"    + {line}");
                    }
                }
            } 
        }

        await WriteOutputToFileAsync(options.OutputFilePath, diffHasSignificantChanges, results.Max(r => r.MaxDistance()));

        Console.WriteLine($"Done");
        return 0;
  }

  // Create a function to write the output to a file
  static  async Task WriteOutputToFileAsync(string outputFilePath, bool hasSignificantChanges, int maxDistance)
  {
        using(var writer = new StreamWriter(outputFilePath))
        {
            await writer.WriteLineAsync($"impact-level={(hasSignificantChanges ? "considerable" : "low")}");	
            await writer.WriteLineAsync($"max-ld-distance={maxDistance}");
        }
  }
}