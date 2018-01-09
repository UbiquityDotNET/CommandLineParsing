# CommandlineParsing
When processing command line arguments applications commonly perform the following basic conceptual steps.

1. Parse the arguments into an analyzable form including identifying options/switches with values
1. Bind the results of the parse to a data structure that is then used by the application to adapt
   behavior based on the command line arguments.

The command line parsing library splits the argument processing into these two basic steps. Furthermore,
the actual parsing is isolated by an interface so that multiple implementations are allowed. The first
implementation uses the [Sprache Parser Combinator Library](https://github.com/sprache/Sprache), which
is a light weight (<50KB) parsing library useful for many applications. Other parsers are possible if
an application already needs to use a more traditional parser engine but given the size of the Sprache
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



