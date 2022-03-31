using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {
        public Client()
        {

        }

        public void StartClient()
        {
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint remoteEndpoint = 
                new IPEndPoint(ipAddress,11000);

            Socket socket =
                new Socket(ipAddress.AddressFamily,SocketType.Stream,ProtocolType.Tcp);

            try
            {
                socket.Connect(remoteEndpoint);
                Console.WriteLine("Socket connected to " + socket.RemoteEndPoint.ToString());

                NetworkStream stream = new NetworkStream(socket);
                StreamReader sr = new StreamReader(stream);
                StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true};
                string line = "";
                bool shutdown = false;
                do
                {
                    string input = Console.ReadLine();
                    sw.WriteLine(input);
                    Console.WriteLine("# From server: ");
                    do
                    {
                        line = sr.ReadLine();
                        if (line.Equals("EXIT"))
                        {
                            shutdown = true;
                            break;
                        }
                        if(!line.Equals("END"))
                            Console.WriteLine(line);
                    } while (!line.Equals("END"));

                } while (!shutdown);

                sw.Close();
                sr.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
