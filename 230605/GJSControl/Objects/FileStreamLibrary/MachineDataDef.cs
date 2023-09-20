using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CommonLibrary;

namespace FileStreamLibrary
{
    public enum MachineModules
    {
        Main,
        Count,
    };

    public class MachineDataDef : IDisposable
    {
        private string[] _CaptionDouble;
        private double[] _PreValueDouble;
        private double[] _ValueDouble;
        private string[] _CaptionInt;
        private int[] _PreValueInt;
        private int[] _ValueInt;
        private string[] _CaptionString;
        private string[] _PreValueString;
        private string[] _ValueString;
        private String FolderPath;

        public MachineDataDef(String sFolderPath)
        {
            FolderPath = sFolderPath;

            InitializeComponet();

            Load();
        }

        public void InitializeComponet()
        {
            _CaptionDouble = new String[(int)EMachineDouble.Count];
            _PreValueDouble = new double[(int)EMachineDouble.Count];
            _ValueDouble = new double[(int)EMachineDouble.Count];

            for (int i = 0; i < (int)EMachineDouble.Count; i++)
                _CaptionDouble[i] = ((EMachineDouble)i).ToString();

            _CaptionInt = new String[(int)EMachineInt.Count];
            _PreValueInt = new int[(int)EMachineInt.Count];
            _ValueInt = new int[(int)EMachineInt.Count];

            for (int i = 0; i < (int)EMachineInt.Count; i++)
                _CaptionInt[i] = ((EMachineInt)i).ToString();

            _CaptionString = new String[(int)EMachineString.Count];
            _PreValueString = new String[(int)EMachineString.Count];
            _ValueString = new String[(int)EMachineString.Count];

            for (int i = 0; i < (int)EMachineString.Count; i++)
            {
                _CaptionString[i] = ((EMachineString)i).ToString();
                _PreValueString[i] = "";
                _ValueString[i] = "";
            }
        }

        public double GetValue(EMachineDouble Index) { return _ValueDouble[(int)Index]; }
        public int GetValue(EMachineInt Index) { return _ValueInt[(int)Index]; }
        public string GetValue(EMachineString Index) { return _ValueString[(int)Index]; }
        public string GetCaption(EMachineString Index) { return _CaptionString[(int)Index]; }
        public string GetCaption(EMachineDouble Index) { return _CaptionDouble[(int)Index]; }
        public string GetCaption(EMachineInt Index) { return _CaptionInt[(int)Index]; }
        public void SetValue(EMachineDouble Index, double Value) { _ValueDouble[(int)Index] = Value; }
        public void SetValue(EMachineString Index, string Value) { _ValueString[(int)Index] = Value; }
        public void SetValue(EMachineInt Index, int Value) { _ValueInt[(int)Index] = Value; }

        public void Save()
        {
            IniFile cRecFileInfo = new IniFile(FolderPath + "\\Machine.ini", false);

            String sSection = "EMachineDouble";
            for (int i = 0; i < _CaptionDouble.Count(); i++)
            {
                String sKeyFront = ((EMachineDouble)i).ToString() + "_";
                cRecFileInfo.WriteStr(sSection, sKeyFront + "Caption", _CaptionDouble[i]);
                cRecFileInfo.WriteDouble(sSection, sKeyFront + "Value", (double)_ValueDouble[i]);
            }

            sSection = "EMachineInt";
            for (int i = 0; i < _CaptionInt.Count(); i++)
            {
                String sKeyFront = ((EMachineInt)i).ToString() + "_";
                cRecFileInfo.WriteStr(sSection, sKeyFront + "Caption", _CaptionInt[i]);
                cRecFileInfo.WriteInt(sSection, sKeyFront + "Value", _ValueInt[i]);
            }

            sSection = "EMachineString";
            for (int i = 0; i < _CaptionString.Count(); i++)
            {
                String sKeyFront = ((EMachineString)i).ToString() + "_";
                cRecFileInfo.WriteStr(sSection, sKeyFront + "Caption", _CaptionString[i]);
                cRecFileInfo.WriteStr(sSection, sKeyFront + "Value", _ValueString[i]);
            }
            cRecFileInfo.FileClose();
            cRecFileInfo.Dispose();

            ValueChange();
        }

