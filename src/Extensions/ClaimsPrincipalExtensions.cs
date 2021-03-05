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
    using System.Security.Claims;
    using IdentityModel;

    /// <summary>
    /// This class contains extension methods in support of working with claim to model conversions.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the email claim value if found.</returns>
        public static string GetUserName(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.PreferredUserName) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.Upn) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the email claim value if found.</returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.Subject) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.Sid) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Returns the email claim value if found.</returns>
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.Email) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.Email) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the phone number.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public static string GetPhoneNumber(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.PhoneNumber) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.MobilePhone) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the given name claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public static string GetGivenName(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.GivenName) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the family name claim value.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public static string GetFamilyName(this ClaimsPrincipal principal)
        {
            string result = principal?.FindFirstValue(JwtClaimTypes.FamilyName) ?? string.Empty;

            return string.IsNullOrEmpty(result) ? (principal?.FindFirstValue(ClaimTypes.Surname) ?? string.Empty) : string.Empty;
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public static string GetTimeZone(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(JwtClaimTypes.ZoneInfo) ?? string.Empty;
        }

        /// <summary>
        /// Gets the locale.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public static string GetLocale(this ClaimsPrincipal principal)
        {
            string result = principal.FindFirstValue(JwtClaimTypes.Locale) ?? string.Empty;
            return string.IsNullOrWhiteSpace(result) ? principal?.FindFirstValue(ClaimTypes.Locality) ?? string.Empty : result;
        }
    }
}