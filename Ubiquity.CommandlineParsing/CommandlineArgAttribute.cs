// -----------------------------------------------------------------------
// <copyright file="CommandlineArgAttribute.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Allows accessing a property via the command line parser with a name that is different from the property name.</summary>
    [AttributeUsage( AttributeTargets.Property, Inherited = false, AllowMultiple = false )]
    public sealed class CommandlineArgAttribute
        : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="CommandlineArgAttribute"/> class.</summary>
        /// <remarks>
        /// Marks the attached property for command line argument binding. The name of the
        /// property is the option for the command line. If the LongName property is set
        /// it is used as the option name.
        /// </remarks>
        public CommandlineArgAttribute( )
            : this( string.Empty )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineArgAttribute"/> class.</summary>
        /// <param name="shortName">Short name for the argument.</param>
        public CommandlineArgAttribute( string shortName )
        {
            ShortName = shortName;
        }

        /// <summary>Gets short form name for the command line argument.</summary>
        public string ShortName { get; }

        /// <summary>Gets or sets long form name for the command line argument.</summary>
        public string LongName { get; set; }

        /// <summary>Gets or sets a value indicating whether the option allows a space delimited value.</summary>
        /// <remarks>
        /// Normally an option followed by a space is treated as a boolean flag and a distinct positional argument.
        /// Setting this to true will allow treating the positional argument as the value for the item.
        /// <note type="note">Setting this makes the value required such that an exception is generated if the
        /// option is followed by another option or the end of the command line.</note>
        /// </remarks>
        public bool AllowSpaceDelimitedValue { get; set; }
    }
}