        public bool Load()
        {
            if (!File.Exists(FolderPath + "\\Machine.ini"))
                return false;

            IniFile cRecFileInfo = new IniFile(FolderPath + "\\Machine.ini", true);

            String sSection = "EMachineDouble";
            for (int i = 0; i < _CaptionDouble.Count(); i++)
            {
                String sKeyFront = ((EMachineDouble)i).ToString() + "_";
                _CaptionDouble[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Caption", _CaptionDouble[i]);
                _ValueDouble[i] = cRecFileInfo.ReadDouble(sSection, sKeyFront + "Value", _ValueDouble[i]);
                _PreValueDouble[i] = _ValueDouble[i];
            }

            sSection = "EMachineInt";
            for (int i = 0; i < _CaptionInt.Count(); i++)
            {
                String sKeyFront = ((EMachineInt)i).ToString() + "_";
                _CaptionInt[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Caption", _CaptionInt[i]);
                _ValueInt[i] = cRecFileInfo.ReadInt(sSection, sKeyFront + "Value", _ValueInt[i]);
                _PreValueInt[i] = _ValueInt[i];
            }

            sSection = "EMachineString";
            for (int i = 0; i < _CaptionString.Count(); i++)
            {
                String sKeyFront = ((EMachineString)i).ToString() + "_";
                _CaptionString[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Caption", _CaptionString[i]);
                _ValueString[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Value", _ValueString[i]);
                _PreValueString[i] = _ValueString[i];
            }

            cRecFileInfo.FileClose();
            cRecFileInfo.Dispose();

            return true;
        }

        private void ValueChange()
        {
            for (int i = 0; i < _CaptionDouble.Count(); i++)
            {
                if (_ValueDouble[i] != _PreValueDouble[i])
                {
                    LogDef.Add(
                        ELogFileName.MachineData,
                        _CaptionDouble[i],
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Convert.ToString(_PreValueDouble[i]) + " to " + Convert.ToString(_ValueDouble[i]));

                    _PreValueDouble[i] = _ValueDouble[i];
                }
            }

            for (int i = 0; i < _CaptionInt.Count(); i++)
            {
                if (_ValueInt[i] != _PreValueInt[i])
                {
                    LogDef.Add(
                        ELogFileName.MachineData,
                        _CaptionInt[i],
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Convert.ToString(_PreValueInt[i]) + " to " + Convert.ToString(_ValueInt[i]));

                    _PreValueInt[i] = _ValueInt[i];
                }
            }

            for (int i = 0; i < _CaptionString.Count(); i++)
            {
                if (_ValueString[i] != _PreValueString[i])
                {
                    LogDef.Add(
                        ELogFileName.MachineData,
                        _CaptionString[i],
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        _PreValueString[i] + " to " + _ValueString[i]);

                    _PreValueString[i] = _ValueString[i];
                }
            }
        }

        public void Dispose() { }

        public class RobotPosDef : IDisposable
        {
            private String FilePath;
            private String Section;
            private String HeaderName;

            public enum EElbow
            {
                Above,
                Below,
            }

            public enum EHand
            {
                Lefty,
                Righty
            }

            public enum EWrist
            {
                Flip,
                NoFlip,
            }

            public String _Name;
            public float _X;
            public float _Y;
            public float _Z;
            public float _U;
            public float _V;
            public float _W;
            public EHand _Hand;
            public EElbow _Elbow;
            public EWrist _Wrist;
            public int _J6Flg;
            public int _SpeedType;
            public RobotPosDef(String sIniPath, String sSection, String sHeaderName)
            {
                FilePath = sIniPath;
                Section = sSection;
                HeaderName = sHeaderName;

                ReadData();
            }

            public void ReadData()
            {
                if (!File.Exists(FilePath))
                    return;

                IniFile cIniInfo = new IniFile(FilePath, true);
                String sDefaultInfo = HeaderName + ",0,0,0,0,0,0,0,0,0,0,0";
                sDefaultInfo = cIniInfo.ReadStr(Section, HeaderName, sDefaultInfo);
                if (sDefaultInfo != null)
                {
                    _Name = sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(","));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _X = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _Y = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _Z = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _U = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _V = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _W = float.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    if ((sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(","))) == EHand.Lefty.ToString())
                        _Hand = EHand.Lefty;
                    else
                        _Hand = EHand.Righty;

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    if ((sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(","))) == EElbow.Above.ToString())
                        _Elbow = EElbow.Above;
                    else
                        _Elbow = EElbow.Below;

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    if ((sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(","))) == EWrist.Flip.ToString())
                        _Wrist = EWrist.Flip;
                    else
                        _Wrist = EWrist.NoFlip;

                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _J6Flg = int.Parse(sDefaultInfo.Substring(0, sDefaultInfo.IndexOf(",")));


                    sDefaultInfo = sDefaultInfo.Substring(sDefaultInfo.IndexOf(",") + 1, sDefaultInfo.Length - (sDefaultInfo.IndexOf(",") + 1));
                    _SpeedType = int.Parse(sDefaultInfo);
                }

                cIniInfo.FileClose();
                cIniInfo.Dispose();
            }

            public void WritrData()
            {
                IniFile cIniInfo = new IniFile(FilePath, false);
                String sDefaultInfo =
                     HeaderName + "," +
                     _X + "," +
                     _Y + "," +
                     _Z + "," +
                     _U + "," +
                     _V + "," +
                     _W + "," +
                     _Hand.ToString() + "," +
                     _Elbow.ToString() + "," +
                     _Wrist.ToString() + "," +
                     _J6Flg.ToString("0") + "," +
                     _SpeedType.ToString("0");

                cIniInfo.WriteStr(Section, HeaderName, sDefaultInfo);
                cIniInfo.FileClose();
                cIniInfo.Dispose();
            }

            public void Dispose() { }
        }
    }
}
