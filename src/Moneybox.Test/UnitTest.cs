using Moq;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moneybox.App;
using System.ComponentModel.DataAnnotations;

namespace Moneybox.Test
{
    public class Tests
    {
        Mock<IAccountRepository> accountRepositoryMock = new Mock<IAccountRepository>();
        Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
        WithdrawMoney withdrawMoney;


        [SetUp]
        public void Setup()
        {
            withdrawMoney = new WithdrawMoney(accountRepositoryMock.Object, notificationServiceMock.Object);
        }

        [Test]
        public void ShouldWithdrawMoney()
        {
            // Arrange
            Account account = new Account(new User(), 1000);

            accountRepositoryMock.Setup(m => m.GetAccountById(It.IsAny<Guid>()))
                .Returns(account);

            // Act
            withdrawMoney.Execute(Guid.NewGuid(), 200);

            // Assert
            Assert.That(account.Balance, Is.EqualTo(800));
            Assert.That(account.Withdrawn, Is.EqualTo(200));

        }

        [Test]
        public void ShouldNotWithdrawMoneyIfAmountExceedsBalance()
        {
            // Arrange
            User user = new User
            {
                Email = "email@email.com"
            };

            Account account = new Account(user, 200);

            accountRepositoryMock.Setup(m => m.GetAccountById(It.IsAny<Guid>()))
                .Returns(account);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => withdrawMoney.Execute(Guid.NewGuid(), 300));
            Assert.DoesNotThrow(() => withdrawMoney.Execute(Guid.NewGuid(), 100));
        }

        [Test]
        public void ShouldNotifyWhenBalanceLandsBelow500()
        {
            // Arrange
            User user = new User
            {
                Email = "email@email.com"
            };

            Account account = new Account(user, 1000);

            accountRepositoryMock.Setup(m => m.GetAccountById(It.IsAny<Guid>()))
                .Returns(account);

            // Act
            withdrawMoney.Execute(Guid.NewGuid(), 600);

            // Assert
            notificationServiceMock.Verify(m => m.NotifyFundsLow(user.Email), Times.Once);
        }

        [Test]
        public void AccountTransfersMoneyToAnotherAccount()
        {
            //Arrange
            var account1 = new Account(new User(), 5000);
            var account2 = new Account(new User(), 0);

            //Act
            account1.Transfer(account2, 1000);

            //Assert
            Assert.That(account2.Balance, Is.EqualTo(1000));
        }

        [Test]
        public void DoesNotTransferMoneyToItself()
        {
            //Arrange
            var account = new Account(new User(), 1000);

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => account.Transfer(account, 1000));
        }

    }
}