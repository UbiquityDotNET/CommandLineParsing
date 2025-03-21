// -----------------------------------------------------------------------
// <copyright file="ReflectionOptionPropertyProvider.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Ubiquity.CommandLineParsing.Properties;

namespace Ubiquity.CommandLineParsing
{
    /// <summary><see cref="IOptionPropertyProvider"/> that uses Reflection to find the properties to bind to.</summary>
    public class ReflectionOptionPropertyProvider
        : IOptionPropertyProvider
    {
        /// <summary>Initializes a new instance of the <see cref="ReflectionOptionPropertyProvider"/> class.</summary>
        public ReflectionOptionPropertyProvider( )
            : this( StringComparer.InvariantCultureIgnoreCase, true, false )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ReflectionOptionPropertyProvider"/> class.</summary>
        /// <param name="comparer">String comparer to use when matching the properties with an option.</param>
        /// <param name="allowSlash">Flag to indicate if "/" is allowed for options.</param>
        /// <param name="dashesDistinct">Flag to indicate if the "-" and "--" represent distinct short and long namespaces.</param>
        public ReflectionOptionPropertyProvider( StringComparer comparer, bool allowSlash, bool dashesDistinct )
        {
            Comparer = comparer;
            AllowSlash = allowSlash;
            DashesDistinct = dashesDistinct;
        }

        /// <inheritdoc/>
        public IOptionProperty GetPropertyForOption( object instance, CommandLineOption option )
        {
            string name = option.Name;

            foreach( PropertyDescriptor prop in TypeDescriptor.GetProperties( instance ) )
            {
                var argsAttribute = prop.Attributes.OfType<CommandlineArgAttribute>( ).SingleOrDefault( );
                string longName = argsAttribute?.LongName ?? prop.Name;
                string shortName = argsAttribute?.ShortName ?? string.Empty;
                bool isCollection = prop.PropertyType.IsCollection( );
                bool allowSpaceDelimitedValue = argsAttribute?.AllowSpaceDelimitedValue ?? false;
                bool requiresValue = prop.PropertyType != typeof( bool );

                // Check the short and long names of the CommandLineArgAttribute, if there is one
                if( !DashesDistinct || ( DashesDistinct && option.Delimiter == "-" ) || (AllowSlash && option.Delimiter == "/") )
                {
                    if( Comparer.Compare( shortName, name ) == 0 )
                    {
                        return new ReflectionOptionProperty( instance, prop, option, allowSpaceDelimitedValue, isCollection, requiresValue );
                    }
                }

                if( !DashesDistinct || ( DashesDistinct && option.Delimiter == "--" ) || ( AllowSlash && option.Delimiter == "/" ) )
                {
                    if( Comparer.Compare( longName, name ) == 0 )
                    {
                        return new ReflectionOptionProperty( instance, prop, option, allowSpaceDelimitedValue, isCollection, requiresValue );
                    }
                }
            }

            // property for the argument wasn't found
            throw new CommandlineParseException( CultureInfo.CurrentUICulture, Resources.UnknownOption_0, name );
        }

        private readonly StringComparer Comparer;
        private readonly bool AllowSlash;
        private readonly bool DashesDistinct;
    }
}
