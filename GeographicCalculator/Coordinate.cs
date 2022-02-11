
namespace GeographicCalculator
{
    class Coordinate
    {
        int id;
        static int lastId = 0;
        float x;
        float y;
        float z;
        string gesut;

        float wgs84X;
        float wgs84Y;

        //Hermetyzacja i korzystanie z wlasciowsci
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public int Id { get => id; }
        public float Wgs84X { get => wgs84X; set => wgs84X = value; }
        public float Wgs84Y { get => wgs84Y; set => wgs84Y = value; }

        //konstruktor
        public Coordinate(float x, float y, float z, string gesut)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            id = lastId++;
            this.gesut = gesut;
        }

        //nadpisana metoda ToString
        public override string ToString()
        {

            return $"{id};{x.ToString().Replace(',', '.')};{y.ToString().Replace(',', '.')};{z.ToString().Replace(',', '.')};{gesut}";
        }

        public string ToStringForSave()
        {

            return $"{id};{x.ToString().Replace(',', '.')};{y.ToString().Replace(',', '.')};{gesut}";
        }
    }
}
