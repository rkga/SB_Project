using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;


namespace StudioBoss
{
    public partial class StudioBoss : Form
    {
        public StudioBoss()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == true) {
               
                panel1.Visible = true;
                panel2.Visible = false;
                return;

            }
            if (panel1.Visible == false) {
                panel1.Visible = true;
            }
            else {
                panel1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true) {
                panel1.Visible = false;
                panel2.Visible = true;
                return;
            }
            if (panel2.Visible == false) {
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
            }

            try
            {
                panel12.Location = new Point((panel2.Width / 2) - (panel12.Width / 2), (panel2.Height / 2) - (panel12.Height / 2) - panel2.Location.Y);
            }
            catch
            {
            }
            try
            {
                panel13.Location = new Point((panel1.Width / 2) - (panel13.Width / 2), (panel1.Height / 2) - (panel13.Height / 2) - panel1.Location.Y);
            }
            catch
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (button3.BackColor == SystemColors.Control)
            //{
            //    button3.BackColor = SystemColors.ButtonHighlight;
            //}
            //else {
            //    button3.BackColor = SystemColors.Control;
            //}
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_VisibleChanged(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            {
                ////timer1.Enabled = true;
            }
            else { 
                ////timer1.Enabled = false;
                ////button3.BackColor = SystemColors.Control;
            }
        }

        public void make_green_dot_point()
        {
            pictureBox1.Image = new Bitmap(Properties.Resources.gd);
            label2.Text = @"Online, Scanner Paired";
            pictureBox1.Visible = true;
            label2.Visible = true;
            button1.Visible = false;

            button3.Visible = false;
            button17.Visible = true;
        }
        public void make_red_dot_point()
        {
            pictureBox1.Image = new Bitmap(Properties.Resources._480px_Red_Point);
            label2.Text = @"Scanner not configured";
            pictureBox1.Visible = false;
            label2.Visible = false;
            button1.Visible = true;

            button3.Visible = true;
            button17.Visible = false;

        }
        public void Rediscover_Start() {
            make_red_dot_point();
            EventArgs ee = new EventArgs();
            button3_Click(this, ee);
        }

        frmScannerApp sa = new frmScannerApp();
        public bool is_silent_clicked_event=false;
        private void button3_Click(object sender, EventArgs e)
        {
           // frmScannerApp sa = new frmScannerApp();

            sa.Show();

            //sa.init_scann();

            //sa.Show();
            make_red_dot_point();


            sa.frmScannerApp_Load_PANDAN(); //load parameter
            sa.btnGetScanners_Click_PANDAN(); //discover rediscover scanners
            try
            {
                sa.combSlcrScnr_SelectedIndexChanged_PANDAN(); // if found scanner select it, set it as USB HID, and claim it
            }
            catch {
            }

           // chkShmSilentSwitch

           // frmScannerApp mw = new frmScannerApp();
            sa.Owner = this;
            if (sa.is_claimed == true)
            {
                make_green_dot_point();
                Properties.Settings.Default.retain_pair = @"True";
                Properties.Settings.Default.Save();
                panel2.Visible = false;

            }


        }
        public void fill_grid_bars(string ascii_string) {
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = ascii_string;
            row.Cells[1].Value = DateTime.Now;
            dataGridView1.Rows.Add(row);

        }


