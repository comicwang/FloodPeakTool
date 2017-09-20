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
        private TabControl _tabControl = null;
        //项目参数
        private ProjectModel _projectModel = null;
        //切片
        private DomLoader _domLoader = null;

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
        }

        public PnlLeftControl(GlobeView globeView, TabControl tabControl)
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
                pnode.Nodes.Clear();
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
        public void ShowDock(string title,Control dockControl)
        {
            if (_tabControl == null)
                return;
            TabPage tabPage = new TabPage(title);
            dockControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(dockControl);
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;
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
        {   //第一个为图层节点
            Node pnode = advTreeMain.Nodes[0];
            frmLoadVector frm = new frmLoadVector(new List<string>());
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string name = string.Empty;
                string path = string.Empty;
                if (frm.LineLayer != null)
                {
                    name = frm.LineLayer.LayerName;
                    path = frm.LineLayer.ShpLayerPath;
                }
                else if (frm.PntLayer != null)
                {
                    name = frm.PntLayer.LayerName;
                    path = frm.PntLayer.ShpLayerPath;
                }
                else if (frm.PolyLayer != null)
                {
                    name = frm.PolyLayer.LayerName;
                    path = frm.PolyLayer.ShpLayerPath;
                }
                //图层去重
                NodeModel checkModel = _projectModel.Nodes.Where(t => t.PNode == Guids.TCGL && t.NodeName == name).FirstOrDefault();
                if (checkModel != null)
                {
                    MsgBox.ShowInfo("当前名称的图层已经存在,请重命名或者导入其他数据");
                    return;
                }

                NodeModel model = new NodeModel();
                model.PNode = pnode.Name;
                model.NodeName = name;
                model.ShowCheck = true;
                model.ImageIndex = 2;
                model.NodeId = Guid.NewGuid().ToString();
                model.CanRemove = true;
                model.Path = path;
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
            node.Remove();       
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

            LonLatDataLayer lyr = (LonLatDataLayer)_globeView.GlobeLayers.DataLayers.FindLayer(checkNode.Text);
            //开始加载
            if (lyr == null)
            {
                string path = Path.Combine(Path.GetDirectoryName(_projectPath), checkNode.Text + ".xml");
                if (File.Exists(path) == false)
                    return;
                DomLayerInfo lyrInfo = XmlHelper.Deserialize<DomLayerInfo>(path);
                if (lyrInfo == null)
                    return;
                //加载影像图层
                lyr = LayerLoader.CreateDomLyr(lyrInfo, _globeView);
            }
            lyr.Visible = e.Cell.Checked;
            //飞行
            if (e.Cell.Checked)
            {
                double Lat = (lyr.South + lyr.North) / 2;
                double lon = (lyr.East + lyr.West) / 2;
                if (_globeView.GlobeCamera.CameraLatitude != Lat || _globeView.GlobeCamera.CameraLongitude != lon)
                    _globeView.GlobeCamera.FlyTo(lon, Lat, 1000);
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
            if (node != null && node.Parent.Name == Guids.JSGL && OnCaculate != null)
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
