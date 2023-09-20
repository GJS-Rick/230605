using FileStreamLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CommonLibrary
{
    public class DataTransferDef : IDisposable
    {
        private enum EDataTransferFileType//副檔名
        {
            /// <summary>DIO訊號</summary>
            IO,
            /// <summary>僅看檔名資訊</summary>
            Name,
            /// <summary>含有內容資訊</summary>
            Content,

            Count
        }

        public enum EFileCaption//檔案說明
        {
            /// <summary>檔案說明00</summary>
            Comment00,
            /// <summary>檔案說明01</summary>
            Comment01,
            /// <summary>檔案說明02</summary>
            Comment02,
            /// <summary>檔案說明03</summary>
            Comment03,
            /// <summary>檔案說明04</summary>
            Comment04,
            /// <summary>檔案說明05</summary>
            Comment05,
            /// <summary>檔案說明06</summary>
            Comment06,
            /// <summary>檔案說明07</summary>
            Comment07,
            /// <summary>檔案說明08</summary>
            Comment08,
            /// <summary>檔案說明09</summary>
            Comment09,
            /// <summary>檔案說明10</summary>
            Comment10,
            /// <summary>檔案說明11</summary>
            Comment11,
            /// <summary>檔案說明12</summary>
            Comment12,
            /// <summary>檔案說明13</summary>
            Comment13,
            /// <summary>檔案說明14</summary>
            Comment14,
            /// <summary>檔案說明15</summary>
            Comment15,

            /// <summary>無設定</summary>
            None,

            Count
        }

        public struct DataTransferInfo
        {
            /// <summary>名稱</summary>
            public string Name;
            /// <summary>本地位址</summary>
            public string LocalIP;
            /// <summary>遠端位址</summary>
            public string RemoteIP;
            /// <summary>帳號</summary>
            public string User;
            /// <summary>密碼</summary>
            public string Password;
            /// <summary>啟用</summary>
            public bool Enable;
        }

        private string _FilePath;
        private readonly string _ContenFileUseSection;
        private readonly string[] _Name;
        private List<IPListType> _IPList;
        private DataTransferInfo[] _Info;
        private DataTransferInfo[] _InfoBuf;
        private String[] _CaptionName;
        private String _DataTransferPath;
        private String _DataTransferFolder;
        private String _TobeSendPath;

        private bool _ThreadEnd;
        private Thread _ThDataTransfer;

        /// <summary>資料傳遞</summary>
        /// <param name="sFilePath">檔案路徑</param>
        /// <remarks>
        /// <list type="檔案路徑">sFilePath: 檔案路徑</list>
        /// </remarks>
        public DataTransferDef(string[] sName, string sFilePath = "C:\\Automation\\DataTransfer.ini")
        {
            _FilePath = sFilePath;

            _ContenFileUseSection = "Conten";

            if (sName.Length > 0) _Name = sName; else _Name = new string[0];
            _IPList = new List<IPListType>();
            _Info = new DataTransferInfo[_Name.Length];
            _InfoBuf = new DataTransferInfo[_Name.Length];

            for (int i = 0; i < _Name.Length; i++)
            {
                _Info[i].Name = _Name[i];
                _Info[i].LocalIP = "192.168.0.0";
                _Info[i].RemoteIP = "192.168.0.0";
                _Info[i].User = "Guest";
                _Info[i].Password = "";

                _InfoBuf[i].Name = _Name[i];
                _InfoBuf[i].LocalIP = "192.168.0.0";
                _InfoBuf[i].RemoteIP = "192.168.0.0";
                _InfoBuf[i].User = "Guest";
                _InfoBuf[i].Password = "";
                _InfoBuf[i].Enable = true;

                AddIP(_Info[i]);
            }

            _CaptionName = new string[(int)EFileCaption.Count];
            for (int i = 0; i < (int)EFileCaption.Count; i++)
                _CaptionName[i] = ((EFileCaption)i).ToString();

            _DataTransferFolder = "DataTransfer";
            _DataTransferPath = "C:\\" + _DataTransferFolder;
            if (!Directory.Exists(_DataTransferPath))
                Directory.CreateDirectory(_DataTransferPath);

            ShareResource(_DataTransferFolder, "", _DataTransferPath);

            _TobeSendPath = _DataTransferPath + "\\TobeSend";
            if (!Directory.Exists(_TobeSendPath))
                Directory.CreateDirectory(_TobeSendPath);

            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            _ThreadEnd = false;
            _ThDataTransfer = new Thread(new ThreadStart(Execute))
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            _ThDataTransfer.Start();
        }

        ~DataTransferDef() { }

        public void Dispose()
        {
            _ThreadEnd = true;

            if (_ThDataTransfer != null && _ThDataTransfer.IsAlive)
            {
                while (_ThDataTransfer.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }

            for (int i = 0; i < _IPList.Count; i++)
                _IPList[i].Disconnect();
        }

        #region 確認檔案與建立資料夾、建立新檔案、讀取檔案
        private void DirExistsAndCreate(string iniPath)
        {
            if (!Directory.Exists(iniPath))
                Directory.CreateDirectory(iniPath);
        }
        private bool CheckFile(string sFilePath)
        {
            if (!File.Exists(sFilePath))
            {
                string[] _SfilePathArr = sFilePath.Split('\\');
                if (_SfilePathArr[_SfilePathArr.Length - 1].Contains(".ini"))
                {
                    string _iniPath = "";
                    for (int i = 0; i < _SfilePathArr.Length - 1; i++)
                        _iniPath += _SfilePathArr[i];

                    DirExistsAndCreate(_iniPath);
                }

                return false;
            }

            return true;
        }
        private void CreateFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);

            for (int i = 0; i < (int)EFileCaption.Count; i++)
                iniFileInfo.WriteStr("Caption", ((EFileCaption)i).ToString(), _CaptionName[i]);

            for (int i = 0; i < _InfoBuf.Length; i++)
            {
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Remote", _InfoBuf[i].RemoteIP);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Local", _InfoBuf[i].LocalIP);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "User", _InfoBuf[i].User);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Password", _InfoBuf[i].Password);
                iniFileInfo.WriteBool(_InfoBuf[i].Name, "Enable", _InfoBuf[i].Enable);
            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            for (int i = 0; i < _IPList.Count; i++)
                _IPList[i].Disconnect();

            _IPList.Clear();

            IniFile iniFileInfo = new IniFile(_FilePath, true);

            for (int i = 0; i < (int)EFileCaption.Count; i++)
                _CaptionName[i] = iniFileInfo.ReadStr("Caption", ((EFileCaption)i).ToString(), ((EFileCaption)i).ToString());

            _Info = new DataTransferInfo[_Name.Length];
            for (int i = 0; i < _Info.Length; i++)
            {
                _Info[i].Name = _Name[i];
                _Info[i].RemoteIP = iniFileInfo.ReadStr(_Info[i].Name, "Remote", "0.0.0.0");
                _Info[i].LocalIP = iniFileInfo.ReadStr(_Info[i].Name, "Local", "0.0.0.0");
                _Info[i].User = iniFileInfo.ReadStr(_Info[i].Name, "User", "Guest");
                _Info[i].Password = iniFileInfo.ReadStr(_Info[i].Name, "Password", "");
                _Info[i].Enable = iniFileInfo.ReadBool(_Info[i].Name, "Enable", true);

                AddIP(_Info[i]);
            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            if (CheckFile(_FilePath))
                File.WriteAllText(_FilePath, string.Empty);

            IniFile iniFileInfo = new IniFile(_FilePath, false);

            for (int i = 0; i < (int)EFileCaption.Count; i++)
                iniFileInfo.WriteStr("Caption", ((EFileCaption)i).ToString(), _CaptionName[i]);

            for (int i = 0; i < _InfoBuf.Length; i++)
            {
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Remote", _InfoBuf[i].RemoteIP);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Local", _InfoBuf[i].LocalIP);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "User", _InfoBuf[i].User);
                iniFileInfo.WriteStr(_InfoBuf[i].Name, "Password", _InfoBuf[i].Password);
                iniFileInfo.WriteBool(_InfoBuf[i].Name, "Enable", _InfoBuf[i].Enable);
            }

            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        #endregion

        public void Save()
        {
            WriteFile();
            ReadFile();
        }

        public void SetCaption(string[] sArr) { _CaptionName = sArr; }
        public void SetInfoBuf(DataTransferInfo[] eInfoBuf) { _InfoBuf = eInfoBuf; }

        private void ShareResource(string ShareName, string Description, string FolderPath)
        {
            ManagementClass mClass = new ManagementClass("Win32_Share");
            ManagementBaseObject mBaseObj_in = mClass.GetMethodParameters("Create");
            ManagementBaseObject mBaseOjb_out;

            mBaseObj_in["Name"] = ShareName;            //共用名稱
            mBaseObj_in["Description"] = Description;    //詳細資訊
            mBaseObj_in["Path"] = FolderPath;            //資料夾位置
            mBaseObj_in["Type"] = 0x0;                   //共用類型

            /*  分享其他型態的參數
            * 
            *  DISK_DRIVE = 0x0
            *  PRINT_QUEUE = 0x1
            *  DEVICE = 0x2
            *  IPC = 0x3
            *  DISK_DRIVE_ADMIN = 0x80000000
            *  PRINT_QUEUE_ADMIN = 0x80000001
            *  DEVICE_ADMIN = 0x80000002
            *  IPC_ADMIN = 0x8000003
            */

            //檢查看看，是否叫用成功
            mBaseOjb_out = mClass.InvokeMethod("Create", mBaseObj_in, null);

            uint _Result = (uint)(mBaseOjb_out.Properties["ReturnValue"].Value);
            if (_Result != 0 && _Result != 22)
            {
                throw new Exception("無法共享資料夾");
            }
            else
            {
                #region 添加everyone用戶
                //獲得文件夾訊息
                DirectoryInfo dir = new DirectoryInfo(FolderPath);
                //獲得該文件夾的所有訪問權限
                DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
                //設定文件ACL繼承
                InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
                //添加everyone用戶組的訪問權限規則 完全控制權限
                FileSystemAccessRule everyoneFileSystemAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
                //添加User用戶組的訪問權限規則 完全控制權限
                FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);

                bool isModified = false;
                dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified);
                dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);

                //設置訪問權限
                dir.SetAccessControl(dirSecurity);
                #endregion
                //DirectoryInfo di = new DirectoryInfo(FolderPath);
                //DirectorySecurity ds = di.GetAccessControl();
                //FileSystemAccessRule ar1 = new FileSystemAccessRule(Account, FileSystemRights.Read, AccessControlType.Allow);
                //FileSystemAccessRule ar2 = new FileSystemAccessRule(Account, FileSystemRights.Read, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow);

                //ds.AddAccessRule(ar1);
                //ds.AddAccessRule(ar2);
                //di.SetAccessControl(ds);
            }
        }

        private void Execute()
        {
            while (!_ThreadEnd)
            {
                if (_IPList.Count > 0)
                {
                    lock (_IPList)
                    {
                        for (int i = 0; i < _IPList.Count; i++)
                        {
                            if (_IPList[i].TryConnect())
                            {
                                RenewTake(_IPList[i]);
                                Thread.Sleep(5);
                                RenewSend(_IPList[i]);
                                Thread.Sleep(5);

                                if (GetNewest_Test(_IPList[i].GetName_Remote(), out string sTest))
                                {
                                    if (sTest == "Test")
                                        Send_TestDone(_IPList[i].GetName_Remote());
                                    if (sTest == "Done")
                                    {
                                        AlarmTextDisplay.Add("Test", AlarmType.Msg, "DataTransfer", "Success");
                                    }
                                }
                            }
                            else
                                Thread.Sleep(10);
                        }
                    }
                }
                else
                    Thread.Sleep(10);
            }
        }

        public void ResendReady(string eName)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                {
                    ClearTobeSend(_IPList[i]);
                    _IPList[i].ResendReady();
                    break;
                }
            }
        }
        public void SendRun() { for (int i = 0; i < _IPList.Count; i++) _IPList[i].RemoteSendAutoStart(); }
        public void SendStop() { for (int i = 0; i < _IPList.Count; i++) _IPList[i].RemoteSendAutoStop(); }

        private void ClearTobeSend(IPListType eRemotePC)
        {
            DirectoryInfo _UnsendDir = new DirectoryInfo(eRemotePC.GetPath_Unsend());

            foreach (FileInfo File in _UnsendDir.GetFiles())
                File.Delete();
        }

        public string[] GetCaption() { return _CaptionName; }
        public int GetInfoNum() { return _Info.Length; }
        public DataTransferInfo[] GetInfo() { return _Info; }
        public int GetRemotePCNum() { return _IPList.Count; }
        public string GetLocalIP(int iIndex)
        {
            if (_IPList.Count - 1 > iIndex)
                return _IPList[iIndex].GetIP_Local();

            return "Null";
        }
        public string[] GetRemotePCIP()
        {
            string[] _StrIP = new string[_IPList.Count];

            for (int i = 0; i < _IPList.Count; i++)
                _StrIP[i] = _IPList[i].GetIP_Remote();

            return _StrIP;
        }
        public string GetRemotePCIP(int iIndex)
        {
            if (_IPList.Count - 1 > iIndex)
                return _IPList[iIndex].GetIP_Remote();

            return "Null";
        }
        public string[] GetRemotePCName()
        {
            string[] _StrName = new string[_IPList.Count];

            for (int i = 0; i < _IPList.Count; i++)
                _StrName[i] = _IPList[i].GetName_Remote();

            return _StrName;
        }
        public string GetRemotePCName(int iIndex)
        {
            if (_IPList.Count - 1 > iIndex)
                return _IPList[iIndex].GetName_Remote();

            return "Null";
        }


        #region IP & Disk
        private void AddIP(DataTransferInfo eDT) { _IPList.Add(new IPListType(_DataTransferFolder, eDT.Name, eDT.LocalIP, eDT.RemoteIP, eDT.User, eDT.Password, eDT.Enable)); }

        public bool IsAutoAlive(string eName)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                    return _IPList[i].IsAutoAlive();
            }
            return false;
        }
        public bool IsDiskConnect(string eName)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                    return _IPList[i].IsDiskConnect();
            }
            return false;
        }
        #endregion

        public string GetFolderPath() { return _DataTransferPath; }

        #region Take
        private void RenewTake(IPListType _RemotePC)
        {
            DirectoryInfo _LocalDir = new DirectoryInfo(_RemotePC.GetPath_Local());
            _RemotePC.RenewTake(_LocalDir.GetFiles());
        }

        public bool GetNewest_IO(EDataTransferName eName, EFileCaption eCaption, out string eData)
        {
            string eFileName = "";
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                    return _IPList[i].GetNewest(EDataTransferFileType.IO.ToString(), eCaption.ToString(), out eData, out eFileName, true);
            }

            eData = "";
            return false;
        }
        public bool GetNewest_Name(EDataTransferName eName, EFileCaption eCaption, out string eData)
        {
            string eFileName = "";

            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                    return _IPList[i].GetNewest(EDataTransferFileType.Name.ToString(), eCaption.ToString(), out eData, out eFileName, true);
            }

            eData = "";
            return false;
        }
        public bool GetNewest_Content(EDataTransferName eName, EFileCaption eCaption, out string eData, out string[] eKey, out string[] eValue)
        {
            string eFileName = "";

            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                {
                    if (_IPList[i].GetNewest(EDataTransferFileType.Content.ToString(), eCaption.ToString(), out eData, out eFileName, false))
                    {
                        string _NotdeletePath = _IPList[i].GetPath_NotDelete() + "\\" + eFileName;

                        IniFile iniFileInfo = new IniFile(_NotdeletePath, false);

                        eKey = iniFileInfo.GetKey(_ContenFileUseSection);
                        eValue = new string[eKey.Length];
                        for (int j = 0; j < eKey.Length; j++)
                            eValue[j] = iniFileInfo.ReadStr(_ContenFileUseSection, eKey[j], "");

                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();

                        FileInfo _BufFile = new FileInfo(_NotdeletePath);
                        _BufFile.Delete();
                    }
                    else
                        break;

                    return true;
                }
            }

            eData = "";
            eKey = new string[0];
            eValue = new string[0];
            return false;
        }
        private bool GetNewest_Test(string eName, out string eData)
        {
            string eFileName = "";

            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                {
                    _IPList[i].GetNewest(EDataTransferFileType.Name.ToString(), "Test", out eData, out eFileName, true);
                    return true;
                }
            }

            eData = "";
            return false;
        }
        #endregion

        #region Send
        private void RenewSend(IPListType _RemotePC)
        {
            #region Send Auto
            if (_RemotePC.IsNeedSend_Auto(out string _AutoFilePath, out string _AutoLocalFilePath))
            {
                try
                {
                    IniFile iniFileInfo = new IniFile(_AutoLocalFilePath, false);
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    Thread.Sleep(1);

                    FileInfo _File = new FileInfo(_AutoLocalFilePath);
                    _File.CopyTo(_AutoFilePath, true);
                    _File.Delete();

                    _RemotePC.SetCanSend();
                    _RemotePC.ResetNeedSend_Auto();
                }
                catch
                {
                    _RemotePC.ResetCanSend();
                }
            }
            #endregion
            #region Send Ready
            if (_RemotePC.IsNeedSend_Ready(out string _ReadyFilePath, out string _ReadyLocalFilePath))
            {
                _RemotePC.ResetSendReady();

                try
                {
                    IniFile iniFileInfo = new IniFile(_ReadyLocalFilePath, false);
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    Thread.Sleep(1);

                    FileInfo _File = new FileInfo(_ReadyLocalFilePath);
                    _File.CopyTo(_ReadyFilePath, true);
                    _File.Delete();

                    _RemotePC.SetCanSend();
                    _RemotePC.SetSendReady();
                    _RemotePC.ResetNeedSend_Ready();
                }
                catch
                {
                    _RemotePC.ResetCanSend();
                }
            }
            #endregion
            #region Send Done
            if (_RemotePC.IsNeedSend_Done(out string _DoneFilePath, out string _DoneLocalFilePath))
            {
                try
                {
                    IniFile iniFileInfo = new IniFile(_DoneLocalFilePath, false);
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    Thread.Sleep(1);

                    FileInfo _File = new FileInfo(_DoneLocalFilePath);
                    _File.CopyTo(_DoneFilePath, true);
                    _File.Delete();

                    _RemotePC.SetCanSend();
                    _RemotePC.ResetNeedSend_Done();
                }
                catch
                {
                    _RemotePC.ResetCanSend();
                }
            }
            #endregion

            #region Send Run
            if (_RemotePC.IsRemoteSendAutoStart(out string _RunFilePath, out string _RunLocalFilePath))
            {
                try
                {
                    IniFile iniFileInfo = new IniFile(_RunLocalFilePath, false);
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    Thread.Sleep(1);

                    FileInfo _File = new FileInfo(_RunLocalFilePath);
                    _File.CopyTo(_RunFilePath, true);
                    _File.Delete();

                    _RemotePC.SetCanSend();
                    _RemotePC.ResetRemoteSendAutoStart();
                }
                catch
                {
                    _RemotePC.ResetCanSend();
                }
            }
            #endregion
            #region Send Stop
            if (_RemotePC.IsRemoteSendAutoStop(out string _StopFilePath, out string _StopLocalFilePath))
            {
                try
                {
                    IniFile iniFileInfo = new IniFile(_StopLocalFilePath, false);
                    iniFileInfo.FileClose();
                    iniFileInfo.Dispose();
                    Thread.Sleep(1);

                    FileInfo _File = new FileInfo(_StopLocalFilePath);
                    _File.CopyTo(_StopFilePath, true);
                    _File.Delete();

                    _RemotePC.SetCanSend();
                    _RemotePC.ResetRemoteSendAutoStop();
                }
                catch
                {
                    _RemotePC.ResetCanSend();
                }
            }
            #endregion

            foreach (string[] _LogStr in _RemotePC.CheckUnsend())
                LogDef.Add(ELogFileName.DataTransfer, "Send", _LogStr[0], _LogStr[1]);
        }

        public void Send_IO(EDataTransferName eName, EFileCaption eCaption, string eData)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                {
                    try
                    {
                        string _BufPath;
                        _IPList[i].GetFilePath(out string _BufName, eData, EDataTransferFileType.IO.ToString(), eCaption.ToString(), "From");
                        _BufPath = _IPList[i].GetPath_Unsend() + "\\" + _BufName;

                        IniFile iniFileInfo = new IniFile(_BufPath, false);
                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();
                    }
                    catch { }

                    return;
                }
            }
        }
        public void Send_Name(EDataTransferName eName, EFileCaption eCaption, string eData)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                {
                    try
                    {
                        string _BufPath;
                        _IPList[i].GetFilePath(out string _BufName, eData, EDataTransferFileType.Name.ToString(), eCaption.ToString(), "From");
                        _BufPath = _IPList[i].GetPath_Unsend() + "\\" + _BufName;

                        IniFile iniFileInfo = new IniFile(_BufPath, false);
                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();
                    }
                    catch { }

                    return;
                }
            }
        }
        public void Send_Content(EDataTransferName eName, EFileCaption eCaption, string eData, string[] eKey, string[] eValue)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName.ToString())
                {
                    try
                    {
                        string _BufPath;
                        _IPList[i].GetFilePath(out string _BufName, eData, EDataTransferFileType.Content.ToString(), eCaption.ToString(), "From");
                        _BufPath = _IPList[i].GetPath_Unsend() + "\\" + _BufName;

                        IniFile iniFileInfo = new IniFile(_BufPath, false);
                        for (int j = 0; j < eKey.Length; j++)
                        {
                            string _ValueBuf = "";
                            if (eValue.Length > j)
                                _ValueBuf = eValue[j];

                            iniFileInfo.WriteStr(_ContenFileUseSection, eKey[j], _ValueBuf);
                        }
                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();
                    }
                    catch { }

                    return;
                }
            }
        }
        public void Send_Test(string eName)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                {
                    try
                    {
                        string _BufPath;
                        _IPList[i].GetFilePath(out string _BufName, "Test", EDataTransferFileType.Name.ToString(), "Test", "From");
                        _BufPath = _IPList[i].GetPath_Unsend() + "\\" + _BufName;

                        IniFile iniFileInfo = new IniFile(_BufPath, false);
                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();
                    }
                    catch { }

                    return;
                }
            }
        }
        private void Send_TestDone(string eName)
        {
            for (int i = 0; i < _IPList.Count; i++)
            {
                if (_IPList[i].GetName_Remote() == eName)
                {
                    try
                    {
                        string _BufPath;
                        _IPList[i].GetFilePath(out string _BufName, "Done", EDataTransferFileType.Name.ToString(), "Test", "From");
                        _BufPath = _IPList[i].GetPath_Unsend() + "\\" + _BufName;

                        IniFile iniFileInfo = new IniFile(_BufPath, false);
                        iniFileInfo.FileClose();
                        iniFileInfo.Dispose();
                    }
                    catch { }

                    return;
                }
            }
        }
        #endregion

        public string[][] GetLocalMac()
        {
            List<string> macList = new List<string>();
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    macList.Add(nic.GetPhysicalAddress().ToString());
            }

            List<string[]> list_MacAndIP = new List<string[]>();
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    list_MacAndIP.Add(new string[] { mo["MacAddress"].ToString(), ((string[])mo["IPAddress"])[0] });
            }
            return (list_MacAndIP.ToArray());
        }
    }

    public class IPListType
    {
        /// <summary>IP位置</summary>
        private enum IPPos
        {
            /// <summary>本地</summary>
            Local,
            /// <summary>遠端</summary>
            Remote,

            Count
        }

        /// <summary>基本副檔名</summary>
        private enum BasicExtension
        {
            /// <summary>自動</summary>
            Auto,

            /// <summary>遠端寄出自動開始</summary>
            RemoteSendAutoStart,
            /// <summary>遠端寄出自動停止</summary>
            RemoteSendAutoStop,

            /// <summary>已準備</summary>
            Ready,
            /// <summary>完成</summary>
            Done,

            Count
        }

        /// <summary>分割符號</summary>
        private readonly string _Separator;
        private readonly string _RemoteName;
        private readonly string _RemoteIP;
        private readonly string _LocalIP;
        private readonly string _UseFolderName;
        private readonly string _UserName;
        private readonly string _Password;

        private bool[] _IPCorrect;
        private readonly string[] _DataPath;
        private readonly string _UnsentPath;
        private readonly string _NotDeletePath;
        private string[] _BasicFileName;
        private bool[] _NeedSendBasicFile;
        private bool _DiskConnected;
        private bool _IsSendReady;
        private bool _CanTake;
        private bool _CanSend;
        /// <summary>遠端準備完成</summary>
        private bool _RemoteReady;
        private bool _IsRemoteRun;
        private bool _IsRemoteStop;
        private bool _AutoAlive;
        private bool _Enable;

        private int _AutoAliveTick;
        private int _SendIdleTick;
        private int _TimeOut;

        private List<FileInfo> _TakeLocalFiles;

        #region 查看檔案是否被佔用
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        #endregion

        public IPListType(string eFolder, string eName, string eLocalIP, string eRemoteIP, string eUserName, string ePW, bool bEnable = true)
        {
            _Separator = "@#";
            _RemoteName = eName;
            _RemoteIP = eRemoteIP;
            if (string.IsNullOrEmpty(eLocalIP))
                _LocalIP = GetNetAddress()[2];
            else
                _LocalIP = eLocalIP;
            _UseFolderName = eFolder;
            _UserName = eUserName;
            _Password = ePW;

            _IPCorrect = new bool[(int)IPPos.Count];
            _DataPath = new string[(int)IPPos.Count];
            for (int i = 0; i < (int)IPPos.Count; i++)
            {
                string _BufIP = "";
                string _BasicPath = "C:\\";
                switch ((IPPos)i)
                {
                    case IPPos.Local:
                        _BufIP = _LocalIP;
                        _BasicPath = "C:\\";
                        break;
                    case IPPos.Remote:
                        _BufIP = _RemoteIP;
                        _BasicPath = "\\\\" + _BufIP + "\\";
                        break;
                    default:
                        break;
                }

                _IPCorrect[i] = IsIPCorrect(_BufIP);
                if (_IPCorrect[i])
                    _DataPath[i] = _BasicPath + _UseFolderName;
                else
                    _DataPath[i] = "C:\\" + _UseFolderName;
            }//確認IP正確並設定本地與遠端路徑

            _UnsentPath = "C:\\" + _UseFolderName + "\\TobeSend\\" + GetIP_Remote("_");
            if (!Directory.Exists(_UnsentPath))
                Directory.CreateDirectory(_UnsentPath);
            else
            {
                DirectoryInfo _UnsendDir = new DirectoryInfo(_UnsentPath);
                foreach (FileInfo File in _UnsendDir.GetFiles())
                    File.Delete();
            }

            _NotDeletePath = "C:\\" + _UseFolderName + "\\TobeSend\\NotDeleted";
            if (!Directory.Exists(_NotDeletePath))
                Directory.CreateDirectory(_NotDeletePath);
            else
            {
                DirectoryInfo _NotDeleteDir = new DirectoryInfo(_NotDeletePath);
                foreach (FileInfo File in _NotDeleteDir.GetFiles())
                    File.Delete();
            }

            _BasicFileName = new string[(int)BasicExtension.Count];
            _NeedSendBasicFile = new bool[(int)BasicExtension.Count];
            for (int i = 0; i < (int)BasicExtension.Count; i++)
            {
                _BasicFileName[i] = CreatFileName("Basic", ((BasicExtension)i).ToString(), "None", "None");
                _NeedSendBasicFile[i] = false;
            }

            _DiskConnected = false;
            _IsSendReady = false;
            _CanTake = false;
            _CanSend = false;
            _RemoteReady = false;
            _IsRemoteRun = false;
            _IsRemoteStop = true;
            _AutoAlive = false;
            _Enable = bEnable;

            _AutoAliveTick = Environment.TickCount;
            _SendIdleTick = Environment.TickCount - 10000;
            _TimeOut = 1000;

            _TakeLocalFiles = new List<FileInfo>();
        }

        private bool IsIPCorrect(string eIP)
        {
            System.Net.IPAddress _IPAddress;
            bool _Result = false;

            if (!string.IsNullOrEmpty(eIP))
                _Result = System.Net.IPAddress.TryParse(eIP, out _IPAddress);

            if (!_Result)
                AlarmTextDisplay.Add("IP Error", AlarmType.Warning, "DataTransfer", "IP格式錯誤");

            return _Result;
        }

        /// <summary>查看檔案是否被佔用</summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool IsFileOccupied(string filePath)
        {
            IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
            CloseHandle(vHandle);
            return vHandle == HFILE_ERROR ? true : false;
        }

        #region GetIP
        public string GetIP_Local(string eStr = "")
        {
            if (string.IsNullOrEmpty(eStr))
                return _LocalIP;
            else
                return _LocalIP.Replace(".", eStr);
        }
        public string GetIP_Remote(string eStr = "")
        {
            if (string.IsNullOrEmpty(eStr))
                return _RemoteIP;
            else
                return _RemoteIP.Replace(".", eStr);
        }
        public string GetName_Remote() { return _RemoteName; }
        #endregion

        #region Get Path
        public string GetPath_Local() { return _DataPath[(int)IPPos.Local]; }
        public string GetPath_Remote() { return _DataPath[(int)IPPos.Remote]; }
        public string GetPath_Unsend() { return _UnsentPath; }
        public string GetPath_NotDelete() { return _NotDeletePath; }
        #endregion

        #region Need Send File
        public void ResendReady()
        {
            _RemoteReady = false;
            _NeedSendBasicFile[(int)BasicExtension.Ready] = true;
        }
        public void RemoteSendAutoStart() { _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStart] = true; }
        public void RemoteSendAutoStop() { _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStop] = true; }
        public bool IsNeedSend_Auto(out string eFileFullPath, out string eLocalFileFullPath)
        {
            eFileFullPath = _DataPath[(int)IPPos.Remote] + "\\" + _BasicFileName[(int)BasicExtension.Auto];
            eLocalFileFullPath = _UnsentPath + "\\" + _BasicFileName[(int)BasicExtension.Auto];
            return _NeedSendBasicFile[(int)BasicExtension.Auto];
        }
        public bool IsNeedSend_Ready(out string eFileFullPath, out string eLocalFileFullPath)
        {
            eFileFullPath = _DataPath[(int)IPPos.Remote] + "\\" + _BasicFileName[(int)BasicExtension.Ready];
            eLocalFileFullPath = _UnsentPath + "\\" + _BasicFileName[(int)BasicExtension.Ready];
            return _NeedSendBasicFile[(int)BasicExtension.Ready];
        }
        public bool IsNeedSend_Done(out string eFileFullPath, out string eLocalFileFullPath)
        {
            eFileFullPath = _DataPath[(int)IPPos.Remote] + "\\" + _BasicFileName[(int)BasicExtension.Done];
            eLocalFileFullPath = _UnsentPath + "\\" + _BasicFileName[(int)BasicExtension.Done];
            return _NeedSendBasicFile[(int)BasicExtension.Done];
        }
        public bool IsRemoteSendAutoStart(out string eFileFullPath, out string eLocalFileFullPath)
        {
            eFileFullPath = _DataPath[(int)IPPos.Remote] + "\\" + _BasicFileName[(int)BasicExtension.RemoteSendAutoStart];
            eLocalFileFullPath = _UnsentPath + "\\" + _BasicFileName[(int)BasicExtension.RemoteSendAutoStart];
            return _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStart];
        }
        public bool IsRemoteSendAutoStop(out string eFileFullPath, out string eLocalFileFullPath)
        {
            eFileFullPath = _DataPath[(int)IPPos.Remote] + "\\" + _BasicFileName[(int)BasicExtension.RemoteSendAutoStop];
            eLocalFileFullPath = _UnsentPath + "\\" + _BasicFileName[(int)BasicExtension.RemoteSendAutoStop];
            return _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStop];
        }
        public void ResetNeedSend_Auto() { _NeedSendBasicFile[(int)BasicExtension.Auto] = false; }
        public void ResetNeedSend_Ready() { _NeedSendBasicFile[(int)BasicExtension.Ready] = false; }
        public void ResetNeedSend_Done() { _NeedSendBasicFile[(int)BasicExtension.Done] = false; }
        public void ResetRemoteSendAutoStart() { _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStart] = false; }
        public void ResetRemoteSendAutoStop() { _NeedSendBasicFile[(int)BasicExtension.RemoteSendAutoStop] = false; }
        #endregion

        public bool IsRemoteReady() { return _RemoteReady; }
        public bool IsAutoAlive() { return _AutoAlive; }
        public bool IsDiskConnect() { return _DiskConnected; }
        public bool IsSendReady() { return _IsSendReady; }
        public void SetSendReady() { _IsSendReady = true; }
        public void ResetSendReady() { _IsSendReady = false; }
        public bool IsCanSend() { return _CanSend; }
        public void SetCanSend() { _CanSend = true; }
        public void ResetCanSend() { _CanSend = false; }

        #region 建立檔案名稱
        public string GetFilePath(out string eFileName, string eData, string eExtension, string eCaption = "None", string eDir = "None")
        {
            eFileName = CreatFileName(eData, eExtension, eCaption, eDir);
            return _DataPath[(int)IPPos.Remote] + eFileName;
        }
        private string CreatFileName(string eData, string eExtension, string eCaption, string eDir)
        {
            if (string.IsNullOrEmpty(eData) || string.IsNullOrEmpty(eExtension))
                return "";

            if (string.IsNullOrEmpty(eCaption))
                eCaption = "None";

            return string.Join(_Separator, new string[] { eDir, GetIP_Local("_"), eCaption, eData }) + "." + eExtension;
        }
        #endregion

        #region 連接磁碟機
        public bool TryConnect()
        {
            if (!_DiskConnected)
                ConnectDisk();

            return _DiskConnected;
        }
        /// <summary>連線遠端共享資料夾</summary>
        /// <param name="path">遠端共享資料夾的路徑</param>
        /// <param name="userName">使用者名稱</param>
        /// <param name="passWord">密碼</param>
        /// <returns></returns>
        public void ConnectDisk()
        {
            Disconnect();

            if (!_Enable)
            {
                _DiskConnected = false;
                _RemoteReady = false;
                _NeedSendBasicFile[(int)BasicExtension.Ready] = false;
                return;
            }

            if (_RemoteIP != _LocalIP)
            {
                if (_RemoteIP.Split('.')[0] == "0" || _LocalIP.Split('.')[0] == "0")
                    return;

                Process proc = new Process();
                try
                {
                    proc.StartInfo.FileName = "cmd.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    
                    if (_IPCorrect[(int)IPPos.Remote])
                    {
                        int tickBuf = Environment.TickCount;
                        bool timeOut = false;

                        string dosLine = "net use " + _DataPath[(int)IPPos.Remote] + " " + _Password + " /user:" + _UserName + " /persistent:yes";
                        proc.StandardInput.WriteLine(dosLine);
                        proc.StandardInput.WriteLine("exit");
                        while (!proc.HasExited && !timeOut)
                        {
                            proc.WaitForExit(500);
                            timeOut = Environment.TickCount - tickBuf > _TimeOut;
                        }

                        string errormsg;
                        if (timeOut)
                            errormsg = "連接逾時: " + _DataPath[(int)IPPos.Remote];
                        else
                        {
                            errormsg = proc.StandardError.ReadToEnd();
                            proc.StandardError.Close();
                        }

                        if (string.IsNullOrEmpty(errormsg))
                        {
                            _DiskConnected = true;
                            _NeedSendBasicFile[(int)BasicExtension.Ready] = true;
                        }
                        else
                        {
                            _NeedSendBasicFile[(int)BasicExtension.Ready] = false;
                            throw new Exception(errormsg);
                        }
                    }
                }
                catch
                {
                    Disconnect();
                }
                finally
                {
                    proc.Close();
                    proc.Dispose();
                }
            }
            else
            {
                _DiskConnected = true;
                _RemoteReady = false;
                _NeedSendBasicFile[(int)BasicExtension.Ready] = true;
                return;
            }
        }
        /// <summary>斷開遠端共享資料夾</summary>
        /// <param name="path">遠端共享資料夾的路徑</param>
        /// <returns></returns>
        public void Disconnect()
        {
            if (_RemoteIP != _LocalIP)
            {
                if (_IPCorrect[(int)IPPos.Remote])
                {
                    Process proc = new Process();
                    try
                    {
                        proc.StartInfo.FileName = "cmd.exe";
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardInput = true;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.StartInfo.RedirectStandardError = true;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.Start();

                        _DiskConnected = false;

                        proc.StandardInput.WriteLine("net use " + _DataPath[(int)IPPos.Remote] + " /delete");
                        proc.StandardInput.WriteLine("exit");
                        while (!proc.HasExited)
                        {
                            proc.WaitForExit(1000);
                        }
                        string errormsg = proc.StandardError.ReadToEnd();
                    }
                    catch { }
                    finally
                    {
                        proc.Close();
                        proc.Dispose();
                    }
                }
            }
            else
            {
                _DiskConnected = false;
                return;
            }
        }

        /// <summary>向遠端資料夾儲存本地內容，或者從遠端資料夾下載檔案到本地</summary>
        /// <param name="src">要儲存的檔案的路徑，如果儲存檔案到共享資料夾，這個路徑就是本地檔案路徑如：@"D:\1.avi"</param>
        /// <param name="dst">儲存檔案的路徑，不含名稱及副檔名</param>
        /// <param name="fileName">儲存檔案的名稱以及副檔名</param>
        public static void Transport(string src, string dst, string fileName)
        {
            FileStream inFileStream = new FileStream(src, FileMode.Open);
            if (!Directory.Exists(dst)) { Directory.CreateDirectory(dst); }
            dst = dst + fileName;
            FileStream outFileStream = new FileStream(dst, FileMode.OpenOrCreate);
            byte[] buf = new byte[inFileStream.Length];
            int byteCount;
            while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
            {
                outFileStream.Write(buf, 0, byteCount);
            }

            inFileStream.Flush();
            inFileStream.Close();
            outFileStream.Flush();
            outFileStream.Close();
        }
        #endregion

        private static string[] GetNetAddress()
        {
            List<string> NetAddressInfo = new List<string>();

            // 取得本機名稱
            String strHostName = Dns.GetHostName();
            // 取得本機的 IpHostEntry 類別實體
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            NetAddressInfo.Add(strHostName);

            // 取得所有 IP 位址
            int num = 1;
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                NetAddressInfo.Add(ipaddress.ToString());
                num = num + 1;
            }

            return NetAddressInfo.ToArray();
        }

        public void ParseFileName(FileInfo eFile, out string eIP, out string eCaption, out string eData, out string eExtension)
        {
            string[] _BugStr = Regex.Split(eFile.Name, _Separator);

            if (_BugStr.Length == 4)
            {
                eIP = _BugStr[1];
                eCaption = _BugStr[2];
                eData = Regex.Split(_BugStr[3], eFile.Extension)[0];
                eExtension = eFile.Extension.Trim('.');
            }
            else
            {
                eIP = "";
                eCaption = "";
                eData = "";
                eExtension = "";
            }
        }

        #region Send
        public List<string[]> CheckUnsend()
        {
            List<string[]> _BufStr = new List<string[]>();

            if (_CanSend)
            {
                DirectoryInfo _UnsendDir = new DirectoryInfo(_UnsentPath);
                FileInfo[] _UnsendFiles = _UnsendDir.GetFiles();

                if (_RemoteReady)
                {
                    foreach (FileInfo _UnsendFile in _UnsendFiles)
                    {
                        ParseFileName(_UnsendFile, out string eIP, out string eCaption, out string eData, out string eExtension);

                        try
                        {
                            if (GetIP_Local("_") == eIP)
                            {
                                bool _isBasicExt = false;
                                for (int j = 0; j < (int)BasicExtension.Count; j++)
                                {
                                    if (eExtension.ToLower() == ((BasicExtension)j).ToString().ToLower())
                                        _isBasicExt = true;
                                }
                                if (!_isBasicExt)
                                {
                                    _UnsendFile.CopyTo(_DataPath[(int)IPPos.Remote] + "\\" + _UnsendFile, true);
                                    _UnsendFile.Delete();

                                    _SendIdleTick = Environment.TickCount;//檔案搬移有成功，更新閒置時間
                                }

                                _CanSend = true;
                                _BufStr.Add(new string[] { eIP, "Success, " + eCaption + ": " + eData });
                            }
                        }
                        catch (Exception ex)
                        {
                            _CanSend = false;
                            Disconnect();
                            _BufStr.Add(new string[] { eIP, "Fail, " + eCaption + "; " + ex.Message.ToString() });
                        }
                    }

                    if (_IsRemoteRun && Environment.TickCount - _SendIdleTick > 2500)//超出2.5秒則送出Auto檔案
                    {
                        _SendIdleTick = Environment.TickCount;
                        _NeedSendBasicFile[(int)BasicExtension.Auto] = true;
                    }
                }
                else
                {
                    foreach (FileInfo _UnsendFile in _UnsendFiles)
                    {
                        ParseFileName(_UnsendFile, out string eIP, out string eCaption, out string eData, out string eExtension);

                        if (eCaption == "Test")
                        {
                            try
                            {
                                if (GetIP_Local("_") == eIP)
                                {
                                    bool _isBasicExt = false;
                                    for (int j = 0; j < (int)BasicExtension.Count; j++)
                                    {
                                        if (eExtension.ToLower() == ((BasicExtension)j).ToString().ToLower())
                                            _isBasicExt = true;
                                    }
                                    if (!_isBasicExt)
                                    {
                                        _UnsendFile.CopyTo(_DataPath[(int)IPPos.Remote] + "\\" + _UnsendFile, true);
                                        _UnsendFile.Delete();

                                        _SendIdleTick = Environment.TickCount;//檔案搬移有成功，更新閒置時間
                                    }

                                    _CanSend = true;
                                    _BufStr.Add(new string[] { eIP, "Success, Test" });
                                }
                            }
                            catch (Exception ex)
                            {
                                _CanSend = false;
                                Disconnect();
                                _BufStr.Add(new string[] { eIP, "Fail, Test: " + ex.Message.ToString() });
                            }
                        }
                    }
                }
            }

            return _BufStr;
        }
        #endregion

        #region Take
        public FileInfo[] GetAllTakeList() { lock (_TakeLocalFiles) { return _TakeLocalFiles.ToArray(); } }
        public bool GetNewest(string eExtension, string eCaption, out string eData, out string eFileName, bool eDelete)
        {
            bool _Result = false;
            FileInfo _EarliestFile = null;
            DateTime _Earliest = DateTime.Now;

            lock (_TakeLocalFiles)
            {
                for (int i = 0; i < _TakeLocalFiles.Count; i++)
                {
                    ParseFileName(_TakeLocalFiles[i], out string IP, out string Caption, out string Data, out string Extension);

                    if (Extension == eExtension && Caption == eCaption)
                    {
                        if (_EarliestFile == null || _TakeLocalFiles[i].CreationTime < _Earliest)
                        {
                            _EarliestFile = _TakeLocalFiles[i];
                            _Earliest = _TakeLocalFiles[i].CreationTime;
                        }
                    }
                }

                if (_EarliestFile != null)
                {
                    if (!IsFileOccupied(_EarliestFile.FullName))
                    {
                        _TakeLocalFiles.Clear();

                        try
                        {
                            if (!eDelete)
                                _EarliestFile.CopyTo(_NotDeletePath + "\\" + _EarliestFile.Name);

                            _EarliestFile.Delete();
                        }
                        catch { }

                        _Result = true;
                    }

                    ParseFileName(_EarliestFile, out string IP, out string Caption, out string Data, out string Extension);
                    eData = Data;
                    eFileName = _EarliestFile.Name;
                }
                else
                {
                    eData = "";
                    eFileName = "";
                }
            }

            return _Result;
        }
        public void RenewTake(FileInfo[] eAllFile)
        {
            List<FileInfo> _NeedDelFile = new List<FileInfo>();
            bool _NeedClear = false;

            lock (_TakeLocalFiles)
            {
                if (Environment.TickCount - _AutoAliveTick > 5000)
                    _AutoAlive = false;
                else
                    _AutoAlive = true;

                _TakeLocalFiles.Clear();

                for (int i = 0; i < eAllFile.Length; i++)
                {
                    ParseFileName(eAllFile[i], out string IP, out string Caption, out string Data, out string Extension);

                    if (GetIP_Remote("_") == IP)
                    {
                        _NeedDelFile.Add(eAllFile[i]);

                        if (Extension.ToLower() == BasicExtension.Ready.ToString().ToLower())
                        {
                            _CanTake = false;
                            _NeedClear = true;

                            LogDef.Add(ELogFileName.DataTransfer, "Take", IP, BasicExtension.Ready.ToString());
                        }

                        if (_IsSendReady)//已寄送過Ready
                        {
                            if (Extension.ToLower() == BasicExtension.Done.ToString().ToLower())
                            {
                                _RemoteReady = true;
                                _IsSendReady = false;

                                if (!IsFileOccupied(eAllFile[i].FullName))
                                {
                                    try
                                    {
                                        eAllFile[i].Delete();
                                        LogDef.Add(ELogFileName.DataTransfer, "Done", IP, BasicExtension.Done.ToString());
                                    }
                                    catch { }
                                }
                            }
                        }

                        if (_RemoteReady)//收到Done並遠端PC已清除檔案完成
                        {
                            if (Extension.ToLower() == BasicExtension.RemoteSendAutoStart.ToString().ToLower())
                            {
                                _IsRemoteStop = false;
                                _IsRemoteRun = !_IsRemoteStop;

                                if (!IsFileOccupied(eAllFile[i].FullName))
                                {
                                    try
                                    {
                                        eAllFile[i].Delete();
                                        LogDef.Add(ELogFileName.DataTransfer, "Take", IP, BasicExtension.RemoteSendAutoStart.ToString());
                                    }
                                    catch { }
                                }
                            }
                            if (Extension.ToLower() == BasicExtension.RemoteSendAutoStop.ToString().ToLower())
                            {
                                _IsRemoteStop = true;
                                _IsRemoteRun = !_IsRemoteStop;

                                if (!IsFileOccupied(eAllFile[i].FullName))
                                {
                                    try
                                    {
                                        eAllFile[i].Delete();
                                        LogDef.Add(ELogFileName.DataTransfer, "Take", IP, BasicExtension.RemoteSendAutoStop.ToString());
                                    }
                                    catch { }
                                }
                            }

                            if (Extension.ToLower() == BasicExtension.Auto.ToString().ToLower())
                            {
                                _AutoAliveTick = Environment.TickCount;
                                _CanTake = true;
                                if (!_IsRemoteStop)
                                    _IsRemoteRun = true;

                                if (!IsFileOccupied(eAllFile[i].FullName))
                                {
                                    try
                                    {
                                        eAllFile[i].Delete();
                                        LogDef.Add(ELogFileName.DataTransfer, "Take", IP, BasicExtension.Auto.ToString());
                                    }
                                    catch { }
                                }
                            }
                        }

                        if (_CanTake)//已收到Ready並清除本地檔案 or 收到Done後又收到Auto檔案
                        {
                            #region 其他非基本副檔名的檔案
                            bool _isBasicExt = false;
                            for (int j = 0; j < (int)BasicExtension.Count; j++)
                            {
                                if (Extension.ToLower() == ((BasicExtension)j).ToString().ToLower())
                                    _isBasicExt = true;
                            }
                            if (!_isBasicExt)
                                _TakeLocalFiles.Add(eAllFile[i]);
                            #endregion
                        }
                    }
                }

                if (_NeedClear)
                {
                    List<FileInfo> _NeedDelFileBuf = new List<FileInfo>();

                DelFile:
                    foreach (FileInfo File in _NeedDelFile)
                    {
                        try
                        {
                            File.Delete();
                        }
                        catch
                        {
                            if (File.Exists)//檔案是否存在
                                _NeedDelFileBuf.Add(File);//加入未刪除檔案清單
                        }
                    }

                    if (_NeedDelFileBuf.Count > 0)
                    {
                        _NeedDelFile.Clear();
                        _NeedDelFile = _NeedDelFileBuf;
                        _NeedDelFileBuf.Clear();
                        goto DelFile;
                    }//是否有刪除失敗的檔案

                    _NeedSendBasicFile[(int)BasicExtension.Done] = true;
                    _CanTake = true;
                }

                _NeedDelFile.Clear();
            }
        }
        #endregion

        ~IPListType() { }
    }
}