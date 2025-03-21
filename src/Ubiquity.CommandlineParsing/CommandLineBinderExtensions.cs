// -----------------------------------------------------------------------
// <copyright file="CommandLineBinderExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Immutable;

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Utility extension class to simplify parsing/binding operations.</summary>
    public static class CommandLineBinderExtensions
    {
        /// <summary>Binds a parsed argument list to an object instance.</summary>
        /// <typeparam name="TResult">Type of the object to bind to.</typeparam>
        /// <param name="self">The object instance to bind.</param>
        /// <param name="parseResults">Parsed argument list to bind.</param>
        /// <returns>The input <paramref name="self"/> after binding for fluent style use.</returns>
        public static TResult BindArguments<TResult>( this TResult self, IImmutableList<ICommandLineArgument> parseResults )
        {
            ArgumentNullException.ThrowIfNull(self);

            new CommandLineBinder( ).BindParseResults( self, parseResults );
            return self;
        }
    }
}
