using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;

namespace Projecten2_DOTNET.tests {
	public class DummyData {

		#region StandaardGebruikers

		public Gebruiker pieter = new Gebruiker("Tom  Devries", "123456td", 9876543219876);
		public Gebruiker jan = new Gebruiker("Arnold Bedroefd", "654321ab", 9876543219876);
		public Gebruiker thomas = new Gebruiker("Berend Ernst", "432561be", 9876543219876);
		public Gebruiker pol = new Gebruiker("Boer Kwartel", "432561bc", 9876543219876);
		public Gebruiker piet = new Gebruiker("Henk Everaar", "432561he", 9876543219876);


		#endregion

		public static Verantwoordelijke hoofdV = new Verantwoordelijke("Mik Koleman", "234567mk", 9876543219876);


		#region Lokalen
		public static Lokaal lokaal1 = new Lokaal("B0.001", 20);
		public static Lokaal lokaal2 = new Lokaal("B0.010", 20);
		public static Lokaal lokaal3 = new Lokaal("B0.100", 30);
		public static Lokaal lokaal4 = new Lokaal("B0.150", 40);
		#endregion



		#region Sessies

		public Sessie s1 = new Sessie(hoofdV, "JAVAFX", "GastSpreker1", lokaal1, new DateTime(2020, 3, 1, 12, 30, 0), new DateTime(2020, 3, 1, 13, 30, 0));
		public Sessie s2 = new Sessie(hoofdV, "DOTNET", "GastSpreker1", lokaal2, new DateTime(2020, 4, 1, 12, 30, 0), new DateTime(2020, 4, 1, 13, 30, 0));
		public Sessie s3 = new Sessie(hoofdV, "ANGULAR", "GastSpreker1", lokaal3, new DateTime(2020, 5, 1, 12, 30, 0), new DateTime(2020, 5, 1, 13, 30, 0));
		public Sessie s4 = new Sessie(hoofdV, "YOLOSCRIPT", "GastSpreker1", lokaal4, new DateTime(2020, 6, 1, 12, 30, 0), new DateTime(2020, 6, 1, 13, 30, 0));

		#endregion


		#region Inhouden
		public string lorem100 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m";
		public string lorem255 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis,";
		public string lorem256 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, ";
		public string lorem500 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu";
		#endregion





	}
}