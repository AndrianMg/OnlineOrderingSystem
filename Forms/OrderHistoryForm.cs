using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Database;
using System.Linq; // Added for Sum()

namespace OnlineOrderingSystem.Forms
{
    public class OrderHistoryForm : Form
    {
        private int customerId;
        private ListBox lstOrderHistory;
        private ListBox lstOrderDetails;
        private Button btnRefresh;
        private Button btnBack;
        private Label lblTitle;
        private Label lblOrderHistory;
        private Label lblOrderDetails;

        public OrderHistoryForm(int customerId = 1)
        {
            this.customerId = customerId;
            InitializeComponent();
            LoadOrderHistory();
        }

        private void InitializeComponent()
        {
            this.Text = "Tasty Eats - Order History";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 248, 240);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateControls();
            SetupEventHandlers();
        }

        private void CreateControls()
        {
            // Title
            lblTitle = new Label
            {
                Text = "📋 Order History",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(900, 40)
            };

            // Order History Section
            lblOrderHistory = new Label
            {
                Text = "Your Orders:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(50, 90),
                Size = new Size(200, 25)
            };

            lstOrderHistory = new ListBox
            {
                Location = new Point(50, 120),
                Size = new Size(400, 400),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Segoe UI", 11)
            };

            // Order Details Section
            lblOrderDetails = new Label
            {
                Text = "Order Details:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(500, 90),
                Size = new Size(200, 25)
            };

            lstOrderDetails = new ListBox
            {
                Location = new Point(500, 120),
                Size = new Size(400, 400),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94),
                Font = new Font("Segoe UI", 11)
            };

            // Buttons
            btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                Location = new Point(50, 540),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnBack = new Button
            {
                Text = "← Back",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                Location = new Point(200, 540),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblOrderHistory, lstOrderHistory,
                lblOrderDetails, lstOrderDetails, btnRefresh, btnBack
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
            btnRefresh.Click += BtnRefresh_Click;
            btnBack.Click += BtnBack_Click;
            lstOrderHistory.SelectedIndexChanged += LstOrderHistory_SelectedIndexChanged;
        }

        private void LoadOrderHistory()
        {
            try
            {
                var orderDataAccess = new OrderDataAccess();
                var orderHistory = orderDataAccess.GetOrderHistoryWithPayments(customerId);

                lstOrderHistory.Items.Clear();
                lstOrderDetails.Items.Clear();

                if (orderHistory.Count == 0)
                {
                    lstOrderHistory.Items.Add("No orders found. Start ordering to see your history here!");
                    return;
                }

                foreach (var orderWithPayment in orderHistory)
                {
                    var order = orderWithPayment.Order;
                    var payment = orderWithPayment.Payment;
                    
                    var orderSummary = $"Order #{order.OrderID} - {order.OrderDate:dd/MM/yyyy HH:mm}";
                    var status = $"Status: {order.OrderStatus} | Payment: {order.PaymentStatus}";
                    var total = $"Total: £{order.TotalAmount:F2}";
                    
                    var displayText = $"{orderSummary}\n{status}\n{total}";
                    if (payment != null)
                    {
                        displayText += $"\nPayment: {payment.PaymentMethod} - {payment.TransactionID}";
                    }
                    
                    lstOrderHistory.Items.Add(displayText);
                }

                // Show order statistics
                var stats = orderDataAccess.GetOrderStatistics(customerId);
                var statsText = $"📊 Order Statistics:\n" +
                               $"Total Orders: {stats.totalOrders}\n" +
                               $"Total Spent: £{stats.totalSpent:F2}\n" +
                               $"Average Order: £{stats.averageOrderValue:F2}";
                
                lstOrderDetails.Items.Add(statsText);
                lstOrderDetails.Items.Add(""); // Empty line
                lstOrderDetails.Items.Add("Select an order from the left to view details.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order history: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstOrderHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstOrderHistory.SelectedIndex >= 0)
            {
                try
                {
                    var orderDataAccess = new OrderDataAccess();
                    var orderHistory = orderDataAccess.GetOrderHistoryWithPayments(customerId);
                    
                    if (lstOrderHistory.SelectedIndex < orderHistory.Count)
                    {
                        var selectedOrderWithPayment = orderHistory[lstOrderHistory.SelectedIndex];
                        var order = selectedOrderWithPayment.Order;
                        var payment = selectedOrderWithPayment.Payment;

                        lstOrderDetails.Items.Clear();

                        // Order Details
                        lstOrderDetails.Items.Add($"📋 Order #{order.OrderID}");
                        lstOrderDetails.Items.Add($"Date: {order.OrderDate:dd/MM/yyyy HH:mm}");
                        lstOrderDetails.Items.Add($"Status: {order.OrderStatus}");
                        lstOrderDetails.Items.Add($"Payment Status: {order.PaymentStatus}");
                        lstOrderDetails.Items.Add($"Delivery: {(order.IsDelivery ? "Yes" : "No")}");
                        if (order.IsDelivery)
                        {
                            lstOrderDetails.Items.Add($"Address: {order.DeliveryAddress}");
                            lstOrderDetails.Items.Add($"Estimated: {order.EstimatedDeliveryTime:HH:mm}");
                        }
                        lstOrderDetails.Items.Add("");

                        // Order Items
                        lstOrderDetails.Items.Add("🍽️ Order Items:");
                        foreach (var item in order.OrderItems)
                        {
                            lstOrderDetails.Items.Add($"  {item.ItemName} x{item.Quantity} - £{item.TotalPrice:F2}");
                        }
                        lstOrderDetails.Items.Add("");

                        // Totals
                        lstOrderDetails.Items.Add($"Subtotal: £{order.Subtotal:F2}");
                        lstOrderDetails.Items.Add($"Tax: £{order.TaxAmount:F2}");
                        lstOrderDetails.Items.Add($"Delivery Fee: £{order.DeliveryFee:F2}");
                        lstOrderDetails.Items.Add($"Total: £{order.TotalAmount:F2}");
                        lstOrderDetails.Items.Add("");

                        // Payment Details
                        if (payment != null)
                        {
                            lstOrderDetails.Items.Add("💳 Payment Details:");
                            lstOrderDetails.Items.Add($"  Method: {payment.PaymentMethod}");
                            lstOrderDetails.Items.Add($"  Amount: £{payment.Amount:F2}");
                            lstOrderDetails.Items.Add($"  Status: {payment.PaymentStatus}");
                            lstOrderDetails.Items.Add($"  Transaction ID: {payment.TransactionID}");
                            lstOrderDetails.Items.Add($"  Date: {payment.PaymentDate:dd/MM/yyyy HH:mm}");
                        }

                        // Status History
                        if (order.StatusHistory.Any())
                        {
                            lstOrderDetails.Items.Add("");
                            lstOrderDetails.Items.Add("📝 Status History:");
                            foreach (var status in order.StatusHistory.OrderBy(s => s.Timestamp))
                            {
                                lstOrderDetails.Items.Add($"  {status.Timestamp:HH:mm} - {status.Status}");
                                if (!string.IsNullOrEmpty(status.Message))
                                {
                                    lstOrderDetails.Items.Add($"    {status.Message}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lstOrderDetails.Items.Clear();
                    lstOrderDetails.Items.Add($"Error loading order details: {ex.Message}");
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrderHistory();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}