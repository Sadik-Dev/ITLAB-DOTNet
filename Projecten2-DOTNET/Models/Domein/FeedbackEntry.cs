using Projecten2_DOTNET.Models.Domein;
using System;


public class FeedbackEntry {
	private int score;
	private string inhoud;
	public int Id { get; set; }
	public Gebruiker Auteur { get; set; }
	public int Score {
		get { return score; }
		set {
			if (value < 0 || value > 5) {
				throw new ArgumentException();
			}
			else {
				score = value;
			}
		}
	}

	public DateTime Datum { get; set; }
	public string Inhoud {
		get { return inhoud; }
		set {
			if (value.Length > 255) {
				throw new ArgumentException();
			}
			inhoud = value;
		}
	}


	public FeedbackEntry()
	{

	}
		
	
	public FeedbackEntry(Gebruiker auteur, int score, String inhoud) {
		Auteur = auteur;
		Score = score;
		Inhoud = inhoud;
		Datum = DateTime.Now;
	}


}
