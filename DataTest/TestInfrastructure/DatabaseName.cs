namespace DataTest.TestInfrastructure;

internal static class DatabaseName
{
    public static string ForTest(string? testName = null)
    {
        var prefix = string.IsNullOrWhiteSpace(testName) ? "DataTest" : testName.Replace(' ', '_');
        return $"{prefix}_{Guid.NewGuid():N}";
    }
}


