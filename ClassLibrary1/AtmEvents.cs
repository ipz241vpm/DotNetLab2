using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class AuthEventArgs : EventArgs
    {
        public string CardNumber { get; }
        public bool Success { get; }
        public string Message { get; }

        public AuthEventArgs(string cardNumber, bool success, string message)
        {
            CardNumber = cardNumber;
            Success = success;
            Message = message;
        }
    }

    public class BalanceEventArgs : EventArgs
    {
        public string CardNumber { get; }
        public decimal Balance { get; }

        public BalanceEventArgs(string cardNumber, decimal balance)
        {
            CardNumber = cardNumber;
            Balance = balance;
        }
    }

    public class OperationEventArgs : EventArgs
    {
        public string CardNumber { get; }
        public bool Success { get; }
        public string Message { get; }

        public OperationEventArgs(string cardNumber, bool success, string message)
        {
            CardNumber = cardNumber;
            Success = success;
            Message = message;
        }
    }
}
