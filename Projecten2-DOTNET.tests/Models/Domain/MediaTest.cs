using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Projecten2_DOTNET.tests.Models.Domain {
	public class MediaTest {

	


		#region Constructor
		[Fact]
		public void MediaImage_WordtJuistAangemaakt() {
			Media med = new Media("/images/MediaImages/BillGatesProjecten.jpg");
			Assert.Equal("BillGatesProjecten", med.Description);
			Assert.Equal(MediaType.MediaImage, med.Type);
			Assert.Equal("/images/MediaImages/BillGatesProjecten.jpg", med.Path);
		}
		
		[Fact]
		public void MediaFile_WordtJuistAangemaakt() {
			Media med = new Media("/images/MediaFiles/BillGates.pdf");
			Assert.Equal("BillGates", med.Description);
			Assert.Equal(MediaType.MediaFile, med.Type);
			Assert.Equal("/images/MediaFiles/BillGates.pdf", med.Path);
		}

		#endregion

	}
}
