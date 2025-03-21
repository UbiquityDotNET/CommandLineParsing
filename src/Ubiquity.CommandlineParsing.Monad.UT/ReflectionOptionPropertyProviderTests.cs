// -----------------------------------------------------------------------
// <copyright file="ReflectionOptionPropertyProviderTests.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.CommandLineParsing.Monad.UT
{
    [TestClass]
    public class ReflectionOptionPropertyProviderTests
    {
        [TestMethod]
        public void ConstructorTest( )
        {
            _ = new TestOptions( );
            var provider = new ReflectionOptionPropertyProvider( );
            Assert.IsNotNull( provider );
        }

        [TestMethod]
        public void GetMultiPropertyTest( )
        {
            var options = new TestOptions( );
            var provider = new ReflectionOptionPropertyProvider( );
            Assert.IsNotNull( provider );
            var option = new CommandLineOption( "-", "m", ":", new CommandLineValue( "'Multi 1'" ) );
            IOptionProperty prop = provider.GetPropertyForOption( options, option );
            Assert.IsNotNull( prop );
            Assert.AreSame( prop.Option, option );
            Assert.IsFalse( prop.AllowSpaceDelimitedValue );
            Assert.IsFalse( prop.IsSet );
            Assert.IsTrue( prop.IsCollection );
        }

        [TestMethod]
        public void SetMultiPropertyTest( )
        {
            var options = new TestOptions( );
            var provider = new ReflectionOptionPropertyProvider( );
            Assert.IsNotNull( provider );
            var option = new CommandLineOption( "-", "m", ":", new CommandLineValue( "'Multi 1'" ) );
            IOptionProperty prop = provider.GetPropertyForOption( options, option );
            Assert.IsNotNull( prop );
            Assert.AreSame( prop.Option, option );
            Assert.IsFalse( prop.IsSet );
            prop.BindValue( );
            Assert.IsFalse( prop.IsSet );
            Assert.AreEqual( 1, options.MultiOption.Count, "Expected count of 1 since item should have been added" );
            Assert.AreEqual( option.Value.Text, options.MultiOption[ 0 ]);
        }
    }
}
