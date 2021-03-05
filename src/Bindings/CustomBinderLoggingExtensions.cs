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
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Contains logging extensions for logging binding event information.
    /// </summary>
    internal static class CustomBinderLoggingExtensions
    {
        #region Private Static Fields

        /// <summary>
        /// Contains the no value in request action.
        /// </summary>
        private static readonly Action<ILogger, string, Type, Exception> FoundNoValueInRequestAction = LoggerMessage.Define<string, Type>(LogLevel.Debug, new EventId(46, "FoundNoValueInRequest"), "Could not find a value in the request with name '{ModelName}' of type '{ModelType}'.");

        /// <summary>
        /// Contains the no value in property action.
        /// </summary>
        private static readonly Action<ILogger, string, Type, string, Type, Exception> FoundNoValueForPropertyInRequestAction = LoggerMessage.Define<string, Type, string, Type>(LogLevel.Debug, new EventId(15, "FoundNoValueForPropertyInRequest"), "Could not find a value in the request with name '{ModelName}' for binding property '{PropertyContainerType}.{ModelFieldName}' of type '{ModelType}'.");

        /// <summary>
        /// Contains the no value in parameter action.
        /// </summary>
        private static readonly Action<ILogger, string, string, Type, Exception> FoundNoValueForParameterInRequestAction = LoggerMessage.Define<string, string, Type>(LogLevel.Debug, new EventId(16, "FoundNoValueForParameterInRequest"), "Could not find a value in the request with name '{ModelName}' for binding parameter '{ModelFieldName}' of type '{ModelType}'.");

        /// <summary>
        /// Contains the done binding attempt action.
        /// </summary>
        private static readonly Action<ILogger, Type, string, Exception> DoneAttemptingToBindModelAction = LoggerMessage.Define<Type, string>(LogLevel.Debug, new EventId(25, "DoneAttemptingToBindModel"), "Done attempting to bind model of type '{ModelType}' using the name '{ModelName}'.");

        /// <summary>
        /// Contains the done binding parameter action.
        /// </summary>
        private static readonly Action<ILogger, string, Type, Exception> DoneAttemptingToBindParameterModelAction = LoggerMessage.Define<string, Type>(LogLevel.Debug, new EventId(45, "DoneAttemptingToBindParameterModel"), "Done attempting to bind parameter '{ParameterName}' of type '{ModelType}'.");

        /// <summary>
        /// Contains the done binding property action.
        /// </summary>
        private static readonly Action<ILogger, Type, string, Type, Exception> DoneAttemptingToBindPropertyModelAction = LoggerMessage.Define<Type, string, Type>(LogLevel.Debug, new EventId(14, "DoneAttemptingToBindPropertyModel"), "Done attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}'.");

        /// <summary>
        /// Contains the attempt to bind model action.
        /// </summary>
        private static readonly Action<ILogger, Type, string, Exception> AttemptingToBindModelAction = LoggerMessage.Define<Type, string>(LogLevel.Debug, new EventId(24, "AttemptingToBindModel"), "Attempting to bind model of type '{ModelType}' using the name '{ModelName}' in request data ...");

        /// <summary>
        /// Contains the attempt to bind property model action.
        /// </summary>
        private static readonly Action<ILogger, Type, string, Type, string, Exception> AttemptingToBindPropertyModelAction = LoggerMessage.Define<Type, string, Type, string>(LogLevel.Debug, new EventId(13, "AttemptingToBindPropertyModel"), "Attempting to bind property '{PropertyContainerType}.{PropertyName}' of type '{ModelType}' using the name '{ModelName}' in request data ...");

        /// <summary>
        /// Contains the attempt to bind parameter model action.
        /// </summary>
        private static readonly Action<ILogger, string, Type, string, Exception> AttemptingToBindParameterModelAction = LoggerMessage.Define<string, Type, string>(LogLevel.Debug, new EventId(44, "AttemptingToBindParameterModel"), "Attempting to bind parameter '{ParameterName}' of type '{ModelType}' using the name '{ModelName}' in request data ...");

        #endregion

        #region Logging Methods

        /// <summary>
        /// This method is called to log no value in model binding request.
        /// </summary>
        /// <param name="logger">Contains the logger.</param>
        /// <param name="bindingContext">Contains the binding context.</param>
        public static void FoundNoValueInRequest(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var modelMetadata = bindingContext.ModelMetadata;

                switch (modelMetadata.MetadataKind)
                {
                    case ModelMetadataKind.Parameter:
                        FoundNoValueForParameterInRequestAction(logger, bindingContext.ModelName, modelMetadata.ParameterName, bindingContext.ModelType, null);
                        break;

                    case ModelMetadataKind.Property:
                        FoundNoValueForPropertyInRequestAction(logger, bindingContext.ModelName, modelMetadata.ContainerType, modelMetadata.PropertyName, bindingContext.ModelType, null);
                        break;

                    case ModelMetadataKind.Type:
                        FoundNoValueInRequestAction(logger, bindingContext.ModelName, bindingContext.ModelType, null);
                        break;
                }
            }
        }

        /// <summary>
        /// This method is called to log attempting binding model request.
        /// </summary>
        /// <param name="logger">Contains the logger.</param>
        /// <param name="bindingContext">Contains the binding context.</param>
        public static void AttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                ModelMetadata modelMetadata = bindingContext.ModelMetadata;

                switch (modelMetadata.MetadataKind)
                {
                    case ModelMetadataKind.Parameter:
                        AttemptingToBindParameterModelAction(logger, modelMetadata.ParameterName, modelMetadata.ModelType, bindingContext.ModelName, null);
                        break;

                    case ModelMetadataKind.Property:
                        AttemptingToBindPropertyModelAction(logger, modelMetadata.ContainerType, modelMetadata.PropertyName, modelMetadata.ModelType, bindingContext.ModelName, null);
                        break;

                    case ModelMetadataKind.Type:
                        AttemptingToBindModelAction(logger, bindingContext.ModelType, bindingContext.ModelName, null);
                        break;
                }
            }
        }

        /// <summary>
        /// This method is called to log done attempting binding model request.
        /// </summary>
        /// <param name="logger">Contains the logger.</param>
        /// <param name="bindingContext">Contains the binding context.</param>
        public static void DoneAttemptingToBindModel(this ILogger logger, ModelBindingContext bindingContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var modelMetadata = bindingContext.ModelMetadata;

                switch (modelMetadata.MetadataKind)
                {
                    case ModelMetadataKind.Parameter:
                        DoneAttemptingToBindParameterModelAction(logger, modelMetadata.ParameterName, modelMetadata.ModelType, null);
                        break;

                    case ModelMetadataKind.Property:
                        DoneAttemptingToBindPropertyModelAction(logger, modelMetadata.ContainerType, modelMetadata.PropertyName, modelMetadata.ModelType, null);
                        break;

                    case ModelMetadataKind.Type:
                        DoneAttemptingToBindModelAction(logger, bindingContext.ModelType, bindingContext.ModelName, null);
                        break;
                }
            }
        }

        #endregion
    }
}