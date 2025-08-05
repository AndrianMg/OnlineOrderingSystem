using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Forms
{
    public class CustomizationForm : Form
    {
        private Item selectedItem;
        private CheckedListBox chkCustomizations;
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblItemName;
        private Label lblDescription;
        private Label lblPrice;
        private Label lblTotalPrice;
        private List<string> selectedCustomizations;

        public List<string> SelectedCustomizations => selectedCustomizations;

        public CustomizationForm(Item item)
        {
            selectedItem = item;
            selectedCustomizations = new List<string>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Tasty Eats - Customize Your Order";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(33, 33, 33);
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
                Text = "Customize Your Order",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(500, 30)
            };

            // Item Information
            lblItemName = new Label
            {
                Text = $"Item: {selectedItem.Name}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(50, 80),
                Size = new Size(500, 25)
            };

            lblDescription = new Label
            {
                Text = $"Description: {selectedItem.Description}",
                Font = new Font("Arial", 10),
                ForeColor = Color.White,
                Location = new Point(50, 110),
                Size = new Size(500, 20)
            };

            lblPrice = new Label
            {
                Text = $"Base Price: £{selectedItem.Price:F2}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 140),
                Size = new Size(200, 25)
            };

            // Dietary Information
            if (selectedItem.DietaryTags.Count > 0)
            {
                var lblDietary = new Label
                {
                    Text = $"Dietary: {selectedItem.GetDietaryTagsString()}",
                    Font = new Font("Arial", 10),
                    ForeColor = Color.FromArgb(0, 255, 0),
                    Location = new Point(50, 165),
                    Size = new Size(500, 20)
                };
                this.Controls.Add(lblDietary);
            }

            // Customization Options
            var lblCustomizations = new Label
            {
                Text = "Available Customizations:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 200),
                Size = new Size(200, 25)
            };

            chkCustomizations = new CheckedListBox
            {
                Location = new Point(50, 230),
                Size = new Size(500, 150),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 11)
            };

            // Total Price
            lblTotalPrice = new Label
            {
                Text = $"Total Price: £{selectedItem.Price:F2}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 165, 0),
                Location = new Point(50, 400),
                Size = new Size(300, 25)
            };

            // Buttons
            btnConfirm = new Button
            {
                Text = "Confirm Customizations",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.FromArgb(0, 150, 0),
                Location = new Point(50, 440),
                Size = new Size(180, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 100, 100),
                Location = new Point(250, 440),
                Size = new Size(120, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                lblTitle, lblItemName, lblDescription, lblPrice,
                lblCustomizations, chkCustomizations, lblTotalPrice,
                btnConfirm, btnCancel
            });

            // Populate customization options
            PopulateCustomizations();
        }

        private void SetupEventHandlers()
        {
            btnConfirm.Click += BtnConfirm_Click;
            btnCancel.Click += BtnCancel_Click;
            chkCustomizations.ItemCheck += ChkCustomizations_ItemCheck;
        }

        private void PopulateCustomizations()
        {
            if (selectedItem.CustomizationOptions.Count > 0)
            {
                foreach (var option in selectedItem.CustomizationOptions)
                {
                    var displayText = $"{option.Name}";
                    if (option.AdditionalCost > 0)
                    {
                        displayText += $" (+£{option.AdditionalCost:F2})";
                    }
                    if (!string.IsNullOrEmpty(option.Description))
                    {
                        displayText += $" - {option.Description}";
                    }
                    chkCustomizations.Items.Add(displayText);
                }
            }
            else
            {
                chkCustomizations.Items.Add("No customizations available for this item");
                chkCustomizations.Enabled = false;
            }
        }

        private void ChkCustomizations_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Calculate total price based on selected customizations
            var totalPrice = selectedItem.Price;
            
            for (int i = 0; i < chkCustomizations.Items.Count; i++)
            {
                if (i == e.Index)
                {
                    // This is the item being checked/unchecked
                    if (e.NewValue == CheckState.Checked)
                    {
                        var option = selectedItem.CustomizationOptions[i];
                        totalPrice += option.AdditionalCost;
                    }
                }
                else if (chkCustomizations.GetItemChecked(i))
                {
                    // This is an already checked item
                    var option = selectedItem.CustomizationOptions[i];
                    totalPrice += option.AdditionalCost;
                }
            }

            lblTotalPrice.Text = $"Total Price: £{totalPrice:F2}";
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            selectedCustomizations.Clear();
            
            for (int i = 0; i < chkCustomizations.Items.Count; i++)
            {
                if (chkCustomizations.GetItemChecked(i))
                {
                    var option = selectedItem.CustomizationOptions[i];
                    selectedCustomizations.Add(option.Name);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 