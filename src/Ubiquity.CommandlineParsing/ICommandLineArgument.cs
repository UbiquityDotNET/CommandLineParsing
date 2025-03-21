// -----------------------------------------------------------------------
// <copyright file="ICommandLineArgument.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Interface for a command line argument.</summary>
    /// <remarks>
    /// An argument is either positional or a switch/option. This interface provides the
    /// basic functionality required for either type, which is just a single property
    /// to get the text of the original input.
    /// </remarks>
    public interface ICommandLineArgument
    {
        /// <summary>Gets the full text of argument.</summary>
        /// <remarks>
        /// <para>This is generally used for debugging and reporting errors to the user so that the actual full text of an invalid
        /// option is available to aid in clarity for the user.</para>
        /// <para>For a switch/option the full text of the value includes the leading switch, the name of the option any value separator and
        /// the value. For positional arguments it includes the full string (without the outermost quotes, if any, to allow spaces, etc.. in
        /// the value).</para>
        /// </remarks>
        string Text { get; }
    }
}
