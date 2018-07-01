using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WordFun
{
    class Node
    {
        public string MyValue;//the letter this node stores
        public object[] MyChildArray = new object[26];//defines the array to allow subnodes to be stores/connected below this node
        public bool Terminator = false;//defines and sets the terminator value to false, this is so the structure can no if its the end of a word
        /// <summary>
        /// This constructor sets the Varaible MyValue of the object being created to
        /// the parameter being parsed to it (Letter)
        /// </summary>
        /// <param name="Letter"></param>
        public Node(string Letter)
        {
            MyValue = Letter;//Sets MyValue to Letter
        }
        /// <summary>
        /// this function will break the word that is parsed to it into a head and tail, the
        /// head is used to see if there is an existing subnode with the same MyValue.
        /// if there is it parses the tail to it and continues the process, if it does not
        /// it creates a sub node with that letter (head value) and parses the tail to the
        /// new node. If there is no letters left to parse, it sets the current active nodes
        /// terminator value to true.
        /// </summary>
        /// <param name="Word"></param>
        public void BuildWord(string Word)
        {
            if (Word != "")
            {
                int TargetLocation = (int)Word[0] - 65;//gets the index for the array to get the correct target subnode
                if (MyChildArray[TargetLocation] == null)
                {
                    MyChildArray[TargetLocation] = new Node("" + Convert.ToChar(Word[0]));//creates a new subnode with the letter value
                }//if the target location in the array = null
                Word = Word.Remove(0, 1);//Gets the tail of the word
                Node Child = (Node)MyChildArray[TargetLocation];//gets a reference to the target node
                Child.BuildWord(Word);//parses remaining word to the next sub target node
            }//Checks if there is any remaining letters of the word left to parse.
            else { Terminator = true; }//If not set this nodes terminator value to true
        }
        /// <summary>
        /// this function will keep parsing the tails of a string from node to subnode until there
        /// is no word left; if the current nodes terminator value is true it returns true, if not
        /// it returns false.
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        public bool WordCheck(string Word)
        {
            if (Word != "")
            {
                Word = Word.ToUpper();//converts word to upper, just to avoid errors
                string Character = "" + Convert.ToChar(Word[0]);//gets the first letter in the string in Word as a string
                int TargetLocation = Word[0] - 65;//gets the Index of the target node in the sub nodes array
                Node Child = (Node)MyChildArray[TargetLocation];//gets a reference to the target node
                if (Child != null)
                {
                    if (Child.MyValue == Character)
                    {
                        Word = Word.Remove(0, 1);//gets the words tail
                        return Child.WordCheck(Word);//parse the tail of of the word to the target sub node
                    }//if the element in the array is null, return false as the word cannot be mapped on the structure - it must not be real
                    else
                    {
                        return false;
                    }
                }//if the target node is not null
                else { return false; };//if it is, the Word to check must not be real; return false
            }//While the string being parsed is not empty, keep parsing
            else
            {
                if (Terminator)
                { return true; }//return true if this nodes terminator value is true
                else
                { return false; }//return false if this nodes terminator value is false
            }//if there is no word left
        }
        /// <summary>
        /// This function is exactly the same as the function GenerateWord from the header
        /// node class except it references a different array of objects and performs a
        /// terminator check (code that is different from the other class is documented)
        /// </summary>
        /// <param name="Letters"></param>
        /// <returns></returns>
        public List<string> GenerateWord(string Letters)
        {
            List<string> ChildNodeLetterAvaliable = new List<string>();
            List<string> PossibleWords = new List<string>();
            for (int i = 0; i < 26; i++)
            {
                try
                {
                    Node Child = (Node)MyChildArray[i];//Creates reference to a node in the MyChildArray
                    ChildNodeLetterAvaliable.Add(Child.MyValue);
                }
                catch
                {
                    ChildNodeLetterAvaliable.Add("-");
                }
            }
            foreach (char Letter in Letters)
            {
                string CurrentLetter = "" + Letter;
                List<string> tempPossibleWords = new List<string>();
                if (ChildNodeLetterAvaliable.Contains(CurrentLetter) == true)
                {
                    int Index = ChildNodeLetterAvaliable.IndexOf(CurrentLetter);
                    Node NodeToParse = (Node)MyChildArray[Index];
                    var RegexPattern = new Regex(Regex.Escape(CurrentLetter));
                    string LettersToParse = RegexPattern.Replace(Letters, String.Empty, 1);
                    tempPossibleWords.AddRange(NodeToParse.GenerateWord(LettersToParse));
                    for (int j = 0; j < tempPossibleWords.Count(); j++)
                    {
                        tempPossibleWords[j] = CurrentLetter + tempPossibleWords[j];
                    }
                }
                PossibleWords.AddRange(tempPossibleWords);
            }
            if (this.Terminator == true)
            {
                PossibleWords.Add(this.MyValue);
            }//if this nodes terminator value is true, add its MyValue to possibleWords
            return PossibleWords;
        }
    }
}
