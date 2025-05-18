using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Services;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace EventApiTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<MyAppContext> _mockContext;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockContext = new Mock<MyAppContext>();
            _userService = new UserService(_mockContext.Object);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnTrue_WhenUserIsCreated()
        {
            // Arrange
            var userRole = new Usersrole { Roleid = 1, Rolename = Constants.CustomerUserRole };
            var userData = new InitialUser
            {
                Email = "test@example.com",
                Username = "testuser",
                Passwordhash = "hashedpassword",
                Firstname = "Test",
                Lastname = "User"
            };

            _mockContext.Setup(c => c.Usersroles).ReturnsDbSet(new List<Usersrole> { userRole });
            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User> { new User { } });

            // Act
            var result = await _userService.CreateNewUser(userData);

            // Assert
            result.Should().BeTrue();
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateNewUser_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            // Arrange
            var userRole = new Usersrole { Roleid = 1, Rolename = Constants.CustomerUserRole };
            var existingUser = new User { Email = "test@example.com", Username = "testuser" };
            var userData = new InitialUser
            {
                Email = "test@example.com",
                Username = "testuser",
                Passwordhash = "hashedpassword",
                Firstname = "Test",
                Lastname = "User"
            };

            var mockUsers = new List<User>() { existingUser };
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockUsers);
            _mockContext.Setup(c => c.Usersroles).ReturnsDbSet(new List<Usersrole> { userRole });

            // Act
            var result = await _userService.CreateNewUser(userData);

            // Assert
            result.Should().BeFalse();
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task SetManagerStatusForUser_ShouldReturnTrue_WhenRoleIsUpdated()
        {
            // Arrange
            var userRole = new Usersrole { Roleid = 2, Rolename = Constants.ManagerUserRole };
            var dbUser = new User { Email = "test@example.com", Roleid = 1 };

            _mockContext.Setup(c => c.Usersroles).ReturnsDbSet(new List<Usersrole>() { userRole });
            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User>() { dbUser });

            // Act
            var result = await _userService.SetManagerStatusForUser("test@example.com");

            // Assert
            result.Should().BeTrue();
            dbUser.Roleid.Should().Be(userRole.Roleid);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SetManagerStatusForUser_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var userRole = new Usersrole { Roleid = 2, Rolename = Constants.ManagerUserRole };

            _mockContext.Setup(c => c.Usersroles).ReturnsDbSet(new List<Usersrole>() { userRole });

            // Act
            var result = await _userService.SetManagerStatusForUser("nonexistent@example.com");

            // Assert
            result.Should().BeFalse();
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
    }
}