// -----------------------------------------------------------------------
// <copyright file="GrammarLexemeTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

using static Ubiquity.CommandLineParsing.Monad.Grammar;

namespace Ubiquity.CommandLineParsing.Monad.UT
{
    [TestClass]
    public class GrammarLexemeTests
    {
        [TestMethod]
        public void SingleQuoteTest( )
        {
            char result = SingleQuote.Parse( "'" );
            Assert.AreEqual( '\'', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void SingleQuoteExceptionTest( )
        {
            _ = SingleQuote.Parse( "\"" );
        }

        [TestMethod]
        public void DoubleQuoteTest( )
        {
            char result = DoubleQuote.Parse( "\"" );
            Assert.AreEqual( '\"', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void DoubleQuoteExceptionTest( )
        {
            _ = DoubleQuote.Parse( "'" );
        }

        [TestMethod]
        public void EqualTest( )
        {
            char result = Equal.Parse( "=" );
            Assert.AreEqual( '=', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void EqualExceptionTest( )
        {
            _ = Equal.Parse( "+" );
        }

        [TestMethod]
        public void ColonTest( )
        {
            char result = Colon.Parse( ":" );
            Assert.AreEqual( ':', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void ColonExceptionTest( )
        {
            _ = Colon.Parse( "+" );
        }

        [TestMethod]
        public void DashTest( )
        {
            char result = Dash.Parse( "-" );
            Assert.AreEqual( '-', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void DashExceptionTest( )
        {
            _ = Dash.Parse( "+" );
        }

        [TestMethod]
        public void SlashTest( )
        {
            char result = Slash.Parse( "/" );
            Assert.AreEqual( '/', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void SlashExceptionTest( )
        {
            _ = Slash.Parse( "+" );
        }

        [TestMethod]
        public void EqualOrColonTest( )
        {
            char result = EqualOrColon.Parse( ":" );
            Assert.AreEqual( ':', result );
            result = EqualOrColon.Parse( "=" );
            Assert.AreEqual( '=', result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void EqualOrColonExceptionTest( )
        {
            _ = EqualOrColon.Parse( "a" );
        }

        [TestMethod]
        public void DoubleDashTest( )
        {
            string result = DoubleDash.Text( ).Parse( "--" );
            Assert.AreEqual( "--", result );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void DoubleDashExceptionTest( )
        {
            _ = DoubleDash.Parse( "++" );
        }

        [TestMethod]
        public void CommonOptionStartTest( )
        {
            Assert.AreEqual( "--", CommonOptionStart.Text( ).Parse( "--" ) );
            Assert.AreEqual( "-", CommonOptionStart.Text( ).Parse( "-" ) );
            Assert.AreEqual( "/", CommonOptionStart.Text( ).Parse( "/" ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void CommonOptionStartExceptionTest( )
        {
            _ = DoubleDash.Parse( "++" );
        }

        [TestMethod]
        public void IdentifierCharTests( )
        {
            Assert.AreEqual( '_', IdentifierChar.Parse( "_" ) );
            Assert.AreEqual( 'a', IdentifierChar.Parse( "a" ) );
            Assert.AreEqual( 'A', IdentifierChar.Parse( "A" ) );
            Assert.AreEqual( '0', IdentifierChar.Parse( "0" ) );
            Assert.AreEqual( '9', IdentifierChar.Parse( "9" ) );

            // Greek alphabet Unicode delta is considered a letter
            Assert.AreEqual( '\u0394', IdentifierChar.Parse( "\u0394" ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void IdentifierCharExceptionTest( )
        {
            _ = IdentifierChar.Parse( ":" );
        }

        [TestMethod]
        public void IdentifierTests( )
        {
            Assert.AreEqual( "_asd", Identifier.Text( ).Parse( " _asd" ) );
            Assert.AreEqual( "asd_", Identifier.Text( ).Parse( "asd_ " ) );
            Assert.AreEqual( "A9_1", Identifier.Text( ).Parse( " A9_1 " ) );
            Assert.AreEqual( "a123", Identifier.Text( ).Parse( "a123 -asd" ) );

            // Greek alphabet Unicode delta is considered a letter
            Assert.AreEqual( "\u0394Test", Identifier.Text( ).Parse( "\u0394Test" ) );
        }

        [TestMethod]
        public void IdentifierExceptionTest( )
        {
            var result = Identifier( new Input( "!abcd" ) );
            Assert.IsFalse( result.WasSuccessful );
        }

        [TestMethod]
        public void NonWhitespaceCharTests( )
        {
            Assert.AreEqual( '_', NonWhiteSpaceChar.Parse( "_ " ) );
            Assert.AreEqual( 'a', NonWhiteSpaceChar.Parse( "a" ) );
            Assert.AreEqual( 'A', NonWhiteSpaceChar.Parse( "ABC" ) );
            Assert.AreEqual( '0', NonWhiteSpaceChar.Parse( "0-1" ) );
            Assert.AreEqual( '9', NonWhiteSpaceChar.Parse( "9" ) );

            // Greek alphabet Unicode delta is considered a letter
            Assert.AreEqual( '\u0394', NonWhiteSpaceChar.Parse( "\u0394" ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void NonWhitespaceExceptionTest( )
        {
            _ = NonWhiteSpaceChar.Parse( "\t" );
        }

        [TestMethod]
        public void DoubleQuotedStringTest( )
        {
            const string doubleQuotedString = "\"asdf ghijk \u0394\"";
            Assert.AreEqual( doubleQuotedString.Trim( ).Trim( '"' ), QuotedString.Text( ).Parse( doubleQuotedString ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void DoubleQuotedStringExceptionTest( )
        {
            const string doubleQuotedString = "\"asdf ghijk \u0394";
            Assert.AreEqual( doubleQuotedString, QuotedString.Parse( doubleQuotedString ) );
        }
    }
}
