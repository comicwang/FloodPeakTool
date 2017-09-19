using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FloodPeakUtility.Model;
using FloodPeakUtility;
using FloodPeakUtility.UI;

namespace FloodPeakToolUI.UI
{
    public partial class FileChooseControl : UserControl
    {
        private PnlLeftControl _model = null;
        private ImportType _fileType = ImportType.Shp;
        private int _selectedValue = -1;

        /// <summary>
        /// 选择的节点
        /// </summary>
        public NodeModel SelectedValue
        {
            get { return cmbAll.SelectedItem as NodeModel; }
        }

        public FileChooseControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或者设置左边文字内容
        /// </summary>
        public string LeftTitle
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        /// <summary>
        /// 选择事件,用于确定参数是否足够计算
        /// </summary>
        public event EventHandler OnSelectIndexChanged;

        /// <summary>
        /// 获取文件路径
        /// </summary>
        public string FilePath
        {
            get 
            {
                return cmbAll.SelectedValue == null ? "" : cmbAll.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// 获取或者设置文件的操作类型
        /// </summary>
        public ImportType FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        /// <summary>
        /// 导入文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            switch (FileType)
            {
                case ImportType.Dom:
                    _model.ctxImportDom_Click(this, null);
                    break;
                case ImportType.Shp:
                    _model.ctxImportShp_Click(this, null);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 提供地址选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void _model_OnChanged(object sender, ProjectChangedEventArgs arg)
        {
            IniliazeCombox(_model.ProjectModel);
        }

        private void IniliazeCombox(ProjectModel model)
        {
            var result = model.Nodes.Where(t => t.PNode == Guids.TCGL);
            switch (FileType)
            {
                case ImportType.Dom:
                    result = result.Where(t => t.Path.EndsWith("tif"));
                    break;
                case ImportType.Shp:
                    result = result.Where(t => t.Path.EndsWith("shp"));
                    break;
                default:
                    result = result.Where(t => t.Path.EndsWith("tif"));
                    break;
            }
            //保存选中的文件
            _selectedValue = cmbAll.SelectedIndex;
            cmbAll.DataSource = result.ToList();
            if (_selectedValue <= cmbAll.Items.Count - 1)
                cmbAll.SelectedIndex = _selectedValue;
            else
                cmbAll.SelectedIndex = -1;
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="model"></param>
        public void BindSource(PnlLeftControl model)
        {
            _model = model;
            _model.OnChanged += _model_OnChanged;
            cmbAll.DisplayMember = "NodeName";
            cmbAll.ValueMember = "Path";
            IniliazeCombox(model.ProjectModel);

        }

        private void cmbAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            //用于确定计算按钮
            if (OnSelectIndexChanged != null)
                OnSelectIndexChanged(sender, e);
        }
    }
}
