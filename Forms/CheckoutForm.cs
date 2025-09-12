using System;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Database;

namespace OnlineOrderingSystem.Forms
{
    public class CheckoutForm : Form
    {
        private Cart cart;
        private int customerId; // Add customer ID parameter
        private ComboBox cmbPaymentMethod = null!;
        private TextBox txtDeliveryAddress = null!;
        private TextBox txtSpecialInstructions = null!;
        private CheckBox chkDelivery = null!;
        private Label lblSubtotal = null!;
        private Label lblTax = null!;
        private Label lblDeliveryFee = null!;
        private Label lblTotal = null!;
        private Button btnPlaceOrder = null!;
        private Button btnCancel = null!;
        private Label lblAlert = null!;

        // Properties
        public bool OrderPlaced { get; private set; } = false;
        public event Action<Order>? OrderPlacedEvent;

        public CheckoutForm(Cart cart, int customerId = 1)
        {
            this.cart = cart;
            this.customerId = customerId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Tasty Eats - Checkout";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 248, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateControls();
            SetupEventHandlers();
            CalculateTotals();
        }

        private void CreateControls()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "💳 Checkout",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(700, 40)
            };

            // Order Summary
            var lblOrderSummary = new Label
            {
                Text = "Order Summary:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                BackColor = Color.Transparent,
                Location = new Point(50, 90),
                Size = new Size(200, 25)
            };

            var lstOrderItems = new ListBox
            {
                Location = new Point(50, 120),
                Size = new Size(400, 150),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Segoe UI", 10)
            };

            // Populate order items
            foreach (var cartItem in cart.Items)
            {
                lstOrderItems.Items.Add($"{cartItem.Item.Name} - £{cartItem.Item.Price:F2} x {cartItem.Quantity}");
            }

            // Delivery Options
            var lblDelivery = new Label
            {
                Text = "Delivery Options:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 290),
                Size = new Size(200, 25)
            };

