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

        public Vertex multiplyPerspectiveMatrixVector(Vertex vector)
        {
            Vertex oVector = new Vertex();
            oVector.X = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0,2]*vector.Z;
            oVector.Y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z;
            return oVector;
        }

        public Vertex multiplyMatrixVector(Vertex vector)
        {
            Vertex oVector = new Vertex();
            oVector.X = vector.X * matrix[0, 0] + vector.Y * matrix[0, 1] + vector.Z * matrix[0, 2];
            oVector.Y = vector.X * matrix[1, 0] + vector.Y * matrix[1, 1] + vector.Z * matrix[1, 2];
            oVector.Z = vector.X * matrix[2, 0] + vector.Y * matrix[2, 1] + vector.Z * matrix[2, 2];

            return oVector;
        }



    }
}
