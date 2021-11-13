using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Sortera_grafiskt
{
    public partial class Form1 : Form
    {
        int[] array = new int[700];
        int c_draw, c_sort, delay = 0;
        System.Drawing.Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread drawThread = new Thread(new ThreadStart(Draw));
            drawThread.Start();
        }

        void Draw()
        {
            g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Red, 1);
            Random r = new Random();
            if (c_draw == 0)
            {
                // Slumpmässigt
                for (int i = 0; i < 700; i++)
                {
                    array[i] = r.Next(400);
                    g.DrawLine(pen, i, 400, i, array[i]);
                }
            }
            else if (c_draw == 1)
            {
                // Rekursivt sorterad
                for (int i = 0; i < 700; i++)
                {
                    array[i] = (int)((double)i / 700.0 * 400.0);
                    g.DrawLine(pen, i, 400, i, array[i]);
                }
            }
            else if (c_draw == 2)
            {
                // Nästan sorterad
                for (int i = 0; i < 700; i++)
                {
                    array[i] = r.Next(400);
                }
                Array.Sort(array);
                Array.Reverse(array);
                for (int i = 0; i < 150; i++)
                {
                    int a = r.Next(700);
                    int b = r.Next(700);
                    Swap(a, b);
                }
                for (int i = 0; i < 700; i++)
                {
                    g.DrawLine(pen, i, 400, i, array[i]);
                }
            }
            else if (c_draw == 3)
            {
                // Block
                for (int i = 0; i < 700; i += 20)
                {
                    int value = r.Next(400);
                    for (int j = i; j < i + 20; j++)
                    {
                        array[j] = value;
                        g.DrawLine(pen, j, 400, j, array[j]);
                    }
                }
            }
            else if (c_draw == 4)
            {
                // Symmetriska kurvor
                bool shiftUp;
                array[0] = r.Next(20);
                g.DrawLine(pen, 0, 400, 0, 400 - array[0]);
                for (int i = 1; i < 700; i++)
                {
                    int value = r.Next(20);
                    if (array[i - 1] + value > 399)
                        shiftUp = false;
                    else
                        shiftUp = true;
                    if (shiftUp)
                    {
                        array[i] = 400 - (array[i - 1] + value);
                        g.DrawLine(pen, i, 400, i, array[i]);
                    }
                    else
                    {
                        array[i] = 400 - (array[i - 1] - value);
                        g.DrawLine(pen, i, 400, i, array[i]);
                    }
                }
            }
            else if (c_draw == 5)
            {
                // Fallande trianglar
                for (int i = 0; i < 700; i += 50)
                {
                    int q = 0;
                    for (int j = i; j < i + 50; j++)
                    {
                        array[j] = (int)((double)q / 50.0 * 400.0);
                        g.DrawLine(pen, j, 400, j, array[j]);
                        q++;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread sortThread = new Thread(new ThreadStart(Sort));
            sortThread.Start();
        }

        void Sort()
        {
            g = pictureBox1.CreateGraphics();
            if (c_sort == 0)
            {
                // Bubbelsortering
                for (int i = 0; i < 700; i++)
                {
                    for (int j = 1; j < 700 - i; j++)
                    {
                        if (array[j - 1] < array[j])
                        {
                            RedrawItems(j - 1, j);
                            Swap(j, j - 1);
                        }
                    }
                }
            }
            else if (c_sort == 1)
            {
                // Shakesort
                bool sorted = false;
                int n = 0;
                while (!sorted)
                {
                    sorted = true;
                    for (int i = 1; i < 700 - n; i++)
                    {
                        if (array[i - 1] < array[i])
                        {
                            RedrawItems(i, i - 1);
                            Swap(i, i - 1);
                            sorted = false;
                        }
                    }
                    for (int i = 699; i > 0 + n; i--)
                    {
                        if (array[i - 1] < array[i])
                        {
                            RedrawItems(i, i - 1);
                            Swap(i, i - 1);
                            sorted = false;
                        }
                    }
                    n++;
                }
            }
            else if (c_sort == 2)
            {
                // Gnome sort
                for (int m = 1; m < 700; ++m)
                {
                    for (int p = m; p > 0 && array[p] > array[p - 1]; p--)
                    {
                        RedrawItems(p, p - 1);
                        Swap(p, p - 1);
                    }
                }
            }
            else if (c_sort == 3)
            {
                // Urvalssortering
                for (int m = 0; m < 700 - 1; ++m)
                {
                    int pos = m;
                    for (int i = m + 1; i < 700; i++)
                    {
                        if (array[i] > array[pos])
                        {
                            pos = i;
                        }
                    }
                    if (m != pos)
                    {
                        RedrawItems(m, pos);
                        Swap(m, pos);
                    }
                }
            }
            else if (c_sort == 4)
            {
                // Instickssortering
                for (int m = 0; m < 700 - 1; ++m)
                {
                    for (int i = m + 1; i < 700; ++i)
                    {
                        if (array[m] < array[i])
                        {
                            RedrawItems(m, i);
                            Swap(m, i);
                        }
                    }
                }
            }
            else if (c_sort == 5)
            {
                // Shell sort
                int[] gaps = new int[6] { 112, 48, 21, 7, 3, 1 }; // Incerpi och Sedgewick gap sequence
                foreach (int gap in gaps)
                {
                    for (int i = gap; i < 700; i++)
                    {
                        int temp = array[i];
                        int j;
                        for (j = i; j >= gap && array[j - gap] < temp; j -= gap)
                        {
                            RedrawItems(j - gap, j);
                            Swap(j - gap, j);
                        }
                        array[j] = temp;
                    }
                }
            }
            else if (c_sort == 6)
            {
                // Heapsort
                int i;
                for (i = 700 / 2 - 1; i >= 0; i--)
                {
                    siftDown(i, 700 - 1);
                }
                for (i = 700 - 1; i >= 1; i--)
                {
                    RedrawItems(i, 0);
                    Swap(i, 0);
                    siftDown(0, i - 1);
                }
            }
            else if (c_sort == 7)
            {
                // Quicksort
                Quicksort(0, 699);
            }
            else if (c_sort == 8)
            {
                // Quertionsort
                Quertionsort(0, 699);
            }
        }

        void Swap(int a, int b)
        {
            int temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }

        void RedrawItems(int a, int b)
        {
            lock (pictureBox1)
            {
                Pen pen = new Pen(Color.Red, 1);
                Pen eraser = new Pen(Color.White, 1);
                int temp = array[a];
                g.DrawLine(eraser, a, 700, a, array[a]);
                if (delay != 0)
                {
                    Thread.Sleep(delay);
                }
                g.DrawLine(pen, a, 700, a, array[b]);
                g.DrawLine(eraser, b, 700, b, array[b]);
                if (delay != 0)
                {
                    Thread.Sleep(delay);
                }
                g.DrawLine(pen, b, 700, b, temp);
            }
        }

        void Quicksort(int left, int right)
        {
            int l_hold = left;
            int r_hold = right;

            while (left < right)
            {
                while (array[left] > array[right] && left < right)
                    left++;
                if (array[left] <= array[right] && left < right)
                {
                    RedrawItems(left, right);
                    Swap(left, right);
                    right--;
                }
                while (array[right] <= array[left] && left < right)
                    right--;
                if (array[right] > array[left] && left < right)
                {
                    RedrawItems(left, right);
                    Swap(left, right);
                    left++;
                }
            }
            if (l_hold < right)
                Quicksort(l_hold, right - 1);
            if (left < r_hold)
                Quicksort(left + 1, r_hold);
        }

        void Quertionsort(int left, int right)
        {
            if (right - left < 9) // Kör instickssortering
            {
                for (int m = left; m < right; ++m)
                {
                    for (int i = m + 1; i < 700; ++i)
                    {
                        if (array[m] < array[i])
                        {
                            RedrawItems(m, i);
                            Swap(m, i);
                        }
                    }
                }
            }
            else // Partionera listan med Quicksort rekursivt
            {
                int l_hold = left;
                int r_hold = right;

                while (left < right)
                {
                    while (array[left] > array[right] && left < right)
                        left++;
                    if (array[left] <= array[right] && left < right)
                    {
                        RedrawItems(left, right);
                        Swap(left, right);
                        right--;
                    }
                    while (array[right] <= array[left] && left < right)
                        right--;
                    if (array[right] > array[left] && left < right)
                    {
                        RedrawItems(left, right);
                        Swap(left, right);
                        left++;
                    }
                }
                if (l_hold < right)
                    Quertionsort(l_hold, right - 1);
                if (left < r_hold)
                    Quertionsort(left + 1, r_hold);
            }
        }

        void siftDown(int root, int bottom)
        {
            bool done = false;
            int maxChild;
            while (root * 2 <= bottom && !done)
            {
                if (root * 2 == bottom)
                    maxChild = root * 2;
                else if (array[root * 2] < array[root * 2 + 1])
                    maxChild = root * 2;
                else
                    maxChild = root * 2 + 1;
                if (array[root] > array[maxChild])
                {
                    RedrawItems(maxChild, root);
                    Swap(maxChild, root);
                    root = maxChild;
                }
                else
                    done = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            c_draw = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            c_sort = comboBox2.SelectedIndex;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            delay = trackBar1.Value;
        }
    }
}
