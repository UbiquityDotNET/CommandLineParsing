// -----------------------------------------------------------------------
// <copyright file="CommandLineParseException.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Exception for errors in parsing or binding command line arguments.</summary>
    [Serializable]
    public class CommandlineParseException
        : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class.</summary>
        public CommandlineParseException( )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class.</summary>
        /// <param name="message">Exception message.</param>
        public CommandlineParseException( string message )
            : base( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class.</summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public CommandlineParseException( string message, Exception inner )
            : base( message, inner )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class with a formatted message.</summary>
        /// <param name="formatProvider">Format provider.</param>
        /// <param name="fmt">Composition string for the message.</param>
        /// <param name="args">Arguments fir the message.</param>
        public CommandlineParseException( IFormatProvider formatProvider, string fmt, params object[ ] args )
            : base( string.Format( formatProvider, fmt, args ) )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class with a formatted message.</summary>
        /// <param name="inner">Inner exception to wrap.</param>
        /// <param name="formatProvider">Format provider.</param>
        /// <param name="fmt">Composition string for the message.</param>
        /// <param name="args">Arguments fir the message.</param>
        public CommandlineParseException( Exception inner
                                        , IFormatProvider formatProvider
                                        , string fmt
                                        , params object[ ] args
                                        )
            : base( string.Format( formatProvider, fmt, args ), inner )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandlineParseException"/> class from serialization.</summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Context for serialization.</param>
        protected CommandlineParseException( SerializationInfo info
                                           , StreamingContext context
                                           )
            : base( info, context )
        {
        }
    }
}
