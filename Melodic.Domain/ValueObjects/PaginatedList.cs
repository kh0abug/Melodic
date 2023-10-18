using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Domain.ValueObjects;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPage { get; set; }
    public PaginatedList(List<T> items,int count,int pageIndex,int pageSize)
    {
        PageIndex = pageIndex;
        TotalPage = (int)Math.Ceiling((double)count / pageSize);
        this.AddRange(items);
    }

    public bool HasPrevious => PageIndex > 1;
    public bool HasNext => PageIndex < TotalPage;

    public static PaginatedList<T> Paging(List<T> obj,int pageIndex,int pageSize)
    {
        var count = obj.Count;
        var items = obj.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items,count,pageIndex,pageSize);
    }
}
