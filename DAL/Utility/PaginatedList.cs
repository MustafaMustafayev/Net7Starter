using Microsoft.EntityFrameworkCore;

namespace DAL.Utility;

public class PaginatedList<T> : PaginationInfo
{
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        if (pageIndex != 0)
        {
            PageIndex = pageIndex;
            TotalPageCount = (int)Math.Ceiling(count / (double)pageSize);
        }
        else
        {
            PageIndex = 1;
            TotalPageCount = 1;
        }

        TotalRecordCount = count;
        Datas = items;
    }

    public List<T> Datas { get; set; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex,
        int pageSize)
    {
        var count = await source.CountAsync();
        List<T> items;
        if (pageIndex != 0)
            items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        else
            items = await source.ToListAsync();

        var response = new PaginatedList<T>(items, count, pageIndex, pageSize);
        return response;
    }
}