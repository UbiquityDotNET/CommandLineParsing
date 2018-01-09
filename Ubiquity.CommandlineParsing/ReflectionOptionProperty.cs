// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// Licensed under the MIT license. See the LICENSE.md file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using CommandlineParsing.Properties;

namespace Ubiquity.CommandlineParsing
{
    internal class ReflectionOptionProperty
        : OptionPropertyBase
    {
        internal ReflectionOptionProperty( object instance
                                         , PropertyDescriptor prop
                                         , CommandlineOption option
                                         , bool allowSpaceDelimitedValue
                                         , bool isCollection
                                         , bool requiresValue
                                         )
            : base( option, allowSpaceDelimitedValue, isCollection, requiresValue )
        {
            Instance = instance;
            Prop = prop;
        }

        protected override void InternalSetValue( string value )
        {
            object convertedValue = value;

            // if the property is a collection, add the item to the list
            if( IsCollection )
            {
                object propValue = Prop.GetValue( Instance );
                var genericCollectionType = Prop.PropertyType.GetInterface( "System.Collections.Generic.ICollection`1" );
                if( genericCollectionType != null )
                {
                    var converter = TypeDescriptor.GetConverter( genericCollectionType.GetGenericArguments( )[ 0 ] );
                    convertedValue = converter.ConvertFromString( value );
                }

                const BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;
                propValue.GetType().InvokeMember( "Add", bindingFlags, null, propValue, new object[ ] { value } );

                return;
            }

            // handle boolean flags, which might not have a value to convert
            // assume true unless a value is specified
            if( Prop.PropertyType == typeof( bool ) )
            {
                convertedValue = true;

                // If there's a value attached, convert it to a boolean
                if( !string.IsNullOrWhiteSpace( value ) )
                {
                    try
                    {
                        convertedValue = Convert.ToBoolean( value, CultureInfo.CurrentCulture );
                    }
                    catch( FormatException ex )
                    {
                        throw new CommandlineParseException( CultureInfo.CurrentUICulture, Resources.InvalidOptionFormat_0_1, value, ex.Message );
                    }
                }
            }
            else
            {
                if( string.IsNullOrWhiteSpace( value ) )
                {
                    throw new CommandlineParseException( CultureInfo.CurrentUICulture, Resources.MissingValueForOption_0, Option.Text );
                }

                if( Prop.Converter != null )
                {
                    try
                    {
                        convertedValue = Prop.Converter.ConvertFromString( value );
                    }
                    catch( NotSupportedException ex )
                    {
                        throw new CommandlineParseException( CultureInfo.CurrentUICulture, Resources.InvalidOptionFormat_0_1, value, ex.Message );
                    }
                }
            }

            Prop.SetValue( Instance, convertedValue );
        }

        private readonly object Instance;
        private readonly PropertyDescriptor Prop;
    }
}
