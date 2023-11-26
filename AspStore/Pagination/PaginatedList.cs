using Microsoft.EntityFrameworkCore;

namespace AspStore.Pagination;

public class PaginatedList<T> : List<T>
{
    //https://www.youtube.com/watch?v=md3W1Ccssg8 - video explaining how the pagination works
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public int CurrentRange { get; private set; }
    public int InitialRange { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int productsPerPage)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)productsPerPage);
        InitialRange = 10;
        CurrentRange = ((PageIndex - 1) / InitialRange) * InitialRange + InitialRange;
        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1; //if i dont use the arrow function it would look like => HasPreviousPage {get{return PageIndex>1}}
    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(List<T> source, int pageIndex, int productsPerPage)
    {
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * productsPerPage).Take(productsPerPage).ToList();
        return new PaginatedList<T>(items, count, pageIndex, productsPerPage);
    }
}