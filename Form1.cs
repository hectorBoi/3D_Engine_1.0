using System;
using System.Numerics;

namespace _3D_Engine_1._0
{
    public partial class Form1 : Form
    {
        static Bitmap bmp;
        static Graphics g;
        float Wx, Hy, fThetaX, fThetaY;
        Point3D centroid;
        Vertex v1, v2, v3, v4;
        Matrix2D matProj, matRotX, matRotY, matRotZ;
        const float radian = 0.0174533f;
        const float pi = 3.14159265359f;
        float angle;
        Mesh meshCube;
        List<Vertex> puntosSphere;

        public struct Point3D
        {
            public float X, Y, Z;
        }

        public struct Triangle
        {
            public Vertex[] Vertices;
        }

        public struct Mesh
        {
            public Triangle[] triangles;
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {


            //bool valid = float.TryParse(angleTextBox.Text, out angle);

            
            if (angle >= 360)
            {
                angle = 0;
            }
            angle += 3.0f;

            fThetaX = angle * radian;

            matRotX.matrix[0, 0] = 1;
            matRotX.matrix[1, 1] = (float)Math.Cos(fThetaX);
            matRotX.matrix[1, 2] = (float)-Math.Sin(fThetaX);
            matRotX.matrix[2, 1] = (float)Math.Sin(fThetaX);
            matRotX.matrix[2, 2] = (float)Math.Cos(fThetaX);

            matRotZ.matrix[0, 0] = (float)Math.Cos(fThetaX);
            matRotZ.matrix[0, 1] = (float)-Math.Sin(fThetaX);
            matRotZ.matrix[1, 0] = (float)Math.Sin(fThetaX);
            matRotZ.matrix[1, 1] = (float)Math.Cos(fThetaX);
            matRotZ.matrix[2, 2] = 1;
            Render();
        }

        
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(PCT_CANVAS.Width, PCT_CANVAS.Height);
            Wx = bmp.Width/2;
            Hy = bmp.Height/2;
            matProj = new Matrix2D();
            matRotX = new Matrix2D();
            matRotZ = new Matrix2D();
            g = Graphics.FromImage(bmp);
            PCT_CANVAS.Image = bmp;

            Init();

            //vCentroid = new Veretex(new float[] {})
        }

        private void Init()
        {
            Sphere(1, 21);
            map(0, 0, 10, -pi, pi);
            angle = 2;
            fThetaX = 0;
            //Projection Matrix
            
            matProj.matrix[0, 0] = 1;
            matProj.matrix[1, 1] = 1;

            //Rotation Matrix Z
            matRotZ.matrix[0, 0] = (float)Math.Cos(fThetaX);
            matRotZ.matrix[0, 1] = (float)-Math.Sin(fThetaX);
            matRotZ.matrix[1, 0] = (float)Math.Sin(fThetaX);
            matRotZ.matrix[1, 1] = (float)Math.Cos(fThetaX);
            matRotZ.matrix[2, 2] = 1;

            //Rotation Matrix X
            matRotX.matrix[0, 0] = 1;
            matRotX.matrix[1, 1] = (float)Math.Cos(fThetaX);
            matRotX.matrix[1, 2] = (float)-Math.Sin(fThetaX);
            matRotX.matrix[2, 1] = (float)Math.Sin(fThetaX);
            matRotX.matrix[2, 2] = (float)Math.Cos(fThetaX);


            //Cube Declaration
            Vertex aV = new Vertex() {X = 1, Y = 1, Z = 1};
            Vertex bV = new Vertex() { X = -1, Y = 1, Z = 1 };
            Vertex cV = new Vertex() { X = -1, Y = -1, Z = 1 };
            Vertex dV = new Vertex() { X = 1, Y = -1, Z = 1 };
            Vertex eV = new Vertex() { X = 1, Y = 1, Z = -1 };
            Vertex fV = new Vertex() { X = -1, Y = 1, Z = -1 };
            Vertex gV = new Vertex() { X = -1, Y = -1, Z = -1 };
            Vertex hV = new Vertex() { X = 1, Y = -1, Z = -1 };

            Triangle t1 = new Triangle() {Vertices = new Vertex[] {aV, bV, cV} };
            Triangle t2 = new Triangle() { Vertices = new Vertex[] { aV, cV, dV } };

            Triangle t3 = new Triangle() { Vertices = new Vertex[] { eV, aV, dV } };
            Triangle t4 = new Triangle() { Vertices = new Vertex[] { eV, dV, hV } };
            Triangle t5 = new Triangle() { Vertices = new Vertex[] { fV, eV, hV } };
            Triangle t6 = new Triangle() { Vertices = new Vertex[] { fV, hV, gV } };
            Triangle t7 = new Triangle() { Vertices = new Vertex[] { bV, fV, gV } };
            Triangle t8 = new Triangle() { Vertices = new Vertex[] { bV, gV, cV } };
            Triangle t9 = new Triangle() { Vertices = new Vertex[] { eV, fV, bV } };
            Triangle t10 = new Triangle() { Vertices = new Vertex[] { eV, bV, aV } };
            Triangle t11 = new Triangle() { Vertices = new Vertex[] { cV, gV, hV } };
            Triangle t12 = new Triangle() { Vertices = new Vertex[] { cV, hV, dV } };
            
            meshCube = new Mesh() { triangles = new Triangle[] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12 } };
        }

