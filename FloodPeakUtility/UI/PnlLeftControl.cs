using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTelluro.Explorer.PluginEngine;
using System.IO;
using FloodPeakUtility.Model;
using DevComponents.AdvTree;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.Explorer.DOMImport;
using iTelluro.Explorer.VectorLoader;
using iTelluro.Explorer.DOMImport.Model;
using FloodPeakUtility;
using iTelluro.GlobeEngine.DataSource.Layer;
using iTelluro.Explorer.DOMImport.Utility;
using iTelluro.Explorer.VectorLoader.Utility;
using iTelluro.Explorer.VectorLoader.ShpSymbolModel;
using iTelluro.Explorer.VectorLoader.ShpSymbolConfig;
using iTelluro.Explorer.Vector;
using DevComponents.DotNetBar;

namespace FloodPeakUtility.UI
{
    /// <summary>
    /// Author:王冲
    /// Date:2017-09-12
    /// Function:管理程序的树节点和提供文件操作接口
    /// </summary>
    public partial class PnlLeftControl : UserControl
    {
        #region Fields

        private bool _isIni = true;  //是否为初始化项目，用于标示操作xml
        private bool _isChanged = false;  //项目内容是否改变
        private bool _isNew = false;  //项目是否为新项目

        //路径
        private string _projectPath = string.Empty;
        private string _xmlPath = string.Empty;

        //三维球
        private GlobeView _globeView = null;
        //DockUI
        private DevComponents.DotNetBar.TabControl _tabControl = null;
        //项目参数
        private ProjectModel _projectModel = null;
        //切片
        private DomLoader _domLoader = null;
        //shp文件加载
        private ShpLayerLoader _shpLoader = null;

        #endregion

        #region Attributes

        /// <summary>
        /// 获取项目内容是否改变
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
            private set
            {
                _isChanged = value;
                if (OnChanged != null)
                    OnChanged(this, new ProjectChangedEventArgs() { Chnaged = value });
            }
        }

        /// <summary>
        /// 计算界面的父容器
        /// </summary>
        public Control UIParent { get { return grpCaculate; } }

        /// <summary>
        /// 项目参数
        /// </summary>
        public ProjectModel ProjectModel
        {
            get { return _projectModel; }
            set { _projectModel = value; }
        }

        /// <summary>
        /// 项目内容改变的事件句柄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
    
        public delegate void OnChangedEventHandler(object sender, ProjectChangedEventArgs arg);
        /// <summary>
        /// 分发项目改变事件
        /// </summary>
        public event OnChangedEventHandler OnChanged;

        public delegate void OnCaculateEventHandler(object sender, CaculateEventArgs arg);
        /// <summary>
        /// 开始计算
        /// </summary>
        public event OnCaculateEventHandler OnCaculate;
     
        #endregion

        #region Ctro

        /// <summary>
        /// 构造函数
        /// </summary>
        public PnlLeftControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化三维球构造函数
        /// </summary>
        /// <param name="app">三维球</param>
        public PnlLeftControl(GlobeView app)
            : this()
        {
            _globeView = app;
            _domLoader = new DomLoader(app);
            _shpLoader = new ShpLayerLoader(app);
            //加载所有配置文件的shp图层信息
            _shpLoader.LoadConfigShpLayers();
        }

        public PnlLeftControl(GlobeView globeView, DevComponents.DotNetBar.TabControl tabControl)
            : this(globeView)
        {
            this._tabControl = tabControl;
            InitializeTabEvent();
        }

        #endregion

        #region public methods

        /// <summary>
        /// 初始化项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public void InitialzeProject(string projectPath)
        {
            //绑定日志
            LogHelper.BindLog(projectPath);
            _isIni = true;
            _projectPath = projectPath;
            InitializeConfig();
            InitializeTree();
            _isIni = false;
        }

        /// <summary>
        /// 保存项目更改
        /// </summary>
        /// <param name="save">是否保存</param>
        public void SaveProjectChanges(bool save)
        {
            //保存
            if(save)
            {
                XmlHelper.Serialize<ProjectModel>(_projectModel, _xmlPath);
                IsChanged = false;
            }
            //删除
            else if (_isNew)
            {
                Directory.Delete(Path.GetDirectoryName(_xmlPath), true);
            }
        }

