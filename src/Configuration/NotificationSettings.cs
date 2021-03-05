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

namespace Talegen.AspNetCore.Web.Configuration
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Contains an enumerated list of SignalR backplane types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BackplaneType
    {
        /// <summary>
        /// No backplane connection shall be attempted.
        /// </summary>
        /// <remarks>Typically used for single instances of the application.</remarks>
        None,

        /// <summary>
        /// A Redis pub/sub server shall be used as backplane for web-farm and Azure scaled application instances.
        /// </summary>
        Redis,

        /// <summary>
        /// An Azure SignalR Service shall be used for SignalR communication.
        /// </summary>
        Azure
    }

    /// <summary>
    /// Contains server settings related to notifications.
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether administrator has disabled SignalR notifications.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the backplane notifications type.
        /// </summary>
        public BackplaneType BackingType { get; set; } = BackplaneType.None;

        /// <summary>
        /// Gets or sets an alternative Redis connection string for alternative backplane server different from the default Redis connection string.
        /// </summary>
        public string AlternativeRedisConnectionConfig { get; set; }

        /// <summary>
        /// Gets or sets the server instance backplane channel prefix. This allows for a channel to be created for a specific application instance
        /// environment/slot with unique name.
        /// </summary>
        /// <remarks>
        /// Messages sent over the channel will continue to have sub-channel traffic based off of the requestor or server domain key for multi-tenant server instances.
        /// </remarks>
        public string BackplaneChannelPrefix { get; set; }
    }
}