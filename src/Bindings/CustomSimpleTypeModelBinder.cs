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
    using System;
    using System.ComponentModel;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using Talegen.Common.Core.Extensions;

    /// <summary>
    /// This class implements a custom binder for simple types that is less aggressive than .NET core binder and mimics original binder where default values are
    /// returned for empty parameters
    /// </summary>
    public class CustomSimpleTypeModelBinder : IModelBinder
    {
        #region Private Fields

        /// <summary>
        /// Contains the fallback type converter
        /// </summary>
        private readonly TypeConverter typeConverter;

        /// <summary>
        /// Contains the logger
        /// </summary>
        private readonly ILogger logger;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSimpleTypeModelBinder" /> class.
        /// </summary>
        /// <param name="type">The type to create binder for.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory" />.</param>
        public CustomSimpleTypeModelBinder(Type type, ILoggerFactory loggerFactory)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this.typeConverter = TypeDescriptor.GetConverter(type);
            this.logger = loggerFactory.CreateLogger<CustomSimpleTypeModelBinder>();
        }

        /// <summary>
        /// This call is made to execute the binding.
        /// </summary>
        /// <param name="bindingContext">Contains the model binding context.</param>
        /// <returns>Returns the parsed long using TryParse instead of the harsher</returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                this.logger.FoundNoValueInRequest(bindingContext);

                // no entry
                this.logger.DoneAttemptingToBindModel(bindingContext);
            }
            else
            {
                this.logger.AttemptingToBindModel(bindingContext);

                // set model state
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                try
                {
                    string value = valueProviderResult.FirstValue;
                    object model;

                    if (bindingContext.ModelType == typeof(string))
                    {
                        // Already have a string. No further conversion required but handle ConvertEmptyStringToNull.
                        if (bindingContext.ModelMetadata.ConvertEmptyStringToNull && string.IsNullOrWhiteSpace(value))
                        {
                            model = null;
                        }
                        else
                        {
                            model = value;
                        }
                    }
                    else if (bindingContext.ModelType == typeof(long))
                    {
                        model = value.ToLong();
                    }
                    else if (bindingContext.ModelType == typeof(bool))
                    {
                        model = value.ToBoolean();
                    }
                    else if (bindingContext.ModelType == typeof(int))
                    {
                        model = value.ToInt();
                    }
                    else if (bindingContext.ModelType == typeof(Guid))
                    {
                        model = value.ToGuid();
                    }
                    else if (bindingContext.ModelType == typeof(decimal))
                    {
                        model = value.ToDecimal();
                    }
                    else if (string.IsNullOrWhiteSpace(value))
                    {
                        // Other than the StringConverter, converters Trim() the value then throw if the result is empty.
                        model = null;
                    }
                    else
                    {
                        model = this.typeConverter.ConvertFrom(
                            context: null,
                            culture: valueProviderResult.Culture,
                            value: value);
                    }

                    this.CheckModel(bindingContext, valueProviderResult, model);

                    this.logger.DoneAttemptingToBindModel(bindingContext);
                }
                catch (Exception exception)
                {
                    var isFormatException = exception is FormatException;
                    if (!isFormatException && exception.InnerException != null)
                    {
                        // TypeConverter throws System.Exception wrapping the FormatException, so we capture the inner exception.
                        exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                    }

                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        exception,
                        bindingContext.ModelMetadata);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is used to check the model and set the binding context result.
        /// </summary>
        /// <param name="bindingContext">Contains the binding context result.</param>
        /// <param name="valueProviderResult">Contains the value provider result.</param>
        /// <param name="model">Contains the converted value model result.</param>
        protected virtual void CheckModel(ModelBindingContext bindingContext, ValueProviderResult valueProviderResult, object model)
        {
            // When converting newModel a null value may indicate a failed conversion for an otherwise required model (can't set a ValueType to null). This
            // detects if a null model value is acceptable given the current bindingContext. If not, an error is logged.
            if (model == null && !bindingContext.ModelMetadata.IsReferenceOrNullableType)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    bindingContext.ModelMetadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(valueProviderResult.ToString()));
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
    }
}