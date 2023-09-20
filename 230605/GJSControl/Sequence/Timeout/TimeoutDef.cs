using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using FileStreamLibrary;
using CommonLibrary;

namespace nsSequence
{
    public class TimeoutDef : IDisposable
    {
        private struct TimeoutInfoDef
        {
            public Enum Step;
            public int TimeoutIndex;
        }

        private enum EStep
        {
            IsTimeout,
            ShowForm,
            GetResult
        }

        private TimeoutInfoDef _currentInfo;
        private String _AlarmFilePath;
        private Form _frmActiveFm;
        private FmTimeoutMsg _frmTimeoutFm;
        private bool _ThreadEnd;
        private Thread _Polling;
        private BaseSequenceDef[] _Seqeunce;
        public DialogResult Result;
        private EStep _step;
        private bool _alarm;
        private string _condition;
        private BaseStep.BtnType _BtnType;

        public TimeoutDef(Form frmActiveFm, String sFolder, BaseSequenceDef[] seqeunce)
        {
            _condition = string.Empty;
            _frmActiveFm = frmActiveFm;
            _AlarmFilePath = sFolder + "\\Alarm.ini";
            _Seqeunce = seqeunce;
            _ThreadEnd = false;
            _currentInfo.TimeoutIndex = -1;
            _Polling = new Thread(new ThreadStart(Execute))
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            };
            _Polling.Start();
        }

        public void Dispose()
        {
            SaveFile(_AlarmFilePath);
            ThreadEnd();
            while (_Polling != null && _Polling.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
            _Polling = null;
        }

        private void SaveFile(String sPath)
        {
            IniFile cIni = new IniFile(sPath, true);
            List<String[]> strArrayList = new List<string[]>();

            //  read
            for (int i = 0; i < _Seqeunce.Length; i++)
            {

                String sSection = _Seqeunce[i].GetStep().GetType().ToString();
                String[] strArray = new String[Enum.GetNames(_Seqeunce[i].GetStep().GetType()).Count()];
                for (int j = 0; j < Enum.GetNames(_Seqeunce[i].GetStep().GetType()).Count(); j++)
                    strArray[j] = cIni.ReadStr(sSection, Enum.GetNames(_Seqeunce[i].GetStep().GetType())[j].ToString(), "");

                strArrayList.Add(strArray);
            }
            cIni.FileClose();
            cIni.Dispose();

            cIni = new IniFile(sPath, false);
            for (int i = 0; i < _Seqeunce.Length; i++)
            {
                String sSection = _Seqeunce[i].GetStep().GetType().ToString();
                for (int j = 0; j < Enum.GetNames(_Seqeunce[i].GetStep().GetType()).Count(); j++)
                {
                    String[] strArray = strArrayList[i][j].Split('#');
                    if (strArray.Count() < 1)
                        strArray = new string[1];

                    strArray[0] = (i + 1).ToString("00") + j.ToString("000");

                    String sContent = "";
                    for (int l = 0; l < strArray.Count() - 1; l++)
                        sContent += strArray[l] + "#";
                    sContent += strArray[strArray.Count() - 1];

                    cIni.WriteStr(sSection, Enum.GetNames(_Seqeunce[i].GetStep().GetType())[j].ToString(), sContent);
                }

            }
            cIni.FileClose();
            cIni.Dispose();

            strArrayList.Clear();
        }

        public void ResetAll()
        {
            if (_frmTimeoutFm != null)
                _frmTimeoutFm.Close();

            for (int i = 0; i < _Seqeunce.Length; i++)
                _Seqeunce[i].Cancel();

            Result = DialogResult.None;
            _step = EStep.IsTimeout;
        }

        public DialogResult GetResult(Enum step)
        {
            if (_currentInfo.TimeoutIndex > -1)
                return DialogResult.None;

            if (_currentInfo.Step != null && step.GetType() == _currentInfo.Step.GetType())
                return Result;

            return DialogResult.None;
        }

        private void Execute()
        {
            while (!_ThreadEnd)
            {
                switch (_step)
                {
                    case EStep.IsTimeout:
                        _alarm = false;
                        Result = DialogResult.None;

                        for (int i = 0; i < _Seqeunce.Length; i++)
                        {
                            if (_Seqeunce[i].Timeout(ref _currentInfo.Step, ref _currentInfo.TimeoutIndex, ref _condition, ref _BtnType))
                            {
                                _step = EStep.ShowForm;
                                break;
                            }
                        }
                        break;

                    case EStep.ShowForm:
                        {
                            Result = DialogResult.None;

                            String errorCode = _currentInfo.Step.ToString();
                            String Description = errorCode;
                            int codeNum = 0;
                            ReadAlarmCode(_currentInfo, out codeNum, out Description);
                            _frmTimeoutFm = new FmTimeoutMsg(_BtnType, codeNum.ToString() + ":" + Description, _condition, this);
                            _frmActiveFm.Invoke(new Action(() => { ((nsUI.FmMain)_frmActiveFm).PanelShow(_frmTimeoutFm, true); }));
                            _step = EStep.GetResult;
                            AlarmTextDisplay.Add(
                                (int)AlarmCode.Alarm_Timeout, 
                                AlarmType.Warning, 
                                codeNum.ToString() + ":" + Description + ":" + _condition);
                        }
                        break;

                    case EStep.GetResult:
                        if (Result == DialogResult.None)
                            break;

                        for (int i = 0; i < _Seqeunce.Length; i++)
                        {
                            if (_Seqeunce[i].GetStep().GetType() == _currentInfo.Step.GetType())
                            {
                                if (Result == DialogResult.Retry)
                                {
                                    _Seqeunce[i].Retry();
                                }
                                else if (Result == DialogResult.Ignore)
                                {
                                    _Seqeunce[i].Ignore();
                                }
                                else
                                {
                                    _Seqeunce[i].Cancel();
                                    _alarm = true;
                                }

                                _frmActiveFm.Invoke(new Action(() => { ((nsUI.FmMain)_frmActiveFm).ShowFuncPage(); }));
                                _frmTimeoutFm = null;
                                Result = DialogResult.None;
                            }
                        }
                        _step = EStep.IsTimeout;
                        break;
                }

                Thread.Sleep(50);
            }
        }

        public bool Showing() { return _step == EStep.ShowForm; }

        public bool Alarm() { return _alarm; }
        private void ThreadEnd() { _ThreadEnd = true; }

        private void ReadAlarmCode(TimeoutInfoDef info, out int alarmCode, out string description)
        {
            String sKey = info.Step.ToString();
            alarmCode = 0;
            description = sKey;
            if (!File.Exists(_AlarmFilePath))
                return;

            IniFile cIniFileInfo = new IniFile(_AlarmFilePath, true);

            String sSection = info.Step.GetType().ToString();
            String sContent = cIniFileInfo.ReadStr(sSection, sKey, sKey);

            String[] strAry = sContent.Split('#');

            if (strAry.Count() > 0)
            {
                int.TryParse(strAry[0], out alarmCode);
                if (strAry.Count() > 1)
                    description = strAry[1];
            }
        }

        public FmTimeoutMsg GetForm() { return _frmTimeoutFm; }

        public void SetResult(DialogResult dialogResult) { Result = dialogResult; }
    }
}