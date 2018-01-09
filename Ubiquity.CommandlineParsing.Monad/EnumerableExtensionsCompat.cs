// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

// Despite some claims/confusion from MS to the contrary net47 **is NOT** fully netstandard2.0 compliant.
// In particular, for this module, it does not provide the Append or Prepend Enumerable extension methods
// see: https://developercommunity.visualstudio.com/content/problem/123356/enumerableappend-extension-method-is-missing-in-ne.html
#if NET47
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>This is an internal duplicate of Extensions added to .NET Standard 2.0 but isn't part of .NET4.7 or earlier</summary>
    /// <remarks>
    /// This is duped here to enable use in down-level run-times. Furthermore, it uses a different
    /// name and is marked internal to prevent conflicts with the official implementation when
    /// built for run-times supporting that or other libraries that may use a similar trick but make the
    /// extensions public. (See: https://github.com/dotnet/corefx/pull/5947)
    /// </remarks>
    /// <seealso href="https://github.com/dotnet/corefx/pull/5947">GitHub PR that introduced these extensions</seealso>
    internal static class EnumerableExtensionsCompat
    {
        public static IEnumerable<TSource> Append<TSource>( this IEnumerable<TSource> source, TSource element )
        {
            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            return AppendIterator( source, element );
        }

        public static IEnumerable<TSource> Prepend<TSource>( this IEnumerable<TSource> source, TSource element )
        {
            if( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            return PrependIterator( source, element );
        }

        private static IEnumerable<TSource> AppendIterator<TSource>( IEnumerable<TSource> source, TSource element )
        {
            foreach( TSource e1 in source )
            {
                yield return e1;
            }

            yield return element;
        }

        private static IEnumerable<TSource> PrependIterator<TSource>( IEnumerable<TSource> source, TSource element )
        {
            yield return element;

            foreach( TSource e1 in source )
            {
                yield return e1;
            }
        }
    }
}
#endif
