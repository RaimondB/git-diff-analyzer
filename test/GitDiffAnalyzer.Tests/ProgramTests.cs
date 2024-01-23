namespace GitDiffAnalyzer.Tests;
using GitDiffAnalyzer;

public class IntegrationTests
{
    [Fact]
    public void TestGitDiffWithFilesAndChangeBlocks()
    {
        //Check that running the program does not throw an exception
        var result = Program.RunAsync(new Program.Options
        {
            FilePath = "gitdiff.txt",
            OutputFilePath = "testoutput/gitdiff_with_files_and_change_blocks_output.txt",
            Threshold = 10,
            Verbose = true,
            FileTypeToScan = new List<string> { ".md" }
        });
    }

    [Fact]
    public void TestEmptyGitDiff()
    {
        //Check that running the program does not throw an exception
        var result =  Program.RunAsync(new Program.Options
        {
            FilePath = "empty-gitdiff.txt",
            OutputFilePath = "testoutput/gitdiff_empty_output.txt",
            Threshold = 10,
            Verbose = true,
            FileTypeToScan = new List<string> { ".md" }
        });
    }


    [Fact]
    public void TestGitDiffWithFilesThatHaveNoChangeBlocks()
    {
        //Check that running the program does not throw an exception
        var result =  Program.RunAsync(new Program.Options
        {
            FilePath = "empty-changeblocks-gitdiff.txt",
            OutputFilePath = "testoutput/gitdiff_with_files_no_change_blocks_output.txt",
            Threshold = 10,
            Verbose = true,
            FileTypeToScan = new List<string> { ".md" }
        });
    }

}