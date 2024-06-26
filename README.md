# RawFormatter

[![NuGet Version](https://img.shields.io/nuget/v/RawFormatter)](https://www.nuget.org/packages/RawFormatter/) ![Build Status](https://github.com/rkttu/RawFormatter/actions/workflows/dotnet.yml/badge.svg) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

A library that combines raw string literals and string interpolation introduced in C# 11, compatible with .NET Standard 2.0, to implement fast-acting template features.

## Minimum Requirements

- Requires a platform with .NET Standard 2.0 or later, and Windows Vista+, Windows Server 2008+
  - Supported .NET Version: .NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+, Mono 5.4+, UWP 10.0.16299+, Unity 2018.1+

## How to use

### Use with conditional block

Initialize and pass instances of the `If`, `For`, `Switch` and `ForEach` classes in the string interpolation interval. At this point, you can use collection initializers to implement a simplified syntax.

```csharp
using RawFormatter;

...

var @namespace = "Hello";
var isEnumType = DateTime.Now.Second % 2 == 0;
var typeName = "Candidates";
var memberType = "int";
var cases = DateTime.Now.Second % 3;
var members = Enumerable.Range(0, 5).Select(x => new KeyValuePair<string, int>($"Member{x}", x)).ToArray();

var fragment =
	$$"""
	namespace {{@namespace}}
	{
		using System;

		{{new For(5) { (int i) => $"/* forloop / {i} time(s) */" } }}

		/*
		{{new Switch(cases)
		{
			{ 3, (int num) => $"switch case / {num} with 3" },
			{ Switch.Else, (int num) => $"switch default / {num}" }
		}
		}}
		*/
			
		{{new If(isEnumType)
		{
			$$"""
			public enum {{typeName}} : {{memberType}}
			{
				{{new ForEach(members) {
					(KeyValuePair<string, int> item) => $$"""
					{{item.Key}} = {{item.Value}}, /*if - true*/ /* foreach */
					"""
				}}}
			}
			""",

			$$"""
			public static class {{typeName}}
			{
				{{new ForEach(members) {
					(KeyValuePair<string, int> item) => $$"""
					// if - false && foreach
					public static readonly {{item.Value.GetType()}} {{item.Key}} = {{item.Value}};
					"""
				}}}
			}
			""",
		}}}
	}
	""";

Console.Out.WriteLine(fragment);
```

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
