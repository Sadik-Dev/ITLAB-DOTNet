using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using Xunit;
using static Projecten2_DOTNET.tests.DummyData;

namespace Projecten2_DOTNET.tests {
	public class FeedbackEntryTest {
		private FeedbackEntry _entry;
		private readonly DummyData _dummydata;
		private static Gebruiker jan;
		private static string lorem100;
		private static string lorem255;
		private static string lorem256;
		private static string lorem500;

		public FeedbackEntryTest() {
			_dummydata = new DummyData();
			jan = _dummydata.jan;
			lorem100 = _dummydata.lorem100;
			lorem255 = _dummydata.lorem255;
			lorem256 = _dummydata.lorem256;
			lorem500 = _dummydata.lorem500;
		}

		[Fact]
		public void FeedbackEntry_CorrectInfo_MakesNewFeedbackEntry() {
			_entry = null;
			Gebruiker geb = new Gebruiker("Peiter Declerq", "321654pd", 9876543219876);
			int score = 0;
			string inhoud = "Heel leuke sessie";
			_entry = new FeedbackEntry(geb, score, inhoud);

			Assert.Equal(geb, _entry.Auteur);
			Assert.Equal(score, _entry.Score);
			Assert.Equal(inhoud, _entry.Inhoud);

		}

		//public static IEnumerable<object[]> JuisteEntryData => new List<object[]> {
		//	new object[] { new Gebruiker("Jan Bromper", "321654jb", "P@ssword1"), 0, "Dit is de inhoud van een feedbackentry"},
		//	new object[]{ jan, 0, lorem100}
		//	//new object[] { pieter, 2, lorem255},
		//	//new object[] { thomas, 5, lorem255},
		//	//new object[] { jan, 3, lorem100}
		//};

		[Theory]
		[MemberData(nameof(FouteEntryData))]
		public void FeedbackEntry_FouteInfo_ThrowsException(Gebruiker gebruiker, int score, string inhoud) {
			_entry = null;
			Assert.Throws<ArgumentException>(() => new FeedbackEntry(gebruiker, score, inhoud));


		}

		public static IEnumerable<object[]> FouteEntryData => new List<object[]> {
			new object[] { new Gebruiker("Jan Koene", "654654jk", 9876543219876), 10, "Dit is de inhoud van de FeedbackEntry"},
			new object[] { jan, -1, lorem100},
			new object[] { jan, -5, lorem255},
			new object[] { jan, 6, lorem255},
			new object[] { jan, 10, lorem100},
			new object[] { jan, 2, lorem256},
			new object[] { jan, 3, lorem500}
		};
	}
}
