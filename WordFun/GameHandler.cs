using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;

namespace WordFun
{
    class GameHandler
    {
        //Variables
        public bool PlayersTurn = true;//Stores the move state of the game, if its the players turn or not
        public int PlayersScore = 0;//Stores the Players score
        public int AiScore = 0;//Stores the Ais score
        public static Dictionary<int, int> PlayerScoreData = new Dictionary<int, int>();// Dictionary to store points against the player id (key)
        public GameEngine Game;//Used to store a reference to attach to a GameEngine Instance
        public ImageHandler GameImageHandler;//Used to store a reference to the Imagehandler instance
        public AI GameAI;//Used to store a reference to the AI instance
        public GameScreen ScreenReference;//Used to store a reference to the GameScreen instance
        /// <summary>
        /// This is the constructor for the game handler, apon class creation a reference to the
        /// game screen is parsed and store in a variable so that it can be manipulated from
        /// this class. The contructor will also Create the instances for the GameEngine,
        /// GameImageHandler and the GameAi. After this it will run the functions to build
        /// the game tiles in the game image handler, and update the board tiles on the game screen
        /// form
        /// </summary>
        /// <param name="GameScreen"></param>
        public GameHandler(GameScreen GameScreen)
        {
            ScreenReference = GameScreen;//Sets the class reference to the parsed parameter from the constructors parameter
            Console.WriteLine("GAMEHANDLER STARTED SUCCESSFULLY");//Writes to debug console that the game handler has started
            Console.WriteLine("START ENGINE");//Writes to debug console that the game handler has initiated the game engine
            Game = new GameEngine();//Creates a new instance of game engine and stores it in the variable Game
            Console.WriteLine("START IMAGEHANDLER");//Writes to debug console that the game handler has initiated the image handler
            GameImageHandler = new ImageHandler(GameScreen);//Creates a new instance of the imagehandler and parsing to its constructor the reference to the Game Screen
            Console.WriteLine("START AI");//Writes to debug console that the game handler has initiated the ai
            GameAI = new AI(Game.DataStructure, Game);//Creates a new instance of the Ai and parsing to its constructor the game engines data structure and the game engine itself
            BuildAllTiles(GameScreen, GameImageHandler);//Builds all the tiles in the image handler
            UpdateBoardTiles(Game, GameImageHandler);//Updates the tiles in the gamescreen
        }
        /// <summary>
        /// This fuction tells the game image handler to generate all the tiles in the game
        /// for every coordinate. it also then tell it to build the backround highlight tile.
        /// </summary>
        /// <param name="GameScreen"></param>
        /// <param name="GameImageHandler"></param>
        private void BuildAllTiles(GameScreen GameScreen, ImageHandler GameImageHandler)
        {
            int TileCounter = 0;//Defines and set TileCounter to 0
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    GameImageHandler.BuildNewTile("-");//creates a new tile
                    GameImageHandler.MoveATile(TileCounter, 20 + i * 40, 20 + j * 40);//moves the tile to its position on the board
                    TileCounter++;//increase the tile counter so that the next tile to be created can be referenced and translated
                }//loop for each row
            }//loop for each col
            GameImageHandler.BuildHighlightTile();//Builds the HighlightTile
        }
        /// <summary>
        /// This function handles the process of intiating all the required functions to make
        /// the ais move. It first parses the ai its hand of letters, it then makes a 15% chance
        /// using random numbers to see if it should draw a tile, or make a move. If it makes
        /// a move it tells the Ai class to make the move and return the score, and adds a
        /// tile to its hand.
        /// </summary>
        public void AiTurn()
        {
            Console.WriteLine("AITURNSTART");//Write to the Debug console that the Ai turn has started
            ScreenReference.WriteToBox("The AI is making its Move!");//Tells the game screen to write to the box that the ai is making its move
            string TempString = "";//Defines and sets TempString to "" so that the ais hand can be stored in it
            foreach(string Letter in Game.AIHand)
            {
                TempString += Letter;
            }//stores the ais hand of tiles as a string
            Random rnd = new Random();//Define the Random object class
            int RandomNumber = rnd.Next(1, 101);//Generates a random number and stores it to the variable RandomNumber
            if (RandomNumber > 15)
            {
                int TempScore = GameAI.CalculateAndMakeBestMove(TempString);//stores the score calulated by the Ai class 
                Console.WriteLine("Score Calculated=" + TempScore);//Write to the Debug console the score calculated for the Ais move
                if (TempScore != 0)
                {
                    Console.WriteLine("TempString=" + TempString);//Write to the Debug console the ais hand of tiles
                    AiScore += TempScore;//add the calculated score to the Ais score
                    ScreenReference.WriteToBox("The AI made its move, Your turn!");//Tells the game screen to write to the box that the ai has made its move
                }//if the score isnt 0, this means a move was made
                else
                {
                    Game.Refill1AIHand();//Add a tile to the Ais hand
                    ScreenReference.WriteToBox("The AI decided to draw a tile and skip its turn, Your turn!");//Tells the game screen to write to the box that the ai skipped its turn and drew a tile
                }//if the temp score is equal to 0, it means no moves are avaliable so it draws a new tile
            }//15% to just draw a tile
            else
            {
                Game.Refill1AIHand();//Add a tile to the Ais hand
                ScreenReference.WriteToBox("The AI decided to draw a tile and skip its turn, Your turn!");//Tells the game screen to write to the box that the ai skipped its turn and drew a tile
            }//draw a tile
            PlayersTurn = true;//Sets the players turn start to true
            UpdateBoardTiles(Game, GameImageHandler);//Updates the Tiles on the game screen
            ScreenReference.UpdatePlayersTilesAndScores();//Tells the gamescreen to update the score values
        }
        /// <summary>
        /// This Function runs through two for loops to tell all tiles to update
        /// their images.
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="GameImageHandler"></param>
        public void UpdateBoardTiles(GameEngine Game, ImageHandler GameImageHandler)
        {
            int TileCounter = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (Game.Board[i,j] != "-")
                    {
                        GameImageHandler.ActiveImages[TileCounter].Update(Game.Board[i, j]);//Tells the individual tiles to update their images
                    }
                    TileCounter++;//So that the next tile can bre referenced correctly
                }//loop for each row
            }//loop for each col
        }
        /// <summary>
        /// this function is used to completely close the game to make sure no data is left stored
        /// in the memory, it does this by setting key varaibles to null.
        /// </summary>
        public void TerminateGame()
        {
            Game.wordHandle = null;//Sets Game.wordHandle to null
            Game.DataStructure = null;//Sets Game.DataStructure to null
            Game.TidyLists();//Tells the GameEngine to clear all lists
            Game = null;//Sets the game engine to null
            GameImageHandler = null;//Sets the game image handler to null
        }
    }
}
