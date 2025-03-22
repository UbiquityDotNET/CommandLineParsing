// -----------------------------------------------------------------------
// <copyright file="Parser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Sprache;

namespace Ubiquity.CommandLineParsing.Monad
{
    /// <summary>Implementation of <see cref="CommandLineParsing.ICommandLineParser"/> using the Sprache parser combinator library.</summary>
    public class Parser
        : ICommandLineParser
    {
        /// <inheritdoc/>
        public IImmutableList<ICommandLineArgument> Parse( IEnumerable<string> args )
        {
            return ( from arg in args
                     select Grammar.Option.Or( Grammar.PositionalArg ).Parse( arg )
                   ).ToImmutableList( );
        }

        /// <summary>Static method to parse a command line.</summary>
        /// <param name="commandLine">Command line to parser.</param>
        /// <returns>Results of the parse.</returns>
        public static IImmutableList<ICommandLineArgument> ParseCommandLine( string commandLine )
        {
            try
            {
                return Grammar.CommandLine.Parse( commandLine ).ToImmutableList( );
            }
            catch( ParseException ex )
            {
                throw new CommandLineParseException( ex.Message, ex );
            }
        }
    }
}
