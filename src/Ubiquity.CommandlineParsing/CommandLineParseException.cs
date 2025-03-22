// -----------------------------------------------------------------------
// <copyright file="CommandLineParseException.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Exception for errors in parsing or binding command line arguments.</summary>
    [Serializable]
    public class CommandLineParseException
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="CommandLineParseException"/> class.</summary>
        public CommandLineParseException( )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandLineParseException"/> class.</summary>
        /// <param name="message">Exception message.</param>
        public CommandLineParseException( string message )
            : base( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandLineParseException"/> class.</summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public CommandLineParseException( string message, Exception inner )
            : base( message, inner )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandLineParseException"/> class with a formatted message.</summary>
        /// <param name="formatProvider">Format provider.</param>
        /// <param name="fmt">Composition string for the message.</param>
        /// <param name="args">Arguments fir the message.</param>
        public CommandLineParseException( IFormatProvider formatProvider, string fmt, params object[ ] args )
            : base( string.Format( formatProvider, fmt, args ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandLineParseException"/> class with a formatted message.</summary>
        /// <param name="inner">Inner exception to wrap.</param>
        /// <param name="formatProvider">Format provider.</param>
        /// <param name="fmt">Composition string for the message.</param>
        /// <param name="args">Arguments fir the message.</param>
        public CommandLineParseException( Exception inner
                                        , IFormatProvider formatProvider
                                        , string fmt
                                        , params object[ ] args
                                        )
            : base( string.Format( formatProvider, fmt, args ), inner )
        {
        }
    }
}
