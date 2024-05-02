using GaEpd.AppLibrary.Pagination;

namespace AirWeb.WebApp.Models;

public record PaginationNavModel(IPaginatedResult Paging, IDictionary<string, string?> RouteValues);
