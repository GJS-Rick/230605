using System;
using System.IO;
using FileStreamLibrary;

namespace CommonLibrary
{
    public enum ECylMotion
    {
        /// <summary>縮回</summary>
        Retract,
        /// <summary>伸出</summary>
        Extension,
    }

    public class CylinderDef : IDisposable
    {
        private string _FilePath;
        private SingleCylinderDef[] _AllCylinder;
        private EDI_TYPE[][] _CylinderIn;
        private EDO_TYPE[][] _CylinderOut;

        /// <summary>氣缸Def</summary>
        /// <param name="eExtension_DI">伸出SensorDI</param>
        /// <param name="eRetract_DI">縮回SensorDI</param>
        /// <param name="eExtension_DO">伸出DO</param>
        /// <param name="eRetract_DO">縮回DO</param>
        /// <remarks>
        /// <list type="伸出DI">eExtension_DI: 伸出SensorDI</list>
        /// <list type="縮回SensorDI">eRetract_DI: 縮回SensorDI</list>
        /// <list type="伸出DO">eExtension_DO: 伸出DO(單線圈電磁閥設定)</list>
        /// <list type="縮回DO">eRetract_DO: 縮回DO</list>
        /// </remarks>
        public CylinderDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei CylinderDef
        {
            _CylinderIn = new EDI_TYPE[(int)ECylName.Count][];
            _CylinderOut = new EDO_TYPE[(int)ECylName.Count][];
            for (int i = 0; i < (int)ECylName.Count; i++)
            {
                _CylinderIn[i] = new EDI_TYPE[2];
                for (int j = 0; j < _CylinderIn[i].Length; j++)
                    _CylinderIn[i][j] = EDI_TYPE.DI_COUNT;

                _CylinderOut[i] = new EDO_TYPE[2];
                for (int j = 0; j < _CylinderIn[i].Length; j++)
                    _CylinderOut[i][j] = EDO_TYPE.DO_COUNT;
            }

            _AllCylinder = new SingleCylinderDef[(int)ECylName.Count];

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~CylinderDef() { }
        public void Dispose()
        {
            for (int i = 0; i < _AllCylinder.Length; i++)
                _AllCylinder[i].Dispose();

            _AllCylinder = null;
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
            for (int i = 0; i < (int)ECylName.Count; i++)
            {
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DI", EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DI", EDI_TYPE.DI_COUNT.ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DO", EDO_TYPE.DO_COUNT.ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DO", EDO_TYPE.DO_COUNT.ToString());
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            string _BufStr = "";

            IniFile iniFileInfo = new IniFile(_FilePath, true);
            for (int i = 0; i < (int)ECylName.Count; i++)
            {
                #region DI
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DI", EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _CylinderIn[i][(int)ECylMotion.Retract] = (EDI_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DI", EDI_TYPE.DI_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _CylinderIn[i][(int)ECylMotion.Extension] = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion

                #region DO
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DO", EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _CylinderOut[i][(int)ECylMotion.Retract] = (EDO_TYPE)j;
                        break;
                    }
                }

                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DO", EDO_TYPE.DO_COUNT.ToString()).Trim();
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _CylinderOut[i][(int)ECylMotion.Extension] = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void WriteFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, false);
            for (int i = 0; i < (int)ECylName.Count; i++)
            {
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DI", _CylinderIn[i][(int)ECylMotion.Retract].ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DI", _CylinderIn[i][(int)ECylMotion.Extension].ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Retract.ToString() + "_DO", _CylinderOut[i][(int)ECylMotion.Retract].ToString());
                iniFileInfo.WriteStr(((ECylName)i).ToString(), ECylMotion.Extension.ToString() + "_DO", _CylinderOut[i][(int)ECylMotion.Extension].ToString());
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        #endregion

