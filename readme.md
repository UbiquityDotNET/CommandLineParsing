# Purpose of this project
To be clear up front, this is **NOT** a production ready general purpose command line parsing library,
__nor is it intended to be one__.

This repository is intended to serve a few roles for the Ubiquity.NET set of projects:

1. Acts as a template for new projects
2. Provides and experimental playground for the Ubiquity DocFX template and general docs build infrastructure.
3. Test project for various aspects of cross targeting .NET run-times (.NET Core or desktop) including non-Windows
target run-times

While the code is functional and even has some tests it isn't maintained or released as
an actual supported project. New ideas in the way things are built including docs and versioning etc... will
appear here first before actual projects. To accomplish the goals some reasonable real code was needed so this
old library originally written to explore the Sprache parse was given a new lease on life as a "test dummy" of sorts.

# OK, But I'm looking for a commandline parser...
May we suggest [this one]( https://github.com/commandlineparser/commandline ) - it's on GitHub and MUCH more full
featured, in fact command line apps for Ubiquity.NET will use that one instead of this one, so that should tell
you something if you are considering using this.

# But I really like this library because...
OK, what the heck, it's OSS on GitHub, fork it and party on! Just be aware you are on your own for maintaining it.