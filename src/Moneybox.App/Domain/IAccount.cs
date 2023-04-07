namespace Moneybox.App.Domain
{
    public interface IAccount
    {

        void Withdraw(decimal amount);
        void Transfer(Account to, decimal amount);
    }
}
