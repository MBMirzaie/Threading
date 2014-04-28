using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment4
{
    public partial class Form3 : Form
    {
        private int sizeOfDoubles = 25000;

        public Form3()
        {
            InitializeComponent();
            comboBox1.Items.Add("25000");
            comboBox1.Items.Add("100000");
            comboBox1.Items.Add("250000");
            comboBox1.Items.Add("500000");
            comboBox1.Items.Add("1000000");
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sizeOfDoubles = int.Parse(comboBox1.SelectedItem.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "dbl files (*.dbl)|*.dbl|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    double[] myArray = new double[sizeOfDoubles];
                    Random random = new Random();
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        myArray[i] = random.Next() + random.NextDouble();
                        myStream.Write(BitConverter.GetBytes(myArray[i]), 0, sizeof(double));
                    }
                    for (int i = 0; i < 10; i++)
                        Console.WriteLine(myArray[i]);
                    myStream.Close();
                    this.Close();
                }
            }
        }
    }
}
