using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GeographicCalculator
{

    class Program
    {
        //sciezka do pliku z danymi
        static string path = "../../../wspolrzedneV2.txt";
        static string pathToSave = "../../../wspolrzedneZapisane.txt";
        //lista ktora przechowa wszystkie wczytane obiekty coordinate
        static List<Coordinate> coordinates = new List<Coordinate>();

        //stale obliczone wczesniej pomagajace przeliczac systemy
        const float xChanger = 113000.54f;
        const float yChanger = 280779.49f;

        static void Main(string[] args)
        {
            LoadData(path, coordinates);
            SaveData(pathToSave, coordinates);
            Console.WriteLine("\nPrzeliczenie koordynatow z systemu 2000 do WGS84");
            foreach (Coordinate coordinate in coordinates)
            {
                ChangeFrom2000ToWGS84(coordinate);
            }

            Console.WriteLine("\nPrzeliczenie koordynatow z systemu WGS84 do 2000");
            foreach (Coordinate coordinate in coordinates)
            {
                ChangeFromWGS84To2000(coordinate);
            }
        }

        static void LoadData(string path, List<Coordinate> coordinates)
        {
            bool first = true;
            //odczytywanie pliku linia po linii
            foreach (string line in File.ReadAllLines(path))
            {
                //sprawdzenie czy linia nie jest pusta
                if (line != "")
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        //Console.WriteLine(line);
                        //normalizacja danych, zmiana . na , aby mozna bylo zaminic stringi na floaty
                        string lineNormalized = line.Replace('.', ',');
                        string[] info = lineNormalized.Split(';');
                        //Console.WriteLine(info.Length);


                        //zamiana string na float, jesli sie nie uda obiekt nie zostanie utworzony
                        bool isX = float.TryParse(info[1], out float x);
                        bool isY = float.TryParse(info[2], out float y);
                        bool isZ = float.TryParse(info[3], out float z);
                        //jesli zamiana sie powiodla utworzenie obiektu coordinate z danymi
                        //Console.WriteLine($"{isX},{isY},{isZ}");
                        if (isX && isY && isZ)
                        {
                            Coordinate coordinate = new Coordinate(x, y, z, info[4]);
                            //dodanie koordynatu do listy
                            coordinates.Add(coordinate);

                        }
                    }
                }
            }

            Console.WriteLine($"Wczytano {coordinates.Count} obiekty/ow z danymi");
            foreach (Coordinate coordinate1 in coordinates)
            {
                Console.WriteLine(coordinate1);
            }
        }

        static void SaveData(string path, List<Coordinate> coordinates)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            string[] lines = new string[coordinates.Count];
            int i = 0;
            foreach (Coordinate coordinate in coordinates)
            {
                lines[i] = coordinate.ToString();
                i++;
            }
            File.WriteAllLines(path, lines);
        }

        static void ChangeFrom2000ToWGS84(Coordinate coordinate)
        {

            float wgs84X = coordinate.X / xChanger;
            float wgs84Y = coordinate.Y / yChanger;

            //zapisuje przeliczone wartosci do obiektu
            coordinate.Wgs84X = wgs84X;
            coordinate.Wgs84Y = wgs84Y;

            Console.WriteLine($"{coordinate.Id} X:{wgs84X} Y:{wgs84Y}");
        }

        static void ChangeFromWGS84To2000(Coordinate coordinate)
        {

            float x2000 = coordinate.Wgs84X * xChanger;
            float y2000 = coordinate.Wgs84Y * yChanger;

            //brak zapisu do obiektu poniewaz wartosci te zostaly wpisane z pliku
            //metoda jedynie pokazuje ze mozliwa jest inwersja takze w druga strone


            Console.WriteLine($"{coordinate.Id} X:{x2000} Y:{y2000}");
        }
    }


}
