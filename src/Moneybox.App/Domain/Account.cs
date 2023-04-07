using Moneybox.App.Domain;
using System;

namespace Moneybox.App
{
    public class Account : IAccount
    {
        public const decimal PayInLimit = 4000m;

        private Guid Id { get; set; }

        public User User { get; set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        private decimal PaidIn { get; set; }

        public Account(User user, decimal balance)
        {
            User = user;
            Balance = balance;
            Withdrawn = 0;
            PaidIn = 0;
        }

        public void Transfer(Account to, decimal amount)
        {
            if (to.Equals(this))
            {
                throw new InvalidOperationException("Cannot transfer money to itself");
            }

            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            var paidIn = to.PaidIn + amount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            Balance = Balance - amount;
            Withdrawn = Withdrawn - amount;

            to.Balance = to.Balance + amount;
            to.PaidIn = to.PaidIn + amount;
        }


        public bool IsFundsLow(decimal limit)
        {
            return Balance < limit;
        }

        public bool ReachingPaidInLimit(decimal limit)
        {
            return PayInLimit - PaidIn < limit;
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
