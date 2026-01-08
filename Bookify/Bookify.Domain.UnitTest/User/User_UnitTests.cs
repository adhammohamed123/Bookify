using Bookify.Domain.Abstractions;
using Bookify.Domain.User;
using Bookify.Domain.User.Events;
using FluentAssertions;

namespace Bookify.Domain.UnitTest.User
{
    public class User_UnitTests
    {
        [Fact]
        public void Create_Should_Return_Correct_User()
        {
            //Arrage
            //Act
            var userResult= UserModel.CreateNewUser(Guid.NewGuid(),UserData.FirstName,UserData.LastName,UserData.Email);
            //Assert 
            userResult.IsSuccess.Should().BeTrue();
            userResult.Error.Should().Be(Error.NoError);
            userResult.Value.FirstName.Should().Be(UserData.FirstName);
            userResult.Value.LastName.Should().Be(UserData.LastName);
            userResult.Value.Email.Should().Be(UserData.Email);
        }

        [Fact]
        public void Create_Should_Raise_UserCreatedDomainEvent()
        {
            //Arrage
            var userId= Guid.NewGuid(); 
            //Act
            var userResult = UserModel.CreateNewUser(userId, UserData.FirstName, UserData.LastName, UserData.Email);
            var domainEvent = userResult.Value.GetDomainEvents.OfType<UserCreatedDomainEvent>().SingleOrDefault();
            //Assert 
            userResult.IsSuccess.Should().BeTrue();
            userResult.Error.Should().Be(Error.NoError);
            domainEvent.Should().NotBeNull();
            domainEvent.email.Should().BeEquivalentTo(UserData.Email.Value);
            domainEvent.userId.Should().Be(userId);
        }

        public class User_Email_UnitTest
        {
            [Fact]
            public void Create_Should_Return_Failure_when_Email_Value_Is_Null()
            {
                string emailValue= null; 
                var email = Email.Create(emailValue);
                // assert
                email.IsFaliure.Should().BeTrue();
                email.Error.Should().Be(Email.InValid);

            }
            [Fact]
            public void Create_Should_Return_Failure_when_Email_Value_Is_WhiteSpace()
            {
                string emailValue = ""; 
                var email = Email.Create(emailValue);
                // assert
                email.IsFaliure.Should().BeTrue();
                email.Error.Should().Be(Email.InValid);

            }
            [Fact]
            public void Create_Should_Return_Failure_when_Email_Value_Is_NotContainCharAT ()
            {
                string emailValue = "test.com"; 
                var email = Email.Create(emailValue);
                // assert
                email.IsFaliure.Should().BeTrue();
                email.Error.Should().Be(Email.InValid);

            }
            [Fact]
            public void Create_Should_Return_Success_when_AllPasses()
            {
                string emailValue = "test@test.com";
                var email = Email.Create(emailValue);
                // assert
                email.IsSuccess.Should().BeTrue();
                email.Error.Should().Be(Error.NoError);
                email.Value.Value.Should().BeEquivalentTo(emailValue);

            }
        }
    }
}
