using iTelluro.Explorer.DOMImport.Model;
using iTelluro.GlobeEngine.DataSource.Layer;
using iTelluro.GlobeEngine.MapControl3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility
{
    public class LayerLoader
    {
        /// <summary>
        /// 加载tif到三维视图
        /// </summary>
        /// <param name="name">图层名称</param>
        /// <param name="Categorytype">出图类型</param>
        /// <param name="tifFilePath">tif文件路径</param>
        /// <param name="globe">三维球实例</param>
        /// <returns>返回生成的图层</returns>
        public static void LoadTifToMap(DomLayerInfo layerInfo, GlobeView globe)
        {
            LonLatDataLayer lyr = (LonLatDataLayer)globe.GlobeLayers.DataLayers.FindLayer(layerInfo.LyrName);
            if (lyr == null)
            {
                LonLatDataLayer.LonLatDataLayerDescriptor des =
        new LonLatDataLayer.LonLatDataLayerDescriptor();

                des.Category = layerInfo.Category;
                des.DataName = layerInfo.LyrName;
                des.FileExtension = "png";
                des.LayerName = layerInfo.LyrName;
                des.LevelZeroTileSizeDegrees = 2.25;
                des.LocalPath = layerInfo.TileDirPath;
                des.NumLevels = layerInfo.LevelNum;
                des.Opacity = 200;
                des.Url = "http://localhost";
                des.Visible = true;
                des.East = layerInfo.East;
                des.West = layerInfo.West;
                des.North = layerInfo.North;
                des.South = layerInfo.South;
                lyr = LonLatDataLayer.LoadDataLayer(des);
                globe.GlobeLayers.DataLayers.Add(lyr);
            }
            lyr.ResetCache();
            lyr.Visible = false;
        }

        public static LonLatDataLayer CreateDomLyr(DomLayerInfo lyrInfo, GlobeView globe)
        {
            if (lyrInfo == null)
            {
                return null;
            }

            if (!File.Exists(lyrInfo.SrcTifFilePath)
                && !File.Exists(lyrInfo.TileDirPath)//mbt
                && !Directory.Exists(lyrInfo.TileDirPath))//切片目录
            {
                return null;
            }

            DataLayer tmp = globe.GlobeLayers.DataLayers.FindLayer(lyrInfo.LyrName);
            if (tmp != null)
            {
                MsgBox.ShowError("同名图层已存在,请尝试点击刷新按钮或修改图层名！");
                return null;
            }

            LonLatDataLayer.LonLatDataLayerDescriptor des = new LonLatDataLayer.LonLatDataLayerDescriptor();
            if (lyrInfo.Category.Trim() == string.Empty)
            {
                des.Category = "本地影像数据";
            }
            else
            {
                des.Category = "本地影像数据\\" + lyrInfo.Category;
            }
            des.DataName = lyrInfo.LyrName;
            des.FileExtension = "png";
            des.LayerName = lyrInfo.LyrName;
            des.LevelZeroTileSizeDegrees = lyrInfo.BeginLevelSize;
            des.LocalPath = lyrInfo.TileDirPath;
            des.NumLevels = lyrInfo.LevelNum;
            des.Opacity = 255;
            //des.Url = "http://localhost";
            des.Visible = true;
            des.East = lyrInfo.East;
            des.West = lyrInfo.West;
            des.North = lyrInfo.North;
            des.South = lyrInfo.South;

            LonLatDataLayer dataLyr = LonLatDataLayer.LoadDataLayer(des);
            globe.GlobeLayers.DataLayers.Add(dataLyr);
            //dataLyr.ResetCache();
            return dataLyr;
        }

    }
}
