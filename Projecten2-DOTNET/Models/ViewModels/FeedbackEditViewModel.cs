using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.ViewModels
{
    public class FeedbackEditViewModel
    {
        [Required]
        [Range(1, 5, ErrorMessage = "{0} moet tussen 1 en 5 liggen")]
        public int Score { get; set; }

        [Display(Name = "Beschrijving")]
        public string Inhoud { get; set; }

        [Required]
        public Gebruiker Auteur { get; set; }

        public FeedbackEditViewModel()
        {
        }

        public FeedbackEditViewModel(FeedbackEntry feedbackEntry, Gebruiker auteur)
        {
            Inhoud = feedbackEntry.Inhoud;
            Score = feedbackEntry.Score;
            Auteur = auteur;
            
        }
    }
}
