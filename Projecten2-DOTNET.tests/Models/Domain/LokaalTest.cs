using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using Xunit;

namespace Projecten2_DOTNET.tests {
	public class LokaalTest {
		private Lokaal _lokaal;

		[Theory]
		[InlineData("B1.026", 28)]
		[InlineData("E4.008", 16)]
		[InlineData("P2.036", 40)]
		[InlineData("D5.005", 120)]
		public void Lokaal_JuisteInfo_MaaktNieuwLokaal(string code, int aantalPlaatsen) {
			_lokaal = null;
			_lokaal = new Lokaal(code, aantalPlaatsen);
		}


		// DEZE TEST VALT WEG OMDAT ER GEEN TEST GEBEURT AAN DE WEB-ZIJDE VAN HET PRODUCT. DE JUISTE DATA WORDT GECHECKED AAN DE DESKTOP-KANT
		//[Theory]
		//[InlineData(null, 28)]
		//[InlineData("   ", 28)]
		//[InlineData("G1.026", 28)]
		//[InlineData("M4.008", 16)]
		//[InlineData("G1.0263", 40)]
		//[InlineData("B1.026", -20)]
		//[InlineData("5.050", 120)]
		//[InlineData("B2036", 40)]
		//public void Lokaal_FouteInfo_ThrowsExceprtion(string code, int aantalPlaatsen) {
		//	_lokaal = null;
		//	Assert.Throws<FormatException>(() => new Lokaal(code, aantalPlaatsen));
		//}

	}
}
