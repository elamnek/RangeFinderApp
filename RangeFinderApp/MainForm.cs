using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RangeFinderApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //create the serial connection
        private SerialPort sp_1;
        private NpgsqlConnection m_connPG = null;

        private Boolean blnMeasuring = false;

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtPort.Text = Properties.Settings.Default.Port;
                txtDBConn.Text = Properties.Settings.Default.DBConn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Port = txtPort.Text;
                Properties.Settings.Default.DBConn = txtDBConn.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {

                sp_1 = new SerialPort(txtPort.Text, 9600, Parity.None, 8, StopBits.One);

                blnMeasuring = true;

                if (!sp_1.IsOpen) {

                    sp_1.DataReceived += new SerialDataReceivedEventHandler(sp_1_DataReceived);
                    //sp_1.ErrorReceived += new SerialErrorReceivedEventHandler(sp_1_ErrorReceived);

                    sp_1.Open();
                    
                    
                    byte[] buffer = { 0x80, 0x06, 0x03, 0x77 };
                    sp_1.Write(buffer, 0, 4);
  
                }


                //postgres connection
                if (txtDBConn.Text.Length > 0)
                {     
                    //open database
                    m_connPG = new NpgsqlConnection(txtDBConn.Text);
                    m_connPG.Open();
                }

               
            }
            catch (Exception ex)
            {
               MessageBox.Show("Could not open range finder port: " + ex.Message, "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        

        public static void SetControlText(Control ctl, string text)
        {
            ctl.Invoke((MethodInvoker)delegate { ctl.Text = text; });
        }

        public static void SetRTBText(Control rtb, string text)
        {
            rtb.Invoke((MethodInvoker)delegate { rtb.Text = text; });
        }

        
        
        private void sp_1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                if (!blnMeasuring) { return; }

                //get the data from the serial port and extract the distance from it
                SerialPort sp = (SerialPort)sender;
                string strData = sp.ReadExisting();
                string[] parts = strData.Split('?'); //? -? 000.442 ?
                string strDist = parts[2].Trim();
                SetRTBText(rtb, strDist);
                if (double.TryParse(strDist, out double dblDist))
                {
                    SetControlText(lblRange, dblDist.ToString());

                    string strTime = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f") + "'";

                    if (double.TryParse(txtMaxRange.Text,out double dblMaxRange))
                    {
                        if (dblDist <= dblMaxRange)
                        {
                            //do the insert into the postgres realtime table here
                            RealtimeInsertIntoDT(strTime, dblDist);
                        }
                    }

                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RealtimeInsertIntoDT(string strTime, double dblDist)
        {
            try
            {

                if (m_connPG == null) { return; }

                string strSQL = "insert into dt_ts_realtime_data (time,data_channel,range_m)" + " values (" + strTime + ",3,"+ dblDist.ToString() + ") ON CONFLICT DO NOTHING";

                NpgsqlCommand commPG = new NpgsqlCommand(strSQL, m_connPG);
                commPG.ExecuteNonQuery();
                commPG.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            blnMeasuring = false;
        }
    }
}


//dead car bodies


//private void sp_1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
//{
//    string strReceived = sp_1.ReadLine().Trim();
//    txtData.Text = strReceived;
//}



//blnMeasuring = true;

//byte[] buffer = { 0x80, 0x06, 0x03, 0x77 };
//sp_1.Write(buffer, 0, 4);

//while (blnMeasuring)
//{

//    //System.Threading.Thread.Sleep(1000);

//    int btr = sp_1.BytesToRead;
//    if (btr > 0)
//    {
//        int[] data = new int[btr];

//        for (int i = 0; i < btr - 1; i++)
//        {
//            data[i] = sp_1.ReadByte();
//        }

//        if (data[3] == 'E' && data[4] == 'R' && data[5] == 'R')
//        {
//            //serialRemote.println("Out of range");
//            lblRange.Text = "ZZ";
//        }
//        else
//        {
//            //float distance = 0;
//            double dblDistance = (data[3] - 0x30) * 100 + (data[4] - 0x30) * 10 + (data[5] - 0x30) * 1 + (data[7] - 0x30) * 0.1 + (data[8] - 0x30) * 0.01 + (data[9] - 0x30) * 0.001;

//            lblRange.Text = dblDistance.ToString();

//            //DateTime now = rtc.now();
//            //m_strSend = "{13|" + String(now.hour()) + ':' + String(now.minute()) + ':' + String(now.second()) + " " + String(now.day()) + '/' + String(now.month()) + '/' + String(now.year()) + ",20|" + String(distance) + "}";
//            //serialRemote.println(m_strSend);

//            //add this so data send rate to remote is around 4Hz
//            // delay(200);
//        }

//        sp_1.DiscardInBuffer();


//    }

//}
