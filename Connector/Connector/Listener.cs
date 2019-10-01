using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Connector
{
    class Listener
    {
        public System.Net.HttpListenerResponse Response { get; }
        public static void Listen()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13050;
                IPAddress localAddr = IPAddress.Parse(MainForm.GetIPAddress());
                server = new TcpListener(localAddr, port);

                server.Start();

                Byte[] bytes = new Byte[256];
                string data = "";

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = "";

                    NetworkStream stream = client.GetStream();

                    var content = stream.Read(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, content);
                    Console.WriteLine("Received: {0}", data);

                    var response = buildResponse();
                    
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(response.ToString());

                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine(String.Format("Sent: {0}", response.ToString()));

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }
        public static StringBuilder buildResponse() {
            var builder = new StringBuilder();
            StringBuilder response = new StringBuilder();
            response.AppendLine("XML - BUILD");
            builder.Append("HTTP/1.1 200 OK\r\n");
            builder.Append("\r\n" + response.ToString());

            return builder;
        }
    }
}