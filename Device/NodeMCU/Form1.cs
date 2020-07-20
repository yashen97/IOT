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
using Google.Cloud.Firestore;

namespace NodeMCU
{
    public partial class Form1 : Form
    {
        //Creating Firebase Connection
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "puFaElXh8bI4TL91C4eOpXt3ovZnHDtGgMTf8cFT",
            BasePath = "https://ply-iot-39c44.firebaseio.com/"
        };
        IFirebaseClient client;

        double pmin, pmax, vol;
        FirestoreDb db;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"ply-iot.json";
        
        private async void timer1_Tick(object sender, EventArgs e)
        {

            FirebaseResponse response = await client.GetTaskAsync("1000");
            Read obj = response.ResultAs<Read>();

            int lvl = Convert.ToInt32(obj.LVL);
            double pH = Convert.ToDouble(obj.PH);
            double temp = Convert.ToDouble(obj.TEMP);
            double qty = Convert.ToDouble(obj.QTY);
            string motor = obj.Motor;
            string lockD = obj.isLockdown;

            if (lockD.Equals("NO"))
            {
                label8.Visible = true;
                trackBar1.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
            else
            {
                label8.Visible = false;
                trackBar1.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }

            label9.Text = "Motor Status : " + motor;
            trackBar1.Value = lvl;
            label1.Text = "Level(" + lvl + " %)";
            label4.Text = "Level(" + lvl + " %)";
            label4.Visible = true;
           
            
                lvl += 1;
                if (lvl >= 100)
                {
                    timer1.Enabled = false;
                    timer1.Stop();

                    trackBar1.Value = lvl;
                }
                //continue
                
                // ScrollBar1 = progress.ToString() + "%";
            

        }

        private async void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Level(" + trackBar1.Value.ToString() + " %)";
            var data = new Data
            {
                LVL = trackBar1.Value.ToString()
            };

            SetResponse response = await client.SetTaskAsync("1000/", data);
           // Data result = response.ResultAs<Data>();
           
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                
                if (textBox1.Text.Length == 0) e.Handled = true;
                
                if (textBox1.SelectionStart == 0) e.Handled = true;
                
                if (alreadyExist(textBox1.Text, ref sepratorChar)) e.Handled = true;
               
                if (textBox1.SelectionStart != textBox1.Text.Length && e.Handled == false)
                {
                   
                    string AfterDotString = textBox1.Text.Substring(textBox1.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
           
            if (Char.IsDigit(e.KeyChar))
            {
                
                if (alreadyExist(textBox1.Text, ref sepratorChar))
                {
                    int sepratorPosition = textBox1.Text.IndexOf(sepratorChar);
                    string afterSepratorString = textBox1.Text.Substring(sepratorPosition + 1);
                    if (textBox1.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }

        }

        private bool alreadyExist(string _text, ref char KeyChar)
        {
            if (_text.IndexOf('.') > -1)
            {
                KeyChar = '.';
                return true;
            }
            if (_text.IndexOf(',') > -1)
            {
                KeyChar = ',';
                return true;
            }
            return false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
           
            
                timer1.Enabled = true;
                timer1.Interval = 1500;
            
          


           
                var data = new Data
                {
                    LVL = trackBar1.Value.ToString()
                };
            
              SetResponse response = await client.SetTaskAsync("1000/", data);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                MessageBox.Show("Device Online");

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                db = FirestoreDb.Create("ply-iot-39c44");

                DocumentReference docc = db.Collection("Devices").Document("1000");
                DocumentSnapshot snapp = await docc.GetSnapshotAsync();


                FirebaseResponse response = await client.GetTaskAsync("1000");
                Read obj = response.ResultAs<Read>();

                int lvl = Convert.ToInt32(obj.LVL);
                double pH = Convert.ToDouble(obj.PH);
                double temp = Convert.ToDouble(obj.TEMP);
                double qty = Convert.ToDouble(obj.QTY);
                string motor = obj.Motor;
                string lockD = obj.isLockdown;

                label9.Text = "Motor Status : " + motor;
                textBox1.Text = temp.ToString();
                textBox2.Text = pH.ToString();
                textBox3.Text = qty.ToString();

                trackBar1.Value = lvl;
                label1.Text = "Water Level("+lvl+" %)";

                if (snapp.Exists)
                {
                    Device dc = snapp.ConvertTo<Device>();
                    pmin = Convert.ToDouble(dc.pmin);
                    pmax = Convert.ToDouble(dc.pmax);
                    vol = Convert.ToDouble(dc.volume);
                    
                }
            }
        }
    }
}
