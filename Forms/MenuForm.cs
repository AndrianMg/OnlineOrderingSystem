using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;
using System.Linq; // Added for .Select() and .Distinct()

namespace OnlineOrderingSystem.Forms
{
    public class MenuForm : Form
    {
        private ComboBox cmbCategory;
        private ListBox lstMenu;
        private ListBox lstCart;
        private Button btnAddToCart;
        private Button btnViewCart;
        private Button btnCheckout;
        private Button btnBack;
        private Label lblTotal;
        private Label lblAlert;
        private Cart currentCart;
        private List<Item> allItems;

        public MenuForm()
        {
            InitializeComponent();
            LoadMenuItems();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // MenuForm
            // 
            BackColor = Color.FromArgb(33, 33, 33);
            ClientSize = new Size(1184, 761);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tasty Eats - Menu";
            Load += MenuForm_Load;
            ResumeLayout(false);
        }

        private void CreateControls()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "🍽️ TASTY EATS MENU",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(1100, 50)
            };

            // Category Filter
            var lblCategory = new Label
            {
                Text = "Category:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 100),
                Size = new Size(100, 25)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(160, 100),
                Size = new Size(200, 30),
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Menu Items
            var lblMenu = new Label
            {
                Text = "Menu Items:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(50, 150),
                Size = new Size(200, 25)
            };

            lstMenu = new ListBox
            {
                Location = new Point(50, 180),
                Size = new Size(600, 400),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 11)
            };

            // Cart
            var lblCart = new Label
            {
                Text = "Your Cart:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(700, 150),
                Size = new Size(200, 25)
            };

