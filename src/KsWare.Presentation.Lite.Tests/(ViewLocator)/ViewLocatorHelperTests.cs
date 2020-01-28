
using NUnit.Framework;

namespace KsWare.Presentation.Lite.Tests {

	[TestFixture]
	public class ViewLocatorHelperTests {

		[TestCase("Root.CustomView", "customview.baml")]
		[TestCase("Root.Views.CustomView", "views/customview.baml")]
		public void BestResourceMatch(string possibleViewName, string expectedResult) {
			var match=ViewLocatorHelper.BestResourceMatch(possibleViewName, new[] {
				"customview.baml",
				"views/customview.baml",
				"another/customview.baml"
			});
			Assert.That(match, Is.EqualTo(expectedResult));
		}

		[TestCase("Root.ShellViewModel"           , "ShellViewModel", ""          , "ViewModel", ""     , "View", "Root.ShellView")]
		[TestCase("Root.ViewModels.ShellViewModel", "ShellViewModel", "ViewModels", "ViewModel", "Views", "View", "Root.Views.ShellView")]
		public void BuildViewName(string viewmodelFullName, string viewmodelName, string parentFolder, string viewmodelSuffix, string viewFolder, string viewSuffix, string expectedResult) {
			var result = ViewLocatorHelper.BuildViewName(viewmodelFullName, viewmodelName, parentFolder, viewmodelSuffix, viewFolder, viewSuffix);
			Assert.That(result, Is.EqualTo(expectedResult));
		}

	}

}


