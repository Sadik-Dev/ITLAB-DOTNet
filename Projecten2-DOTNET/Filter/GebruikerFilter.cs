using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Filter {
	public class GebruikerFilter : ActionFilterAttribute {
		private readonly IGebruikerRepository _gebruikerRepository;
		private readonly ISessieRepository _sessieRepository;

		public GebruikerFilter(IGebruikerRepository gebruikerRepository, ISessieRepository sessieRepository) {
			_gebruikerRepository = gebruikerRepository;
			_sessieRepository = sessieRepository;
		}

		public override void OnActionExecuting(ActionExecutingContext context) {
			//TODO Dit moet terug uit commentaar als het inloggen gefixt is
			context.ActionArguments["gebruiker"] = context.HttpContext.User.Identity.IsAuthenticated ? _gebruikerRepository.GetByGebruikersnaam(context.HttpContext.User.Identity.Name) : null;
			//context.ActionArguments["gebruiker"] = _gebruikerRepository.GetByGebruikersNaam("123456kd");

			base.OnActionExecuting(context);
		}
	}
}
