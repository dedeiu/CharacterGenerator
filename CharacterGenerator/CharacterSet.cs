using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CharacterGenerator
{
    public class CharacterSet
    {
        #region private variables
        private int charLength = 7;
        private int charByteStringLength = 0;
        private int loopDelay = 1;
        /// <summary>
        /// Current BuildCharSet(int combination) method will work only with a length of 32 elements 
        /// for char[] characters. However if you change the length, a new algorithm
        /// implementation for BuildCharSet(int combination) method is needed.
        /// I.E.: For a length of 8 character / code you need 8 loop, or even better
        /// a recursive method.
        /// </summary>
        private char[] characters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
                                      'L', 'M', 'N', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 
                                      'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', 
                                      '8', '9'};
        private double charNumber = 0;
        private StreamWriter file;
        private string filePath;
        private int charArraySize = 1000;
        private Random randomCombination = new Random();
        #endregion

        #region public variables
        public int CharLength
        {
            get
            {
                return this.charLength;
            }
            set
            {
                if (value <= 0) throw new ArgumentException("CharLength must be a positive integer. It should be uint, i know.");

                this.charLength = value;

                this.charNumber = Math.Pow(this.characters.Length, this.charLength);
            }
        }
        public double CharNumber
        {
            get
            {
                return this.charNumber;
            }
            set
            {
                this.charNumber = value;
            }
        }
        #endregion

        #region constructor
        public CharacterSet()
        {
            this.charByteStringLength = 5;
            this.loopDelay = 99;
            this.filePath = System.AppDomain.CurrentDomain.BaseDirectory + string.Format(@"{0}", Guid.NewGuid());
        }
        #endregion

        #region public methods
        public void Generate()
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            this.file = new StreamWriter(this.filePath);

            this.file.WriteLine("Started at: - {0}", sw.Elapsed);

            string[] charArray = new string[this.charArraySize];

            int index = 0;
            int combination = 0;
            for (int i = 0; i < this.charNumber; i++)
            {
                if ((index % this.charArraySize) == 0 && index != 0)
                {
                    System.Console.WriteLine("Write {0} to file {1}.", i, this.filePath);
                    this.SaveCodes(charArray);
                    charArray = new string[this.charArraySize];
                    index = 0;
                }

                var limit = combination + this.loopDelay;
                combination = this.randomCombination.Next(combination, limit);
                charArray[index] = this.BuildCharSet(combination);
                combination = limit + 1;
                index++;
            }
            this.SaveCodes(charArray);

            sw.Stop();
            this.file.WriteLine("Finished at: - {0}", sw.Elapsed);
            this.file.Close();
        }
        #endregion

        #region private methods
        private string BuildCharSet(Int64 combination)
        {
            string byteString = String.Empty;
            byteString = Convert.ToString(combination, 2);
            byteString = byteString.PadLeft(this.charByteStringLength * this.charLength, '0');

            string charString = String.Empty;
            for (var i = 0; i < byteString.Length; i = i + this.charByteStringLength)
            {
                var byteStringChunk = byteString.Substring(i, this.charByteStringLength);
                var stringChunk = this.BuildHash(byteStringChunk);
                charString = String.Format("{0}{1}", charString, stringChunk);
            }


            return charString;
        }

        private char BuildHash(string byteString)
        {
            int characterIndex = Convert.ToInt32(byteString, 2);
            char sequence = this.characters[characterIndex];
            return sequence;
        }

        private void SaveCodes(string[] charArray)
        {
            foreach (string charCode in charArray)
            {
                this.file.WriteLine(charCode);
            }
        }
        #endregion
    }
}
