using System.Numerics;

namespace _3D_Engine_1._0
{
    public partial class Form1 : Form
    {
        static Bitmap bmp;
        static Graphics g;
        float Wx, Hy, angle;
        Point3D centroid;
        Vertex v1, v2, v3, v4;
        Matrix2D matProj;
        Mesh meshCube;

        


        public struct Point3D
        {
            public float X, Y, Z;
        }

        struct Triangle
        {
            public Vertex[] Vertices;
        }

        struct Mesh
        {
            public Triangle[] triangles;
        }
        
        Point3D a, b, c, d = new Point3D();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (angle > 360)
                angle = 360;

            if (angle < -360)
                angle = 0;
            angle++;
            /*
            p1 = new PointF(p1.X - centroid2.X, p1.Y - centroid2.Y);
            p2 = new PointF(p2.X - centroid2.X, p2.Y - centroid2.Y);
            p3 = new PointF(p3.X - centroid2.X, p3.Y - centroid2.Y);
            p4 = new PointF(p4.X - centroid2.X, p4.Y - centroid2.Y);

            angle = (float)1.5708;
            p1 = rotation(p1, angle);
            p2 = rotation(p2, angle);
            p3 = rotation(p3, angle);
            p4 = rotation(p4, angle);

            p1 = new PointF(p1.X + centroid2.X, p1.Y + centroid2.Y);
            p2 = new PointF(p2.X + centroid2.X, p2.Y + centroid2.Y);
            p3 = new PointF(p3.X + centroid2.X, p3.Y + centroid2.Y);
            p4 = new PointF(p4.X + centroid2.X, p4.Y + centroid2.Y);
            */


            Render();
        }

        PointF p1, p2, p3, p4, centroid2 = new PointF();

        
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(PCT_CANVAS.Width, PCT_CANVAS.Height);
            Wx = bmp.Width/2;
            Hy = bmp.Height/2;
            matProj = new Matrix2D();

            g = Graphics.FromImage(bmp);
            PCT_CANVAS.Image = bmp;
            a.X = 0;
            a.Y = 0;
            a.Z = 0;

            b.X = 0;
            b.Y = 100;
            b.Z = 0;

            c.X = 100;
            c.Y = 100;

            d.X = 100;
            d.Y = 0;

            centroid.X = 50;
            centroid.Y = 50;

            centroid2 = normalize(centroid);

            p1 = normalize(a);
            p2 = normalize(b);
            p3 = normalize(c);
            p4 = normalize(d);

            Init();

