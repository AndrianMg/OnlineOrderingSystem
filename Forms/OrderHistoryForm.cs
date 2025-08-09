using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq; // Added for Sum()

namespace OnlineOrderingSystem.Forms
{
    public class OrderHistoryForm : Form
    {
        private List<OrderHistoryItem> orderHistory;
        private Panel orderListPanel;
        private Label totalOrdersLabel;
        private Label totalSpentLabel;
        private Label loyaltyLabel;

        public OrderHistoryForm()
        {
            InitializeComponent();
            LoadOrderHistory();
        }

        private void InitializeComponent()
        {
            this.Text = "Order History - Tasty Eats";
            this.Size = new Size(830, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(33, 33, 33);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            CreateControls();
        }

        private void CreateControls()
        {
            // Header
            var lblHeader = new Label
            {
                Text = "📋 Order History",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(760, 40)
            };

            // Filter Panel
            var filterPanel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(760, 50),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            var lblFilter = new Label
            {
                Text = "Filter by Status:",
                Font = new Font("Arial", 10),
                ForeColor = Color.White,
                Location = new Point(10, 15),
                Size = new Size(100, 20)
            };

            var cmbFilter = new ComboBox
            {
                Location = new Point(120, 12),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilter.Items.AddRange(new object[] { "All Orders", "Delivered", "In Progress", "Preparing" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => FilterOrders();

            var btnExport = new Button
            {
                Text = "📄 Export PDF",
                Font = new Font("Arial", 9),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 130, 180),
                Location = new Point(260, 12),
                Size = new Size(100, 25),
                FlatStyle = FlatStyle.Flat
            };
            btnExport.Click += (s, e) => ExportToPDF();

            var btnPrint = new Button
            {
                Text = "🖨️ Print",
                Font = new Font("Arial", 9),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 130, 180),
                Location = new Point(370, 12),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat
            };
            btnPrint.Click += (s, e) => PrintHistory();

            filterPanel.Controls.AddRange(new Control[] { lblFilter, cmbFilter, btnExport, btnPrint });

            // Order List Panel (Scrollable)
            orderListPanel = new Panel
            {
                Location = new Point(20, 140),
                Size = new Size(785, 400),
                BackColor = Color.FromArgb(40, 40, 40),
                AutoScroll = true
            };

            // Summary Panel
            var summaryPanel = new Panel
            {
                Location = new Point(20, 560),
                Size = new Size(785, 80),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            totalOrdersLabel = new Label
            {
                Text = "📊 Total Orders: 0",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                Size = new Size(200, 25)
            };

            totalSpentLabel = new Label
            {
                Text = "💰 Total Spent: £0.00",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 45),
                Size = new Size(200, 25)
            };

            loyaltyLabel = new Label
            {
                Text = "🎁 Loyalty Rewards: You've saved £5.20 this month!",
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(255, 215, 0),
                Location = new Point(250, 15),
                Size = new Size(500, 25)
            };

            summaryPanel.Controls.AddRange(new Control[] { totalOrdersLabel, totalSpentLabel, loyaltyLabel });

            // Close Button
            var btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(350, 650),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblHeader, filterPanel, orderListPanel, summaryPanel, btnClose
            });
        }

        private void LoadOrderHistory()
        {
            // Simulate order history data
            orderHistory = new List<OrderHistoryItem>
            {
                new OrderHistoryItem
                {
                    OrderNumber = "001",
                    Amount = 25.47m,
                    Status = "Delivered",
                    ItemCount = 2,
                    OrderDate = DateTime.Now.AddDays(-3),
                    Items = new List<string> { "Chicken Wrap", "French Fries" },
                    EstimatedDelivery = null
                },
                new OrderHistoryItem
                {
                    OrderNumber = "002",
                    Amount = 18.99m,
                    Status = "In Progress",
                    ItemCount = 3,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Items = new List<string> { "Margherita Pizza", "Caesar Salad", "Coke" },
                    EstimatedDelivery = DateTime.Now.AddMinutes(25)
                },
                new OrderHistoryItem
                {
                    OrderNumber = "003",
                    Amount = 32.50m,
                    Status = "Preparing",
                    ItemCount = 4,
                    OrderDate = DateTime.Now.AddHours(-2),
                    Items = new List<string> { "Pepperoni Pizza", "Garlic Bread", "Chicken Wings", "Sprite" },
                    EstimatedDelivery = DateTime.Now.AddMinutes(45)
                },
                new OrderHistoryItem
                {
                    OrderNumber = "004",
                    Amount = 15.75m,
                    Status = "Delivered",
                    ItemCount = 2,
                    OrderDate = DateTime.Now.AddDays(-5),
                    Items = new List<string> { "Veggie Burger", "Onion Rings" },
                    EstimatedDelivery = null
                }
            };

            DisplayOrders(orderHistory);
            UpdateSummary();
        }

        private void DisplayOrders(List<OrderHistoryItem> orders)
        {
            orderListPanel.Controls.Clear();
            int yPosition = 10;

            foreach (var order in orders)
            {
                var orderPanel = CreateOrderPanel(order, yPosition);
                orderListPanel.Controls.Add(orderPanel);
                yPosition += 120;
            }
        }

        private Panel CreateOrderPanel(OrderHistoryItem order, int yPosition)
        {
            var panel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(740, 110),
                BackColor = Color.FromArgb(60, 60, 60),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Status icon and color
            string statusIcon = GetStatusIcon(order.Status);
            Color statusColor = GetStatusColor(order.Status);

            var lblStatus = new Label
            {
                Text = $"{statusIcon} {order.Status}",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(15, 10),
                Size = new Size(150, 20)
            };

            // Order number and date
            var lblOrderInfo = new Label
            {
                Text = $"📋 Order #{order.OrderNumber} • {order.OrderDate:MMM dd, yyyy}",
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 35),
                Size = new Size(300, 20)
            };

