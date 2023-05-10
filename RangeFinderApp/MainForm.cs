using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangeFinderApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //create the serial connection
        private SerialPort sp_1 = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);//

        private Boolean blnMeasuring = false;

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sp_1.IsOpen) {

                    //sp_1.RtsEnable = true;

                    sp_1.DataReceived += new SerialDataReceivedEventHandler(sp_1_DataReceived);
                    //sp_1.ErrorReceived += new SerialErrorReceivedEventHandler(sp_1_ErrorReceived);

                    sp_1.Open();
                    
                    
                    byte[] buffer = { 0x80, 0x06, 0x03, 0x77 };
                    sp_1.Write(buffer, 0, 4);

                    blnMeasuring = true;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Could not open range finder port: " + ex.Message, "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            try
            {

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void chkStoreInDB_CheckedChanged(object sender, EventArgs e)
        {

        }

        public static void SetControlText(Control ctl, string text)
        {
            ctl.Invoke((MethodInvoker)delegate { ctl.Text = text; });
        }

        public static void SetRTBText(Control rtb, string text)
        {
            rtb.Invoke((MethodInvoker)delegate { rtb.Text = text; });
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            blnMeasuring = false;
        }
        //private void sp_1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        //{
        //    string strReceived = sp_1.ReadLine().Trim();
        //    txtData.Text = strReceived;
        //}
        private void sp_1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {

                if (!blnMeasuring) { return; }

                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();

                
                
                


                


                string[] parts = indata.Split('?'); //? -? 000.442 ?

                string strDist = parts[2].Trim();


                SetRTBText(rtb, strDist);

                if (double.TryParse(strDist, out double val))
                {
                    SetControlText(lblRange,val.ToString());
                }


                //string strReceived = sp_1.ReadLine().Trim();
                //txtData.Text = indata;
                //SetRTBText(rtb, strReceived);
                //if (strReceived.StartsWith("{") && strReceived.EndsWith("}"))
                //{
                //    //SetRTBText(rtb, strReceived + Environment.NewLine);
                //    String[] arrayValue = strReceived.Trim().TrimStart('{').TrimEnd('}').Split(',');
                //    string strRange = arrayValue[1];
                //    String[] arrayRange = strRange.Trim().Split('|');
                //    string strRangeValue = arrayRange[1];
                //    SetControlText(this.meta_id_20, strRangeValue);

                //    if (m_swRangeDataFile != null)
                //    {
                //        m_swRangeDataFile.WriteLine(strReceived);
                //        m_swRangeDataFile.Flush();
                //    }

                //    //do the insert into the postgres realtime table here
                //    RealtimeInsertIntoDT(strReceived, 3, m_listRealtimeDataTableColDefs);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
