﻿using FloodPeakUtility;
using FloodPeakUtility.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakToolUI.UI
{
    public partial class FormCaculateArgView : Form
    {
        private string _projectForlder = string.Empty;
        public FormCaculateArgView()
        {
            InitializeComponent();
        }

        public FormCaculateArgView(string projectForlder)
            : this()
        {
            _projectForlder = projectForlder;
            //根据文件夹来获取里面的参数文件
            string xmlPath = Path.Combine(projectForlder, ConfigNames.RainStormSub);
            //暴雨衰减赋值
            if (File.Exists(xmlPath))
            {
                BYSJResult bysj = XmlHelper.Deserialize<BYSJResult>(xmlPath);
                txtsd.Text = bysj.Sd.ToString();
                txtnd.Text = bysj.nd.ToString();
                txtd.Text = bysj.d.ToString();
            }
            //暴雨损失赋值
            xmlPath = Path.Combine(projectForlder, ConfigNames.RainStormLoss);
            if (File.Exists(xmlPath))
            {
                BYSSResult byss = XmlHelper.Deserialize<BYSSResult>(xmlPath);
                txtF.Text = byss.F.ToString();
                txtR.Text = byss.R.ToString();
                txtr1.Text = byss.r1.ToString();
            }
            //沟道汇流赋值
            xmlPath = Path.Combine(projectForlder, ConfigNames.RiverConfluence);
            if (File.Exists(xmlPath))
            {
                HCHLResult hchl = XmlHelper.Deserialize<HCHLResult>(xmlPath);
                txtA1.Text = hchl.A1.ToString();
                txtI1.Text = hchl.l1.ToString();
                txtL1.Text = hchl.L1.ToString();
            }
            //坡面汇流赋值
            xmlPath = Path.Combine(projectForlder, ConfigNames.SlopeConfluence);
            if (File.Exists(xmlPath))
            {
                PMHLResult pmhl = XmlHelper.Deserialize<PMHLResult>(xmlPath);
                txtA2.Text = pmhl.A2.ToString();
                txtI2.Text = pmhl.l2.ToString();
                txtL2.Text = pmhl.L2.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //保存参数

            //暴雨衰减保存
            string xmlPath = Path.Combine(_projectForlder, ConfigNames.RainStormSub);
            BYSJResult bysj = new BYSJResult()
            {
                Sd = string.IsNullOrWhiteSpace(txtsd.Text) ? 0 : Convert.ToDouble(txtsd.Text),
                nd = string.IsNullOrWhiteSpace(txtnd.Text) ? 0 : Convert.ToDouble(txtnd.Text),
                d = string.IsNullOrWhiteSpace(txtd.Text) ? 0 : Convert.ToDouble(txtd.Text),
            };
            XmlHelper.Serialize<BYSJResult>(bysj, xmlPath);

            //暴雨损失保存
            xmlPath = Path.Combine(_projectForlder, ConfigNames.RainStormLoss);
            BYSSResult byss = new BYSSResult()
            {
                F = string.IsNullOrWhiteSpace(txtF.Text) ? 0 : Convert.ToDouble(txtF.Text),
                R = string.IsNullOrWhiteSpace(txtR.Text) ? 0 : Convert.ToDouble(txtR.Text),
               r1 = string.IsNullOrWhiteSpace(txtr1.Text) ? 0 : Convert.ToDouble(txtr1.Text),
            };
            XmlHelper.Serialize<BYSSResult>(byss, xmlPath);

            //沟道汇流保存
            xmlPath = Path.Combine(_projectForlder, ConfigNames.RiverConfluence);
            HCHLResult hchl = new HCHLResult()
            {
                A1 = string.IsNullOrWhiteSpace(txtA1.Text) ? 0 : Convert.ToDouble(txtA1.Text),
                l1 = string.IsNullOrWhiteSpace(txtI1.Text) ? 0 : Convert.ToDouble(txtI1.Text),
                L1 = string.IsNullOrWhiteSpace(txtL1.Text) ? 0 : Convert.ToDouble(txtL1.Text),
            };
            XmlHelper.Serialize<HCHLResult>(hchl, xmlPath);

            //沟道汇流保存
            xmlPath = Path.Combine(_projectForlder, ConfigNames.SlopeConfluence);
            PMHLResult pmhl = new PMHLResult()
            {
                A2 = string.IsNullOrWhiteSpace(txtA2.Text) ? 0 : Convert.ToDouble(txtA2.Text),
                l2 = string.IsNullOrWhiteSpace(txtI2.Text) ? 0 : Convert.ToDouble(txtI2.Text),
                L2 = string.IsNullOrWhiteSpace(txtL2.Text) ? 0 : Convert.ToDouble(txtL2.Text),
            };
            XmlHelper.Serialize<PMHLResult>(pmhl, xmlPath);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
