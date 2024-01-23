namespace GitDiffAnalyzer.Tests;
using GitDiffAnalyzer;

public class AnalyzerUnitTests
{
    [Fact]
    public async void TestGitDiffWithFilesAndChangeBlocks()
    {
        //Validate Content of the diff file analysis
        var sut = new Analyzer("gitdiff.txt");
        var result = await sut.AnalyzeSignificantChangesAsync();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async void TestEmptyGitDiff()
    {
        //Validate Content of the diff file analysis
        var sut = new Analyzer("empty-gitdiff.txt");
        var result = await sut.AnalyzeSignificantChangesAsync();

        Assert.Equal(0, result.Count);
    }


    [Fact]
    public async void TestGitDiffWithFilesThatHaveNoChangeBlocks()
    {
        //Validate Content of the diff file analysis
        var sut = new Analyzer("empty-changeblocks-gitdiff.txt");
        var result = await sut.AnalyzeSignificantChangesAsync();

        var filesWithNoChangeBlocks = result.Where(f => f.NrOfChangeBlocks == 0).ToList();

        Assert.Equal(14, result.Count);
        Assert.Equal(2, filesWithNoChangeBlocks.Count);
    }
}