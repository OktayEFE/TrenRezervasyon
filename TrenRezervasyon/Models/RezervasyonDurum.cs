using System;

namespace TrenRezervasyon.Models
{
	public class RezervasyonDurum
	{
		public Tren? Tren { get; set; }
		public int RezervasyonYapilacakKisiSayisi { get; set; }
		public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
	}
}

