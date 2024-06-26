namespace RawFormatter.Test
{
    public class FormatterTest
    {
        [Fact]
        public void BasicTest()
        {
			// Arrange
            var @namespace = "Hello";
            var isEnumType = DateTime.Now.Second % 2 == 0;
            var typeName = "Candidates";
            var memberType = "int";
			var cases = DateTime.Now.Second % 3;
            var members = Enumerable.Range(0, 5).Select(x => new KeyValuePair<string, int>($"Member{x}", x)).ToArray();

			// Act
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

			// Assert
			Assert.NotEmpty(fragment);
            Assert.Contains("if - ", fragment);
            Assert.Contains("forloop", fragment);
            Assert.Contains("foreach", fragment);
            Assert.Contains("switch", fragment);
		}
	}
}