using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;
        private INotificationService notificationService;

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.notificationService = notificationService;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var transferFrom = accountRepository.GetAccountById(fromAccountId);
            var transferTo = accountRepository.GetAccountById(toAccountId);

            if (transferTo.Equals(transferFrom))
            {
                throw new InvalidOperationException("Cannot transfer money to itself");
            }

            if (transferFrom.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            var paidIn = transferTo.PaidIn + amount;
            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            transferFrom.TakeOut(amount);
            transferTo.Pay(amount);
        }
    }
}
