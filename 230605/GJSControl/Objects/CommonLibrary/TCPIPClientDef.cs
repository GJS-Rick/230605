using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SimpleTCP;
using FileStreamLibrary;
using System.Net.Sockets;
using System.Threading;
namespace CommonLibrary
{
    public enum ETCPIPCommand
    {
        Online,
        PanelHeight,
        PanelWidth,
        TotalThickness,

        PutType,
        CanExport,

        Run,
        End,
        GetStatus,

        RacipeName,
        LotID,
        Count
    }

    public class TCPIPClientDef : IDisposable
    {
        private SimpleTcpClient _client;
        private string _reply;
        private string _ip;
        private int _port;
        private object _lock;
        private int _tick;
        private int _timeoutTick;
        private Thread _task;
        private bool _connected;
        private bool _threadEnd;
        public TCPIPClientDef(string Folder)
        {
            IniFile ini = new IniFile(Folder + "\\TCPIPClient.ini", true);
            _ip = ini.ReadStr("Server", "IP", "127.0.0.1");
            _port = ini.ReadInt("Server", "PORT", 5000);
            ini.Dispose();

            _client = new SimpleTcpClient();
            _client.StringEncoder = Encoding.UTF8;
            _client.DelimiterDataReceived += DelimiterDataReceived;
            _client.Delimiter = 13;

            _lock = new object();

            _timeoutTick = 0;
            _threadEnd = false;
            _connected = false;
            _task = new Thread(DoLoop);
            _task.Priority = ThreadPriority.Lowest;
            _task.Start();

        }

        public void Dispose()
        {
            _threadEnd = true;
            while (_task != null && _task.IsAlive)
            {
                Thread.Sleep(5);
            }
            _client.Dispose();
        }

        private void DoLoop()
        {
            while (!_threadEnd)
            {
                if (!_connected)
                {
                    Thread.Sleep(50);

                    try
                    {
                        _client.Connect(_ip, _port);

                        _connected = true;

                        Thread.Sleep(1000);
                        SendCommand(ETCPIPCommand.Online);
                    }
                    catch
                    {
                        _connected = false;
                    }
                }
                else
                {
                    if (Timeout())
                    {
                        _connected = false;
                    }
                    else
                    {
                        if (Environment.TickCount - _tick > 3000)
                            SendCommand(ETCPIPCommand.Online);
                    }
                }
            }
        }

        private void DelimiterDataReceived(object sender, SimpleTCP.Message e)
        {
            _tick = Environment.TickCount;
            _reply = e.MessageString;

            string[] s = new string[1];
            s[0] = ",";
            string[] re = _reply.Split(s, 2, StringSplitOptions.RemoveEmptyEntries);
            if (re.Length > 0)
            {
                int function = int.Parse(re[0]);
                _timeoutTick = Environment.TickCount;
            }
        }

        public void SendCommand(ETCPIPCommand function, string message = "0")
        {
            if (!_connected)
                return;
            try
            {
                lock (_lock)
                {
                    _tick = Environment.TickCount;

                    _reply = string.Empty;

                    _client.WriteLine(((int)function).ToString() + "," + message);

                    _timeoutTick = Environment.TickCount;
                }
            }
            catch
            {
                _connected = false;
            }
        }

        public bool Timeout()
        {
            if (Environment.TickCount - _timeoutTick > 5000)
            {
                return true;
            }

            return false;
        }

        public void SendReply(ETCPIPCommand function, string Message)
        {
            try
            {
                lock (_lock)
                {
                    if (!_client.TcpClient.Connected)
                        return;

                    _reply = string.Empty;
                    _timeoutTick = Environment.TickCount;
                    _client.WriteLine(((int)function).ToString() + "," + Message);
                }
            }
            catch
            {
            }
        }

        public bool GetReply(ref ETCPIPCommand function, ref string Message)
        {
            if (string.IsNullOrEmpty(_reply))
                return false;

            string[] s = new string[1];
            s[0] = ",";
            string[] re = _reply.Split(s, 2, StringSplitOptions.RemoveEmptyEntries);
            if (re.Count() < 2)
            {
                return false;
            }

            function = (ETCPIPCommand)int.Parse(re[0]);
            Message = re[1];

            return true;
        }

        public bool GetCommand(ref ETCPIPCommand function)
        {
            if (string.IsNullOrEmpty(_reply))
                return false;

            string[] s = new string[1];
            s[0] = ",";
            string[] re = _reply.Split(s, 2, StringSplitOptions.RemoveEmptyEntries);
            if (re.Count() != 1)
            {
                return false;
            }

            function = (ETCPIPCommand)int.Parse(re[0]);
            return true;
        }
    }
}
