using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AtmService
    {
        private readonly Bank _bank;
        private readonly AutomatedTellerMachine _atm;

        public AtmService(Bank bank, AutomatedTellerMachine atm)
        {
            _bank = bank;
            _atm = atm;
        }

        public event EventHandler<AuthEventArgs>? AuthPerformed;
        public event EventHandler<BalanceEventArgs>? BalanceChecked;
        public event EventHandler<OperationEventArgs>? WithdrawPerformed;
        public event EventHandler<OperationEventArgs>? DepositPerformed;
        public event EventHandler<OperationEventArgs>? TransferPerformed;

        public bool Authenticate(string cardNumber, string pin)
        {
            var acc = _bank.FindByCard(cardNumber);
            bool ok = acc != null && acc.PinCode == pin;
            AuthPerformed?.Invoke(this, new AuthEventArgs(cardNumber, ok, ok ? "Успішна аутентифікація" : "Невірний PIN"));
            return ok;
        }

        public void CheckBalance(string cardNumber)
        {
            var acc = _bank.FindByCard(cardNumber);
            BalanceChecked?.Invoke(this, new BalanceEventArgs(cardNumber, acc?.Balance ?? 0));
        }

        public void Withdraw(string cardNumber, decimal amount)
        {
            var acc = _bank.FindByCard(cardNumber);
            if (acc == null)
            {
                WithdrawPerformed?.Invoke(this, new OperationEventArgs(cardNumber, false, "Рахунок не знайдено"));
                return;
            }
            if (amount <= 0 || !acc.Withdraw(amount) || !_atm.Dispense(amount))
            {
                acc.Deposit(amount);
                WithdrawPerformed?.Invoke(this, new OperationEventArgs(cardNumber, false, "Помилка операції"));
                return;
            }
            WithdrawPerformed?.Invoke(this, new OperationEventArgs(cardNumber, true, $"Видача {amount:C} успішна"));
        }

        public void Deposit(string cardNumber, decimal amount)
        {
            var acc = _bank.FindByCard(cardNumber);
            if (acc == null || amount <= 0)
            {
                DepositPerformed?.Invoke(this, new OperationEventArgs(cardNumber, false, "Помилка"));
                return;
            }
            acc.Deposit(amount);
            _atm.AddCash(amount);
            DepositPerformed?.Invoke(this, new OperationEventArgs(cardNumber, true, $"Зараховано {amount:C}"));
        }

        public void Transfer(string fromCard, string toCard, decimal amount)
        {
            var accFrom = _bank.FindByCard(fromCard);
            var accTo = _bank.FindByCard(toCard);

            if (accFrom == null || accTo == null || amount <= 0 || !accFrom.Withdraw(amount))
            {
                TransferPerformed?.Invoke(this, new OperationEventArgs(fromCard, false, "Помилка переказу"));
                return;
            }
            accTo.Deposit(amount);
            TransferPerformed?.Invoke(this, new OperationEventArgs(fromCard, true, $"Переказ {amount:C} на {toCard} успішний"));
        }
    }
}
