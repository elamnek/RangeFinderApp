﻿using System;
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

        private DateTime m_dteStart;

        private DateTime m_dtePrevious;
        private double m_dblReadingPrevious;
        private double m_dblReadingPrevious2;
        private double m_dblVelocityPrevious;

        

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtPort.Text = Properties.Settings.Default.Port;
                txtDBConn.Text = Properties.Settings.Default.DBConn;
                txtSampleRate.Text = Properties.Settings.Default.SampleRate;
                txtMinRange.Text = Properties.Settings.Default.MinRange;
                txtMaxRange.Text = Properties.Settings.Default.MaxRange;
                txtMaxSeparation.Text = Properties.Settings.Default.MaxSeparation;
                txtRunNum.Text = Properties.Settings.Default.RunNum;

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
                Properties.Settings.Default.SampleRate = txtSampleRate.Text;
                Properties.Settings.Default.MinRange = txtMinRange.Text;
                Properties.Settings.Default.MaxRange = txtMaxRange.Text;
                Properties.Settings.Default.MaxSeparation = txtMaxSeparation.Text;
                Properties.Settings.Default.RunNum = txtRunNum.Text;
                Properties.Settings.Default.Save();

                if (sp_1 != null)
                {
                    if (sp_1.IsOpen)
                    {
                        sp_1.Close();
                    }
                }

                if (m_connPG != null)
                {
                    if (m_connPG.State == ConnectionState.Open)
                    {
                        m_connPG.Close();
                    }
                }
                

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

                sp_1 = new SerialPort(txtPort.Text, 115200, Parity.None, 8, StopBits.One);

                blnMeasuring = true;

                if (!sp_1.IsOpen) {

                    sp_1.DataReceived += new SerialDataReceivedEventHandler(sp_1_DataReceived);
                    //sp_1.ErrorReceived += new SerialErrorReceivedEventHandler(sp_1_ErrorReceived);

                    sp_1.Open();

                    //tell the unit to send text rather than binary
                    byte[] buffer7 = { 0x00, 0x11, 0x01, 0x45 };
                    sp_1.Write(buffer7, 0, 4);

                    m_dteStart = DateTime.Now;
                }

                sp_1.DiscardInBuffer();

                
                m_dblReadingPrevious = 0;
                m_dblReadingPrevious2 = 0;
                m_dblVelocityPrevious = 0;
                m_dtePrevious = DateTime.Now;


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

                DateTime dteNow = DateTime.Now;
                TimeSpan ts = dteNow.Subtract(m_dteStart);

                double dblSampleRate = double.Parse(txtSampleRate.Text);
                double dblMinRange = double.Parse(txtMinRange.Text);
                double dblMaxRange = double.Parse(txtMaxRange.Text);
                double dblMaxSeparation = double.Parse(txtMaxSeparation.Text);

                SerialPort sp = (SerialPort)sender;
                string strData = sp.ReadExisting();

                double dblReadingThis;
                if (double.TryParse(strData, out dblReadingThis))
                {

                    if (dblReadingThis >= (dblMinRange*1000) && dblReadingThis <= (dblMaxRange*1000))
                    {

                        //double dblDistDiff2 = Math.Abs(m_dblReadingPrevious2 - dblReadingThis);
                        //if (dblDistDiff2 <= (dblMaxSeparation*1000))
                        //{
                           
                            

                            if (ts.Milliseconds > dblSampleRate)
                            {
                            //calc velocity and accel.

                            SetControlText(lblRange, strData);

                            DateTime dteThis = DateTime.Now;

                            double dblDistDiff = (m_dblReadingPrevious - dblReadingThis) / 1000; //m
                            double dblTimeDiff = dteThis.Subtract(m_dtePrevious).TotalSeconds;
                            double dblVelocityThis = dblDistDiff / dblTimeDiff; //m/s
                            double dblVelocityDiff = m_dblVelocityPrevious - dblVelocityThis;
                            double dblAccelThis = dblVelocityDiff / dblTimeDiff;

                            //SetControlText(lblVelocity, Math.Round(dblVelocityThis,2).ToString() + "m/s");
                            //SetControlText(lblAcceleration, Math.Round(dblAccelThis, 2).ToString() + "m/s^2");

                            //do the insert into the postgres realtime table here
                            string strTime = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f") + "'";
                            RealtimeInsertIntoDT(strTime, dblReadingThis, dblVelocityThis, dblAccelThis);


                            m_dteStart = DateTime.Now;
                            m_dblReadingPrevious = dblReadingThis;
                            m_dblVelocityPrevious = dblVelocityThis;
                            m_dtePrevious = dteThis;
                        }
                        //}
                        //m_dblReadingPrevious2 = dblReadingThis;
                    }
                }
                sp.DiscardOutBuffer();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RealtimeInsertIntoDT(string strTime, double dblDist, double dblVelocityThis, double dblAccelThis)
        {
            try
            {

                if (m_connPG == null) { return; }

                int intRunNum = int.Parse(txtRunNum.Text);

                string strSQL = "insert into dt_ts_realtime_data (time,data_channel,range_m,run_number)" + " values (" + strTime + ",3,"+ dblDist.ToString() + "," + intRunNum.ToString() + ") ON CONFLICT DO NOTHING";

                NpgsqlCommand commPG = new NpgsqlCommand(strSQL, m_connPG);
                commPG.ExecuteNonQuery();
                commPG.Dispose();

                //strSQL = "insert into dt_ts_rangefindersensor (time,range_m_dq, range_m,net_velocity,net_acceleration)" + " values (" + strTime + ",1," + dblDist.ToString() + "," + dblVelocityThis.ToString() + "," + dblAccelThis.ToString() + ") ON CONFLICT DO NOTHING";
                //commPG = new NpgsqlCommand(strSQL, m_connPG);
                //commPG.ExecuteNonQuery();
                //commPG.Dispose();

                //strSQL = "insert into dt_ts_master (time)" + " values (" + strTime + ") ON CONFLICT DO NOTHING";
                //commPG = new NpgsqlCommand(strSQL, m_connPG);
                //commPG.ExecuteNonQuery();
                //commPG.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (sp_1.IsOpen)
            {
                sp_1.Close();
            }

            if (m_connPG.State == ConnectionState.Open)
            {
                m_connPG.Close();
            }
               
            blnMeasuring = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                byte[] buffer = { 0xFA, 0x04, 0x05, 0x14 };


                int CSsum = 0;

                for (int i = 0; i < buffer.Length; i++)
                {
                    CSsum += buffer[i];
                }

                int CS = (~CSsum) + 1;
                MessageBox.Show(CS.ToString("X2"));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                int intNextRunNum = int.Parse(txtRunNum.Text) + 1;
                txtRunNum.Text = intNextRunNum.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

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
