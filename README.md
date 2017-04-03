# PolyBoolCS (C# port of polybooljs)

Boolean operations on polygons

This is a fairly straightforward port of the polybooljs library to the C# programming language. As much of the design 
flavor of the original library has been kept as possible, and changes have been kept primarily to those that were 
necessary to get the library to compile in C#. All original source code comments have been retained, and the usage of
the library's public API is nearly identical to the original.

# Features

1. Performs all boolean operations on polygons (union, intersection, difference, xor)
2. Explicit support for handling segments and points that are coincident
3. Handles any number of subject polygons and any number of clip polygons
4. Places no restrictions on input polygon type (can be strictly simple, weakly simple, self-intersecting, etc)
5. Provides an API for constructing efficient sequences of operations

# Resources

* Inspired by the F. Martinez (2008) algorithm:
    * [Paper](http://www.cs.ucr.edu/~vbz/cs230papers/martinez_boolean.pdf)
    * [Code](https://github.com/akavel/martinez-src)
* [View the polybooljs GitHub repo and documentation](https://github.com/voidqk/polybooljs)
* [View the polybooljs companion tutorial](http://syntheti.cc/article/polygon-clipping-pt2/)

# NOTES

Does not include any sort of visualization or demo, unlike polybooljs. It does include a few limited
unit tests, but those are restricted to verifying that the output exactly matches the output of 
polybooljs when using the polybooljs demo data as input, and does not attempt to verify correctness 
any further than that. This port was done as an intellectual exercise rather than for actual production
usage, and it is unlikely that there will be any further development or bugfixes other than perhaps 
incorporating future bug fixed from polybooljs as time permits (if any should be posted).
