using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projecten2_DOTNET {
	public class NietActiefState : GebruikerState {
		public NietActiefState(Gebruiker context) : base(context) {

		}
		public NietActiefState() {

		}

		override
		public void activeerGebruiker() {
			if (Context.GetType() == typeof(HoofdVerantwoordelijke)) {
				Context.ToState(new ActiefState(Context));
			}
			else {
				throw new InvalidOperationException("gebruiker mag enkel worden geactiveerd door hoofdverantwoordelijke");
			}
		}
	}
}