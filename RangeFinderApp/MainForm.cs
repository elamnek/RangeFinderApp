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

namespace RangeFinderApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //create the serial connection
        private SerialPort sp_1 = new SerialPort("COM7", 9600, Parity.None, 8, StopBits.One);

        private Boolean blnMeasuring = false;

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {

                blnMeasuring = true;


                if (!sp_1.IsOpen) {

                    sp_1.DataReceived += new SerialDataReceivedEventHandler(sp_1_DataReceived);
                    //sp_1.ErrorReceived += new SerialErrorReceivedEventHandler(sp_1_ErrorReceived);

                    sp_1.Open();
                    
                    
                    byte[] buffer = { 0x80, 0x06, 0x03, 0x77 };
                    sp_1.Write(buffer, 0, 4);

                   
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

                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                string[] parts = indata.Split('?'); //? -? 000.442 ?
                string strDist = parts[2].Trim();
                SetRTBText(rtb, strDist);
                if (double.TryParse(strDist, out double val))
                {
                    SetControlText(lblRange,val.ToString());
                }


                

                

                   //do the insert into the postgres realtime table here
                /    RealtimeInsertIntoDT(strReceived, 3, m_listRealtimeDataTableColDefs);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void RealtimeInsertIntoDT(string strReceived, int intDataChannel, ArrayList listColDefs)
        {
            try
            {

                if (listColDefs == null)
                {
                    return;
                }

                //read a line and split it up
                String strLine = strReceived.Trim().TrimStart('{').TrimEnd('}');
                String[] arrayLine = strLine.Split(new char[] { ',' }, StringSplitOptions.None);

                //get a hash of all data values
                string strTime = "";
                Hashtable hashInsertData = new Hashtable();
                foreach (string strDataValue in arrayLine)
                {

                    String[] arrayParts = strDataValue.Split(new char[] { '|' }, StringSplitOptions.None);
                    if (arrayParts.Length == 2)
                    {
                        string strMetaID = arrayParts[0].Trim();
                        string strValue = arrayParts[1].Trim();
                        int intMetaID = int.Parse(strMetaID);

                        if (intMetaID == 13)
                        {
                            int intMillis = -1;
                            if (strValue.Contains("#"))
                            {
                                //this is a sub second time with millis at the start - split this off
                                string[] arrayTime = strValue.Split(new char[] { '#' }, StringSplitOptions.None);
                                intMillis = int.Parse(arrayTime[0]);
                                strValue = arrayTime[1].Trim();
                            }


                            DateTime dteThis = DateTime.ParseExact(strValue, "H:m:s d/M/yyyy", null); //16:56:13 5/2/2023

                            //datetimes need to be inserted in the local timezone
                            //the postgres timestamptz field type is time zone aware and assumes any insert datetime is local
                            //it will store the actual datetime as utc, but whenever it is queried using sql the local time will be returned
                            //the postgres database timezone is stored against the postgres server properties and this is used to define
                            //what timezone the data is displayed in
                            strTime = "'" + dteThis.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            if (!hashInsertData.ContainsKey(intMetaID)) { hashInsertData.Add(intMetaID, strValue); }
                        }
                    }
                }


                if (strTime.Length > 0)
                {
                    ArrayList listSQLInserts = new ArrayList();

                    //this is a destination table
                    string strColumns = "time,data_channel";
                    string strValues = strTime + "," + intDataChannel.ToString();

                    //go through columns and build sql 
                    foreach (Hashtable hashColDef in listColDefs)
                    {
                        int intThisMetaID = (int)hashColDef["METADATAID"];
                        if (hashInsertData.ContainsKey(intThisMetaID))
                        {
                            //the log record contains this metadata id

                            string strValue = hashInsertData[intThisMetaID].ToString();
                            string strColName = hashColDef["DESTCOL"].ToString();
                            string strType = hashColDef["DESTCOLTYPE"].ToString();

                            if (strType == "str")
                            {
                                if (strValue.Length > 0)
                                {
                                    strValue = "'" + strValue + "'";
                                }
                            }
                            else if (strType == "int")
                            {
                                int intValue;
                                if (int.TryParse(strValue, out intValue))
                                {
                                    strValue = intValue.ToString();
                                }
                                else { strValue = ""; }
                            }
                            else if (strType == "dbl")
                            {
                                double dblValue;
                                if (double.TryParse(strValue, out dblValue))
                                {
                                    strValue = dblValue.ToString();
                                }
                                else { strValue = ""; }
                            }

                            if (strValue.Length > 0)
                            {

                                strColumns = strColumns + "," + strColName;
                                strValues = strValues + "," + strValue;
                            }
                        }

                    }

                    if (strColumns != "time")
                    {
                        //build sql insert for this table
                        listSQLInserts.Add("insert into dt_ts_realtime_data (" + strColumns + ")" + " values (" + strValues + ") ON CONFLICT DO NOTHING");
                    }


                    //do the inserts for this record (time point)
                    PGInsert(listSQLInserts);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void PGInsert(ArrayList listSQLInserts)
        {
            try
            {
                foreach (string strSQL in listSQLInserts)
                {
                    NpgsqlCommand commPG = new NpgsqlCommand(strSQL, m_connPG);
                    commPG.ExecuteNonQuery();
                    commPG.Dispose();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
