using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebdevPeriod3.Models
{
    public class ProductDto
	{
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }
		public List<string> SubSystems { get; set; } = new List<string>();
		public string Description { get; set; }
		[Required]
		public bool ShowInCatalog { get; set; }
		[Required]
		public bool TermsGDPR { get; set; }
	}
}