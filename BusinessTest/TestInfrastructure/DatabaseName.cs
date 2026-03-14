namespace BusinessTest.TestInfrastructure;

internal static class DatabaseName
{
    public static string ForTest(string? testName = null)
    {
        var prefix = string.IsNullOrWhiteSpace(testName) ? "BusinessTest" : testName.Replace(' ', '_');
        return $"{prefix}_{Guid.NewGuid():N}";
    }
}

