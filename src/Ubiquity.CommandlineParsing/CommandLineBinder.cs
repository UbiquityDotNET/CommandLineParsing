// -----------------------------------------------------------------------
// <copyright file="CommandLineBinder.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using Ubiquity.CommandLineParsing.Properties;

namespace Ubiquity.CommandLineParsing
{
    /// <summary>Type specific command line argument binder.</summary>
    /// <remarks>
    /// <para>Switches are matched against properties by an implementation of
    /// <see cref="IOptionPropertyProvider"/> to provide the properties.
    /// If no property name matches the command line switch, then a property
    /// with a <see cref="CommandLineArgAttribute"/> ShortName or LongName
    /// matching the switch is used. Duplicate switches are allowed if the
    /// property the option is bound to implements <see cref="System.Collections.Generic.IList{T}"/>.
    /// </para>
    /// <para>Property values are set by using a TypeConverter
    /// associated with the property. Using converters allows for a class to
    /// support custom parsing of values (e.g. GUID values can be parsed directly to
    /// a GUID type). Custom types with a defined TypeConverter will support parsing
    /// to custom types.</para>
    /// <para>Positional arguments are stored into an <see cref="System.Collections.Generic.IList{T}"/>
    /// of strings, which must be designated by the DefaultPropertyAttribute on the class of the instance
    /// the arguments are bound to. If no default property or an unsupported type exists for the property
    /// and positional arguments are provided an exception is thrown.</para>
    /// </remarks>
    public class CommandLineBinder
    {
        /// <summary>Initializes a new instance of the <see cref="CommandLineBinder"/> class using <see cref="ReflectionOptionPropertyProvider"/>as the property provider.</summary>
        public CommandLineBinder()
            : this( new ReflectionOptionPropertyProvider() )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="CommandLineBinder"/> class for a given object instance.</summary>
        /// <param name="propertyProvider">Property provider for resolving property names.</param>
        public CommandLineBinder( IOptionPropertyProvider propertyProvider )
        {
            PropertyProvider = propertyProvider;
        }

        /// <summary>Binds a previously parsed list of arguments to an object instance.</summary>
        /// <param name="instance">Instance of object to bind the arguments to.</param>
        /// <param name="parsedResults">Results to bind to the object this binder is for.</param>
        public void BindParseResults( object instance, IImmutableList<ICommandLineArgument> parsedResults )
        {
            for( int i = 0; i < parsedResults.Count; ++i )
            {
                switch( parsedResults[ i ] )
                {
                case CommandLineOption option:
                    BindOptionValue( instance, parsedResults, option, ref i );
                    break;

                case CommandLineValue value:
                    BindPositionalValue( instance, value );
                    break;

                default:
                    throw new CommandLineParseException( CultureInfo.CurrentUICulture
                                                       , Resources.UnknownOption_0
                                                       );
                }
            }
        }

        private void BindOptionValue( object instance, IImmutableList<ICommandLineArgument> parsedResults, CommandLineOption option, ref int i )
        {
            var prop = PropertyProvider.GetPropertyForOption( instance, option );
            if( option.Value != null || !prop.RequiresValue )
            {
                prop.BindValue( );
                return;
            }

            if( prop.AllowSpaceDelimitedValue )
            {
                // peek ahead to see if the next item is a usable value
                if( ( ( i < parsedResults.Count - 2 ) && ( parsedResults[ i + 1 ] is CommandLineValue value ) ) )
                {
                    // OK, it's valid so use it and advanced to the next item so it isn't consumed as positional
                    prop.BindValue( value.Text );
                    ++i;
                    return;
                }
            }

            throw new CommandLineParseException( CultureInfo.CurrentUICulture, Resources.MissingValueForOption_0, option.Text );
        }

        private static void BindPositionalValue( object instance, CommandLineValue value )
        {
            // positionalArgs may be null if the type doesn't have a default property
            // with the correct type signature.
            var targetPositionalArgs = GetTargetPositionalArgList( instance )
                                     ?? throw new CommandLineParseException( CultureInfo.CurrentUICulture
                                                                           , Resources.UnknownOption_0
                                                                           , value.Text
                                                                           );

            targetPositionalArgs.Add( value.Text );
        }

        // finds the default property with a string list type
        private static IList<string>? GetTargetPositionalArgList( object instance )
        {
            var p = TypeDescriptor.GetDefaultProperty( instance );
            return p == null ? null : p.GetValue( instance ) as IList<string>;
        }

        private readonly IOptionPropertyProvider PropertyProvider;
    }
}