        /// <summary>
        /// 公共操作节点方法
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="model"></param>
        /// <param name="oparate">0-新增，1-删除</param>
        public void OporateCaculateNode(string pid, NodeModel[] models)
        {
            Node pnode = advTreeMain.Nodes[1].Nodes.Find(pid, false).FirstOrDefault();
            if (pnode != null)
            {
                //清除节点和集合信息
                pnode.Nodes.Clear();
                var clearModels = _projectModel.Nodes.Where(t => t.PNode == pid).ToArray();
                if (clearModels != null)
                    foreach (var item in clearModels)
                    {
                        SaveModel(item, 1);
                    }
                foreach (var item in models)
                {
                    if (item != null)
                        pnode.Nodes.Add(CreateNode(item));
                }
            }
        }

        /// <summary>
        /// 显示到与三维球同级的UI
        /// </summary>
        /// <param name="dockControl"></param>
        public void ShowDock(string title, Control dockControl)
        {
            if (_tabControl == null)
                return;
            if (!this.ContainsTab(title))
            {
                TabItem tabPage = new TabItem();
                tabPage.Text = title;
                tabPage.Name = title;
                tabPage.CloseButtonVisible = false;
                dockControl.Dock = DockStyle.Fill;
                tabPage.AttachedControl = dockControl;
                _tabControl.Tabs.Add(tabPage);
            }
            _tabControl.SelectedTab = _tabControl.Tabs[title];
        }

        private bool ContainsTab(string tabName)
        {
            foreach (TabItem item in _tabControl.Tabs)
            {
                if (item.Name == tabName)
                    return true;
            }
            return false;
        }

        #endregion

        #region private methods

        /// <summary>
        /// 初始化配置文件
        /// </summary>
        private void InitializeConfig()
        {
            string forlder = Path.GetDirectoryName(_projectPath);
            string projectName = Path.GetFileNameWithoutExtension(_projectPath);
            _xmlPath = Path.Combine(forlder, projectName + ".xml");
            //存在打开已有的项目
            if (File.Exists(_xmlPath))
            {
                _isNew = false;
                _projectModel = XmlHelper.Deserialize<ProjectModel>(_xmlPath);
                //防止文件夹被移动之后失效，需要时时更新路径
                _projectModel.ProjectPath = _projectPath;
                XmlHelper.Serialize<ProjectModel>(_projectModel, _xmlPath);
            }
            //不存在创建基本项目信息
            else
            {
                _isNew = true;
                IsChanged = true;
                _projectModel = new ProjectModel();
                _projectModel.ProjectPath = _projectPath;
                _projectModel.ProjectName = projectName;

                #region 初始化节点

                List<NodeModel> lstNodes = new List<NodeModel>();
                NodeModel node = new NodeModel();
                //string id = Guid.NewGuid().ToString();
                node.NodeId = Guids.TCGL;
                node.NodeName = "图层数据";
                node.CanRemove = false;
                node.ShowCheck = false;
                node.ImageIndex = 0;
                lstNodes.Add(node);

                node = new NodeModel();
                //id = Guid.NewGuid().ToString();
                node.NodeId = Guids.JSGL;
                node.NodeName = "项目计算";
                node.CanRemove = false;
                node.ShowCheck = false;
                node.ImageIndex = 3;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                string sid = Guids.BYPL;
                node.NodeId = sid;
                node.NodeName = "暴雨频率计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.BYSJ;
                node.NodeId = sid;
                node.NodeName = "暴雨衰减参数计算";
                node.CanRemove = false;
                node.ShowCheck = false;
                node.ImageIndex = 4;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.BYSS;
                node.NodeId = sid;
                node.NodeName = "暴雨损失参数计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.HCHL;
                node.NodeId = sid;
                node.NodeName = "河槽汇流参数计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.PMHL;
                node.NodeId = sid;
                node.NodeName = "坡面汇流参数计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.QSHF;
                node.NodeId = sid;
                node.NodeName = "清水洪峰流量计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                node = new NodeModel();
                node.PNode = Guids.JSGL;
                sid = Guids.NSNHF;
                node.NodeId = sid;
                node.NodeName = "泥石流洪峰流量计算";
                node.ImageIndex = 4;
                node.CanRemove = false;
                node.ShowCheck = false;
                lstNodes.Add(node);

                #endregion

                _projectModel.Nodes = lstNodes.ToArray();
            }
        }

