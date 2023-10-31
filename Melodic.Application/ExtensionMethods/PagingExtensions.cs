using AutoMapper;
using AutoMapper.QueryableExtensions;
using Melodic.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Melodic.Application.ExtensionMethods;

public static class PagingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
}
