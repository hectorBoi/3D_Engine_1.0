using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine_1._0
{
    public class Matrix2D
    {
        public float[,] matrix = new float[4,4];
        public Matrix2D()
        {
            matrix = new float[4, 4];
            Array.Clear(matrix, 0, matrix.Length);
        }
         
        public Vertex multiply(Vertex vector)
        {
            return null;
        }
        public Vertex multiplyMatrixVector(Vertex vector)
        {
            Vertex oVector = new Vertex();
            oVector.X = vector.X * matrix[0, 0] + vector.Y * matrix[1, 0] + vector.Z * matrix[2, 0] + matrix[3, 0];
            oVector.Y = vector.X * matrix[0, 1] + vector.Y * matrix[1, 1] + vector.Z * matrix[2, 1] + matrix[3, 1];
            oVector.Z = vector.X * matrix[0, 2] + vector.Y * matrix[1, 2] + vector.Z * matrix[2, 2] + matrix[3, 2];
            float w = vector.X * matrix[0, 3] + vector.Y * matrix[1, 3] + vector.Z * matrix[2, 3] + matrix[3, 3];

            if (w != 0)
            {
                oVector.X /= w; oVector.Y /= w; oVector.Z /= w;
            }
            return oVector;
        }
    }
}
