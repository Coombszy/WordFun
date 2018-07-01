using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordFun
{
    class HeaderNode
    {
        private static WordHandler MyWordHandler = new WordHandler();//Creates an Instance of the WordHandler Class, This is used to Build the DataStructure
        public static Dictionary<char, int> LetterScores = new Dictionary<char, int>();//Creates the Table of scores for each letter
        public object[] HeaderArray = new object[26];//creates and Obeject array used To store The ChildNodes so that they can referenced
        /// <summary>
        /// This constructor tells the object/class on creation to run the CreateChildNodesFunction()
        /// </summary>
        public HeaderNode()
        {
            CreateChildNodes();//Build All the Child Nodes on HeaderNode Definition
        }
        /// <summary>
        /// This function will fill each slot of the header array with and object (Node class)
        /// as well as setting the nodes its storing to their corrosponding letter.
        /// </summary>
        private void CreateChildNodes()
        {
            for (int i = 0; i < 26; i++)
            {
                HeaderArray[i] = new Node("" + Convert.ToChar(65 + i));//Sets the element in the array to a new instance of Node with the correct letter
            }//Fills all 26 Slots with a Node and its Correct Alphabet value
        }
        /// <summary>
        /// this is a debug function that will go through the HeaderArray and write the 
        /// object type, objects.MyValue of each element in the array. This allows me
        /// to make sure that the array is filled correctly.
        /// </summary>
        /// <param name="array"></param>
        public void D_WriteArray(object[] array)
        {
            int i = 0;//defines and sets the counter i to 0
            foreach (object element in array)
            {
                if (element != null)
                {
                    Node Child = (Node)array[i];//Gets a reference to the Node in position i
                    Console.WriteLine(element.GetType());//Prints to console the type of the Object in the Array element
                    Console.WriteLine("MyValue="+Child.MyValue);//Prints the value stored in the Node in that Element
                    i++;
                    Console.WriteLine("---");
                }//Error catch if the Value in the Array is Null
            }//Repeats for each Node/Element in the array
        }
        /// <summary>
        /// This function takes the tail of a node and parses it to the node in the array
        /// that has the same value as the header value. This is so the node can build all
        /// the sub nodes.
        /// </summary>
        /// <param name="Word"></param>
        public void BuildWord(string Word)
        {
            Word = Word.ToUpper();//Makes the word Upper case
            int TargetLocation = Word[0] - 65;//Uses the First Letter for the First Target Node
            Word = Word.Remove(0, 1);//Removes the first letter
            Node Child = (Node)HeaderArray[TargetLocation];//Gets a reference to the target Node
            Child.BuildWord(Word);//Parses the Remaining Word to the Target Child Node to Continue the Process
        }//Adds a New Word to the Data Structure
        /// <summary>
        /// This function also takes the tail of the word and attempts to parse it to the 
        /// sub nodes, although this returns false if non of the sub nodes contain the 
        /// letter/character being parsed
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        public bool InternalWordCheck(string Word)
        {
            Word = Word.ToUpper();//Converts the Word to Upper Case
            string Character = "" + Convert.ToChar(Word[0]);//Gets the first Character of the word
            int TargetLocation = Word[0] - 65;//Gets the Target Location in the Array
            Node Child = (Node)HeaderArray[TargetLocation];//Gets a Reference to the Target Node in the array
            if (Child.MyValue == Character)
            {
                Word = Word.Remove(0, 1);//Removes to first Letter from the Word
                return Child.WordCheck(Word);//Parses to the Child node
            }//If the Target Nodes Value Matches the First Character, It parses the word to the Child Node to Continue the Process
            else
            {
                return false;
            }//Return False if the First Character does not match any of the Child Nodes Characters in the Array
        }
        /// <summary>
        /// this function returns the value of CalculateWordScore if the word parses the
        /// InternalwordCheck function.
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        public int WordCheckAndCalcScore(string Word)
        {
            if (InternalWordCheck(Word))
            {
                return MyWordHandler.CalculateWordScore(Word);//return the value of WordHandler CalculateWordScore
            }//InternalWordCheck to make sure its a valid word
            else { return 0; }//If not return 0
        }
        /// <summary>
        /// This function parses the letters minus one of the chars to the sub nodes to see
        /// if they will return any possible words, if they do it adds the letter back to them
        /// to complete the word and adds them to a list, once the functionhas made all possible
        /// words it returns the list.
        /// </summary>
        /// <param name="Letters"></param>
        /// <returns></returns>
        public List<string> GenerateWord(string Letters)
        {
            List<string> ChildNodeLetterAvaliable = new List<string>();//Creates a new list to store the letters avaliable in the sub nodes
            List<string> PossibleWords = new List<string>();//A list to store the possible words
            for (int i = 0; i < 26; i++)
            {
                try
                {
                    Node Child = (Node)HeaderArray[i];//Creates reference to a node in the HeaderArray
                    ChildNodeLetterAvaliable.Add(Child.MyValue);//Adds to the list the value stored in the node
                }
                catch
                {
                    ChildNodeLetterAvaliable.Add("-");//if the node doesnt exist it will fail the try and be caught here, where it will add "-" to the list instead
                }
            }//this will add the letters of all the subnodes in HeaderArray to the list ChildNodeLetterAvaliable
            foreach (char Letter in Letters)
            {
                string CurrentLetter = "" + Letter;//Gets the current letter as a string
                List<string> tempPossibleWords = new List<string>();//creates a temporary list for possible words
                if (ChildNodeLetterAvaliable.Contains(CurrentLetter) == true)
                {
                    int Index = ChildNodeLetterAvaliable.IndexOf(CurrentLetter);//gets the location of the target node for that letter
                    Node NodeToParse = (Node)HeaderArray[Index];//Gets a reference to the correct node from HeaderArray using the calculated index
                    var RegexPattern = new Regex(Regex.Escape(CurrentLetter));//Creates a regex pattern of the Current letter
                    string LettersToParse = RegexPattern.Replace(Letters, String.Empty, 1);//uses the regex pattern to remove the current letter from the string of letters
                    tempPossibleWords.AddRange(NodeToParse.GenerateWord(LettersToParse));//parses the letters remaining to the targetnode to repeat the process, add the list it returns to the tempPossibleWords
                    for (int j = 0; j < tempPossibleWords.Count(); j++)
                    {
                        tempPossibleWords[j] = CurrentLetter + tempPossibleWords[j];
                    }//This then adds the current letter to the strings returned in the list so complete the words.
                }//if the one of the subnodes contains the Current letter
                PossibleWords.AddRange(tempPossibleWords);//Adds tempPossiblewords to Possible words
            }//for each letter in the letters being parsed
            return PossibleWords;//returns all the possible words
        }
    }
}