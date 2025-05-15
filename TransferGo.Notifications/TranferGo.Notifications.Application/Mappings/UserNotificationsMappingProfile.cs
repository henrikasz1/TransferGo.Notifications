using AutoMapper;
using TranferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;
using TranferGo.Notifications.Domain.Entities;

namespace TranferGo.Notifications.Application.Mappings;

public class UserNotificationsMappingProfile : Profile
{
    public UserNotificationsMappingProfile()
    {
        CreateMap<Notification, UserNotification>()
            .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (Common.Enums.Channel)src.Channel))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => (Common.Enums.State)src.State));
    }
}