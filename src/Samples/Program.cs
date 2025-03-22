
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Ubiquity.CommandLineParsing;

namespace SampleArgs
{
    public static class Program
    {
        public static void Main( string[] args )
        {
            Console.WriteLine( "ARGS:" );
            for( int i = 0; i< args.Length; ++i )
            {
                Console.WriteLine( "{0}: [{1}]", i, args[ i ] );
            }

            // Run with arguments like:
            // >SampleArgs.exe positionalarg0 -Option1 "option1 value" -Option2="this is a test" positional2 "positional\foo 3\\"
            try
            {
                var options = Options.ParseFrom( args );
                Console.WriteLine( "Option1: {0}", options.Option1 );
                Console.WriteLine( "Option2: {0}", options.Option2 );
                Console.WriteLine( "PositionalArgs:" );
                foreach( string positional in options.PositionalArgs )
                {
                    Console.WriteLine( positional );
                }
            }
            catch(CommandLineParseException ex)
            {
                Console.Error.WriteLine( ex.Message );
                Options.ShowHelp( );
            }
        }

        [DefaultProperty( "PositionalArgs" )]
        internal class Options
        {
            public List<string> PositionalArgs { get; } = [];

            [CommandLineArg( AllowSpaceDelimitedValue = true )]
            public string Option1 { get; init; } = string.Empty;

            public string Option2 { get; init; } = string.Empty;

            public static Options ParseFrom( string[] args )
            {
                var parser = new Ubiquity.CommandLineParsing.Monad.Parser( );
                return new Options( ).BindArguments( parser.Parse( args ) );
            }

            public static void ShowHelp()
            {
                Console.WriteLine(
                    "Sample Command line paring app\n" +
                    "Usage:\n" +
                    "    SampeArgs (positionalarg | [option])*\n" +
                    "where:\n" +
                    "  positionalarg is a sequence of non-whitespace chars or a quoted string (single or double quotes)\n" +
                    "  option is one of the following options:\n" +
                    "    -Option1 value\n"+
                    "    -Option2=value\n"
                );
            }
        }
    }
}