        /// <summary>
        /// 初始化化树控件
        /// </summary>
        private void InitializeTree()
        {
            advTreeMain.Nodes.Clear();
            var result = _projectModel.Nodes.Where(t => string.IsNullOrEmpty(t.PNode));
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    Node temp = CreateNode(item);
                    advTreeMain.Nodes.Add(temp);
                    AddNode(temp);
                }
            }
            advTreeMain.ExpandAll();
        }

        /// <summary>
        /// 初始化Tab的事件
        /// </summary>
        private void InitializeTabEvent()
        {
            //if (_tabControl == null)
            //    return;
           // _tabControl
        }

        /// <summary>
        /// 添加父节点下的所有子节点
        /// </summary>
        /// <param name="node">父节点</param>
        private void AddNode(Node node)
        {
            var result = _projectModel.Nodes.Where(t => t.PNode == node.Name);
            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    Node temp = CreateNode(item);
                    node.Nodes.Add(temp);
                    AddNode(temp);
                }
            }
        }

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <param name="model">节点模型</param>
        /// <returns></returns>
        private Node CreateNode(NodeModel model)
        {
            Node temp = new Node(model.NodeName);
            temp.Name = model.NodeId;
            temp.CheckBoxVisible = model.ShowCheck;
            temp.Tag = model;
            temp.ImageIndex = model.ImageIndex;
            return temp;
        }

        /// <summary>
        /// 保存模型
        /// </summary>
        /// <param name="model">需要操作的模型</param>
        /// <param name="oparate">操作类型：0-add,1-remove,2-update</param>
        private void SaveModel(NodeModel model, int oparate)
        {
            NodeModel[] temp = _projectModel.Nodes;
            List<NodeModel> finalNodes = new List<NodeModel>();
            finalNodes.AddRange(temp);
            if (oparate == 0)
                finalNodes.Add(model);
            else if (oparate == 1)
                finalNodes.Remove(model);
            else
            {
                NodeModel oldModel = temp.Where(t => t.NodeId == model.NodeId).FirstOrDefault();
                if (oldModel != null)
                    finalNodes.Remove(oldModel);
                finalNodes.Add(model);
            }
            _projectModel.Nodes = finalNodes.ToArray();
            IsChanged = true;
        }

        /// <summary>
        /// 获取globeview的DataLayers和FloatLayers中已经存在的图层名称
        /// </summary>
        /// <returns></returns>
        private List<string> GetExistLayerNames()
        {
            List<string> names = new List<string>();
            foreach (DataLayer lyr in _globeView.GlobeLayers.DataLayers)
            {
                names.Add(lyr.Name);
            }
            foreach (FloatLayer lyr in _globeView.GlobeLayers.FloatLayers)
            {
                names.Add(lyr.Name);
            }
            return names;
        }

        /// <summary>
        /// 根据图层名获取抽象图层
        /// </summary>
        /// <param name="lyrName">图层名</param>
        /// <returns>AbstractLayer对象</returns>
        private AbstractLayer GetLayerByName(string lyrName)
        {
            foreach (AbstractLayer lyr in _globeView.GlobeLayers.DataLayers)
            {
                if (lyr.Name == lyrName)
                {
                    return lyr;
                }
            }
            foreach (AbstractLayer lyr in _globeView.GlobeLayers.FloatLayers)
            {
                if (lyr.Name == lyrName)
                {
                    return lyr;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据图层名称来定位图层
        /// </summary>
        /// <param name="lyrName"></param>
        private void LocLyrByName(string lyrName)
        {
            foreach (DataLayer lyr in _globeView.GlobeLayers.DataLayers)
            {
                if (lyr.Name == lyrName)
                {
                    double lon = (lyr.East + lyr.West) / 2;
                    double lat = (lyr.North + lyr.South) / 2;
                    double alt = Math.Abs(lyr.East - lyr.West) / 360 * _globeView.GlobeViewSetting.EquatorialRadius * 4 * Math.PI;
                    _globeView.GlobeCamera.GotoLatLonAltitude(lat, lon, alt);
                }
            }
            foreach (FloatLayer lyr in _globeView.GlobeLayers.FloatLayers)
            {
                if (lyr.Name == lyrName)
                {
                    double lon = (lyr.Rect.East + lyr.Rect.West) / 2;
                    double lat = (lyr.Rect.North + lyr.Rect.South) / 2;
                    double alt = Math.Abs(lyr.Rect.East - lyr.Rect.West) / 360 * _globeView.GlobeViewSetting.EquatorialRadius * 4 * Math.PI;
                    _globeView.GlobeCamera.GotoLatLonAltitude(lat, lon, alt);
                }
            }
        }

        /// <summary>
        /// 是否存在点shp
        /// </summary>
        /// <param name="lyr"></param>
        /// <returns></returns>
        private bool ShpPointLyrExits(ShpPointLayer lyr)
        {
            ShpPointLayerList lst = ShpPointConfig.ReadConfigFile();
            if (lst == null || lst.Count == 0)
                return false;
            foreach (var item in lst.LayerList)
            {
                if (item.LayerName == lyr.LayerName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否存在线shp
        /// </summary>
        /// <param name="lyr"></param>
        /// <returns></returns>
        private bool ShpLineLyrExits(ShpLineLayer lyr)
        {
            ShpLineLayerList lst = ShpLineConfig.ReadConfigFile();
            if (lst == null || lst.Count == 0)
                return false;
            foreach (var item in lst.LayerList)
            {
                if (item.LayerName == lyr.LayerName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否存在面shp
        /// </summary>
        /// <param name="lyr"></param>
        /// <returns></returns>
        private bool ShpPolygonLyrExits(ShpPolygonLayer lyr)
        {
            ShpPolygonLayerList lst = ShpPolygonConfig.ReadConfigFile();
            if (lst == null || lst.Count == 0)
                return false;
            foreach (var item in lst.LayerList)
            {
                if (item.LayerName == lyr.LayerName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据名称删除三维球图层
        /// </summary>
        /// <param name="name"></param>
        private void DeleteLayerByName(string name)
        {
            foreach (DataLayer lyr in _globeView.GlobeLayers.DataLayers)
            {
                if (lyr.Name == name)
                {
                    _globeView.GlobeLayers.DataLayers.Remove(lyr);
                }
            }
            foreach (FloatLayer lyr in _globeView.GlobeLayers.FloatLayers)
            {
                if (lyr.Name == name)
                {
                    _globeView.GlobeLayers.FloatLayers.Remove(lyr);
                }
            }
        }

        #endregion

        #region events

        /// <summary>
        /// 节点选中事件，用于切换上下文ContextMenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMain_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;
            //选中图层管理
            if (e.Node.Name == Guids.TCGL)
                advTreeMain.ContextMenuStrip = ctxMain;
            else if (e.Node.Level > 0 && e.Node.Parent.Name == Guids.JSGL)
                advTreeMain.ContextMenuStrip = ctxCaculate;
            //选中可删除节点
            else if (e.Node.Level > 0 && e.Node.Parent.Name != Guids.JSGL)
                advTreeMain.ContextMenuStrip = ctxDelete;
            else
                advTreeMain.ContextMenuStrip = null;

        }

        /// <summary>
        /// 导入Dom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ctxImportDom_Click(object sender, EventArgs e)
        {
            //第一个为图层节点
            Node pnode = advTreeMain.Nodes[0];
            //开始导入Dom文件
            frmDomLyr frm = new frmDomLyr(new List<DomLayerInfo>(), null, false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DomLayerInfo lyrInfo=frm.DomLyrInfo;
          
                string name = lyrInfo.LyrName;
                string path = lyrInfo.SrcTifFilePath;
                //图层去重
                NodeModel checkModel = _projectModel.Nodes.Where(t => t.PNode == Guids.TCGL && t.NodeName == name).FirstOrDefault();
                if(checkModel!=null)
                {
                    MsgBox.ShowInfo("当前名称的图层已经存在,请重命名或者导入其他数据");
                    return;
                }

                //创建节点
                NodeModel model = new NodeModel();
                model.PNode = pnode.Name;
                model.NodeName = name;
                model.ShowCheck = false;
                model.ImageIndex = 5;
                model.NodeId = Guid.NewGuid().ToString();
                model.CanRemove = true;
                model.Path = path;
                Node node = CreateNode(model);
                pnode.Nodes.Add(node);

                //开始切片
                bgwTile.RunWorkerAsync(new object[] { lyrInfo, node });
              
            }
        }

        /// <summary>
        /// 导入Shp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ctxImportShp_Click(object sender, EventArgs e)
        { //第一个为图层节点
            Node pnode = advTreeMain.Nodes[0];
            //List<string> existlyrs = this.GetExistLayerNames();
            frmLoadVector frm = new frmLoadVector(new List<string>());
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string name = string.Empty;
                string path = string.Empty;
                switch (frm.Reader.GeoBigType)
                {
                    case GeometryBigType.Point:
                        name = frm.PntLayer.LayerName;
                        //图层去重
                        NodeModel checkModel = _projectModel.Nodes.Where(t => t.PNode == Guids.TCGL && t.NodeName == name).FirstOrDefault();
                        if (checkModel != null)
                        {
                            MsgBox.ShowInfo("当前名称的图层已经存在,请重命名或者导入其他数据");
                            return;
                        }
                        path = frm.PntLayer.ShpLayerPath;
                        ShpPointLayer pntLyr = frm.PntLayer;
                        //保存图层信息到配置文件
                        if (this.ShpPointLyrExits(pntLyr))
                        {
                            ShpPointConfig.DeleteLayer(pntLyr.LayerName);
                            _shpLoader.PntLyrList.Remove(pntLyr.LayerName);
                            this.DeleteLayerByName(pntLyr.LayerName);
                        }
                        ShpPointConfig.AppendLayer(pntLyr);
                        _shpLoader.LoadShpPointLayer(pntLyr);
                        break;
                    case GeometryBigType.Line:
                        name = frm.LineLayer.LayerName;
                        //图层去重
                        checkModel = _projectModel.Nodes.Where(t => t.PNode == Guids.TCGL && t.NodeName == name).FirstOrDefault();
                        if (checkModel != null)
                        {
                            MsgBox.ShowInfo("当前名称的图层已经存在,请重命名或者导入其他数据");
                            return;
                        }
                        path = frm.LineLayer.ShpLayerPath;
                        ShpLineLayer lineLyr = frm.LineLayer;
                        if (this.ShpLineLyrExits(lineLyr))
                        {
                            ShpLineConfig.DeleteLayer(lineLyr.LayerName);
                            _shpLoader.LineLyrList.Remove(lineLyr.LayerName);
                            this.DeleteLayerByName(lineLyr.LayerName);
                        }
                        ShpLineConfig.AppendLayer(lineLyr);
                        _shpLoader.LoadShpLineLayer(lineLyr);
                        break;
                    case GeometryBigType.Polygon:
                        name = frm.PolyLayer.LayerName;
                        //图层去重
                        checkModel = _projectModel.Nodes.Where(t => t.PNode == Guids.TCGL && t.NodeName == name).FirstOrDefault();
                        if (checkModel != null)
                        {
                            MsgBox.ShowInfo("当前名称的图层已经存在,请重命名或者导入其他数据");
                            return;
                        }
                        path = frm.PolyLayer.ShpLayerPath;
                        ShpPolygonLayer polyLyr = frm.PolyLayer;
                        if (this.ShpPolygonLyrExits(polyLyr))
                        {
                            ShpPolygonConfig.DeleteLayer(polyLyr.LayerName);
                            _shpLoader.PolyLyrList.Remove(polyLyr.LayerName);
                            this.DeleteLayerByName(polyLyr.LayerName);
                        }
                        ShpPolygonConfig.AppendLayer(polyLyr);
                        _shpLoader.LoadShpPolygonLayer(polyLyr);
                        break;
                    default:
                        break;
                }


                NodeModel model = new NodeModel();
                model.PNode = pnode.Name;
                model.NodeName = name;
                model.ShowCheck = true;
                model.ImageIndex = 2;
                model.NodeId = Guid.NewGuid().ToString();
                model.CanRemove = true;
                model.Path = path;
                model.BigType = frm.Reader.GeoBigType;
                pnode.Nodes.Add(CreateNode(model));
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctxDeleteNode_Click(object sender, EventArgs e)
        {
            Node node = advTreeMain.SelectedNode;
            try
            {
                string lyrName = node.Text;
                this.DeleteLayerByName(lyrName);
                //删除tif配置
                string path = Path.Combine(Path.GetDirectoryName(_projectPath), lyrName + ".xml");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                //删除shp配置
                else if (node.Tag is NodeModel)
                {
                    GeometryBigType geoType = ((NodeModel)node.Tag).BigType;
                    switch (geoType)
                    {
                        case GeometryBigType.Point:
                            ShpPointConfig.DeleteLayer(lyrName);
                            _shpLoader.PntLyrList.Remove(lyrName);
                            break;
                        case GeometryBigType.Line:
                            ShpLineConfig.DeleteLayer(lyrName);
                            _shpLoader.LineLyrList.Remove(lyrName);
                            break;
                        case GeometryBigType.Polygon:
                            ShpPolygonConfig.DeleteLayer(lyrName);
                            _shpLoader.PolyLyrList.Remove(lyrName);
                            break;
                        default:
                            break;
                    }
                }
                node.Remove();
            }
            catch (Exception ex)
            {
                MsgBox.ShowInfo(ex.Message);
            }
        }

        /// <summary>
        /// 节点新增时间，用于同步监控项目内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMain_AfterNodeInsert(object sender, TreeNodeCollectionEventArgs e)
        {
            if (_isIni == false)
            {
                NodeModel model = e.Node.Tag as NodeModel;
                SaveModel(model, 0);
            }
        }

        /// <summary>
        /// 节点删除事件，用于同步监控项目内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMain_AfterNodeRemove(object sender, TreeNodeCollectionEventArgs e)
        {
            if (_isIni == false)
            {
                NodeModel model = e.Node.Tag as NodeModel;
                SaveModel(model, 1);
            }
        }

        /// <summary>
        /// 节点Check事件，用于查询图层内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMain_AfterCheck(object sender, AdvTreeCellEventArgs e)
        {
            Node checkNode = e.Cell.Parent;

            AbstractLayer lyr = this.GetLayerByName(checkNode.Text);
            //影像图层加载
            if (lyr == null)
            {
                string path = Path.Combine(Path.GetDirectoryName(_projectPath), checkNode.Text + ".xml");
                if (File.Exists(path) == false)
                    return;
                DomLayerInfo lyrInfo = XmlHelper.Deserialize<DomLayerInfo>(path);
                if (lyrInfo == null)
                    return;
                lyr = LayerLoader.CreateDomLyr(lyrInfo, _globeView);
            }

            if (lyr != null)
                lyr.Visible = e.Cell.Checked;
            //飞行
            if (e.Cell.Checked)
            {
                this.LocLyrByName(checkNode.Text);
            }
        }

        /// <summary>
        /// 双击开始计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMain_NodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node node = e.Node;
            if (node != null && node.Parent != null && node.Parent.Name == Guids.JSGL && OnCaculate != null)
            {
                OnCaculate(this, new CaculateEventArgs() { CaculateId = node.Name });
            }
        }

        /// <summary>
        /// 右键计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeginCaculate_Click(object sender, EventArgs e)
        {
            Node node = advTreeMain.SelectedNode;
            if (node != null && node.Parent.Name == Guids.JSGL && OnCaculate != null)
            {
                OnCaculate(this, new CaculateEventArgs() { CaculateId = node.Name });
            }
        }

        #endregion

        #region Tif切片

        private void bgwTile_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] objs = e.Argument as object[];
                DomLayerInfo lyrInfo = objs[0] as DomLayerInfo;
                if (lyrInfo == null)
                {
                    e.Result = null;
                    return;
                }
                if (_domLoader.ClipAndPackgeTile(lyrInfo, false, true, ref e))
                {
                    e.Result = objs;
                    if (File.Exists(lyrInfo.TempTifFilePath))
                    {
                        File.Delete(lyrInfo.TempTifFilePath);
                    }

                    if (File.Exists(lyrInfo.TempTifFilePath + ".nodata"))
                    {
                        File.Delete(lyrInfo.TempTifFilePath + ".nodata");
                    }
                }
                else
                {
                    if (e.Cancel)
                    {
                        string temp = lyrInfo.TileDirPath + "_png";
                        if (Directory.Exists(temp))
                        {
                            Directory.Delete(temp, true);
                        }
                    }
                    e.Result = null;
                }
            }
            catch /*(Exception ex)*/
            {
                e.Result = null;
            }

        }

        private void bgwTile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] objs = e.Result as object[];
            DomLayerInfo lyrInfo = objs[0] as DomLayerInfo;           
            //保存切片信息
            string path = Path.Combine(Path.GetDirectoryName(_projectPath), lyrInfo.LyrName + ".xml");
            XmlHelper.Serialize<DomLayerInfo>(lyrInfo, path);
            //显示CheckBox
            Node node = objs[1] as Node;
            node.CheckBoxVisible = true;
            node.ImageIndex = 1;
            //更新模型
            NodeModel model = node.Tag as NodeModel;
            model.ImageIndex = 1;
            model.ShowCheck = true;
            SaveModel(model, 2);
        }

        #endregion
    }
}
