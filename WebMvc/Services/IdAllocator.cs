namespace WebMvc.Services;

public static class IdAllocator
{
    public static int NextId(IEnumerable<int> ids)
    {
        var max = 0;
        foreach (var id in ids)
        {
            if (id > max)
            {
                max = id;
            }
        }

        return max + 1;
    }
}

