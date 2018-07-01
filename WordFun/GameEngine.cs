using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WordFun
{
    class GameEngine
    {
        #region Variables
        //These are variables that are required by the engine to function and make calculations

        //--Board & Game Mechanics Definitions
        public const int ROW = 15, COL = 15;//Defines the Boards Dimensions
        public string[,] Board = new string[ROW, COL];//Created 2D array - used to store data like a board
        public static List<string> LetterPile = new List<string>();//This represents the bag of tiles to hand out to gameplayers

        //--Player & Lobby data Definitions
        public List<string> PlayerHand = new List<string>();//Player 1 Hand of tiles
        public List<string> AIHand = new List<string>();//AI Hand of tiles
        public List<string> ListToRemove = new List<string>();//This is store strings to be removed from a players hand
        public static string PlayerN = "";//Player 1 Name
        public static bool EndGame = false;//To know if its time to end the game

        //--WordHandler Interaction
        public WordHandler wordHandle = new WordHandler();//Creates an Instance of the Word Handler Class to be used in the engine

        //--DataStructure
        public HeaderNode DataStructure = new HeaderNode();

        //--Variables for Wordchecking
        //----Variables Specifically for Vertical Word Placing
        public List<string> P_CharsYB = new List<string>();//Used to store letters that are already on the board on the Y Axis Before the users Target
        public string P_StringYB;//This the Words/Characters already placed on the Y coordinate before the users target
        public List<string> P_CharsYA = new List<string>();//Used to store letters that are already on the board on the Y Axis After the users Target
        public string P_StringYA;//This the Words/Characters already placed on the Y coordinate After the users target

        //----Variables Specifically for Horizontal Word Placing
        public List<string> P_CharsXB = new List<string>();//Used to store letters that are already on the board on the Y Axis Before the users Target
        public string P_StringXB;//This the Words/Characters already placed on the Y coordinate before the users target
        public List<string> P_CharsXA = new List<string>();//Used to store letters that are already on the board on the Y Axis After the users Target
        public string P_StringXA;//This the Words/Characters already placed on the Y coordinate After the users target

        //----Variables for Overlap Checking
        public List<string> P_Overlap = new List<string>();//this is used to store values to check for overlap

        //----Variables for Perpendicular Checking
        List<string> Pc_Left = new List<string>(); string LeftString;
        List<string> Pc_Right = new List<string>(); string RightString;
        List<string> Pc_Above = new List<string>(); string AboveString;
        List<string> Pc_Below = new List<string>(); string BelowString;
        // All these lists and strings are used to temporarily store data on the board so that it can be manipulated without effect the board when checking for valid moves

        #endregion Variables
        /// <summary>
        /// This is the constructor for the GameEngine, Its primary purpose is to parse
        /// the DataStructure to the WordHandler (wordHandle) and then tell the word handler
        /// to load all words into the datastructure. Then the constructor will call
        /// the new game function.
        /// </summary>
        public GameEngine()
        {
            wordHandle.MyStructure(DataStructure);//Parses data structure to the wordhandle
            wordHandle.MakeDict();//Fill Data structure with all words
            NewGame();//Excutes the creation of a new game
        }
        /// <summary>
        /// This function begins the process of starting a new game, It first create the 
        /// TilePile and shuffles it. Then it fills the  board with blank spaces ("-" mean empty).
        /// After this the center/starter Tile is placed from the pile and the Ai and Players
        /// hands are filled from the pile
        /// </summary>
        private void NewGame()
        {
            CreatePile();//Creates the pile of tiles
            ShufflePile();//Shuffles the pile
            for (int i = 0; i < ROW; i++)//This fills the board array with Blank Spaces
            {
                for (int k = 0; k < COL; k++)
                {
                    Board[i, k] = "-";//Sets the current location/coordinate to "-" (empty slot)
                }//for each x coordinate
            }//for each y coordinate
            Board[7, 7] = ProduceTile();//Sets the middle tile to random tile from the tile pile
            SetupPlayerHand();//Fills the players hand with tiles
            SetupAIHand();//Fills the ais hand with tiles
        }
        /// <summary>
        /// in this function the letter pile is randomised by swapping the locations of
        /// elements randomly.
        /// </summary>
        public void ShufflePile()
        {
            Random ran = new Random();
            int i = LetterPile.Count;
            while (i > 1)
            {
                i--;
                int RandomLoc = ran.Next(i + 1);
                string Value = LetterPile[RandomLoc];
                LetterPile[RandomLoc] = LetterPile[i];
                LetterPile[i] = Value;
            }//randomises the list by counting down through the list and swaps the location of letters at random
        }
        /// <summary>
        /// This adds the correct number of tiles to the pile, It does this by looping x
        /// amount of times for tiles sharing the same quantity.
        /// </summary>
        public void CreatePile()
        {
            //Adds all letters involved in the game (Based of rule book), For loops make editing easier to change letter quantities
            LetterPile.Add("K");
            LetterPile.Add("J");
            LetterPile.Add("X");
            LetterPile.Add("Q");
            LetterPile.Add("Z");

            for (int i = 0; i < 2; i++)//Letters with quantity of 2
            {

                LetterPile.Add("B");
                LetterPile.Add("C");
                LetterPile.Add("M");
                LetterPile.Add("P");

                LetterPile.Add("F");
                LetterPile.Add("H");
                LetterPile.Add("V");
                LetterPile.Add("W");
                LetterPile.Add("Y");
            }

            for (int i = 0; i < 3; i++)//Letters with quantity of 3
            {
                LetterPile.Add("G");
            }

            for (int i = 0; i < 4; i++)//Letters with quantity of 4
            {
                LetterPile.Add("L");
                LetterPile.Add("S");
                LetterPile.Add("U");
                LetterPile.Add("D");
            }

            for (int i = 0; i < 6; i++)//Letters with quantity of 5
            {

                LetterPile.Add("N");
                LetterPile.Add("R");
                LetterPile.Add("T");
            }
            for (int i = 0; i < 8; i++)//Letters with quantity of 8
            {
                LetterPile.Add("O");
            }
            for (int i = 0; i < 9; i++)//Letters with quantity of 9
            {
                LetterPile.Add("A");
                LetterPile.Add("I");
            }

            for (int i = 0; i < 12; i++)//Letters with quantity of 12
            {
                LetterPile.Add("E");

            }

        }
        /// <summary>
        /// This function is used to convert a char to as string, this function is not
        /// required for the program to operate, but did make it easier to understand
        /// whend coding it what things did. CharToString(A) made more sense than
        /// ""+A.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		private string ChartoString(char value)
        {
            return new string(value, 1);
        }
        /// <summary>
        /// This function returns the last letter/tile int the list (TilePile) and removes it
        /// from the pile.
        /// </summary>
        /// <returns></returns>
        public static string ProduceTile()
        {
            string temp = LetterPile[LetterPile.Count - 1];// Stores the last letter in the list into Temp
            LetterPile.RemoveAt(LetterPile.Count - 1);//Removes the letter from the list (Temp)
            if (LetterPile.Count ==0)
            {
                EndGame = true;
            }//Checks if the game should end as the TilePile has reached 0
            return temp;//Returns the removed letter from the list
        }
        /// <summary>
        /// This is a Debug function that will place a word on the board without checking
        /// if its a valid move, This was fro testing purposes to allow me to force words
        /// i wanted to experiment with onto the board. It placed word using at the target
        /// location and direction specified in the Parameters
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        public void D_MakeMove(string UserWord, int TargX, int TargY, char Direction)
		{
			int WordLength = UserWord.Length;//Stores the length of the word
			if (Direction == 'H'){
				for (int i = 0; i < WordLength; i++)
				{
				    Board[TargX+i, TargY] = ChartoString(UserWord[i]);//places a Char (converted into a string) at the the target location (from the functions parameters) 
				}//for each char in the word
			}//If target direction is Horizontal
			else if (Direction == 'V'){
				for (int i = 0; i < WordLength; i++)
				{
				    Board[TargX, TargY+i] = ChartoString(UserWord[i]);//places a Char (converted into a string) at the the target location (from the functions parameters) 
                }//for each char in the word
            }//If target direction is Vertical
		}
        /// <summary>
        /// This function will make a given move on the board(using the parameters parsed to it)
        /// , without overriding tiles already on the board. operates the same as the Debug 
        /// counterpart. It also removes the tiles that are being used from the player hand on the board.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public int MakeMove(string UserWord, int TargX, int TargY, char Direction)
        {
            int WordLength = UserWord.Length;//Stores the word length
            List<string> ExistingTiles = new List<string>();//A list to store existing tiles on the board
            if (Direction == 'H')
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX + i, TargY]); }//uses tries incase the word is running of the "edge of the board" (Out of array bounds)
                    catch { ExistingTiles.Add("-"); }
                }
            }//Adds the exsiting tiles to the list (Horizontally)
            else
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX, TargY + i]); }//uses tries incase the word is running of the "edge of the board" (Out of array bounds)
                    catch { ExistingTiles.Add("-"); }
                }
            }//Adds the exsiting tiles to the list (Vertically)
            if (Direction == 'H')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    if ((PlayerHand.Contains(""+UserWord[i]))&&(!(ExistingTiles.Contains(""+UserWord[i]))))
                    {
                        PlayerHand.Remove("" + UserWord[i]);
                    }//If the players hand contains the tile, and the board doesnt contain the tile, remove the tile from the players hand
                    Board[TargX + i, TargY] = ChartoString(UserWord[i]);//places a Char (converted into a string) at the the target location (from the functions parameters) 
                }//For each letter in the users word
            }
            else if (Direction == 'V')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    if ((PlayerHand.Contains("" + UserWord[i])) && (!(ExistingTiles.Contains("" + UserWord[i]))))
                    {
                        PlayerHand.Remove("" + UserWord[i]);
                    }// If the players hand contains the tile, and the board doesnt contain the tile, remove the tile from the players hand
                    Board[TargX, TargY + i] = ChartoString(UserWord[i]);//places a Char (converted into a string) at the the target location (from the functions parameters) 
                }//For each letter in the users word
            }
            TidyLists();//Clears the temporary lists used in this function
            return (PerpScore(UserWord, TargX, TargY, Direction) + ScoreOnTargetedPlane(UserWord, TargX, TargY, Direction));//returns the perpendicular score and score on the target plane
        }
        /// <summary>
        /// The same as the players Make Move function but tiles are subtracted from the Ais
        /// hand instead, and that it returns the amount of times a tile was removed from
        /// the ais hand. This is used in the AI class to check to see if tiles have been
        /// removed or not.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public int AI_MakeMove(string UserWord, int TargX, int TargY, char Direction)
        {
            int flags = 0;//Defines and sets the value of Flags to 0
            int WordLength = UserWord.Length;
            List<string> ExistingTiles = new List<string>();
            if (Direction == 'H')
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX + i, TargY]); }
                    catch { ExistingTiles.Add("-"); }
                }
            }
            else
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX, TargY + i]); }
                    catch { ExistingTiles.Add("-"); }
                }
            }
            if (Direction == 'H')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    if ((AIHand.Contains("" + UserWord[i])) && (!(ExistingTiles.Contains("" + UserWord[i]))))
                    {
                        AIHand.Remove("" + UserWord[i]);
                        flags++;//if it removes a tile, the flag value is increased
                    }
                    Board[TargX + i, TargY] = ChartoString(UserWord[i]);
                }
            }
            else if (Direction == 'V')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    if ((AIHand.Contains("" + UserWord[i])) && (!(ExistingTiles.Contains("" + UserWord[i]))))
                    {
                        AIHand.Remove("" + UserWord[i]);
                        flags++;//if it removes a tile, the flag value is increased
                    }
                    Board[TargX, TargY + i] = ChartoString(UserWord[i]);
                }
            }
            TidyLists();
            return (flags);//returns flag value
        }
        /// <summary>
        /// This is the function that checks that all move checking conditions pass. if any of
        /// them fail, it returns the reason it failed. If it passes it returns PASS. The
        /// parameters are passed on to the moveing cheking condition functions.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public string MoveCheck(string Word, int TargX, int TargY, char Direction)
		{
            if (DataStructure.InternalWordCheck(Word))
            {
                TidyLists();
                if (IsConnected(Word, TargX, TargY, Direction))
                {
                    TidyLists();
                    if (OverlapCheck(Word, TargX, TargY, Direction))
                    {
                        TidyLists();
                        if (StringOnTargetedPlane(Word, TargX, TargY, Direction))
                        {
                            TidyLists();
                            if (PerpCheck(Word, TargX, TargY, Direction))
                            {
                                TidyLists();
                                if (TileSubtractCheck(Word, TargX, TargY, Direction))
                                {
                                    TidyLists();//Tidys list up used by the checking conditions
                                    return "PASS";//return pass if all conditions pass
                                }
                                else { return "You do not actually use any of your tiles"; }//Checks to see if any of the tiles from the hand are used
                            }
                            else { return "Conflicting with Tiles perpendicular to the target"; }//Checks if it is conflicting with tiles perpendicular to the words tiles
                        }
                        else { return "Conflicting with Tiles before or after Target"; }//Checks if it conflicts with tiles before or after the target word position
                    }
                    else { return "Conflicts with existing tiles"; }//Checks if it conflicts with other tiles
                }
                else { return "Is not Connected"; }//Checks if its connected to other tiles/words
            }
            else { return "Not a real word"; }//Checks if its a real word or not
		}
        /// <summary>
        /// This function will check to see if any tiles are actually subtracted from the
        /// players/Ais hand. This to prevent making words over and over without using
        /// any tiles up (Or a logical loop in the AI). It does this be counting the amount
        /// of tiles that are not already on the board. If the value is not 0 then tiles
        /// must have been subtracted from the players hand.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool TileSubtractCheck(string UserWord, int TargX, int TargY, char Direction)
        {
            int flags = 0;//defines and sets the flags variable to 0
            int WordLength = UserWord.Length;//stores the word length to the variable WordLength
            List<string> ExistingTiles = new List<string>();
            if (Direction == 'H')
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX + i, TargY]); }//add the tiles where the words target location to ExistingTiles 
                    catch { ExistingTiles.Add("-"); }//If the location runs of the board this catch prevents an error and just adds a empty tile to the List
                }
            }//If the direction is Horizontal
            else
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX, TargY + i]); }//add the tiles where the words target location to ExistingTiles 
                    catch { ExistingTiles.Add("-"); }//If the location runs of the board this catch prevents an error and just adds a empty tile to the List
                }
            }//If the direction is Vertical

            foreach (char Letter in UserWord)
            {
                if (!(ExistingTiles.Contains("" + Letter)))
                {
                    flags++;//If it does not contain the letter, it increases the value of flags by one
                }
            }//for each letter in UserWord it checks if the list contains the letter
            if (flags != 0)
            {
                return true;
            }//if the flags value is not 0, it returns true
            else
            {
                return false;
            }//if it is 0 it returns false

        }
        /// <summary>
        /// This function gets the string before and after the target word location if
        /// there is anyway, and performs a word check on the enter line 
        /// (StringBefore + UsersWord+ StringAfter). if the word check passes it returns 
        /// true, otherwise false
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool StringOnTargetedPlane(string UserWord, int TargX, int TargY, char Direction)
        {
            string CharsBefore= "", CharsAfter="";//Stores the string of characters before and after the users target location
            int WordLength = UserWord.Length;//Stores the length of the users word
            string CharBefore="", CharAfter="";//Stores the SINGLE character before and after the users target location

            if (Direction == 'H')
            {
                try { CharBefore = Board[TargX - 1, TargY].Replace("-", ""); }
                catch { }
                try { CharAfter = Board[TargX + WordLength, TargY].Replace("-", ""); }
                catch { }
            }//gets the letters before and after the target location, this is used to see if its worth getting the characters of the entire plane (Horizontal)
            if (Direction == 'V')
            {
                try { CharBefore = Board[TargX, TargY -1].Replace("-", ""); }
                catch { }
                try { CharAfter = Board[TargX, TargY + WordLength].Replace("-", ""); }
                catch { }
            }//gets the letters before and after the target location, this is used to see if its worth getting the characters of the entire plane (Vetical)


            if (Direction == 'H')
            {
                //Finding the String before the target location if there is any
                if (CharBefore != "")
                {
                    int XDifference = TargX;
                    for (int i = 0; i < XDifference; i++)
                    {
                        P_CharsXB.Add(Board[i, TargY]);
                    }//from 0 to the start of the target location, it gets the tiles and builds a string
                    try { P_StringXB = P_CharsXB.Aggregate((a, b) => a + b); } catch { P_StringXB = ""; }//Converts to a string
                    CharsBefore = P_StringXB.Replace("-", "");
                    P_CharsXB.Clear();
                }
                //Finding the String after the target location if there is any
                if (CharAfter != "")
                {
                    int P_Start = (TargX) + WordLength;
                    for (int i = P_Start; i < 15; i++)
                    {
                        P_CharsXA.Add(Board[i, TargY]);
                    }//from the end of the users word at the target lation to the end of the board (14), it collects all the tiles and makes a string
                    try { P_StringXA = P_CharsXA.Aggregate((a, b) => a + b); } catch { P_StringXA = ""; }//Converts to a string
                    P_CharsXA.Clear();
                    CharsAfter = P_StringXA.Replace("-", "");
                }
            }//if the target direction is Horizontal
            if (Direction == 'V')
            {
                if (CharBefore != "")
                {
                    //Finding the String before the target location if there is any
                    int YDifference = TargY;
                    for (int i = 0; i < YDifference; i++)
                    {
                        P_CharsYB.Add(Board[TargX, i]);
                    }//from 0 to the start of the target location, it gets the tiles and builds a string
                    try { P_StringYB = P_CharsYB.Aggregate((a, b) => a + b); } catch { P_StringYB = ""; }//Converts to a string
                    CharsBefore = P_StringYB.Replace("-", "");
                    P_CharsYB.Clear();
                }
                //Finding the String after the target location if there is any
                if (CharAfter != "")
                {
                    int P_Start = (TargY) + WordLength;
                    for (int i = P_Start; i < 15; i++)
                    {
                        P_CharsYA.Add(Board[TargX, i]);
                    }//from the end of the users word at the target lation to the end of the board (14), it collects all the tiles and makes a string
                    try { P_StringYA = P_CharsYA.Aggregate((a, b) => a + b); } catch { P_StringYA = ""; }//Converts to a string
                    P_CharsYA.Clear();
                    CharsAfter = P_StringYA.Replace("-", "");
                }
            }//if the target direction is Vertical
            if (wordHandle.WordCheck(CharsBefore + UserWord + CharsAfter))
            {
                return true;
            }//if the wordcheck pass return true
            else
            {
                return false;
            }//if the wordcheck fails return false
        }
        /// <summary>
        /// This function makes sure that if the players word was to placed the tiles existing
        /// on the board match to the tiles of the word being placed. It does this by checking
        /// the location where each tile will be placed, if the location tile and the word
        /// tile do not match, it increases a Overlapflag value. By the end of the function
        /// if there are no overlap flags the function will return true, otherwise it will return
        /// false.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool OverlapCheck(string UserWord, int TargX, int TargY, char Direction)
        {
            int OverlapFlags = 0;//This is used to store the amount of invalid checks
            int WordLength = UserWord.Length;//Stores the length of the users word
            if (Direction == 'H')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    P_Overlap.Add(Board[TargX + i, TargY]);
                }//Gets the contents of Target Location where the New Word would be created
                OverlapFlags = 0;
                for (int i = 0; i < WordLength; i++)
                {
                    if ((P_Overlap[i] == "-") || (P_Overlap[i] == ""+UserWord[i])) { }
                    else { OverlapFlags++; }
                }//For each letter of the target location for the new word it checks its contents, if it is equal to the letter in the word, or that the slot is empty, it passes.
            }
            if (Direction == 'V')
            {
                for (int i = 0; i < UserWord.Length; i++)
                {
                    P_Overlap.Add(Board[TargX, TargY + i]);
                }//Gets the contents of Target Location where the New Word would be created
                OverlapFlags = 0;
                for (int i = 0; i < WordLength; i++)
                {
                    if ((P_Overlap[i] == "-") || (P_Overlap[i] == ""+UserWord[i])) { }
                    else { OverlapFlags++;}
                }//For each letter of the target location for the new word it checks its contents, if it is equal to the letter in the word, or that the slot is empty, it passes.
            }
            if (!(OverlapFlags == 0)) { return false; }
            else { return true; }
        }
        /// <summary>
        /// This function checks that the tiles being placed do not conflict with tiles existing
        /// perpendicular to the target words location. it does this by getting the strings to the left
        /// and right of the tile if it is not against the edge of the board, a flag value. 
        /// and then wordchecking them. If the word check fails it increases
        /// After it has does this for every letter in the word it returns true if
        /// the flag value is 0, anything else it returns false
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool PerpCheck(string UserWord, int TargX, int TargY, char Direction)
        {
            int WordLength = UserWord.Length;//Defines and sets WordLength to the length of the parameter UserWord
            int PerpFlags = 0;//Defines and sets the flag counter (PerpFlags) to 0
            if (Direction == 'V')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    string Val_Left = "-"; string Val_Right = "-";//Creates the temporary storage locations for left and right
                    string TileCollection;//Defines tile collection
                    int Difference = 0;//Defines and sets Difference variable to 0
                    string tempChar;//Defines the tempChar variable
                    Pc_Right.Clear(); Pc_Left.Clear();//Clears the lists, just to make sure they are empty to stop old data affecting the calculations
                    bool ToTheRight = true;//Defines and sets the ToTheRight variable to true, These are used to check if the target words tiles are against the edge of the board
                    bool ToTheLeft = true;//Defines and sets the ToTheLeft variable to true, These are used to check if the target words tiles are against the edge of the board
                    try { Val_Right = Board[TargX + 1, TargY + i]; } catch { ToTheRight = false; }//If it cant the next value along, it sets the value to false as it IS against the edge of the board
                    try { Val_Left = Board[TargX - 1, TargY + i]; } catch { ToTheLeft = false; }//If it cant the next value along, it sets the value to false as it IS against the edge of the board
                    if (!(Val_Left == "-") && ToTheLeft)
                    {
                        for (int j = TargX - 1; j >= 0 && j < TargX; j--)
                        {
                            Difference = (TargX - j);//calculates the difference from the edge to the target locations tile
                            tempChar = Board[TargX - Difference, TargY + i];
                            if (tempChar == "-") { j = 99; }//breaks out the for loop if the word on the plane ends
                            else { Pc_Left.Add(Board[TargX - Difference, TargY + i]); }
                        }
                    }//Gets the string to the left of the target tiles location, if its not against the edge
                    Pc_Left.Reverse();
                    try { LeftString = Pc_Left.Aggregate((a, b) => a + b); } catch { LeftString = ""; }//converts the list of left tiles into a string
                    if (!(Val_Right == "-") && ToTheRight)
                    {
                        for (int j = TargX + 1; j < 20 && j > TargX; j++)
                        {
                            Difference = (j - TargX);//calculates the difference from the edge to the target locations tile
                            tempChar = Board[TargX + Difference, TargY + i];
                            if (tempChar == "-") { j = 99; }//breaks out the for loop if the word on the plane ends
                            else { Pc_Right.Add(Board[TargX + Difference, TargY + i]); }
                        }
                    }//Gets the string to the right of the target tiles location, if its not against the edge
                    try { RightString = Pc_Right.Aggregate((a, b) => a + b); } catch { RightString = ""; }//converts the list of right tiles into a string

                    TileCollection = (LeftString + (""+UserWord[i]) + RightString);//gets the complete string of that plane
                    if ((LeftString == "") && (RightString == "")) {  }
                    else
                    {
                        if (wordHandle.WordCheck(TileCollection)) { }//if the word check passes, it does nothing
                        else { PerpFlags++; }//if the word check fails increase the flag value
                    }
                }//Loops so that the following code is ran for each letter
            }
            else
            {
                for (int i = 0; i < WordLength; i++)
                {
                    string Val_Above = "-"; string Val_Below = "-";//Creates the temporary storage locations for up and down
                    string TileCollection;//defines tile collection
                    int Difference = 0;//defines and sets Difference variable to 0
                    string tempChar;//Defines the tempChar variable
                    bool ToTheBelow = true;//Defines and sets the ToTheBelow variable to true, These are used to check if the target words tiles are against the edge of the board
                    bool ToTheAbove = true;//Defines and sets the ToTheAbove variable to true, These are used to check if the target words tiles are against the edge of the board
                    try { Val_Below = Board[TargX + i, TargY + 1]; } catch { ToTheBelow = false; }//If it cant the next value along, it sets the value to false as it IS against the edge of the board
                    try { Val_Above = Board[TargX + i, TargY - 1]; } catch { ToTheAbove = false; }//If it cant the next value along, it sets the value to false as it IS against the edge of the board
                    Pc_Above.Clear(); Pc_Below.Clear();//Clears the lists, just to make sure they are empty to stop old data affecting the calculations


                    if (!(Val_Above == "-") && ToTheAbove)
                    {
                        for (int j = TargY - 1; j >= 0 && j < TargY; j--)
                        {
                            Difference = (TargY - j);//calculates the difference from the edge to the target locations tile
                            tempChar = Board[TargX + i, TargY - Difference];
                            if (tempChar == "-") { j = 99; }//breaks out the for loop if the word on the plane ends
                            else { Pc_Above.Add(Board[TargX + i, TargY - Difference]); }
                        }
                    }//Gets the string to the above of the target tiles location, if its not against the edge
                    Pc_Above.Reverse();
                    try { AboveString = Pc_Above.Aggregate((a, b) => a + b); } catch { AboveString = ""; }//converts the list of above tiles into a string

                    if (!(Val_Below == "-") && ToTheBelow)
                    {
                        for (int j = TargY + 1; j < 20 && j > TargY; j++)
                        {
                            Difference = (j - TargY);//calculates the difference from the edge to the target locations tile
                            tempChar = Board[TargX + i, TargY + Difference];
                            if (tempChar == "-") { j = 99; }//breaks out the for loop if the word on the plane ends
                            else { Pc_Below.Add(Board[TargX + i, TargY + Difference]); }
                        }
                    }//Gets the string to the below of the target tiles location, if its not against the edge
                    try { BelowString = Pc_Below.Aggregate((a, b) => a + b); } catch { BelowString = ""; }//converts the list of below tiles into a string

                    TileCollection = (AboveString + ("" + UserWord[i]) + BelowString);//gets the complete string of that plane
                    if ((AboveString == "") && (BelowString == "")) { }
                    else
                    {
                        if (wordHandle.WordCheck(TileCollection)) { }//if the word check passes, it does nothing
                        else { PerpFlags++; }//if the word check fails increase the flag value
                    }
                }//Loops so that the following code is ran for each letter

            }
            if (PerpFlags == 0) { return true; }//If there is no flags from the strings perpendicular, return true
            else { return false; }//if there is flags, return false
        }
        /// <summary>
        /// This function checks if players move is actually connected to other words/tiles on the board.
        /// It does this by getting the value of all the tiles around the target word location, even
        /// if they are blank and adds them to a list. After it has all values it increase a flag counter
        /// for every tile that isnt an empty space, this means that if the flag counter isnt 0 it returns
        /// true as the word would be connected to other tiles. If the flag counter is 0, it returns false.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool IsConnected(string UserWord, int TargX, int TargY, char Direction)
        {
            int ConnectFlags = 0;//Defines and sets the flag counter (ConnectFlags) to 0
            List<string> StringStorage = new List<string>();//Defines the list to store the values of all the tiles around the word
            int WordLength = UserWord.Length;//Defines and sets WordLength to the length of the parameter UserWord
            if (Direction == 'H')
            {
                //Getting Strings before and after target word location
                try { StringStorage.Add(Board[TargX + WordLength, TargY]); } catch { }//After
                try { StringStorage.Add(Board[TargX - 1, TargY]); } catch { }//Before

                //Getting Strings above and below target word location
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX + i, TargY + 1]); } catch { }//gets strings Above
                }
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX + i, TargY - 1]); } catch { }//gets strings Below
                }
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX+i, TargY]); }
                    catch { StringStorage.Add("-"); }
                }
            }//if the move is Horizontal
            else
            {
                //Getting Strings before and after target word location
                try { StringStorage.Add(Board[TargX, TargY + WordLength]); } catch { }//gets strings After
                try { StringStorage.Add(Board[TargX, TargY - 1]); }
                catch { }//gets strings Before

                //Getting Strings above and below target word location
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX + 1, TargY + i]); }catch { }//gets strings Above
                }
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX - 1, TargY + i]); } catch { }//gets strings Below
                }
                for (int i = 0; i < WordLength; i++)
                {
                    try { StringStorage.Add(Board[TargX, TargY+i]); }
                    catch { StringStorage.Add("-"); }
                }
            }//If the move is Vertical

            for (int i = 0; i < StringStorage.Count; i++)
            {
                try
                {
                    if (StringStorage[i] != "-")
                    {
                        ConnectFlags++;//increases flag value
                    }//if element of StringStorage isnt equal to "-"
                }
                catch { }
            }//if for every StringStorage element that is equal to "-"

            if(ConnectFlags != 0)
            {
                return true;//returns true if the flag value isnt 0
            }
            else
            {
                return false;//returns false if it is equal to 0
            }


        }
        /// <summary>
        /// This function just clears all lists used for move checking conditions, this is done
        /// to make sure data isnt accidentally carried accross calculations causing logical
        /// problems in the algorithms
        /// </summary>
        public void TidyLists()
        {
            Pc_Above.Clear();//Clears Pc_Above
            Pc_Below.Clear();//Clears Pc_Below
            Pc_Left.Clear();//Clears Pc_Left
            Pc_Right.Clear();//Clears Pc_Right
            P_CharsXA.Clear();//Clears P_CharsXA
            P_CharsXB.Clear();//Clears P_CharsXB
            P_CharsYA.Clear();//Clears P_CharsYA
            P_CharsYB.Clear();//Clears P_CharsYB
            P_Overlap.Clear();////Clears P_Overlap
        }
        /// <summary>
        /// This function is used to calculate the scores on the target plane for the word.
        /// It does this similarly to how the StringOnTargetedPlane function operates, but
        /// instead of increasing a flag value for failed word checks, it calculates the score
        /// of the string on the entire plane and returns the value.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public int ScoreOnTargetedPlane(string UserWord, int TargX, int TargY, char Direction)
        {
            string CharsBefore = "", CharsAfter = "";//Stores the string of characters before and after the users target location
            int WordLength = UserWord.Length;//Stores the length of the users word
            string CharBefore = "", CharAfter = "";//Stores the SINGLE character before and after the users target location
            //This Code is exactly the same as StringOnTargetedPlane --------------------------------------------------------------------------------------------VVVV
            #region StringOnTargetedPlane code
            if (Direction == 'H')
            {
                try { CharBefore = Board[TargX - 1, TargY].Replace("-", ""); }
                catch { }
                try { CharAfter = Board[TargX + WordLength, TargY].Replace("-", ""); }
                catch { }
            }//gets the letters before and after the target location, this is used to see if its worth getting the characters of the entire plane (Horizontal)
            if (Direction == 'V')
            {
                try { CharBefore = Board[TargX, TargY - 1].Replace("-", ""); }
                catch { }
                try { CharAfter = Board[TargX, TargY + WordLength].Replace("-", ""); }
                catch { }
            }//gets the letters before and after the target location, this is used to see if its worth getting the characters of the entire plane (Vetical)


            if (Direction == 'H')
            {
                //Finding the String before the target location if there is any
                if (CharBefore != "")
                {
                    int XDifference = TargX;
                    for (int i = 0; i < XDifference; i++)
                    {
                        P_CharsXB.Add(Board[i, TargY]);
                    }
                    try { P_StringXB = P_CharsXB.Aggregate((a, b) => a + b); } catch { P_StringXB = ""; }
                    CharsBefore = P_StringXB.Replace("-", "");
                    P_CharsXB.Clear();
                }
                //Finding the String after the target location if there is any
                if (CharAfter != "")
                {
                    int P_Start = (TargX) + WordLength;
                    for (int i = P_Start; i < 15; i++)
                    {
                        P_CharsXA.Add(Board[i, TargY]);
                    }
                    try { P_StringXA = P_CharsXA.Aggregate((a, b) => a + b); } catch { P_StringXA = ""; }
                    P_CharsXA.Clear();
                    CharsAfter = P_StringXA.Replace("-", "");
                }
            }
            if (Direction == 'V')
            {
                if (CharBefore != "")
                {
                    //Finding the String before the target location if there is any
                    int YDifference = TargY;
                    for (int i = 0; i < YDifference; i++)
                    {
                        P_CharsYB.Add(Board[TargX, i]);
                    }
                    try { P_StringYB = P_CharsYB.Aggregate((a, b) => a + b); } catch { P_StringYB = ""; }
                    CharsBefore = P_StringYB.Replace("-", "");
                    P_CharsYB.Clear();
                }
                //Finding the String after the target location if there is any
                if (CharAfter != "")
                {
                    int P_Start = (TargY) + WordLength;
                    for (int i = P_Start; i < 15; i++)
                    {
                        P_CharsYA.Add(Board[TargX, i]);
                    }
                    try { P_StringYA = P_CharsYA.Aggregate((a, b) => a + b); } catch { P_StringYA = ""; }
                    P_CharsYA.Clear();
                    CharsAfter = P_StringYA.Replace("-", "");
                }
            }
            #endregion
            //This Code is no longer exactly the same as StringOnTargetedPlane ----------------------------------------------------------------------------------^^^^
            if (wordHandle.WordCheck(CharsBefore + UserWord + CharsAfter))
            {
                return (wordHandle.CalculateWordScore(CharsBefore + UserWord + CharsAfter));
            }//if it passes the wordcheck, it returns the entire planes score
            else if (wordHandle.WordCheck(UserWord))
            {
                return (wordHandle.CalculateWordScore(UserWord));
            }//if the entire plane failed the wordcheck, it returns the score of just the UserWord
            else
            {
                return 0;
            }//if all wordcheck values fail, return 0
        }
        /// <summary>
        /// This function is to calculate the score on the the planes perpendicular to the 
        /// words target location. This function primarily uses the same code as PerpCheck() so
        /// to see how the code fetches the strings on the target planes, Refer to PerpCheck()s.
        /// The code that differs is commented on how it functions.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public int PerpScore(string UserWord, int TargX, int TargY, char Direction)
        {
            int WordLength = UserWord.Length;
            int PerpScoreTotal = 0;//Defines and sets the value of PerpScoreTotal to 0. this is for storing the total score of every plane perpendicular to the target word location
            if (Direction == 'V')
            {
                for (int i = 0; i < WordLength; i++)
                {
                    string Val_Left = "-"; string Val_Right = "-";
                    string TileCollection;
                    int Difference = 0;
                    string tempChar;
                    Pc_Right.Clear(); Pc_Left.Clear();

                    bool ToTheRight = true;
                    bool ToTheLeft = true;
                    try { Val_Right = Board[TargX + 1, TargY + i]; } catch { ToTheRight = false; }
                    try { Val_Left = Board[TargX - 1, TargY + i]; } catch { ToTheLeft = false; }
                    if (!(Val_Left == "-") && ToTheLeft)
                    {
                        for (int j = TargX - 1; j >= 0 && j < TargX; j--)
                        {
                            Difference = (TargX - j);
                            tempChar = Board[TargX - Difference, TargY + i];
                            if (tempChar == "-") { j = 99; }
                            else { Pc_Left.Add(Board[TargX - Difference, TargY + i]); }
                        }
                    }
                    Pc_Left.Reverse();
                    try { LeftString = Pc_Left.Aggregate((a, b) => a + b); } catch { LeftString = ""; }
                    if (!(Val_Right == "-") && ToTheRight)
                    {
                        for (int j = TargX + 1; j < 20 && j > TargX; j++)
                        {
                            Difference = (j - TargX);
                            tempChar = Board[TargX + Difference, TargY + i];
                            if (tempChar == "-") { j = 99; }
                            else { Pc_Right.Add(Board[TargX + Difference, TargY + i]); }
                        }
                    }
                    try { RightString = Pc_Right.Aggregate((a, b) => a + b); } catch { RightString = ""; }

                    TileCollection = (LeftString + ("" + UserWord[i]) + RightString);
                    if ((LeftString == "") && (RightString == "")) { }
                    else
                    {
                        if (wordHandle.WordCheck(TileCollection)) { PerpScoreTotal += wordHandle.CalculateWordScore(TileCollection); }//for each line that passes the wordcheck, it calculates it scores and adds it to the value PerpScoreTotal
                        else {  }
                    }
                }
            }
            else
            {
                for (int i = 0; i < WordLength; i++)
                {
                    string Val_Above = "-"; string Val_Below = "-";
                    string TileCollection;
                    int Difference = 0;
                    string tempChar;
                    bool ToTheBelow = true;
                    bool ToTheAbove = true;
                    try { Val_Below = Board[TargX + i, TargY + 1]; } catch { ToTheBelow = false; }
                    try { Val_Above = Board[TargX + i, TargY - 1]; } catch { ToTheAbove = false; }
                    Pc_Above.Clear(); Pc_Below.Clear();


                    if (!(Val_Above == "-") && ToTheAbove)
                    {
                        for (int j = TargY - 1; j >= 0 && j < TargY; j--)
                        {
                            Difference = (TargY - j);
                            tempChar = Board[TargX + i, TargY - Difference];
                            if (tempChar == "-") { j = 99; }
                            else { Pc_Above.Add(Board[TargX + i, TargY - Difference]); }
                        }
                    }
                    Pc_Above.Reverse();
                    try { AboveString = Pc_Above.Aggregate((a, b) => a + b); } catch { AboveString = ""; }

                    if (!(Val_Below == "-") && ToTheBelow)
                    {
                        for (int j = TargY + 1; j < 20 && j > TargY; j++)
                        {
                            Difference = (j - TargY);
                            tempChar = Board[TargX + i, TargY + Difference];
                            if (tempChar == "-") { j = 99; }
                            else { Pc_Below.Add(Board[TargX + i, TargY + Difference]); }
                        }
                    }
                    try { BelowString = Pc_Below.Aggregate((a, b) => a + b); } catch { BelowString = ""; }

                    TileCollection = (AboveString + ("" + UserWord[i]) + BelowString);
                    if ((AboveString == "") && (BelowString == "")) { }
                    else
                    {
                        if (wordHandle.WordCheck(TileCollection)) { PerpScoreTotal += wordHandle.CalculateWordScore(TileCollection); }//for each line that passes the wordcheck, it calculates it scores and adds it to the value PerpScoreTotal
                        else {  }
                    }
                }

            }
            return PerpScoreTotal;//returns the total score calulated (PerpScoreTotal)
        }
        /// <summary>
        /// This function uses the Produce a tile function to draw and add 7 random tiles
        /// to the players hand.
        /// </summary>
        public void SetupPlayerHand()
        {
            for (int i = 0; i < 7; i++)//Adds 7 Letters to the Players Hand
            {
                PlayerHand.Add(ProduceTile());//Adds the tile to the users hand
            }
        }
        /// <summary>
        /// This function uses the Produce a tile function to draw and add 7 random tiles
        /// to the Ais hand.
        /// </summary>
        public void SetupAIHand()
        {
            for (int i = 0; i < 7; i++)//Adds 7 Letters to the Ais Hand
            {
                AIHand.Add(ProduceTile());//Adds the tile to the Ais hand
            }
        }
        /// <summary>
        /// This function takes the users move and test to see if they can even make their 
        /// word using the tile on the board and from their hand. its algorithm is similar
        /// to the OverlapCheck function and MakeMove function, it gets the moves already
        /// on the board and makes sure that the users words letters are either in their
        ///  hand or on the board at the target location. If its not in hand or on board
        ///  it increases the flag value by 1. If the flag value at the is 0, it returns true
        ///  anything else its false
        /// </summary>
        /// <param name="WordToMake"></param>
        /// <param name="TargX"></param>
        /// <param name="TargY"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        public bool UserMakeWord(string WordToMake, int TargX, int TargY, char Direction)
        {
            int flags = 0;//Defines and sets the value of flags to 0
            List<string> ExistingTiles = new List<string>();//A list to store existing tiles on the board
            if (Direction == 'H')
            {
                for (int i = 0; i < WordToMake.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX + i, TargY]); }//uses tries incase the word is running of the "edge of the board" (Out of array bounds)
                    catch { ExistingTiles.Add("-"); }
                }
            }//Adds the exsiting tiles to the list (Horizontally)
            else
            {
                for (int i = 0; i < WordToMake.Length; i++)
                {
                    try { ExistingTiles.Add(Board[TargX, TargY + i]); }//uses tries incase the word is running of the "edge of the board" (Out of array bounds)
                    catch { ExistingTiles.Add("-"); }
                }
            }//Adds the exsiting tiles to the list (Vertically)
            for (int i = 0; i < WordToMake.Length; i++)
            {
                if (!(PlayerHand.Contains(""+WordToMake[i])))
                {
                    if((!(ExistingTiles.Contains("" + WordToMake[i]))))
                    { 
                        flags++;//increase the flag value by one
                    }//And the letter is not on the board
                }//If the players hand doesnt contain the words letter
                else if ((!(ExistingTiles.Contains("" + WordToMake[i]))))
                {
                    ListToRemove.Add(""+WordToMake[i]);//Adds to the ListToRemove list (defined in class above), this is so the game know which letters to remove from the players hand
                }//If the Tile isnt on the board but is in the players hand

            }
            if (flags == 0)
            {
                return true;// if the flag value is 0, return true
            }
            else
            {
                return false;// Any other value of flags, return false
            }
        }
        /// <summary>
        /// This function is used to draw and add one tile to the players hand
        /// </summary>
        public void Refill1PlayersHand()
        {
            PlayerHand.Add(ProduceTile());//Adds one produced tile to the players hand
        }
        /// <summary>
        /// This function is used to draw and add one tile to the Ais hand
        /// </summary>
        public void Refill1AIHand()
        {
            AIHand.Add(ProduceTile());//Adds one produced tile to the Ais hand
        }
    }
}
