﻿// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System.Collections.Immutable;

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Utility extension class to simplify parsing/binding operations</summary>
    public static class CommandLineBinderExtensions
    {
        /// <summary>Binds a parsed argument list to an object instance</summary>
        /// <typeparam name="TResult">Type of the object to bind to</typeparam>
        /// <param name="self">The object instance to bind</param>
        /// <param name="parseResults">Parsed argument list to bind</param>
        /// <returns>The input <paramref name="self"/> after binding for fluent style use</returns>
        public static TResult BindArguments<TResult>( this TResult self, IImmutableList<ICommandlineArgument> parseResults )
        {
            new CommandlineBinder( ).BindParseResults( self, parseResults );
            return self;
        }
    }
}
