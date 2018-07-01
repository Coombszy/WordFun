using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordFun
{
    public partial class GameScreen : Form
    {
        private GameHandler GameHandle;//stores a reference to a GameHandler
        private static int TargetX = 99;//stores a target board coordinate x
        private static int TargetY = 99;//stores a target board coordinate y
        /// <summary>
        /// This constructor sets the next menu to 99 (close game menu) and writes to the debug console that it has 
        /// started successfully. 
        /// </summary>
        public GameScreen()
        {
            Console.WriteLine("GameScreenStart");//Writes to debug that its started successfully
            Program.NextMenuID = 99;//Sets the next program menu to 99
            InitializeComponent();//initializes the form
        }
        /// <summary>
        /// This button handles move making, it will read in the users word, target location (x and y from TargetX and TargetY) and 
        /// direction. It then checks if its a valid move, if it is it makes the move and then tells the ai to make its turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if(GameEngine.EndGame == true)
            {

            }
            string Word = ""; int targx = 0; int targy = 0; char Direction = 'H'; string ReturnString = "";//Defines and sets variables to empty
            if (GameHandle.PlayersTurn)
            {
                try
                {
                    Word = (textBox1.Text).ToUpper();
                    targx = TargetX;
                    targy = TargetY;
                }//tries to get the users inputs
                catch { ReturnString = "FAILINPUTS"; }//Sets the return string to FAILINPUTS so the turn cannot be made

                //Sets the direction based on the track bar value
                if (trackBar1.Value == 1)
                {
                    Direction = 'V';//if trackbar is 1, direction is Vertical
                }
                else
                {
                    Direction = 'H';//if trackbar is 0, direction is Horizontal
                }


                if (Word == "")
                {
                    ReturnString = "FAILNOWORD";//Sets ReturnString to FAILNOWORD
                }//If the users word it empty
                else if (!(GameHandle.Game.UserMakeWord(Word, TargetX, TargetY, Direction)))
                {
                    ReturnString = "FAILCANTMAKEWORD";//Sets ReturnString to FAILCANTMAKEWORD
                }//Checks if the user can make the move with their tiles and the tiles on the board
                else if (targx > 14 || targy > 14)
                {
                    ReturnString = "FAILNOCOORDS";//Sets ReturnString to FAILNOCOORDS
                }//Checks if the user has selected valid coords
                else
                {
                    ReturnString = GameHandle.Game.MoveCheck(Word, targx, targy, Direction);//Sets the the return string to the string produced by movecheck
                }//If it passes the other checks it performs another move check
                if (ReturnString == "PASS")
                {
                    GameHandle.PlayersScore += GameHandle.Game.MakeMove(Word, targx, targy, Direction);//calculates and adds the score of the move ot the players total score
                    TargetX = 99; TargetY = 99;//Resets the target coordinates to default
                    richTextBox1.Text = "Move made!";//Writes to the Game chat that the move was made
                    GameHandle.PlayersTurn = false;//Set the players turn bool to false
                    GameHandle.AiTurn();//Triggers the ais turn in the game handle
                }//if the return string is true
                else if (ReturnString == "FAILINPUTS")
                {
                    richTextBox1.Text = "Your inputs were invalid! Try again!";//Writes to the Game chat that their inputs were invalid
                }//If return string is FAILINPUTS
                else if (ReturnString == "FAILNOCOORDS")
                {
                    richTextBox1.Text = "Please select a target tile!";//Writes to the Game chat that they have not chosen a tile
                }//If return string is FAILNOCOORDS
                else if (ReturnString == "FAILNOWORD")
                {
                    richTextBox1.Text = "Please write a word!";//Writes to the Game chat that they need to actually write a word
                }//If return string is FAILNOWORD
                else if (ReturnString == "FAILCANTMAKEWORD")
                {
                    richTextBox1.Text = "You cannot make that word with your tiles!";//Writes to the Game chat that they cant make that word with the tiles available
                }//If return string is FAILCANTMAKEWORD
                else
                {
                    richTextBox1.Text = ReturnString;//Writes to the Game chat whatever string is stored in ReturnString
                }//If no other move check events pass
                ImageHandler.HighlightATile(99, 99);//Move the highlight tile off the board
                GameHandle.UpdateBoardTiles(GameHandle.Game, GameHandle.GameImageHandler);//update all tiles to make sure the board is shown correctly
            }//If its the PlayersTurn is true, let the player make a move
            else
            {
                richTextBox1.Text = "Not your turn!";//Writes to the game chat that its not the players turn
            }//If its not their turn, so the ais turn
            UpdatePlayersTilesAndScores();//Updates the screen showing the players tiles and scores
        }
        /// <summary>
        /// This function creates a game handler for this screen and parses a reference to this screen to it.
        /// </summary>
        public void CreateGameHandle()
        {
            GameHandle = new GameHandler(this);//Sets Gamehandle to a new instance of GameHandler with this object/form parsing to it
        }
        /// <summary>
        /// This function is used to exit to the main menu, it sets the program next menu id to 0 (Main menu) and then runs the terminate
        /// game function in the gamehandler to make sure the memory is cleared. it then sets the gamehandle varaible to null and then 
        /// closes this form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click(object sender, EventArgs e)
        {
            Program.NextMenuID = 0;//sets teh next menu id to 0
            GameHandle.TerminateGame();//Runs the terminate game function in the gamehandler
            GameHandle = null;//Sets the GameHandle variable to null
            this.Close();//Closes the form
        }
        /// <summary>
        /// On the game screen load event it writes to the Game chat that the game has started and its the players turn and updates the
        /// players tile and scores boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameScreen_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Welcome! the game has started and it's your turn!";//Writes to the Game chat that The game has begun and its the players turn
            UpdatePlayersTilesAndScores();//updates the players tiles box and scores.
        }
        /// <summary>
        /// Updates the coordinates for the highlight tile, and updates its location on the form.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void UpdateCoords(int x, int y)
        {
            TargetX = x;//Sets the TargetX to the parameter x
            TargetY = y;//Sets the TargetY to the parameter y
            ImageHandler.HighlightATile(TargetX, TargetY);//sets the highlight tile location to the coordinates of the users tile
        }
        /// <summary>
        /// This function updates the box that shows the users tiles and their score by writing the lables and boxes again.
        /// </summary>
        public void UpdatePlayersTilesAndScores()

        {

            string TempString = "";//sets and defines the TempString to "" (Nothing/empty)
            try
            {
                TempString = GameHandle.Game.PlayerHand[0];
                for (int i = 1; i < GameHandle.Game.PlayerHand.Count; i++)
                {
                    TempString += ", " + GameHandle.Game.PlayerHand[i];//add all letters to a string
                }//loops for all tiles in the list
            }catch { }
            richTextBox2.Text = "Your Tiles: " + TempString;//writes to the richTextbox the Your Tiles: + the users tiles
            label4.Text = "Your Score: " + GameHandle.PlayersScore + " | " + "Ai's Score: " + GameHandle.AiScore;//Writes to the label the users store and ais score
        }
        /// <summary>
        /// This function allows the user to draw a new tile if its the users turn. if its not their turn it writes to the game chat
        /// game that its not their turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (GameHandle.PlayersTurn)
            {
                GameHandle.Game.Refill1PlayersHand();//draw and add a new tile to the user hand
                GameHandle.PlayersTurn = false;//sets the players turn bool to false
                GameHandle.AiTurn();//Starts the users turn
            }//if its the players turn
            else
            {
                richTextBox1.Text = "Not your turn!";//writes to the game chat that its not the users turn
            }
            UpdatePlayersTilesAndScores();//updates the users hand and score
        }
        /// <summary>
        /// This function allows me to easily set the rich text box to the parameter being parsed to it
        /// </summary>
        /// <param name="String"></param>
        public void WriteToBox(string String)
        {
            richTextBox1.Text = String;//sets richTextBox1.text to the parameter string
        }
    }
}
