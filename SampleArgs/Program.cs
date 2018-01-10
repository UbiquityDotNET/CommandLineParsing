// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Ubiquity.CommandlineParsing;

namespace SampleArgs
{
    public static class Program
    {
        public static void Main( )
        {
            // Run with arguments like:
            // >SampleArgs.exe positionalarg0 -Option1 "positional arg 1" -Option2="this is a test" positional2 "positional\foo 3\\"
            try
            {
                var options = Options.ParseFrom( EnvironmentEx.CommandLine );
            }
            catch(CommandlineParseException ex)
            {
                Console.Error.WriteLine( ex.Message );
                Options.ShowHelp( );
            }
        }

        [DefaultProperty( "PositionalArgs" )]
        internal class Options
        {
            public List<string> PositionalArgs { get; } = new List<string>( );

            [CommandlineArg( AllowSpaceDelimitedValue = true )]
            public string Option1 { get; set; }

            public string Option2 { get; set; }

            public static Options ParseFrom( string commandLine )
            {
                return CommandlineBinder.ParseAndBind<Options>( new Ubiquity.CommandlineParsing.Monad.Parser( ), commandLine );
            }

            public static void ShowHelp()
            {
                Console.WriteLine(
                    "Sample CommandlineParing app\n" +
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
