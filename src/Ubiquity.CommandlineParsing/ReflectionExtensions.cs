// -----------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Ubiquity.CommandLineParsing
{
    internal static class ReflectionExtensions
    {
        internal static bool IsCollection( this Type type )
        {
            return type.GetInterface( "System.Collections.Generic.ICollection`1" ) != null
                || type.GetInterface( "System.Collections.Generic.IList" ) != null;
        }
    }
}
