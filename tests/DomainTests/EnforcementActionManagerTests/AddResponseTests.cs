﻿using AirWeb.Domain.EnforcementEntities.EnforcementActions;
using AirWeb.Domain.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DomainTests.EnforcementActionManagerTests
{
    public class AddResponseTests
    {
        private EnforcementActionManager _manager;

        [SetUp]
        public void SetUp() =>
            _manager = new EnforcementActionManager(Substitute.For<ILogger<EnforcementActionManager>>());

        [Test]
        public void AddResponse_ValidResponse_SetsResponseReceivedAndComment()
        {
            // Arrange
            var enforcementAction = new MockResponseRequestedAction { ResponseRequested = true };
            var responseDate = DateOnly.FromDateTime(DateTime.Today);
            const string comment = "Test comment";
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };

            // Act
            _manager.AddResponse(enforcementAction, responseDate, comment, user);

            // Assert
            enforcementAction.ResponseReceived.Should().Be(responseDate);
            enforcementAction.ResponseComment.Should().Be(comment);
        }

        [Test]
        public void AddResponse_NotResponseRequested_ThrowsInvalidOperationException()
        {
            // Arrange
            var enforcementAction = new MockNonResponseRequestedAction();
            var responseDate = DateOnly.FromDateTime(DateTime.Today);
            const string comment = "Test comment";
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };

            // Act
            Action act = () => _manager.AddResponse(enforcementAction, responseDate, comment, user);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        private class MockResponseRequestedAction : EnforcementAction, IResponseRequested
        {
            public bool ResponseRequested { get; set; }
            public DateOnly? ResponseReceived { get; set; }
            public string? ResponseComment { get; set; }
        }

        private class MockNonResponseRequestedAction : EnforcementAction;
    }
}
