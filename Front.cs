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
    public partial class Front : Form
    {
        // Store the selected mode (AI or Player)
        private string selectedMode = "";

        // Store the selected play mode (playby2, playby3, playby4)
        private string selectedPlayMode = "";

        /// Constructor for the Front form.
        /// Initializes the form components.
        public Front()
        {
            InitializeComponent();
        }

        /// Event handler for the "Play" button.
        /// Validates the selected mode and play mode, then redirects to the game form.
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if a mode has been selected
            if (string.IsNullOrEmpty(selectedMode))
            {
                MessageBox.Show("You must select a mode before proceeding.", "Mode Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if a play mode has been selected
            if (string.IsNullOrEmpty(selectedPlayMode))
            {
                MessageBox.Show("You must select a play mode before proceeding.", "Play Mode Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Redirect to Form1.cs with the selected mode and play mode
            Form1 gameForm = new Form1(selectedMode, selectedPlayMode);
            gameForm.Show();
            this.Hide(); // Hide the Front form
        }

        /// Event handler for the "Exit" button.
        /// Exits the application.
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// Event handler for the "Rules" button.
        /// Displays the rules and mechanics of the Snake and Ladder game.
        private void button3_Click(object sender, EventArgs e)
        {
            string rules = "Snake and Ladder Game Instructions & Rules:\n\n" +
                           "Game Setup:\n" +
                           "1. Choose the number of players (2 to 4).\n" +
                           "2. Select whether to play against AI or real players.\n" +
                           "3. Once selected, you will be redirected to the home screen.\n" +
                           "4. Click 'Play' to begin the game.\n\n" +
                           "Game Rules:\n" +
                           "1. Players take turns rolling a dice to move their token forward.\n" +
                           "2. If a player lands on the bottom of a ladder, they climb up to the top.\n" +
                           "3. If a player lands on the head of a snake, they slide down to the tail.\n" +
                           "4. The first player to reach or exceed square 100 wins immediately.\n\n" +
                           "Additional Game Features:\n" +
                           "- AI Opponents: Play against computer-controlled players.\n" +
                           "- Dice Rolling System: Click a button to roll.\n" +
                           "- Automatic Movement: Tokens move based on dice rolls.\n" +
                           "Winning Condition:\n" +
                           "1. The first player to reach square 100 wins immediately.\n" +
                           "2. If multiple players reach 100 in the same round, the one who rolled first wins.\n";

            MessageBox.Show(rules, "Game Rules & Mechanics", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// Event handler for the "Mode Selection" button.
        /// Opens a dialog to let the player choose between AI or Player mode and the number of players.
        private void button4_Click(object sender, EventArgs e)
        {
            using (var modeForm = new ModeSelectionForm())
            {
                if (modeForm.ShowDialog() == DialogResult.OK)
                {
                    selectedMode = modeForm.SelectedMode;
                    selectedPlayMode = modeForm.SelectedPlayMode;
                }
            }
        }

        /// Event handler for the label click (no functionality).
        private void label1_Click(object sender, EventArgs e) { }

        /// Event handler for the form load event.
        /// Configures the form's appearance and loads GIFs for the snake and ladder animations.
        private void Front_Load(object sender, EventArgs e)
        {
            // Prevent resizing and hide the close (X) and maximize buttons
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false; // Hides entire control buttons (X, -, ⬜)
            this.MinimizeBox = true; // Enable only the Minimize button

            // Suspend layout to prevent unnecessary redraws
            this.SuspendLayout();

            // Load and play the snake GIF
            LoadGif(snakegif, @"C:\Users\HP\Downloads\jv\snake ladder\output-onlinegiftools.gif");

            // Load and play the ladder GIF
            LoadGif(laddergif, @"C:\Users\HP\Downloads\jv\snake ladder\output-onlinegiftools (2).gif");

            // Resume layout after all changes are made
            this.ResumeLayout();
        }

        /// <summary>
        /// Loads a GIF into a PictureBox control.
        /// </summary>
        /// <param name="pictureBox">The PictureBox control to load the GIF into.</param>
        /// <param name="gifPath">The file path of the GIF.</param>
        private void LoadGif(PictureBox pictureBox, string gifPath)
        {
            try
            {
                if (System.IO.File.Exists(gifPath))
                {
                    pictureBox.Image = Image.FromFile(gifPath);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Adjust the image size mode
                }
                else
                {
                    MessageBox.Show($"GIF file not found: {gifPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the GIF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// Event handler for the snake GIF click (no functionality).
        private void snakegif_Click(object sender, EventArgs e) { }

        /// Event handler for the ladder GIF click (no functionality).
        private void laddergif_Click(object sender, EventArgs e) { }
    }
}