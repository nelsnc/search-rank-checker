namespace SearchRankChecker.Tests.TestHelpers;

public static class HtmlLoader
{
    public static string Load(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
        return File.ReadAllText(path);
    }
}