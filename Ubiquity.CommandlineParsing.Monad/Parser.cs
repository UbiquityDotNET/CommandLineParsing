// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;
using Sprache;

namespace Ubiquity.CommandlineParsing.Monad
{
    /// <summary>Implementation of <see cref="CommandlineParsing.ICommandlineParser"/> using the Sprache parser combinator library</summary>
    public class Parser
        : ICommandlineParser
    {
        /// <summary>Parse a command line into base components</summary>
        /// <param name="commandLine">Command line to parse</param>
        /// <returns>Results of the parse</returns>
        public IImmutableList<ICommandlineArgument> Parse( string commandLine )
        {
            return ParseCommandLine( commandLine );
        }

        /// <summary>Static method to parse a command line</summary>
        /// <param name="commandLine">Command line to parser</param>
        /// <returns>Results of the parse</returns>
        public static IImmutableList<ICommandlineArgument> ParseCommandLine( string commandLine )
        {
            try
            {
                return Grammar.CommandLine.Parse( commandLine ).ToImmutableList( );
            }
            catch( ParseException ex )
            {
                throw new CommandlineParseException( ex.Message, ex );
            }
        }
    }
}
