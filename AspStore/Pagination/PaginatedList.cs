namespace AspStore.Pagination;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int count, int pageIndex, int productsPerPage)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)productsPerPage);
        InitialRange = 10;
        CurrentRange = (PageIndex - 1) / InitialRange * InitialRange + InitialRange;
        AddRange(items);
    }

    //https://www.youtube.com/watch?v=md3W1Ccssg8 - video explaining how the pagination works
    public int PageIndex { get; }
    public int TotalPages { get; }
    public int CurrentRange { get; private set; }
    public int InitialRange { get; }

    public bool HasPreviousPage =>
        PageIndex > 1; //if i dont use the arrow function it would look like => HasPreviousPage {get{return PageIndex>1}}

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(List<T> source, int pageIndex, int productsPerPage)
    {
        var count = source.Count;
        var items = source.Skip((pageIndex - 1) * productsPerPage).Take(productsPerPage).ToList();
        return new PaginatedList<T>(items, count, pageIndex, productsPerPage);
    }
}