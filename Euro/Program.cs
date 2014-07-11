using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euro
{
    using Euro.GmServices;
    using Euro.WebServiceX;

    class Program
    {
        static void Main(string[] args)
        {
            var priceWrabber = new PartsWSClient();
            var currencyConvertor = new CurrencyConvertorSoapClient();

            var partPricing = priceWrabber.getPartPricing();

            Console.OutputEncoding = Encoding.Default;
            Console.WriteLine("Quering part prices...");

            var usdToEuroRate = currencyConvertor.ConversionRate(Currency.USD, Currency.EUR);

            Console.WriteLine("Quering current currency change rate...");

            Console.WriteLine("Current currency rate from USD to EUR is: {0}", usdToEuroRate);
            Console.WriteLine();
            Console.WriteLine("{0,-10}\t{1,7}\t{2,15}", "Part Number", "USD", "EUR");

            foreach (var pp in partPricing)
            {
                Console.WriteLine("{0,10}\t{1,10:$#,##0.00}\t{2,10:€#,##0.00}", pp.part_number, pp.msrp, pp.msrp * usdToEuroRate);
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
