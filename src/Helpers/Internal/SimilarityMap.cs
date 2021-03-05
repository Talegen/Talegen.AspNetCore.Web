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

namespace Talegen.AspNetCore.Web.Helpers.Internal
{
    using System.Collections.Generic;

    /// <summary>
    /// This class represents a dictionary of similar characters used for canonicalization of words.
    /// </summary>
    public class SimilarityMap : Dictionary<char, char>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarityMap" /> class.
        /// </summary>
        public SimilarityMap()
        {
            this.Add('3', 'e');
            this.Add('x', 'k');
            this.Add('5', 's');
            this.Add('$', 's');
            this.Add('6', 'g');
            this.Add('7', 't');
            this.Add('8', 'b');
            this.Add('|', 'l');
            this.Add('9', 'g');
            this.Add('+', 't');
            this.Add('@', 'a');
            this.Add('0', 'o');
            this.Add('1', 'l');
            this.Add('2', 'z');
            this.Add('!', 'i');
        }
    }
}