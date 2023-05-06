using Moneybox.App.Domain;
using System;

namespace Moneybox.App
{
    public class Account { 
        public const decimal PayInLimit = 4000m;

        private Guid Id { get; set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        public Account(User user, decimal balance)
        {
            User = user;
            Balance = balance;
            Withdrawn = 0;
            PaidIn = 0;
        }

        public bool IsFundsLow(decimal limit)
        {
            return Balance < limit;
        }

        public bool ReachingPaidInLimit(decimal limit)
        {
            return PayInLimit - PaidIn < limit;
        }

        public void TakeOut(decimal amount)
        {
            Balance -= amount;
        }

        public void Pay(decimal amount)
        {
            Balance += amount;
            PaidIn += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds to make withdrawal");
            }

            Balance = Balance - amount;
            Withdrawn = Withdrawn + amount;
        }

        public bool ReachingBalanceLimit(decimal limit)
        {
            return Balance < limit;
        }
    }
}
