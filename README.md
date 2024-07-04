# RawFormatter

**Important notice: This library is no longer maintained. It was developed to verify the idea of ​​templating using raw strings, but development was discontinued because it was judged to be complicated for actual use. If you are interested, please fork and use this project.**

[![NuGet Version](https://img.shields.io/nuget/v/RawFormatter)](https://www.nuget.org/packages/RawFormatter/) ![Build Status](https://github.com/rkttu/RawFormatter/actions/workflows/dotnet.yml/badge.svg)

A library that combines raw string literals and string interpolation introduced in C# 11, compatible with .NET Standard 2.0, to implement fast-acting template features.

## Minimum Requirements

- Requires a platform with .NET Standard 2.0 or later, and Windows Vista+, Windows Server 2008+
  - Supported .NET Version: .NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+, Mono 5.4+, UWP 10.0.16299+, Unity 2018.1+

## Breaking Change

### From 0.5.1 to 0.5.2

- Considering that the type name `Switch` may exist among namespaces linked by implicit reference in the BCL, change the type name to `Select`.

## How to use

### Use with conditional block

Initialize and pass instances of the `If`, `For`, `Select` and `ForEach` classes in the string interpolation interval. At this point, you can use collection initializers to implement a simplified syntax.

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
		{{new Select(cases)
		{
			{ 3, (int num) => $"select case / {num} with 3" },
			{ Select.Else, (int num) => $"select default / {num}" }
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
