using System;
using System.Collections.Generic;
using System.Text;

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

        public Coordinate(float x, float y, float z, string gesut)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            id = lastId++;
            this.gesut = gesut;
        }

        public override string ToString()
        {
            
            return $"{id};{x.ToString().Replace(',', '.')};{y.ToString().Replace(',', '.')};{z.ToString().Replace(',', '.')};{gesut}";
        }
    }
}
