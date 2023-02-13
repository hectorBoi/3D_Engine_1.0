using System.Numerics;

namespace _3D_Engine_1._0
{
    public partial class Form1 : Form
    {
        static Bitmap bmp;
        static Graphics g;
        struct Point3D
        {
            float X, Y, Z;
        }

        struct Triangle
        {
            Point3D[] Vertices;
        }

        struct Mesh
        {
            Vector<Triangle> triangles;
        }
        
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(PCT_CANVAS.Width, PCT_CANVAS.Height);
            g = Graphics.FromImage(bmp);
            PCT_CANVAS.Image = bmp;
        }

    }
}