using System;
namespace TrenRezervasyon.Models
{
	public class RezervasyonSonuc
	{
		public bool RezervasyonYapilabilir { get; set; }
		public List<YerlesimAyrinti>? YerlesimAyrinti { get; set; }
	}
}

