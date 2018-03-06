using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BilibiliGirls
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 鼠标位置（用于拖动时的位置计算）
        /// </summary>
        private Point mouseLocation = Point.Empty;

        /// <summary>
        /// 正在显示的Girl
        /// </summary>
        private IGirl Girl = null;

        /// <summary>
        /// 配置文件XML根节点
        /// </summary>
        private XmlNode rootNode = null;


        public MainForm()
        {
            InitializeComponent();
            //初始化配置文件根节点
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("config.xml");
            rootNode = xmlDoc.ChildNodes[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Girl = new Girl_22(imageList, rootNode.ChildNodes[0]);
            召唤22娘ToolStripMenuItem.Visible = false;
            召唤33娘ToolStripMenuItem.Visible = true;
            timer.Enabled = true;
            InitOpenContextMenu();
            openContextMenu.ItemClicked += OpenContextMenu_ItemClicked;
        }

        private async void InitOpenContextMenu()
        {
            openContextMenu.Items.Clear();
            string proFilePath = Path.Combine(Application.StartupPath, "Open.txt");
            if (File.Exists(proFilePath))
            {
                List<string> lines = new List<string>(File.ReadAllLines(proFilePath));
                for (int i = 0; i < lines.Count; i++)
                {
                    Icon ico = null;
                    try
                    {
                        if (File.Exists(lines[i + 1]) && lines[i + 1].EndsWith(".exe"))
                        {
                            ico = Icon.ExtractAssociatedIcon(lines[i + 1]);
                        }
                        else if (lines[i + 1].StartsWith("http"))
                        {
                            var req = System.Net.WebRequest.CreateHttp(lines[i + 1]);
                            var icoUrl = string.Format("{0}://{1}/favicon.ico", lines[i + 1].StartsWith("https") ? "https" : "http", req.Host);
                            var icoReq = System.Net.WebRequest.CreateHttp(icoUrl);
                            var resp = await icoReq.GetResponseAsync();
                            MemoryStream ms = new MemoryStream();
                            resp.GetResponseStream().CopyTo(ms);
                            ms.Position = 0;
                            ico = new System.Drawing.Icon(ms);
                        }
                    }
                    catch (Exception exc)
                    { }
                    if (ico == null)
                    {
                        openContextMenu.Items.Add(lines[i]);
                    }
                    else
                    {
                        openContextMenu.Items.Add(lines[i], ico.ToBitmap());
                    }
                    openContextMenu.Items[openContextMenu.Items.Count - 1].Tag = lines[i + 1];
                    i++;
                }
            }
        }

        private void OpenContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string fileName = e.ClickedItem.Tag as string;
            if (fileName != null)
            {
                try
                {
                    Process.Start(fileName);
                }
                catch (Exception)
                {
                    return;
                }
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moveForms(this.PointToScreen(e.Location));
                if (pictureBox1.Image != Girl.MovingImage)
                {
                    pictureBox1.Image = Girl.MovingImage;
                    label1.Text = Girl.DragingSay();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                timer.Enabled = false;
                pictureBox1.Cursor = Cursors.SizeAll;
                mouseLocation = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            timer.Enabled = true;
            pictureBox1.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="locationToScreen">当前鼠标点相对于屏幕的坐标</param>
        private void moveForms(Point locationToScreen)
        {
            this.Left = locationToScreen.X - mouseLocation.X;
            this.Top = locationToScreen.Y - mouseLocation.Y;
        }

        private int indexOfImage = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = 300;
            this.pictureBox1.Image = Girl.StandingImageList[indexOfImage];
            indexOfImage++;
            if (indexOfImage == Girl.StandingImageList.Count)
            {
                indexOfImage = 0;
            }
            label1.Text = "";
        }

        private void 召唤22娘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Girl = new Girl_22(imageList, rootNode.ChildNodes[0]);
            召唤22娘ToolStripMenuItem.Visible = false;
            召唤33娘ToolStripMenuItem.Visible = true;
        }

        private void 召唤33娘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Girl = new Girl_33(imageList, rootNode.ChildNodes[1]);
            召唤22娘ToolStripMenuItem.Visible = true;
            召唤33娘ToolStripMenuItem.Visible = false;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 自定义右键菜单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuEditForm f2 = new ContextMenuEditForm();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                InitOpenContextMenu();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && pictureBox1.Image != Girl.MovingImage)
            {
                pictureBox1.Image = Girl.SayingImage;
                label1.Text = Girl.Say();
                timer.Interval = 1500;
            }
        }
    }
}
