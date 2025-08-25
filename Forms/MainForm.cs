using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OnlineOrderingSystem.Forms;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Forms
{
    /// <summary>
    /// Modern main application form that serves as the central hub
    /// Features a visually appealing design with gradient background and card-style layout
    /// </summary>
    public class MainForm : Form
    {
        private int customerId;
        private Panel cardPanel;
        private Label lblLogo;
        private Label lblTagline;

        // Color scheme
        private Color primaryBlue = Color.FromArgb(52, 152, 219);
        private Color successGreen = Color.FromArgb(46, 204, 113);
        private Color warningRed = Color.FromArgb(231, 76, 60);
        private Color softBeige = Color.FromArgb(255, 248, 240);
        private Color warmOrange = Color.FromArgb(255, 165, 0);
        private Color darkGray = Color.FromArgb(52, 73, 94);

        /// <summary>
        /// Initializes the modern main form
        /// </summary>
        public MainForm(int customerId = 0)
        {
            this.customerId = customerId;
            InitializeComponent();
        }

        /// <summary>
        /// Sets up the form properties and creates modern UI controls
        /// </summary>
        private void InitializeComponent()
        {
            // Form configuration
            this.Text = "Tasty Eats - Main Menu";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = softBeige;

            CreateControls();
        }

        /// <summary>
        /// Creates and configures all modern UI controls for the main form
        /// </summary>
        private void CreateControls()
        {
            // Create gradient background
            this.Paint += (sender, e) => DrawGradientBackground(e.Graphics);

            // Main card container with shadow effect
            cardPanel = new Panel
            {
                Location = new Point(100, 50),
                Size = new Size(800, 550),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect to card
            cardPanel.Paint += (sender, e) => DrawCardShadow(e.Graphics);

            // Logo and branding
            lblLogo = new Label
            {
                Text = "🍽️ TASTY EATS",
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = warmOrange,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 40),
                Size = new Size(700, 60)
            };

            // Tagline
            lblTagline = new Label
            {
                Text = "Deliciousness at your fingertips",
                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 100),
                Size = new Size(700, 30)
            };

            // Welcome section
            var lblWelcome = new Label
            {
                Text = "Welcome to Tasty Eats!",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = darkGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 150),
                Size = new Size(700, 40)
            };

            // Clean food emojis
            var lblImagePlaceholder = new Label
            {
                Text = "🍕 🍔 🍜 🍰 — Your Favorite Foods Await!",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 200),
                Size = new Size(700, 25),
                BackColor = Color.Transparent
            };

            // Section header
            var lblNavigationHeader = new Label
            {
                Text = "🏠 Main Navigation",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = darkGray,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(75, 250),
                Size = new Size(300, 25)
            };

            // Create navigation buttons in a clean grid
            var btnMenu = CreateStyledButton("🍽️ Browse Menu", primaryBlue, new Point(75, 290), new Size(180, 60));
            var btnCart = CreateStyledButton("🛒 View Cart", successGreen, new Point(275, 290), new Size(180, 60));
            var btnHistory = CreateStyledButton("📋 Order History", warmOrange, new Point(475, 290), new Size(180, 60));
            
            var btnProfile = CreateStyledButton("👤 Profile", darkGray, new Point(75, 370), new Size(180, 60));
            var btnExit = CreateStyledButton("🚪 Exit", warningRed, new Point(275, 370), new Size(180, 60));

            // Live banner
            var pnlLiveBanner = new Panel
            {
                Location = new Point(50, 450),
                Size = new Size(700, 50),
                BackColor = successGreen,
                BorderStyle = BorderStyle.None
            };

            var lblLiveBannerText = new Label
            {
                Text = "🎉 LIVE: Special offers only today 20% 🎉",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            pnlLiveBanner.Controls.Add(lblLiveBannerText);

            // Event handlers for buttons
            btnMenu.Click += (sender, e) => OpenMenuForm();
            btnCart.Click += (sender, e) => OpenCartForm();
            btnHistory.Click += (sender, e) => OpenOrderHistoryForm();
            btnProfile.Click += (sender, e) => ShowProfileDialog();
            btnExit.Click += (sender, e) => Application.Exit();

            // Add controls to the card panel
            cardPanel.Controls.Add(lblLogo);
            cardPanel.Controls.Add(lblTagline);
            cardPanel.Controls.Add(lblWelcome);
            cardPanel.Controls.Add(lblImagePlaceholder);
            cardPanel.Controls.Add(lblNavigationHeader);
            cardPanel.Controls.Add(btnMenu);
            cardPanel.Controls.Add(btnCart);
            cardPanel.Controls.Add(btnHistory);
            cardPanel.Controls.Add(btnProfile);
            cardPanel.Controls.Add(btnExit);
            cardPanel.Controls.Add(pnlLiveBanner);

            // Add card panel to form
            this.Controls.Add(cardPanel);

            // Add keyboard navigation
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
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
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = backgroundColor,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                TabStop = true
            };

            // Remove border and add rounded appearance
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.BorderColor = backgroundColor;

            // Add hover effects with smoother transitions
            button.MouseEnter += (sender, e) => {
                button.BackColor = ControlPaint.Light(backgroundColor, 0.2f);
                button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            };
            button.MouseLeave += (sender, e) => {
                button.BackColor = backgroundColor;
                button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            };

            // Add focus effects for keyboard navigation
            button.GotFocus += (sender, e) => {
                button.FlatAppearance.BorderSize = 2;
                button.FlatAppearance.BorderColor = Color.White;
            };
            button.LostFocus += (sender, e) => {
                button.FlatAppearance.BorderSize = 0;
            };

            return button;
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
        /// Handles keyboard navigation for the main form
        /// </summary>
        /// <param name="sender">The form that triggered the event</param>
        /// <param name="e">Key event arguments</param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.H:
                    // Open history
                    var historyForm = new OrderHistoryForm();
                    historyForm.ShowDialog();
                    break;
                case Keys.M:
                    // Open menu
                    var menuForm = new EnhancedMenuForm();
                    menuForm.ShowDialog();
                    break;
                case Keys.P:
                    // Open profile
                    ShowProfileDialog();
                    break;
                case Keys.Escape:
                    // Exit application
                    Application.Exit();
                    break;
            }
        }

        /// <summary>
        /// Shows the profile dialog
        /// </summary>
        private void ShowProfileDialog()
        {
            MessageBox.Show(
                "Profile functionality coming soon!\n\n" +
                "Features planned:\n" +
                "• View order history\n" +
                "• Update personal information\n" +
                "• Manage preferences\n" +
                "• Loyalty points",
                "Profile",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Opens the menu form
        /// </summary>
        private void OpenMenuForm()
        {
            var menuForm = new EnhancedMenuForm(customerId);
            menuForm.ShowDialog();
        }

        /// <summary>
        /// Opens the cart form
        /// </summary>
        private void OpenCartForm()
        {
            // Create a default empty cart for demonstration
            var cart = new Cart();
            var cartForm = new CartForm(cart, customerId);
            cartForm.ShowDialog();
        }

        /// <summary>
        /// Opens the order history form
        /// </summary>
        private void OpenOrderHistoryForm()
        {
            var historyForm = new OrderHistoryForm(customerId);
            historyForm.ShowDialog();
        }


    }
} 
