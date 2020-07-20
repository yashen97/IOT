using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace IOT_Sensors
{
    public partial class OpenCon : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "ot8tz6iPK5IzXwbr4ZrGEyI3ywXhB8PXr5Jd3xTb",
            BasePath = "https://yashen-8d9ff.firebaseio.com/"
        };

        IFirebaseClient client;

        public OpenCon()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                MessageBox.Show("Connection Ok");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Fname = textBox1.Text,
                Lname = textBox2.Text,
                Gender = comboBox1.Text,
                Email = textBox3.Text
            };

            SetResponse response = await client.SetAsync("Information/keep", data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted");

        }
       
        private async void trackBar1_Scroll(object sender, EventArgs e)
        {
           int val = trackBar1.Value;
            label5.Text = "Temperature " + val.ToString() + "(°C)";

            SetResponse response = await client.SetAsync("Sensor Readings /Temp (°C)", val);
            // Data result = response.ResultAs<Data>();
            

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 200;

          
                progressBar1.Value =val;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
