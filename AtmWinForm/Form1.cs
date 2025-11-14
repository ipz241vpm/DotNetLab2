using ClassLibrary1;

namespace AtmWinForm
{
    public partial class Form1 : Form
    {
        private readonly Bank _bank;
        private readonly AutomatedTellerMachine _atm;
        private readonly AtmService _service;

        TextBox txtCard, txtPin, txtAmount, txtToCard;
        Button btnAuth, btnBalance, btnWithdraw, btnDeposit, btnTransfer;
        Label lblStatus;

        private string _authenticatedCard = "";
        public Form1()
        {
            InitializeComponent();
            Text = "ATM - Вікно банкомату";
            Size = new Size(500, 320);

            _bank = new Bank("PashaBank");
            _atm = new AutomatedTellerMachine("ATM-001", "Main St. 1", 10000m);

            _bank.AddAccount(new Account("1111-2222-3333-4444", "Павло Вахнюк", "1234", 5000));
            _bank.AddAccount(new Account("2222-3333-4444-5555", "Петро Петренко", "0000", 1500));
            _bank.AddAtm(_atm);

            _service = new AtmService(_bank, _atm);

            _service.AuthPerformed += (s, e) => MessageBox.Show(e.Message, "Аутентифікація");
            _service.BalanceChecked += (s, e) => MessageBox.Show($"Баланс: {e.Balance:C}", "Баланс");
            _service.WithdrawPerformed += (s, e) => MessageBox.Show(e.Message, "Зняття");
            _service.DepositPerformed += (s, e) => MessageBox.Show(e.Message, "Зарахування");
            _service.TransferPerformed += (s, e) => MessageBox.Show(e.Message, "Переказ");

            InitializeControls();
        }



        private void InitializeControls()
        {
            Label lblBottomRight = new Label
            {
                Text = "Вахнюк Павло ІПЗ-24-1",
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 200, this.ClientSize.Height - 30), 
                ForeColor = Color.Black
            };

            lblBottomRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            Controls.Add(lblBottomRight);
            var lblCard = new Label { Text = "Картка:", Location = new Point(20, 20), AutoSize = true };
            txtCard = new TextBox { Location = new Point(120, 18), Width = 250 };

            var lblPin = new Label { Text = "PIN:", Location = new Point(20, 60), AutoSize = true };
            txtPin = new TextBox { Location = new Point(120, 58), Width = 100, PasswordChar = '*' };

            Font buttonFont = new Font("Microsoft Sans Serif", 8);
            Color buttonBack = Color.White;
            Color buttonFore = Color.Black;

            btnAuth = new Button { Text = "Аутентифікувати", Location = new Point(240, 55), Width = 130 };
            btnAuth.Click += BtnAuth_Click;
            btnAuth.BackColor = buttonBack;
            btnAuth.ForeColor = buttonFore;
            btnAuth.Font = buttonFont;

            btnBalance = new Button { Text = "Показати баланс", Location = new Point(20, 100), Width = 140 };
            btnBalance.Click += (s, e) => {
                if (!CheckAuth()) return;
                _service.CheckBalance(_authenticatedCard);
            };
            btnBalance.BackColor = buttonBack;
            btnBalance.ForeColor = buttonFore;
            btnBalance.Font = buttonFont;

            var lblAmount = new Label { Text = "Сума:", Location = new Point(180, 100), AutoSize = true };
            txtAmount = new TextBox { Location = new Point(240, 98), Width = 120 };

            btnWithdraw = new Button { Text = "Зняти", Location = new Point(20, 140), Width = 140 };
            btnWithdraw.Click += (s, e) => {
                if (!CheckAuth()) return;
                if (decimal.TryParse(txtAmount.Text, out var amt))
                    _service.Withdraw(_authenticatedCard, amt);
                else MessageBox.Show("Некоректна сума");
            };
            btnWithdraw.BackColor = buttonBack;
            btnWithdraw.ForeColor = buttonFore;
            btnWithdraw.Font = buttonFont;

            btnDeposit = new Button { Text = "Зарахувати", Location = new Point(180, 140), Width = 140 };
            btnDeposit.Click += (s, e) => {
                if (!CheckAuth()) return;
                if (decimal.TryParse(txtAmount.Text, out var amt))
                    _service.Deposit(_authenticatedCard, amt);
                else MessageBox.Show("Некоректна сума");
            };
            btnDeposit.BackColor = buttonBack;
            btnDeposit.ForeColor = buttonFore;
            btnDeposit.Font = buttonFont;

            var lblToCard = new Label { Text = "На картку (для переказу):", Location = new Point(20, 190), AutoSize = true };
            txtToCard = new TextBox { Location = new Point(220, 188), Width = 180 };

            btnTransfer = new Button { Text = "Переказ", Location = new Point(20, 220), Width = 180 };
            btnTransfer.Click += (s, e) => {
                if (!CheckAuth()) return;
                if (decimal.TryParse(txtAmount.Text, out var amt))
                    _service.Transfer(_authenticatedCard, txtToCard.Text.Trim(), amt);
                else MessageBox.Show("Некоректна сума");
            };
            btnTransfer.BackColor = buttonBack;
            btnTransfer.ForeColor = buttonFore;
            btnTransfer.Font = buttonFont;

            lblStatus = new Label { Text = "Не автентифіковано", Location = new Point(20, 260), AutoSize = true, ForeColor = Color.Red };

            Controls.AddRange(new Control[] {
    lblCard, txtCard, lblPin, txtPin, btnAuth,
    btnBalance, lblAmount, txtAmount,
    btnWithdraw, btnDeposit, lblToCard, txtToCard, btnTransfer, lblStatus
});

        }

        private void BtnAuth_Click(object? sender, EventArgs e)
        {
            var card = txtCard.Text.Trim();
            var pin = txtPin.Text.Trim();
            var ok = _service.Authenticate(card, pin);
            if (ok)
            {
                _authenticatedCard = card;
                lblStatus.Text = $"Автентифіковано: {card}";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                _authenticatedCard = "";
                lblStatus.Text = "Не автентифіковано";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private bool CheckAuth()
        {
            if (string.IsNullOrEmpty(_authenticatedCard))
            {
                MessageBox.Show("Спочатку аутентифікуйтесь");
                return false;
            }
            return true;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
           
    }
}
