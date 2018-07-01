using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace WordFun
{
    class WordHandler
    {
        HeaderNode structure;//Stores a reference to the datastructure
        private static String[] data;//Creates a String array to store the data from the WordList Rescource
        public static Dictionary<char, int> LetterScores = new Dictionary<char, int>();//Creates the Table of scores for each letter
        /// <summary>
        /// This constructor fetches the data stored in the project resources so the data 
        /// structure can be made. It also run s the build letter scores function to fill
        /// the LetterScores dictionary.
        /// </summary>
        public WordHandler()
        {
            data = Properties.Resources.WordList.TrimEnd('\n').Split('\n');//Gets the Data from the WordList rescource
            BuildLetterScores();//builds letters scores dictionary
        }
        /// <summary>
        /// This function allows other objects to parse a reference of the data structure
        /// to use to the word handler and set it to its internal reference.
        /// </summary>
        /// <param name="datastructure"></param>
        public void MyStructure(HeaderNode datastructure)
            {
            structure = datastructure;//Sets the local definition for Data structure to the one parsed to it during creation
            }
        /// <summary>
        /// this function goes through every string in the variable data and parses it to
        /// the data structure to be added as a valid word.
        /// </summary>
        public void MakeDict()
        {
            foreach (string value in data)
            {
                structure.BuildWord(value);//tells the data structure to add the value of Value as a word
            }//foreach string in the varaible data

        }
        /// <summary>
        /// This function adds elements to the dictionary to define the score of each 
        /// letter/character
        /// </summary>
        private void BuildLetterScores()
        {
            LetterScores[' '] = 0;
            LetterScores['A'] = 1;
            LetterScores['B'] = 3;
            LetterScores['C'] = 3;
            LetterScores['D'] = 2;
            LetterScores['E'] = 1;
            LetterScores['F'] = 4;
            LetterScores['G'] = 2;
            LetterScores['H'] = 4;
            LetterScores['I'] = 1;
            LetterScores['J'] = 8;
            LetterScores['K'] = 5;
            LetterScores['L'] = 1;
            LetterScores['M'] = 3;
            LetterScores['N'] = 1;
            LetterScores['O'] = 1;
            LetterScores['P'] = 3;
            LetterScores['Q'] = 10;
            LetterScores['R'] = 1;
            LetterScores['S'] = 1;
            LetterScores['T'] = 1;
            LetterScores['U'] = 1;
            LetterScores['V'] = 4;
            LetterScores['W'] = 4;
            LetterScores['X'] = 8;
            LetterScores['Y'] = 4;
            LetterScores['Z'] = 10;
        }
        /// <summary>
        /// this function gets the score for each character in a string parsed to it and
        /// adds their total value together using the Score Dictionary.
        /// </summary>
        /// <param name="UserWord"></param>
        /// <returns></returns>
        public int CalculateWordScore(string UserWord)
        {
            int ScoreCalc = 0;//Defines ScoreCalc
            for (int i = 0; i < UserWord.Length; i++)
            {
                ScoreCalc = ScoreCalc + LetterScores[Convert.ToChar(UserWord[i])];//Adds the Score value of each letter to scorecalc
            }//For Each letter of the word
            return ScoreCalc;//returns The Final score (ScoreCalc)
        }
        /// <summary>
        /// This function will begin the process of wordchecking going through the header
        /// node in the datastructure. It will return true if the word is valid, false if
        /// it is not
        /// </summary>
        /// <param name="Word"></param>
        /// <returns></returns>
        public bool WordCheck(string Word)
        {
            Word = Word.ToUpper();//Converts the Word to Upper Case
            string Character = "" + Convert.ToChar(Word[0]);//Gets the first Character of the word
            int TargetLocation = Word[0] - 65;//Gets the Target Location in the Array
            Node Child = (Node)structure.HeaderArray[TargetLocation];//Gets a Reference to the Target Node in the array
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
    }
}
