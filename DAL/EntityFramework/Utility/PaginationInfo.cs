namespace DAL.EntityFramework.Utility;

public class PaginationInfo
{
    public int PageIndex { get; set; }

    public int TotalPageCount { get; set; }

    public int TotalRecordCount { get; set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPageCount;
}