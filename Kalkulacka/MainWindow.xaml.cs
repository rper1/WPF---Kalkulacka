using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulacka
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string displ = "";
		string operace = "";
		double? cislo1 = null;
		double cislo2 = 0;
		bool smazat = false;
		bool opakovaneRovnaSe = false;

		public MainWindow()
		{
			InitializeComponent();
			AddKeyEvents();
		}
		/// <summary>
		/// reaguje na ovládání kalkulačky z klávesnice
		/// </summary>
		private void AddKeyEvents()
		{
			KeyDown += (sender, args) =>
			{
				switch (args.Key)
				{
					case Key.NumPad1:
						ZpracujNumKlavesu("1");
						break;
					case Key.NumPad2:
						ZpracujNumKlavesu("2");
						break;
					case Key.NumPad3:
						ZpracujNumKlavesu("3");
						break;
					case Key.NumPad4:
						ZpracujNumKlavesu("4");
						break;
					case Key.NumPad5:
						ZpracujNumKlavesu("5");
						break;
					case Key.NumPad6:
						ZpracujNumKlavesu("6");
						break;
					case Key.NumPad7:
						ZpracujNumKlavesu("7");
						break;
					case Key.NumPad8:
						ZpracujNumKlavesu("8");
						break;
					case Key.NumPad9:
						ZpracujNumKlavesu("9");
						break;
					case Key.NumPad0:
						ZpracujNumKlavesu("0");
						break;
					case Key.Back:
						ZpracujNumKlavesu("<<");
						break;
					case Key.Add:
						ZpracujOperKlavesu("+");
						break;
					case Key.Subtract:
						ZpracujOperKlavesu("-");
						break;
					case Key.Multiply:
						ZpracujOperKlavesu("*");
						break;
					case Key.Divide:
						ZpracujOperKlavesu("/");
						break;
					case Key.Enter:
						ZpracujRovnaSeKlavesu();
						break;
					case Key.Decimal:
						ZpracujNumKlavesu(",");
						break;
					case Key.Escape:
						displ = "";
						displayTextBlock.Text = "0";
						break;
				}
			};
		}
		/// <summary>
		/// reaguje na stisk tlačítek: čísla, +/-, čárka, smazání znaku
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cisloButton_Click(object sender, RoutedEventArgs e)
		{
			string cisloStr = ((Button)sender).Content.ToString();
			ZpracujNumKlavesu(cisloStr);
		}
		/// <summary>
		/// zpracovává stisk tlačítek
		/// </summary>
		/// <param name="cisloStr"></param>
		private void ZpracujNumKlavesu(string cisloStr)
		{
			opakovaneRovnaSe = false;
			if (smazat)
			{
				displ = "";
				smazat = false;
			}
			// nedovolí napsat dvě nuly vedle sebe
			if (cisloStr == "0" && displayTextBlock.Text == "0")
			{
				return;
			}
			// nedovolí napsat více než 18 čísel vedle sebe
			if (displayTextBlock.Text.Length == 18 && cisloStr != "<<" & cisloStr != "+/-")
			{
				return;
			}
			// ruční mazání čísla nebo čárky vpravo
			if (cisloStr == "<<")
			{
				// nesmaže exponent
				if (displayTextBlock.Text.Contains("E+"))
				{
					return;
				}
				// nesmaže exponent
				if (displayTextBlock.Text.Contains("E-"))
				{
					return;
				}
				// zobrazí 0 pokud je zobrazeno 0, nebo -0, nebo jen jedno číslo
				if (displayTextBlock.Text.Length == 2 && displayTextBlock.Text[0] == '-' || displayTextBlock.Text.Length == 3 && displayTextBlock.Text[0] == '-'
					&& displayTextBlock.Text[1] == '0' && displayTextBlock.Text[2] == ',' || displayTextBlock.Text.Length == 1)
				{
					displ = "";
					displayTextBlock.Text = "0";
					return;
				}
				// jinak smaže číslo vpravo
				else if (displayTextBlock.Text.Length > 1)
				{
					displayTextBlock.Text = displayTextBlock.Text.Remove(displayTextBlock.Text.Length - 1);
					displ = displayTextBlock.Text;
					return;
				}
				else
				{
					return;
				}
			}
			// nezobrazí druhou desetinnou čárku
			if (displ.Contains(",") && cisloStr == ",")
			{
				return;
			}
			// po smazání desetinné čárky za samotnou nulou zobrazí nulu
			if (cisloStr == "," && displ == "")
			{
				displ = "0";
			}
			// neguje znaménko, ale ne když je zobrazena pouze nula
			if (cisloStr == "+/-")
			{
				displ = (displayTextBlock.Text == "0") ? displ : displayTextBlock.Text;
				displ = (displ == "") ? "" : (-(double.Parse(displ))).ToString();
				displayTextBlock.Text = (displ == "") ? "0" : displ;
				return;
			}
			displ += cisloStr;
			displayTextBlock.Text = displ;

			// Stisknutí Enteru na klávesnici neopakuje to, co bylo stisknuto pomocí myši, např. číslo
			Keyboard.Focus(rovnitko);
		}
		/// <summary>
		/// reaguje na stisk tlačítek + - * / 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void operaceButton_Click(object sender, RoutedEventArgs e)
		{
			operace = ((Button)sender).Content.ToString();
			ZpracujOperKlavesu(operace);
		}
		/// <summary>
		/// po stisku některého z tlačítek + - * / si uloží obsah displeje
		/// </summary>
		/// <param name="oper"></param>
		private void ZpracujOperKlavesu(string oper)
		{
			operace = oper;
			opakovaneRovnaSe = false;
			try
			{
				cislo1 = double.Parse(displayTextBlock.Text, CultureInfo.GetCultureInfo("cs"));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			smazat = true;
		}
		/// <summary>
		/// reaguje na stisk tlačítka rovnítko
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rovnaSeButton_Click(object sender, RoutedEventArgs e)
		{
			ZpracujRovnaSeKlavesu();
		}
		/// <summary>
		/// po stisku rovnítka provede a zobrazí výpočet, místo NaN zobrazí 0
		/// </summary>
		private void ZpracujRovnaSeKlavesu()
		{
			if (cislo1 == null)
			{
				return;
			}
			if (opakovaneRovnaSe)
			{
				try
				{
					cislo1 = double.Parse(displayTextBlock.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				try
				{
					cislo2 = double.Parse(displayTextBlock.Text);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

			try
			{
				displayTextBlock.Text = Vypocet.Spocitej(cislo1, cislo2, operace).ToString();
				displayTextBlock.Text = (displayTextBlock.Text == "NaN") ? "0" : displayTextBlock.Text;
				smazat = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			opakovaneRovnaSe = true;
		}
		/// <summary>
		/// tlačítko ce smaže display
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ceButton_Click(object sender, RoutedEventArgs e)
		{
			displ = "";
			displayTextBlock.Text = "0";
		}
		/// <summary>
		/// tlačítko c kompletně zresetuje kalkulačku
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cButton_Click(object sender, RoutedEventArgs e)
		{
			displ = "";
			operace = "";
			cislo1 = null;
			cislo2 = 0;
			displayTextBlock.Text = "0";
		}
	}
}
