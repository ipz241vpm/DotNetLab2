using System;
using System.Text;
using AtmWinForm;
using ClassLibrary1;

namespace AtmConsole
{
    class Program
    {
        static Bank bank = new Bank("PashaBank");
        static AutomatedTellerMachine atm = new AutomatedTellerMachine("ATM-001", "Main St.", 10000);
        static AtmService service;

        static string authenticatedCard = "";

        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            InitializeBank();
            service = new AtmService(bank, atm);

            service.AuthPerformed += (s, e) => Console.WriteLine($"[AUTH] {e.Message}");
            service.BalanceChecked += (s, e) => Console.WriteLine($"[BALANCE] {e.CardNumber}: {e.Balance:C}");
            service.WithdrawPerformed += (s, e) => Console.WriteLine($"[WITHDRAW] {e.Message}");
            service.DepositPerformed += (s, e) => Console.WriteLine($"[DEPOSIT] {e.Message}");
            service.TransferPerformed += (s, e) => Console.WriteLine($"[TRANSFER] {e.Message}");

            while (true)
            {
                Console.WriteLine("\nВахнюк Павло ІПЗ-24-1");
                Console.WriteLine("\n--- ПашаБанк ---");
                Console.WriteLine("1) Аутентифікація");
                Console.WriteLine("2) Показати баланс");
                Console.WriteLine("3) Зняти кошти");
                Console.WriteLine("4) Зарахувати кошти");
                Console.WriteLine("5) Переказ на іншу картку");
                Console.WriteLine("0) Вихід");
                Console.Write("Вибір: ");
                var k = Console.ReadLine();
                switch (k)
                {
                    case "1": DoAuth(); break;
                    case "2": DoBalance(); break;
                    case "3": DoWithdraw(); break;
                    case "4": DoDeposit(); break;
                    case "5": DoTransfer(); break;
                    case "0": return;
                    default: Console.WriteLine("Невідомий вибір"); break;
                }
            }
        }

        static void InitializeBank()
        {
            bank = new Bank(BankData.BankName);
            atm = BankData.InitialAtm;

            foreach (var acc in BankData.InitialAccounts)
                bank.AddAccount(acc);

            bank.AddAtm(atm);
        }


        static void DoAuth()
        {
            Console.Write("Введіть номер картки: ");
            var card = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Введіть PIN: ");
            var pin = ReadHidden();
            if (service.Authenticate(card, pin))
            {
                authenticatedCard = card;
                Console.WriteLine("Аутентифікація пройшла успішно");
            }
            else
            {
                authenticatedCard = "";
                Console.WriteLine("Аутентифікація неуспішна");
            }
        }

        static void DoBalance()
        {
            if (!CheckAuth()) return;
            service.CheckBalance(authenticatedCard);
        }

        static void DoWithdraw()
        {
            if (!CheckAuth()) return;
            Console.Write("Сума для зняття: ");
            if (decimal.TryParse(Console.ReadLine(), out var amt))
            {
                service.Withdraw(authenticatedCard, amt);
            }
            else Console.WriteLine("Некоректна сума");
        }

        static void DoDeposit()
        {
            if (!CheckAuth()) return;
            Console.Write("Сума для зарахування: ");
            if (decimal.TryParse(Console.ReadLine(), out var amt))
            {
                service.Deposit(authenticatedCard, amt);
            }
            else Console.WriteLine("Некоректна сума");
        }

        static void DoTransfer()
        {
            if (!CheckAuth()) return;
            Console.Write("Картка-одержувач: ");
            var to = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Сума: ");
            if (decimal.TryParse(Console.ReadLine(), out var amt))
            {
                service.Transfer(authenticatedCard, to, amt);
            }
            else Console.WriteLine("Некоректна сума");
        }

        static bool CheckAuth()
        {
            if (string.IsNullOrEmpty(authenticatedCard))
            {
                Console.WriteLine("Спочатку аутентифікуйтесь.");
                return false;
            }
            return true;
        }

        static string ReadHidden()
        {
            var pin = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
                {
                    pin = pin[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pin += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return pin;
        }
    }
}

