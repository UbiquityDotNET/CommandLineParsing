// -----------------------------------------------------------------------
// <copyright file="IOptionPropertyProvider.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Interface for an option property provider.</summary>
    public interface IOptionPropertyProvider
    {
        /// <summary>Gets an <see cref="IOptionProperty"/> for a given instance and option.</summary>
        /// <param name="instance">Object instance the value is to bind to.</param>
        /// <param name="option">Option to bind.</param>
        /// <returns><see cref="IOptionProperty"/> for the property.</returns>
        /// <remarks>
        /// This method may obtain the information needed by any means it deems
        /// necessary. The <see cref="ReflectionOptionPropertyProvider"/> uses reflection
        /// to lookup the property at runtime. It is conceivable to use some sort of
        /// compile time code generation to create type specific providers that would
        /// avoid run-time reflection for any scenarios where reflection is deemed to
        /// heavy weight. (There isn't any such implementation at this time, but it should
        /// be plausible).
        /// </remarks>
        IOptionProperty GetPropertyForOption( object instance, CommandLineOption option );
    }
}
