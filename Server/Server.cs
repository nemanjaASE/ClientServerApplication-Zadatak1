using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private static List<string> users = new List<string>();
        public Server()
        {
                
        }
        
        public void ServerListening()
        {
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndpoint = 
                new IPEndPoint(ipAddress,11000);

            Socket serverSocket =
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(localEndpoint);
                serverSocket.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket socket = serverSocket.Accept();

                    Task<int> task = Task.Factory.StartNew(() => Run(socket));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        private int Run(Socket socket)
        {
            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true};

            string[] delim = { ",", " ", "  " };
            try
            {
                string msg = "";
                do
                {
                    string line;
                    string[] tokens;
                    try
                    {
                        line = sr.ReadLine();

                        if (line != null)
                        {
                            tokens = line.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine("# From client: " + line);
                        }
                        else
                            return 0;
                    }
                    catch (Exception)
                    {
                        sw.Close();
                        sr.Close();
                        stream.Close();

                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        return -1;
                    }
                    msg = "";
                    if (tokens[0].Equals("EXIT"))
                    {
                        msg = "EXIT\n";
                    }else if(tokens[0].Contains("ADD") && tokens.Length == 2)
                    {
                        if(!users.Contains(tokens[1]))
                        {
                            users.Add(tokens[1]);
                            msg = "Korisnik "+ tokens[1] +" uspesno dodat.\n";
                        }
                        else
                        {
                            msg = "Korisnik " + tokens[1] + " vec postoji.\n";
                        }
                    }else if(tokens[0].Contains("LIST") && tokens.Length == 1)
                    {
                        if (users.Count == 0)
                            msg = "<empty list>\n";
                        foreach (string user in users)
                        {
                            msg += (user + "\n");
                        }
                        
                    }else if(tokens[0].Contains("REMOVE") && tokens.Length == 2)
                    {
                        if (users.Contains(tokens[1]))
                        {
                            users.Remove(tokens[1]);
                            msg = "Korisnik " + tokens[1] + " uspesno obrisan.\n";
                        }
                        else
                        {
                            msg = "Korisnik " + tokens[1] + " ne postoji.\n";
                        }
                    }else if (tokens[0].Contains("FIND") && tokens.Length == 2)
                    {
                        if (users.Contains(tokens[1]))
                        {
                            msg = "Korisnik " + tokens[1] + " je pronadjen.\n";
                        }
                        else
                        {
                            msg = "Korisnik " + tokens[1] + " ne postoji.\n";
                        }
                    }else if(tokens[0].Contains("ADD") && tokens.Length > 2)
                    {
                        string uspesno = "";
                        string neuspesno = "";
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            if(users.Contains(tokens[i]))
                            {
                                neuspesno +=  tokens[i] + " ";
                            }else
                            {
                                users.Add(tokens[i]);
                                uspesno +=  tokens[i] + " ";
                            }
                        }
                        if (uspesno.Equals(""))
                            uspesno = "<empty>";
                        if (neuspesno.Equals(""))
                            neuspesno = "<empty>";
                        msg = "Uspesno: " + uspesno + "\nNeuspesno: " + neuspesno + "\n";
                    }
                    else
                    {
                        msg = "Pogresna komanda!\n";
                    }

                    sw.WriteLine(msg + "END");

                } while (true);
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            sw.Close();
            sr.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            return 0;
        }
    }
}
