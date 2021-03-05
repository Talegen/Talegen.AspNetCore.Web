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

namespace Talegen.AspNetCore.Web.Bindings
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// This class contains the binding extension to add our binding overrides during startup.
    /// </summary>
    /// <remarks>
    /// The single extension method, AddBindingOverrides, is called when initializing a new Web Application within the
    /// <code>
    ///services.AddControllers(options =&gt;
    ///{
    ///options.AddBindingOverrides();
    ///})
    /// </code>
    /// </remarks>
    public static class BindingExtensions
    {
        /// <summary>
        /// This method is used to add the custom model binding handlers to override the built-in inflexible binders.
        /// </summary>
        /// <param name="options">Contains the MVC options to update.</param>
        public static void AddBindingOverrides(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new CustomSimpleTypeModelBinderProvider());
        }
    }
}