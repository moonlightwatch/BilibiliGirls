using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace BilibiliGirls
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string proFilePath = Path.Combine(Application.StartupPath, "Open.txt");
            if (File.Exists(proFilePath))
            {
                List<string> lines = new List<string>(File.ReadAllLines(proFilePath));
                for (int i = 0; i < lines.Count; i++)
                {
                    dataGridView1.Rows.Add(lines[i], lines[i + 1]);
                    i++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string proFilePath = Path.Combine(Application.StartupPath, "Open.txt");
            List<string> lines = new List<string>();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if(row.Cells[0].Value==null|| row.Cells[1].Value==null)
                {
                    continue;
                }
                string name = row.Cells[0].Value.ToString();
                string file = row.Cells[1].Value.ToString();
                
                lines.Add(name);
                lines.Add(file);
            }
            File.WriteAllLines(proFilePath, lines.ToArray());
            this.DialogResult = DialogResult.OK;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==1)
            {
                if(openFileDialog1.ShowDialog()== DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = openFileDialog1.SafeFileName;
                    dataGridView1.Rows[e.RowIndex].Cells[1].Value = openFileDialog1.FileName;
                }
            }
        }
    }
}
