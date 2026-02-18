using ClassLibrary1;
using System.Collections.Generic;

namespace AtmWinForm
{
    public static class BankData
    {
        public static string BankName => "PashaBank";

        public static List<Account> InitialAccounts => new()
        {
            new Account("1111-2222-3333-4444", "Павло Вахнюк", "1234", 5000),
            new Account("2222-3333-4444-5555", "Петро Петренко", "0000", 1500)
        };

        public static AutomatedTellerMachine InitialAtm =>
            new AutomatedTellerMachine("ATM-001", "Main St. 1", 10000m);
    }
}
