using System;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Forms
{
    public class CardDetailsForm : Form
    {
        private TextBox txtCardNumber = null!;
        private TextBox txtCardHolderName = null!;
        private TextBox txtExpiryMonth = null!;
        private TextBox txtExpiryYear = null!;
        private TextBox txtCVV = null!;
        private Button btnConfirm = null!;
        private Button btnCancel = null!;
        private Label lblAlert = null!;

        public Credit? CardDetails { get; private set; }

        public CardDetailsForm(double amount)
        {
            InitializeComponent();
            SetupCardNumberMasking();
        }

        private void InitializeComponent()
        {
            this.Text = "Card Details - Tasty Eats";
            this.Size = new Size(500, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 248, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateControls();
            SetupEventHandlers();
        }

        private void CreateControls()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "💳 Enter Card Details",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 20),
                Size = new Size(400, 30)
            };

            // Card Number
            var lblCardNumber = new Label
            {
                Text = "Card Number:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 70),
                Size = new Size(150, 25)
            };

            txtCardNumber = new TextBox
            {
                Location = new Point(50, 100),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 14),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                MaxLength = 19, // 16 digits + 3 spaces
                TextAlign = HorizontalAlignment.Center,
                Text = "**** **** **** ****"
            };

            // Helpful hints
            var lblHint1 = new Label
            {
                Text = "💡 Enter your 16-digit card number",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(50, 135),
                Size = new Size(300, 15)
            };

            // Card Holder Name
            var lblCardHolderName = new Label
            {
                Text = "Cardholder Name:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 160),
                Size = new Size(180, 25)
            };

            txtCardHolderName = new TextBox
            {
                Location = new Point(50, 190),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                MaxLength = 50
            };

            var lblHint2 = new Label
            {
                Text = "💡 Enter the name exactly as shown on your card",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(50, 225),
                Size = new Size(300, 15)
            };

            // Expiry Date
            var lblExpiry = new Label
            {
                Text = "Expiry Date:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 250),
                Size = new Size(120, 25)
            };

            var lblMonth = new Label
            {
                Text = "MM:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 280),
                Size = new Size(35, 20)
            };

            txtExpiryMonth = new TextBox
            {
                Location = new Point(85, 280),
                Size = new Size(50, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                MaxLength = 2,
                TextAlign = HorizontalAlignment.Center
            };

            var lblYear = new Label
            {
                Text = "YY:",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(145, 280),
                Size = new Size(35, 20)
            };

            txtExpiryYear = new TextBox
            {
                Location = new Point(180, 280),
                Size = new Size(50, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                MaxLength = 2,
                TextAlign = HorizontalAlignment.Center
            };

            var lblHint3 = new Label
            {
                Text = "💡 MM/YY format (e.g., 12/25)",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(50, 315),
                Size = new Size(180, 15)
            };

            // CVV
            var lblCVV = new Label
            {
                Text = "CVV:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(250, 250),
                Size = new Size(50, 25)
            };

            txtCVV = new TextBox
            {
                Location = new Point(250, 280),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                MaxLength = 4,
                TextAlign = HorizontalAlignment.Center,
                UseSystemPasswordChar = true
            };

            var lblHint4 = new Label
            {
                Text = "💡 3-4 digit security code on back of card",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(250, 330),
                Size = new Size(220, 30)
            };

            // Buttons
            btnConfirm = new Button
            {
                Text = "Confirm Payment",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                Location = new Point(50, 380),
                Size = new Size(150, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                Location = new Point(220, 380),
                Size = new Size(130, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Alert label
            lblAlert = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 440),
                Size = new Size(400, 30)
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblCardNumber, txtCardNumber, lblCardHolderName, txtCardHolderName,
                lblExpiry, lblMonth, txtExpiryMonth, lblYear, txtExpiryYear,
                lblCVV, txtCVV, btnConfirm, btnCancel, lblAlert, lblHint1, lblHint2, lblHint3, lblHint4
            });

            foreach (Control control in this.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = Color.Transparent;
                }
            }
        }

        private void SetupEventHandlers()
        {
            btnConfirm.Click += BtnConfirm_Click;
            btnCancel.Click += BtnCancel_Click;
            
            // Add input validation
            txtExpiryMonth.TextChanged += TxtExpiryMonth_TextChanged;
            txtExpiryYear.TextChanged += TxtExpiryYear_TextChanged;
            txtCVV.TextChanged += TxtCVV_TextChanged;
            
            // Clear placeholder text on focus
            txtCardNumber.GotFocus += (sender, e) =>
            {
                if (txtCardNumber.Text == "**** **** **** ****")
                {
                    txtCardNumber.Text = "";
                }
            };
        }

        private void SetupCardNumberMasking()
        {
            txtCardNumber.TextChanged += (sender, e) =>
            {
                var text = txtCardNumber.Text.Replace(" ", "");
                if (text.Length > 16)
                {
                    text = text.Substring(0, 16);
                }

                var masked = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (i > 0 && i % 4 == 0) 
                    {
                        masked += " ";
                    }
                    masked += text[i];
                }

                // Only update if the text actually changed to avoid infinite loop
                if (txtCardNumber.Text != masked)
                {
                    txtCardNumber.Text = masked;
                    txtCardNumber.SelectionStart = txtCardNumber.Text.Length;
                }
            };
        }

        private void TxtExpiryMonth_TextChanged(object sender, EventArgs e)
        {
            var text = txtExpiryMonth.Text;
            if (text.Length == 2)
            {
                var month = int.Parse(text);
                if (month < 1 || month > 12)
                {
                    ShowAlert("Month must be between 01 and 12", true);
                    txtExpiryMonth.Text = "";
                    return;
                }
                txtExpiryYear.Focus();
            }
        }

        private void TxtExpiryYear_TextChanged(object sender, EventArgs e)
        {
            var text = txtExpiryYear.Text;
            if (text.Length == 2)
            {
                var year = int.Parse(text);
                var currentYear = DateTime.Now.Year % 100;
                if (year < currentYear || year > currentYear + 20)
                {
                    ShowAlert($"Year must be between {currentYear:00} and {currentYear + 20:00}", true);
                    txtExpiryYear.Text = "";
                    return;
                }
                txtCVV.Focus();
            }
        }
        // Ensure CVV is numeric
        private void TxtCVV_TextChanged(object sender, EventArgs e)
        {
            var text = txtCVV.Text;
            if (!int.TryParse(text, out _) && text.Length > 0)
            {
                txtCVV.Text = text.Substring(0, text.Length - 1); // 
                txtCVV.SelectionStart = txtCVV.Text.Length;
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                try
                {
                    // Create Credit card object
                    var cardNumber = txtCardNumber.Text.Replace(" ", "");
                    var expiryMonth = int.Parse(txtExpiryMonth.Text);
                    var expiryYear = 2000 + int.Parse(txtExpiryYear.Text);
                    var expiryDate = new DateTime(expiryYear, expiryMonth, 1).AddMonths(1).AddDays(-1);
                    var cvv = int.Parse(txtCVV.Text);

                    CardDetails = new Credit
                    {
                        CardNumber = cardNumber,
                        CardHolderName = txtCardHolderName.Text.Trim(),
                        ExpiryDate = expiryDate,
                        CVV = cvv
                    };

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    ShowAlert($"Error processing card details: {ex.Message}", true);
                }
            }
        }

        private bool ValidateInputs()
        {
            // Clear previous alerts
            lblAlert.Text = "";

            // Validate card number
            var cardNumber = txtCardNumber.Text.Replace(" ", "");
            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
            {
                ShowAlert("Please enter a valid 16-digit card number", true);
                txtCardNumber.Focus();
                return false;
            }

            // Validate cardholder name
            if (string.IsNullOrWhiteSpace(txtCardHolderName.Text))
            {
                ShowAlert("Please enter the cardholder name", true);
                txtCardHolderName.Focus();
                return false;
            }

            // Validate expiry month
            if (txtExpiryMonth.Text.Length != 2 || !int.TryParse(txtExpiryMonth.Text, out var month) || month < 1 || month > 12)
            {
                ShowAlert("Please enter a valid expiry month (01-12)", true);
                txtExpiryMonth.Focus();
                return false;
            }

            // Validate expiry year
            if (txtExpiryYear.Text.Length != 2 || !int.TryParse(txtExpiryYear.Text, out var year))
            {
                ShowAlert("Please enter a valid expiry year (YY)", true);
                txtExpiryYear.Focus();
                return false;
            }

            var currentYear = DateTime.Now.Year % 100;
            if (year < currentYear || year > currentYear + 20)
            {
                ShowAlert($"Expiry year must be between {currentYear:00} and {currentYear + 20:00}", true);
                txtExpiryYear.Focus();
                return false;
            }

            // Validate CVV
            if (txtCVV.Text.Length < 3 || !int.TryParse(txtCVV.Text, out _))
            {
                ShowAlert("Please enter a valid CVV (3 digits)", true);
                txtCVV.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowAlert(string message, bool isError)
        {
            lblAlert.Text = message;
            lblAlert.BackColor = isError ? Color.FromArgb(255, 192, 192) : Color.FromArgb(192, 255, 192);
            lblAlert.ForeColor = isError ? Color.Black : Color.Black;
        }
    }
}