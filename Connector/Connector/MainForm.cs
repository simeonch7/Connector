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
        public static List<String> queryInfoList = new List<String>();
        public static List<String> colsNameList = new List<String>();

        String querySQL = "SELECT * FROM Goods";
        String separator = "^";

        public MainForm()
        {
            InitializeComponent();
            label2.Text = "This Service's IP: " + getIPAddress();
            Console.WriteLine("starting");
            Listener.listen();
/*            StartServer();*/
        }

        private void ExecuteRequest(String IP, String DBName, String userName, String DBPassword, bool isMssql)
        {
            SqlConnection conn = new SqlConnection("Server=" + IP + "; " + "Database=" + DBName + "; " + "User Id=" + userName + "; " + "Password=" + DBPassword + ";");
            try
            {
                conn.Open();

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
                reader.Close();
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not open Connection!", "Oops!");
            }
        }

        public List<String> ReturnReportsInformation()
        {
            return queryInfoList;
        }

        public List<String> ReturnReportsColumnNames()
        {
            return colsNameList;
        }


        private void Button1_Click(object sender, EventArgs e)
        {
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
            bool isMssql = false;
            if (radioButton1.Checked == true)
            {
                isMssql = true;
            }
            ExecuteRequest(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, isMssql);
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static String getIPAddress()
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


       /* public static void StartServer()
        {
            var httpListener = new HttpListener();
            Listener server = new Listener(httpListener, "http://localhost:4444/", ProcessYourResponse);
            server.Start();
        }

        public static byte[] ProcessYourResponse(string test)
        {
            Console.WriteLine(test);
            return new byte[0]; // TODO when you want return some response
        }
*/

    }
}