        public void GetScannedBarcodeXMLandTurnItIntoBarcode(string xml_data)
        {
            //  string s = xml_data.Replace(" ", ",");
            //  string[] res = s.Split(new string[] { "</datalabel>" }, StringSplitOptions.None);
            //  string new_s = res[0];
            //  string[] res_final = new_s.Split(new string[] { "<datalabel>" }, StringSplitOptions.None);
      //      try
      //      {
              string[] res = xml_data.Split(new string[] { "</datalabel>" }, StringSplitOptions.None);
              string new_s = res[0];
              string[] res_final = new_s.Split(new string[] { "<datalabel>" }, StringSplitOptions.None);
              string s = res_final[1].Replace(" ", ",");

            byte[] dt = StringToByteArrayFastest(s);


            string ascii = Encoding.ASCII.GetString(dt);
            // MessageBox.Show(ascii);


                // THIS HERE ENTERS THE DATA TO MEMORY - IT QUEUES IT AS ITS OWN OBJECT
          //  DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
           // row.Cells[0].Value = ascii;
           // row.Cells[1].Value = DateTime.Now;
           // dataGridView1.Rows.Add(row);
                make_row_data(ascii);
              //  SaveDataToXML();

            //      }
            //      catch {
            //       }




        }
 DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int unsucc_index = 0;
        private void make_row_data(string brcde)
        {
            //create a data set
    
            //create a data table for the data set

            //create some columns for the datatable

            //  dc3.DataType = typeof(DataGridViewCheckBoxCell); 
            //add the columns to the datatable


            //  dt.Columns.Add(dc3);



            DataRow dr = dt.NewRow();
            dr[0] = brcde;
            dr[1] = DateTime.Now;
            string successful_to_web = "Complete";



            new Thread(() => send_barcode_to_web(textBox1.Text, brcde, (DateTime)dr[1], ref successful_to_web, unsucc_index )).Start();
            //bool successful_to_web = send_barcode_to_web(textBox1.Text, brcde, (DateTime)dr[1]);

            dr[2] = successful_to_web;
         //   DataGridViewCheckBoxCell ck = new DataGridViewCheckBoxCell();
         //   ck.ThreeState= false;

         //   ck.Value = true;
         //   dr[2] = ck;
            dt.Rows.Add(dr);


            try
            {
                ds.Tables.Add(dt);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch
            {
                dataGridView1.DataSource = ds.Tables[0];

            }
            finally { }

            //add the datatable to the datasource





            //  dt.Columns.Add(new DataColumn("", typeof(System.Web.UI.WebControls.CheckBox)));
            // j += 1;
            //  CheckBox ck = new CheckBox();
            //  ck.Checked = true;
            //    dr[role.RoleName] = ck;
            // dataGridView1.Columns[2].ValueType.

            //make this data the datasource of our gridview



            dataGridView1.Columns[0].Width = 230;
            dataGridView1.Columns[1].Width = 230;
            dataGridView1.Columns[2].Width = 60;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13F);
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;

            try {
                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];
            } catch { }
            

            SaveDataToXML();

            scrollbar1.Visible = true;
            panel15.Visible = true;


