using System;
namespace TrenRezervasyon.Function
{
	public class ControlFunction
	{
		public int vagonControl(int kapasite, int doluKoltukSayisi)
		{
			double value = (doluKoltukSayisi*100) /kapasite;
			if (value < 70)
			{
				return (((kapasite*70)/100)-doluKoltukSayisi);
			}
			else
				return (0); 
		}
	}
}

