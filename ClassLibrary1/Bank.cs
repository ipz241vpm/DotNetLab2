using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Bank
    {
        public string Name { get; set; }
        public List<Account> Accounts { get; } = new();
        public List<AutomatedTellerMachine> Atms { get; } = new();

        public Bank(string name) => Name = name;

        public Account? FindByCard(string cardNumber) => Accounts.FirstOrDefault(a => a.CardNumber == cardNumber);

        public void AddAccount(Account acc) => Accounts.Add(acc);
        public void AddAtm(AutomatedTellerMachine atm) => Atms.Add(atm);
    }
}
