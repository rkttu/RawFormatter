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
            var members = Enumerable.Range(0, 5).Select(x => new KeyValuePair<string, int>($"Member{x}", x)).ToArray();

			// Act
            var fragment =
				$$"""
				namespace {{@namespace}}
				{
					using System;
			
					{{new If(isEnumType)
					{
						$$"""
						public enum {{typeName}} : {{memberType}}
						{
							{{new ForEach(members) {
								(KeyValuePair<string, int> item) => $$"""
								{{item.Key}} = {{item.Value}},
								"""
							}}}
						}
						""",

						$$"""
						public static class {{typeName}}
						{
							{{new ForEach(members) {
								(KeyValuePair<string, int> item) => $$"""
								// Test
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
        }
    }
}