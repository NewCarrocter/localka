using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public static class NetworkClass
{
    internal static IPEndPoint self;
    internal static IPEndPoint[] clients;
    internal static bool send;
    private static readonly int port = 22000;
    public static bool CheckConnection() => NetworkInterface.GetAllNetworkInterfaces().Any(face => face.OperationalStatus == OperationalStatus.Up && (face.NetworkInterfaceType == NetworkInterfaceType.Ethernet || face.NetworkInterfaceType == NetworkInterfaceType.Wireless80211));
    public static IPEndPoint[] SetClients() => GetArpTable.GetIPformArpTable().AsParallel().Select(ip => new IPEndPoint(ip, port)).ToArray(); // get ips from ARP table
    public enum Command
    {
        startSend,
        changeSenderPort,
        changeSendDelay,
        stopSend,
        sendFile,
        changeState
    }
    internal static async void SendCommand(Command num, IPEndPoint[] useable, string? message = null)
    {/* 0 - start send; 1 - change self port; 2 - change delay in seconds; 3 - stop send; 4 - send file 5 - change state*/
        if (!CheckConnection())
            throw new Exception("Сетевой интерфейс не включен или тип сетевого интерфейса использующегося в программме нет!");
        switch (num)
        {
            case Command.startSend:
                StartConnectionAsync(useable);
                break;
            case Command.changeSenderPort:
                //useable.AsParallel().ForAll(async ep => await sender.SendAsync(new byte[] { 1 }, 1, ep));
                //useable.AsParallel().ForAll(async ep => await sender.SendAsync(Encoding.UTF8.GetBytes(message), Encoding.UTF8.GetBytes(message).Length, ep));
                //useable.AsParallel().ForAll(ep => ep.Port = int.Parse(message));
                break;
            case Command.changeSendDelay:
                //useable.AsParallel().ForAll(async ep => await sender.SendAsync(new byte[] { 2 }, 1, ep));
                //useable.AsParallel().ForAll(async ep => await sender.SendAsync(Encoding.UTF8.GetBytes(message), Encoding.UTF8.GetBytes(message).Length, ep));
                break;
            case Command.stopSend:
                //useable.AsParallel().ForAll(ep => sender.SendAsync(new byte[] { 3 }, 1, ep));
                break;
            case Command.sendFile:
                List<byte[]> messages = new();
                List<string> noNulls = message.Split('|').ToList();
                Debug.Log($"Names: {message}");
                noNulls.RemoveAll(str => String.IsNullOrEmpty(str));
                Debug.Log($"Parced names: {String.Join('\n', noNulls.ToArray())}");
                foreach (string s in noNulls)
                {
                    messages.Add(System.IO.File.ReadAllBytes(s));
                }
                string[] fnames = noNulls.Select(ph => ph.Split('\\').Last()).ToArray();
                fnames[^1] = "|" + fnames[^1];
                Debug.Log($"{String.Join('\n', fnames)}"); 
                foreach (IPEndPoint ipEp in useable)
                {
                    SendFiles(ipEp, messages, fnames);
                }
                await Task.WhenAll();
                Debug.Log("All files sended!");
            break;
            case Command.changeState:
                foreach (IPEndPoint ipEp in useable)
                {
                    SendState(ipEp, byte.Parse(message));
                }
                await Task.WhenAll();
                Debug.Log("All states sended!");
            break;
        }
    }
    private static async Task SendState(IPEndPoint sendTo, byte state)
    {
        TcpClient tcpClient = new();
        await tcpClient.ConnectAsync(sendTo.Address, sendTo.Port);
        NetworkStream ns = tcpClient.GetStream();
        ns.WriteByte(5);
        ns.WriteByte(state);

    }
    private static async Task SendFiles(IPEndPoint sendTo, List<byte[]> files, string[] filesName)
    {
        TcpClient tcpClient = new();
        Debug.Log("Connecting to "+ sendTo.ToString());
        try
        {
            await tcpClient.ConnectAsync(sendTo.Address, sendTo.Port);
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.WriteByte(4);
            Debug.Log("waiting for the response");
            networkStream.ReadByte();
            int a;
            byte[] file, encod;
            foreach (var i in files.Zip(filesName, (a, b) => new { A = a, B = b }))
            {
                a = 1;
                encod = Encoding.UTF8.GetBytes(i.B);
                Debug.Log($"{i.B} length {encod.Length}");
                Debug.Log($"send length {encod.Length}");
                networkStream.Write(encod);
                networkStream.ReadByte();
                do
                {
                    file = i.A.Skip(102400 * (a++ - 1)).Take(102400).ToArray();
                    Debug.Log($"Data length {file.Length}");
                    encod = Encoding.UTF8.GetBytes(file.Length.ToString());
                    Debug.Log($"Data length in bytes {String.Join('-', encod)}");
                    Debug.Log($"{a - 1}: Data length decoded {Encoding.UTF8.GetString(encod)}\n");
                    await networkStream.WriteAsync(encod);
                    networkStream.ReadByte();
                    await Task.Delay(100);
                    await networkStream.WriteAsync(file);
                    networkStream.ReadByte();
                } while (file.Length == 102400);
            }
            networkStream.Read(new byte[4]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            Debug.Log("Close connection with " + sendTo.ToString());
        }
        Debug.Log("end");
    }
    private static async Task StartConnectionAsync(IPEndPoint[] getImgFrom) // 10.45.0.91:22000
    {
        TcpClient tcpClient = new();
        try
        {
            foreach (IPEndPoint ipEp in getImgFrom)
            {
                Debug.Log(ipEp.ToString());
                await tcpClient.ConnectAsync(ipEp.Address, port);
                if (!tcpClient.Connected) throw new Exception("Не удалось подключиться к " + ipEp.ToString());
                NetworkStream networkStream = tcpClient.GetStream();
                networkStream.WriteByte(0);
                string testStr = UnityEngine.Random.Range(10000, 100000).ToString();
                await networkStream.WriteAsync(Encoding.UTF8.GetBytes(testStr));
                await Task.Delay(500);
                byte[] buffer = new byte[5];
                await networkStream.ReadAsync(buffer);
                if (Encoding.UTF8.GetString(buffer) == testStr)
                    Debug.Log("Sucsess!");
                else
                    throw new Exception("Не удалось подключиться к " + ipEp.ToString());
                networkStream.Dispose();
                networkStream.Close();
                tcpClient.Close();
                //GetImg(ipEp.Port);
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            tcpClient.Close();
        }
    }
    private static class GetArpTable
    {
        [StructLayout(LayoutKind.Sequential)]
        struct MIB_IPNETROW
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwIndex;
            [MarshalAs(UnmanagedType.U4)]
            public int dwPhysAddrLen;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            [MarshalAs(UnmanagedType.U4)]
            public int dwAddr;
            [MarshalAs(UnmanagedType.U4)]
            public int dwType;
        }
        [DllImport("IpHlpApi.dll")]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern int GetIpNetTable(IntPtr pIpNetTable, [MarshalAs(UnmanagedType.U4)] ref int pdwSize, bool bOrder);
        [DllImport("IpHlpApi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int FreeMibTable(IntPtr plpNetTable);
        // The insufficient buffer error.
        const int ERROR_INSUFFICIENT_BUFFER = 122;
        public static IPAddress[] GetIPformArpTable()
        {
            IPAddress[] ips;
            int bytesNeeded = 0;
            int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);
            if (result != ERROR_INSUFFICIENT_BUFFER)
                throw new Win32Exception(result);
            IntPtr buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocCoTaskMem(bytesNeeded);
                result = GetIpNetTable(buffer, ref bytesNeeded, false);
                if (result != 0)
                    throw new Win32Exception(result);
                int entries = Marshal.ReadInt32(buffer);
                IntPtr currentBuffer = new IntPtr(buffer.ToInt64() + Marshal.SizeOf(typeof(int)));
                MIB_IPNETROW[] table = new MIB_IPNETROW[entries];
                for (int index = 0; index < entries; index++)
                    table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() + (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))), typeof(MIB_IPNETROW));
                ips = table.AsParallel().Where(row => row.dwType == 3).Select(row => new IPAddress(BitConverter.GetBytes(row.dwAddr))).ToArray();
            }
            finally
            {
                // Release the memory.
                FreeMibTable(buffer);
                GC.Collect();
            }
            return ips;
        }
    }
}