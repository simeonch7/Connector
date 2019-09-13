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
    /*    public delegate byte[] ProcessDataDelegate(string data);*/

    /* public class Listener
     {
         private const int HandlerThread = 2;
         private readonly ProcessDataDelegate handler;
         private readonly HttpListener listener;

         public Listener(HttpListener listener, string url, ProcessDataDelegate handler)
         {
             this.listener = listener;
             this.handler = handler;
             listener.Prefixes.Add("http://*:4444/" + Guid.NewGuid().ToString("D") + "/");
         }

         public void Start()
         {
             if (listener.IsListening)
                 return;
             listener.Start();

             for (int i = 0; i < HandlerThread; i++)
             {
                 listener.GetContextAsync().ContinueWith(ProcessRequestHandler);
             }
         }

         public void Stop()
         {
             if (listener.IsListening)
                 listener.Stop();
         }

         private void ProcessRequestHandler(Task<HttpListenerContext> result)
         {
             var context = result.Result;

             if (!listener.IsListening)
                 return;

             // Start new listener which replace this
             listener.GetContextAsync().ContinueWith(ProcessRequestHandler);

             // Read request
             string request = new StreamReader(context.Request.InputStream).ReadToEnd();

             // Prepare response
             var responseBytes = handler.Invoke(request);
             context.Response.ContentLength64 = responseBytes.Length;

             var output = context.Response.OutputStream;
             output.WriteAsync(responseBytes, 0, responseBytes.Length);
             output.Close();
         }
     }*/
    class Listener
    {
        public System.Net.HttpListenerResponse Response { get; }
        public static void listen()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse(MainForm.getIPAddress());

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = "";
                Console.Write("start loop ");
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = "";

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();


                    // Translate data bytes to a ASCII string.
                    var content = stream.Read(bytes, 0, bytes.Length);
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, content);
                    Console.WriteLine("Received: {0}", data);


                    var builder = new StringBuilder();
                    StringBuilder response = new StringBuilder();
                    response.AppendLine("XML");
                    response.AppendLine("ITEMS");
                    response.Append("HELLOS");
                    builder.Append("HTTP/1.1 200 OK\r\n");
                    builder.Append("\r\n" + response.ToString() + "\r\n");
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(builder.ToString());

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine(String.Format("Sent: {0}", builder.ToString()));
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }

    /* class Listener
     {
         public static void listen()
         {
             // Data buffer for incoming data.  
             byte[] bytes = new byte[1024];

             // Connect to a remote device.  
             try
             {
                 // Establish the remote endpoint for the socket.  
                 // This example uses port 11000 on the local computer.  
                 IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                 IPAddress ipAddress = IPAddress.Parse(MainForm.getIPAddress());

                 IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                 // Create a TCP/IP  socket.  
                 Socket sender = new Socket(ipAddress.AddressFamily,
                     SocketType.Stream, ProtocolType.Tcp);
                 // Connect the socket to the remote endpoint. Catch any errors.  
                 try
                 {
                     sender.Connect(remoteEP);

                     Console.WriteLine("Socket connected to {0}",
                         sender.RemoteEndPoint.ToString());

                     // Encode the data string into a byte array.  
                     byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                     // Send the data through the socket.  
                     int bytesSent = sender.Send(msg);

                     // Receive the response from the remote device.  
                     int bytesRec = sender.Receive(bytes);
                     Console.WriteLine("Echoed test = {0}",
                         Encoding.ASCII.GetString(bytes, 0, bytesRec));

                     // Release the socket.  
                     sender.Shutdown(SocketShutdown.Both);
                     sender.Close();

                 }
                 catch (ArgumentNullException ane)
                 {
                     Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                 }
                 catch (SocketException se)
                 {
                     Console.WriteLine("SocketException : {0}", se.ToString());
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine("Unexpected exception : {0}", e.ToString());
                 }

             }
             catch (Exception e)
             {
                 Console.WriteLine(e.ToString());
             }
         }
     }
 */
}
