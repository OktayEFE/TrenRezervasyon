using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrenRezervasyon.Function;
using TrenRezervasyon.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrenRezervasyon.Controllers
{
	[Route("api/[controller]")]
	public class TrenController : Controller
	{
		ControlFunction control = new ControlFunction();
		RezervasyonSonuc rezervasyonSonuc = new RezervasyonSonuc();
		List<YerlesimAyrinti> yerlesimAyrintis = new List<YerlesimAyrinti>();
		List<string> islemSonucu = new List<string>();

		// GET: api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/5
		[HttpGet("{id:int}")]
		public string Get(int id)
		{
			return "value";
		}


		// POST api/values
		[HttpPost]
		public IActionResult Post([FromBody]RezervasyonDurum rezervasyonDurum)
		{
			List<VagonBosKoltuk> vagonBosKoltuk = new List<VagonBosKoltuk>();
			
			int rezervasyonSayisi = rezervasyonDurum.RezervasyonYapilacakKisiSayisi;
			int rezervasyonBoskoltukSayisi = 0;//farklı vagonlarda toplam rezervasyon sayısına ulaşmak için kulanılacak değişken
			int toplamBosKoltukSayisi = 0;//Toplam boş koltuk sayısını tutmak için değişken
			try
			{
				// Boş koltuk sayılarının tutulduğu listenin doldurulması
				foreach (var item in rezervasyonDurum.Tren.Vagonlar)
				{
					vagonBosKoltuk.Add(new VagonBosKoltuk()
					{
						VagonAdi = item.Ad,
						BosKoltukSayisi = control.vagonControl(item.Kapasite, item.DoluKoltukAdet)
					}
						);
					toplamBosKoltukSayisi += control.vagonControl(item.Kapasite, item.DoluKoltukAdet);
				}
				//////////////////////////////////////////////////////////
				// Rezervasyon kontrol işlemleri
				
				if (toplamBosKoltukSayisi<rezervasyonDurum.RezervasyonYapilacakKisiSayisi)
				{//Boş koltuk sayısı yeterli değil ise
					rezervasyonSonuc.YerlesimAyrinti =yerlesimAyrintis;
					rezervasyonSonuc.RezervasyonYapilabilir = false;
					return BadRequest(rezervasyonSonuc);
				}
				else
				{//Boş koltuk sayısıı yeterli ise 
					if (rezervasyonDurum.KisilerFarkliVagonlaraYerlestirilebilir)
					{//Farklı vagon için rezervasyon alınması
						foreach (var item in vagonBosKoltuk)
						{
						rezervasyonSonuc.RezervasyonYapilabilir = true;
						
							if (rezervasyonDurum.RezervasyonYapilacakKisiSayisi == rezervasyonBoskoltukSayisi)
								return Ok(rezervasyonSonuc);
							if(item.BosKoltukSayisi!=0)
							{
								if (item.BosKoltukSayisi<=rezervasyonSayisi)
								{
								yerlesimAyrintis.Add(new YerlesimAyrinti()
									{
										VagonAdi = item.VagonAdi,
										KisiSayisi = item.BosKoltukSayisi
									}
									);
									rezervasyonSayisi -= item.BosKoltukSayisi;
								}
								else
								{
									yerlesimAyrintis.Add(new YerlesimAyrinti()
									{
										VagonAdi = item.VagonAdi,
										KisiSayisi = rezervasyonSayisi
									}
									);
									rezervasyonSayisi =0;
								}
								
								if (rezervasyonSayisi == 0)
								{
								rezervasyonSonuc.YerlesimAyrinti = yerlesimAyrintis;
								return Ok(rezervasyonSonuc);
								}
							}
						}
					rezervasyonSonuc.YerlesimAyrinti = yerlesimAyrintis;
						return Ok(rezervasyonSonuc);
					}
					else
					{//Herkesin aynı vagon için rezervasyon alınmasın
						rezervasyonSonuc.RezervasyonYapilabilir = true;
						yerlesimAyrintis.Add(new YerlesimAyrinti()
						{
							VagonAdi = vagonBosKoltuk.Where(x => x.BosKoltukSayisi == rezervasyonDurum.RezervasyonYapilacakKisiSayisi).FirstOrDefault().VagonAdi,
							KisiSayisi = vagonBosKoltuk.Where(x => x.BosKoltukSayisi == rezervasyonDurum.RezervasyonYapilacakKisiSayisi).FirstOrDefault().BosKoltukSayisi
						});
					rezervasyonSonuc.YerlesimAyrinti = yerlesimAyrintis;
					return Ok(rezervasyonSonuc);

					}
					//return Ok(rezervasyonDurum);
				}

			}
			catch (Exception ex)
			{
				return new JsonResult(ex.Message)
				{
					StatusCode = (int)HttpStatusCode.InternalServerError
				};
			}


		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}

