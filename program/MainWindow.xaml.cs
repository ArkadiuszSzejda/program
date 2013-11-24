using System;
using System.Collections.Generic;
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
using System.Globalization;
namespace program
{


    public partial class MainWindow : Window
    {
        string fName;
        const int CHARNUMBER = 64;              //zmniejszone z 128 by mozna bylo cos zobaczyc na ekranie
        const int ITERATIONS = 5;               //jak wyzej
        const int X = 1000;                     //szerokosc ekranu
        const int Y = 1000;                     //wysokosc ekranu

        public MainWindow()
        {
            InitializeComponent();
            fName = CreateFile();
            OpenFileAndDraw(fName);
        }

        private string[] CreateCharacterArray() //metoda tworzaca tablice ze znakami 0-9, A-Z, a-z
        {
            char asciiCode;            
            string[] table = new string[62];

            int j = 48;                         //zaczynajac od 48 = 0 w tablicy znakow ascii
            for (int i = 0; i < 10; i++)
            {
                asciiCode = (char)j;
                table[i] = asciiCode.ToString();
                j++;
            }

            j = 65;                             //A
            for (int i = 10; i < 36; i++)
            {
                asciiCode = (char)j;
                table[i] = asciiCode.ToString();
                j++;
            }

            j = 97;                             //a
            for (int i = 36; i < 62; i++)
            {
                asciiCode = (char)j;
                table[i] = asciiCode.ToString();
                j++;
            }
            return table;
        }

        private string CreateFile()
        {
            Random rnd = new Random();
            string fileName = null;
            string[] characterArray = new string[62];
            string randomCharacterString = null;
            int itr = 0;

            characterArray = CreateCharacterArray(); 
            for (int i = 0; i < 6; i++)                 
            {
                fileName += characterArray[rnd.Next(62)];         //losowanie nazwy pliku przy wykorzystaniu tablicy pomocniczej    
            }
            fileName += ".csv";            

            do                                                  
            {
                for (int i = 0; i < CHARNUMBER; i++)            
                {
                    randomCharacterString += characterArray[rnd.Next(62)];       //losowanie znakow i dodawanie do ciagu             
                }
                if (itr != ITERATIONS - 1)
                    randomCharacterString += "--!!!--";         //rozdzielenie ciagow stringiem 
                itr++;
            } while (itr < ITERATIONS);                         //ilość powtorzen ciagu znakow

            System.IO.File.WriteAllText(fileName, randomCharacterString);  //zapisanie tekstu do pliku
            return fileName;
        }

        private void OpenFileAndDraw(string fileName)
        {
            
            string text = System.IO.File.ReadAllText(fileName);    //wczytanie tekstu z pliku
            int length = text.Length;
            double dLength = text.Length;
            char[] characterArray = new char[length];            
            characterArray = text.ToCharArray();                                //wpisanie tekstu do tablicy znakowej
            int numberOfWords = (int)Math.Ceiling(dLength / 3);                 //dzielenie z zaokragleniem w gore aby nie ucielo koncowki dwu lub jedno znakowej
            string[] words = new string[numberOfWords];
            int j = 0;
            int k = 0;

            for (int i = 0; i < length; i++)                            //utworzenie tablicy po 3 znaki w komorce
            {
                if (k < 3)
                {
                    words[j] += characterArray[i].ToString();
                    k++;
                }
                else
                {
                    k = 1;
                    j++;
                    words[j] += characterArray[i].ToString();
                }
            }



            /**************       ustawienia rysowanego tekstu      ****************************************************************/                 
            CultureInfo p2 = new CultureInfo("en-us");
            FlowDirection p3 = new FlowDirection();
            Typeface p4 = new Typeface(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Light, FontStretches.Normal);
            int p5 = 10;
            /***********************************************************************************************************************/


            
            RenderTargetBitmap bmp = new RenderTargetBitmap(X, Y, 100, 100, PixelFormats.Pbgra32);              //utworzenie bitmapy na ktorej wyswietlony zostanie tekst

            j = 0;
            int wordHigh = Y / numberOfWords;
            int wordWidth = X / numberOfWords;
            for (int i = numberOfWords; i > 0; i--)
            {
                FormattedText textToDraw = new FormattedText(words[i - 1], p2, p3, p4, p5, Brushes.Black);  
                bmp.Render(DrawNext3Chars(i * wordWidth, j * wordHigh, textToDraw));                             //wypisywanie 3 znakowych slow na bitmapie
                j++;
            } 

            myImage.Source = bmp;           //wyswietlenie tekstu
        }

        private Visual DrawNext3Chars(int x, int y, FormattedText textToDraw)    //metoda rysujaca
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                drC.DrawText(textToDraw, new Point(x, y));
            }
            return drV;
        }
        


    }
}
