using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor
{
    public partial class Form1 : Form
    {
        public enum Mode
        {
            Drawing,
            NotDrawing,
        }
        public static Mode mode;
        public Stream bgStream = new FileStream("../../../../LevelEditorContent/images/null.png", FileMode.Open, FileAccess.Read, FileShare.Read);
        public Form1()
        {
            InitializeComponent();
            mode = Mode.NotDrawing;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            bgStream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mode = Mode.Drawing;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mode = Mode.NotDrawing;
        }
    }
}
