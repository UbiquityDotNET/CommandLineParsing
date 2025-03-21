// -----------------------------------------------------------------------
// <copyright file="ParserTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

using static Ubiquity.CommandlineParsing.Monad.Grammar;

namespace Ubiquity.CommandlineParsing.Monad.UT
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void OptionTests( )
        {
            CommandlineOption option = Option.Parse( "-f0" );
            Assert.IsNotNull( option );
            Assert.AreEqual( "-f0", option.Text );
            Assert.AreEqual( "-", option.SwitchLeader );
            Assert.AreEqual( "f0", option.Name );
            Assert.IsNull( option.Delimiter );
            Assert.IsNull( option.Value );
        }

        [TestMethod]
        public void OptionWithColonValueTests( )
        {
            CommandlineOption option = Option.Parse( "-f:0" );
            Assert.IsNotNull( option );
            Assert.AreEqual( "-f:0", option.Text );
            Assert.AreEqual( "-", option.SwitchLeader );
            Assert.AreEqual( "f", option.Name );
            Assert.AreEqual( ":", option.Delimiter );
            Assert.IsNotNull( option.Value );
            Assert.AreEqual( "0", option.Value.Text );
        }

        [TestMethod]
        public void OptionWithEqualValueTests( )
        {
            CommandlineOption option = Option.Parse( "-f=0" );
            Assert.IsNotNull( option );
            Assert.AreEqual( "-f=0", option.Text );
            Assert.AreEqual( "-", option.SwitchLeader );
            Assert.AreEqual( "f", option.Name );
            Assert.AreEqual( "=", option.Delimiter );
            Assert.IsNotNull( option.Value );
            Assert.AreEqual( "0", option.Value.Text );
        }

        [TestMethod]
        public void CommandLineParserTest( )
        {
            string[ ] unparsedArgs =
            {
                @"positionalarg0",
                @"--Option1",
                @"positional arg 1",
                @"-Option2=this is a test",
                @"/option3:foo",
                @"positional2",
                @"positional 3\",
            };
            var args = unparsedArgs.Select( CommandlineArg.Parse ).ToList( );

            Assert.IsInstanceOfType( args[ 0 ], typeof( CommandlineValue ) );
            var arg0 = ( CommandlineValue )args[ 0 ];
            Assert.AreEqual( "positionalarg0", arg0.Text );

            Assert.IsInstanceOfType( args[ 1 ], typeof( CommandlineOption ) );
            var arg1 = ( CommandlineOption )args[ 1 ];
            Assert.AreEqual( "--", arg1.SwitchLeader );
            Assert.AreEqual( "Option1", arg1.Name );
            Assert.IsNull( arg1.Delimiter );
            Assert.IsNull( arg1.Value );

            Assert.IsInstanceOfType( args[ 2 ], typeof( CommandlineValue ) );
            var arg2 = ( CommandlineValue )args[ 2 ];
            Assert.AreEqual( "positional arg 1", arg2.Text );

            Assert.IsInstanceOfType( args[ 3 ], typeof( CommandlineOption ) );
            var arg3 = ( CommandlineOption )args[ 3 ];
            Assert.AreEqual( "-", arg3.SwitchLeader );
            Assert.AreEqual( "Option2", arg3.Name );
            Assert.AreEqual( "=", arg3.Delimiter );
            Assert.AreEqual( "this is a test", arg3.Value.Text );

            Assert.IsInstanceOfType( args[ 4 ], typeof( CommandlineOption ) );
            var arg4 = ( CommandlineOption )args[ 4 ];
            Assert.AreEqual( "/", arg4.SwitchLeader );
            Assert.AreEqual( "option3", arg4.Name );
            Assert.AreEqual( ":", arg4.Delimiter );
            Assert.AreEqual( "foo", arg4.Value.Text );

            Assert.IsInstanceOfType( args[ 5 ], typeof( CommandlineValue ) );
            var arg5 = ( CommandlineValue )args[ 5 ];
            Assert.AreEqual( "positional2", arg5.Text );

            Assert.IsInstanceOfType( args[ 6 ], typeof( CommandlineValue ) );
            var arg6 = ( CommandlineValue )args[ 6 ];
            Assert.AreEqual( @"positional 3\", arg6.Text );
        }
    }
}
