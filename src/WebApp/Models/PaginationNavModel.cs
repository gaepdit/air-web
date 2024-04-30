using GaEpd.AppLibrary.Pagination;

namespace MyApp.WebApp.Models;

public record PaginationNavModel(IPaginatedResult Paging, IDictionary<string, string?> RouteValues);
