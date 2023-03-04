using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine_1._0
{
    public class Vertex
    {
        public float X, Y, Z;
        public Vertex()
        {
            
        }

        public override string ToString()
        {
            string output = X+", "+Y+", "+Z;

            return output;
        }
    }
}
