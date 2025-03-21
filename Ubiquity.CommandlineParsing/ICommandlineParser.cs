// -----------------------------------------------------------------------
// <copyright file="ICommandlineParser.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ubiquity.CommandlineParsing
{
    /// <summary>Interface for parsing command line arguments.</summary>
    /// <remarks>
    /// <para>This interface allows for different underlying parsing engines,
    /// depending on the needs of an application. For example there is a
    /// monad parser available that uses the Sprache parser combinator
    /// library. However, if the app already has a full featured parser,
    /// like ANTLR, it may not make sense to pull in the dependencies for
    /// the Sprache parser and instead, just use the ANTLR version instead.</para>
    /// <para>The parser accepts a superset of all valid command lines. That is,
    /// it parses and accepts without error all syntactically and semantically valid
    /// input AND some semantically invalid input. It is then up to the consuming
    /// application or binder to perform the final semantic evaluation. This helps
    /// keep the parser implementations generalized enough for re-use as-is while
    /// supporting a wide variety of command line styles without enforcing any
    /// one in particular.
    /// </para>
    /// <note type="Important">
    /// It is important to note that the result of the parse is intentionally
    /// **not** necessarily semantically valid for a given application. That is,
    /// the parser does not implement all the subtle rules of a particular use
    /// case. Instead it parses the fundamental syntax, and leaves the semantics
    /// to the consumer of the results. For example, '-a --all' would parse as
    /// two distinct options. It is up to the application or binder it uses to
    /// decide if that is an error. (They may represent distinct options or the
    /// '-a' may be a short form of '--all'. If they represent the same option, it
    /// is up to the application to decide if the multiple occurrences are valid
    /// or an error) This helps keep the parser implementation generally reusable
    /// and simplifies most applications to focusing on the task of validating
    /// the arguments instead of parsing them.
    /// </note>
    /// </remarks>
    public interface ICommandlineParser
    {
        /// <summary>Parse arguments list from platform provided list of args.</summary>
        /// <param name="args">Args list provided to Main() or via <see cref="System.Environment.GetCommandLineArgs"/>.</param>
        /// <returns>The list of parsed arguments in the order they appeared on the command line.</returns>
        /// <remarks>
        /// <para>This will parse the command line into the various components in the order they appeared
        /// on the command line. The ordering is important as some application may depend on specific
        /// ordering of the command line. For example git command lines usually take the form:
        /// `git (-o --option)? command (-o --option)*`
        /// Where each command has a different set of options it allows. Thus the first item must always
        /// be a positional value. (or not, as git itself may have some options too). Thus, the actual
        /// ordering of the results matters as the options that precede or follow a positional value may
        /// act as modifiers for the values.</para>
        /// <para>Another, related, benefit of returning the arguments in the order supplied is to allow
        /// for applications that desire space delimited values of options. For example the input text
        /// `-optionWithValue optionvalue -optionwithoutvalue positional` will return four items in
        /// the list. Only the application, which understands the semantics, can know that `optionvalue`
        /// really is the value to associate with the preceding option `-optionWithValue`.</para>
        /// </remarks>
        /// <seealso cref="CommandlineArgAttribute.AllowSpaceDelimitedValue"/>
        IImmutableList<ICommandlineArgument> Parse( IEnumerable<string> args );
    }
}
