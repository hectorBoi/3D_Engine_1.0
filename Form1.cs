using System;
using System.Drawing.Text;
using System.Numerics;
using System.Security.Cryptography.Xml;
using static _3D_Engine_1._0.Form1;

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
        Vertex[,] spherePoints;

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
            //DrawSphere(Sphere(1, 25));
            //DrawIcosahedron(Icosahedron(1f));
            DrawCone(Cone(0.6f, 2, 50));
            //DrawShape(Sphere(1, 25));
            //DrawShape(meshCube);
            //DrawShape(Icosahedron(1));

            PCT_CANVAS.Invalidate();

        }

        public void DrawShape(Mesh shape)
        {
            for(int i = 0; i< shape.triangles.Length; i++)
            {
                Triangle triangle = shape.triangles[i];
                CalculateTriangle(triangle);
            }
        }

        public void DrawCone(Mesh cone)
        {
            for(int i = 0; i<cone.triangles.Length; i++)
            {
                Triangle triangle = cone.triangles[i];
                CalculateTriangle(triangle);
            }
        }
        public void DrawSphere(Mesh sphere)
        {
            for(int i = 0; i<sphere.triangles.Length; i++)//sphere.triangles.Lenght
            {
                Triangle triangle = new Triangle();
                triangle = sphere.triangles[i];
                CalculateTriangle(triangle);
            }

        }

        public void DrawIcosahedron(Mesh icosahedron)
        {
            for(int i = 0; i< icosahedron.triangles.Length; i++)
            {
                Triangle triangle = new Triangle();
                triangle = icosahedron.triangles[i];
                CalculateTriangle(triangle);
            }
        }
        public void DrawSquare()
        {
            for(int i = 0; i<meshCube.triangles.Length;i++)
            {
                Triangle triangle = new Triangle();
                triangle = meshCube.triangles[i];
                CalculateTriangle(triangle);

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
            g.DrawLine(Pens.Cyan , x1, y1, x2, y2);
            g.DrawLine(Pens.Cyan, x3, y3, x1, y1);
            g.DrawLine(Pens.Cyan, x2, y2, x3, y3);
        }

        public void CalculateTriangle(Triangle triangle)
        {

            Triangle triProj, triRot;
            
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

            }
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

        private void PCT_CANVAS_Click(object sender, EventArgs e)
        {

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


        private Mesh Icosahedron(float sideLenght)
        {
            Mesh Icosahedron = new Mesh();

            List<Triangle> icosTraingles = IcosahedronTriangles(sideLenght);
            Icosahedron.triangles = new Triangle[icosTraingles.Count];
            for(int i = 0; i < Icosahedron.triangles.Length; i++)
            {
                Icosahedron.triangles[i] = icosTraingles[i];
            }
            
            return Icosahedron;
        }
        public List<Triangle> IcosahedronTriangles(float sideLenght)
        {
            Vertex[] points = IcosahedronVertices(sideLenght).ToArray();

            // Make the triangles.
            List<Triangle> triangles = new List<Triangle>();
            List <Triangle> triangles2 = new List<Triangle>();

            Triangle tri1 = new Triangle();
            tri1.Vertices = new Vertex[3];
            tri1.Vertices[0] = points[0];
            tri1.Vertices[1] = points[2];
            tri1.Vertices[2] = points[1];
            triangles.Add(tri1);

            Triangle tri2 = new Triangle();
            tri2.Vertices = new Vertex[3];
            tri2.Vertices[0] = points[0];
            tri2.Vertices[1] = points[3];
            tri2.Vertices[2] = points[2];
            triangles.Add(tri2);

            Triangle tri3 = new Triangle();
            tri3.Vertices = new Vertex[3];
            tri3.Vertices[0] = points[0];
            tri3.Vertices[1] = points[4];
            tri3.Vertices[2] = points[3];
            triangles.Add(tri3);

            Triangle tri4 = new Triangle();
            tri4.Vertices = new Vertex[3];
            tri4.Vertices[0] = points[0];
            tri4.Vertices[1] = points[5];
            tri4.Vertices[2] = points[4];
            triangles.Add(tri4);

            Triangle tri5 = new Triangle();
            tri5.Vertices = new Vertex[3];
            tri5.Vertices[0] = points[0];
            tri5.Vertices[1] = points[1];
            tri5.Vertices[2] = points[5];
            triangles.Add(tri5);

            Triangle tri6 = new Triangle();
            tri6.Vertices = new Vertex[3];
            tri6.Vertices[0] = points[1];
            tri6.Vertices[1] = points[2];
            tri6.Vertices[2] = points[9];
            triangles.Add(tri6);

            Triangle tri7 = new Triangle();
            tri7.Vertices = new Vertex[3];
            tri7.Vertices[0] = points[2];
            tri7.Vertices[1] = points[3];
            tri7.Vertices[2] = points[10];
            triangles.Add(tri7);

            Triangle tri8 = new Triangle();
            tri8.Vertices = new Vertex[3];
            tri8.Vertices[0] = points[3];
            tri8.Vertices[1] = points[4];
            tri8.Vertices[2] = points[6];
            triangles.Add(tri8);

            Triangle tri9 = new Triangle();
            tri9.Vertices = new Vertex[3];
            tri9.Vertices[0] = points[4];
            tri9.Vertices[1] = points[5];
            tri9.Vertices[2] = points[7];
            triangles.Add(tri9);

            Triangle tri10 = new Triangle();
            tri10.Vertices = new Vertex[3];
            tri10.Vertices[0] = points[5];
            tri10.Vertices[1] = points[1];
            tri10.Vertices[2] = points[8];
            triangles.Add(tri10);

            Triangle tri11 = new Triangle();
            tri11.Vertices = new Vertex[3];
            tri11.Vertices[0] = points[6];
            tri11.Vertices[1] = points[4];
            tri11.Vertices[2] = points[7];
            triangles.Add(tri11);

            Triangle tri12 = new Triangle();
            tri12.Vertices = new Vertex[3];
            tri12.Vertices[0] = points[7];
            tri12.Vertices[1] = points[5];
            tri12.Vertices[2] = points[8];
            triangles.Add(tri12);

            Triangle tri13 = new Triangle();
            tri13.Vertices = new Vertex[3];
            tri13.Vertices[0] = points[8];
            tri13.Vertices[1] = points[1];
            tri13.Vertices[2] = points[9];
            triangles.Add(tri13);

            Triangle tri14 = new Triangle();
            tri14.Vertices = new Vertex[3];
            tri14.Vertices[0] = points[9];
            tri14.Vertices[1] = points[2];
            tri14.Vertices[2] = points[10];
            triangles.Add(tri14);

            Triangle tri15 = new Triangle();
            tri15.Vertices = new Vertex[3];
            tri15.Vertices[0] = points[10];
            tri15.Vertices[1] = points[3];
            tri15.Vertices[2] = points[6];
            triangles.Add(tri15);

            Triangle tri16 = new Triangle();
            tri16.Vertices = new Vertex[3];
            tri16.Vertices[0] = points[11];
            tri16.Vertices[1] = points[6];
            tri16.Vertices[2] = points[7];
            triangles.Add(tri16);

            Triangle tri17 = new Triangle();
            tri17.Vertices = new Vertex[3];
            tri17.Vertices[0] = points[11];
            tri17.Vertices[1] = points[7];
            tri17.Vertices[2] = points[8];
            triangles.Add(tri17);

            Triangle tri18 = new Triangle();
            tri18.Vertices = new Vertex[3];
            tri18.Vertices[0] = points[11];
            tri18.Vertices[1] = points[8];
            tri18.Vertices[2] = points[9];
            triangles.Add(tri18);

            Triangle tri19 = new Triangle();
            tri19.Vertices = new Vertex[3];
            tri19.Vertices[0] = points[11];
            tri19.Vertices[1] = points[9];
            tri19.Vertices[2] = points[10];
            triangles.Add(tri19);

            Triangle tri20 = new Triangle();
            tri20.Vertices = new Vertex[3];
            tri20.Vertices[0] = points[11];
            tri20.Vertices[1] = points[10];
            tri20.Vertices[2] = points[6];
            triangles.Add(tri20);

            triangles2 = subdivideTriangles(triangles);

            return triangles2;
        }
        public List<Vertex> IcosahedronVertices(float sideLength)
        {
            List<Vertex> icosahedronPoints = new List<Vertex>();
            float S = sideLength;
            float t2 = pi / 10.0f;
            float t4 = pi / 5.0f;
            float R = (S / 2.0f) / (float)Math.Sin(t4);
            float H = (float)Math.Cos(t4) * R;
            float Cx = R * (float)Math.Sin(t2);
            float Cz = R * (float)Math.Cos(t2);
            float H1 = (float)Math.Sqrt(S * S - R * R);
            float H2 = (float)Math.Sqrt((H + R) * (H + R) - H * H);
            float Y2 = (H2 - H1) / 2.0f;
            float Y1 = Y2 + H1;

            icosahedronPoints.Add(new Vertex() { X = 0, Y = Y1, Z = 0});
            icosahedronPoints.Add(new Vertex() { X = R, Y = Y2, Z = 0 });
            icosahedronPoints.Add(new Vertex() { X = Cx, Y = Y2, Z = Cz });
            icosahedronPoints.Add(new Vertex() { X = -H, Y = Y2, Z = S / 2 });
            icosahedronPoints.Add(new Vertex() { X = -H, Y = Y2, Z = -S / 2 });
            icosahedronPoints.Add(new Vertex() { X = Cx, Y = Y2, Z = -Cz });
            icosahedronPoints.Add(new Vertex() { X = -R, Y = -Y2, Z = 0 });
            icosahedronPoints.Add(new Vertex() { X = -Cx, Y = -Y2, Z = -Cz });
            icosahedronPoints.Add(new Vertex() { X = H, Y = -Y2, Z = -S / 2 });
            icosahedronPoints.Add(new Vertex() { X = H, Y = -Y2, Z = S / 2 });
            icosahedronPoints.Add(new Vertex() { X = -Cx, Y = -Y2, Z = Cz });
            icosahedronPoints.Add(new Vertex() { X = 0, Y = -Y1, Z = 0 });
            return icosahedronPoints;
        }

        public Mesh Sphere(float radius, int segments)
        {
            Mesh Sphere = new Mesh();

            List<Triangle>sphereTriangles = new List<Triangle>();
            spherePoints = new Vertex[segments+1, segments+1];

            for(int i = 0; i<segments+1; i++)
            {
                float lat = map(i, 0 , segments, 0, pi);
                for(int j = 0; j<segments+1; j++)
                {
                    float lon = map(j, 0, segments, 0, pi*2);
                    Vertex vertex = new Vertex();
                    vertex.X = radius * (float)Math.Sin(lat) * (float)Math.Cos(lon);
                    vertex.Y = radius * (float)Math.Sin(lat) * (float)Math.Sin(lon);
                    vertex.Z = radius * (float)Math.Cos(lat);
                    spherePoints[i, j] = vertex;
                }
            }
            for(int i = 0; i<segments; i++)
            {
                for(int j = 0; j<segments; j++)
                {
                    Triangle tri2 = new Triangle();
                    if (i == segments-1)
                    {
                        Triangle tri = new Triangle();
                        tri.Vertices = new Vertex[3];
                        tri.Vertices[0] = spherePoints[i, j + 1];
                        tri.Vertices[1] = spherePoints[i, j];
                        tri.Vertices[2] = spherePoints[i + 1, j];
                        sphereTriangles.Add(tri);
                    }
                    else
                    {
                        
                        tri2.Vertices = new Vertex[3];
                        tri2.Vertices[0] = spherePoints[i, j + 1];
                        tri2.Vertices[1] = spherePoints[i + 1, j];
                        tri2.Vertices[2] = spherePoints[i + 1, j + 1];
                        sphereTriangles.Add(tri2);

                        tri2.Vertices = new Vertex[3];
                        tri2.Vertices[0] = spherePoints[i, j + 1];
                        tri2.Vertices[1] = spherePoints[i + 1, j];
                        tri2.Vertices[2] = spherePoints[i + 1, j + 1];
                        sphereTriangles.Add(tri2);

                    }
                }
            }
            Sphere.triangles = new Triangle[sphereTriangles.Count];
            for(int i = 0; i< sphereTriangles.Count; i++)
            {
                Sphere.triangles[i] = sphereTriangles[i];
            }

            return Sphere;

        }

        public List<Triangle> subdivideTriangles(List<Triangle> oldTriangles)
        {
            float radius = oldTriangles[0].Vertices[0].Y;
            List <Triangle> newTriangles= new List<Triangle>();
            for(int i = 0; i < oldTriangles.Count; i++)
            {
                Vertex v01 = new Vertex() {X = oldTriangles[i].Vertices[1].X- oldTriangles[i].Vertices[0].X, 
                    Y = oldTriangles[i].Vertices[1].Y - oldTriangles[i].Vertices[0].Y, 
                    Z = oldTriangles[i].Vertices[1].Z - oldTriangles[i].Vertices[0].Z
                };

                Vertex v02 = new Vertex() { X = oldTriangles[i].Vertices[2].X - oldTriangles[i].Vertices[0].X, 
                    Y = oldTriangles[i].Vertices[2].Y - oldTriangles[i].Vertices[0].Y, 
                    Z = oldTriangles[i].Vertices[2].Z - oldTriangles[i].Vertices[0].Z
                };
                Vertex v12 = new Vertex() { X = oldTriangles[i].Vertices[2].X - oldTriangles[i].Vertices[1].X, 
                    Y = oldTriangles[i].Vertices[2].Y - oldTriangles[i].Vertices[1].Y, 
                    Z = oldTriangles[i].Vertices[2].Z - oldTriangles[i].Vertices[1].Z
                };

                Vertex A = new Vertex();
                A.X = oldTriangles[i].Vertices[0].X + v01.X * 1.0f / 3.0f;
                A.Y = oldTriangles[i].Vertices[0].Y + v01.Y * 1.0f / 3.0f;
                A.Z = oldTriangles[i].Vertices[0].Z + v01.Z * 1.0f / 3.0f;

                Vertex B = new Vertex();
                B.X = oldTriangles[i].Vertices[0].X + v02.X * 1.0f / 3.0f;
                B.Y = oldTriangles[i].Vertices[0].Y + v02.Y * 1.0f / 3.0f;
                B.Z = oldTriangles[i].Vertices[0].Z + v02.Z * 1.0f / 3.0f;

                Vertex C = new Vertex();
                C.X = oldTriangles[i].Vertices[0].X + v01.X * 2.0f / 3.0f;
                C.Y = oldTriangles[i].Vertices[0].Y + v01.Y * 2.0f / 3.0f;
                C.Z = oldTriangles[i].Vertices[0].Z + v01.Z * 2.0f / 3.0f;

                Vertex D = new Vertex();
                D.X = oldTriangles[i].Vertices[0].X + v01.X * 2.0f / 3.0f + v12.X * 1.0f / 3.0f;
                D.Y = oldTriangles[i].Vertices[0].Y + v01.Y * 2.0f / 3.0f + v12.Y * 1.0f / 3.0f;
                D.Z = oldTriangles[i].Vertices[0].Z + v01.Z * 2.0f / 3.0f + v12.Z * 1.0f / 3.0f;

                Vertex E = new Vertex();
                E.X = oldTriangles[i].Vertices[0].X + v02.X * 2.0f / 3.0f;
                E.Y = oldTriangles[i].Vertices[0].Y + v02.Y * 2.0f / 3.0f;
                E.Z = oldTriangles[i].Vertices[0].Z + v02.Z * 2.0f / 3.0f;

                Vertex F = new Vertex();
                F.X = oldTriangles[i].Vertices[1].X + v12.X * 1.0f / 3.0f;
                F.Y = oldTriangles[i].Vertices[1].Y + v12.Y * 1.0f / 3.0f;
                F.Z = oldTriangles[i].Vertices[1].Z + v12.Z * 1.0f / 3.0f;

                Vertex G = new Vertex();
                G.X = oldTriangles[i].Vertices[1].X + v12.X * 2.0f / 3.0f;
                G.Y = oldTriangles[i].Vertices[1].Y + v12.Y * 2.0f / 3.0f;
                G.Z = oldTriangles[i].Vertices[1].Z + v12.Z * 2.0f / 3.0f;

                Vertex Center = new Vertex() { X = 0, Y = 0, Z = 0};
                NormalizePoint(ref A, Center, radius);
                NormalizePoint(ref B, Center, radius);
                NormalizePoint(ref C, Center, radius);
                NormalizePoint(ref D, Center, radius);
                NormalizePoint(ref E, Center, radius);
                NormalizePoint(ref F, Center, radius);
                NormalizePoint(ref G, Center, radius);

                Triangle newTriangle1 = new Triangle();
                newTriangle1.Vertices = new Vertex[3];
                newTriangle1.Vertices[0] = oldTriangles[i].Vertices[0];
                newTriangle1.Vertices[1] = A;
                newTriangle1.Vertices[2] = B;
                newTriangles.Add(newTriangle1);

                Triangle newTriangle2 = new Triangle();
                newTriangle2.Vertices = new Vertex[3];
                newTriangle2.Vertices[0] = A;
                newTriangle2.Vertices[1] = C;
                newTriangle2.Vertices[2] = D;
                newTriangles.Add(newTriangle2);

                Triangle newTriangle3 = new Triangle();
                newTriangle3.Vertices = new Vertex[3];
                newTriangle3.Vertices[0] = A;
                newTriangle3.Vertices[1] = D;
                newTriangle3.Vertices[2] = B;
                newTriangles.Add(newTriangle3);

                Triangle newTriangle4 = new Triangle();
                newTriangle4.Vertices = new Vertex[3];
                newTriangle4.Vertices[0] = B;
                newTriangle4.Vertices[1] = D;
                newTriangle4.Vertices[2] = E;
                newTriangles.Add(newTriangle4);

                Triangle newTriangle5 = new Triangle();
                newTriangle5.Vertices = new Vertex[3];
                newTriangle5.Vertices[0] = C;
                newTriangle5.Vertices[1] = oldTriangles[i].Vertices[1];
                newTriangle5.Vertices[2] = F;
                newTriangles.Add(newTriangle5);

                Triangle newTriangle6 = new Triangle();
                newTriangle6.Vertices = new Vertex[3];
                newTriangle6.Vertices[0] = C;
                newTriangle6.Vertices[1] = F;
                newTriangle6.Vertices[2] = D;
                newTriangles.Add(newTriangle6);

                Triangle newTriangle7 = new Triangle();
                newTriangle7.Vertices = new Vertex[3];
                newTriangle7.Vertices[0] = D;
                newTriangle7.Vertices[1] = F;
                newTriangle7.Vertices[2] = G;
                newTriangles.Add(newTriangle7);

                Triangle newTriangle8 = new Triangle();
                newTriangle8.Vertices = new Vertex[3];
                newTriangle8.Vertices[0] = D;
                newTriangle8.Vertices[1] = G;
                newTriangle8.Vertices[2] = E;
                newTriangles.Add(newTriangle8);

                Triangle newTriangle9 = new Triangle();
                newTriangle9.Vertices = new Vertex[3];
                newTriangle9.Vertices[0] = E;
                newTriangle9.Vertices[1] = G;
                newTriangle9.Vertices[2] = oldTriangles[i].Vertices[2];
                newTriangles.Add(newTriangle9);
            }

            return newTriangles;
            void NormalizePoint(ref Vertex point, Vertex center, float distance)
            {
                Vertex vertex = new Vertex() {X = point.X-center.X, Y = point.Y-center.Y, Z = point.Z-center.Z};
                float lenght = (float)Math.Sqrt(vertex.X*vertex.X+vertex.Y*vertex.Y+vertex.Z*vertex.Z);
                point.X = center.X + vertex.X / lenght * distance;
                point.Y = center.Y + vertex.Y / lenght * distance;
                point.Z = center.Z + vertex.Z / lenght * distance;
            }

        }

        public Mesh Cone(float radius, float height, int segments)
        {
            Mesh Cone = new Mesh();
            Vertex[] circlePoints = new Vertex[segments+1];
            for(int i = 0; i< segments+1; i++)
            {
                float lon = map(i, 0, segments, 0, pi * 2);
                Vertex vertex = new Vertex();
                vertex.Z = radius * (float)Math.Cos(lon);
                vertex.X = radius * (float)Math.Sin(lon);
                vertex.Y =- height / 2;
                circlePoints[i] = vertex;
            }

            List<Triangle> baseTriangles = new List<Triangle>();
            Vertex top = new Vertex() { X = 0, Y = height/2 , Z= 0 };
            for(int i = 0; i<segments; i++)
            {
                Triangle baseTri = new Triangle();
                baseTri.Vertices = new Vertex[3];
                baseTri.Vertices[0] = circlePoints[i];
                baseTri.Vertices[1] = new Vertex() {X = 0, Y = -height/2, Z =0};
                baseTri.Vertices[2] = circlePoints[i+1];

                baseTriangles.Add(baseTri);

                Triangle topTri = new Triangle();
                topTri.Vertices = new Vertex[3];
                topTri.Vertices[0] = circlePoints[i+1];
                topTri.Vertices[1] = top;
                topTri.Vertices[2] = circlePoints[i];
                baseTriangles.Add(topTri);
            }
            Cone.triangles = new Triangle[baseTriangles.Count];
            for (int i = 0; i < baseTriangles.Count; i++)
            {
                Cone.triangles[i] = baseTriangles[i];
            }

            return Cone;
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