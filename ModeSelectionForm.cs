using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_and_ladder
{
    public partial class ModeSelectionForm : Form
    {
        // Property to store the selected mode (AI or Player)
        public string SelectedMode { get; private set; } = "";

        // Property to store the selected play mode (playby2, playby3, playby4)
        public string SelectedPlayMode { get; private set; } = "";

        /// Constructor for the ModeSelectionForm.
        /// Initializes the form components and attaches the Load event handler.
        public ModeSelectionForm()
        {
            InitializeComponent();
            this.Load += ModeSelection_Load; // Attach the Load event handler
        }

        /// Event handler for the form load event.
        /// Configures the form's appearance and loads a GIF for the background.
        private void ModeSelection_Load(object sender, EventArgs e)
        {
            // Prevent resizing and hide the close (X) and maximize buttons
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false; // Hides entire control buttons (X, -, ⬜)
            this.MinimizeBox = true; // Enable only the Minimize button

            try
            {
                // Load and play the snake GIF
                string gifPath = @"C:\Users\HP\Downloads\jv\snake ladder\Untitledvideo-MadewithClipchamp1-ezgif.com-gif-maker.gif";
                if (System.IO.File.Exists(gifPath))
                {
                    pictureBox1.Image = Image.FromFile(gifPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Adjust the image size mode
                }
                else
                {
                    MessageBox.Show("GIF file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the GIF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// Event handler for the "AI" button.
        /// Sets the selected mode to AI and shows the play mode selection.
        private void buttonAI_Click(object sender, EventArgs e)
        {
            SelectedMode = "AI";
            ShowPlayModeSelection();
        }

        /// Event handler for the "Player" button.
        /// Sets the selected mode to Player and shows the play mode selection.
        private void buttonPlayer_Click(object sender, EventArgs e)
        {
            SelectedMode = "Player";
            ShowPlayModeSelection();
        }

        /// Validates the selected play mode and closes the form with DialogResult.OK.
        private void ShowPlayModeSelection()
        {
            // Ensure the player selects a play mode before proceeding
            if (string.IsNullOrEmpty(SelectedPlayMode))
            {
                MessageBox.Show("You must select a play mode (2, 3, or 4) before proceeding in selecting AI or Players.", "Play Mode Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Close the form and return OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// Event handler for the "2 Players" button.
        /// Sets the selected play mode to playby2 and updates the UI.
        private void playby2_Click(object sender, EventArgs e)
        {
            SelectedPlayMode = "playby2";
            UpdatePlayModeSelectionUI();
        }

        /// Event handler for the "3 Players" button.
        /// Sets the selected play mode to playby3 and updates the UI.
        private void playby3_Click(object sender, EventArgs e)
        {
            SelectedPlayMode = "playby3";
            UpdatePlayModeSelectionUI();
        }

        /// Event handler for the "4 Players" button.
        /// Sets the selected play mode to playby4 and updates the UI.
        private void playby4_Click(object sender, EventArgs e)
        {
            SelectedPlayMode = "playby4";
            UpdatePlayModeSelectionUI();
        }

        /// Updates the UI to reflect the selected play mode.
        private void UpdatePlayModeSelectionUI()
        {
            // Update button colors to indicate the selected play mode
            playby2.BackColor = SelectedPlayMode == "playby2" ? Color.LightGreen : SystemColors.Control;
            playby3.BackColor = SelectedPlayMode == "playby3" ? Color.LightGreen : SystemColors.Control;
            playby4.BackColor = SelectedPlayMode == "playby4" ? Color.LightGreen : SystemColors.Control;
        }

        /// Event handler for the "How to Play" button.
        /// Displays the instructions and rules for the Snake and Ladder game.
        private void How_Click(object sender, EventArgs e)
        {
            string instructions = "Game Setup Instructions:\n\n" +
                                  "1. Choose the number of players (2 to 4).\n" +
                                  "2. Select whether to play against AI or real players.\n" +
                                  "3. Once selected, you will be redirected to the home screen.\n" +
                                  "4. Click 'Play' to begin the game.\n\n" +
                                  "Rules of Snake and Ladder:\n\n" +
                                  "1. Players take turns rolling a dice to move their token.\n" +
                                  "2. Landing at the bottom of a ladder allows you to climb up.\n" +
                                  "3. Landing on a snake's head makes you slide down to its tail.\n" +
                                  "4. The first player to reach square 100 wins the game.";

            MessageBox.Show(instructions, "Game Instructions & Rules", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// Event handler for the "Exit" button.
        /// Closes the current form and returns to the Front form.
        private void EXIT_Click(object sender, EventArgs e)
        {
            Front frontForm = new Front();
            this.Hide(); // Hide the current form
            this.Close(); // Close the current form
        }

        /// Event handler for the PictureBox click (no functionality).
        private void pictureBox1_Click(object sender, EventArgs e) { }

        /// Event handler for the form load event (no functionality).
        private void Mode_Load(object sender, EventArgs e) { }
    }
}