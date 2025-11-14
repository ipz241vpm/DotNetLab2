using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Account
    {
        public string CardNumber { get; init; }
        public string OwnerFullName { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; private set; }

        public Account(string cardNumber, string ownerFullName, string pinCode, decimal initialBalance = 0)
        {
            CardNumber = cardNumber;
            OwnerFullName = ownerFullName;
            PinCode = pinCode;
            Balance = initialBalance;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0 || Balance < amount) return false;
            Balance -= amount;
            return true;
        }

        public bool Deposit(decimal amount)
        {
            if (amount <= 0) return false;
            Balance += amount;
            return true;
        }
    }
}
