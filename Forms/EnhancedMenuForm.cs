using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;
using OnlineOrderingSystem.Database;
using OnlineOrderingSystem.Services;
using System.Linq;

namespace OnlineOrderingSystem.Forms
{
    /// <summary>
    /// Modern enhanced menu form with advanced features like search, categories, and live cart preview
    /// Features a visually appealing design with gradient background and card-style layout
    /// </summary>
    public class EnhancedMenuForm : Form
    {
        // UI Controls
        private TabControl categoryTabs;
        private ListBox lstMenu;
        private TextBox txtSearch;
        private Panel cardPanel;
        private Label lblLogo;
        private Label lblTagline;
        
        // Notification controls
        private Panel notificationPanel;
        private ListBox lstNotifications;
        private Label lblNotificationTitle;
        private Button btnClearNotifications;
        private Button btnNotificationStats;
        
        // Data management
        private Cart currentCart;
        private List<Item> allItems;
        private List<Item> filteredItems;
        private Dictionary<string, List<Item>> itemsByCategory;
        private MenuDataAccess menuDataAccess;
        private int customerId;
        
        // Services
        private NotificationService notificationService;

        // Color scheme
        private Color primaryBlue = Color.FromArgb(52, 152, 219);
        private Color successGreen = Color.FromArgb(46, 204, 113);
        private Color warningRed = Color.FromArgb(231, 76, 60);
        private Color softBeige = Color.FromArgb(255, 248, 240);
        private Color warmOrange = Color.FromArgb(255, 165, 0);
        private Color darkGray = Color.FromArgb(52, 73, 94);

        /// <summary>
        /// Initializes the modern enhanced menu form
        /// </summary>
        public EnhancedMenuForm(int customerId = 1)
        {
            this.customerId = customerId;
            InitializeComponent();
            menuDataAccess = new MenuDataAccess();
            notificationService = NotificationService.GetInstance();
            LoadMenuItems();
            InitializeNotifications();
        }

        /// <summary>
        /// Sets up the form properties and creates modern UI controls
        /// </summary>
        private void InitializeComponent()
        {
            // Form configuration
            this.Text = "Tasty Eats - Enhanced Menu";
            this.Size = new Size(1600, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = softBeige;

            CreateControls();
            SetupEventHandlers();
        }

        /// <summary>
        /// Creates and configures all modern UI controls for the enhanced menu form
        /// </summary>
        private void CreateControls()
        {
            // Create gradient background
            this.Paint += (sender, e) => DrawGradientBackground(e.Graphics);

            // Main card container with shadow effect
            cardPanel = new Panel
            {
                Location = new Point(50, 50),
                Size = new Size(1500, 900),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect to card
            cardPanel.Paint += (sender, e) => DrawCardShadow(e.Graphics);

            // Logo and branding
            lblLogo = new Label
            {
                Text = "🍽️ TASTY EATS",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = warmOrange,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(400, 50)
            };

            // Tagline
            lblTagline = new Label
            {
                Text = "Explore our delicious menu",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 80),
                Size = new Size(400, 25)
            };

            // Header section with search functionality
            var headerPanel = new Panel
            {
                Location = new Point(50, 120),
                Size = new Size(1400, 100), // Increased height to accommodate larger buttons
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.None
            };

            // Main title
            var lblTitle = new Label
            {
                Text = "📋 MENU",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = darkGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(300, 40)
            };

            // Search functionality
            var lblSearch = new Label
            {
                Text = "🔍 Search:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = darkGray,
                Location = new Point(400, 25),
                Size = new Size(80, 25)
            };

            txtSearch = new TextBox
            {
                Location = new Point(490, 22),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.White,
                ForeColor = darkGray,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Search by name or dietary preference..."
            };

            // Add placeholder behavior
            AddPlaceholderBehavior(txtSearch, "Search by name or dietary preference...");

            // Action buttons - Made larger and better spaced
            var btnBack = CreateStyledButton("← Back", primaryBlue, new Point(900, 20), new Size(120, 45));
            btnBack.Click += BtnBack_Click;

            var btnViewCart = CreateStyledButton("🛒 View Cart", successGreen, new Point(1040, 20), new Size(140, 45));
            btnViewCart.Click += BtnViewCart_Click;

            var btnCheckout = CreateStyledButton("💳 Checkout", warningRed, new Point(1200, 20), new Size(140, 45));
            btnCheckout.Click += BtnCheckout_Click;

            headerPanel.Controls.AddRange(new Control[] {
                lblTitle, lblSearch, txtSearch, btnBack, btnViewCart, btnCheckout
            });

            // Menu list - positioned below the category tabs
            lstMenu = new ListBox
            {
                Location = new Point(50, 270),
                Size = new Size(800, 450), // Reduced from 1000 to 800 to make room for notification panel
                Font = new Font("Segoe UI", 11),
                BackColor = Color.White,
                ForeColor = darkGray,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = true,
                Enabled = true
            };

            // Category tabs - positioned above the menu list
            categoryTabs = new TabControl
            {
                Location = new Point(50, 220),
                Size = new Size(800, 40), // Reduced from 1000 to 800 to match menu width
                Font = new Font("Segoe UI", 9),
                BackColor = Color.White
            };

            // Notification panel - positioned to the right of the menu
            CreateNotificationPanel();

            // Add all controls to the card panel
            cardPanel.Controls.AddRange(new Control[] {
                lblLogo, lblTagline, headerPanel, categoryTabs, lstMenu, notificationPanel
            });

            // Add card panel to form
            this.Controls.Add(cardPanel);
        }

        /// <summary>
        /// Creates a styled button with hover effects and rounded corners
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="backgroundColor">Button background color</param>
        /// <param name="location">Button location</param>
        /// <param name="size">Button size</param>
        /// <returns>Styled button control</returns>
        private Button CreateStyledButton(string text, Color backgroundColor, Point location, Size size)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = backgroundColor,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };

