using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Database;
using System.Linq; // Added for .Select() and .Distinct()

namespace OnlineOrderingSystem.Forms
{
    public class MenuForm : Form
    {
        private ComboBox cmbCategory;
        private ListBox lstMenu;
        private Button btnAddToCart;
        private Button btnViewCart;
        private Button btnCheckout;
        private Button btnBack;
        private Label lblAlert;
        private Cart currentCart;
        private List<Item> allItems;
        private MenuDataAccess menuDataAccess;

        public MenuForm()
        {
            InitializeComponent();
            menuDataAccess = new MenuDataAccess();
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

            // Menu Items - Now using full width since cart is removed
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
                Size = new Size(1000, 400), // Increased width to use full form width
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 11)
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

            // Add controls to form - removed cart-related controls
            this.Controls.AddRange(new Control[] {
                lblTitle, lblCategory, cmbCategory, lblMenu, lstMenu,
                btnAddToCart, btnViewCart, btnCheckout, btnBack, lblAlert
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
            try
            {
                // Load items from database
                allItems = menuDataAccess.GetAvailableMenuItems();
                
                if (allItems == null || allItems.Count == 0)
                {
                    // If no items found, show an error message
                    ShowAlert("No menu items found in database. Please check database connection.", true);
                    allItems = new List<Item>(); // Initialize empty list to prevent crashes
                    return;
                }

                // Add customizations to some items (this could also be stored in DB)
                AddCustomizations();

                // Populate category dropdown
                var categories = allItems.Select(i => i.Category).Distinct().OrderBy(c => c).ToList();
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");
                cmbCategory.Items.AddRange(categories.ToArray());
                cmbCategory.SelectedIndex = 0;

                // Display all items initially
                DisplayItems(allItems);

                // Show success message
                ShowAlert($"Loaded {allItems.Count} menu items from database successfully!", false);
            }
            catch (Exception ex)
            {
                // Handle database connection errors
                ShowAlert($"Error loading menu items: {ex.Message}", true);
                allItems = new List<Item>(); // Initialize empty list to prevent crashes
                
                // Log the error (in a real app, you'd use a logging framework)
                Console.WriteLine($"MenuForm LoadMenuItems Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        // Removed CreateMenuItem method - now loading from database

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
                if (item.DietaryTags != null && item.DietaryTags.Count > 0)
                    displayText += $" ({string.Join(", ", item.DietaryTags)})";
                displayText += $" - {item.Rating:F1}★ ({item.ReviewCount} reviews)";
                
                // Add preparation time and calories info
                displayText += $" | {item.PrepTime}min | {item.Calories}cal";

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

            // The original code had lstCart.Items.Add(cartItemText);
            // Since lstCart is removed, this line is no longer relevant.
            // The total update logic is also removed as lblTotal is removed.
            ShowAlert($"{item.Name} added to cart!", false);
        }

        private void UpdateTotal()
        {
            // The original code had var total = currentCart.CalculateTotal();
            // Since lblTotal is removed, this line is no longer relevant.
            // The total update logic is also removed as lblTotal is removed.
        }

        private void BtnViewCart_Click(object sender, EventArgs e)
        {
            if (currentCart.Items.Count > 0)
            {
                var cartForm = new CartForm(currentCart);
                cartForm.ShowDialog();
                // RefreshCartDisplay(); // This method is no longer available
            }
            else
            {
                ShowAlert("Your cart is empty", true);
            }
        }

        private void RefreshCartDisplay()
        {
            // This method is no longer available
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
                    // RefreshCartDisplay(); // This method is no longer available
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
            CreateControls();
            SetupEventHandlers();
        }
    }
} 