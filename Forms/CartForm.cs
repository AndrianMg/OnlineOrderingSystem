using System;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Forms
{
    public class CartForm : Form
    {
        private Cart cart;
        private int customerId;
        private ListBox? lstCartItems;
        private Button? btnRemoveItem;
        private Button? btnUpdateQuantity;
        private Button? btnClearCart;
        private Button? btnCheckout;
        private Button? btnBack;
        private Label? lblTotal;
        private Label? lblItemCount;

        public CartForm(Cart cart, int customerId = 1)
        {
            this.cart = cart;
            this.customerId = customerId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // CartForm
            // 
            BackColor = Color.FromArgb(255, 248, 240); // Soft beige like other forms
            ClientSize = new Size(800, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CartForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tasty Eats - Your Cart";
            Load += CartForm_Load;
            ResumeLayout(false);
        }

        private void CreateControls()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "🛒 Your Shopping Cart",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94), // Dark gray
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(700, 40)
            };

            // Cart Items
            var lblCartItems = new Label
            {
                Text = "Cart Items:",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 152, 219), // Primary blue
                Location = new Point(50, 90),
                Size = new Size(200, 25)
            };

            lstCartItems = new ListBox
            {
                Location = new Point(50, 120),
                Size = new Size(500, 300),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(52, 73, 94), // Dark gray
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Item Count
            lblItemCount = new Label
            {
                Text = "Items: 0",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(50, 440),
                Size = new Size(200, 25)
            };

            // Total
            lblTotal = new Label
            {
                Text = "Total: £0.00",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 204, 113), // Success green
                Location = new Point(50, 470),
                Size = new Size(300, 30)
            };

            // Buttons
            btnRemoveItem = new Button
            {
                Text = "🗑️ Remove Item",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60), // Warning red
                Location = new Point(580, 120),
                Size = new Size(150, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnUpdateQuantity = new Button
            {
                Text = "✏️ Update Quantity",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219), // Primary blue
                Location = new Point(580, 165),
                Size = new Size(150, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnClearCart = new Button
            {
                Text = "🧹 Clear Cart",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(108, 117, 125), // Gray
                Location = new Point(580, 210),
                Size = new Size(150, 35),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnCheckout = new Button
            {
                Text = "💳 Proceed to Checkout",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113), // Success green
                Location = new Point(50, 520),
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnBack = new Button
            {
                Text = "🛍️ Continue Shopping",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219), // Primary blue
                Location = new Point(270, 520),
                Size = new Size(150, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblCartItems, lstCartItems, lblItemCount, lblTotal,
                btnRemoveItem, btnUpdateQuantity, btnClearCart, btnCheckout, btnBack
            });
        }

        private void SetupEventHandlers()
        {
            btnRemoveItem!.Click += BtnRemoveItem_Click;
            btnUpdateQuantity!.Click += BtnUpdateQuantity_Click;
            btnClearCart!.Click += BtnClearCart_Click;
            btnCheckout!.Click += BtnCheckout_Click;
            btnBack!.Click += BtnBack_Click;
        }

        private void RefreshCartDisplay()
        {
            lstCartItems!.Items.Clear();

            foreach (var cartItem in cart.Items)
            {
                var displayText = $"{cartItem.Item.Name} - £{cartItem.Item.Price:F2} x {cartItem.Quantity} = £{cartItem.TotalPrice:F2}";
                if (cartItem.Item.CustomizationOptions.Count > 0)
                {
                    displayText += " (Customizable)";
                }
                lstCartItems!.Items.Add(displayText);
            }

            lblItemCount!.Text = $"Items: {cart.Items.Count}";
            var total = cart.CalculateTotal();
            lblTotal!.Text = $"Total: £{total:F2}";

            // Enable/disable buttons based on cart state
            bool hasItems = cart.Items.Count > 0;
            btnRemoveItem!.Enabled = hasItems;
            btnUpdateQuantity!.Enabled = hasItems;
            btnClearCart!.Enabled = hasItems;
            btnCheckout!.Enabled = hasItems;
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (lstCartItems!.SelectedIndex >= 0)
            {
                var selectedCartItem = cart.Items[lstCartItems!.SelectedIndex];
                cart.RemoveItem(selectedCartItem.Item.ItemID);
                RefreshCartDisplay();
                MessageBox.Show($"{selectedCartItem.Item.Name} removed from cart.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUpdateQuantity_Click(object sender, EventArgs e)
        {
            if (lstCartItems!.SelectedIndex >= 0)
            {
                var selectedCartItem = cart.Items[lstCartItems!.SelectedIndex];

                using (var inputForm = new QuantityInputForm(selectedCartItem.Item.Name))
                {
                    if (inputForm.ShowDialog() == DialogResult.OK)
                    {
                        var newQuantity = inputForm.Quantity;
                        if (newQuantity > 0)
                        {
                            // Update quantity using the new method
                            cart.UpdateItemQuantity(selectedCartItem.Item.ItemID, newQuantity);
                            RefreshCartDisplay();
                            MessageBox.Show($"Quantity updated for {selectedCartItem.Item.Name}.", "Quantity Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            cart.RemoveItem(selectedCartItem.Item.ItemID);
                            RefreshCartDisplay();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item to update.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear your cart?", "Clear Cart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cart.Clear();
                RefreshCartDisplay();
                MessageBox.Show("Cart cleared successfully.", "Cart Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (cart.Items.Count > 0)
            {
                var checkoutForm = new CheckoutForm(cart, customerId);
                checkoutForm.ShowDialog();
                if (checkoutForm.OrderPlaced)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Your cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CartForm_Load(object sender, EventArgs e)
        {
            CreateControls();
            SetupEventHandlers();
            RefreshCartDisplay();
        }
    }

    // Helper form for quantity input
    public class QuantityInputForm : Form
    {
        private NumericUpDown? numQuantity;
        private Button? btnOK;
        private Button? btnCancel;

        public int Quantity => (int)numQuantity!.Value;

        public QuantityInputForm(string itemName)
        {
            InitializeComponent(itemName);
        }

        private void InitializeComponent(string itemName)
        {
            this.Text = "Update Quantity";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(33, 33, 33);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            var lblTitle = new Label
            {
                Text = $"Update quantity for {itemName}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(260, 25)
            };

            // Quantity Label
            var lblQuantity = new Label
            {
                Text = "Quantity:",
                Font = new Font("Arial", 11),
                ForeColor = Color.White,
                Location = new Point(20, 60),
                Size = new Size(80, 25)
            };

            // Quantity Input
            numQuantity = new NumericUpDown
            {
                Location = new Point(110, 60),
                Size = new Size(80, 25),
                Minimum = 1,
                Maximum = 99,
                Value = 1,
                Font = new Font("Arial", 11)
            };

            // Buttons
            btnOK = new Button
            {
                Text = "OK",
                Font = new Font("Arial", 11),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(0, 150, 0),
                Location = new Point(80, 120),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Arial", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(180, 120),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblTitle, lblQuantity, numQuantity, btnOK, btnCancel
            });
        }
    }
} 