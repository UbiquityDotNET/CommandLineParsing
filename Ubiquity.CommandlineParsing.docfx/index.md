# CommandlineParsing
When processing command line arguments applications commonly perform the following basic conceptual steps.

1. Parse the arguments into an analyzable form including identifying options/switches with values
1. Bind the results of the parse to a data structure that is then used by the application to adapt
   behavior based on the command line arguments.

The command line parsing library splits the argument processing into these two basic steps. Furthermore,
the actual parsing is isolated by an interface so that multiple implementations are allowed. The first
implementation uses the [Sprache Parser Combinator Library](https://github.com/sprache/Sprache), which
is a light weight (<50KB) parsing library useful for many applications. Other parsers are possible if
an application already needs to use a more traditional parser engine, but given the size of the Sprache
library it isn't likely to save much.

The parsing stage produces an ordered list of arguments that are either switches/options with a possible
value and general positional arguments that may be quoted to include whitespace. For some applications
the list is all that is needed. However, for others an object model is desired. This is where the binding
comes in. The binding stage takes the list of arguments and, with the aid of .NET attributes binds the
values from the command line to properties of an object instance. To allow for greater flexibility, the
actual property binding is isolated behind an interface to allow multiple implementations. The library
includes a standard .NET Reflection based property provider which is likely to serve for most uses. Though,
it is plausible to implement a compile time code generator that created a property provider that didn't need
reflection, making it suitable for .NET Native AOT compilation scenarios where minimizing reflection is desired.

## Key Features
* Flexible parsing
* Flexible binding

### Flexible Parsing
The commandLineParsing library takes a generalized approach to parsing command line arguments. While there
are a large variety of styles for providing arguments they all tend to share some things in common. Generally
speaking the grammar of command lines looks something like this:

```EBNF
PositionalArg = QuotedValue | UnquotedValue;
Option = SWITCH, Identifier [DELIM, (QuotedValue | UnquotedValue ) ];
args = {PositionalArg | Option };
```

One challenge with any general library for command line arguments is in determining what to allow for SWITCH
and DELIM. Generally, SWITCH is either a `-`, `--` or `/` though, for some applications multiple forms are
allowed and each may have distinct meanings or even namespaces for the options. Furthermore, many command line
styles use different delimiters for options that accept a value. Common delimiters are `:` and `=` though some
applications use whitespace as a delimiter, which poses a real challenge as the parser doesn't know which options
even allow a value.

To support all the variations a generalized parsing library requires some level of abstraction to achieve the
needed flexibility. The CommandLineParsing library provides this by using a approach that can parse all of the
variations and leaves the correctness to the application or binder. That is, the parser accepts all valid command
lines and some that are not valid for a given application. This, helps keep the application logic simpler and
allows for defining common semantic processing and object binding for specific scenarios.

### Flexible Binding
Parsing arguments produces an immutable list of [ICommandlineArgument](xref:Ubiquity.CommandlineParsing.ICommandlineArgument)
which is a common interface for either [CommandlineOption](xref:Ubiquity.CommandlineParsing.CommandlineOption) or
[CommandlineValue](xref:Ubiquity.CommandlineParsing.CommandlineValue). The application can work with that list directly or
use a binder to bind the parsed arguments to properties of an object instance. The [CommandlineBinder](xref:Ubiquity.CommandlineParsing.CommandlineBinder)
class provides common logic for walking the list of arguments to bind the properties to an object instance. The
actual binding of the value to the property is performed by an implementation of [IOptionProperty](xref:Ubiquity.CommandlineParsing.IOptionProperty).
Instances of IOptionProperty are provided by an implementation of another interface [IOptionPropertyProvider](xref:Ubiquity.CommandlineParsing.IOptionPropertyProvider)
which, given a CommandlineOption, will look up the property for the object to bind to and provides the IOptionProperty
implementation to do the binding.

>[!NOTE]
>The design the IOPtionPropertyProvider and IOptionProperty interfaces intentionally **does not require** the use of refelction, though it is allowed.
>In fact the default provider is [ReflectionOptionPropertyProvider](xref:Ubiquity.CommandlineParsing.ReflectionOptionPropertyProvider). The reflection provider
>covers the large majority of cases. However it isn't the only possible implementation. It is plausible to use some form of compile time reflection/AOP
>Weaver to generate an implementation of IOptionPropertyProvider for a given options class that does not require any run-time reflection.

### Example
The following example comes from the unit tests for the CommandlineParsing library and shows many of the capabilities for handling complex options
parsing and binding to an options class the application can use.

[!code-csharp[Test](../Ubiquity.CommandlineParsing.Monad.UT/BinderTests.cs)]

>[!NOTE]
>This example treats `-`, `--` and `/` as equivalent, though other behavior is possible
>by providing an instance of [ReflectionOptionPropertyProvider](xref:Ubiquity.CommandlineParsing.ReflectionOptionPropertyProvider), or some other implementation
>of IOptionPropertyProvider, to the binder that will select the appropriate property or reject the parsed arguments as appropriate.