            //  dataGridView1.Invalidate();
            // dataGridView1.Refresh();
        }

        //THIS MAKES THE DATAGRID OBJECTS PERSISTENT BY LOADING THEM INTO PHYSICAL MEMORY
        private void SaveDataToXML()
        {
           // DataTable dt = (DataTable)dataGridView1.DataSource;
            try
            {
                ds.WriteXmlSchema(Application.StartupPath + @"\BarCodesQueueSchema.xml");
                ds.WriteXml(Application.StartupPath + @"\BarCodesQueue.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ReadDataFromXML()
        {
            
          //  dataGridView1.DataSource = ds.Tables[0];
            try
            {
                ds.ReadXmlSchema(Application.StartupPath + @"\BarCodesQueueSchema.xml");
                ds.ReadXml(Application.StartupPath + @"\BarCodesQueue.xml");
                dataGridView1.DataSource = ds.Tables[0];
                 dt = (DataTable)dataGridView1.DataSource;


                dataGridView1.Columns[0].Width = 230;
                dataGridView1.Columns[1].Width = 230;
                dataGridView1.Columns[2].Width = 60;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 13F);
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;

                dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public static byte[] StringToByteArrayFastest(string hex)
        {
            string[] sa = hex.Split(new string[] { "x" }, StringSplitOptions.None);
            byte[] arr = new byte[sa.Length - 1];
            sa = sa.Where(w => w != sa[0]).ToArray();
            for (int i = 0; i < sa.Length; ++i)
            {
                //  arr[i] = sa[i]; //((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
                // MessageBox.Show(sa[i].Split(new string[] { "," }, StringSplitOptions.None)[0]);
                //sa[i].Split(new string[] { "," }, StringSplitOptions.None)[0].ToString()
                //  arr[i] = Convert.ToByte(int.Parse(sa[i].Split(new string[] { "," }, StringSplitOptions.None)[0])); //Int32.Parse("11");
                arr[i] = Convert.ToByte(Convert.ToInt32(int.Parse(sa[i].Split(new string[] { "," }, StringSplitOptions.None)[0]).ToString(), 16)); //Int32.Parse("11");
                                                                                                                                                   // int decValue = Convert.ToInt32(int.Parse(sa[i].Split(new string[] { "," }, StringSplitOptions.None)[0]).ToString(), 16);
            }
            return arr;
        }


        //CONVERT HEX ARRAY TO STRING-------------------------------------------------------------------------
        //byte[] data = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };
        //string ascii = Encoding.ASCII.GetString(data);
        //MessageBox.Show(ascii);
        //----------------------------------------------------------------------------------------------------

        private void button4_Click(object sender, EventArgs e)
        {
            frmScannerApp sa = new frmScannerApp();

            sa.Show();
        }


        public static bool CheckForInternetConnectionIsItActive() {
            try {
                using (var clnt = new WebClient())
                {
                    using (clnt.OpenRead("https://mystudioboss.com/")) {
                        return true;
                    }
                }
            } catch {
                return false;
            }
            return false;
        }

        bool api_connected = false;
        string api_name = "";
      
        public void Verify_Location_API(string api_key_string, ref bool r_result)
        {

            //   string endPoint = @"https://mystudioboss.com/api/1.1/wf/verify_key";
            //   var client = new RestClient(endPoint, HttpVerb.POST);
            //   var json = client.MakeRequest("?api_key=vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN");
            //   MessageBox.Show(json);

            if (api_key_string == "") { return; }


            try {
                using (var wb = new WebClient())
                {
                    var data1 = new NameValueCollection();
                    data1["api_key"] = api_key_string; // textBox1.Text; // "vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN";


                    var response1 = wb.UploadValues("https://mystudioboss.com/api/1.1/wf/verify_key", "POST", data1);
                    string s = System.Text.Encoding.UTF8.GetString(response1, 0, response1.Length);
                    string res = s.Split(new string[] { "location" }, StringSplitOptions.None)[1].Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)[0].Split(new string[] { ":" }, StringSplitOptions.None)[1].Trim().Trim('"').ToString();
                    api_name = res;
                    //  string sdsd = res[1].Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)[0].Split(new string[] { ":" }, StringSplitOptions.None)[1].ToString();
                    //MessageBox.Show(res);
                    //////////////label1.Text = res;
                    //////////////pictureBox3.Image = new Bitmap(Properties.Resources.rest_api_icon_online);
                    api_connected = true;
                    r_result = true;
                }
            }
            catch {
                ////////////////label1.Text = "No Linked Studio";
                ////////////////pictureBox3.Image = new Bitmap(Properties.Resources.rest_api_icon_broken);
                api_connected = false;
                r_result = false;
            }



            //  var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://mystudioboss.com/api/1.1/wf/verify_key?api_key=vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN");
            //     httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            //vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN
            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = new JavaScriptSerializer().Serialize(new { Barcode = "12345678901", TimeScanned = "13-10-2017 00:00:00" });
            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //    var answer_response = (HttpWebResponse)httpWebRequest.GetResponse();
            //    using (var streamReader = new StreamReader(answer_response.GetResponseStream()))
            //    {
            //        var server_res = streamReader.ReadToEnd();
            //        MessageBox.Show(server_res.ToString());
            //    }

        }

        string[] un_successful_barcode;
        string[] un_successful_tmpstmp;
  


        public bool send_barcode_to_web(string api_key, string bar_code, DateTime time_stamp, ref string retAnswer, int unsuccesfull_index)
        {

            //   string endPoint = @"https://mystudioboss.com/api/1.1/wf/verify_key";
            //   var client = new RestClient(endPoint, HttpVerb.POST);
            //   var json = client.MakeRequest("?api_key=vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN");
            //   MessageBox.Show(json);

            try {
                using (var wb = new WebClient())
                {
                    var data1 = new NameValueCollection();
                    data1["api_key"] = api_key; // "vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN";
                    data1["barcode_string"] = bar_code; // "barcode_string";
                    data1["timestamp"] = time_stamp.ToString("yyyy/MM/dd hh:mm:ss tt"); //Convert.ToDateTime(time_stamp); // "vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN";


                    var response1 = wb.UploadValues("https://mystudioboss.com/api/1.1/wf/verify_scan", "POST", data1);
                    string s = System.Text.Encoding.UTF8.GetString(response1, 0, response1.Length);
                    string res = s.Split(new string[] { "response" }, StringSplitOptions.None)[2].Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)[0].Split(new string[] { ":" }, StringSplitOptions.None)[1].Trim().Trim('"').ToString();

                    StringComparison comp = StringComparison.Ordinal;
                    

                    if (res.ToLower().Contains("scan received") == true)
                    {
                        retAnswer = "Complete";
                        return true;
                    }
                    else {
                        unsucc_index += 1;
                        Array.Resize(ref un_successful_barcode, unsucc_index);
                        Array.Resize(ref un_successful_tmpstmp, unsucc_index);
                   
                        

                        un_successful_barcode[unsucc_index-1] = bar_code;
                        un_successful_tmpstmp[unsucc_index-1] = time_stamp.ToString("yyyy/MM/dd hh:mm:ss tt");
                     
                        retAnswer = "Pending...";
                        return false;
                    }

                    //  string sdsd = res[1].Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None)[0].Split(new string[] { ":" }, StringSplitOptions.None)[1].ToString();
                        //MessageBox.Show(res);
            
                }
            }
            catch {
                unsucc_index += 1;
                Array.Resize(ref un_successful_barcode, unsucc_index);
                Array.Resize(ref un_successful_tmpstmp, unsucc_index);
                
                

                un_successful_barcode[unsucc_index-1] = bar_code;
                un_successful_tmpstmp[unsucc_index-1] = time_stamp.ToString("yyyy/MM/dd hh:mm:ss tt");
               

                retAnswer = "Pending...";
                return false;
            }

 

            //retAnswer = false;
            return false;

            //  var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://mystudioboss.com/api/1.1/wf/verify_key?api_key=vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN");
            //     httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            //vU9dVLj15DouvAVaoLotoX15jA7Lv7NTvLEN
            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = new JavaScriptSerializer().Serialize(new { Barcode = "12345678901", TimeScanned = "13-10-2017 00:00:00" });
            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //    var answer_response = (HttpWebResponse)httpWebRequest.GetResponse();
            //    using (var streamReader = new StreamReader(answer_response.GetResponseStream()))
            //    {
            //        var server_res = streamReader.ReadToEnd();
            //        MessageBox.Show(server_res.ToString());
            //    }

        }


        private void button5_Click(object sender, EventArgs e)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.mocky.io/v2/59ee0dc93300002b00b5c824");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Post";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new { Barcode = "12345678901", TimeScanned = "13-10-2017 00:00:00" });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var answer_response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(answer_response.GetResponseStream())) {
                var server_res = streamReader.ReadToEnd();
                MessageBox.Show(server_res.ToString());
            }

        }
            string xmlpull = "<? ?>" + "  <outArgs>" +
