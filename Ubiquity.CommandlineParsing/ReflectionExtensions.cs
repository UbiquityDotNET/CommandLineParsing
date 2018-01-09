// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System;

namespace Ubiquity.CommandlineParsing
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