        public void Save()
        {
            WriteFile();
            ReNew();
        }
        private void ReNew()
        {
            for (int i = 0; i < (int)ECylName.Count; i++)
                _AllCylinder[i] = new SingleCylinderDef(
                    _CylinderIn[i][(int)ECylMotion.Extension],
                _CylinderIn[i][(int)ECylMotion.Retract],
                _CylinderOut[i][(int)ECylMotion.Extension],
                _CylinderOut[i][(int)ECylMotion.Retract]);
        }
        public void SetCylinderDI(ECylName eCylName, ECylMotion eCylMotion, EDI_TYPE eDI_TYPE) { _CylinderIn[(int)eCylName][(int)eCylMotion] = eDI_TYPE; }
        public void SetCylinderDO(ECylName eCylName, ECylMotion eCylMotion, EDO_TYPE eDO_TYPE) { _CylinderOut[(int)eCylName][(int)eCylMotion] = eDO_TYPE; }
        public EDI_TYPE GetCylinderDI(ECylName eCylName, ECylMotion eCylMotion) { return _CylinderIn[(int)eCylName][(int)eCylMotion]; }
        public EDO_TYPE GetCylinderDO(ECylName eCylName, ECylMotion eCylMotion) { return _CylinderOut[(int)eCylName][(int)eCylMotion]; }

        /// <summary>氣缸Sensor確認</summary>
        /// <param name="eCYL">氣缸名稱</param>
        /// <param name="eHoldTime">訊號保持時間</param>
        /// <remarks>
        /// <list type="訊號保持時間">eHoldTime: DI On保持時間, On的時間大於指定值輸出On(單位ms)</list>
        /// </remarks>
        /// <returns>對應DI布林值</returns>
        public bool CheckIO(ECylName eCYL, int eHoldTime = 0)
        {
            if (eCYL != ECylName.Count)
                return _AllCylinder[(int)eCYL].CheckIO(eHoldTime);

            return true;
        }
        /// <summary>氣缸縮回</summary>
        public void Retract(ECylName eCYL)
        {
            if (eCYL != ECylName.Count)
                _AllCylinder[(int)eCYL].Retract();
        }
        /// <summary>氣缸伸出</summary>
        public void Extension(ECylName eCYL)
        {
            if (eCYL != ECylName.Count)
                _AllCylinder[(int)eCYL].Extension();
        }
        /// <summary>氣缸Sensor名稱</summary>
        public string GetIOName(ECylName eCYL)
        {
            if (eCYL != ECylName.Count)
                return _AllCylinder[(int)eCYL].GetIOName();

            return "Null";
        }
        public ECylMotion GetCYLStatus(ECylName eCYL) { return _AllCylinder[(int)eCYL].GetCylinderStatus(); }
        public ECylMotion GetPreCYLStatus(ECylName eCYL) { return _AllCylinder[(int)eCYL].GetPreCylinderStatus(); }
    }

    public class SingleCylinderDef : IDisposable
    {
        private ECylMotion _PreCylinderAction;
        private ECylMotion _CylinderAction;

        private EDO_TYPE[] _CylinderOut;
        private EDI_TYPE[] _CylinderIn;
        private bool _Done;
        private int _DoneTime;

        /// <summary>單一氣缸Def</summary>
        /// <param name="eExtension_DI">伸出SensorDI</param>
        /// <param name="eRetract_DI">縮回SensorDI</param>
        /// <param name="eExtension_DO">伸出DO</param>
        /// <param name="eRetract_DO">縮回DO</param>
        /// <remarks>
        /// <list type="伸出DI">eExtension_DI: 伸出SensorDI</list>
        /// <list type="縮回伸出SensorDI">eRetract_DI: 縮回SensorDI</list>
        /// <list type="伸出DO">eExtension_DO: 伸出DO(單線圈電磁閥設定)</list>
        /// <list type="縮回DO">eRetract_DO: 縮回DO</list>
        /// </remarks>
        public SingleCylinderDef(EDI_TYPE eExtension_DI, EDI_TYPE eRetract_DI, EDO_TYPE eExtension_DO, EDO_TYPE eRetract_DO)
        {
            _Done = true;
            _DoneTime = Environment.TickCount;

            _CylinderIn = new EDI_TYPE[2];
            _CylinderOut = new EDO_TYPE[2];

            _PreCylinderAction = ECylMotion.Extension;
            _CylinderAction = ECylMotion.Extension;

            _CylinderIn[(int)ECylMotion.Extension] = eExtension_DI;
            _CylinderIn[(int)ECylMotion.Retract] = eRetract_DI;
            _CylinderOut[(int)ECylMotion.Extension] = eExtension_DO;
            _CylinderOut[(int)ECylMotion.Retract] = eRetract_DO;
        }

