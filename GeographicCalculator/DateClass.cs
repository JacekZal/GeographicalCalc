using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GeographicCalculator
{
    static class DateClass
    {
        //sciezki do pomocniczych danych gml
        static string gmlSchemaPath = "../../../schemaHelps/gmlSchemaHelp.txt";
        static string gmlPointPath = "../../../schemaHelps/gmlObject.txt";

        //sciezki do pomocniczych danych kml
        static string kmlSchemaPath = "../../../schemaHelps/kmlSchemaHelp.txt";
        static string kmlPointPath = "../../../schemaHelps/pointinKML.txt";

        //sciezki do pomocniczych danych i folderow kmz
        //folder z kopia kml
        static string kmlDir = "../../../wspolrzedneKML";
        //folder docelowy kmz
        static string kmzDir = "../../../wspolrzedneKMZ";
        //kopia pliku kml
        static string kmzFilePath = "../../../wspolrzedneKML/wspolrzedneKMZ.kml";
        public static void MakeGMLFile(List<Coordinate> coordinates, string path )
        {
            //schemat ze znacznikami do zmiany
            string allSchema = File.ReadAllText(gmlSchemaPath);
            //schemat pojedynczego punktuze znacznikami do zmiany
            string onepoint = File.ReadAllText(gmlPointPath);
            string objects = "";

            //wymiana znacznikow na wartosci dla wszystkich punktow
            foreach (Coordinate coordinate in coordinates) {
                string point = onepoint.Replace("{yHERE}", (coordinate.Wgs84Y+"").Replace(',', '.'));
                point = point.Replace("{xHERE}", (coordinate.Wgs84X + "").Replace(',', '.'));
                point = point.Replace("{IDHERE}", coordinate.Id + "");
                objects += point;
            }

            //dodanie punktow do calego schema
            allSchema = allSchema.Replace("{OBJHERE}", objects);
            //zapis do pliku 
            File.WriteAllText(path, allSchema); 
        }

        public static void MakeKMLFile(List<Coordinate> coordinates, string path)
        {
            //schemat ze znacznikami do zmiany
            string allSchema = File.ReadAllText(kmlSchemaPath);
            //schemat pojedynczego punktuze znacznikami do zmiany
            string onepoint = File.ReadAllText(kmlPointPath);
            string objects = "";

            //wymiana znacznikow na wartosci dla wszystkich punktow
            foreach (Coordinate coordinate in coordinates) {
                string point = onepoint.Replace("{yHERE}", (coordinate.Wgs84Y + "").Replace(',', '.'));
                point = point.Replace("{xHERE}", (coordinate.Wgs84X + "").Replace(',', '.'));
                string id = "ID_";
                for (int i = 0; i < 5; i++)
                {
                    //ID w kml maja wartosc ID_xxxxx wiec jesli liczba nie ma 5 cyfr przed nia dopisujemy zera
                    if (coordinate.Id < (int)MathF.Pow(10,(i+1)))
                    {
                        for (int j = 4-i; j > 0; j--)
                        {
                            id += "0";
                        }
                        id += coordinate.Id;
                        break;
                    }
                }
                point = point.Replace("{IDHERE}", id);
                objects += point;
            }

            //dodanie punktow do calego schema
            allSchema = allSchema.Replace("{OBJHERE}", objects);
            //zapis do pliku 
            File.WriteAllText(path, allSchema);
            
        }

        public static void MakeKMZFile(string pathToKMLfile)
        {
            //stworzenie folderu dla kmz ktory potem spakujemy
            Directory.CreateDirectory(kmzDir);
            //usuniencie starej wersji kmz
            if (File.Exists(kmzFilePath))
            {
                File.Delete(kmzFilePath);
            }
            //skopiowanie pliku kml do katalogu kmz
            File.Copy(pathToKMLfile, kmzFilePath);
            //usuniecie starego pliku kmz
            File.Delete(kmzDir + ".kmz");
            //spakowanie nowego pliku kml do kmz
            ZipFile.CreateFromDirectory(kmlDir, kmzDir + ".kmz") ;
        }

    }
}
