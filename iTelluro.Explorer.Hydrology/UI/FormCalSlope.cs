using iTelluro.DataTools.Utility.GeoAlg;
using iTelluro.DataTools.Utility.GIS;
using iTelluro.DataTools.Utility.SHP;
using iTelluro.Explorer.Raster;
using iTelluro.GlobeEngine.Analyst;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.GlobeEngine.DataSource.Geometry;
using OSGeo.OGR;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using iTelluro;

namespace FloodPeakToolUI.UI
{
    public partial class FormCalSlope : Form
    {
        private GlobeView _globeview;
        public List<iTelluro.DataTools.Utility.GIS.LineString> contourLines = new List<iTelluro.DataTools.Utility.GIS.LineString>();//等高线线集
        public List<iTelluro.DataTools.Utility.GIS.LineString> riverLines = new List<iTelluro.DataTools.Utility.GIS.LineString>();//河网数据
        public List<iTelluro.DataTools.Utility.GIS.LineString> watershedLines = new List<iTelluro.DataTools.Utility.GIS.LineString>();//流域面线集
        public List<iTelluro.DataTools.Utility.GIS.Point> points = new List<iTelluro.DataTools.Utility.GIS.Point>();//集水点点集
        public FormCalSlope(GlobeView globeview)
        {
            InitializeComponent();
            this._globeview = globeview;
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            //读取河网数据
            ShpReader shp = new ShpReader(this.textBox1.Text);
            Feature ofea;
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int id = ofea.GetFID();

                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                iTelluro.DataTools.Utility.GIS.Point[] points = new iTelluro.DataTools.Utility.GIS.Point[count];
                List<iTelluro.GlobeEngine.DataSource.Geometry.Point3d> points3d = new List<iTelluro.GlobeEngine.DataSource.Geometry.Point3d>();
                for (int i = 0; i < count; i++)
                {
                    points[i] = new iTelluro.DataTools.Utility.GIS.Point(ofea.GetGeometryRef().GetX(i), ofea.GetGeometryRef().GetY(i));
                    double lng=ofea.GetGeometryRef().GetX(i);
                    double lat=ofea.GetGeometryRef().GetY(i);
                    double z=_globeview.GetElevation(lng,lat);
                    points3d.Add(new iTelluro.GlobeEngine.DataSource.Geometry.Point3d(lng,lat,z));
                }
                iTelluro.DataTools.Utility.GIS.LineString line = new iTelluro.DataTools.Utility.GIS.LineString(points);
                riverLines.Add(line);
            }

            //读取集水点数据
            shp = new ShpReader(this.textBox3.Text);
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int id = ofea.GetFID();
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                for (int i = 0; i < count; i++)
                {
                    points.Add(new iTelluro.DataTools.Utility.GIS.Point(ofea.GetGeometryRef().GetX(i), ofea.GetGeometryRef().GetY(i)));
                }
            }

            //判断集水点是否在河网上
             List<iTelluro.GlobeEngine.DataSource.Geometry.Point3d> jPoints = new List<iTelluro.GlobeEngine.DataSource.Geometry.Point3d>();
            for (int j = 0; j < points.Count; j++)
            {
                for (int i = 0; i < riverLines.Count; i++)
                {
                    iTelluro.DataTools.Utility.GIS.Point sPoint = riverLines[i].StartPoint;
                    iTelluro.DataTools.Utility.GIS.Point ePoint = riverLines[i].EndPoint;
                    if (iTelluro.DataTools.Utility.GeoAlg.PointLine.IsLineContainPoint(points[j].X, points[j].Y, sPoint.X,sPoint.Y ,ePoint.X,ePoint.Y))
                    {
                        jPoints.Add(new iTelluro.GlobeEngine.DataSource.Geometry.Point3d(points[j].X,points[j].Y,_globeview.GetElevation(points[j].X,points[j].Y)));
                    }
                }
            }

            //流域面数据读取
            shp = new ShpReader(this.textBox2.Text);
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int id = ofea.GetFID();
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                iTelluro.DataTools.Utility.GIS.Point[] polyPoints = new iTelluro.DataTools.Utility.GIS.Point[count];
                for (int i = 0; i < count; i++)
                {
                    polyPoints[i] = new iTelluro.DataTools.Utility.GIS.Point(ofea.GetGeometryRef().GetX(i), ofea.GetGeometryRef().GetY(i));
                }
            }
        }

        private void btnAddContour_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "shp文件|*.shp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = ofd.FileName;
            }
        }

        private void btnAddFlow_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "shp文件|*.shp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "shp文件|*.shp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = ofd.FileName;
            }
        }

    }
}
