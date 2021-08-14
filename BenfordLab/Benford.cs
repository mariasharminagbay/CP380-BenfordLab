using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BenfordLab
{
    public class BenfordData
    {
        public int Digit { get; set; }
        public int Count { get; set; }

        public BenfordData() { }
    }

    public class Benford
    {
       
        public static BenfordData[] calculateBenford(string csvFilePath)
        {
            // load the data
            var data = File.ReadAllLines(csvFilePath)
                .Skip(1) // For header
                .Select(s => Regex.Match(s, @"^(.*?),(.*?)$"))
                .Select(data => new
                {
                    Country = data.Groups[1].Value,
                    Population = int.Parse(data.Groups[2].Value)
                });

            // manipulate the data!
            //
            // Select() with:
            //   - Country
            //   - Digit (using: FirstDigit.getFirstDigit() )
            var m_data = data
                .Select(m_data => new
                {
                    Country = m_data.Country,
                    Digit = FirstDigit.getFirstDigit(m_data.Population)
                });
            // Then:
            //   - you need to count how many of *each digit* there are

            Func<int, int> countDigit = i =>
            {
                var s = 0;
                foreach (var item in m_data)
                {
                    if (i == item.Digit)
                    {
                        s++;
                    }
                }
                return s;
            };

            int[] count = new int[9];
            int[] digit = new int[9];
            for (var i = 0; i < 9; i++)
            {

                digit[i] = i + 1;
                count[i] = countDigit(i + 1);
            }

            int temp, a;
            for (int j = 0; j <= count.Length - 2; j++)
            {
                for (int i = 0; i <= count.Length - 2; i++)
                {
                    if (count[i] > count[i + 1])
                    {
                        temp = count[i + 1];
                        count[i + 1] = count[i];
                        count[i] = temp;
                        a = digit[i + 1];
                        digit[i + 1] = digit[i];
                        digit[i] = a;


                    }
                }
            }
            Console.WriteLine("*********************************");
            // Lastly:
            //   - transform (select) the data so that you have a list of
            //     BenfordData objects
            //
            BenfordData[] b = new BenfordData[9];
            for (var i = 0; i < 9; i++)
            {
                BenfordData bo = new BenfordData()
                {
                    Count = count[i],
                    Digit = digit[i]

                };
                b[i] = bo;
            }
            var m = b;
            return m.ToArray();

        }
    }
}
