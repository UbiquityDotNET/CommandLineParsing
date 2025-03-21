// -----------------------------------------------------------------------
// <copyright file="CommandLineValue.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Command line argument value.</summary>
    /// <remarks>
    /// A value is either a positional argument (with outer most quoting removed)
    /// or the value of an option.
    /// </remarks>
    public class CommandLineValue
        : ICommandLineArgument
    {
        /// <summary>Initializes a new instance of the <see cref="CommandLineValue"/> class.</summary>
        /// <param name="text">Full text of the value (without the outermost quotes that may be applied to allow spaces, etc...)</param>
        public CommandLineValue( string text )
        {
            Text = text;
        }

        /// <inheritdoc/>
        public string Text { get; }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return Text;
        }
    }
}
