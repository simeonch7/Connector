using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connector
{

    public partial class MainForm : Form
    {
        static bool isMssql = false;

        public static List<String> queryInfoList = new List<String>();
        public static List<String> colsNameList = new List<String>();

        const String  separator = "^";

        public MainForm()
        {
            InitializeComponent();
            Console.WriteLine("starting");
            label2.Text = "This Service\'s IP: " + GetIPAddress();
        }

        public List<string> ReturnReportsInformation()
        {
            return queryInfoList;
        }

        public List<string> ReturnReportsColumnNames()
        {
            return colsNameList;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Listener.Listen();
            foreach (Control child in this.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox != null)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        MessageBox.Show("Please fill all the information we need!");
                        return;
                    }
                }
            }
            if (radioButton1.Checked == true)
            {
                isMssql = true;
            }
            ExecuteRequest();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void ExecuteRequest()
        {
            DBConnect(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, isMssql);
        }

        private void DBConnect(string IP, string DBName, string userName, string DBPassword, bool isMssql)
        {

            string querySQL;

            SqlConnection conn = new SqlConnection("Server=" + IP + "; " + "Database=" + DBName + "; " + "User Id=" + userName + "; " + "Password=" + DBPassword + ";");
            try
            {
                conn.Open();
                
                querySQL = XMLBuilder.XMLBuild(isMssql);


                SqlCommand cmd = new SqlCommand(querySQL, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colsNameList.Add(reader.GetName(i));
                }
                while (reader.Read())
                {
                    string CurrentRow = "";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        TypeCode typeCodeOfInfo = Type.GetTypeCode(reader.GetValue(i).GetType());
                        switch (typeCodeOfInfo)
                        {
                            case TypeCode.Int32:
                                CurrentRow += reader.GetInt32(i) + separator;
                                break;
                            case TypeCode.String:
                                CurrentRow += reader.GetString(i) + separator;
                                break;
                            case TypeCode.DateTime:
                                CurrentRow += reader.GetDateTime(i) + separator;
                                break;
                        }
                    }
                    queryInfoList.Add(CurrentRow);
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not open Connection!", "Oops!");
            }
            conn.Close();
        }

        public static string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}