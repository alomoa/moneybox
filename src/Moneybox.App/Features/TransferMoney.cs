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
            var from = accountRepository.GetAccountById(fromAccountId);
            var to = accountRepository.GetAccountById(toAccountId);

            try
            {
                from.Transfer(to, amount);
            }
            catch{
                throw;
            }
            
            if (to.ReachingPaidInLimit(500))
            {
                notificationService.NotifyApproachingPayInLimit(to.User.Email);
            }

            if (from.IsFundsLow(amount))
            {
                notificationService.NotifyFundsLow(from.User.Email);
            }

            accountRepository.Update(from);
            accountRepository.Update(to);
        }
    }
}
