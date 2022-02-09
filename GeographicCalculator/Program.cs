using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            LoadData(path, coordinates);
            SaveData(pathToSave, coordinates);
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
            int i= 0;
            foreach(Coordinate coordinate in coordinates)
            {
                lines[i] = coordinate.ToString();
                i++;
            }
            File.WriteAllLines(path, lines);
        }
    }


}
