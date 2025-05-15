using TransferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;
using TransferGo.Notifications.Domain.Entities;

namespace TransferGo.Notifications.Application.Mappings;

using AutoMapper;
using Common.Enums;

public class UserNotificationsMappingProfile : Profile
{
    public UserNotificationsMappingProfile()
    {
        CreateMap<Notification, UserNotification>()
            .ForMember(dest => dest.Channel, opt => opt.MapFrom(src => (Channel)src.Channel))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => (State)src.State));
    }
}