        public void Render()
        {
            g.Clear(Color.Black);
            //DrawSquare();
            DrawSphere();

            PCT_CANVAS.Invalidate();

        }

        public void DrawSphere()
        {
            List<Vertex> vertices2 = new List<Vertex>();
            for (int i = 0; i<puntosSphere.Count; i++)
            {
                vertices2.Add(matRotZ.multiplyMatrixVector(puntosSphere[i]));
                vertices2[i] = matRotX.multiplyMatrixVector(vertices2[i]);
                vertices2[i] = (projection(vertices2[i]));
                vertices2[i] = (scale(vertices2[i], 300));
                vertices2[i] = (TranslateToO(vertices2[i]));
                
                g.FillRectangle(Brushes.Lime, vertices2[i].X, vertices2[i].Y, 4, 4);
            }
        }
        public void DrawSquare()
        {
            for(int i = 0; i<meshCube.triangles.Length;i++)
            {
                Triangle triProj, triRot, triangle;
                triangle = meshCube.triangles[i];
                //Rotate on X
                triRot = new Triangle();
                triRot.Vertices = new Vertex[3];

                
                triRot.Vertices[0] = matRotX.multiplyMatrixVector(triangle.Vertices[0]);
                triRot.Vertices[1] = matRotX.multiplyMatrixVector(triangle.Vertices[1]);
                triRot.Vertices[2] = matRotX.multiplyMatrixVector(triangle.Vertices[2]);
                
                triRot.Vertices[0] = matRotZ.multiplyMatrixVector(triRot.Vertices[0]);
                triRot.Vertices[1] = matRotZ.multiplyMatrixVector(triRot.Vertices[1]);
                triRot.Vertices[2] = matRotZ.multiplyMatrixVector(triRot.Vertices[2]);
                

                if (getNormal(triRot.Vertices).Z < 0)
                {
                    //2D from here--------------------------------------
                    triProj = new Triangle();
                    triProj.Vertices = new Vertex[3];
                    triProj.Vertices[0] = projection(triRot.Vertices[0]);
                    triProj.Vertices[1] = projection(triRot.Vertices[1]);
                    triProj.Vertices[2] = projection(triRot.Vertices[2]);

                    triProj.Vertices[0] = scale(triProj.Vertices[0], 300f);
                    triProj.Vertices[1] = scale(triProj.Vertices[1], 300f);
                    triProj.Vertices[2] = scale(triProj.Vertices[2], 300f);

                    triProj.Vertices[0] = TranslateToO(triProj.Vertices[0]);
                    triProj.Vertices[1] = TranslateToO(triProj.Vertices[1]);
                    triProj.Vertices[2] = TranslateToO(triProj.Vertices[2]);

                    DrawTriangle(triProj.Vertices[0].X, triProj.Vertices[0].Y,
                            triProj.Vertices[1].X, triProj.Vertices[1].Y,
                            triProj.Vertices[2].X, triProj.Vertices[2].Y);
                    
                    //g.DrawLine(Pens.Yellow, triCentr.X, triCentr.Y, normal.X, normal.Y);
                }

            }

        }

        public Vertex projection(Vertex vertice)
        {
            Vertex vertice2 = new Vertex();

            vertice2.X = vertice.X * (1 / (3 - vertice.Z));
            vertice2.Y = vertice.Y * (1 / (3 - vertice.Z));

            return vertice2;
        }

