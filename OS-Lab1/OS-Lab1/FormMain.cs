using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Lab1
{
    public partial class FormMain : Form
    {
        public Graphics g;

        private int height;

        Pen[] processes = { new Pen(Color.Red), new Pen(Color.Green), new Pen(Color.Blue), new Pen(Color.Black) };

        public List<(int, int, int, int)> pointList = new List<(int, int, int, int)>();

        public List<(int, int, int, int)> startPoints = new List<(int, int, int, int)>();

        public List<(int, int, int)> threadList = new List<(int, int, int)>();

        public FormMain()
        {
            InitializeComponent();
            height = pictureBox.Height / 6;
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(bmp);
            DrawMarking(g);
            pictureBox.Image = bmp;
            startPoints.Add((1, 0, 1, height));            
        }

        public void AddThread(int ProccessId, int ThreadId, int IterationTime)
        {
            threadList.Add((ProccessId, ThreadId, IterationTime));
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            SystemCore systemCore = new SystemCore(this);

            Random random = new Random();

            for (int i = 0; i < random.Next() % 4 + 1; i++)
            {
                systemCore.CreateProcesses();
            }

            systemCore.StartPlanning();

            startPoints.Add(pointList.Last());
        }

        private void DrawMarking(Graphics g)
        {
            for (int i = 0; i < pictureBox.Height / height; i++)
            {
                g.DrawLine(new Pen(Color.Black), 0, (i + 1) * height, pictureBox.Width, (i + 1) * height);
            }
        }

        public void Draw(Graphics g)
        {
            int tempWidth = 0;

            int tempHeight = 20;

            foreach (var thread in threadList)
            {
                g.DrawLine(processes[thread.Item1], tempWidth * 10 + 2, thread.Item2 * 25 + tempHeight, (tempWidth + thread.Item3) * 10, thread.Item2 * 25 + tempHeight);
                tempWidth += thread.Item3;

                if (tempWidth + 10 > pictureBox.Width / 10)
                {
                    tempWidth = 0;
                    tempHeight += height;
                }
            }

            foreach (var mark in startPoints)
            {
                g.DrawLine(new Pen(Color.Green), mark.Item1 + 2, mark.Item2, mark.Item3 + 2, mark.Item4);
            }

            pointList.Add((tempWidth * 10, tempHeight - 20, tempWidth * 10, tempHeight + height - 20));
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            pointList = new List<(int, int, int, int)>();
            startPoints = new List<(int, int, int, int)>();
            threadList = new List<(int, int, int)>();
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(bmp);
            DrawMarking(g);
            pictureBox.Image = bmp;
            startPoints.Add((1, 0, 1, height));
        }
    }
}
