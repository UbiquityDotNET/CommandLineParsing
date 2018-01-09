// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sprache;

using static Ubiquity.CommandlineParsing.Monad.Grammar;

namespace Ubiquity.CommandlineParsing.Monad.UT
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
            char result = SingleQuote.Parse( "\"" );
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
            char result = DoubleQuote.Parse( "'" );
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
            char result = Equal.Parse( "+" );
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
            char result = Colon.Parse( "+" );
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
            char result = Dash.Parse( "+" );
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
            char result = Slash.Parse( "+" );
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
            char result = EqualOrColon.Parse( "a" );
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
            var result = DoubleDash.Parse( "++" );
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
            var result = DoubleDash.Parse( "++" );
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
            char result = IdentifierChar.Parse( ":" );
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
            char result = NonWhiteSpaceChar.Parse( "\t" );
        }

        [TestMethod]
        public void SingleQuotedStringTest( )
        {
            const string singleQuotedString = " 'asdf \"ghijk\" \u0394' ";
            Assert.AreEqual( singleQuotedString.Trim( ), QuotedString.Text( ).Parse( singleQuotedString ) );
        }

        [TestMethod]
        [ExpectedException( typeof( ParseException ) )]
        public void SingleQuotedStringExceptionTest( )
        {
            const string singleQuotedString = "'asdf ghijk \u0394";
            Assert.AreEqual( singleQuotedString, QuotedString.Parse( singleQuotedString ) );
        }

        [TestMethod]
        public void DoubleQuotedStringTest( )
        {
            const string doubleQuotedString = "\"asdf ghijk \u0394\"";
            Assert.AreEqual( doubleQuotedString, QuotedString.Text( ).Parse( doubleQuotedString ) );
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
