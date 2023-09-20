using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using FileStreamLibrary;
using CommonLibrary;

namespace FileStreamLibrary
{
    public class RecipeDef : IDisposable
    {
        private String[] _doubleCaption;
        private double[] _doubleValue;
        private String[] _intCaption;
        private int[] _intValue;

        private String _name;
        private String _notice;
        private String _date;
        private String _user;
        private String FolderPath;

        public RecipeDef(String sFolderPath, String sName)
        {
            InitializeComponet();
            FolderPath = sFolderPath;

            _notice = "";
            Load(sName);
        }

        // 指定類型
        public void InitializeComponet()
        {
            _doubleCaption = new String[(int)ERecipeDouble.Count];
            _doubleValue = new double[(int)ERecipeDouble.Count];

            _intCaption = new String[(int)ERecipeInt.Count];
            _intValue = new int[(int)ERecipeInt.Count];
        }

        public int GetFactorNum()
        {
            return _doubleCaption.Count() + _intCaption.Count();
        }

        public void GetFactor(ERecipeDouble eIndex, ref String Caption, ref double Value)
        {
            Caption = _doubleCaption[(int)eIndex];
            Value = _doubleValue[(int)eIndex];
        }

        public void GetFactor(ERecipeInt eIndex, ref String Caption, ref int Value)
        {
            Caption = _intCaption[(int)eIndex];
            Value = _intValue[(int)eIndex];
        }

        public double GetValue(ERecipeDouble eIndex)
        {
            return _doubleValue[(int)eIndex];
        }

        public int GetValue(ERecipeInt eIndex)
        {
            return _intValue[(int)eIndex];
        }

        public void SetValue(ERecipeDouble eIndex, double Value)
        {
            if (_doubleValue[(int)eIndex] != Value)
                LogDef.Add(ELogFileName.MachineData, "Recipe_" + _name, eIndex.ToString(), _doubleValue[(int)eIndex].ToString() + " to " + Value.ToString());

            _doubleValue[(int)eIndex] = Value;
        }

        public void SetValue(ERecipeInt eIndex, int Value)
        {
            if (_intValue[(int)eIndex] != Value)
                LogDef.Add(ELogFileName.MachineData, "Recipe_" + _name, eIndex.ToString(), _intValue[(int)eIndex].ToString() + " to " + Value.ToString());

            _intValue[(int)eIndex] = Value;
        }

        public void Dispose()
        {

        }

        public String GetName()
        {
            return _name;
        }

        public void GetInfo(ref string Date, ref string User)
        {
            Date = _date;
            User = _user;
        }

        public String GetNotice()
        {
            return _notice;
        }

        public void SetName(String sName)
        {
            _name = sName;
        }

        public void SetNotice(String sNotice)
        {
            _notice = sNotice;
        }

        public void Save(String sName, string User)
        {
            IniFile cRecFileInfo = new IniFile(FolderPath + "\\" + sName + "\\Recipe.ini", false);

            String sSection = "Info";
            cRecFileInfo.WriteStr(sSection, "Notice", _notice);
            cRecFileInfo.WriteStr(sSection, "Date", DateTime.Now.ToString("yyyyMMdd"));
            cRecFileInfo.WriteStr(sSection, "User", User);

            sSection = "Recipe";
            for (int i = 0; i < _doubleCaption.Count(); i++)
            {
                String sKeyFront = ((ERecipeDouble)i).ToString() + "_";

                cRecFileInfo.WriteStr(sSection, sKeyFront + "Caption", _doubleCaption[i]);
                cRecFileInfo.WriteDouble(sSection, sKeyFront + "Value", (double)_doubleValue[i]);
            }

            for (int i = 0; i < _intCaption.Count(); i++)
            {
                String sKeyFront = ((ERecipeInt)i).ToString() + "_";

                cRecFileInfo.WriteStr(sSection, sKeyFront + "Caption", _intCaption[i]);
                cRecFileInfo.WriteInt(sSection, sKeyFront + "Value", _intValue[i]);
            }

            cRecFileInfo.FileClose();
            cRecFileInfo.Dispose();
        }

        public bool Load(String sSubFolderName)
        {
            if (!File.Exists(FolderPath + "\\" + sSubFolderName + "\\Recipe.ini"))
                return false;

            _name = sSubFolderName;

            IniFile cRecFileInfo = new IniFile(FolderPath + "\\" + _name + "\\Recipe.ini", true);

            String sSection = "Info";
            _notice = cRecFileInfo.ReadStr(sSection, "Notice", _notice);
            _date = cRecFileInfo.ReadStr(sSection, "Date", _date);
            _user = cRecFileInfo.ReadStr(sSection, "User", _user);

            sSection = "Recipe";
            for (int i = 0; i < _doubleCaption.Count(); i++)
            {
                String sKeyFront = ((ERecipeDouble)i).ToString() + "_";

                _doubleCaption[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Caption", _doubleCaption[i]);
                _doubleValue[i] = cRecFileInfo.ReadDouble(sSection, sKeyFront + "Value", 0);
            }

            for (int i = 0; i < _intCaption.Count(); i++)
            {
                String sKeyFront = ((ERecipeInt)i).ToString() + "_";

                _intCaption[i] = cRecFileInfo.ReadStr(sSection, sKeyFront + "Caption", _intCaption[i]);
                _intValue[i] = cRecFileInfo.ReadInt(sSection, sKeyFront + "Value", 0);
            }
            cRecFileInfo.FileClose();
            cRecFileInfo.Dispose();

            return true;
        }
    }
}