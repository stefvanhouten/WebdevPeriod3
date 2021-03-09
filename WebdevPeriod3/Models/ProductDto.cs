using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebdevPeriod3.Models
{
	public class ProductDto
	{
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }
		public List<string> SubSystems { get; set; }
		public string Description { get; set; }
		[Required]
		public bool TermsGDPR { get; set; }
	}
}