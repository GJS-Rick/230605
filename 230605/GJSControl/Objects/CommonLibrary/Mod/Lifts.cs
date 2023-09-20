using System;
using System.Drawing;
using System.IO;
using CommonLibrary;
using Emgu.CV.Tracking;
using FileStreamLibrary;
using static CommonLibrary.SingleLifts;

namespace CommonLibrary
{


    public class LiftsDef : IDisposable
    {
        private string _FilePath;

        private SingleLifts[] _AllLifts;
        private LiftsInfo[] _PreLiftsInfo;
        private LiftsInfo[] _LiftsInfo;

        /// <summary>升降台Def</summary>
        public LiftsDef(string sFilePath = "C:\\Automation\\Mod.ini")// BoWei TrolleyDef
        {
            _AllLifts = new SingleLifts[(int)ELifts.Count];
            _PreLiftsInfo = new LiftsInfo[(int)ELifts.Count];
            _LiftsInfo = new LiftsInfo[(int)ELifts.Count];

            for (int i = 0; i < (int)ELifts.Count; i++)
            {
                _PreLiftsInfo[i].InPlace = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].ULim_Board = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].ULim_Work = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].LLim_Work = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].ULim_Move = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].LLim_Move = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].HavePallet = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].BoardOnPallet = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].MotorError = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].LiftsDown = EDO_TYPE.DO_COUNT;
                _PreLiftsInfo[i].LiftsUp = EDO_TYPE.DO_COUNT;
                _PreLiftsInfo[i].LiftsBtnUp = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].LiftsBtnUpLED = EDO_TYPE.DO_COUNT;
                _PreLiftsInfo[i].LiftsBtnDown = EDI_TYPE.DI_COUNT;
                _PreLiftsInfo[i].LiftsBtnDownLED = EDO_TYPE.DO_COUNT;

                _LiftsInfo[i].InPlace = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].ULim_Board = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].ULim_Work = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].LLim_Work = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].ULim_Move = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].LLim_Move = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].HavePallet = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].BoardOnPallet = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].MotorError = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].LiftsDown = EDO_TYPE.DO_COUNT;
                _LiftsInfo[i].LiftsUp = EDO_TYPE.DO_COUNT;
                _LiftsInfo[i].LiftsBtnUp = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].LiftsBtnUpLED = EDO_TYPE.DO_COUNT;
                _LiftsInfo[i].LiftsBtnDown = EDI_TYPE.DI_COUNT;
                _LiftsInfo[i].LiftsBtnDownLED = EDO_TYPE.DO_COUNT;
            }

            _FilePath = sFilePath;
            if (CheckFile(_FilePath))
                ReadFile();
            else
                CreateFile();

            ReNew();
        }

        ~LiftsDef() { }
        public void Dispose()
        {
            for (int i = 0; i < _AllLifts.Length; i++)
                _AllLifts[i].Dispose();

            _AllLifts = null;
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
            for (int i = 0; i < (int)ELifts.Count; i++)
            {
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "InPlace_Board", _LiftsInfo[i].InPlace.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Board", _LiftsInfo[i].ULim_Board.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Work", _LiftsInfo[i].ULim_Work.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Work", _LiftsInfo[i].LLim_Work.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Move", _LiftsInfo[i].ULim_Move.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Move", _LiftsInfo[i].LLim_Move.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock1", _LiftsInfo[i].SafeLock1.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock2", _LiftsInfo[i].SafeLock2.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Pallet", _LiftsInfo[i].HavePallet.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BoardOnPallet", _LiftsInfo[i].BoardOnPallet.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Err", _LiftsInfo[i].MotorError.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Down", _LiftsInfo[i].LiftsDown.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Up", _LiftsInfo[i].LiftsUp.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnUp", _LiftsInfo[i].LiftsBtnUp.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnUpLED", _LiftsInfo[i].LiftsBtnUpLED.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnDown", _LiftsInfo[i].LiftsBtnDown.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnDownLED", _LiftsInfo[i].LiftsBtnDownLED.ToString());
            }
            iniFileInfo.FileClose();
            iniFileInfo.Dispose();
        }
        private void ReadFile()
        {
            IniFile iniFileInfo = new IniFile(_FilePath, true);
            string _BufStr = "";

            for (int i = 0; i < (int)ELifts.Count; i++)
            {
                #region InPlace
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "InPlace_Board", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].InPlace = (EDI_TYPE)j;
                        _LiftsInfo[i].InPlace = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region ULim_Board
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Board", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].ULim_Board = (EDI_TYPE)j;
                        _LiftsInfo[i].ULim_Board = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region ULim_Work
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Work", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].ULim_Work = (EDI_TYPE)j;
                        _LiftsInfo[i].ULim_Work = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LLim_Work
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Work", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LLim_Work = (EDI_TYPE)j;
                        _LiftsInfo[i].LLim_Work = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region ULim_Move
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Move", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].ULim_Move = (EDI_TYPE)j;
                        _LiftsInfo[i].ULim_Move = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LLim_Move
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Move", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LLim_Move = (EDI_TYPE)j;
                        _LiftsInfo[i].LLim_Move = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region SafeLock1
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock1", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].SafeLock1 = (EDI_TYPE)j;
                        _LiftsInfo[i].SafeLock1 = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region SafeLock2
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock2", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].SafeLock2 = (EDI_TYPE)j;
                        _LiftsInfo[i].SafeLock2 = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region HavePallet
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "Pallet", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].HavePallet = (EDI_TYPE)j;
                        _LiftsInfo[i].HavePallet = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region BoardOnPallet
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "BoardOnPallet", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].BoardOnPallet = (EDI_TYPE)j;
                        _LiftsInfo[i].BoardOnPallet = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region MotorError
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "Err", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].MotorError = (EDI_TYPE)j;
                        _LiftsInfo[i].MotorError = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsDown
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "Down", EDO_TYPE.DO_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsDown = (EDO_TYPE)j;
                        _LiftsInfo[i].LiftsDown = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsUp
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "Up", EDO_TYPE.DO_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsUp = (EDO_TYPE)j;
                        _LiftsInfo[i].LiftsUp = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsBtnUp
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "BtnUp", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsBtnUp = (EDI_TYPE)j;
                        _LiftsInfo[i].LiftsBtnUp = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsBtnUpLED
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "BtnUpLED", EDO_TYPE.DO_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsBtnUpLED = (EDO_TYPE)j;
                        _LiftsInfo[i].LiftsBtnUpLED = (EDO_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsBtnDown
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "BtnDown", EDI_TYPE.DI_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDI_TYPE.DI_COUNT; j++)
                {
                    if (((EDI_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsBtnDown = (EDI_TYPE)j;
                        _LiftsInfo[i].LiftsBtnDown = (EDI_TYPE)j;
                        break;
                    }
                }
                #endregion
                #region LiftsBtnDownLED
                _BufStr = "";
                _BufStr = iniFileInfo.ReadStr("Lifts_" + ((ELifts)i).ToString(), "BtnDownLED", EDO_TYPE.DO_COUNT.ToString()).Trim();

                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    if (((EDO_TYPE)j).ToString() == _BufStr)
                    {
                        _PreLiftsInfo[i].LiftsBtnDownLED = (EDO_TYPE)j;
                        _LiftsInfo[i].LiftsBtnDownLED = (EDO_TYPE)j;
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
            for (int i = 0; i < (int)ELifts.Count; i++)
            {
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "InPlace_Board", _LiftsInfo[i].InPlace.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Board", _LiftsInfo[i].ULim_Board.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Work", _LiftsInfo[i].ULim_Work.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Work", _LiftsInfo[i].LLim_Work.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "UpperLimit_Move", _LiftsInfo[i].ULim_Move.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "LowerLimit_Move", _LiftsInfo[i].LLim_Move.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock1", _LiftsInfo[i].SafeLock1.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "SafeLock2", _LiftsInfo[i].SafeLock2.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Pallet", _LiftsInfo[i].HavePallet.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BoardOnPallet", _LiftsInfo[i].BoardOnPallet.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Err", _LiftsInfo[i].MotorError.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Down", _LiftsInfo[i].LiftsDown.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "Up", _LiftsInfo[i].LiftsUp.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnUp", _LiftsInfo[i].LiftsBtnUp.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnUpLED", _LiftsInfo[i].LiftsBtnUpLED.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnDown", _LiftsInfo[i].LiftsBtnDown.ToString());
                iniFileInfo.WriteStr("Lifts_" + ((ELifts)i).ToString(), "BtnDownLED", _LiftsInfo[i].LiftsBtnDownLED.ToString());
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
            for (int i = 0; i < (int)ELifts.Count; i++)
            {
                _AllLifts[i] = new SingleLifts(
                    _LiftsInfo[i].InPlace,
                    _LiftsInfo[i].ULim_Board,
                    _LiftsInfo[i].ULim_Work,
                    _LiftsInfo[i].LLim_Work,
                    _LiftsInfo[i].ULim_Move,
                    _LiftsInfo[i].LLim_Move,
                    _LiftsInfo[i].SafeLock1,
                    _LiftsInfo[i].SafeLock2,
                    _LiftsInfo[i].HavePallet,
                    _LiftsInfo[i].BoardOnPallet,
                    _LiftsInfo[i].MotorError,
                    _LiftsInfo[i].LiftsUp,
                    _LiftsInfo[i].LiftsDown,
                    _LiftsInfo[i].LiftsBtnUp,
                    _LiftsInfo[i].LiftsBtnUpLED,
                    _LiftsInfo[i].LiftsBtnDown,
                    _LiftsInfo[i].LiftsBtnDownLED);

                #region InPlace
                if (_PreLiftsInfo[i].InPlace != _LiftsInfo[i].InPlace)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "InPlace", _PreLiftsInfo[i].InPlace.ToString() + " to " + _LiftsInfo[i].InPlace.ToString());
                    _PreLiftsInfo[i].InPlace = _LiftsInfo[i].InPlace;
                }
                #endregion
                #region ULim_Board
                if (_PreLiftsInfo[i].ULim_Board != _LiftsInfo[i].ULim_Board)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "ULim_Board", _PreLiftsInfo[i].ULim_Board.ToString() + " to " + _LiftsInfo[i].ULim_Board.ToString());
                    _PreLiftsInfo[i].ULim_Board = _LiftsInfo[i].ULim_Board;
                }
                #endregion
                #region ULim_Work
                if (_PreLiftsInfo[i].ULim_Work != _LiftsInfo[i].ULim_Work)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "ULim_Work", _PreLiftsInfo[i].ULim_Work.ToString() + " to " + _LiftsInfo[i].ULim_Work.ToString());
                    _PreLiftsInfo[i].ULim_Work = _LiftsInfo[i].ULim_Work;
                }
                #endregion
                #region LLim_Work
                if (_PreLiftsInfo[i].LLim_Work != _LiftsInfo[i].LLim_Work)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LLim_Work", _PreLiftsInfo[i].LLim_Work.ToString() + " to " + _LiftsInfo[i].LLim_Work.ToString());
                    _PreLiftsInfo[i].LLim_Work = _LiftsInfo[i].LLim_Work;
                }
                #endregion
                #region ULim_Move
                if (_PreLiftsInfo[i].ULim_Move != _LiftsInfo[i].ULim_Move)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "ULim_Move", _PreLiftsInfo[i].ULim_Move.ToString() + " to " + _LiftsInfo[i].ULim_Move.ToString());
                    _PreLiftsInfo[i].ULim_Move = _LiftsInfo[i].ULim_Move;
                }
                #endregion
                #region LLim_Move
                if (_PreLiftsInfo[i].LLim_Move != _LiftsInfo[i].LLim_Move)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LLim_Move", _PreLiftsInfo[i].LLim_Move.ToString() + " to " + _LiftsInfo[i].LLim_Move.ToString());
                    _PreLiftsInfo[i].LLim_Move = _LiftsInfo[i].LLim_Move;
                }
                #endregion
                #region HavePallet
                if (_PreLiftsInfo[i].HavePallet != _LiftsInfo[i].HavePallet)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "HavePallet", _PreLiftsInfo[i].HavePallet.ToString() + " to " + _LiftsInfo[i].HavePallet.ToString());
                    _PreLiftsInfo[i].HavePallet = _LiftsInfo[i].HavePallet;
                }
                #endregion
                #region BoardOnPallet
                if (_PreLiftsInfo[i].BoardOnPallet != _LiftsInfo[i].BoardOnPallet)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "BoardOnPallet", _PreLiftsInfo[i].BoardOnPallet.ToString() + " to " + _LiftsInfo[i].BoardOnPallet.ToString());
                    _PreLiftsInfo[i].BoardOnPallet = _LiftsInfo[i].BoardOnPallet;
                }
                #endregion
                #region MotorError
                if (_PreLiftsInfo[i].MotorError != _LiftsInfo[i].MotorError)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "MotorError", _PreLiftsInfo[i].MotorError.ToString() + " to " + _LiftsInfo[i].MotorError.ToString());
                    _PreLiftsInfo[i].MotorError = _LiftsInfo[i].MotorError;
                }
                #endregion
                #region LiftsDown
                if (_PreLiftsInfo[i].LiftsDown != _LiftsInfo[i].LiftsDown)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsDown", _PreLiftsInfo[i].LiftsDown.ToString() + " to " + _LiftsInfo[i].LiftsDown.ToString());
                    _PreLiftsInfo[i].LiftsDown = _LiftsInfo[i].LiftsDown;
                }
                #endregion
                #region LiftsUp
                if (_PreLiftsInfo[i].LiftsUp != _LiftsInfo[i].LiftsUp)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsUp", _PreLiftsInfo[i].LiftsUp.ToString() + " to " + _LiftsInfo[i].LiftsUp.ToString());
                    _PreLiftsInfo[i].LiftsUp = _LiftsInfo[i].LiftsUp;
                }
                #endregion

                #region LiftsBtnUp
                if (_PreLiftsInfo[i].LiftsBtnUp != _LiftsInfo[i].LiftsBtnUp)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsBtnUp", _PreLiftsInfo[i].LiftsBtnUp.ToString() + " to " + _LiftsInfo[i].LiftsBtnUp.ToString());
                    _PreLiftsInfo[i].LiftsBtnUp = _LiftsInfo[i].LiftsBtnUp;
                }
                #endregion
                #region LiftsBtnUpLED
                if (_PreLiftsInfo[i].LiftsBtnUpLED != _LiftsInfo[i].LiftsBtnUpLED)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsBtnUpLED", _PreLiftsInfo[i].LiftsBtnUpLED.ToString() + " to " + _LiftsInfo[i].LiftsBtnUpLED.ToString());
                    _PreLiftsInfo[i].LiftsBtnUpLED = _LiftsInfo[i].LiftsBtnUpLED;
                }
                #endregion
                #region LiftsBtnDown
                if (_PreLiftsInfo[i].LiftsBtnDown != _LiftsInfo[i].LiftsBtnDown)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsBtnDown", _PreLiftsInfo[i].LiftsBtnDown.ToString() + " to " + _LiftsInfo[i].LiftsBtnDown.ToString());
                    _PreLiftsInfo[i].LiftsBtnDown = _LiftsInfo[i].LiftsBtnDown;
                }
                #endregion
                #region LiftsBtnDownLED
                if (_PreLiftsInfo[i].LiftsBtnDownLED != _LiftsInfo[i].LiftsBtnDownLED)
                {
                    LogDef.Add(ELogFileName.MachineData, ((ELifts)i).ToString(), "LiftsBtnDownLED", _PreLiftsInfo[i].LiftsBtnDownLED.ToString() + " to " + _LiftsInfo[i].LiftsBtnDownLED.ToString());
                    _PreLiftsInfo[i].LiftsBtnDownLED = _LiftsInfo[i].LiftsBtnDownLED;
                }
                #endregion
            }
        }

        #region Board
        #region Board InPlace
        public bool OnInPlace(ELifts eELifts) { return _AllLifts[(int)eELifts].OnInPlace(true); }
        public bool LeaveInPlace(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnInPlace(false); }

        public string GetInPlaceStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_InPlace(); }
        public EDI_TYPE GetInPlace(ELifts eELifts) { return _LiftsInfo[(int)eELifts].InPlace; }
        public void SetInPlace(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].InPlace = eDI; }
        #endregion

        #region Board ULim
        public bool OnULim_Board(ELifts eELifts) { return _AllLifts[(int)eELifts].OnULim_Board(true); }
        public bool LeaveULim_Board(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnULim_Board(false); }

        public string GetULim_BoardStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_ULim_Board(); }
        public EDI_TYPE GetULim_Board(ELifts eELifts) { return _LiftsInfo[(int)eELifts].ULim_Board; }
        public void SetULim_Board(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].ULim_Board = eDI; }
        #endregion
        #endregion

        #region Work
        #region Work ULim
        public bool OnULim_Work(ELifts eELifts) { return _AllLifts[(int)eELifts].OnULim_Work(true); }
        public bool LeaveULim_Work(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnULim_Work(false); }

        public string GetULim_WorkStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_ULim_Work(); }
        public EDI_TYPE GetULim_Work(ELifts eELifts) { return _LiftsInfo[(int)eELifts].ULim_Work; }
        public void SetULim_Work(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].ULim_Work = eDI; }
        #endregion

        #region Work LLim
        public bool OnLLim_Work(ELifts eELifts) { return _AllLifts[(int)eELifts].OnLLim_Work(true); }
        public bool LeaveLLim_Work(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnLLim_Work(false); }

        public string GetLLim_WorkStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_LLim_Work(); }
        public EDI_TYPE GetLLim_Work(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LLim_Work; }
        public void SetLLim_Work(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].LLim_Work = eDI; }
        #endregion
        #endregion

        #region Move
        #region Move ULim
        public bool OnULim_Move(ELifts eELifts) { return _AllLifts[(int)eELifts].OnULim_Move(true); }
        public bool LeaveULim_Move(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnULim_Move(false); }

        public string GetULim_MoveStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_ULim_Move(); }
        public EDI_TYPE GetULim_Move(ELifts eELifts) { return _LiftsInfo[(int)eELifts].ULim_Move; }
        public void SetULim_Move(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].ULim_Move = eDI; }
        #endregion

        #region Move LLim
        public bool OnLLim_Move(ELifts eELifts) { return _AllLifts[(int)eELifts].OnLLim_Move(true); }
        public bool LeaveLLim_Move(ELifts eELifts) { return !_AllLifts[(int)eELifts].OnLLim_Move(false); }

        public string GetLLim_MoveStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_LLim_Move(); }
        public EDI_TYPE GetLLim_Move(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LLim_Move; }
        public void SetLLim_Move(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].LLim_Move = eDI; }
        #endregion
        #endregion

        #region SafeLock
        public bool IsSafeLock(ELifts eELifts) { return _AllLifts[(int)eELifts].IsSafeLock(true); }
        public string GetSafeLockStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_SafeLock(); }
        #region SafeLock 1
        public string GetSafeLock1StatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_SafeLock1(); }
        public EDI_TYPE GetSafeLock1(ELifts eELifts) { return _LiftsInfo[(int)eELifts].SafeLock1; }
        public void SetSafeLock1(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].SafeLock1 = eDI; }
        #endregion

        #region SafeLock 2
        public string GetSafeLock2StatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_SafeLock2(); }
        public EDI_TYPE GetSafeLock2(ELifts eELifts) { return _LiftsInfo[(int)eELifts].SafeLock2; }
        public void SetSafeLock2(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].SafeLock2 = eDI; }
        #endregion
        #endregion

        #region Pallet
        #region Have Pallet
        public bool HavePallet(ELifts eELifts) { return _AllLifts[(int)eELifts].HavePallet(true); }
        public bool NoPallet(ELifts eELifts) { return !_AllLifts[(int)eELifts].HavePallet(false); }

        public string GetPalletStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_HavePallet(); }
        public EDI_TYPE GetPallet(ELifts eELifts) { return _LiftsInfo[(int)eELifts].HavePallet; }
        public void SetPallet(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].HavePallet = eDI; }
        #endregion

        #region Board on Pallet
        public bool BoardOnPallet(ELifts eELifts) { return _AllLifts[(int)eELifts].BoardOnPallet(true); }
        public bool NoBoardOnPallet(ELifts eELifts) { return !_AllLifts[(int)eELifts].BoardOnPallet(false); }

        public string GetOnPalletStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_BoardOnPallet(); }
        public EDI_TYPE GetOnPallet(ELifts eELifts) { return _LiftsInfo[(int)eELifts].BoardOnPallet; }
        public void SetOnPallet(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].BoardOnPallet = eDI; }
        #endregion
        #endregion

        #region Motor
        public bool MotorErr(ELifts eELifts) { return _AllLifts[(int)eELifts].MotorErr(false); }
        public bool MotorNoErr(ELifts eELifts) { return !_AllLifts[(int)eELifts].MotorErr(false); }

        public string GetMotorErrorStatusName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetStatusName_MotorErr(); }
        public string GetMotorErrorName(ELifts eELifts) { return _AllLifts[(int)eELifts].GetName_MotorErr(); }
        public EDI_TYPE GetMotorError(ELifts eELifts) { return _LiftsInfo[(int)eELifts].MotorError; }
        public void SetMotorError(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].MotorError = eDI; }
        #endregion

        public void CheckBtnUDClick()
        {
            for (int i = 0; i < _AllLifts.Length; i++)
                _AllLifts[i].CheckBtnUDClick();
        }

        public void Up(ELifts eELifts, bool bPassULim_B = false) { _AllLifts[(int)eELifts].Up(bPassULim_B); }
        public void Down(ELifts eELifts) { _AllLifts[(int)eELifts].Down(); }
        public void Stop(ELifts eELifts) { _AllLifts[(int)eELifts].Stop(); }
        public void Pause(ELifts eELifts, bool bNeedPause, EMotion eMotion = EMotion.Count)
        {
            switch (_AllLifts[(int)eELifts].NowMotion())
            {
                case EMotion.Down:
                    if (bNeedPause)
                        _AllLifts[(int)eELifts].Stop();
                    break;
                case EMotion.Stop:
                    {
                        if (bNeedPause)
                            break;

                        switch (eMotion)
                        {
                            case EMotion.Down:
                                _AllLifts[(int)eELifts].Down();
                                break;
                            case EMotion.Stop:
                                _AllLifts[(int)eELifts].Stop();
                                break;
                            case EMotion.Up:
                                _AllLifts[(int)eELifts].Up(_AllLifts[(int)eELifts].IsPassULim_B());
                                break;
                            default:
                                {
                                    switch (_AllLifts[(int)eELifts].ProMotion())
                                    {
                                        case EMotion.Down:
                                            _AllLifts[(int)eELifts].Down();
                                            break;
                                        case EMotion.Up:
                                            _AllLifts[(int)eELifts].Up(_AllLifts[(int)eELifts].IsPassULim_B());
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case EMotion.Up:
                    if (bNeedPause)
                        _AllLifts[(int)eELifts].Stop();
                    break;
            }
        }

        public EDO_TYPE GetDown(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsDown; }
        public void SetDown(ELifts eELifts, EDO_TYPE eDO) { _LiftsInfo[(int)eELifts].LiftsDown = eDO; }
        public EDO_TYPE GetUp(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsUp; }
        public void SetUp(ELifts eELifts, EDO_TYPE eDO) { _LiftsInfo[(int)eELifts].LiftsUp = eDO; }

        public EDI_TYPE GetBtnUp(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsBtnUp; }
        public void SetBtnUp(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].LiftsBtnUp = eDI; }
        public EDO_TYPE GetBtnUpLED(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsBtnUpLED; }
        public void SetBtnUpLED(ELifts eELifts, EDO_TYPE eDO) { _LiftsInfo[(int)eELifts].LiftsBtnUpLED = eDO; }
        public EDI_TYPE GetBtnDown(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsBtnDown; }
        public void SetBtnDown(ELifts eELifts, EDI_TYPE eDI) { _LiftsInfo[(int)eELifts].LiftsBtnDown = eDI; }
        public EDO_TYPE GetBtnDownLED(ELifts eELifts) { return _LiftsInfo[(int)eELifts].LiftsBtnDownLED; }
        public void SetBtnDownLED(ELifts eELifts, EDO_TYPE eDO) { _LiftsInfo[(int)eELifts].LiftsBtnDownLED = eDO; }
    }

    public class SingleLifts : IDisposable
    {
        public struct LiftsInfo
        {
            /// <summary>有板到位Sensor</summary>
            public EDI_TYPE InPlace;
            public bool Readity_InPlace;
            /// <summary>有板上極限</summary>
            public EDI_TYPE ULim_Board;
            public bool Readity_ULim_Board;
            /// <summary>升降載台工作上極限</summary>
            public EDI_TYPE ULim_Work;
            public bool Readity_ULim_Work;
            /// <summary>升降載台工作下極限</summary>
            public EDI_TYPE LLim_Work;
            public bool Readity_LLim_Work;
            /// <summary>升降載台移動上極限</summary>
            public EDI_TYPE ULim_Move;
            public bool Readity_ULim_Move;
            /// <summary>升降載台移動下極限</summary>
            public EDI_TYPE LLim_Move;
            public bool Readity_LLim_Move;
            /// <summary>升降載台棧板安全扣1</summary>
            public EDI_TYPE SafeLock1;
            public bool Readity_SafeLock1;
            /// <summary>升降載台棧板安全扣2</summary>
            public EDI_TYPE SafeLock2;
            public bool Readity_SafeLock2;
            /// <summary>棧板有無Sensor</summary>
            public EDI_TYPE HavePallet;
            public bool Readity_HavePallet;
            /// <summary>有無板在棧板上</summary>
            public EDI_TYPE BoardOnPallet;
            public bool Readity_BoardOnPallet;
            /// <summary>升降載台馬達異常</summary>
            public EDI_TYPE MotorError;
            public bool Readity_MotorError;
            /// <summary>升降載台下降</summary>
            public EDO_TYPE LiftsDown;
            public bool Readity_LiftsDown;
            /// <summary>升降載台上升</summary>
            public EDO_TYPE LiftsUp;
            public bool Readity_LiftsUp;

            /// <summary>升降載台上升按鈕</summary>
            public EDI_TYPE LiftsBtnUp;
            /// <summary>升降載台上升按鈕LED</summary>
            public EDO_TYPE LiftsBtnUpLED;
            /// <summary>升降載台下降按鈕</summary>
            public EDI_TYPE LiftsBtnDown;
            /// <summary>升降載台下降按鈕LED</summary>
            public EDO_TYPE LiftsBtnDownLED;
        }

        public enum EMotion
        {
            Down,
            Stop,
            Up,

            Count
        }

        private LiftsInfo _Info;
        private EMotion _ProMotion;
        private EMotion _NowMotion;
        private bool _PassULim_B;

        /// <summary></summary>
        /// <param name="iInPlace">有板到位Sensor</param>
        /// <param name="iULimi_B">有板上極限</param>
        /// <param name="iULimi_W">升降載台工作上極限</param>
        /// <param name="iLLimi_W">升降載台工作下極限</param>
        /// <param name="iULimi_M">升降載台移動上極限</param>
        /// <param name="iLLimi_M">升降載台移動下極限</param>
        /// <param name="iPallet">棧板有無Sensor</param>
        /// <param name="iOnPallet">有無板在棧板上</param>
        /// <param name="iMErr">升降載台馬達異常</param>
        /// <param name="iDown">升降載台下降</param>
        /// <param name="iUp">升降載台上升</param>
        /// <param name="iBtnUp">升降載台上升按鈕</param>
        /// <param name="iBtnLEDUp">升降載台上升按鈕LED</param>
        /// <param name="iBtnDown">升降載台下降按鈕</param>
        /// <param name="iBtnLEDDown">升降載台下降按鈕LED</param>
        /// <remarks>
        /// <list type="有板到位Sensor">iInPlace: 有板到位Sensor</list>
        /// <list type="有板上極限">iULimi_B: 有板上極限</list>
        /// <list type="升降載台工作上極限">iULimi_W: 升降載台工作上極限</list>
        /// <list type="升降載台工作下極限">iLLimi_W: 升降載台工作下極限</list>
        /// <list type="升降載台移動上極限">iULimi_M: 升降載台移動上極限</list>
        /// <list type="升降載台移動下極限">iLLimi_M: 升降載台移動下極限</list>
        /// <list type="棧板有無Sensor">iPallet: 棧板有無Sensor</list>
        /// <list type="有無板在棧板上">iOnPallet: 有無板在棧板上</list>
        /// <list type="升降載台馬達異常">iMErr: 升降載台馬達異常</list>
        /// <list type="升降載台下降">iDown: 升降載台下降</list>
        /// <list type="升降載台上升">iUp: 升降載台上升</list>
        /// <list type="升降載台上升按鈕">iBtnUp: 升降載台上升按鈕</list>
        /// <list type="升降載台上升按鈕LED">iBtnLEDUp: 升降載台上升按鈕LED</list>
        /// <list type="升降載台下降按鈕">iBtnDown: 升降載台下降按鈕</list>
        /// <list type="升降載台下降按鈕LED">iBtnLEDDown: 升降載台下降按鈕LED</list>
        /// </remarks>
        public SingleLifts(EDI_TYPE iInPlace,
            EDI_TYPE iULimi_B,
            EDI_TYPE iULimi_W,
            EDI_TYPE iLLimi_W,
            EDI_TYPE iULimi_M,
            EDI_TYPE iLLimi_M,
            EDI_TYPE iSafeLock1,
            EDI_TYPE iSafeLock2,
            EDI_TYPE iPallet,
            EDI_TYPE iOnPallet,
            EDI_TYPE iMErr,
            EDO_TYPE iDown,
            EDO_TYPE iUp,
            EDI_TYPE iBtnUp,
            EDO_TYPE iBtnLEDUp,
            EDI_TYPE iBtnDown,
            EDO_TYPE iBtnLEDDown)
        {
            _PassULim_B = false;

            _Info.InPlace = iInPlace;
            _Info.Readity_InPlace = false;
            _Info.ULim_Board = iULimi_B;
            _Info.Readity_ULim_Board = false;
            _Info.ULim_Work = iULimi_W;
            _Info.Readity_ULim_Work = false;
            _Info.LLim_Work = iLLimi_W;
            _Info.Readity_LLim_Work = false;
            _Info.ULim_Move = iULimi_M;
            _Info.Readity_ULim_Move = false;
            _Info.LLim_Move = iLLimi_M;
            _Info.Readity_LLim_Move = false;
            _Info.SafeLock1 = iSafeLock1;
            _Info.Readity_SafeLock1 = false;
            _Info.SafeLock2 = iSafeLock2;
            _Info.Readity_SafeLock2 = false;
            _Info.HavePallet = iPallet;
            _Info.Readity_HavePallet = false;
            _Info.BoardOnPallet = iOnPallet;
            _Info.Readity_BoardOnPallet = false;
            _Info.MotorError = iMErr;
            _Info.Readity_MotorError = false;
            _Info.LiftsDown = iDown;
            _Info.Readity_LiftsDown = false;
            _Info.LiftsUp = iUp;
            _Info.Readity_LiftsUp = false;

            _Info.LiftsBtnUp = iBtnUp;
            _Info.LiftsBtnUpLED = iBtnLEDUp;
            _Info.LiftsBtnDown = iBtnDown;
            _Info.LiftsBtnDownLED = iBtnLEDDown;

            _ProMotion = EMotion.Stop;
            _NowMotion = EMotion.Stop;
        }

        ~SingleLifts() { }
        public void Dispose() { }

        public bool MotorErr(bool bReadity)
        {
            if (_Info.MotorError == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_MotorError = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.MotorError, _Info.Readity_MotorError);
        }
        public string GetStatusName_MotorErr() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.MotorError, _Info.Readity_MotorError); }
        public string GetName_MotorErr()
        {
            if (_Info.MotorError == EDI_TYPE.DI_COUNT)
                return "None";

            return G.Comm.IOCtrl.GetDIName(_Info.MotorError);
        }

        public bool HavePallet(bool bReadity)
        {
            if (_Info.HavePallet == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_HavePallet = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.HavePallet, _Info.Readity_HavePallet);
        }
        public string GetStatusName_HavePallet() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.HavePallet, _Info.Readity_HavePallet); }
        public bool BoardOnPallet(bool bReadity)
        {
            if (_Info.BoardOnPallet == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_BoardOnPallet = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.BoardOnPallet, _Info.Readity_BoardOnPallet);
        }
        public string GetStatusName_BoardOnPallet() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.BoardOnPallet, _Info.Readity_BoardOnPallet); }
        public bool OnULim_Board(bool bReadity)
        {
            if (_Info.ULim_Board == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_ULim_Board = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.ULim_Board, _Info.Readity_ULim_Board);
        }
        public string GetStatusName_ULim_Board() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.ULim_Board, _Info.Readity_ULim_Board); }
        public bool OnInPlace(bool bReadity)
        {
            if (_Info.InPlace == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_InPlace = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.InPlace, _Info.Readity_InPlace);
        }
        public string GetStatusName_InPlace() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.InPlace, _Info.Readity_InPlace); }
        public bool OnULim_Move(bool bReadity)
        {
            if (_Info.ULim_Move == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_ULim_Move = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.ULim_Move, _Info.Readity_ULim_Move);
        }
        public string GetStatusName_ULim_Move() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.ULim_Move, _Info.Readity_ULim_Move); }
        public bool OnULim_Work(bool bReadity)
        {
            if (_Info.ULim_Work == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_ULim_Work = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.ULim_Work, _Info.Readity_ULim_Work);
        }
        public string GetStatusName_ULim_Work() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.ULim_Work, _Info.Readity_ULim_Work); }
        public bool OnLLim_Move(bool bReadity)
        {
            if (_Info.LLim_Move == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_LLim_Move = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.LLim_Move, _Info.Readity_LLim_Move);
        }
        public string GetStatusName_LLim_Move() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.LLim_Move, _Info.Readity_LLim_Move); }
        public bool OnLLim_Work(bool bReadity)
        {
            if (_Info.LLim_Work == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_LLim_Work = bReadity;
            return G.Comm.IOCtrl.GetDI(_Info.LLim_Work, _Info.Readity_LLim_Work);
        }
        public string GetStatusName_LLim_Work() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.LLim_Work, _Info.Readity_LLim_Work); }
        public bool IsSafeLock(bool bReadity)
        {
            if (_Info.SafeLock1 == EDI_TYPE.DI_COUNT && _Info.SafeLock2 == EDI_TYPE.DI_COUNT)
                return bReadity;

            _Info.Readity_SafeLock1 = bReadity;
            _Info.Readity_SafeLock2 = bReadity;

            if (_Info.SafeLock1 != EDI_TYPE.DI_COUNT && _Info.SafeLock2 == EDI_TYPE.DI_COUNT)
                return G.Comm.IOCtrl.GetDI(_Info.SafeLock1, _Info.Readity_SafeLock1);
            else if (_Info.SafeLock1 == EDI_TYPE.DI_COUNT && _Info.SafeLock2 != EDI_TYPE.DI_COUNT)
                return G.Comm.IOCtrl.GetDI(_Info.SafeLock2, _Info.Readity_SafeLock2);
            else
                return G.Comm.IOCtrl.GetDI(_Info.SafeLock1, _Info.Readity_SafeLock1) && G.Comm.IOCtrl.GetDI(_Info.SafeLock2, _Info.Readity_SafeLock2);
        }
        public string GetStatusName_SafeLock()
        {
            if (_Info.SafeLock1 == EDI_TYPE.DI_COUNT && _Info.SafeLock2 == EDI_TYPE.DI_COUNT)
                return "No set any safe lock sensor";
            else if (_Info.SafeLock1 != EDI_TYPE.DI_COUNT && _Info.SafeLock2 == EDI_TYPE.DI_COUNT)
                return G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock1, _Info.Readity_SafeLock1);
            else if (_Info.SafeLock1 == EDI_TYPE.DI_COUNT && _Info.SafeLock2 != EDI_TYPE.DI_COUNT)
                return G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock2, _Info.Readity_SafeLock2);
            else
                return G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock1, _Info.Readity_SafeLock1) + "\n" + G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock2, _Info.Readity_SafeLock2);
        }
        public string GetStatusName_SafeLock1() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock1, _Info.Readity_SafeLock1); }
        public string GetStatusName_SafeLock2() { return G.Comm.IOCtrl.GetDINameWithStatus(_Info.SafeLock2, _Info.Readity_SafeLock2); }
        public bool IsPassULim_B() { return _PassULim_B; }
        public EMotion NowMotion() { return _NowMotion; }
        public EMotion ProMotion() { return _ProMotion; }

        public void Down()
        {
            bool _bDI = false;

            if (_Info.LLim_Work != EDI_TYPE.DI_COUNT && _Info.LLim_Move != EDI_TYPE.DI_COUNT)
                _bDI = G.Comm.IOCtrl.GetDI(_Info.LLim_Work, true) || G.Comm.IOCtrl.GetDI(_Info.LLim_Move, true);
            else if (_Info.LLim_Work != EDI_TYPE.DI_COUNT && _Info.LLim_Move == EDI_TYPE.DI_COUNT)
                _bDI = G.Comm.IOCtrl.GetDI(_Info.LLim_Work, true);
            else if (_Info.LLim_Work == EDI_TYPE.DI_COUNT && _Info.LLim_Move != EDI_TYPE.DI_COUNT)
                _bDI = G.Comm.IOCtrl.GetDI(_Info.LLim_Move, true);

            if (!_bDI)
            {
                if (_Info.LiftsDown != EDO_TYPE.DO_COUNT)
                {
                    RenewProMotion(EMotion.Down);
                    G.Comm.IOCtrl.SetDO(_Info.LiftsDown, true);
                }
                if (_Info.LiftsUp != EDO_TYPE.DO_COUNT)
                {
                    RenewProMotion(EMotion.Down);
                    G.Comm.IOCtrl.SetDO(_Info.LiftsUp, false);
                }
            }
            else
                Stop();
        }
        public void Up(bool bPassULim_B = false)
        {
            _PassULim_B = bPassULim_B;

            bool _bDI = false;

            if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.InPlace, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.InPlace, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                if (_PassULim_B)
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true);
                }
                else
                {
                    _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                        G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
                }
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board != EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Board, true);
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                    G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                    G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true) ||
                    G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                    G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace != EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.InPlace, true);
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true) ||
                    G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true);
            }
            else if (_Info.ULim_Work == EDI_TYPE.DI_COUNT && _Info.ULim_Move != EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Move, true);
            }
            else if (_Info.ULim_Work != EDI_TYPE.DI_COUNT && _Info.ULim_Move == EDI_TYPE.DI_COUNT && _Info.InPlace == EDI_TYPE.DI_COUNT && _Info.ULim_Board == EDI_TYPE.DI_COUNT)
            {
                _bDI = G.Comm.IOCtrl.GetDI(_Info.ULim_Work, true);
            }

            if (!_bDI)
            {
                if (_Info.LiftsDown != EDO_TYPE.DO_COUNT)
                {
                    RenewProMotion(EMotion.Up);
                    G.Comm.IOCtrl.SetDO(_Info.LiftsDown, false);
                }
                if (_Info.LiftsUp != EDO_TYPE.DO_COUNT)
                {
                    RenewProMotion(EMotion.Up);
                    G.Comm.IOCtrl.SetDO(_Info.LiftsUp, true);
                }
            }
            else
                Stop();
        }
        public void Stop()
        {
            RenewProMotion(EMotion.Stop);

            if (_Info.LiftsDown != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Info.LiftsDown, false);
            if (_Info.LiftsUp != EDO_TYPE.DO_COUNT)
                G.Comm.IOCtrl.SetDO(_Info.LiftsUp, false);
        }

        private void RenewProMotion(EMotion eNowMotion)
        {
            if (_ProMotion != _NowMotion)
                _ProMotion = _NowMotion;

            _NowMotion = eNowMotion;
        }

        public void CheckBtnUDClick()
        {
            if (_Info.LiftsBtnUp != EDI_TYPE.DI_COUNT && _Info.LiftsBtnDown != EDI_TYPE.DI_COUNT)
            {
                if (G.Comm.IOCtrl.GetDI(_Info.LiftsBtnUp, false) && G.Comm.IOCtrl.GetDI(_Info.LiftsBtnDown, false))
                {
                    Stop();
                    return;
                }
            }

            if (_Info.LiftsBtnUp != EDI_TYPE.DI_COUNT)
            {
                switch (G.Comm.IOCtrl.GetDIEdge(_Info.LiftsBtnUp))
                {
                    case EDIO_SingleEdge.RisingEdge:
                        if (_Info.LiftsBtnUpLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_Info.LiftsBtnUpLED, true);
                        break;
                    case EDIO_SingleEdge.FallingEdge:
                        if (_Info.LiftsBtnUpLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_Info.LiftsBtnUpLED, false);
                        Stop();
                        break;
                    case EDIO_SingleEdge.On:
                        Up();
                        break;
                }
            }

            if (_Info.LiftsBtnDown != EDI_TYPE.DI_COUNT)
            {
                switch (G.Comm.IOCtrl.GetDIEdge(_Info.LiftsBtnDown))
                {
                    case EDIO_SingleEdge.RisingEdge:
                        if (_Info.LiftsBtnDownLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_Info.LiftsBtnDownLED, true);
                        break;
                    case EDIO_SingleEdge.FallingEdge:
                        if (_Info.LiftsBtnDownLED != EDO_TYPE.DO_COUNT)
                            G.Comm.IOCtrl.SetDO(_Info.LiftsBtnDownLED, false);
                        Stop();
                        break;
                    case EDIO_SingleEdge.On:
                        Down();
                        break;
                }
            }
        }
    }
}