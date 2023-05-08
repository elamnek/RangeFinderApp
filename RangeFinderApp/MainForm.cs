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
        private SerialPort sp_1 = new SerialPort("COM6", 9600);//,Parity.None,8,StopBits.One

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!sp_1.IsOpen) { sp_1.Open(); }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Could not open range finder port: " + ex.Message, "Port Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            try
            {

                byte[] buffer = { 0x80, 0x06, 0x02, 0x78 };
                sp_1.Write(buffer, 0, 4);
                //sp_1.WriteLine("/n");

                System.Threading.Thread.Sleep(1000);

                int btr = sp_1.BytesToRead;
                if (btr > 0)
                {
                    int[] data = new int[btr];

                    for (int i = 0; i < btr - 1; i++)
                    {
                        data[i] = sp_1.ReadByte();
                    }

                    if (data[3] == 'E' && data[4] == 'R' && data[5] == 'R')
                    {
                        //serialRemote.println("Out of range");
                        lblRange.Text = "ZZ";
                    }
                    else
                    {
                        //float distance = 0;
                        double dblDistance = (data[3] - 0x30) * 100 + (data[4] - 0x30) * 10 + (data[5] - 0x30) * 1 + (data[7] - 0x30) * 0.1 + (data[8] - 0x30) * 0.01 + (data[9] - 0x30) * 0.001;

                        lblRange.Text =  dblDistance.ToString();

                        //DateTime now = rtc.now();
                        //m_strSend = "{13|" + String(now.hour()) + ':' + String(now.minute()) + ':' + String(now.second()) + " " + String(now.day()) + '/' + String(now.month()) + '/' + String(now.year()) + ",20|" + String(distance) + "}";
                        //serialRemote.println(m_strSend);

                        //add this so data send rate to remote is around 4Hz
                        // delay(200);
                    }

                    sp_1.DiscardInBuffer();


                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void chkStoreInDB_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
