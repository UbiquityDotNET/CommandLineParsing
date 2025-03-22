// -----------------------------------------------------------------------
// <copyright file="BinderTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace Ubiquity.CommandLineParsing.Monad.UT
{
    [DefaultProperty( "PositionalArgs" )]
    internal class TestOptions
    {
        public enum Option3Values
        {
            Foo,
            Bar,
            Baz,
        }

        public List<string> PositionalArgs { get; } = [];

        [CommandLineArg( "o1", AllowSpaceDelimitedValue = true )]
        public string Option1 { get; set; } = string.Empty;

        public string Option2 { get; set; } = string.Empty;

        // standard converter for enums should not need to be explicitly declared so test that.
        // [TypeConverter(typeof(EnumConverter))]
        public Option3Values Option3 { get; set; }

        [CommandLineArg( "o4" )]
        public bool Option4 { get; set; }

        [CommandLineArg( "m" )]
        public IList<string> MultiOption { get; } = new List<string>( );
    }
}
