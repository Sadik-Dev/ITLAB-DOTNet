using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Models.Domein {
	public class Media {

		public string Path { get; set; }
		public string Description { get; set; }
		public int MediaID { get; set; }
		public MediaType Type { get ; set; }

		//public string URLPath { get; set; }

		public Sessie Sessie
		{
			get; set;
		}
		
		public Media(string path) {
			Path = path;
			
			//instellen description en type
			string[] pathArray = path.Split("/");
			string TextName = pathArray[pathArray.Length - 1];
			string[] DescriptionArray = TextName.Split(".");
			string TypeMap = pathArray[pathArray.Length - 2];
			switch (TypeMap)
			{
				case "MediaFiles": Type = MediaType.MediaFile; break;
				case "MediaImages": Type = MediaType.MediaImage; break;
				case "MediaLinks": Type = MediaType.MediaUrl;
									this.Path = File.ReadAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/MediaLinks/", TextName)); 
									break;
				default: throw new ArgumentException("Mediatype niet herkend");
			}

			Description = DescriptionArray[0];
		}

		public Media()
		{

		}

		

		
	}
}
