namespace ClassLibrary1
{
    public class AutomatedTellerMachine
    {
        public string Id { get; init; }
        public string Address { get; set; }
        public decimal CashInside { get; private set; }

        public AutomatedTellerMachine(string id, string address, decimal initialCash)
        {
            Id = id;
            Address = address;
            CashInside = initialCash;
        }

        public bool CanDispense(decimal amount) => amount > 0 && CashInside >= amount;

        public bool Dispense(decimal amount)
        {
            if (!CanDispense(amount)) return false;
            CashInside -= amount;
            return true;
        }

        public void AddCash(decimal amount)
        {
            if (amount > 0) CashInside += amount;
        }
    }
}
