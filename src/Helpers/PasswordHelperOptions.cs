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
    /// <summary>
    /// This class defines the password requirement options for the <see cref="PasswordHelper" /> class.
    /// </summary>
    public sealed class PasswordHelperOptions
    {
        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>The minimum length.</value>
        public int MinimumLength { get; set; } = 6;

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaximumLength { get; set; } = 15;

        /// <summary>
        /// Gets or sets the minimum complexity.
        /// </summary>
        /// <value>The minimum complexity.</value>
        public PasswordComplexityLevel MinimumComplexityLevel { get; set; } = PasswordComplexityLevel.Medium;

        /// <summary>
        /// Gets or sets the required lower case count.
        /// </summary>
        /// <value>The required lower case count.</value>
        public int RequiredLowerCaseCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the required upper case count.
        /// </summary>
        /// <value>The required upper case count.</value>
        public int RequiredUpperCaseCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the required numeric count.
        /// </summary>
        /// <value>The required numeric count.</value>
        public int RequiredNumericCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the required special character count.
        /// </summary>
        /// <value>The required special character count.</value>
        public int RequiredSpecialCount { get; set; }
    }
}