using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Assignment4
{
    public partial class Form1 : Form
    {
        private double[] newDoubles = null;
        private double[,] sortArray = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 MyForm = new Form3();
            MyForm.Activate();
            MyForm.Show();
        }
        // Open file, load doubles from file, populate list
        private void button2_Click(object sender, EventArgs e)
        {
            //Stream myStream = null;
            String myFile = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "dbl files (*.dbl)|*.dbl|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myFile = openFileDialog1.FileName.ToString()) != null)
                    {
                        FileInfo fi1 = new FileInfo(myFile);
                        byte[] newBytes = File.ReadAllBytes(myFile);
                        newDoubles = new double[newBytes.Length / sizeof(double)];
                        for (int i = 0; i < newBytes.Length; i += 8)
                            newDoubles[(i + 1) / 8] = BitConverter.ToDouble(newBytes, i);
                        richTextBox1.Clear();
                        richTextBox1.Text = string.Join("\n",newDoubles.ToArray());
                        //listBox1.Items.Clear();
                        //for (int i = 0; i < newDoubles.Length; i++)
                        //{
                        //    listBox1.Items.Add(newDoubles[i]);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        // Sort loaded doubles with set amount of threads.
        private void button3_Click(object sender, EventArgs e)
        {

            if (newDoubles != null)
            {
                int numThreads = int.Parse(comboBox1.SelectedItem.ToString());
                double whoCares = newDoubles.Length / numThreads;
                int portion = (int)Math.Floor(whoCares);
                var threads = new List<Thread>();
                int[] sTart = new int[numThreads];
                DateTime start = DateTime.Now;
                toolStripStatusLabel1.Text = "Sorting...";
                Thread myThread;
                // Loops failed due to lack of better call methods for thread creation...
                // i.e. the iterator would increase before the thread was created so
                // the parameters would always get at least one index out of bounds
                // Solution:  Use this case switch.
                sortArray = new double[portion, numThreads];
                    switch (numThreads)
                    {
                        case 1:
                            myThread = new Thread(() => SortList(0, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            break;
                        case 2:
                            myThread = new Thread(() => SortList(0, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(1, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            break;
                        case 4:
                            myThread = new Thread(() => SortList(0, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(1, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(2, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(3, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            break;
                        case 10:
                            myThread = new Thread(() => SortList(0, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(1, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(2, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(3, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(4, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(5, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(6, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(7, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(8, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            myThread = new Thread(() => SortList(9, portion));
                            myThread.Start();
                            threads.Add(myThread);
                            break;
                        default:
                            Console.WriteLine("Error in thread count.");
                            break;
                    }
                foreach (var t in threads)
                {
                    t.Join();
                }
                SortWhole(newDoubles.Length, numThreads);
                TimeSpan timeItTook = DateTime.Now - start;
                toolStripStatusLabel1.Text = "Sort Time:  " + timeItTook.ToString();
                richTextBox1.Clear();
                richTextBox1.Text = string.Join("\n", newDoubles.ToArray());
            }
        }

        // Sort each section of the list in an independent thread
        private void SortList(int start, int numSorted)
        {
            for (int i = 0; i < numSorted; i++)
            {
                sortArray[i,start] = newDoubles[start*numSorted + i];
            }
            // Standard bubble sort
            for (int i = 0; i < numSorted; i++)
            {

                for (int j = 0; j < numSorted - 1; j++)
                {

                    if (sortArray[j, start] > sortArray[j + 1, start])
                    {
                        double tmp = sortArray[j + 1, start];
                        sortArray[j + 1, start] = sortArray[j, start];
                        sortArray[j, start] = tmp;
                    }

                }
            }
        }
        // Custom Merge Sort (My personal creation - not the real thing)
        private void SortWhole(int size, int sections)
        {
            int portion = size / sections;
            int[] sC = new int[sections];
            int chosen = 0;
            for(int i = 0; i < size; i++)
            {

                for (int c = 0; c < sections; c++)
                {
                    if (sC[c] < portion) 
                    {
                        chosen = c;
                        break;
                    }
                    else
                    {
                        chosen = c + 1;
                    }
                }
                if (chosen >= sections)
                    { break; }
                for(int s = 0; s < sections; s++)
                {
                    if(sC[s] < portion && sortArray[sC[s], s] < sortArray[sC[chosen], chosen])
                    {
                        chosen = s;
                    }
                }
                newDoubles[i] = sortArray[sC[chosen], chosen];
                sC[chosen]++;
            }
        }
    }
}
