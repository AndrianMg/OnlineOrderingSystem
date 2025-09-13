using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Forms
{
    /// <summary>
    /// Represents a form for customizing a selected menu item.
    /// </summary>
    /// <remarks>
    /// This form allows users to select from a list of available customizations
    /// for a specific item, dynamically updating the total price based on selections.
    /// </remarks>
    public class CustomizationForm : Form
    {
        // The menu item being customized.
        private Item selectedItem;

        // UI Controls
        private CheckedListBox? chkCustomizations;
        private Button? btnConfirm;
        private Button? btnCancel;
        private Label? lblItemName;
        private Label? lblDescription;
        private Label? lblPrice;
        private Label? lblTotalPrice;

        // Holds the names of the chosen customizations.
        private List<string> selectedCustomizations;

        /// <summary>
        /// Gets the list of selected customization names.
        /// </summary>
        public List<string> SelectedCustomizations => selectedCustomizations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizationForm"/> class.
        /// </summary>
        /// <param name="item">The item to be customized.</param>
        public CustomizationForm(Item item)
        {
            selectedItem = item;
            selectedCustomizations = new List<string>();
            InitializeComponent();
        }

        /// <summary>
        /// Initializes and configures the form's components and layout.
        /// </summary>
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

        /// <summary>
        /// Creates and configures the UI controls on the form.
        /// </summary>
        private void CreateControls()
        {
            // Title Label
            var lblTitle = new Label
            {
                Text = "Customize Your Order",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 30),
                Size = new Size(500, 30)
            };

            // Item Information Labels
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

            // Dietary Information Label (optional)
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

            // Customization Options Section
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

            // Total Price Label
            lblTotalPrice = new Label
            {
                Text = $"Total Price: £{selectedItem.Price:F2}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 165, 0),
                Location = new Point(50, 400),
                Size = new Size(300, 25)
            };

            // Action Buttons
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

            // Add all created controls to the form's control collection.
            this.Controls.AddRange(new Control[] {
                lblTitle, lblItemName, lblDescription, lblPrice,
                lblCustomizations, chkCustomizations, lblTotalPrice,
                btnConfirm, btnCancel
            });

            // Populate the list of customization options.
            PopulateCustomizations();
        }

        /// <summary>
        /// Wires up event handlers for the interactive controls.
        /// </summary>
        private void SetupEventHandlers()
        {
            if (btnConfirm != null)
                btnConfirm.Click += BtnConfirm_Click;
            if (btnCancel != null)
                btnCancel.Click += BtnCancel_Click;
            if (chkCustomizations != null)
                chkCustomizations.ItemCheck += ChkCustomizations_ItemCheck;
        }

        /// <summary>
        /// Populates the CheckedListBox with customization options for the selected item.
        /// </summary>
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
                    if (chkCustomizations != null)
                    {
                        chkCustomizations.Items.Add(displayText);
                    }
                }
            }
            else
            {
                // Handle items with no customization options.
                if (chkCustomizations != null)
                {
                    chkCustomizations.Items.Add("No customizations available for this item");
                    chkCustomizations.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Handles the ItemCheck event of the chkCustomizations CheckedListBox.
        /// Recalculates and updates the total price in real-time as options are checked or unchecked.
        /// </summary>
        private void ChkCustomizations_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (chkCustomizations == null)
                return;

            // Start with the base price of the item.
            var totalPrice = selectedItem.Price;

            // Iterate through all customization options to calculate the new total price.
            for (int i = 0; i < chkCustomizations.Items.Count; i++)
            {
                // Check the future state of the item being changed.
                if (i == e.Index)
                {
                    if (e.NewValue == CheckState.Checked)
                    {
                        var option = selectedItem.CustomizationOptions[i];
                        totalPrice += option.AdditionalCost;
                    }
                }
                // For all other items, check their current checked state.
                else if (chkCustomizations.GetItemChecked(i))
                {
                    var option = selectedItem.CustomizationOptions[i];
                    totalPrice += option.AdditionalCost;
                }
            }

            // Update the total price label.
            if (lblTotalPrice != null)
                lblTotalPrice.Text = $"Total Price: £{totalPrice:F2}";
        }

        /// <summary>
        /// Handles the Click event of the Confirm button.
        /// Finalizes the customization selections and closes the form.
        /// </summary>
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            selectedCustomizations.Clear();

            if (chkCustomizations != null)
            {
                // Gather all checked customization options.
                for (int i = 0; i < chkCustomizations.Items.Count; i++)
                {
                    if (chkCustomizations.GetItemChecked(i))
                    {
                        var option = selectedItem.CustomizationOptions[i];
                        selectedCustomizations.Add(option.Name);
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Cancel button.
        /// Closes the form without saving any customization changes.
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}