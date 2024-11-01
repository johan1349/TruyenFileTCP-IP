﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    internal class Client
    {
        public static string MessageCurrent = "Chọn file để gửi ";
        
        public static void SendFile(string fName)
        {
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint end = new IPEndPoint(ip, 999);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                string path = "";
                fName = fName.Replace("\\", "/");
                while (fName.IndexOf("/") > -1)
                {
                    path += fName.Substring(0, fName.IndexOf("/") + 1);
                    fName = fName.Substring(fName.IndexOf("/") + 1);
                }
                byte[] fNameByte = Encoding.ASCII.GetBytes(fName);
                MessageCurrent = "Buffering .....";
                byte[] fileData = File.ReadAllBytes(path + fName);
                byte[] clientData = new byte[4 + fNameByte.Length + fileData.Length];
                byte[] fNameLen = BitConverter.GetBytes(fNameByte.Length);
                fNameLen.CopyTo(clientData, 0);
                fNameByte.CopyTo(clientData, 4);
                fileData.CopyTo(clientData, 4 + fNameByte.Length);
                MessageCurrent = "Đang kết nối tới server";
                sock.Connect(end);
                MessageCurrent = "Đang gửi file";
                sock.Send(clientData);
                sock.Close();
                MessageCurrent = "File đã được gửi";
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageCurrent = "Access error: " + ex.Message;
            }
            catch (SocketException ex)
            {
                MessageCurrent = "Socket error: " + ex.Message;
            }
            catch (IOException ex)
            {
                MessageCurrent = "IO error: " + ex.Message;
            }
            catch (Exception ex)
            {
                MessageCurrent = "General error: " + ex.Message;
            }
        }
    }

}

