// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Result of a single parsed command line option</summary>
    public class CommandlineOption
        : ICommandlineArgument
    {
        /// <summary>Initializes a new instance of the <see cref="CommandlineOption"/> class.</summary>
        /// <param name="switchLeader">The text of the token that started the option (i.e. "-", "--", "/")</param>
        /// <param name="name">Name of the option</param>
        /// <param name="delimiter">Delimiter between the option and the value (i.e. "=" or ":")</param>
        /// <param name="value">Value for the option</param>
        public CommandlineOption( string switchLeader, string name, string delimiter, CommandlineValue value )
            : this( string.Empty, switchLeader, name, delimiter, value )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineOption"/> class.</summary>
        /// <param name="fullText">Full text of the option from the command line</param>
        /// <param name="switchLeader">The text of the token that started the option (i.e. "-", "--", "/")</param>
        /// <param name="name">Name of the option</param>
        /// <param name="delimiter">Delimiter between the option and the value (i.e. "=" or ":")</param>
        /// <param name="value">Value for the option</param>
        public CommandlineOption( string fullText, string switchLeader, string name, string delimiter, CommandlineValue value )
        {
            if( string.IsNullOrWhiteSpace( fullText ) )
            {
                Text = $"{switchLeader}{name}{delimiter}{value}";
            }
            else
            {
                Text = fullText;
            }

            SwitchLeader = switchLeader;
            Name = name;
            Delimiter = delimiter;
            Value = value;
        }

        /// <inheritdoc/>
        public string Text { get; }

        /// <summary>Gets the switch that introduced this option</summary>
        /// <remarks>
        /// This allows bindings that differentiate on the switch (i.e. - vs. --) to
        /// distinguish the options. (e.g. -a and --all may represent the same option
        /// but would be parsed as distinct options)
        /// </remarks>
        /// <value>Text for the switch leader as a string</value>
        public string SwitchLeader { get; }

        /// <summary>Gets the name of the option this may be an empty string for parsers that allow that</summary>
        /// <value>Name of the  option from the command line</value>
        public string Name { get; }

        /// <summary>Gets the value delimiter, if any</summary>
        /// <value>Delimiter parsed from the command line</value>
        public string Delimiter { get; }

        /// <summary>Gets the value of the option</summary>
        /// <value>Option's value or <see langword="null"/> if no value was provided</value>
        public CommandlineValue Value { get; }

        /// <inheritdoc/>
        public override string ToString( )
        {
            return Text;
        }
    }
}
