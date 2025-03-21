// -----------------------------------------------------------------------
// <copyright file="Grammar.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Ubiquity.CommandLineParsing.Monad
{
    // NOTE: Order of layout matters here as the static initializers need to occur
    // in order of dependencies, otherwise additional "ref" monads are needed, which
    // add overhead and needless complexity.

    /// <summary>Static class to provide parser monads for parsing a command line.</summary>
    /// <remarks>
    /// The fields of this class are the lexer rules, while the Properties are the
    /// parser rules. The methods are utility combinators used to build the other
    /// rules.
    /// </remarks>
    public static class Grammar
    {
        /// <summary>Parser monad for a single quote character.</summary>
        public static readonly Parser<char> SingleQuote = Parse.Char( '\'' );

        /// <summary>Parser monad for a double quote character.</summary>
        public static readonly Parser<char> DoubleQuote = Parse.Char( '"' );

        /// <summary>Parser monad for the '=' symbol.</summary>
        public static readonly Parser<char> Equal = Parse.Char( '=' ).Token();

        /// <summary>Parser monad for the ':' symbol.</summary>
        public static readonly Parser<char> Colon = Parse.Char( ':' ).Token();

        /// <summary>Parser monad for the '-' symbol.</summary>
        public static readonly Parser<char> Dash = Parse.Char( '-' ).Token();

        /// <summary>Parser monad for the '/' symbol.</summary>
        public static readonly Parser<char> Slash = Parse.Char( '/' ).Token();

        /// <summary>Parser monad for either the '=' or ':' symbols.</summary>
        /// <remarks>This is commonly used to match the value separator for a command line option switch.</remarks>
        public static readonly Parser<char> EqualOrColon = Equal.XOr( Colon ).Token();

        /// <summary>Parser monad for a double dash sequence (e.g. '--').</summary>
        public static readonly Parser<IEnumerable<char>> DoubleDash = Parse.String( "--" ).Token();

        /// <summary>Parser monad for common command line option start (e.g. '/' | '-' | '--' ).</summary>
        public static readonly Parser<IEnumerable<char>> CommonOptionStart = DoubleDash.Or( Slash.Once() )
                                                                              .Or( Dash.Once() )
                                                                              .Token( );

        /// <summary>Parser for a single character as part of an identifier.</summary>
        public static readonly Parser<char> IdentifierChar = Parse.Chars( '_','-' ).XOr( Parse.LetterOrDigit );

        /// <summary>Parser monad for a common identifier (e.g. IdentifierChar*).</summary>
        public static readonly Parser<IEnumerable<char>> Identifier
            = Parse.Identifier( IdentifierChar.Except( CommonOptionStart ), IdentifierChar ).Token( );

        /// <summary>Block quoted string, with custom open and closing delimiter parsers.</summary>
        /// <param name="openDelimiter">Parser for the opening delimiter of the string.</param>
        /// <param name="closeDelimiter">Parser for the closing delimiter of the string.</param>
        /// <returns>Parser for a block string.</returns>
        public static Parser<IEnumerable<char>> BlockString( Parser<IEnumerable<char>> openDelimiter
                                                           , Parser<IEnumerable<char>> closeDelimiter
                                                           )
        {
            return ( from start in openDelimiter
                     from content in Parse.AnyChar.Except( closeDelimiter ).Many()
                     from end in closeDelimiter
                     select content
                   ).Token( );
        }

        /// <summary>Parser combinator for a delimited string.</summary>
        /// <param name="delimiter">Delimiter parser for the start and end of the string.</param>
        /// <returns>Parser monad.</returns>
        public static Parser<IEnumerable<char>> BlockString( Parser<IEnumerable<char>> delimiter)
            => BlockString( delimiter, delimiter );

        /// <summary>Parser monad to parse non-whitespace characters.</summary>
        public static readonly Parser<char> NonWhiteSpaceChar
            = Parse.AnyChar.Except( Parse.WhiteSpace );

        /// <summary> Parser monad for a Quoted string (either single or double quotes).</summary>
        public static readonly Parser<IEnumerable<char>> QuotedString
            = BlockString( DoubleQuote.Once() );

        /// <summary>Parser monad for an unquoted positional value or the value of an option.</summary>
        public static readonly Parser<IEnumerable<char>> UnquotedValue
            = ( from firstChar in Parse.CharExcept( "-\"" )
                from rest in Parse.CharExcept( "\"" ).Many()
                select rest.Prepend( firstChar )
              ).Token();

        /// <summary>Gets a Parser monad for a value (either a positional argument or the value of a switch/option).</summary>
        public static Parser<IEnumerable<char>> ValueContent => QuotedString.XOr( UnquotedValue );

        /// <summary>Parser combinator for a command line switch/option.</summary>
        /// <param name="leadingParser">Parser for the switch leading (typically parses '-','--' or '/'. <see cref="CommonOptionStart"/>).</param>
        /// <param name="valueSep">Parser to match the value separator(s) for the parser (typically parser '=' or ':'. <see cref="EqualOrColon"/>).</param>
        /// <returns>Parser for the option as defined by the parameters.</returns>
        public static Parser<CommandLineOption> OptionParser( Parser<IEnumerable<char>> leadingParser, Parser<IEnumerable<char>> valueSep )
        {
            return from leading in leadingParser.Text( )
                   from optionalName in Identifier.Text( ).Optional( )
                   from optionalValue in SepAndValue( valueSep ).Optional( )
                   let name = optionalName.GetOrElse( string.Empty )
                   let option = optionalValue.GetOrElse<(string sep, CommandLineValue value)>((string.Empty, new(string.Empty)))
                   let sep = option.sep
                   let optionValue = option.value
                   select new CommandLineOption( leading, name, sep, optionValue );
        }

        /// <summary>Parser monad to parse a value separator and value.</summary>
        /// <param name="valueSep">Parser that will parse the allowed separator(s).</param>
        /// <returns>Parser to parse the separator and value.</returns>
        public static Parser<(string sep, CommandLineValue value)> SepAndValue( Parser<IEnumerable<char>> valueSep )
        {
            return from sep in valueSep.Text( )
                   from value in ValueContent.Text( )
                   select (sep, value: new CommandLineValue( value ));
        }

        /// <summary>Parser combinator for a command line switch/option.</summary>
        /// <param name="valueSep">Parser to match the value separator(s) for the parser (typically parser '=' or ':'. <see cref="EqualOrColon"/>).</param>
        /// <returns>Parser for the option as defined by the parameters.</returns>
        public static Parser<CommandLineOption> OptionParser( Parser<IEnumerable<char>> valueSep )
            => OptionParser( CommonOptionStart, valueSep );

        /// <summary>Parser monad for a command line option that accepts values.</summary>
        /// <remarks>
        /// This is a convenience wrapper for <see cref="OptionParser(Parser{IEnumerable{char}}, Parser{IEnumerable{char}})"/>
        /// using <see cref="CommonOptionStart"/> and <see cref="EqualOrColon"/>.
        /// </remarks>
        public static readonly Parser<CommandLineOption> Option = OptionParser( CommonOptionStart, EqualOrColon.Once() ).Named("Option Switch");

        /// <summary>Gets a Parser monad for a position argument that matches the <see cref="ValueContent"/> parser.</summary>
        public static Parser<ICommandLineArgument> PositionalArg =>
            from value in ValueContent.Named( "Positional Argument" ).Text( )
            select new CommandLineValue( value );

        /// <summary>Gets a parser for a single command line argument.</summary>
        public static Parser<ICommandLineArgument> CommandlineArg
            => Option.Or( PositionalArg );

        /// <summary>Gets the Top level parser to parse a command line into a sequence of <see cref="ICommandLineArgument"/>.</summary>
        public static Parser<IEnumerable<ICommandLineArgument>> CommandLine
            => ( CommandlineArg ).DelimitedBy( Parse.WhiteSpace.Many() ).End();

        /// <summary>Parser combinator to provide semantic action support for matched parsers.</summary>
        /// <typeparam name="T">Type of elements the parser produces.</typeparam>
        /// <param name="parser">parser to process the input sequence and, if matched, run <paramref name="action"/>.</param>
        /// <param name="action">action to run if the parser matches the input.</param>
        /// <returns>
        /// New parser that will pass the input through <paramref name="parser"/> and run <paramref name="action"/>
        /// on a successful match.
        /// </returns>
        public static Parser<T> OnMatch<T>( this Parser<T> parser, Action<IResult<T>> action )
        {
            ArgumentNullException.ThrowIfNull( parser );
            ArgumentNullException.ThrowIfNull( action );

            return i =>
            {
                var result = parser( i );
                if( result.WasSuccessful )
                {
                    action( result );
                }

                return result;
            };
        }
    }
}
