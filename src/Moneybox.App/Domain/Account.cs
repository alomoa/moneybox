using System;

namespace Moneybox.App
{
    public class Account
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


        public bool HasMoreThan(decimal amount)
        {
            return Balance > amount;
        }

        public bool IsFundsLow()
        {
            return Balance < 500;
        }

        public bool ReachingPaidInLimit()
        {
            return PayInLimit < 500;
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

        public bool ReachingBalanceLimit()
        {
            return Balance < 500;
        }
    }

}
