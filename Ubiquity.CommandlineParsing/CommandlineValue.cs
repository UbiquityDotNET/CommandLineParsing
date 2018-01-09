// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Command line argument value</summary>
    /// <remarks>
    /// A value is either a positional argument (with outer most quoting removed)
    /// or the value of an option.
    /// </remarks>
    public class CommandlineValue
        : ICommandlineArgument
    {
        /// <summary>Initializes a new instance of the <see cref="CommandlineValue"/> class.</summary>
        /// <param name="text">Full text of the value (without the outermost quotes that may be applied to allow spaces, etc...)</param>
        public CommandlineValue( string text )
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