            // Add hover effects
            button.MouseEnter += (sender, e) => {
                button.BackColor = ControlPaint.Light(backgroundColor);
            };
            button.MouseLeave += (sender, e) => {
                button.BackColor = backgroundColor;
            };

            return button;
        }

        /// <summary>
        /// Adds placeholder behavior to a text box
        /// </summary>
        /// <param name="textBox">The text box to add placeholder behavior to</param>
        /// <param name="placeholder">The placeholder text</param>
        private void AddPlaceholderBehavior(TextBox textBox, string placeholder)
        {
            textBox.Enter += (sender, e) => {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = darkGray;
                }
            };
            textBox.Leave += (sender, e) => {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        /// <summary>
        /// Draws gradient background for the form
        /// </summary>
        /// <param name="g">Graphics object</param>
        private void DrawGradientBackground(Graphics g)
        {
            using (var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, this.Height),
                Color.FromArgb(255, 248, 240), // Soft beige
                Color.FromArgb(255, 235, 205)  // Warmer beige
            ))
            {
                g.FillRectangle(brush, 0, 0, this.Width, this.Height);
            }
        }

        /// <summary>
        /// Draws shadow effect for the card panel
        /// </summary>
        /// <param name="g">Graphics object</param>
        private void DrawCardShadow(Graphics g)
        {
            // Draw shadow
            using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            {
                g.FillRectangle(shadowBrush, 5, 5, cardPanel.Width - 10, cardPanel.Height - 10);
            }

            // Draw card background
            using (var cardBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(cardBrush, 0, 0, cardPanel.Width, cardPanel.Height);
            }
        }

        /// <summary>
        /// Sets up event handlers for all interactive controls
        /// </summary>
        private void SetupEventHandlers()
        {
            txtSearch.TextChanged += TxtSearch_TextChanged;
            lstMenu.SelectedIndexChanged += LstMenu_SelectedIndexChanged;
            lstMenu.MouseMove += LstMenu_MouseMove;
            lstMenu.MouseLeave += LstMenu_MouseLeave;

            // Initialize cart
            currentCart = new Cart();
            allItems = new List<Item>();
            filteredItems = new List<Item>();
            itemsByCategory = new Dictionary<string, List<Item>>();
        }

        /// <summary>
        /// Loads menu items and initializes the cart
        /// </summary>
        private void LoadMenuItems()
        {
            // Initialize data structures
            currentCart = new Cart();
            allItems = new List<Item>();
            itemsByCategory = new Dictionary<string, List<Item>>();

            try
            {
                // Load items from database
                allItems = menuDataAccess.GetAvailableMenuItems();
                
                if (allItems == null || allItems.Count == 0)
                {
                    // If no items found in database, fall back to sample data
                    LoadSampleData();
                    Console.WriteLine("No items found in database. Using sample data.");
                }
                else
                {
                    // Alert will be shown after form is loaded
                    Console.WriteLine($"Loaded {allItems.Count} items from database successfully!");
                }
            }
            catch (Exception ex)
            {
                // If database connection fails, fall back to sample data
                LoadSampleData();
                Console.WriteLine($"Database connection failed: {ex.Message}. Using sample data.");
            }

            // Group items by category for filtering
            foreach (var item in allItems)
            {
                if (!itemsByCategory.ContainsKey(item.Category))
                    itemsByCategory[item.Category] = new List<Item>();
                itemsByCategory[item.Category].Add(item);
            }

            CreateCategoryTabs();
            DisplayItems(allItems);

            // Debug: Ensure the ListBox is properly configured
            if (lstMenu != null)
            {
                lstMenu.Visible = true;
                lstMenu.Enabled = true;
                lstMenu.BringToFront();
            }
        }

        /// <summary>
        /// Loads sample menu data with various categories and items
        /// </summary>
        private void LoadSampleData()
        {
            // Sample data as fallback when database is not available
            var items = new List<Item>
            {
                CreateEnhancedMenuItem(1, "Garlic Bread", "Fresh baked bread with garlic butter", 4.99, "Starters", true, 8, 4.2, new List<string> { "Vegetarian", "Popular" }),
                CreateEnhancedMenuItem(2, "Bruschetta", "Toasted bread with tomatoes and basil", 5.49, "Starters", true, 10, 4.5, new List<string> { "Vegetarian", "Healthy" }),
                CreateEnhancedMenuItem(3, "Chicken Wings", "Crispy wings with choice of sauce", 8.99, "Starters", true, 15, 4.3, new List<string> { "Spicy", "Popular" }),
                CreateEnhancedMenuItem(4, "Margherita Pizza", "Classic tomato and mozzarella", 12.99, "Mains", true, 20, 4.6, new List<string> { "Vegetarian", "Chef's Pick" }),
                CreateEnhancedMenuItem(5, "Pepperoni Pizza", "Spicy pepperoni with melted cheese", 14.99, "Mains", true, 22, 4.7, new List<string> { "Spicy", "Popular" }),
                CreateEnhancedMenuItem(6, "Chicken Caesar Salad", "Fresh lettuce with grilled chicken", 9.99, "Mains", true, 12, 4.4, new List<string> { "Healthy", "Gluten-Free" }),
                CreateEnhancedMenuItem(7, "Beef Burger", "Juicy beef patty with fresh vegetables", 11.99, "Mains", true, 18, 4.5, new List<string> { "Popular", "British Classic" }),
                CreateEnhancedMenuItem(8, "Fish & Chips", "Crispy battered fish with chips", 13.99, "Mains", true, 25, 4.8, new List<string> { "British Classic", "Chef's Pick" }),
                CreateEnhancedMenuItem(9, "Chocolate Cake", "Rich chocolate cake with cream", 6.99, "Desserts", true, 5, 4.3, new List<string> { "Vegetarian", "Popular" }),
                CreateEnhancedMenuItem(10, "Ice Cream", "Vanilla ice cream with toppings", 4.99, "Desserts", true, 3, 4.1, new List<string> { "Vegetarian", "Gluten-Free" }),
                CreateEnhancedMenuItem(11, "Soft Drinks", "Coca-Cola, Sprite, Fanta", 2.99, "Drinks", true, 2, 4.0, new List<string> { "Vegetarian", "Gluten-Free" }),
                CreateEnhancedMenuItem(12, "Coffee", "Fresh brewed coffee", 3.49, "Drinks", true, 4, 4.2, new List<string> { "Vegetarian", "Gluten-Free" }),
                CreateEnhancedMenuItem(13, "Tea", "English breakfast tea", 2.99, "Drinks", true, 3, 4.1, new List<string> { "Vegetarian", "Gluten-Free" })
            };

            allItems.AddRange(items);
        }

        /// <summary>
        /// Creates an enhanced menu item with all properties
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="name">Item name</param>
        /// <param name="description">Item description</param>
        /// <param name="price">Item price</param>
        /// <param name="category">Item category</param>
        /// <param name="available">Whether item is available</param>
        /// <param name="prepTime">Preparation time in minutes</param>
        /// <param name="rating">Item rating (0-5)</param>
        /// <param name="dietaryTags">List of dietary tags</param>
        /// <returns>Configured Item object</returns>
        private Item CreateEnhancedMenuItem(int id, string name, string description, double price, string category, bool available, int prepTime, double rating, List<string> dietaryTags)
        {
            return new Item
            {
                Id = id,
                Name = name,
                Description = description,
                Price = price,
                Category = category,
                Available = available,
                PrepTime = prepTime,
                Rating = rating,
                DietaryTags = dietaryTags
            };
        }

        /// <summary>
        /// Creates category tabs for filtering menu items
        /// </summary>
        private void CreateCategoryTabs()
        {
            categoryTabs.TabPages.Clear();

            // Add "All" tab for showing all items
            var allTab = new TabPage("🍽️ All")
            {
                BackColor = Color.FromArgb(50, 50, 50)
            };
            categoryTabs.TabPages.Add(allTab);

            // Add category-specific tabs
            foreach (var category in itemsByCategory.Keys)
            {
                var tab = new TabPage(GetCategoryIcon(category) + " " + category)
                {
                    BackColor = Color.FromArgb(50, 50, 50)
                };
                categoryTabs.TabPages.Add(tab);
            }

            categoryTabs.SelectedIndexChanged += CategoryTabs_SelectedIndexChanged;
        }

        /// <summary>
        /// Returns the appropriate icon for each category
        /// </summary>
        /// <param name="category">Category name</param>
        /// <returns>Emoji icon for the category</returns>
        private string GetCategoryIcon(string category)
        {
            return category switch
            {
                "Starters" => "🥗",
                "Mains" => "🍽️",
                "Desserts" => "🍰",
                "Drinks" => "🥤",
                _ => "🍴"
            };
        }

        /// <summary>
        /// Displays a list of items in the menu listbox
        /// </summary>
        /// <param name="items">List of items to display</param>
        private void DisplayItems(List<Item> items)
        {
            lstMenu.Items.Clear();
            filteredItems = items;

            // Ensure the ListBox is visible and enabled
            lstMenu.Visible = true;
            lstMenu.Enabled = true;

            foreach (var item in items)
            {
                var displayText = FormatItemDisplay(item);
                lstMenu.Items.Add(displayText);
            }

            // Force refresh the ListBox
            lstMenu.Refresh();
        }

        /// <summary>
        /// Formats an item for display with all relevant information
        /// </summary>
        /// <param name="item">Item to format</param>
        /// <returns>Formatted display string</returns>
        private string FormatItemDisplay(Item item)
        {
            var stars = GetStarRating(item.Rating);
            var dietaryIcons = GetDietaryIcons(item.DietaryTags);
            var prepTimeText = $"⏱️ {item.PrepTime}min";
            var priceText = $"💰 £{item.Price:F2}";

            return $"{item.Name} {dietaryIcons}\n   {stars} ({item.Rating:F1}) • {prepTimeText} • {priceText}\n   {item.Description}";
        }

        /// <summary>
        /// Converts a numeric rating to star display
        /// </summary>
        /// <param name="rating">Numeric rating (0-5)</param>
        /// <returns>String representation with stars</returns>
        private string GetStarRating(double rating)
        {
            var fullStars = (int)rating;
            var hasHalfStar = rating % 1 >= 0.5;
            var emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

            return new string('★', fullStars) + (hasHalfStar ? "☆" : "") + new string('☆', emptyStars);
        }

        /// <summary>
        /// Converts dietary tags to emoji icons
        /// </summary>
        /// <param name="dietaryTags">List of dietary tags</param>
        /// <returns>String of emoji icons</returns>
        private string GetDietaryIcons(List<string> dietaryTags)
        {
            var icons = new List<string>();
            foreach (var tag in dietaryTags)
            {
                icons.Add(tag switch
                {
                    "Vegetarian" => "🥦",
                    "Spicy" => "🌶️",
                    "Healthy" => "🧘",
                    "Gluten-Free" => "🌾",
                    "Popular" => "🔥",
                    "Chef's Pick" => "👨‍🍳",
                    "British Classic" => "🇬🇧",
                    _ => ""
                });
            }
            return string.Join(" ", icons);
        }

        /// <summary>
        /// Handles category tab selection changes
        /// </summary>
        /// <param name="sender">The tab control</param>
        /// <param name="e">Event arguments</param>
        private void CategoryTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryTabs.SelectedIndex == 0)
            {
                // Show all items
                DisplayItems(allItems);
            }
            else
            {
                // Show items from selected category
                var selectedCategory = itemsByCategory.Keys.ElementAt(categoryTabs.SelectedIndex - 1);
                DisplayItems(itemsByCategory[selectedCategory]);
            }
        }

        /// <summary>
        /// Handles search text changes and filters items accordingly
        /// </summary>
        /// <param name="sender">The search textbox</param>
        /// <param name="e">Event arguments</param>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchTerm = txtSearch.Text.Trim();
            
            if (string.IsNullOrEmpty(searchTerm))
            {
                DisplayItems(allItems);
                return;
            }

            // Local search (original logic)
            var filteredItems = allItems.Where(item =>
                item.Name.ToLower().Contains(searchTerm.ToLower()) ||
                item.Description.ToLower().Contains(searchTerm.ToLower()) ||
                item.DietaryTags.Any(tag => tag.ToLower().Contains(searchTerm.ToLower()))
            ).ToList();

            DisplayItems(filteredItems);
        }

        /// <summary>
        /// Handles menu item selection and shows quantity dialog
        /// </summary>
        /// <param name="sender">The listbox</param>
        /// <param name="e">Event arguments</param>
        private void LstMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMenu.SelectedIndex >= 0)
            {
                var selectedItem = GetSelectedItem();
                if (selectedItem != null)
                {
                    ShowQuantityDialog(selectedItem);
                }
            }
        }

        /// <summary>
        /// Handles mouse movement over menu items (disabled to prevent flickering)
        /// </summary>
        /// <param name="sender">The listbox</param>
        /// <param name="e">Mouse event arguments</param>
        private void LstMenu_MouseMove(object sender, MouseEventArgs e)
        {
            // Disable automatic selection on hover to prevent text refreshing
            // The hover effect is handled by the ListBox's built-in highlighting
        }

        /// <summary>
        /// Handles mouse leave events (no action needed)
        /// </summary>
        /// <param name="sender">The listbox</param>
        /// <param name="e">Event arguments</param>
        private void LstMenu_MouseLeave(object sender, EventArgs e)
        {
            // No action needed - let the ListBox handle its own hover state
        }

        /// <summary>
        /// Gets the currently selected item from the filtered list
        /// </summary>
        /// <returns>Selected item or null if none selected</returns>
        private Item? GetSelectedItem()
        {
            if (lstMenu.SelectedIndex >= 0 && filteredItems != null)
            {
                return filteredItems[lstMenu.SelectedIndex];
            }
            return null;
        }

        /// <summary>
        /// Shows a dialog for selecting item quantity
        /// </summary>
        /// <param name="item">Item to add to cart</param>
        private void ShowQuantityDialog(Item item)
        {
            var quantityForm = new Form
            {
                Text = $"Add {item.Name}",
                Size = new Size(300, 200),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(33, 33, 33),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            // Quantity label
            var lblQuantity = new Label
            {
                Text = "Quantity:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(100, 25)
            };

            // Quantity selector
            var numQuantity = new NumericUpDown
            {
                Location = new Point(130, 20),
                Size = new Size(100, 25),
                Minimum = 1,
                Maximum = 10,
                Value = 1
            };

            // Add button
            var btnAdd = new Button
            {
                Text = "Add to Cart",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(76, 175, 80),
                Location = new Point(20, 60),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat
            };

            // Cancel button
            var btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(140, 60),
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat
            };

            btnAdd.Click += (s, e) =>
            {
                AddItemToCart(item, (int)numQuantity.Value);
                quantityForm.Close();
            };

            btnCancel.Click += (s, e) => quantityForm.Close();

            quantityForm.Controls.AddRange(new Control[] { lblQuantity, numQuantity, btnAdd, btnCancel });
            quantityForm.ShowDialog();
        }

        /// <summary>
        /// Adds an item to the cart with the specified quantity
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="quantity">The quantity to add</param>
        private void AddItemToCart(Item item, int quantity)
        {
            // Add the item with quantity to the CartItem list
            currentCart.Items.Add(new CartItem(item, quantity));
            ShowAlert($"Added {quantity}x {item.Name} to cart!", false);
        }

        /// <summary>
        /// Shows an alert dialog with a message
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="isError">Whether this is an error message</param>
        private void ShowAlert(string message, bool isError)
        {
            var alertForm = new Form
            {
                Text = isError ? "Error" : "Success",
                Size = new Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(33, 33, 33),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            var lblMessage = new Label
            {
                Text = message,
                Font = new Font("Arial", 12),
                ForeColor = isError ? Color.FromArgb(244, 67, 54) : Color.FromArgb(76, 175, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(260, 60)
            };

            var btnOK = new Button
            {
                Text = "OK",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(100, 80),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat
            };

            btnOK.Click += (s, e) => alertForm.Close();

            alertForm.Controls.AddRange(new Control[] { lblMessage, btnOK });
            alertForm.ShowDialog();
        }

        /// <summary>
        /// Handles back button click - closes the form
        /// </summary>
        /// <param name="sender">The back button</param>
        /// <param name="e">Event arguments</param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles view cart button click - opens cart form
        /// </summary>
        /// <param name="sender">The view cart button</param>
        /// <param name="e">Event arguments</param>
        private void BtnViewCart_Click(object sender, EventArgs e)
        {
            if (currentCart.IsEmpty())
            {
                ShowAlert("Your cart is empty!", true);
                return;
            }

            var cartForm = new CartForm(currentCart, customerId);
            cartForm.ShowDialog();
        }

        /// <summary>
        /// Handles checkout button click
        /// </summary>
        /// <param name="sender">The checkout button</param>
        /// <param name="e">Event arguments</param>
        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (currentCart.IsEmpty())
            {
                ShowAlert("Your cart is empty!", true);
                return;
            }

            var checkoutForm = new CheckoutForm(currentCart, customerId);
            checkoutForm.OrderPlacedEvent += CheckoutForm_OrderPlaced; // Subscribe to order placed event
            checkoutForm.ShowDialog();
        }

        /// <summary>
        /// Handles the event when an order is placed in checkout
        /// </summary>
        /// <param name="order">The placed order</param>
        private void CheckoutForm_OrderPlaced(Order order)
        {
            if (order != null)
            {
                // Start monitoring the order for status updates
                StartMonitoringOrder(order, $"customer{customerId}@example.com");
                
                // Show success message
                ShowAlert($"Order #{order.OrderID} placed successfully! Monitoring for updates...", false);
                
                // Refresh the notification display
                DisplayNotifications();
            }
        }

        /// <summary>
        /// Creates and configures the notification panel
        /// </summary>
        private void CreateNotificationPanel()
        {
            notificationPanel = new Panel
            {
                Location = new Point(850, 270), // Position it to the right of the menu
                Size = new Size(500, 450), // Adjust size as needed
                BackColor = Color.FromArgb(255, 255, 240), // Light yellow background to make it stand out
                BorderStyle = BorderStyle.Fixed3D, // Add 3D border to make it more visible
                Visible = true,
                Enabled = true
            };

            // Add a colored border around the notification panel
            notificationPanel.Paint += (sender, e) => {
                using (var pen = new Pen(Color.FromArgb(52, 152, 219), 3)) // Blue border
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, notificationPanel.Width - 1, notificationPanel.Height - 1);
                }
            };

            // Notification title with background
            lblNotificationTitle = new Label
            {
                Text = "🔔 LIVE ORDER UPDATES 🔔",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219), // Blue background
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 10),
                Size = new Size(480, 30)
            };

            // Clear notifications button
            btnClearNotifications = CreateStyledButton("🗑️ Clear All", warningRed, new Point(10, 45), new Size(480, 40));
            btnClearNotifications.Click += BtnClearNotifications_Click;

            // Notification stats button
            btnNotificationStats = CreateStyledButton("📊 Show Stats", primaryBlue, new Point(10, 90), new Size(480, 40));
            btnNotificationStats.Click += BtnNotificationStats_Click;

            // Demo status update button
            var btnDemoUpdate = CreateStyledButton("🎮 Demo Status Update", successGreen, new Point(10, 135), new Size(480, 40));
            btnDemoUpdate.Click += BtnDemoUpdate_Click;

            // Notification list
            lstNotifications = new ListBox
            {
                Location = new Point(10, 185),
                Size = new Size(480, 255),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                ForeColor = darkGray,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = true,
                Enabled = true
            };

            // Add all controls to the notification panel
            notificationPanel.Controls.AddRange(new Control[] {
                lblNotificationTitle, btnClearNotifications, btnNotificationStats, btnDemoUpdate, lstNotifications
            });
        }

        /// <summary>
        /// Handles clear notifications button click
        /// </summary>
        /// <param name="sender">The clear notifications button</param>
        /// <param name="e">Event arguments</param>
        private void BtnClearNotifications_Click(object sender, EventArgs e)
        {
            lstNotifications.Items.Clear();
            ShowAlert("All notifications cleared!", false);
        }

        /// <summary>
        /// Handles notification stats button click
        /// </summary>
        /// <param name="sender">The notification stats button</param>
        /// <param name="e">Event arguments</param>
        private void BtnNotificationStats_Click(object sender, EventArgs e)
        {
            var stats = notificationService.GetNotificationStats();
            MessageBox.Show(stats, "Notification Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Handles demo status update button click
        /// </summary>
        /// <param name="sender">The demo status update button</param>
        /// <param name="e">Event arguments</param>
        private void BtnDemoUpdate_Click(object sender, EventArgs e)
        {
            // Simulate an order status update for a random order
            var orders = notificationService.GetMonitoredOrders();
            if (orders.Count > 0)
            {
                var randomOrder = orders[new Random().Next(orders.Count)];
                var newStatus = randomOrder.OrderStatus switch
                {
                    "Preparing" => "Ready for Pickup",
                    "Ready for Pickup" => "Completed",
                    "Completed" => "Preparing",
                    _ => "Preparing" // Default to preparing
                };
                notificationService.UpdateOrderStatus(randomOrder.OrderID, newStatus);
                ShowAlert($"Simulated status update for Order #{randomOrder.OrderID}: {newStatus}", false);
                DisplayNotifications();
            }
            else
            {
                ShowAlert("No orders to simulate update for.", true);
            }
        }

        /// <summary>
        /// Displays notifications in the notification listbox
        /// </summary>
        private void DisplayNotifications()
        {
            lstNotifications.Items.Clear();
            
            // Get monitored orders and display their status
            var monitoredOrders = notificationService.GetMonitoredOrders();
            if (monitoredOrders.Count > 0)
            {
                foreach (var order in monitoredOrders)
                {
                    var notificationText = $"Order #{order.OrderID}: {order.OrderStatus}";
                    lstNotifications.Items.Add(notificationText);
                }
            }
            else
            {
                lstNotifications.Items.Add("No active orders being monitored");
            }
            
            lstNotifications.Refresh();
        }

        /// <summary>
        /// Initializes notifications and starts the notification display loop
        /// </summary>
        private void InitializeNotifications()
        {
            // Debug: Ensure notification panel is visible
            if (notificationPanel != null)
            {
                notificationPanel.Visible = true;
                notificationPanel.BringToFront();
                Console.WriteLine($"Notification panel created at: {notificationPanel.Location}, Size: {notificationPanel.Size}");
            }
            else
            {
                Console.WriteLine("ERROR: Notification panel is null!");
            }

            // Set up a timer to refresh notifications every 5 seconds
            var notificationTimer = new System.Windows.Forms.Timer
            {
                Interval = 5000, // 5 seconds
                Enabled = true
            };
            
            notificationTimer.Tick += (sender, e) => {
                DisplayNotifications();
            };
            
            DisplayNotifications(); // Initial display
        }

        /// <summary>
        /// Starts monitoring an order for status updates
        /// </summary>
        /// <param name="order">The order to monitor</param>
        /// <param name="customerEmail">Customer email for notifications</param>
        public void StartMonitoringOrder(Order order, string customerEmail = "customer@example.com")
        {
            if (order != null)
            {
                notificationService.StartMonitoringOrder(order, customerEmail);
                DisplayNotifications();
                ShowAlert($"Started monitoring Order #{order.OrderID}", false);
            }
        }
    }
} 