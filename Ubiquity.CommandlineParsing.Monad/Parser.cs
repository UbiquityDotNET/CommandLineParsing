// -----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sprache;

namespace Ubiquity.CommandlineParsing.Monad
{
    /// <summary>Implementation of <see cref="CommandlineParsing.ICommandlineParser"/> using the Sprache parser combinator library.</summary>
    public class Parser
        : ICommandlineParser
    {
        /// <inheritdoc/>
        public IImmutableList<ICommandlineArgument> Parse( IEnumerable<string> args )
        {
            return ( from arg in args
                     select Grammar.Option.Or( Grammar.PositionalArg ).Parse( arg )
                   ).ToImmutableList( );
        }

        /// <summary>Static method to parse a command line.</summary>
        /// <param name="commandLine">Command line to parser.</param>
        /// <returns>Results of the parse.</returns>
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
