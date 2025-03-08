using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Snake_and_ladder
{
    public partial class Form1 : Form
    {
        // Random number generator for dice rolls
        private Random random = new Random();

        // Array to store dice roll values for Player 1, Player 2, Player 3, and Player 4
        private int[] diceRolls = new int[4] { 1, 1, 1, 1 };

        // Array to store positions of all players (Player 1, Player 2, Player 3, and Player 4)
        private int[] playerPositions = new int[4] { 1, 1, 1, 1 };

        // Dictionary to map board positions (1-100) to PictureBox controls
        private Dictionary<int, PictureBox> positionToPictureBox = new Dictionary<int, PictureBox>();

        // Index of the current player (0 for Player 1, 1 for Player 2, etc.)
        private int currentPlayer = 0;

        // Stores the selected mode (AI or Player)
        private string selectedMode;

        // Stores the selected play mode (playby2, playby3, playby4)
        private string selectedPlayMode;

        // Flag to check if the game has ended
        private bool gameEnded = false;

        // Constructor for Form1. Initializes the board and sets up the game.
        public Form1(string mode, string playMode)
        {
            InitializeComponent(); // Initialize the form components
            InitializeBoard(); // Map board positions to PictureBoxes
            selectedMode = mode; // Set the selected mode
            selectedPlayMode = playMode; // Set the selected play mode

            // Set the initial currentPlayer to Player 1
            currentPlayer = 0;

            // Display the selected mode and play mode
            MessageBox.Show($"Selected Mode: {selectedMode}\nSelected Play Mode: {selectedPlayMode}", "Game Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Hide UI elements based on the number of players
            ConfigureUIForPlayers();
        }

        // Initializes the board by mapping positions (1-100) to PictureBox controls.
        private void InitializeBoard()
        {
            for (int i = 1; i <= 100; i++)
            {
                // Find the PictureBox control for the current position
                var pictureBox = Controls.Find($"pictureBox{i}a", true).FirstOrDefault() as PictureBox;
                if (pictureBox != null)
                {
                    // Map the position to the PictureBox
                    positionToPictureBox[i] = pictureBox;
                }
            }
        }

        // Dictionary to store ladder positions (key: bottom, value: top)
        private Dictionary<int, int> stairs = new Dictionary<int, int>()
        {
            { 2, 38 },  { 7, 14 },  { 8, 31 },  { 15, 26 },
            { 21, 42 }, { 28, 84 }, { 36, 44 }, { 51, 67 },
            { 71, 91 }, { 78, 98 }, { 87, 94 }
        };

        // Dictionary to store snake positions (key: head, value: tail)
        private Dictionary<int, int> snakes = new Dictionary<int, int>()
        {
            { 16, 6 },  { 46, 25 }, { 49, 11 }, { 62, 19 },
            { 64, 60 }, { 74, 53 }, { 89, 68 }, { 92, 88 },
            { 95, 75 }, { 99, 80 }
        };

        // Configures the UI based on the number of players.
        private void ConfigureUIForPlayers()
        {
            // Determine the number of players based on the selected play mode
            int numberOfPlayers = selectedPlayMode == "playby2" ? 2 :
                                  selectedPlayMode == "playby3" ? 3 : 4;

            // Hide UI elements for players 3 and 4 if not needed
            if (numberOfPlayers < 3)
            {
                dice3.Visible = false;
                ROLL3.Visible = false;
                player3.Visible = false;
                label3.Visible = false;
            }

            if (numberOfPlayers < 4)
            {
                dice4.Visible = false;
                ROLL4.Visible = false;
                player4.Visible = false;
                label4.Visible = false;
            }
        }

        // Updates the state of the roll buttons based on the current player and game state.
        private void UpdateButtonStates()
        {
            // Enable the roll button for the current player and disable it for others
            ROLL1.Enabled = (currentPlayer == 0 && !gameEnded);
            ROLL2.Enabled = (currentPlayer == 1 && !gameEnded);
            ROLL3.Enabled = (currentPlayer == 2 && !gameEnded);
            ROLL4.Enabled = (currentPlayer == 3 && !gameEnded);

            // Automatically roll the dice for AI players
            if (selectedMode == "AI" && currentPlayer != 0 && !gameEnded)
            {
                RollDice(currentPlayer); // Roll dice for the bot (Player 2, Player 3, or Player 4)
            }
        }

        // Event handler for Player 1's roll button.
        private void ROLL1_Click(object sender, EventArgs e)
        {
            RollDice(0); // Roll dice for Player 1
        }

        // Event handler for Player 2's roll button.
        private void ROLL2_Click(object sender, EventArgs e)
        {
            RollDice(1); // Roll dice for Player 2
        }

        // Event handler for Player 3's roll button.
        private void ROLL3_Click(object sender, EventArgs e)
        {
            RollDice(2); // Roll dice for Player 3
        }

        // Event handler for Player 4's roll button.
        private void ROLL4_Click(object sender, EventArgs e)
        {
            RollDice(3); // Roll dice for Player 4
        }

        // Rolls the dice for the specified player and updates the game state.
        private void RollDice(int playerIndex)
        {
            // Check if the game has ended
            if (gameEnded)
            {
                MessageBox.Show("The game has ended. Please restart the game to play again.");
                return;
            }

            // Check if it's the current player's turn
            if (playerIndex != currentPlayer)
            {
                MessageBox.Show($"It's not Player {playerIndex + 1}'s turn!");
                return;
            }

            // Generate a random dice roll (1 to 6)
            diceRolls[playerIndex] = random.Next(1, 7);

            // Update the dice image
            UpdateDiceImage(playerIndex);

            // Move the player
            MovePlayer(playerIndex, diceRolls[playerIndex]);

            // Switch to the next player
            int numberOfPlayers = selectedPlayMode == "playby2" ? 2 :
                                  selectedPlayMode == "playby3" ? 3 : 4;
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;

            // Update button states
            UpdateButtonStates();
        }

        // Updates the dice image for the specified player.
        private void UpdateDiceImage(int playerIndex)
        {
            // Determine which PictureBox to update based on the player index
            PictureBox dicePictureBox = playerIndex == 0 ? dice :
                                        playerIndex == 1 ? dice2 :
                                        playerIndex == 2 ? dice3 : dice4;

            // Set the dice image based on the roll value
            switch (diceRolls[playerIndex])
            {
                case 1:
                    dicePictureBox.Image = Properties.Resources.dice1;
                    break;
                case 2:
                    dicePictureBox.Image = Properties.Resources.dice2;
                    break;
                case 3:
                    dicePictureBox.Image = Properties.Resources.dice3;
                    break;
                case 4:
                    dicePictureBox.Image = Properties.Resources.dice4;
                    break;
                case 5:
                    dicePictureBox.Image = Properties.Resources.dice5;
                    break;
                case 6:
                    dicePictureBox.Image = Properties.Resources.dice6;
                    break;
            }
        }

        // Moves the specified player by the given number of steps.
        private void MovePlayer(int playerIndex, int steps)
        {
            // Calculate the new position
            int newPosition = playerPositions[playerIndex] + steps;

            // Ensure the new position does not exceed 100
            if (newPosition > 100)
            {
                newPosition = 100;
            }

            // Update the player's position
            playerPositions[playerIndex] = newPosition;

            // Check for stairs or snakes
            if (stairs.ContainsKey(newPosition))
            {
                newPosition = stairs[newPosition]; // Move up the stair
                playerPositions[playerIndex] = newPosition;
                MessageBox.Show($"Player {playerIndex + 1} climbed a stair to {newPosition}!");
            }
            else if (snakes.ContainsKey(newPosition))
            {
                newPosition = snakes[newPosition]; // Slide down the snake
                playerPositions[playerIndex] = newPosition;
                MessageBox.Show($"Player {playerIndex + 1} slid down a snake to {newPosition}!");
            }

            // Move the player's PictureBox to the new position
            PictureBox playerPictureBox = playerIndex == 0 ? player1 :
                                          playerIndex == 1 ? player2 :
                                          playerIndex == 2 ? player3 : player4;

            PictureBox targetPictureBox = positionToPictureBox[newPosition];

            // Center the player token on the square
            playerPictureBox.Left = targetPictureBox.Left + (targetPictureBox.Width - playerPictureBox.Width) / 2;
            playerPictureBox.Top = targetPictureBox.Top + (targetPictureBox.Height - playerPictureBox.Height) / 2;

            // Check for a win
            if (newPosition == 100)
            {
                MessageBox.Show($"Player {playerIndex + 1} wins!");
                gameEnded = true; // Set the gameEnded flag to true

                // Show the "Restart Game" button
                button3.Visible = true;
            }
        }

        // Resets the game to its initial state.
        private void ResetGame()
        {
            // Reset player positions and dice rolls
            for (int i = 0; i < 4; i++)
            {
                playerPositions[i] = 1;
                diceRolls[i] = 1;
            }

            // Move players back to the starting position and center them
            PictureBox startingSquare = positionToPictureBox[1];
            player1.Left = startingSquare.Left + (startingSquare.Width - player1.Width) / 2;
            player1.Top = startingSquare.Top + (startingSquare.Height - player1.Height) / 2;
            player2.Left = startingSquare.Left + (startingSquare.Width - player2.Width) / 2;
            player2.Top = startingSquare.Top + (startingSquare.Height - player2.Height) / 2;
            player3.Left = startingSquare.Left + (startingSquare.Width - player3.Width) / 2;
            player3.Top = startingSquare.Top + (startingSquare.Height - player3.Height) / 2;
            player4.Left = startingSquare.Left + (startingSquare.Width - player4.Width) / 2;
            player4.Top = startingSquare.Top + (startingSquare.Height - player4.Height) / 2;

            // Reset dice images
            dice.Image = Properties.Resources.dice1;
            dice2.Image = Properties.Resources.dice1;
            dice3.Image = Properties.Resources.dice1;
            dice4.Image = Properties.Resources.dice1;

            // Reset turn to Player 1
            currentPlayer = 0;
            UpdateButtonStates();

            // Hide the "Restart Game" button
            button3.Visible = false;

            // Reset the gameEnded flag
            gameEnded = false;
        }

        // Event handler for the form load event.
        private void Form1_Load(object sender, EventArgs e)
        {
            // Prevent resizing and hide the close (X) and maximize buttons
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false; // Hides entire control buttons (X, -, ⬜)
            this.MinimizeBox = true; // Enable only the Minimize button

            // Set PictureBox size modes
            dice.SizeMode = PictureBoxSizeMode.StretchImage;
            dice2.SizeMode = PictureBoxSizeMode.StretchImage;
            dice3.SizeMode = PictureBoxSizeMode.StretchImage;
            dice4.SizeMode = PictureBoxSizeMode.StretchImage;

            // Initialize player positions and center them on the starting square
            PictureBox startingSquare = positionToPictureBox[1];
            player1.Left = startingSquare.Left + (startingSquare.Width - player1.Width) / 2;
            player1.Top = startingSquare.Top + (startingSquare.Height - player1.Height) / 2;
            player2.Left = startingSquare.Left + (startingSquare.Width - player2.Width) / 2;
            player2.Top = startingSquare.Top + (startingSquare.Height - player2.Height) / 2;
            player3.Left = startingSquare.Left + (startingSquare.Width - player3.Width) / 2;
            player3.Top = startingSquare.Top + (startingSquare.Height - player3.Height) / 2;
            player4.Left = startingSquare.Left + (startingSquare.Width - player4.Width) / 2;
            player4.Top = startingSquare.Top + (startingSquare.Height - player4.Height) / 2;

            // Initialize button states
            UpdateButtonStates();

            // Hide the "Restart Game" button initially
            button3.Visible = false;
        }

        // Empty event handlers (retained to avoid errors)
        private void pictureBox2a_Click(object sender, EventArgs e) { }
        private void pictureBox31_Click(object sender, EventArgs e) { }
        private void pictureBox30_Click(object sender, EventArgs e) { }
        private void pictureBox21_Click(object sender, EventArgs e) { }
        private void pictureBox81_Click(object sender, EventArgs e) { }
        private void pictureBox1a_Click(object sender, EventArgs e) { }
        private void pictureBox3a_Click(object sender, EventArgs e) { }
        private void pictureBox4a_Click(object sender, EventArgs e) { }
        private void pictureBox5a_Click(object sender, EventArgs e) { }
        private void player1_Click_1(object sender, EventArgs e) { }
        private void player2_Click_1(object sender, EventArgs e) { }
        private void dice2_Click(object sender, EventArgs e) { }
        private void dice_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void player3_Click(object sender, EventArgs e) { }
        private void player4_Click(object sender, EventArgs e) { }
        private void dice3_Click(object sender, EventArgs e) { }
        private void dice4_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }

        // Event handler for the "Back to Main Menu" button.
        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the Front form
            Front frontForm = new Front();

            // Show the Front form
            frontForm.Show();

            // Hide or close the current form
            this.Hide(); // Use this.Hide() if you want to keep the current form in memory
                         // this.Close(); // Use this.Close() if you want to close the current form
        }

        // Event handler for the "How to Play" button.
        private void button2_Click(object sender, EventArgs e)
        {
            // Display the rules and mechanics of the Snake and Ladder game
            string howToPlay = "Snake and Ladder Game Instructions:\n\n" +
                               "1. Each player starts the game with their token on square 1.\n" +
                               "2. Click on the 'Roll Dice' button to determine the number of spaces to move.\n" +
                               "3. Your token will automatically move across the board.\n" +
                               "4. If you land at the bottom of a ladder, climb up!\n" +
                               "5. If you land on the head of a snake, slide down!\n" +
                               "6. The first player to reach or exceed square 100 wins the game.\n";

            MessageBox.Show(howToPlay, "How To Play", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Event handler for the "Restart Game" button.
        private void button3_Click(object sender, EventArgs e)
        {
            ResetGame(); // Reset the game
            button3.Visible = false; // Hide the "Restart Game" button
        }
    }
}