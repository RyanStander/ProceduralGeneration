using System;
using System.Text;
using UnityEngine;
using TownGeneration.Rules;

namespace TownGeneration
{
    public class LSystemGenerator : MonoBehaviour
    {
        public Rule[] rules;
        public string rootSentence;
        [Range(0,10)]
        public int iterationLimit = 1;

        private void Start()
        {
            Debug.Log(GenerateSentence());
        }

        public string GenerateSentence(string givenWord = null)
        {
            //if given word is null set it ot the root sentence
            givenWord ??= rootSentence;

            //return a recursive string
            return GrowRecursive(givenWord);
        }

        private string GrowRecursive(string givenWord, int iterationIndex=0)
        {
            //if it has reached the limit or exceeded, return he final string
            if (iterationIndex>=iterationLimit)
            {
                return givenWord;
            }
            //otherwise create a string builder and add new words
            var newWord = new StringBuilder();

            //goes over each letter and ads to it
            foreach (var letter in givenWord)
            {
                newWord.Append(letter);
                ProcessRulesRecursively(newWord, letter, iterationIndex);
            }

            //return the new word
            return newWord.ToString();
        }

        private void ProcessRulesRecursively(StringBuilder newWord, char letter, int iterationIndex)
        {
            //goes through all rules and finds a matching case and appends the result of a rule to each matching letter in the new word
            foreach (var rule in rules)
            {
                if (rule.letter==letter.ToString())
                {
                    newWord.Append(GrowRecursive(rule.GetResult(),iterationIndex+1));
                } 
            }
        }
    }
}
