# Access Bridge Explorer

[Access Bridge Explorer](https://github.com/google/access-bridge-explorer) is
a Windows application that allows exploring, as well as interacting with, the
Accessibility tree of any Java applications that uses the
[Java Access Bridge](https://www.google.com/?gws_rd=ssl#q=java+access+bridge)
to expose their accessibility features, for example
[Android Studio](http://developer.android.com/sdk/index.html) and
[IntelliJ](https://www.jetbrains.com/idea/).

[Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
consumes the same API that Windows screen readers supporting the
[Java Access Bridge](https://www.google.com/?gws_rd=ssl#q=java+access+bridge)
(e.g. [nvda](http://www.nvaccess.org/),
[Jaws](http://www.freedomscientific.com/Products/Blindness/JAWS)) consume.
As such, [Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
can be useful for validating accessibility support or identifying accessibilty
issues of such Java applications without having to rely on a screen reader.

**Note**: [Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
should not considered a screen reader, as it is merely a debugging tools
useful to developers of Java applications who want to validated/ensure
holistic support for screen readers in their application.


## Requirements

The [Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
application requires Windows 7 or later, and .NET 4.0 or later.
It is compatible with both the 32-bit and the 64-bit versions of Windows.

[Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
is written in C#, the source code can be compiled with Visual Studio 2015,
or later, including [Visual Studio 2015 Community](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx).

**Note**: The choice of C# as the programming language to implement
[Access Bridge Explorer](https://github.com/google/access-bridge-explorer)
was justified as a good compromise between ease of development and
ease of interacting with the Java Access Bridge native DLLs
(WindowsAccessBridge-32.dll and WindowsAccessBridge-64.dll).

## Disclaimer

This is not an official Google product.
