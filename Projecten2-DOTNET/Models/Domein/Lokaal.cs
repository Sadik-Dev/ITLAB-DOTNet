using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class Lokaal {

		#region Properties
		public int Id { get; set; }

		//public string Lokaalcode {
		//	get { return Lokaalcode; }
		//	set {
		//		//stack overflow
		//		string pattern = @"[ABCDEP][0-9]\.[0-9]{3}";

		//		Regex regex = new Regex(pattern);
		//		if (!regex.IsMatch(value)) {
		//			throw new FormatException();
		//		}
		//		else {
		//			this.Lokaalcode = value;
		//		}
		//		Lokaalcode = value;

		//	}
		//}

		public string Lokaalcode { get; set; }

		public int AantalPlaatsen { get; set; }
		#endregion
		public LokaalType lokaalType { get; set; }


		//constructor die overeenkomt met lokalen in databank
		public Lokaal(String lokaalcode, int aantalPlaatsen, string type) { //string type
			switch (type)
			{
				case "Vergaderzaal": lokaalType = LokaalType.Vergaderzaal; break;
				case "Leslokaal": lokaalType = LokaalType.Leslokaal; break;
				case "PC-klas": lokaalType = LokaalType.PCklas; break;
				case "Auditorium": lokaalType = LokaalType.Auditorium; break;
			}

			Lokaalcode = lokaalcode;
			AantalPlaatsen = aantalPlaatsen;
		}

		//constructor voor dummydata in datainitializer
		public Lokaal(String lokaalcode, int aantalPlaatsen)
		{ 
			Lokaalcode = lokaalcode;
			AantalPlaatsen = aantalPlaatsen;
		}

	}
}