            lstCart = new ListBox
            {
                Location = new Point(700, 180),
                Size = new Size(400, 300),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 10)
            };

            // Total
            lblTotal = new Label
            {
                Text = "Total: £0.00",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(700, 500),
                Size = new Size(200, 25)
            };

            // Buttons
            btnAddToCart = new Button
            {
                Text = "Add to Cart",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(0, 150, 0),
                Location = new Point(50, 600),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnViewCart = new Button
            {
                Text = "View Cart",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 100, 200),
                Location = new Point(190, 600),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnCheckout = new Button
            {
                Text = "Checkout",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(255, 165, 0),
                Location = new Point(330, 600),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnBack = new Button
            {
                Text = "Back to Main",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(470, 600),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Alert label
            lblAlert = new Label
            {
                Text = "",
                Font = new Font("Arial", 10),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(33, 33, 33),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 660),
                Size = new Size(1100, 25)
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblCategory, cmbCategory, lblMenu, lstMenu,
                lblCart, lstCart, lblTotal, btnAddToCart, btnViewCart,
                btnCheckout, btnBack, lblAlert
            });

            // Initialize cart
            currentCart = new Cart();
        }

        private void SetupEventHandlers()
        {
            btnAddToCart.Click += BtnAddToCart_Click;
            btnViewCart.Click += BtnViewCart_Click;
            btnCheckout.Click += BtnCheckout_Click;
            btnBack.Click += BtnBack_Click;
            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            lstMenu.SelectedIndexChanged += LstMenu_SelectedIndexChanged;
        }

        private void LoadMenuItems()
        {
            // Create sample menu items for Tasty Eats
            allItems = new List<Item>
            {
                // Starters
                CreateMenuItem(1, "Garlic Bread", "Fresh baked garlic bread with herbs", 4.99, "Starters", true, 10, 4.5, new List<string> { "Vegetarian" }),
                CreateMenuItem(2, "Bruschetta", "Toasted bread with tomatoes and basil", 5.99, "Starters", true, 8, 4.3, new List<string> { "Vegetarian", "Gluten-Free" }),
                CreateMenuItem(3, "Chicken Wings", "Crispy wings with your choice of sauce", 8.99, "Starters", true, 15, 4.7, new List<string> { "Spicy" }),

                // Main Courses
                CreateMenuItem(4, "Margherita Pizza", "Classic tomato and mozzarella", 12.99, "Main Courses", true, 20, 4.6, new List<string> { "Vegetarian" }),
                CreateMenuItem(5, "Pepperoni Pizza", "Spicy pepperoni with cheese", 14.99, "Main Courses", true, 20, 4.8, new List<string> { "Spicy" }),
                CreateMenuItem(6, "Chicken Caesar Salad", "Fresh lettuce with grilled chicken", 11.99, "Main Courses", true, 12, 4.4, new List<string> { "Healthy" }),
                CreateMenuItem(7, "Beef Burger", "Juicy beef burger with fries", 13.99, "Main Courses", true, 18, 4.7, new List<string> { "Popular" }),
                CreateMenuItem(8, "Fish & Chips", "Crispy battered cod with chips", 15.99, "Main Courses", true, 25, 4.5, new List<string> { "British Classic" }),

                // Desserts
                CreateMenuItem(9, "Chocolate Cake", "Rich chocolate cake with cream", 6.99, "Desserts", true, 10, 4.6, new List<string> { "Vegetarian" }),
                CreateMenuItem(10, "Ice Cream", "Vanilla ice cream with toppings", 4.99, "Desserts", true, 5, 4.3, new List<string> { "Vegetarian" }),

                // Drinks
                CreateMenuItem(11, "Soft Drinks", "Coca-Cola, Sprite, Fanta", 2.99, "Drinks", true, 2, 4.2, new List<string> { "Vegetarian" }),
                CreateMenuItem(12, "Coffee", "Fresh brewed coffee", 3.50, "Drinks", true, 3, 4.4, new List<string> { "Vegetarian" }),
                CreateMenuItem(13, "Tea", "English breakfast tea", 2.50, "Drinks", true, 2, 4.1, new List<string> { "Vegetarian" })
            };

            // Add customizations to some items
            AddCustomizations();

            // Populate category dropdown
            var categories = allItems.Select(i => i.Category).Distinct().ToList();
            cmbCategory.Items.Add("All Categories");
            cmbCategory.Items.AddRange(categories.ToArray());
            cmbCategory.SelectedIndex = 0;

            // Display all items initially
            DisplayItems(allItems);
        }

        private Item CreateMenuItem(int id, string name, string description, double price, string category, bool available, int prepTime, double rating, List<string> dietaryTags)
        {
            var item = new Item
            {
                ItemID = id,
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                Availability = available,
                PreparationTime = prepTime,
                Rating = rating,
                ReviewCount = (int)(rating * 20), // Simulate review count
                IsPopular = rating >= 4.5,
                IsChefSpecial = rating >= 4.7
            };

            foreach (var tag in dietaryTags)
            {
                item.AddDietaryTag(tag);
            }

            return item;
        }

        private void AddCustomizations()
        {
            // Add customization options to pizza items
            var pizzaItems = allItems.Where(i => i.Name.Contains("Pizza")).ToList();
            foreach (var pizza in pizzaItems)
            {
                pizza.AddCustomizationOption(new CustomizationOption("Extra Cheese", 1.50));
                pizza.AddCustomizationOption(new CustomizationOption("Extra Toppings", 2.00));
                pizza.AddCustomizationOption(new CustomizationOption("Spicy Level", 0.50));
            }

            // Add customization options to burger
            var burger = allItems.FirstOrDefault(i => i.Name.Contains("Burger"));
            if (burger != null)
            {
                burger.AddCustomizationOption(new CustomizationOption("Extra Patty", 3.00));
                burger.AddCustomizationOption(new CustomizationOption("Bacon", 1.50));
                burger.AddCustomizationOption(new CustomizationOption("Cheese", 1.00));
            }
        }

        private void DisplayItems(List<Item> items)
        {
            lstMenu.Items.Clear();
            foreach (var item in items)
            {
                var displayText = $"{item.Name} - £{item.Price:F2}";
                if (item.IsPopular)
                    displayText += " ⭐";
                if (item.IsChefSpecial)
                    displayText += " 👨‍🍳";
                if (item.DietaryTags.Count > 0)
                    displayText += $" ({string.Join(", ", item.DietaryTags)})";
                displayText += $" - {item.Rating:F1}★ ({item.ReviewCount} reviews)";

                lstMenu.Items.Add(displayText);
            }
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
            {
                var selectedCategory = cmbCategory.SelectedItem.ToString();
                if (selectedCategory == "All Categories")
                {
                    DisplayItems(allItems);
                }
                else
                {
                    var filteredItems = allItems.Where(i => i.Category == selectedCategory).ToList();
                    DisplayItems(filteredItems);
                }
            }
        }

        private void LstMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMenu.SelectedIndex >= 0)
            {
                btnAddToCart.Enabled = true;
            }
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (lstMenu.SelectedIndex >= 0)
            {
                var selectedItem = GetSelectedItem();
                if (selectedItem != null)
                {
                    // Check if item is customizable
                    if (selectedItem.IsCustomizable())
                    {
                        var customizationForm = new CustomizationForm(selectedItem);
                        if (customizationForm.ShowDialog() == DialogResult.OK)
                        {
                            AddItemToCart(selectedItem, customizationForm.SelectedCustomizations);
                        }
                    }
                    else
                    {
                        AddItemToCart(selectedItem, new List<string>());
                    }
                }
            }
        }

        private Item? GetSelectedItem()
        {
            if (lstMenu.SelectedIndex >= 0 && lstMenu.SelectedItem != null)
            {
                var selectedText = lstMenu.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    var itemName = selectedText.Split('-')[0].Trim();
                    return allItems.FirstOrDefault(i => i.Name == itemName);
                }
            }
            return null;
        }

        private void AddItemToCart(Item item, List<string> customizations)
        {
            currentCart.AddItem(item, 1);

            var cartItemText = $"{item.Name} - £{item.Price:F2}";
            if (customizations.Count > 0)
            {
                cartItemText += $" ({string.Join(", ", customizations)})";
            }

            lstCart.Items.Add(cartItemText);
            UpdateTotal();
            ShowAlert($"{item.Name} added to cart!", false);
        }

        private void UpdateTotal()
        {
            var total = currentCart.CalculateTotal();
            lblTotal.Text = $"Total: £{total:F2}";
        }

        private void BtnViewCart_Click(object sender, EventArgs e)
        {
            if (currentCart.Items.Count > 0)
            {
                var cartForm = new CartForm(currentCart);
                cartForm.ShowDialog();
                RefreshCartDisplay();
            }
            else
            {
                ShowAlert("Your cart is empty", true);
            }
        }

        private void RefreshCartDisplay()
        {
            lstCart.Items.Clear();
            foreach (var cartItem in currentCart.Items)
            {
                lstCart.Items.Add($"{cartItem.Item.Name} - £{cartItem.Item.Price:F2} x {cartItem.Quantity}");
            }
            UpdateTotal();
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (currentCart.Items.Count > 0)
            {
                var checkoutForm = new CheckoutForm(currentCart);
                checkoutForm.ShowDialog();
                if (checkoutForm.OrderPlaced)
                {
                    currentCart.Clear();
                    RefreshCartDisplay();
                    ShowAlert("Order placed successfully! Thank you for choosing Tasty Eats!", false);
                }
            }
            else
            {
                ShowAlert("Your cart is empty", true);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowAlert(string message, bool isError)
        {
            lblAlert.Text = message;
            lblAlert.BackColor = isError ? Color.FromArgb(255, 192, 192) : Color.FromArgb(192, 255, 192);
            lblAlert.ForeColor = isError ? Color.Black : Color.Black;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }
    }
} 