            chkDelivery = new CheckBox
            {
                Text = "Delivery (45-60 minutes)",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 320),
                Size = new Size(250, 25),
                Checked = true
            };

            var lblAddress = new Label
            {
                Text = "Delivery Address:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 360),
                Size = new Size(150, 25)
            };

            txtDeliveryAddress = new TextBox
            {
                Location = new Point(50, 390),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                Text = "123 Main Street, London, SW1A 1AA"
            };

            var lblInstructions = new Label
            {
                Text = "Special Instructions:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(50, 430),
                Size = new Size(200, 25)
            };

            txtSpecialInstructions = new TextBox
            {
                Location = new Point(50, 460),
                Size = new Size(400, 60),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Payment Method
            var lblPayment = new Label
            {
                Text = "Payment Method:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 540),
                Size = new Size(200, 25)
            };

            cmbPaymentMethod = new ComboBox
            {
                Location = new Point(50, 570),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbPaymentMethod.Items.AddRange(new object[] { "Cash on Delivery", "Credit Card", "Debit Card", "PayPal" });
            cmbPaymentMethod.SelectedIndex = 0;

            // Cost Breakdown
            var lblCostBreakdown = new Label
            {
                Text = "Cost Breakdown:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(500, 90),
                Size = new Size(200, 25)
            };

            lblSubtotal = new Label
            {
                Text = "Subtotal: £0.00",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(500, 130),
                Size = new Size(200, 25)
            };

            lblTax = new Label
            {
                Text = "VAT (20%): £0.00",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(500, 160),
                Size = new Size(200, 25)
            };

            lblDeliveryFee = new Label
            {
                Text = "Delivery Fee: £0.00",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(500, 190),
                Size = new Size(200, 25)
            };

            lblTotal = new Label
            {
                Text = "Total: £0.00",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                Location = new Point(500, 230),
                Size = new Size(250, 30)
            };

            // Buttons
            btnPlaceOrder = new Button
            {
                Text = "Place Order",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                Location = new Point(500, 570),
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
                Location = new Point(670, 570),
                Size = new Size(100, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Alert label
            lblAlert = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 630),
                Size = new Size(700, 25)
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblOrderSummary, lstOrderItems, lblDelivery, chkDelivery,
                lblAddress, txtDeliveryAddress, lblInstructions, txtSpecialInstructions,
                lblPayment, cmbPaymentMethod, lblCostBreakdown, lblSubtotal, lblTax,
                lblDeliveryFee, lblTotal, btnPlaceOrder, btnCancel, lblAlert
            });

            // Set all labels to transparent background
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
            btnPlaceOrder.Click += BtnPlaceOrder_Click;
            btnCancel.Click += BtnCancel_Click;
            chkDelivery.CheckedChanged += ChkDelivery_CheckedChanged;
        }

        private void CalculateTotals()
        {
            var subtotal = cart.CalculateTotal();
            var tax = subtotal * 0.20; // 20% VAT
            var deliveryFee = chkDelivery.Checked ? 2.99 : 0.0;
            var total = subtotal + tax + deliveryFee;

            lblSubtotal.Text = $"Subtotal: £{subtotal:F2}";
            lblTax.Text = $"VAT (20%): £{tax:F2}";
            lblDeliveryFee.Text = $"Delivery Fee: £{deliveryFee:F2}";
            lblTotal.Text = $"Total: £{total:F2}";
        }

        private void ChkDelivery_CheckedChanged(object sender, EventArgs e)
        {
            CalculateTotals();
            txtDeliveryAddress.Enabled = chkDelivery.Checked;
        }

        private void BtnPlaceOrder_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (chkDelivery.Checked && string.IsNullOrWhiteSpace(txtDeliveryAddress.Text))
            {
                ShowAlert("Please enter a delivery address", true);
                return;
            }

            if (cmbPaymentMethod.SelectedItem == null)
            {
                ShowAlert("Please select a payment method", true);
                return;
            }

            // Process the order
            try
            {
                // Create order
                var order = new Order();
                order.CreateOrder(customerId, cart); // Use customerId
                order.DeliveryAddress = txtDeliveryAddress.Text;
                order.DeliveryInstructions = txtSpecialInstructions.Text;
                order.IsDelivery = chkDelivery.Checked;
                order.PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty;
                order.DeliveryFee = chkDelivery.Checked ? 2.99 : 0.0;

                // Process payment
                var paymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? string.Empty;
                var total = double.Parse(lblTotal.Text.Split('£')[1]);

                // Create payment object
                Payment? payment = null;
                
                // Handle credit/debit card payment method
                if (paymentMethod.ToLower().Contains("card"))
                {
                    var cardForm = new CardDetailsForm(total);
                    if (cardForm.ShowDialog() == DialogResult.OK)
                    {
                        payment = cardForm.CardDetails;
                        if (payment != null)
                        {
                            payment.Amount = (decimal)total;
                        }
                    }
                    else
                    {
                        // User cancelled card details entry
                        ShowAlert("Payment cancelled. Please try again.", true);
                        return;
                    }
                }
                else
                {
                    // For other payment methods, create payment directly
                    payment = CreatePayment(paymentMethod, total);
                }

                if (payment == null)
                {
                    ShowAlert("Failed to create payment. Please try again.", true);
                    return;
                }

                // Save order and payment to database
                var orderDataAccess = new OrderDataAccess();
                var savedOrder = orderDataAccess.CreateOrderWithPayment(order, payment);

                if (savedOrder != null)
                {
                    OrderPlaced = true;
                    
                    // Fire the OrderPlacedEvent
                    OrderPlacedEvent?.Invoke(savedOrder);
                    
                    ShowAlert("Order placed successfully! Your order is being prepared.", false);
                    
                    // Show order confirmation
                    var confirmationMessage = $"Order #{savedOrder.OrderID} placed successfully!\n\n" +
                                           $"Total: £{total:F2}\n" +
                                           $"Payment Method: {paymentMethod}\n" +
                                           $"Estimated Delivery: {savedOrder.EstimatedDeliveryTime:HH:mm}\n\n" +
                                           "Thank you for choosing Tasty Eats!";
                    
                    MessageBox.Show(confirmationMessage, "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    ShowAlert("Failed to save order to database. Please try again.", true);
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error placing order: {ex.Message}", true);
            }
        }

        private Payment CreatePayment(string paymentMethod, double amount)
        {
            var decimalAmount = (decimal)amount;
            switch (paymentMethod.ToLower())
            {
                case "cash on delivery":
                    return new Cash { AmountTendered = decimalAmount + 10, TotalAmount = decimalAmount, Amount = decimalAmount };
                case "paypal":
                    var check = new Check { ChequeNumber = "PAYPAL123", BankName = "PayPal", Amount = decimalAmount };
                    return check;
                default:
                    throw new ArgumentException($"Unsupported payment method: {paymentMethod}");
            }
        }

        private bool ProcessPayment(Payment payment)
        {
            try
            {
                payment.ProcessPayment();
                return payment.PaymentStatus == "Completed";
            }
            catch
            {
                return false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
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