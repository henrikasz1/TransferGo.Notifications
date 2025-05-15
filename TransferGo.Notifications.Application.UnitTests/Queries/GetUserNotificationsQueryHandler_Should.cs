using TransferGo.Notifications.Application.Queries.GetUserNotifications;
using TransferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;
using TransferGo.Notifications.Application.Repositories;
using TransferGo.Notifications.Domain.Entities;

namespace TransferGo.Notifications.UnitTests.Queries;

using AutoFixture.Xunit2;
using Moq;
using Tests.Common.AutoFixture.Attributes;
using Xunit;

public class GetUserNotificationsQueryHandler_Should
{
    [Theory, AutoTestData]
    public async Task Handle_NotificationExists_ShouldReturn(
        [Frozen] Mock<INotificationRepository> notificationRepositoryMock,
        GetUserNotificationsQuery query,
        List<Notification> notifications,
        GetUserNotificationsQueryHandler sut)
    {
        notificationRepositoryMock
            .Setup(x => x.GetUserNotifications(query.UserId))
            .ReturnsAsync(notifications);

        var result = await sut.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
    }
    
    [Theory, AutoTestData]
    public async Task Handle_NotificationsDoNotExist_ShouldReturnNull(
        [Frozen] Mock<INotificationRepository> notificationRepositoryMock,
        GetUserNotificationsQuery query,
        GetUserNotificationsQueryHandler sut)
    {
        notificationRepositoryMock
            .Setup(x => x.GetUserNotifications(query.UserId))
            .ReturnsAsync([]);

        var result = await sut.Handle(query, CancellationToken.None);

        Assert.Null(result);
    }
    
    [Theory, AutoTestData]
    public async Task Handle_FailedToFetchNotifications_ShouldThrowException(
        [Frozen] Mock<INotificationRepository> notificationRepositoryMock,
        GetUserNotificationsQuery query,
        GetUserNotificationsQueryHandler sut)
    {
        notificationRepositoryMock
            .Setup(x => x.GetUserNotifications(query.UserId))
            .ThrowsAsync(new Exception());

        await Assert.ThrowsAsync<Exception>(() => sut.Handle(query, CancellationToken.None));
    }
}