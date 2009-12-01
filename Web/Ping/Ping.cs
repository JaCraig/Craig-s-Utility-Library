/*
Copyright (c) 2009 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Net;
using System.Net.Sockets;
#endregion

namespace Utilities.Web.Ping
{
    /// <summary>
    /// Class used to ping another computer
    /// </summary>
    public static class Ping
    {
        #region Public Static Functions

        /// <summary>
        /// Does a ping against the host specified
        /// </summary>
        /// <param name="Address">Address of the host</param>
        /// <returns>True if a response is received, false otherwise</returns>
        public static bool PingHost(string Address)
        {
            IPHostEntry Server, From;
            Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
            try
            {
                Server = Dns.GetHostEntry(Address);
                From = Dns.GetHostEntry(Dns.GetHostName());
            }
            catch (Exception)
            {
                Socket.Close();
                throw new Exception("Can't find host");
            }

            EndPoint EndPoint = new IPEndPoint(Server.AddressList[0], 0);
            EndPoint EndPointFrom = new IPEndPoint(From.AddressList[0], 0);

            int PacketSize = 40;
            Packet Packet = new Packet();
            Packet.Type = 8;
            Packet.SubCode = 0;
            Packet.Check = 0;
            Packet.ID = 45;
            Packet.SequenceNumber = 0;
            int DataSize = 32;
            Packet.Data = new byte[DataSize];
            for (int x = 0; x < DataSize; ++x)
            {
                Packet.Data[x] = (byte)'#';
            }
            
            byte[] Buffer = Serialize(PacketSize, Packet);
            double CheckSumSize=System.Math.Ceiling((double)PacketSize/2);

            ushort []CheckSumBuffer=new ushort[(int)CheckSumSize];
            
            int BufferIndex=0;
            for(int x=0;x<(int)CheckSumSize;++x)
            {
                CheckSumBuffer[x]=BitConverter.ToUInt16(Buffer,BufferIndex);
                BufferIndex+=2;
            }
            Packet.Check = CheckSum(CheckSumBuffer);

            Buffer = Serialize(PacketSize, Packet);

            if (Socket.SendTo(Buffer, PacketSize, 0, EndPoint) == -1)
            {
                Socket.Close();
                throw new Exception("Couldn't send packet");
            }
            Buffer=new byte[256];
            int NumBytes = Socket.ReceiveFrom(Buffer, 256, SocketFlags.None, ref EndPointFrom);
            if (NumBytes == -1)
            {
                Socket.Close();
                throw new Exception("Host not responding");
            }
            else if (NumBytes > 0)
            {
                Socket.Close();
                return true;
            }

            Socket.Close();
            return false;
        }

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Does a check sum of an array
        /// </summary>
        /// <param name="CheckSumBuffer">Buffer to do a check sum on</param>
        /// <returns>The check sum of the buffer</returns>
        private static ushort CheckSum(ushort[] CheckSumBuffer)
        {
            Int32 Sum = 0;
            for (int x = 0; x < CheckSumBuffer.Length; ++x)
            {
                Sum += Convert.ToInt32(CheckSumBuffer[x]);
            }
            Sum = (Sum >> 16) + (Sum & 0xffff);
            Sum += (Sum >> 16);
            return (UInt16)(~Sum);
        }

        /// <summary>
        /// Serializes the packet into an array
        /// </summary>
        /// <param name="PacketSize">Size of the packet</param>
        /// <param name="Packet">Packet to serialize</param>
        /// <returns>The packet in byte array form</returns>
        private static byte[] Serialize(int PacketSize, Packet Packet)
        {
            byte[] Buffer = new byte[PacketSize];
            int x = 2;
            Buffer[0] = Packet.Type;
            Buffer[1] = Packet.SubCode;

            byte[] TempArray = BitConverter.GetBytes(Packet.Check);
            Array.Copy(TempArray, 0, Buffer, x, TempArray.Length);
            x += TempArray.Length;

            TempArray = BitConverter.GetBytes(Packet.ID);
            Array.Copy(TempArray, 0, Buffer, x, TempArray.Length);
            x += TempArray.Length;

            TempArray = BitConverter.GetBytes(Packet.SequenceNumber);
            Array.Copy(TempArray, 0, Buffer, x, TempArray.Length);
            x += TempArray.Length;

            Array.Copy(Packet.Data, 0, Buffer, x, Packet.Data.Length);

            return Buffer;
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Acts as a packet holder
        /// </summary>
        private class Packet
        {
            public byte Type;
            public byte SubCode;
            public ushort Check;
            public ushort ID;
            public ushort SequenceNumber;
            public byte[] Data;
        }

        #endregion
    }
}