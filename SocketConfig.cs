using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClientServer
{
    public static class ReturnCode
    {
        private static Dictionary<int, string> errors = new Dictionary<int, string>();
        public static Dictionary<int, string> Errors { get => errors; set => errors = value; }

        static ReturnCode()
        {
            errors.Add(100, "Succes");
            errors.Add(-100, "No byte send");
            errors.Add(-101, "No byte received");
            errors.Add(-200, "An error occur: ");
        }

    }
    class SocketConfig
    {
        private String address;
        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        private int nbrToListen=0;
        public int NbrToListen
        {
            get { return nbrToListen; }
            set { nbrToListen = value; }
        }


        static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream(arrBytes))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                Console.WriteLine(Convert.ToString(arrBytes.Length));
                // memStream.Write(arrBytes, 0, arrBytes.Length);
                // Console.WriteLine(memStream.Length);
                // memStream.Seek(0, SeekOrigin.Begin);
                memStream.Position = 0;
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;

                // Write the first string to the stream.
                memStream.Write(arrBytes, 0, arrBytes.Length);
                Console.WriteLine(
                    "Capacity = {0}, Length = {1}, Position = {2}\n",
                    memStream.Capacity.ToString(),
                    memStream.Length.ToString(),
                    memStream.Position.ToString());

                // Write the second string to the stream, byte by byte.
                memStream.Write(arrBytes);

                // Write the stream properties to the console.
                Console.WriteLine(
                    "Capacity = {0}, Length = {1}, Position = {2}\n",
                    memStream.Capacity.ToString(),
                    memStream.Length.ToString(),
                    memStream.Position.ToString());

                // Set the position to the beginning of the stream.
                memStream.Seek(0, SeekOrigin.Begin);
                return null;
            }
        }

        static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public Socket Connect()
        {
            // Créer une socket s – AdressFamily InterNetwork, SocketType Stream et ProtocoleType Tcp
            Socket s = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            // Créer un IPEndPoint, par exemple, sur l’adresse 127.0.0.1:9050 
            IPEndPoint endPoint = new IPEndPoint(
                IPAddress.Parse(address),
                port
            );
            if (nbrToListen > 0)
            {
                // Associer l’IPEndPoint à la socket
                s.Bind(endPoint);

                // Mettre la socket en écoute
                s.Listen(nbrToListen); //Nbr de bye à écouter

                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("Serveur disponible à l'écoute ...");
                // La fonction doit retourner la socket
                return s;
            }
            else
            {
                // Essayer de se connecter au serveur avec l’IPEndPoint  
                // Attention -  N’oubliez pas l’utilisation de try/catch !
                try
                {
                    s.Connect(endPoint);

                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine("Connecté ...");
                    return s;
                }
                catch (Exception e)
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            
        }

        public Socket AllowConnection(Socket listener)
        {
            Socket clientRemote = listener.Accept();

            /*IPEndPoint clientRemoteEndpoint = clientRemote.RemoteEndPoint as IPEndPoint;

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Connecté avec l'adresse {0} et port {1}", clientRemoteEndpoint.Address, clientRemoteEndpoint.Port);*/

            // Retourner la nouvelle socket établie
            return clientRemote;
        }

        public int SendMessage(Socket remote, object msg)
        {
            byte[] data = ObjectToByteArray(msg);
            try
            {
                int byteCount = remote.Send(data);
                if (byteCount > 0)
                    return 100;
                else
                    return -100;

            }
            catch (Exception e)
            {
                ReturnCode.Errors.Remove(-200);
                ReturnCode.Errors.Add(-200, e.Message);
                return -200;
            }
        }

        public object Receive(Socket remote)
        {
            byte[] buffer = new byte[1024];
            try
            {
                int byteCount = remote.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                object received = ByteArrayToObject(buffer);
                if (byteCount > 0)
                    return received;
                else
                    return -101;
            }
            catch (Exception e)
            {
                ReturnCode.Errors.Remove(-200);
                ReturnCode.Errors.Add(-200, e.Message);
                return -200;
            }
        }


        public void Send(Socket socket, String msg)
        {
            byte[] data;
            data = Encoding.UTF8.GetBytes(msg);
            try
            {
                int byteCount = socket.Send(data);
                /*Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("Sent {0} bytes.", byteCount);*/

            }
            catch (Exception e)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine(e.Message);
            }
        }

        public String ReceiveMessage(Socket socket)
        {
            byte[] buffer = new byte[1024];
            try
            {
                int byteCount = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                String received = Encoding.UTF8.GetString(buffer, 0, byteCount);
                if (byteCount > 0)
                {
                    return received;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
        public int Disconnect(Socket remote)
        {
            try
            {
                remote.Close();
                return 100;
            }
            catch (Exception e)
            {
                ReturnCode.Errors.Remove(-200);
                ReturnCode.Errors.Add(-200, e.Message);
                return -200;
            }
        }

    }
}
