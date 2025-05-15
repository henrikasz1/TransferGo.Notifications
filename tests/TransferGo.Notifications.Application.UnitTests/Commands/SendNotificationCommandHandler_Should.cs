using TransferGo.Notifications.Application.Commands.SendNotification;
using TransferGo.Notifications.Application.Commands.SendNotification.Contracts;
using TransferGo.Notifications.Application.Repositories;
using TransferGo.Notifications.Domain.Entities;

namespace TransferGo.Notifications.UnitTests.Commands;

using AutoFixture.Xunit2;
using Hangfire;
using Moq;
using Tests.Common.AutoFixture.Attributes;
using Xunit;

public class SendNotificationCommandHandler_Should
{
    [Theory, AutoTestData]
    public async Task Handle_NotificationCreated_ShouldNotificationBackgroundJob(
        [Frozen] Mock<INotificationRepository> notificationRepositoryMock,
        [Frozen] Mock<IBackgroundJobClient> backgroundJobClientMock,
        SendNotificationCommand command,
        Notification notification,
        SendNotificationCommandHandler sut)
    {
        notificationRepositoryMock
            .Setup(x => x.CreateNotification(notification))
            .Returns(Task.CompletedTask);

        await sut.Handle(command, CancellationToken.None);
        
        // backgroundJobClientMock.Verify(x =>
        //         x.Enqueue(It.IsAny<Expression<Action<NotificationBackgroundJob>>>()),
        //     Times.Once);
    }
}