        ~SingleCylinderDef() { }
        public void Dispose() { }

        /// <summary>氣缸縮回</summary>
        public void Retract()
        {
            _Done = false;
            _PreCylinderAction = _CylinderAction;

            if (_CylinderOut[(int)ECylMotion.Extension] != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_CylinderOut[(int)ECylMotion.Extension], false);
            if (_CylinderOut[(int)ECylMotion.Retract] != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_CylinderOut[(int)ECylMotion.Retract], true);

            _CylinderAction = ECylMotion.Retract;
        }
        /// <summary>氣缸伸出</summary>
        public void Extension()
        {
            _Done = false;
            _PreCylinderAction = _CylinderAction;

            if (_CylinderOut[(int)ECylMotion.Extension] != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_CylinderOut[(int)ECylMotion.Extension], true);
            if (_CylinderOut[(int)ECylMotion.Retract] != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_CylinderOut[(int)ECylMotion.Retract], false);

            _CylinderAction = ECylMotion.Extension;
        }

        /// <summary>氣缸Sensor確認</summary>
        /// <param name="eHoldTime">訊號保持時間</param>
        /// <remarks>
        /// <list type="訊號保持時間">eHoldTime: DI On保持時間, On的時間大於指定值輸出On(單位ms)</list>
        /// </remarks>
        /// <returns>對應DI布林值</returns>
        public bool CheckIO(int eHoldTime = 0)
        {
            switch (_CylinderAction)
            {
                case ECylMotion.Retract:
                    if (_CylinderIn[(int)ECylMotion.Extension] == EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                    {
                        if (G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Retract], true))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] == EDI_TYPE.DI_COUNT)
                    {
                        if (!G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Extension], false))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                    {
                        if (!G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Extension], false) && G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Retract], true))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else
                    {
                        _Done = true;
                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                case ECylMotion.Extension:
                    if (_CylinderIn[(int)ECylMotion.Extension] == EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                    {
                        if (!G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Retract], false))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] == EDI_TYPE.DI_COUNT)
                    {
                        if (G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Extension], true))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                    {
                        if (G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Extension], true) && !G.Comm.IOCtrl.GetDI(_CylinderIn[(int)ECylMotion.Retract], false))
                            _Done = true;
                        else
                            _DoneTime = Environment.TickCount;

                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
                    else
                    {
                        _Done = true;
                        return (_Done && Environment.TickCount - _DoneTime >= eHoldTime);
                    }
            }

            return false;
        }

        /// <summary>氣缸Sensor名稱</summary>
        public string GetIOName()
        {
            switch (_CylinderAction)
            {
                case ECylMotion.Retract:
                    if (_CylinderIn[(int)ECylMotion.Extension] == EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Retract], true);
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] == EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Extension], false);
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Extension], false) + G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Retract], true);
                    else
                        return "Null sensor";
                case ECylMotion.Extension:
                    if (_CylinderIn[(int)ECylMotion.Extension] == EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Retract], false);
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] == EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Extension], true);
                    else if (_CylinderIn[(int)ECylMotion.Extension] != EDI_TYPE.DI_COUNT && _CylinderIn[(int)ECylMotion.Retract] != EDI_TYPE.DI_COUNT)
                        return G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Extension], true) + G.Comm.IOCtrl.GetDINameWithStatus(_CylinderIn[(int)ECylMotion.Retract], false);
                    else
                        return "Null sensor";
            }

            return "Get Cylinder Name Err";
        }

        public ECylMotion GetCylinderStatus() { return _CylinderAction; }
        public ECylMotion GetPreCylinderStatus() { return _PreCylinderAction; }
    }
}