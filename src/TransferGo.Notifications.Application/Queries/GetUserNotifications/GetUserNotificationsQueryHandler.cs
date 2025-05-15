namespace TransferGo.Notifications.Application.Queries.GetUserNotifications;

using AutoMapper;
using MediatR;
using Contracts;
using Repositories;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, List<UserNotification>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;
    
    public GetUserNotificationsQueryHandler(
        INotificationRepository notificationRepository,
        IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<List<UserNotification>> Handle(GetUserNotificationsQuery query, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetUserNotifications(query.UserId);

        return _mapper.Map<List<UserNotification>>(notifications);
    }
}