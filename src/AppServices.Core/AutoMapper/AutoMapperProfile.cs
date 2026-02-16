using AirWeb.AppServices.Core.EntityServices.AuditPoints;
using AirWeb.AppServices.Core.EntityServices.Comments;
using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Staff.Dto;
using AirWeb.Core.Entities;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AirWeb.AppServices.Core.AutoMapper;

public static class ProfileRegistration
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) =>
        services.AddAutoMapper(expression => expression.AddProfile<AutoMapperProfile>());
}

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Users
        CreateMap<ApplicationUser, StaffSearchResultDto>();
        CreateMap<ApplicationUser, StaffViewDto>();

        // Offices
        CreateMap<Office, OfficeUpdateDto>();
        CreateMap<Office, OfficeViewDto>();

        // Comments
        CreateMap<Comment, CommentViewDto>();

        // AuditPoints()
        CreateMap<AuditPoint, AuditPointViewDto>();
    }
}
