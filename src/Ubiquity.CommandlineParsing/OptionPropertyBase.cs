// -----------------------------------------------------------------------
// <copyright file="OptionPropertyBase.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Ubiquity.CommandLineParsing.Properties;

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Abstract base class for implementing <see cref="IOptionProperty"/>.</summary>
    public abstract class OptionPropertyBase
        : IOptionProperty
    {
        /// <inheritdoc/>
        public bool AllowSpaceDelimitedValue { get; }

        /// <inheritdoc/>
        public CommandLineOption Option { get; }

        /// <inheritdoc/>
        public bool IsCollection { get; }

        /// <inheritdoc/>
        public bool RequiresValue { get; }

        /// <inheritdoc/>
        public bool IsSet { get; private set; }

        /// <inheritdoc/>
        public void BindValue( string newValue )
        {
            ArgumentNullException.ThrowIfNull(newValue);

            if( !AllowSpaceDelimitedValue || Option.Value != null )
            {
                throw new InvalidOperationException( );
            }

            CheckAndSetValue( newValue );
        }

        /// <inheritdoc/>
        public void BindValue( )
        {
            CheckAndSetValue( Option.Value?.Text );
        }

        /// <summary>Initializes a new instance of the <see cref="OptionPropertyBase"/> class.</summary>
        /// <param name="option">Option this property is for.</param>
        /// <param name="allowSpaceDelimitedValue">Flag indicating if this property allows a space delimited value.</param>
        /// <param name="isCollection">Flag to indicate if the property is a collection.</param>
        /// <param name="requiresValue">Flag to indicate if the property requires a value.</param>
        protected OptionPropertyBase( CommandLineOption option, bool allowSpaceDelimitedValue, bool isCollection, bool requiresValue )
        {
            Option = option;
            AllowSpaceDelimitedValue = allowSpaceDelimitedValue;
            IsCollection = isCollection;
            RequiresValue = requiresValue;
        }

        /// <summary>Abstract method to actually set the value of the property.</summary>
        /// <param name="value">value to set on the property.</param>
        /// <remarks>
        /// Implementations should handle value conversion to the proper target type
        /// via a Type converter. This includes converting to the element type of
        /// a generic collection.
        /// </remarks>
        protected abstract void InternalSetValue( string? value );

        private void CheckAndSetValue( string? value )
        {
            if( IsSet )
            {
                throw new CommandlineParseException( CultureInfo.CurrentUICulture, Resources.DuplicateOption_0, Option.Text );
            }

            InternalSetValue( value );

            IsSet |= !IsCollection;
        }
    }
}
