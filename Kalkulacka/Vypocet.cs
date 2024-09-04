using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalkulacka
{
	internal class Vypocet
	{
		public static double Spocitej(double? a, double b, string operace)
		{
			double vysl = 0;
			switch (operace)
			{
				case "+": vysl = (double)(a + b); break;
				case "-": vysl = (double)a - b; break;
				case "*": vysl = (double)a * b; break;
				case "/":
					{
						if (b == 0)
						{
							throw new Exception("Nulou nelze dělit!");
						}
						else
						{
							vysl = (double)a / b; break;
						}
					}
			}
			return vysl;
		}
	}
}
