using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class Program
    {
        private static long BinaryStringToInt(string binaryString)
        {
            var stringArray = binaryString.ToCharArray();
            Array.Reverse(stringArray);
            var reversedArray = new string(stringArray);
            long returnValue = 0;
            for (var i = 0; i < reversedArray.Length; i++)
            {
                if (reversedArray[reversedArray.Length - i - 1] == '0') continue;
                returnValue += (int) Math.Pow(2, i);
            }
            return returnValue;
        }

        private static string BinaryToString(string binaryString)
        {
            var returnString = "";
            var stringToTreat = $"0{binaryString}0";
            Console.WriteLine($"Length: {stringToTreat.Length}");
            for (var i = 0; i < stringToTreat.Length/8; i++)
            {
                var fragment = stringToTreat.Substring(i*8, 8);
                var intValue = BinaryStringToInt(fragment);
                Console.Write((char)intValue);
            }
            Console.WriteLine();

            return binaryString;
        }

        private static string GetMessageFromUrl(string url)
        {
            var client = new WebClient();
            Console.WriteLine($"Downloading {url} from server...");
            return client.DownloadString(url);
        }

        public static void Main(string[] args)
        {
            var message = GetMessageFromUrl("http://www2.mps.mpg.de/homes/heller/downloads/files/SETI_message.txt");
            Console.WriteLine("Done downloading, processing!");

            var nbFragments = message.Length/359.0f;
            const int imgWidth = 359;
            var imgHeight = message.Length / 359 / 7;

            Console.WriteLine($"Length: {nbFragments}");

            for (var img = 0; img < 7; img++)
            {
                var messageStartPosition = (img * imgHeight) * imgWidth;

                var header = "";
                var skipLines = 0;

                if (img >= 3)
                {
                    skipLines = 2;
                }
                var myBitmap = new Bitmap(imgWidth, imgHeight - skipLines);

                for (var i = 0; i < imgHeight; i++)
                {
                    var fragment = message.Substring(messageStartPosition + (i * 359), 359);
                    if (img == 1 || img == 2)
                    {
                        var value = fragment.Substring(0, 16);
                    }

                    if (i < skipLines)
                    {
                        header += fragment;
                    }
                    else
                    {
                        for (var j = 0; j < fragment.Length; j++)
                        {
                            myBitmap.SetPixel(j, i - skipLines, fragment[j] == '1' ? Color.White : Color.Black);
                        }
                    }
                }

                Console.WriteLine($"Header: {BinaryToString(header)}");

                myBitmap.Save($"TestBitmap{img}.bmp");
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