            // Amount
            var lblAmount = new Label
            {
                Text = $"💰 £{order.Amount:F2}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 215, 0),
                Location = new Point(320, 35),
                Size = new Size(100, 20)
            };

            // Items preview
            var itemsText = string.Join(", ", order.Items);
            var lblItems = new Label
            {
                Text = $"🍽️ {itemsText}",
                Font = new Font("Arial", 9),
                ForeColor = Color.LightGray,
                Location = new Point(15, 60),
                Size = new Size(500, 20)
            };

            // Item count
            var lblItemCount = new Label
            {
                Text = $"📦 {order.ItemCount} items",
                Font = new Font("Arial", 9),
                ForeColor = Color.LightGray,
                Location = new Point(15, 80),
                Size = new Size(100, 20)
            };

            // Estimated delivery (for non-delivered orders)
            if (order.EstimatedDelivery.HasValue)
            {
                var timeRemaining = order.EstimatedDelivery.Value - DateTime.Now;
                var lblDelivery = new Label
                {
                    Text = $"⏰ Est. Delivery: {order.EstimatedDelivery.Value:HH:mm} ({timeRemaining.Minutes} min remaining)",
                    Font = new Font("Arial", 9),
                    ForeColor = Color.FromArgb(255, 165, 0),
                    Location = new Point(120, 80),
                    Size = new Size(300, 20)
                };
                panel.Controls.Add(lblDelivery);
            }

            // Progress bar for non-delivered orders
            if (order.Status != "Delivered")
            {
                var progressBar = new ProgressBar
                {
                    Location = new Point(430, 35),
                    Size = new Size(150, 15),
                    Style = ProgressBarStyle.Continuous
                };

                int progress = order.Status switch
                {
                    "Preparing" => 25,
                    "In Progress" => 75,
                    _ => 0
                };
                progressBar.Value = progress;

                var lblProgress = new Label
                {
                    Text = $"{progress}%",
                    Font = new Font("Arial", 8),
                    ForeColor = Color.White,
                    Location = new Point(590, 35),
                    Size = new Size(40, 15)
                };

                panel.Controls.Add(progressBar);
                panel.Controls.Add(lblProgress);
            }

            // View Details button
            var btnDetails = new Button
            {
                Text = "👁️ View Details",
                Font = new Font("Arial", 8),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 130, 180),
                Location = new Point(650, 35),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat
            };
            btnDetails.Click += (s, e) => ShowOrderDetails(order);

            panel.Controls.AddRange(new Control[] {
                lblStatus, lblOrderInfo, lblAmount, lblItems, lblItemCount, btnDetails
            });

            return panel;
        }

        private string GetStatusIcon(string status)
        {
            return status switch
            {
                "Delivered" => "✅",
                "In Progress" => "🚚",
                "Preparing" => "👨‍🍳",
                _ => "📋"
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Delivered" => Color.FromArgb(76, 175, 80), // Green
                "In Progress" => Color.FromArgb(255, 152, 0), // Amber
                "Preparing" => Color.FromArgb(33, 150, 243), // Blue
                _ => Color.Gray
            };
        }

        private void ShowOrderDetails(OrderHistoryItem order)
        {
            var details = $"📋 Order #{order.OrderNumber}\n\n";
            details += $"📅 Date: {order.OrderDate:MMMM dd, yyyy 'at' HH:mm}\n";
            details += $"💰 Amount: £{order.Amount:F2}\n";
            details += $"📦 Items ({order.ItemCount}):\n";
            
            foreach (var item in order.Items)
            {
                details += $"   • {item}\n";
            }
            
            details += $"\n📊 Status: {order.Status}";
            
            if (order.EstimatedDelivery.HasValue)
            {
                details += $"\n⏰ Estimated Delivery: {order.EstimatedDelivery.Value:HH:mm}";
            }

            MessageBox.Show(details, $"Order #{order.OrderNumber} Details", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FilterOrders()
        {
            // Get the selected filter from the combo box
            var filterComboBox = this.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.Controls.OfType<ComboBox>().Any())
                ?.Controls.OfType<ComboBox>().FirstOrDefault();
            
            if (filterComboBox == null) return;
            
            var selectedFilter = filterComboBox.Text;
            List<OrderHistoryItem> filteredOrders;
            
            switch (selectedFilter)
            {
                case "Delivered":
                    filteredOrders = orderHistory.Where(o => o.Status == "Delivered").ToList();
                    break;
                case "In Progress":
                    filteredOrders = orderHistory.Where(o => o.Status == "In Progress").ToList();
                    break;
                case "Preparing":
                    filteredOrders = orderHistory.Where(o => o.Status == "Preparing").ToList();
                    break;
                case "All Orders":
                default:
                    filteredOrders = orderHistory.ToList();
                    break;
            }
            
            DisplayOrders(filteredOrders);
        }

        private void UpdateSummary()
        {
            totalOrdersLabel.Text = $"📊 Total Orders: {orderHistory.Count}";
            
            decimal totalSpent = orderHistory.Sum(o => o.Amount);
            totalSpentLabel.Text = $"💰 Total Spent: £{totalSpent:F2}";
            
            loyaltyLabel.Text = "🎁 Loyalty Rewards: You've saved £5.20 this month!";
        }

        private void ExportToPDF()
        {
            MessageBox.Show("PDF export functionality would be implemented here.\n\nThis would generate a detailed PDF report of your order history.", 
                "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrintHistory()
        {
            MessageBox.Show("Print functionality would be implemented here.\n\nThis would print a formatted version of your order history.", 
                "Print History", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    public class OrderHistoryItem
    {
        public string OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<string> Items { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
    }
} 