"    <scannerID>2</scannerID>  " +
"    <arg-xml>" +
"      <scandata>" +
"        <modelnumber> DS670 - SR20001ZZR </modelnumber>" +
"        <serialnumber> 7116000501003 </serialnumber>" +
"        <GUID> A2E647DED2163545B18BCEBD0A2A133D </GUID>" +
"        <datatype> 8 </datatype>" +
"        <datalabel> 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39 0x31 0x32 </datalabel>" +
"           <rawdata> 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39 0x31 0x32 </rawdata>" +
"            </scandata>" +
"          </arg-xml>" +
"        </outArgs> ";
        private void button6_Click(object sender, EventArgs e)
        {


        GetScannedBarcodeXMLandTurnItIntoBarcode(xmlpull);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //if (System.Windows.Forms.Application.OpenForms["frmScannerApp"] != null)
            // {
            //(System.Windows.Forms.Application.OpenForms["frmScannerApp"] as frmScannerApp).trigger_barRead();
            //  }
            sa.exec_reboot_scanner();


        }



        private void StudioBoss_Load(object sender, EventArgs e)
        {
            //DataColumn dc = new DataColumn("Code (scanned ID)");
            //DataColumn dc2 = new DataColumn("Scan Time");
            //DataColumn dc3 = new DataColumn("Status");
            //dt.Columns.Add(dc);
            //dt.Columns.Add(dc2);

            //dataGridView1.Columns[0].Width = 230;
            //dataGridView1.Columns[1].Width = 230;

            this.AutoScaleMode = AutoScaleMode.Dpi;


            DataColumn dc = new DataColumn("Code (scanned ID)", typeof(String));
            DataColumn dc2 = new DataColumn("Scan Time", typeof(DateTime));
            DataColumn dc3 = new DataColumn("Status", typeof(String));

           
            dt.Columns.Add(dc);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);

            ReadDataFromXML();
            textBox1.Text = Properties.Settings.Default.API_STRING.Replace("Nan", "");
          //  checkBox1.Checked = Convert.ToBoolean(Properties.Settings.Default.auto_auth);

            if (checkBox1.Checked == true) {
                bool r=false;
                Verify_Location_API(textBox1.Text, ref r);
                timer2.Enabled = true;
                timer3.Enabled = true;
            }

            if (Convert.ToBoolean(Properties.Settings.Default.retain_pair) == true) {
                EventArgs ee = new EventArgs();
                button3_Click(this, ee);
            }

            //silent_barcode
            checkBox2.Checked = Convert.ToBoolean(Properties.Settings.Default.silent_barcode);

            try
            {
                if (dataGridView1.Rows.Count == 0) {
                    label10.Visible = true;
                    label10.Location = new Point((panel4.Width / 2) - (label10.Width / 2), (panel4.Height / 2) - (label10.Height / 2) - panel4.Location.Y);
                }
                else
                {
                    label10.Visible = false;
                }
            }
            catch
            {
            }
            dataGridView1.AutoResizeColumns();


            try {
                dataGridView1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[0].DefaultCellStyle.Padding = new Padding(37, 0, 0, 0);
                dataGridView1.Columns[0].HeaderCell.Style.Padding = new Padding(30, 0, 0, 0);

                dataGridView1.Columns[1].DefaultCellStyle.Padding = new Padding(35, 0, 0, 0);
                dataGridView1.Columns[1].HeaderCell.Style.Padding = new Padding(30, 0, 0, 0);

                dataGridView1.Columns[2].DefaultCellStyle.Padding = new Padding(35, 0, 0, 0);
                dataGridView1.Columns[2].HeaderCell.Style.Padding = new Padding(30, 0, 0, 0);
            } catch {
                scrollbar1.Visible = false;
                panel15.Visible = false;
            }
            // Configure the details DataGridView so that its columns automatically
            // adjust their widths when the data changes.


            panel1.Location = new Point(0, 0);
            panel2.Location = new Point(0, -1);

            panel1.Width = this.Width;
            panel2.Width = this.Width;
            panel1.Height = this.Height - panel5.Height- panel3.Height-39;
            panel2.Height = this.Height - panel5.Height- panel3.Height-37;

            try {
                scrollbar1.Height = panel14.Height+5;
                scrollbar1.ddraw();
                scrollbar1.Value = dataGridView1.VerticalScrollingOffset;
                scrollbar1.redraww();
            } catch { }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveDataToXML();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bool r = false;
            Verify_Location_API(textBox1.Text, ref r);
        }
  bool fired_from_visible_changed = false;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (fired_from_visible_changed == true) {
                if (textBox1.Text == Properties.Settings.Default.API_STRING)
                {
                    fired_from_visible_changed = false;
                    return;
                }
                else {
                    fired_from_visible_changed = false;
                    Properties.Settings.Default.API_STRING = textBox1.Text.ToString();
                    Properties.Settings.Default.Save();
                    return;
                }

            }
            Properties.Settings.Default.API_STRING = textBox1.Text.ToString();
            Properties.Settings.Default.Save();
        }
      
        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (panel1.Visible == true) {
                fired_from_visible_changed = true;
                textBox1.Text = Properties.Settings.Default.API_STRING;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            bool r = false;
            Verify_Location_API(textBox1.Text, ref r);
            timer2.Enabled = true;
            timer3.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.auto_auth = checkBox1.Checked.ToString();
            Properties.Settings.Default.Save();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            ReadDataFromXML();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true) {
                pictureBox4.BackgroundImage = new Bitmap(Properties.Resources.right_slide_button);

                try
                {
                    ds.Tables[0].DefaultView.RowFilter = "Status = 'Pending...'";
                    dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];
                }
                catch { }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                try {
                    ds.Tables[0].DefaultView.RowFilter = "Status = 'Complete'";
                    dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];
                }
                catch { }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                pictureBox4.BackgroundImage = new Bitmap(Properties.Resources.left_slide_button);
                try {
                    ds.Tables[0].DefaultView.RowFilter = "Status = 'Pending...' OR Status = 'Complete'";
                    dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];

                }
                catch { }
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                DataView view = new DataView(ds.Tables[0]);
                view.RowFilter = "Status = 'Complete'";

                foreach (DataRowView row in view)
                {
                    row.Delete();
                }
            }
            catch { }
                
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //for (int i = 0; i <= dataGridView1.Rows.Count -1; i++) {
            try
            {
                for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
                {
                    if (dataGridView1[2, j].Value == "Complete")
                    {
                        dataGridView1[2, j].ReadOnly = true;
                    }

                }

            }
            catch { }

            //}
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(dataGridView1.CurrentCell.RowIndex.ToString());  //.Value.ToString());
          //////////  string BARS_CODES = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
          //////////  string BARS_TIMESTMP = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
          //////////  bool successful_to_web = true;


          //////////  new Thread(() => send_barcode_to_web(textBox1.Text, BARS_CODES, Convert.ToDateTime(BARS_TIMESTMP), ref successful_to_web, unsucc_index)).Start();
          //////////  //bool successful_to_web = send_barcode_to_web(textBox1.Text, BARS_CODES, Convert.ToDateTime(BARS_TIMESTMP));
          //////////  if (successful_to_web == true)
          //////////  {
          //////////      SaveDataToXML();
          //////////      dataGridView1[2, dataGridView1.CurrentCell.RowIndex].ReadOnly = true;
          //////////  }
          //////////  else {

          //////////      //   dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value = false;
          //////////      //  (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2] as DataGridViewCheckBoxCell).Value = false;da
          //////////      dataGridView1.CancelEdit();
          //////////      dataGridView1.EndEdit();
          //////////      is_cancel_called = true;
          ////////////      ((DataGridViewCheckBoxCell)dataGridView1.CurrentRow.Cells[2]).Value = false;
          ////////// //     dataGridView1.Refresh();
          //////////  }
          ////////////  MessageBox.Show(successful_to_web.ToString());

        }

        bool is_cancel_called = false;

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

            try {
                if (e.ColumnIndex == 2 && e.RowIndex != -1)
                {
                    if (dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value.ToString() == "Complete") { return; }

                    if (dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value.ToString() == "Pending...") {
                        //MessageBox.Show(dataGridView1.CurrentCell.RowIndex.ToString());  //.Value.ToString());
                        string BARS_CODES = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                        string BARS_TIMESTMP = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
                        string successful_to_web = "Complete";


                        new Thread(() => send_barcode_to_web(textBox1.Text, BARS_CODES, Convert.ToDateTime(BARS_TIMESTMP), ref successful_to_web, unsucc_index)).Start();
                        //bool successful_to_web = send_barcode_to_web(textBox1.Text, BARS_CODES, Convert.ToDateTime(BARS_TIMESTMP));
                        if (successful_to_web == "Complete")
                        {
                            dataGridView1[2, dataGridView1.CurrentCell.RowIndex].ReadOnly = true;
                            dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value = "Complete";
                            SaveDataToXML();

                        }
                        else
                        {

                            //   dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value = false;
                            //  (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2] as DataGridViewCheckBoxCell).Value = false;da
                            dataGridView1.CancelEdit();
                            dataGridView1.EndEdit();
                            is_cancel_called = true;
                            //      ((DataGridViewCheckBoxCell)dataGridView1.CurrentRow.Cells[2]).Value = false;
                            //     dataGridView1.Refresh();
                        }
                        //  MessageBox.Show(successful_to_web.ToString());

                    }

                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
                    if (is_cancel_called == true)
                    {
                        is_cancel_called = false;
                        dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value = "Pending...";
                    }
                    // dataGridView1.EndEdit();

                    for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
                    {
                        if (dataGridView1[2, j].Value == "Complete")
                        {
                            dataGridView1[2, j].ReadOnly = true;
                        }
                        else
                        {
                            dataGridView1[2, j].ReadOnly = false;
                        }

                    }
                }

            }
            catch {
            }


            //////if (e.ColumnIndex == 2 && e.RowIndex != -1)
            //////{
            //////    if (Convert.ToBoolean(dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value) == true) { return; }

            //////    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.CurrentCellChange);
            //////    if (is_cancel_called == true)
            //////    {
            //////        is_cancel_called = false;
            //////        dataGridView1[2, dataGridView1.CurrentCell.RowIndex].Value = false;
            //////    }
            //////    // dataGridView1.EndEdit();

            //////    for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
            //////    {
            //////        if (Convert.ToBoolean(dataGridView1[2, j].Value) == true)
            //////        {
            //////            dataGridView1[2, j].ReadOnly = true;
            //////        }
            //////        else
            //////        {
            //////            dataGridView1[2, j].ReadOnly = false;
            //////        }

            //////    }
            //////}


        }

        private void StudioBoss_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                mark_failed_barcode_sendings();
            }
            catch { }

            Application.Exit();
        }

        private void button13_Click(object sender, EventArgs e)
        {

            try
            {
                string tmpScanData = xmlpull;

                //UpdateResults("Barcode Event fired");
                //ShowBarcodeLabel(tmpScanData);
                string bs = tmpScanData;
               // bs = IndentXmlString(tmpScanData);
                if (System.Windows.Forms.Application.OpenForms["StudioBoss"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["StudioBoss"] as StudioBoss).GetScannedBarcodeXMLandTurnItIntoBarcode(bs);
                }

               // if (txtBarcode.InvokeRequired)
              //  {
                //    txtBarcode.Invoke(new MethodInvoker(delegate
               //     {

               //         txtBarcode.Text = bs;

               //     }));
              //  }
            }
            catch (Exception)
            {
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.silent_barcode = checkBox2.Checked.ToString();
            Properties.Settings.Default.Save();
            sa.beeper_check_box_change(checkBox2.Checked);
            sa.SoundBeeper_PANDAN(sender, e);
            EventArgs ee = new EventArgs();
            button7_Click(this, ee);

        }

        private void checkBox2_MouseClick(object sender, MouseEventArgs e)
        {
            is_silent_clicked_event = true;
        }

        private void checkBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) {
                is_silent_clicked_event = true;

            }
        }

        bool ret = false;
        bool was_internet_down = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if (api_connected)
            {
                //  was_internet_down = false;
                //MessageBox.Show("ima");
                label1.Text = api_name;
                pictureBox3.Image = new Bitmap(Properties.Resources.rest_api_icon_online);
            }
            else
            {
                //MessageBox.Show("nema");
                label1.Text = "No Studio Configured";
                pictureBox3.Image = new Bitmap(Properties.Resources.rest_api_icon_broken);

                //  was_internet_down = true;
            }

        }


        private void button14_Click(object sender, EventArgs e)
        {
            //Func<bool> method = CheckForInternetConnectionIsItActive;
            //var res = method.BeginInvoke(null, null);
            //var ans = method.EndInvoke(res);
            //MessageBox.Show(ans.ToString());

            //           x = new CheckForInternetConnectionIsItActive(() => { }//);
            ////ret = false;

            ////new Thread(() => { ret = CheckForInternetConnectionIsItActive(); }).Start();
            if (un_successful_barcode != null) { 
                for (int i = 0; i < un_successful_barcode.Length -1; i++)
                {
                    for (int j = 0; j <= dataGridView1.Rows.Count - 1; j++)
                    {
                        if (dataGridView1[2, j].Value == "Complete")
                        {
                            dataGridView1[2, j].ReadOnly = true;
                        }
                        else
                        {
                            dataGridView1[2, j].ReadOnly = false;
                        }
                    }
                }
            }


        }

        public delegate void SimpleDelegate(string ak, ref bool rez);
        private void button15_Click(object sender, EventArgs e)
        {
            //if (ret)
            //{
            //    //  was_internet_down = false;
            //    //MessageBox.Show("ima");
            //    if (was_internet_down) {
            //        was_internet_down = false;
            //        //Verify_Location_API();
            bool r = false;
            ////////////SimpleDelegate simpleDelegate = new SimpleDelegate(Verify_Location_API);
            ////////////simpleDelegate(textBox1.Text, ref r);

            new Thread(() => Verify_Location_API(textBox1.Text, ref r)).Start();

            //        }
            //    }
            //    else
            //    {
            //        //MessageBox.Show("nema");

            //        was_internet_down = true;
            //        //  was_internet_down = true;
            //    }
            //    new Thread(() => { ret = CheckForInternetConnectionIsItActive(); }).Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            bool r = false;
            ////////////SimpleDelegate simpleDelegate = new SimpleDelegate(Verify_Location_API);
            ////////////simpleDelegate(textBox1.Text, ref r);

            new Thread(() => Verify_Location_API(textBox1.Text, ref r)).Start();
        }

        private void StudioBoss_Resize(object sender, EventArgs e)
        {
            panel1.Location = new Point(0, 0);
            panel2.Location = new Point(0, -1);

            panel1.Width = this.Width;
            panel2.Width = this.Width;
            panel1.Height = this.Height - panel5.Height - panel3.Height - 39;
            panel2.Height = this.Height - panel5.Height - panel3.Height - 37;

            try {
                label10.Location = new Point((panel4.Width / 2) - (label10.Width / 2), (panel4.Height / 2) - (label10.Height / 2) - panel4.Location.Y);
            }
            catch {
            }
            try
            {
                panel12.Location = new Point((panel2.Width / 2) - (panel12.Width / 2), (panel2.Height / 2) - (panel12.Height / 2) - panel2.Location.Y);
            }
            catch
            {
            }
            try
            {
                panel13.Location = new Point((panel1.Width / 2) - (panel13.Width / 2), (panel1.Height / 2) - (panel13.Height / 2) - panel1.Location.Y);
            }
            catch
            {
            }

            //    Scrollbar1.ddraw()
            //Scrollbar1.Value = DataGridView1.VerticalScrollingOffset

            try {
                //Scrollbar1.redraww()
                scrollbar1.Height = panel14.Height+5;
                scrollbar1.ddraw();
                scrollbar1.Value = dataGridView1.VerticalScrollingOffset;
                scrollbar1.redraww();
            } catch { }



        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true) {
                radioButton2.Checked = true;
                label15.Font = new Font(label15.Font, FontStyle.Regular );
                label16.Font = new Font(label16.Font, FontStyle.Bold );
                return;

            }
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = true;
                label16.Font = new Font(label16.Font, FontStyle.Regular);
                label15.Font = new Font(label15.Font, FontStyle.Bold);
                return;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            try
            {
                panel12.Location = new Point((panel2.Width / 2) - (panel12.Width / 2), (panel2.Height / 2) - (panel12.Height / 2) - panel2.Location.Y);
            }
            catch
            {
            }
            try
            {
                panel13.Location = new Point((panel1.Width / 2) - (panel13.Width / 2), (panel1.Height / 2) - (panel13.Height / 2) - panel1.Location.Y);
            }
            catch
            {
            }
        }

        private void pictureBox1_VisibleChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true) {
                button1.Visible = false;
                return;
            }
            if (pictureBox1.Visible == false) {
                button1.Visible = true;
                return;
            }
        }

        private void label2_VisibleChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true)
            {
                button1.Visible = false;
                return;
            }
            if (pictureBox1.Visible == false)
            {
                button1.Visible = true;
                return;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            label9_Click(sender, e);
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            // If Scrollbar1.block_scroll = True Then Exit Sub
            // Scrollbar1.Value = DataGridView1.VerticalScrollingOffset
            // Scrollbar1.redraww()

            try {
                if (scrollbar1.block_scroll == false) {
                    scrollbar1.Value = dataGridView1.VerticalScrollingOffset;
                    scrollbar1.redraww();

                }
            } catch { }


        }


        private VScrollBar vSBar;
        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //    Dim vv As Integer = 0
            //For Each ctrl As Control In DataGridView1.Controls
            //    If ctrl.GetType.ToString = "System.Windows.Forms.VScrollBar" Then
            //        Dim vsb As VScrollBar = DirectCast(ctrl, VScrollBar)
            //        'vsb.Value = 0
            //        vv = vsb.Maximum

            //    End If

            //Next

            //Scrollbar1.Maximum = vv

            try
            {
                int vv = 0;
                foreach (Control ctrl in dataGridView1.Controls)
                {
                    if (ctrl is VScrollBar)
                    {
                        vSBar = (VScrollBar)ctrl;
                        vv = vSBar.Maximum;

                    }
                }
                scrollbar1.Maximum = vv;
            }
            catch { }


        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try {
                if (dataGridView1.RowCount >= 1)
                {
                    label10.Visible = false;
                }
                else
                {
                    label10.Visible = true;
                }

            }
            catch { }

            try {
                int vv = 0;
                foreach (Control ctrl in dataGridView1.Controls)
                {
                    if (ctrl is VScrollBar)
                    {
                        vSBar = (VScrollBar)ctrl;
                        vv = vSBar.Maximum;

                    }
                }
                scrollbar1.Maximum = vv;
            } catch { }

        }

        private void scrollbar1_Scroll()
        {
            try {
                PropertyInfo verticalOffset = dataGridView1.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                verticalOffset.SetValue(this.dataGridView1, scrollbar1.Value, null);
            } catch { }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                return;
            }
            if (panel2.Visible == false)
            {
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
            }

            try
            {
                panel12.Location = new Point((panel2.Width / 2) - (panel12.Width / 2), (panel2.Height / 2) - (panel12.Height / 2) - panel2.Location.Y);
            }
            catch
            {
            }
            try
            {
                panel13.Location = new Point((panel1.Width / 2) - (panel13.Width / 2), (panel1.Height / 2) - (panel13.Height / 2) - panel1.Location.Y);
            }
            catch
            {
            }
        }


        private void mark_failed_barcode_sendings()
        {
            // un_successful_barcode[unsucc_index - 1] = bar_code;
            // un_successful_tmpstmp[unsucc_index - 1] = time_stamp.ToString("yyyy/MM/dd hh:mm:ss tt");
            for (long ji = 0; (ji <= un_successful_barcode.Length - 1); ji++)
            {
                if (un_successful_barcode[ji] == "Del") { continue; }
                try
                {
                    ds.Tables[0].DefaultView.RowFilter = "[Code (scanned ID)] = '" + un_successful_barcode[ji] + "' AND CONVERT([Scan Time], System.String) = '" + Convert.ToDateTime(un_successful_tmpstmp[ji]).ToString() + "'";
                    foreach (DataRowView rowView in ds.Tables[0].DefaultView)
                    {
                        rowView.BeginEdit();
                        rowView["Status"] = "Pending...";
                        rowView.EndEdit();

                    }
                    un_successful_barcode[ji] = "Del";
                    ds.Tables[0].DefaultView.RowFilter = null;
                }
                catch (Exception ee)
                {
                }
            }
            SaveDataToXML();
            dataGridView1.CurrentCell = dataGridView1[0, dataGridView1.Rows.Count - 2];

        }
        private void button18_Click(object sender, EventArgs e)
        {
            try {
                mark_failed_barcode_sendings();
            }
            catch { }



        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            try
            {
                mark_failed_barcode_sendings();
            }
            catch { }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            //////DataRow dr = dt.NewRow();
            //////dr[0] = "11";
            //////dr[1] = DateTime.Now;
            //////string successful_to_web = "Pending...";




            //////bool successful_to_web = send_barcode_to_web(textBox1.Text, brcde, (DateTime)dr[1]);

            //////dr[2] = successful_to_web;
            //////DataGridViewCheckBoxCell ck = new DataGridViewCheckBoxCell();
            //////ck.ThreeState = false;

            //////ck.Value = true;
            //////dr[2] = ck;
            //////dt.Rows.Add(dr);.

            //make_row_data("11");


            DataRow dr = dt.NewRow();
            dr[0] = "123";
            dr[1] = DateTime.Now;
            string successful_to_web = "Pending...";
            
            dr[2] = successful_to_web;
         
            dt.Rows.Add(dr);
            
            try
            {
                ds.Tables.Add(dt);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch
            {
                dataGridView1.DataSource = ds.Tables[0];

            }
            finally { }
        }
    }
}
