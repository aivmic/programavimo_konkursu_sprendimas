﻿using System;
using System.Collections.Generic;
using NLog;

namespace Solutions
{
    /// <summary>
    /// Solution for Metai Regionas.
    /// </summary>
    public class AZ01201701U2
    {
        /// <summary>
        /// A single input.
        /// </summary>
        public class Input
        {
            public string[] Sequences { get; set; }
        }

        /// <summary>
        /// A single output.
        /// </summary>
        public class Output
        {
            public List<string> Results { get; set; } = new List<string>();
        }

        /// <summary>
        /// Test data. You can use a different layout if you want.
        /// </summary>
        public class TestData
        {
            public static Input[] Inputs { get; } = {
                new Input
                {
                    Sequences = new string[]
                    {
                        "abbbaaacaaaccddaabbb",
                        "abbbaaacKLKccddaabbb",
                        "absabsabsabsbabsabsa"
                    }
                }
            };

            public static Output[] Outputs { get; } = {
                new Output
                {
                    Results = new List<string>
                    {
                        "Max ilgis = 4 simbolis a\nPertvarkyta eilė tuščia.",
                        "Max ilgis = 4 simbolis a\ncKLK",
                        "Nebuvo metamos sekos.\nabsabsabsabsbabsabsa"
                    }
                }
            };
        }

        /// <summary>
        /// Logger for this class.
        /// </summary>
        Logger mLog = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Runs the task solution.
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Output</returns>
        public Output Run(Input input)
        {
            var output = new Output();
            foreach (var sequence in input.Sequences)
            {
                mLog.Info($"Processing sequence: {sequence}");
                string result = ProcessSequence(sequence);
                output.Results.Add(result);
                mLog.Info($"Result for sequence: {result}");
            }
            return output;
        }

        /// <summary>
        /// Processes a single sequence of blocks and returns the result as a string.
        /// </summary>
        /// <param name="sequence">Sequence of blocks</param>
        /// <returns>Processed result</returns>
        private string ProcessSequence(string sequence)
        {
            string originalSequence = sequence;
            int longestGroupLength = 0;
            string longestGroupChar = "";
            bool sequenceModified = false;

            while (true)
            {
                var (newSequence, groupChar, groupLength) = RemoveFirstGroup(sequence);
                if (groupLength > longestGroupLength)
                {
                    longestGroupLength = groupLength;
                    longestGroupChar = groupChar;
                }

                if (groupLength == 0) break;

                sequence = newSequence;
                sequenceModified = true;
                mLog.Info($"Removed group of {groupLength} {groupChar}. New sequence: {newSequence}");
            }

            if (longestGroupLength > 1)
            {
                string result = $"Max ilgis = {longestGroupLength} simbolis {longestGroupChar}\n";
                result += string.IsNullOrEmpty(sequence) ? "Pertvarkyta eilė tuščia." : sequence;
                return result;
            }
            else
            {
                return $"Nebuvo metamos sekos.\n{originalSequence}";
            }
        }

        /// <summary>
        /// Removes the first group of consecutive identical characters from the sequence.
        /// </summary>
        /// <param name="sequence">The input sequence</param>
        /// <returns>New sequence without the first group, the character of the group, and the group's length</returns>
        private (string, string, int) RemoveFirstGroup(string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return (sequence, "", 0);

            int maxLength = 0;
            string groupChar = "";
            int startIndex = -1;

            // Find the first group of consecutive identical characters
            for (int i = 0; i < sequence.Length - 1; i++)
            {
                int j = i;
                while (j < sequence.Length && sequence[j] == sequence[i]) j++;

                int length = j - i;
                if (length > 1)
                {
                    startIndex = i;
                    maxLength = length;
                    groupChar = sequence[i].ToString();
                    break;
                }
            }

            if (maxLength == 0) return (sequence, "", 0);

            // Remove the found group
            string newSequence = sequence.Remove(startIndex, maxLength - 1);
            return (newSequence, groupChar, maxLength);
        }
    }
}