            //vCentroid = new Veretex(new float[] {})
        }

        private void Init()
        {
            //Projection Matrix
            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio = bmp.Height / bmp.Width;
            float fFovRad = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);

            matProj.matrix[0, 0] = fAspectRatio * fFovRad;
            matProj.matrix[1, 1] = fFovRad;
            matProj.matrix[2, 2] = fFar / (fFar - fNear);
            matProj.matrix[3, 2] = (-fFar * fNear) / (fFar - fNear);
            matProj.matrix[2, 3] = 1.0f;
            matProj.matrix[3, 3] = 0.0f;

            //Cube Declaration
            Vertex v1 = new Vertex() { X = 0.0f, Y = 0.0f, Z = 0.0f };
            Vertex v2 = new Vertex() { X = 0.0f, Y = 1.0f, Z = 0.0f };
            Vertex v3 = new Vertex() { X = 1.0f, Y = 1.0f, Z = 0.0f };
            Vertex v4 = new Vertex() { X = 1.0f, Y = 0.0f, Z = 0.0f };
            Vertex v5 = new Vertex() { X = 1.0f, Y = 1.0f, Z = 1.0f };
            Vertex v6 = new Vertex() { X = 1.0f, Y = 0.0f, Z = 1.0f };
            Vertex v7 = new Vertex() { X = 0.0f, Y = 1.0f, Z = 1.0f };
            Vertex v8 = new Vertex() { X = 0.0f, Y = 0.0f, Z = 1.0f };

            Triangle t1  = new Triangle() { Vertices = new Vertex[] { v1, v2, v3 } };
            Triangle t2  = new Triangle() { Vertices = new Vertex[] { v1, v3, v4 } };
            Triangle t3  = new Triangle() { Vertices = new Vertex[] { v4, v3, v5 } };
            Triangle t4  = new Triangle() { Vertices = new Vertex[] { v4, v5, v6 } };
            Triangle t5  = new Triangle() { Vertices = new Vertex[] { v6, v5, v7 } };
            Triangle t6  = new Triangle() { Vertices = new Vertex[] { v6, v7, v8 } };
            Triangle t7  = new Triangle() { Vertices = new Vertex[] { v8, v7, v2 } };
            Triangle t8  = new Triangle() { Vertices = new Vertex[] { v8, v2, v1 } };
            Triangle t9  = new Triangle() { Vertices = new Vertex[] { v2, v7, v5 } };
            Triangle t10 = new Triangle() { Vertices = new Vertex[] { v2, v5, v3 } };
            Triangle t11 = new Triangle() { Vertices = new Vertex[] { v8, v1, v4 } };
            Triangle t12 = new Triangle() { Vertices = new Vertex[] { v8, v6, v4 } };

            meshCube = new Mesh() {triangles = new Triangle[] {t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12} };
        }

        public PointF normalize(Point3D point)
        {
            PointF point2 = new PointF(point.X+Wx, -point.Y+Hy);
            return point2;
        }


        public PointF rotation(PointF point, float angle)
        {
            if (angle > 360)
                angle = 360;

            if (angle < -360)
                angle = 0;

            PointF point2 = new PointF();
            angle = angle / 57.2958f;

            point2.X = (float)((point.X * Math.Cos(angle)) - (point.Y * Math.Sin(angle)));
            point2.Y = (float)((point.X * Math.Sin(angle)) + (point.Y * Math.Cos(angle)));
            return point2;
        }

        public void Render()
        {
            g.Clear(Color.Black);
            /*
            g.DrawLine(Pens.Magenta, p1, p2);
            g.DrawLine(Pens.Magenta, p2, p3);
            g.DrawLine(Pens.Magenta, p3, p4);
            g.DrawLine(Pens.Magenta, p4, p1);
            */
            DrawSquare();
            PCT_CANVAS.Invalidate();
        }

        public void DrawSquare()
        {
            foreach(Triangle triangle in meshCube.triangles)
            {
                Triangle triProj;
                triProj = new Triangle();
                triProj.Vertices = new Vertex[3];
                triProj.Vertices[0] = matProj.multiplyMatrixVector(triangle.Vertices[0]);
                triProj.Vertices[1] = matProj.multiplyMatrixVector(triangle.Vertices[1]);
                triProj.Vertices[2] = matProj.multiplyMatrixVector(triangle.Vertices[2]);

                //Scale to view
                triProj.Vertices[0].X += 1.0f; triProj.Vertices[0].Y += 1.0f;
                triProj.Vertices[1].X += 1.0f; triProj.Vertices[1].Y += 1.0f;
                triProj.Vertices[2].X += 1.0f; triProj.Vertices[2].Y += 1.0f;

                triProj.Vertices[0].X *= 0.5f * (float)PCT_CANVAS.Width;
                triProj.Vertices[0].Y *= 0.5f * (float)PCT_CANVAS.Height;
                triProj.Vertices[1].X *= 0.5f * (float)PCT_CANVAS.Width;
                triProj.Vertices[1].Y *= 0.5f * (float)PCT_CANVAS.Height;
                triProj.Vertices[2].X *= 0.5f * (float)PCT_CANVAS.Width;
                triProj.Vertices[2].Y *= 0.5f * (float)PCT_CANVAS.Height;

                DrawTriangle(triProj.Vertices[0].X, triProj.Vertices[0].Y,
                     triProj.Vertices[1].X, triProj.Vertices[1].Y,
                     triProj.Vertices[2].X, triProj.Vertices[2].Y);
            }


        }

        public void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            g.DrawLine(Pens.Coral , x1, y1, x2, y2);
            g.DrawLine(Pens.Coral, x2, y2, x3, y3);
            g.DrawLine(Pens.Coral, x3, y3, x1, y1);
        }

    }
}