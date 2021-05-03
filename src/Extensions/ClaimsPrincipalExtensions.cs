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

namespace Talegen.AspNetCore.Web.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using IdentityModel;
    using Newtonsoft.Json;
    using Talegen.Common.Core.Extensions;
    using Talegen.Common.Models.Contacts;
    using Talegen.Common.Models.Extensions;

    /// <summary>
    /// This class contains extension methods in support of working with claim to model conversions.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Contains the mobile claim type.
        /// </summary>
        public const string TalegenMobileClaimType = "mobile";

        /// <summary>
        /// Contains the backchannel claim type.
        /// </summary>
        public const string TalegenClientBackchannelClaimType = "client_backchannel";

        /// <summary>
        /// This extension method gets the principal's user name.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the user name claim value if found.</returns>
        public static string UserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.PreferredUserName) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.Upn) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's subject identity value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the subject identity claim value if found.</returns>
        public static string SubjectId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.Subject) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.Sid) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's email claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the email claim value if found.</returns>
        public static string Email(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.Email) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's picture claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the picture claim value if found.</returns>
        public static string Picture(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(JwtClaimTypes.Picture) ?? string.Empty;
        }

        /// <summary>
        /// This extension method gets the principal's phone number claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the phone number claim value if found.</returns>
        public static string HomePhoneNumber(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.PhoneNumber) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.HomePhone) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's mobile phone number claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="useMobileClaimType">Contains a value indicating whether the "mobile" claim type shall be used.</param>
        /// <returns>Returns the phone number claim value if found.</returns>
        public static string MobilePhoneNumber(this ClaimsPrincipal principal, bool useMobileClaimType = true)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(useMobileClaimType ? TalegenMobileClaimType : JwtClaimTypes.PhoneNumber) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.MobilePhone) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's given (first) name claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the given name claim value if found.</returns>
        public static string GivenName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.GivenName) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's family (last) name claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the family name claim value if found.</returns>
        public static string FamilyName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.FamilyName) ?? string.Empty;
            return string.IsNullOrEmpty(result) ? (principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty) : result;
        }

        /// <summary>
        /// This extension method gets the principal's time zone claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the time zone claim value if found.</returns>
        public static string TimeZone(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(JwtClaimTypes.ZoneInfo) ?? string.Empty;
        }

        /// <summary>
        /// This extension method gets the principal's locale claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the locale claim value if found.</returns>
        public static string Locale(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.Locale) ?? string.Empty;
            return string.IsNullOrWhiteSpace(result) ? principal.FindFirstValue(ClaimTypes.Locality) ?? string.Empty : result;
        }

        /// <summary>
        /// This extension method gets the principal's locale claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the locale claim value if found.</returns>
        public static string SessionId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(JwtClaimTypes.SessionId) ?? string.Empty;
        }

        /// <summary>
        /// This extension method gets the principal's street address claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">principal</exception>
        public static Address Address(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            Address address = null;
            string result = principal.FindFirstValue(JwtClaimTypes.Address) ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(result))
            {
                address = principal.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Address)?.ToAddress();
            }

            return address;
        }

        /// <summary>
        /// This extension method gets the principal's gender claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the gender claim value if found.</returns>
        public static string Gender(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.Gender) ?? string.Empty;
            return string.IsNullOrWhiteSpace(result) ? principal.FindFirstValue(ClaimTypes.Gender) ?? string.Empty : result;
        }

        /// <summary>
        /// This extension method gets the principal's birthday claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the birthday claim value if found.</returns>
        public static string Birthday(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.BirthDate) ?? string.Empty;
            return string.IsNullOrWhiteSpace(result) ? principal.FindFirstValue(ClaimTypes.DateOfBirth) ?? string.Empty : result;
        }

        /// <summary>
        /// This extension method gets the principal's token expiration claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the token expiration claim value if found.</returns>
        public static string Expiration(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.FindFirstValue(JwtClaimTypes.Expiration) ?? string.Empty;
            return string.IsNullOrWhiteSpace(result) ? principal.FindFirstValue(ClaimTypes.Expiration) ?? string.Empty : result;
        }

        /// <summary>
        /// This extension method gets the principal's token valid not before claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the token valid not before claim value if found.</returns>
        public static string NotBefore(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(JwtClaimTypes.NotBefore) ?? string.Empty;
        }

        /// <summary>
        /// This extension method gets a value indicating whether this instance is backchannel.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns><c>true</c> if the specified principal is backchannel; otherwise, <c>false</c>.</returns>
        public static bool IsBackchannel(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(TalegenClientBackchannelClaimType).ToBoolean();
        }

        /// <summary>
        /// This method the last name of the current user.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the user name.</returns>
        public static string FullName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string result = principal.GivenName();
            string lastName = principal.FamilyName();

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                result += " " + lastName;
            }

            return result;
        }
    }
}