using System;
using System.Collections.Generic;
using System.IO;

namespace GeographicCalculator
{

    class Program
    {
        //sciezka do pliku z danymi
        static string path = "../../../wspolrzedneWejsciowe.txt";
        //sciezka do zapisywanych plikow
        static string pathToSaveTXT = "../../../wspolrzedneTXT.txt";
        static string pathToSaveCSV = "../../../wspolrzedneCSV.csv";
        static string pathToSaveGML = "../../../wspolrzedneGML.gml";
        static string pathToSaveKML = "../../../wspolrzedneKML.kml";
        static string pathToSaveKMZ = "../../../wspolrzedneKMZ.kmz";
        //lista ktora przechowa wszystkie wczytane obiekty coordinate
        static List<Coordinate> coordinates = new List<Coordinate>();

        //stale obliczone wczesniej pomagajace przeliczac systemy
        const float xChanger = 113000.54f;
        const float yChanger = 280779.49f;

        static void Main(string[] args)
        {
            LoadData(path, coordinates);
            
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

            Console.WriteLine("\nPrzeliczenie koordynatow z systemu WGS84 na stopnie minuty i sekundy");
            foreach (Coordinate coordinate in coordinates)
            {
                ChangeFromWGS84ToDegrees(coordinate);
            }

            
            //zapis do txt
            Console.WriteLine($"\nZapis do .txt do pliku {pathToSaveTXT}");
            SaveData(pathToSaveTXT, coordinates);
            //zapis do csv
            Console.WriteLine($"\nZapis do .csv do pliku {pathToSaveCSV}");
            SaveData(pathToSaveCSV, coordinates);
            //zapis do gml
            Console.WriteLine($"\nZapis do .gml do pliku {pathToSaveGML}");
            DateClass.MakeGMLFile(coordinates, pathToSaveGML);
            //zapis do kml
            Console.WriteLine($"\nZapis do .kml do pliku {pathToSaveKML}");
            DateClass.MakeKMLFile(coordinates, pathToSaveKML);
            //pakowanie pliku kml do kmz
            Console.WriteLine($"\nPakowanie pliku .kml do pliku {pathToSaveKMZ} ");
            DateClass.MakeKMZFile(pathToSaveKML);
        }

        //utworzenie obiektow z kazdej linni pliku
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
                        //normalizacja danych, zmiana . na , aby mozna bylo zamienic stringi na floaty
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

            //wypis wczytanych danych
            Console.WriteLine($"Wczytano {coordinates.Count} obiekty/ow z danymi");
            foreach (Coordinate coordinate1 in coordinates)
            {
                Console.WriteLine(coordinate1);
            }
        }

        //zapisywanie danych do pliku tekstowego i csv
        static void SaveData(string path, List<Coordinate> coordinates)
        {
            //sprawdzenie istnienie pliku, jesli nie istnieje tworzy pusty plik
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            //zapis kazdego obiektu w oddzielnej linii
            string[] lines = new string[coordinates.Count+1];
            lines[0] = "ID;X;Y;GESUT";
            int i = 1;
            foreach (Coordinate coordinate in coordinates)
            {
                lines[i] = coordinate.ToStringForSave();
                i++;
            }
            File.WriteAllLines(path, lines);
        }

        //zmiana danych z formatu 2000 do WGS84 z zapisame w obiekcie
        static void ChangeFrom2000ToWGS84(Coordinate coordinate)
        {

            float wgs84X = coordinate.X / xChanger;
            float wgs84Y = coordinate.Y / yChanger;

            //zapisuje przeliczone wartosci do obiektu
            coordinate.Wgs84X = wgs84X;
            coordinate.Wgs84Y = wgs84Y;

            Console.WriteLine($"{coordinate.Id} X:{wgs84X} Y:{wgs84Y}");
        }

        //zmiana danych z formatu WGS84 do 2000 bez zapisu
        static void ChangeFromWGS84To2000(Coordinate coordinate)
        {

            float x2000 = coordinate.Wgs84X * xChanger;
            float y2000 = coordinate.Wgs84Y * yChanger;

            //brak zapisu do obiektu poniewaz wartosci te zostaly wpisane z pliku
            //metoda jedynie pokazuje ze mozliwa jest inwersja takze w druga strone


            Console.WriteLine($"{coordinate.Id} X:{x2000} Y:{y2000}");
        }

        //zmiana danych z formatu WGS84 na stopnie, minuty, sekundy
        static void ChangeFromWGS84ToDegrees(Coordinate coordinate)
        {
            float xWGS84 = coordinate.Wgs84X;
            float yWGS84 = coordinate.Wgs84Y;
            
            //zmiana wartosci X na stopnie minuty sekundy
            int deegreX = (int)xWGS84;
            int minutesX = (int)((xWGS84-deegreX)*60);
            int secondsX = (int)(((xWGS84 - deegreX) * 3600) - minutesX * 60);

            //zmiana wartosci Y na stopnie minuty sekundy
            int deegreY = (int)yWGS84;
            int minutesY = (int)((yWGS84 - deegreY) * 60);
            int secondsY = (int)(((yWGS84 - deegreY) * 3600) - minutesY * 60);

            Console.WriteLine($"{deegreX}° {minutesX}\' {secondsX}\" | {deegreY}° {minutesY}\' {secondsY}\"");
        }
    }


}
