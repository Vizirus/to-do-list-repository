namespace WebMvc.Services;

public static class Pagination
{
    public static PagedResult<T> Page<T>(IReadOnlyList<T> items, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var total = items.Count;
        var skip = (page - 1) * pageSize;
        var pageItems = items.Skip(skip).Take(pageSize).ToArray();

        return new PagedResult<T>
        {
            Items = pageItems,
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
        };
    }
}

