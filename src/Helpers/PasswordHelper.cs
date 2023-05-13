/*
 * Talegen ASP.net Core Web Library
 * (c) Copyright Talegen, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

namespace Talegen.AspNetCore.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Talegen.AspNetCore.Web.Helpers.Internal;

    /// <summary>
    /// Contains an enumerated list of available password complexity levels.
    /// </summary>
    public enum PasswordComplexityLevel
    {
        /// <summary>
        /// No complexity level.
        /// </summary>
        None,

        /// <summary>
        /// Low complexity.
        /// </summary>
        Low,

        /// <summary>
        /// Medium complexity.
        /// </summary>
        Medium,

        /// <summary>
        /// High complexity.
        /// </summary>
        High
    }

    /// <summary>
    /// Contains an enumerated list of canonicalization types.
    /// </summary>
    public enum CanonicalizationType
    {
        /// <summary>
        /// Canonicalize everything.
        /// </summary>
        Everything = 0,

        /// <summary>
        /// Canonicalize letters only.
        /// </summary>
        LettersOnly = 1
    }

    /// <summary>
    /// Contains an enumeration of password character types.
    /// </summary>
    public enum PasswordCharType
    {
        /// <summary>
        /// Upper-case characters.
        /// </summary>
        UpperLetter = 0,

        /// <summary>
        /// Lower-case characters.
        /// </summary>
        LowerLetter = 1,

        /// <summary>
        /// Numeric 0-9 characters.
        /// </summary>
        Numeric = 2,

        /// <summary>
        /// Contains all special characters.
        /// </summary>
        Special = 3
    }

    /// <summary>
    /// This class offers methods used for password format validation, complexity calculation, and generation.
    /// </summary>
    public class PasswordHelper
    {
        #region Private Constants

        /// <summary>
        /// Contains the available lower-case characters
        /// </summary>
        private const string PasswordCharactersLowerCase = "qwertyuiopasdfghjklzxcvbnm";

        /// <summary>
        /// Contains the available upper-case characters
        /// </summary>
        private const string PasswordCharactersUpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";

        /// <summary>
        /// Contains the available numeric characters
        /// </summary>
        private const string PasswordCharactersNumeric = "0123456789";

        /// <summary>
        /// Contains the available special characters
        /// </summary>
        private const string PasswordCharactersSpecial = "!@#$%^&*?"; // ()_-+=[{]};:>|./";

        /// <summary>
        /// Contains the lower-case character array group index
        /// </summary>
        private const int LowerGroupIndex = 0;

        /// <summary>
        /// Contains the upper-case character array group index
        /// </summary>
        private const int UpperGroupIndex = 1;

        /// <summary>
        /// Contains the numeric character array group index
        /// </summary>
        private const int NumericGroupIndex = 2;

        /// <summary>
        /// Contains the special character array group index
        /// </summary>
        private const int SpecialGroupIndex = 3;

        #endregion

        #region Private Fields

        /// <summary>
        /// Contains an instance of the password lookup dictionary
        /// </summary>
        private readonly PasswordDictionary dictionary;

        /// <summary>
        /// Contains an instance of character similarity map
        /// </summary>
        private readonly SimilarityMap similarityMap;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHelper" /> class.
        /// </summary>
        public PasswordHelper()
            : this(new PasswordHelperOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHelper" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public PasswordHelper(PasswordHelperOptions options)
        {
            this.dictionary = new PasswordDictionary();
            this.similarityMap = new SimilarityMap();
            this.Options = options;
        }

        #endregion

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public PasswordHelperOptions Options { get; }

        #region Public Methods

        /// <summary>
        /// This method is used to determine if a password meets the settings length standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsLengthStandards(string password)
        {
            return this.MeetsLengthStandards(password, this.Options.MinimumLength, this.Options.MaximumLength);
        }

        /// <summary>
        /// This method is used to determine if a password meets the settings length standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <param name="minimumLength">Contains the minimum number of characters</param>
        /// <param name="maximumLength">Contains the maximum number of characters</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsLengthStandards(string password, int minimumLength, int maximumLength)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= minimumLength && password.Length <= maximumLength;
        }

        /// <summary>
        /// This method is used to determine if a password meets the settings complexity standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsComplexityStandards(string password)
        {
            return this.MeetsComplexityStandards(password, this.Options.MinimumComplexityLevel);
        }

        /// <summary>
        /// This method is used to determine if a password meets the settings complexity standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <param name="minimumComplexity">Contains the minimum complexity to meet</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsComplexityStandards(string password, PasswordComplexityLevel minimumComplexity)
        {
            return !string.IsNullOrWhiteSpace(password) && this.CalculateComplexity(password) >= minimumComplexity;
        }

        /// <summary>
        /// This method is used to determine if a password meets all the settings format standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsFormatStandards(string password)
        {
            return this.MeetsFormatStandards(password, this.Options.RequiredLowerCaseCount, this.Options.RequiredUpperCaseCount, this.Options.RequiredNumericCount, this.Options.RequiredSpecialCount, this.Options.MinimumComplexityLevel, this.Options.MinimumLength, this.Options.MaximumLength);
        }

        /// <summary>
        /// This method is used to determine if a password meets all the settings format standards
        /// </summary>
        /// <param name="password">Contains the password to validate</param>
        /// <param name="lowerMinimum">The minimum number of lower-case letters</param>
        /// <param name="upperMinimum">The minimum number of upper-case letters</param>
        /// <param name="numericMinimum">The minimum number of numeric characters</param>
        /// <param name="specialMinimum">The minimum number of special characters</param>
        /// <param name="minimumComplexity">The minimum password complexity</param>
        /// <param name="minimumLength">The minimum overall length</param>
        /// <param name="maximumLength">The maximum overall length</param>
        /// <returns>Returns true if the standards were met</returns>
        public bool MeetsFormatStandards(string password, int lowerMinimum, int upperMinimum, int numericMinimum, int specialMinimum, PasswordComplexityLevel minimumComplexity, int minimumLength, int maximumLength)
        {
            int lowerCount = 0;
            int upperCount = 0;
            int numericCount = 0;
            int specialCount = 0;

            // build a regex pattern
            foreach (char c in password)
            {
                if (this.IsCharOfType(c, PasswordCharType.LowerLetter))
                {
                    lowerCount++;
                }
                else if (this.IsCharOfType(c, PasswordCharType.UpperLetter))
                {
                    upperCount++;
                }
                else if (this.IsCharOfType(c, PasswordCharType.Numeric))
                {
                    numericCount++;
                }
                else if (this.IsCharOfType(c, PasswordCharType.Special))
                {
                    specialCount++;
                }
            }

            return this.MeetsLengthStandards(password, minimumLength, maximumLength) &&
                    lowerCount >= lowerMinimum &&
                    upperCount >= upperMinimum &&
                    numericCount >= numericMinimum &&
                    specialCount >= specialMinimum &&
                    this.MeetsComplexityStandards(password, minimumComplexity);
        }

        /// <summary>
        /// This method is used to calculate the complexity of the password specified
        /// </summary>
        /// <param name="password">Contains the password text to evaluate</param>
        /// <returns>Returns the calculated password complexity level</returns>
        public PasswordComplexityLevel CalculateComplexity(string password)
        {
            PasswordComplexityLevel result = PasswordComplexityLevel.None;

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (password.Length >= (this.Options.MinimumLength > 7 ? this.Options.MinimumLength : 7) && this.SpansEnoughCharacterSets(password, 3) && !this.IsCloseVariationOfAWordInDictionary(password, 0.6))
                {
                    result = PasswordComplexityLevel.High;
                }
                else if (password.Length >= (this.Options.MinimumLength > 7 ? this.Options.MinimumLength : 7) && this.SpansEnoughCharacterSets(password, 2) && !this.FoundInDictionary(password))
                {
                    result = PasswordComplexityLevel.Medium;
                }
                else if (password.Length >= (this.Options.MinimumLength > 6 ? this.Options.MinimumLength : 6))
                {
                    result = PasswordComplexityLevel.Low;
                }
            }

            return result;
        }

        /// <summary>
        /// This method returns a randomly generated password based on specific criteria in settings
        /// </summary>
        /// <returns>Returns the random text</returns>
        public string GeneratePassword()
        {
            return this.GeneratePassword(this.Options.RequiredLowerCaseCount, this.Options.RequiredUpperCaseCount, this.Options.RequiredNumericCount, this.Options.RequiredSpecialCount, this.Options.MinimumComplexityLevel, this.Options.MinimumLength, this.Options.MaximumLength);
        }

        /// <summary>
        /// This method returns a randomly generated password based on specific criteria in settings
        /// </summary>
        /// <param name="lowerCount">The minimum number of lower-case letters</param>
        /// <param name="upperCount">The minimum number of upper-case letters</param>
        /// <param name="numericCount">The minimum number of numeric characters</param>
        /// <param name="specialCount">The minimum number of special characters</param>
        /// <param name="minimumComplexity">The minimum password complexity</param>
        /// <param name="minimumLength">The minimum overall length</param>
        /// <param name="maximumLength">The maximum overall length</param>
        /// <returns>Returns the random text</returns>
        public string GeneratePassword(int lowerCount, int upperCount, int numericCount, int specialCount, PasswordComplexityLevel minimumComplexity, int minimumLength, int maximumLength)
        {
            // contains the number of tries
            int tryCount = 0;

            char[][] charGroups = new char[][]
            {
                PasswordCharactersLowerCase.ToCharArray(),
                PasswordCharactersUpperCase.ToCharArray(),
                PasswordCharactersNumeric.ToCharArray(),
                PasswordCharactersSpecial.ToCharArray()
            };

            // create another string which is a concatenation of all above
            char[] allCharacters = (PasswordCharactersLowerCase + PasswordCharactersUpperCase + PasswordCharactersNumeric + PasswordCharactersSpecial).ToCharArray();
            char[] alphaNumerics = (PasswordCharactersLowerCase + PasswordCharactersUpperCase + PasswordCharactersNumeric).ToCharArray();

            byte[] randomBytes = new byte[4];

            using var rng = RandomNumberGenerator.Create();

            // Generate 4 random bytes.
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            Random random = new Random(seed);

            // Generate four repeating random numbers are positions of lower, upper, numeric and special characters By filling these positions with
            // corresponding characters, we can ensure the password has at least one character of those types
            int lowerTotal = 0;
            int upperTotal = 0;
            int numericTotal = 0;
            int specialTotal = 0;
            int absoluteMinimum = lowerCount + upperCount + numericCount + specialCount;

            // make sure our minimum is not less than absolute
            if (absoluteMinimum > minimumLength)
            {
                minimumLength = absoluteMinimum;
            }

            // max sure our maximum is not less than absolute
            if (absoluteMinimum > maximumLength)
            {
                maximumLength = absoluteMinimum;
            }

            // setup password buffer random length
            bool complete = false;
            char[] password = { };

            while (!complete && tryCount <= 3)
            {
                tryCount++;
                password = new char[random.Next(minimumLength, maximumLength) + 1];

                while (lowerTotal < lowerCount)
                {
                    password[this.AvailablePosition(ref password, random)] = this.RandomCharacter(charGroups[LowerGroupIndex], random);
                    lowerTotal++;
                }

                while (upperTotal < upperCount)
                {
                    password[this.AvailablePosition(ref password, random)] = this.RandomCharacter(charGroups[UpperGroupIndex], random);
                    upperTotal++;
                }

                while (numericTotal < numericCount)
                {
                    password[this.AvailablePosition(ref password, random)] = this.RandomCharacter(charGroups[NumericGroupIndex], random);
                    numericTotal++;
                }

                while (specialTotal < specialCount)
                {
                    password[this.AvailablePosition(ref password, random)] = this.RandomCharacter(charGroups[SpecialGroupIndex], random);
                    specialTotal++;
                }

                while (!complete)
                {
                    password[this.AvailablePosition(ref password, random)] = this.RandomCharacter(alphaNumerics, random);
                    complete = !password.Contains('\0');
                }

                complete = this.MeetsFormatStandards(new string(password), lowerCount, upperCount, numericCount, specialCount, minimumComplexity, minimumLength, maximumLength);
            }

            return new string(password);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to get a random position within the password character array
        /// </summary>
        /// <param name="password">The password array to look within</param>
        /// <param name="random">The random number generator</param>
        /// <returns>Returns a randomly selected available character space within the password array</returns>
        private int AvailablePosition(ref char[] password, Random random)
        {
            // make sure we're not going to infiloop
            int result = password.Contains('\0') ? -1 : 0;

            while (result < 0)
            {
                int index = random.Next(0, password.Length - 1);

                if (password[index] == 0)
                {
                    result = index;
                    break;
                }
                else if (index < password.Length)
                {
                    for (int x = index; x < password.Length; x++)
                    {
                        if (password[x] == 0)
                        {
                            result = x;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method is used to select a random character from the specified source array
        /// </summary>
        /// <param name="source">Contains the source array of characters to select from</param>
        /// <param name="random">Contains the random number generator</param>
        /// <returns>Returns a randomly selected character</returns>
        private char RandomCharacter(char[] source, Random random)
        {
            return source[random.Next(0, source.Length - 1)];
        }

        /// <summary>
        /// This method converts a word into a canonicalized format
        /// </summary>
        /// <param name="word">Contains the word to canonicalize</param>
        /// <param name="type">Contains the canonicalization method type</param>
        /// <returns>Returns the canonicalized version of the word</returns>
        private string CanonicalizeWord(string word, CanonicalizationType type)
        {
            string canonicalizedWord = string.Empty;

            if (!string.IsNullOrWhiteSpace(word))
            {
                char[] canonicalizedArray = word.ToLower().ToCharArray();

                for (int index = 0; index < canonicalizedArray.Length; index++)
                {
                    if (type == CanonicalizationType.Everything || (type == CanonicalizationType.LettersOnly && this.IsCharOfType(canonicalizedArray[index], PasswordCharType.LowerLetter)))
                    {
                        foreach (char charKey in this.similarityMap.Keys)
                        {
                            if (canonicalizedArray[index] == charKey)
                            {
                                canonicalizedArray[index] = this.similarityMap[charKey];
                            }
                        }
                    }
                }

                canonicalizedWord = new string(canonicalizedArray);
            }

            return canonicalizedWord;
        }

        /// <summary>
        /// This method is used to determine if a password was found in the password dictionary verbatim.
        /// </summary>
        /// <param name="password">Contains the password text to evaluate</param>
        /// <returns>Returns true if the password text was found</returns>
        private bool FoundInDictionary(string password)
        {
            string canonicalizedWord = this.CanonicalizeWord(password, CanonicalizationType.LettersOnly);

            return (from p in this.dictionary
                    where p.Value.Contains<string>(canonicalizedWord)
                    select p).Any();
        }

        /// <summary>
        /// This method is used to determine if the password spans enough character set evaluations
        /// </summary>
        /// <param name="password">Contains the password text to evaluate</param>
        /// <param name="minimumCharacterSetsToSpan">Contains the minimum number of character set evaluations to meet</param>
        /// <returns>Returns true if the password text meets the minimum number of sets</returns>
        private bool SpansEnoughCharacterSets(string password, int minimumCharacterSetsToSpan = 0)
        {
            // x = password, y = min char sets to span
            int charSets = 0;

            Dictionary<PasswordCharType, bool> checks = new Dictionary<PasswordCharType, bool>();
            checks.Add(PasswordCharType.UpperLetter, false);
            checks.Add(PasswordCharType.LowerLetter, false);
            checks.Add(PasswordCharType.Numeric, false);
            checks.Add(PasswordCharType.Special, false);

            if (!string.IsNullOrWhiteSpace(password))
            {
                foreach (char c in password)
                {
                    foreach (PasswordCharType type in checks.Keys)
                    {
                        if (!checks[type] && this.IsCharOfType(c, type))
                        {
                            checks[type] = true;
                            break;
                        }
                    }
                }

                charSets = (from cs in checks
                            where cs.Value
                            select cs).Count();
            }

            return charSets >= minimumCharacterSetsToSpan;
        }

        /// <summary>
        /// This method is used to determine if the password is within a certain ratio of like-ness to words in dictionary
        /// </summary>
        /// <param name="password">Contains the password text to evaluate</param>
        /// <param name="minimumVariationRatio">Contains the minimum variation ratio to meet before returning true</param>
        /// <returns>Returns true if the word is close to a dictionary word</returns>
        private bool IsCloseVariationOfAWordInDictionary(string password, double minimumVariationRatio = 0.0)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(password))
            {
                string canonicalizedWord = this.CanonicalizeWord(password, CanonicalizationType.Everything);

                double minimumMeaningfulMatchLength = Math.Floor(minimumVariationRatio * canonicalizedWord.Length);
                for (int subStringLength = canonicalizedWord.Length; subStringLength >= minimumMeaningfulMatchLength; subStringLength--)
                {
                    for (int subStringStart = 0; subStringStart + minimumMeaningfulMatchLength < canonicalizedWord.Length; subStringStart++)
                    {
                        string subWord = canonicalizedWord.Substring(subStringStart, subStringLength - subStringStart - 1);

                        if (this.FoundInDictionary(subWord))
                        {
                            result = true;
                            break;
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// This method is used to determine if a character is of a certain type
        /// </summary>
        /// <param name="testCharacter">Contains the character to test</param>
        /// <param name="type">Contains the password character type</param>
        /// <returns>Returns true if the character meets the type</returns>
        private bool IsCharOfType(char testCharacter, PasswordCharType type)
        {
            bool result = false;

            switch (type)
            {
                case PasswordCharType.UpperLetter:
                    result = char.IsUpper(testCharacter);
                    break;

                case PasswordCharType.LowerLetter:
                    result = char.IsLower(testCharacter);
                    break;

                case PasswordCharType.Numeric:
                    result = char.IsNumber(testCharacter);
                    break;

                case PasswordCharType.Special:
                    result = char.IsPunctuation(testCharacter) || char.IsSeparator(testCharacter) || char.IsSymbol(testCharacter) || char.IsWhiteSpace(testCharacter); // ("!@#$%^&*()_+-='\";:[{]}|.>,</?`~".IndexOf(testCharacter) >= 0);
                    break;
            }

            return result;
        }

        #endregion
    }
}