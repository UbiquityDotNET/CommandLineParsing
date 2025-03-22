// -----------------------------------------------------------------------
// <copyright file="IOptionProperty.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Interface for a property that binds a <see cref="CommandLineOption"/> to a property on an object instance.</summary>
    public interface IOptionProperty
    {
        /// <summary>Gets a value indicating whether this property allows a space delimited value.</summary>
        /// <remarks>
        /// The parser, intentionally, doesn't have knowledge of the semantics of an application's
        /// choices for using a space as the delimiter between an option and it's value. This property
        /// informs the binder if it is allowed to consume an immediately following <see cref="CommandLineValue"/>
        /// as the value for this option. The implementation of, IOptionPropertyProvider normally retrieves
        /// this information from the <see cref="CommandLineArgAttribute.AllowSpaceDelimitedValue"/> attribute
        /// attached to the property this IOptionProperty represents though it can use whatever means is
        /// appropriate to the provider.
        /// </remarks>
        bool AllowSpaceDelimitedValue { get; }

        /// <summary>Gets a value indicating whether this property requires a value.</summary>
        /// <remarks>
        /// Most options require a value, though simple booleans don't as the presence or absence of the
        /// option are enough to determine the value.
        /// </remarks>
        bool RequiresValue { get; }

        /// <summary>Gets a value indicating whether this property is a collection that accepts multiple values.</summary>
        bool IsCollection { get; }

        /// <summary>Gets a value indicating whether this property was set.</summary>
        /// <remarks>
        /// If <see cref="IsCollection"/> is <see langword="true"/> then this must always be <see langword="false"/>.
        /// Otherwise, this is <see langword="false"/> until the first call to <see href="xref:Ubiquity.CommandLineParsing.IOptionProperty.BindValue*">BindValue</see>,
        /// after which it remains <see langword="true"/>. This is used to prevent setting the value multiple times.
        /// </remarks>
        bool IsSet { get; }

        /// <summary>Gets the option to bind to this property.</summary>
        CommandLineOption Option { get; }

        /// <summary>Sets the value of the property from <see cref="Option"/>.</summary>
        void BindValue( );

        /// <summary>Sets a value for the property when the option didn't have a value.</summary>
        /// <param name="newValue">New value for the property.</param>
        /// <exception cref="System.InvalidOperationException">
        /// If the <see cref="CommandLineOption.Value"/> is non-null or <see cref="AllowSpaceDelimitedValue"/> is <see langword="false"/>.
        /// </exception>
        void BindValue( string newValue );
    }
}
