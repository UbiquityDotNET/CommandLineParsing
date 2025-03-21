// -----------------------------------------------------------------------
// <copyright file="EnvironmentEx.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Provides environment extensions for handling platform differences.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Obvious enough" )]
    public static class EnvironmentEx
    {
        /// <summary>Gets the raw unparsed command line (or as close to it as is possible for the given runtime/platform.</summary>
        /// <remarks>
        /// <para>On Desktop .NET 4.7 and earlier <see cref="Environment.CommandLine"/> provides the full command line without any
        /// funky escape character processing. But the args passed to main or returned from <see cref="Environment.GetCommandLineArgs()"/>
        /// have extra escape character processing. This extra processing is very problematic as it is unique to .NET apps
        /// (C and C++ apps don't have this). If the command line `foo.exe "abc\de f\"` is processed with escaping then the
        /// result the app see is `abc\de f"` (Note the inclusion of the trailing quote character), which is clearly not the
        /// path the user intended to provide. This has always been an annoyance for .NET developers but was easily worked around
        /// by not using the args to Main() and instead getting at the full args via Environment.CommandLine and parsing that using
        /// whatever rules the app wants to support for args.</para>
        /// <para>Unfortunately, with .NET Core things behave very differently. <see cref="Environment.CommandLine"/> is no longer
        /// the unprocessed command line and instead it is effectively a space delimited join of Environment.GetCommandLineArgs().
        /// Meaning that is has all the .NET Specific escaping applied and there is no platform independent means of getting at
        /// the unprocessed args.</para>
        /// <para>Thus, this implementation is forced to implement the only viable cross platform approach by doing a space delimited
        /// join of <see cref="Environment.GetCommandLineArgs()"/> with all it's wonky escaping rules.</para>
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0025:Use expression body for properties", Justification = "Makes a mess with comments" )]
        public static string CommandLine
        {
            get
            {
                // On .NET Core - no option exists to get the original command line, it isn't really possible to fully reverse
                // the escaping as the process is lossy. e.g. `"foo\"` -> `foo"`, and `foo\"`-> `foo"` with no way to know if
                // the opening quote was present. For a value with whitespace it can be inferred but otherwise it can't be known
                // which presents a problem as reversing the escaping could create quotes that are unmatched.
                return string.Join( " ", Environment.GetCommandLineArgs( ).Skip( 1 ) );
            }
        }
    }
}