        public Vertex scale(Vertex vertice, float magnitude)
        {
            Vertex vertice2 = new Vertex();

            vertice2.X = vertice.X * magnitude;
            vertice2.Y = vertice.Y * magnitude;

            return vertice2;
        }

        public void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            g.DrawLine(Pens.Coral , x1, y1, x2, y2);
            g.DrawLine(Pens.Coral, x2, y2, x3, y3);
            g.DrawLine(Pens.Coral, x3, y3, x1, y1);
        }

        public Vertex TranslateToO(Vertex vertice)
        {
            Vertex vertice2 = new Vertex();
            vertice2.X += vertice.X + Wx;
            vertice2.Y = -vertice.Y + Hy;
            
            return vertice2;
        }

        public Vertex getCenter(Vertex[] vertices)
        {
            Vertex center = new Vertex();
            center.X = (vertices[0].X + vertices[1].X + vertices[2].X) / 3;
            center.Y = (vertices[0].Y + vertices[1].Y + vertices[2].Y) / 3;
            center.Z = (vertices[0].Z + vertices[1].Z + vertices[2].Z) / 3;

            return center;
        }

        public Vertex getNormal(Vertex[] vertices)
        {
            float n;
            Vertex normal = new Vertex();
            Vertex N = new Vertex();

            Vertex a = new Vertex();
            Vertex b = new Vertex();

            a.X = vertices[1].X - vertices[2].X;
            a.Y = vertices[1].Y - vertices[2].Y;
            a.Z = vertices[1].Z - vertices[2].Z;

            b.X = vertices[0].X - vertices[2].X;
            b.Y = vertices[0].Y - vertices[2].Y;
            b.Z = vertices[0].Z - vertices[2].Z;

            N.X = a.Y * b.Z - a.Z * b.Y;
            N.Y = a.Z * b.X - a.X * b.Z;
            N.Z = a.X * b.Y - a.Y * b.X;

            n = (float)Math.Sqrt((N.X * N.X) + (N.Y * N.Y) + (N.Z * N.Z));
            
            normal.X = N.X/n;
            normal.Y = N.Y/n;
            normal.Z = N.Z/n;

            return normal;

        }

        public Vertex getCentroid(Mesh figure)
        {
            Vertex centroid = new Vertex();
            centroid.X = 0;centroid.Y = 0;centroid.Z = 0;
            int count = 0;

            for(int i = 0; i < figure.triangles.Length; i++)
            {
                for(int j = 0; j< 3; i++)
                {
                    count++;
                    centroid.X +=figure.triangles[i].Vertices[j].X;
                    centroid.Y += figure.triangles[i].Vertices[j].Y;
                    centroid.Z += figure.triangles[i].Vertices[j].Z;
                }
            }
            centroid.X /= count;
            centroid.Y /= count;
            centroid.Z /= count;

            return centroid;
        }

        public Mesh Sphere(float radius, float segments)
        {
            Mesh Sphere = new Mesh();
            puntosSphere = new List<Vertex>();
            for(int i = 0; i<segments; i++)
            {
                float lon = map(i, 0 , segments, -pi, pi);
                for(int j = 0; j<segments; j++)
                {
                    float lat = map(j, 0, segments, -pi / 2, pi / 2);
                    Vertex vertex = new Vertex();
                    vertex.X = radius * (float)Math.Sin(lon) * (float)Math.Cos(lat);
                    vertex.Y = radius * (float)Math.Sin(lon) * (float)Math.Sin(lat);
                    vertex.Z = radius * (float)Math.Cos(lon);
                    puntosSphere.Add(vertex);
                }
            }


            return Sphere;

        }

        public float map(int segment, float start1, float end1, float start2, float end2)
        {
            float output;

            output = start2 +segment*((end2 - start2) / (end1 - start1));
            return output;
        }

        /*
        public Mesh toOrigin(Mesh figure)
        {
            Mesh mesh= new Mesh();
            mesh.triangles = new Triangle[figure.triangles.Length];
            
            Vertex vertice2 = new Vertex();
            Vertex centroid = getCentroid(figure);

            for (int i = 0; i < figure.triangles.Length; i++)
            {
                for (int j = 0; j < 3; i++)
                {
                     figure.triangles[i].Vertices[j].X - centroid.X;
                    centroid.Y += figure.triangles[i].Vertices[j].Y;
                    centroid.Z += figure.triangles[i].Vertices[j].Z;
                }
            }

            return vertice2;
        }
        */
    }
}