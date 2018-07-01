using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace WordFun
{
    class AI
    {
        private HeaderNode MyDataStructure;
		private GameEngine MyGame;
        /// <summary>
        /// This is a class contructor that allows me to pass A reference of the word handlers' data
        /// structure and the game engine. The constructor then parses the parameters to the classes
        /// private varaibles so that they can be used througout the class.
        /// </summary>
        /// <param name="DataStructure"></param>
        /// <param name="Game"></param>
        public AI(HeaderNode DataStructure, GameEngine Game )
        {
            MyDataStructure = DataStructure;//Sets Private variable MyDataStructure to the Parameter DataStructure
			MyGame = Game;//Sets the Private variable MyGame to the parameter Game
            Console.WriteLine("AI STARTED SUCCESSFULLY - With a DataStructure and GameEngine!");//Writes to the Debug console that the Ai was Initiated correctly 
        }
        /// <summary>
        /// This function takes the input of a move (in string form) and then calculates and 
        /// returns the total score the move would earn for the Ai.
        /// </summary>
        /// <param name="Move"></param>
        /// <returns></returns>
        public int CalcTotalScore(string Move)
        {
            List<string> AMove = BreakMoveIntoList(Move);//Uses to break a move function to store the move elements in a list
            int a = MyGame.ScoreOnTargetedPlane(AMove[3], int.Parse(AMove[0]), int.Parse(AMove[1]), AMove[2][0]);//Stores the score calulated on the targeted plane, including the word to placed
            int b = MyGame.PerpScore(AMove[3], int.Parse(AMove[0]), int.Parse(AMove[1]), AMove[2][0]);//Stores the score calculated on the lines/planes perpendicular to the target word location
            return (a + b);//Returns the total score (ScoreOnTargetedPlane + PerpScore)
        }
        /// <summary>
        /// This is function performs the same as the CalcTotalScore function, it takes the 
        /// input of a move (in string form) and then calculates and returns the total score
        /// the move would earn for the Ai. The only difference is this function is to be used
        /// when the break function returns a slightly different list due to data to the
        /// data being manipulated in different functions
        /// </summary>
        /// <param name="Move"></param>
        /// <returns></returns>
        public int CalcFinalScore(string Move)
        {
            List<string> AMove = BreakMoveIntoList(Move);//Uses to break a move function to store the move elements in a list
            int a = MyGame.ScoreOnTargetedPlane(AMove[4], int.Parse(AMove[1]), int.Parse(AMove[2]), AMove[3][0]);//Stores the score calulated on the targeted plane, including the word to placed
            int b = MyGame.PerpScore(AMove[4], int.Parse(AMove[1]), int.Parse(AMove[2]), AMove[3][0]);//Stores the score calculated on the lines/planes perpendicular to the target word location
            return (a + b);//Returns the total score (ScoreOnTargetedPlane + PerpScore)
        }
        /// <summary>
        /// This function will take a move (in string form) and then break and return the list it creates.
        /// this is so that different parts of the move can be parsed on into other functions as just chars,
        /// strings or integers.
        /// </summary>
        /// <param name="Move"></param>
        /// <returns></returns>
        private List<string> BreakMoveIntoList(string Move)
        {
            List<string> AMove = new List<string>();//Creates a new list called AMove
            for (int i = 0; i < (Move.Split('/').Count()); i++)
            {
                AMove.Add(Move.Split('/')[i]);//
            }//For each element in the array created by (Move.Split('/').Count()) it add the data to the A Move list
            return AMove;//Returns the list AMove
        }
        /// <summary>
        /// This function takes a string of letters (all the Ais Tile Letters) and calulates 
        /// all possible words that can be made from the letters avaliable. it does this
        /// by parsing the letters to the data structure for it then to map all different
        /// combinations of the tiles to the tree.
        /// 
        /// There is an issue with the data structure that i was unable to identify the cause of.
        /// When the data structure generated all the words the all duplicated the first letter 
        /// of the word for an unknown reason, while unable to find the root of this issue, i 
        /// could work aroung it by removing the duplicated letter from the front of each word
        /// in the list.
        /// </summary>
        /// <param name="Letters"></param>
        /// <returns></returns>
        public List<string> GenerateAllPossibleWords(string Letters)
        {
            List<string> PossibleWordsList = MyDataStructure.GenerateWord(Letters);//stores the the data (Word list) from the function (GenerateWord(Letters) in a list
            for (int i = 0; i < PossibleWordsList.Count; i++)
            {
                PossibleWordsList[i] = PossibleWordsList[i].Remove(PossibleWordsList[i].Length - 1);
            }//for each item in the list, it will remove the first letter of the word to correct the error in the data structures calculation by triming the first letter and returning it to the list
            return PossibleWordsList;//Returns the corrected list
        }
        /// <summary>
        /// In this function the AiS letters are parsed from external classes to actually make 
        /// a move (This function uses all the other functions found in this class to make a move).
        /// It does this by first generating all possible moves, it then parsed these possible
        /// words into a generateAllMovesForAllWords to try and generate all the moves possible.
        /// If it failes to do this then this means there is no moves possible for the ai to make.
        /// if there is an avaliable move the ai will tell the game engine to make the move.
        /// 
        /// The Function also uses a GrabATile variable to check if the ai has actually used any of its
        /// tiles, this is a relic when one of the newer move conditions was not introduced (DrawnATile).
        /// I left the check still in as a backup to prevent the ai being caught in a logic loop. This
        /// is where the ai would make its best move without using any of its tiles and not drawing any
        /// new tiles, meaning the ai would be stuck making the same move over and over again.
        /// </summary>
        /// <param name="Letters"></param>
        /// <returns></returns>
        public int CalculateAndMakeBestMove(string Letters)
        {
            var Words = GenerateAllPossibleWords(Letters);//Stores the list of all possible words Generated from the parameter "Letters"
            int GrabATile = 0;//defines and sets the GrabATile variable to 0
            try
            {
                Dictionary<string, int> MoveDict = GenerateAllMovesForAllWords(Words);//Stores the moves in a dictionary (string as a key and the score of the move as a value)
                List<string> BrokenMove = BreakMoveIntoList("" + MoveDict.First().Key);//this stores the best move from the dictionary as a broken move (a list) to the varaible BrokenMove
                GrabATile = MyGame.AI_MakeMove(BrokenMove[4], int.Parse(BrokenMove[1]), int.Parse(BrokenMove[2]), BrokenMove[3][0]);//This tells the game engine to make the move stored int BrokenMove and stores the return value to GrabATile
                if (GrabATile != 0)
                {
                    MyGame.Refill1AIHand();//Tells the Game engine to draw a new tile and add it to the Ais hand
                }//It will only draw a new tile for the ai if no tiles were taken from the Ais hand
                Console.WriteLine("WordMade= " + BrokenMove[4] + " ------------------------------------------------------");//This writes to the debug console the word that was chosen for the move
                return CalcFinalScore("" + MoveDict.First().Key);//This return the final score for the Ais Move
            }//To catch an exception due to no moves being avaliable
            catch (Exception e)
            {
                Console.WriteLine("NoMovesAvaliable");
                Console.WriteLine("Exception = " + e.ToString());
                return 0;
            }


        }
        /// <summary>
        /// This function will calulate all possible moves for all the words parsed by the List
        /// Words (from parameter). it does this by running the GenerateAllMovesForAWord function
        /// on every word in the Words List and stores it to the Dictionary with their score 
        /// value, this allows the list to be sorted from highest to lowest value.
        /// </summary>
        /// <param name="Words"></param>
        /// <returns></returns>
        private Dictionary<string, int> GenerateAllMovesForAllWords(List<string> Words)
        {
            Dictionary<string, int> MovesWithScore = new Dictionary<string, int>();
            foreach(string Word in Words)
            {
                var MoveList = GeneratePossibleMovesForAWord(Word);//Stores all the generated moves for that one word
                foreach (string Move in MoveList)
                    {
                        List<string> MoveBroken = Move.Split('/').ToList();
                        try
                        {
                            MovesWithScore.Add("/" + MoveBroken[0] + "/" + MoveBroken[1] + "/" + MoveBroken[2] + "/" + MoveBroken[3], CalcTotalScore(Move));//Calulates score using CalcTotalScore function and stores move with score in Dictionary
                        }
                        catch {}
                    }//For each Move in the list it calculates it score and adds it to the MovesWithScore Dictionary
            }//runs the code within for each word in the List "Words"
            var OrderedDict = (MovesWithScore.OrderBy(x => x.Value)).Reverse();//Sorts the list by Ascending order of the scores and stores to a var (due to it being an enumarable Dictionary)
            Dictionary<string, int> ToReturnSortedDict = new Dictionary<string,int>();//Creates a dictionary from the OrderedDict (type Enurable Dictionary) so that the contents can be stored correctly and returned
            foreach (KeyValuePair<string, int> KeyValue in OrderedDict)
            {
                ToReturnSortedDict.Add(KeyValue.Key,KeyValue.Value);
            }//foreach item in the OrderedDict, it adds it to the ToReturnDict Dictionary
            return ToReturnSortedDict;//Returns OrderedDict
        }
        /// <summary>
        /// Generates all possible moves for one word. It does this by attempting to place a 
        /// word in every possible location on the board for each word in the list. While Time
        /// consuming this is the only method i found to work to produce all possible moves. 
        /// It checks if the moves are valid by using the MoveCheck function in the game 
        /// engine.
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        private List<string> GeneratePossibleMovesForAWord(string Word)
        {
            List<string> PossiblePositions = new List<string>();//Creates a list to store the possible moves
            for (int x = 0; x < (15 - (Word.Length + 1)); x++)
            {
                for (int y = 0; y < (15 - (Word.Length + 1)); y++)
                {
                    if (MyGame.MoveCheck(Word, x, y, 'H') == "PASS")
                    {
                        PossiblePositions.Add(x + "/" + y + "/" + "H/" + Word + "/");
                    }//if the move check on the Horizontal passes, it adds the move to the list
                    if (MyGame.MoveCheck(Word, x, y, 'V') == "PASS")
                    {
                        PossiblePositions.Add(x + "/" + y + "/" + "V/" + Word + "/");
                    }//if the move check on the Vertical passes, it adds the move to the list
                }//for every y coordinate
            }//for every X coordinate
            return PossiblePositions;//returns the list of possible moves
        }
        /// <summary>
        /// This function is a debug function that i created that would generate all possible
        /// words and list them, with a PASS message if the are real words. This was used in 
        /// testing to see if the AI could actually generate words.
        /// </summary>
        /// <param name="Letters"></param>
        public void D_GenerateAllPossibleWords(string Letters)
        {
            List<string> PossibleWordsList = MyDataStructure.GenerateWord(Letters);//Creates a list to store the words
            for (int i = 0; i < PossibleWordsList.Count; i++)
            {
                PossibleWordsList[i] = PossibleWordsList[i].Remove(PossibleWordsList[i].Length - 1);
            }//Trims the duplicate lettes from the words
            Console.WriteLine("You gave me these letters: " + Letters);
            Console.WriteLine("I made this list of Words");
            Console.WriteLine("LIST START");
            foreach (string Word in PossibleWordsList)
            {
                Console.Write(Word + " - ");
                Console.Write("WordCheck, Real Word?" + MyDataStructure.InternalWordCheck(Word));
                Console.WriteLine();
            }//For each word in the list it writes the pass if the word is real
            //Test.ForEach(Console.WriteLine);
            Console.WriteLine("LIST END");
        }
    }
}
