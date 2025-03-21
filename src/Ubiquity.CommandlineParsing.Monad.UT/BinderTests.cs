// -----------------------------------------------------------------------
// <copyright file="BinderTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.CommandLineParsing.Monad.UT
{
    [TestClass]
    public class BinderTests
    {
        [TestMethod]
        public void CommandLineBinderTest()
        {
            var parser = new Parser( );
            TestOptions options = new TestOptions( ).BindArguments( parser.Parse( FullCommandLine ) );
            Assert.IsNotNull( options );
            Assert.AreEqual( 4, options.PositionalArgs.Count );
            Assert.AreEqual( 3, options.MultiOption.Count );
            Assert.AreEqual( "space delimited value1", options.Option1 );
            Assert.AreEqual( "this is a test", options.Option2 );
            Assert.AreEqual( TestOptions.Option3Values.Baz, options.Option3 );
            Assert.IsTrue( options.Option4 );
        }

        // This includes all forms of option switches quoting and the special problematic case of a trailing \ in a quoted string
        // The trailing \ in a quoted string is a notorious hidden gotcha for .NET apps as the default .NET arg parsing generates
        // a quote character as it implements character escaping, unlike any other runtime.
        private readonly string[] FullCommandLine =
            [
                @"positionalarg0",
                @"-m:""Multi 1""",
                @"--o1",
                @"""space delimited value1""",
                @"-MultiOption=""Multi 2""",
                @"positional1",
                @"-Option2=""this is a test""",
                @"/option3:baz",
                @"-m:multi3",
                @"-o4",
                @"positional2",
                @"""positional 3\""",
            ];
    }
}
