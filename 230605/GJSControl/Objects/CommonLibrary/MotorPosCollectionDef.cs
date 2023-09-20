using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FileStreamLibrary;

namespace CommonLibrary
{
    public enum EMotorPos
    {
        CV_Align_Open,
        CV_Align_Close,
        DrillsAlign_Step_Open,
        DrillsAlign_Step_Close,
        DrillsAlign_Servo_Open,
        DrillsAlign_Servo_Close,
        Robot_Take,
        Robot_Put,
        Robot_ChangeDrills,


        Count,
    }
    public class MotorPosCollectionDef : IDisposable
    {
        private MotorPosDef[] _MotorPosArray;
        readonly String _FolderPath;
        public MotorPosCollectionDef(String sFolderPath)
        {
            _FolderPath = sFolderPath;

            _MotorPosArray = new MotorPosDef[(int)EMotorPos.Count];

            //_MotorPosArray[(int)EMotorPos.LiftsL] = new MotorPosDef(
            //     EAXIS_NAME.MylarX);

            _MotorPosArray[(int)EMotorPos.CV_Align_Close] = new MotorPosDef(
                 EAXIS_NAME.CV_Align);
            _MotorPosArray[(int)EMotorPos.CV_Align_Open] = new MotorPosDef(
                 EAXIS_NAME.CV_Align);
            _MotorPosArray[(int)EMotorPos.DrillsAlign_Servo_Close] = new MotorPosDef(
                 EAXIS_NAME.DrillsAlign_Servo);
            _MotorPosArray[(int)EMotorPos.DrillsAlign_Servo_Open] = new MotorPosDef(
                 EAXIS_NAME.DrillsAlign_Servo);
            _MotorPosArray[(int)EMotorPos.DrillsAlign_Step_Close] = new MotorPosDef(
                 EAXIS_NAME.DrillsAlign_Step);
            _MotorPosArray[(int)EMotorPos.DrillsAlign_Step_Open] = new MotorPosDef(
                 EAXIS_NAME.DrillsAlign_Step);
            _MotorPosArray[(int)EMotorPos.Robot_Take] = new MotorPosDef(
                 EAXIS_NAME.Robot);
            _MotorPosArray[(int)EMotorPos.Robot_Put] = new MotorPosDef(
                 EAXIS_NAME.Robot);
            _MotorPosArray[(int)EMotorPos.Robot_ChangeDrills] = new MotorPosDef(
                 EAXIS_NAME.Robot);


            Load();
        }

        public MotorPosDef GetMotorPos(EMotorPos eMotorPos)
        {
            if (eMotorPos == EMotorPos.Count)
                return null;

            return _MotorPosArray[(int)eMotorPos];
        }

        public int GetMotorPosNum()
        {
            return _MotorPosArray.Count();
        }

        public void Dispose()
        {
            _MotorPosArray = null;
        }

        public void Move(EMotorPos ePos)
        {
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i],
                    _MotorPosArray[(int)ePos]._ESpeedType);
            }
        }

        public void Move(EMotorPos ePos, ESPEED_TYPE eSpeed)
        {
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i],
                   eSpeed);
            }
        }

        public bool Stop(EMotorPos ePos, out string Error)
        {
            Error = "Axis Number Wrong";
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (!G.Comm.MtnCtrl.Stop(_MotorPosArray[(int)ePos].GetAxis(i), true))
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(_MotorPosArray[(int)ePos].GetAxis(i)) + " Not Stop";
                    return false;
                }
                if (Math.Abs(G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0) - _MotorPosArray[(int)ePos]._Value[i]) > 0.3)
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(_MotorPosArray[(int)ePos].GetAxis(i)) + " Not On Position";
                    return false;
                }
            }

            return true;
        }

        public bool Stop(EMotorPos ePos, MotorPosDef cShiftMotorPos, out string Error)
        {
            Error = "Axis Number Wrong";
            if (cShiftMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return false;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cShiftMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(cShiftMotorPos.GetAxis(i)) + " Index Wrong";
                    return false;
                }

                if (!G.Comm.MtnCtrl.Stop(_MotorPosArray[(int)ePos].GetAxis(i), true))
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(cShiftMotorPos.GetAxis(i)) + " Not Stop";
                    return false;
                }
                if (Math.Abs(G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0) - (_MotorPosArray[(int)ePos]._Value[i] + cShiftMotorPos._Value[i])) > 1)
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(cShiftMotorPos.GetAxis(i)) + " Not On Position";
                    return false;
                }
            }

            return true;
        }

        public bool Stop(EMotorPos ePos, double[] shift, out string Error)
        {
            Error = "Axis Number Wrong";
            if (shift.Length != _MotorPosArray[(int)ePos].GetAxisNum())
                return false;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (!G.Comm.MtnCtrl.Stop(_MotorPosArray[(int)ePos].GetAxis(i), true))
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(_MotorPosArray[(int)ePos].GetAxis(i)) + " Not Stop";
                    return false;
                }
                if (Math.Abs(G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0) - (_MotorPosArray[(int)ePos]._Value[i] + shift[i])) > 0.2)
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(_MotorPosArray[(int)ePos].GetAxis(i)) + " Not On Position";
                    return false;
                }
            }

            return true;
        }

        public bool Close(EMotorPos ePos, double fDistance, out string Error)
        {
            Error = "";
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (Math.Abs(G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0) - _MotorPosArray[(int)ePos]._Value[i]) > fDistance)
                {
                    Error = G.Comm.MtnCtrl.GetAxisName(_MotorPosArray[(int)ePos].GetAxis(i)) + " Not On Position";
                    return false;
                }
            }

            return true;
        }

        public bool Close(EMotorPos ePos, MotorPosDef cShiftMotorPos, double fDistances)
        {
            if (cShiftMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return false;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cShiftMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return false;

                if (Math.Abs(G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0) - (_MotorPosArray[(int)ePos]._Value[i] + cShiftMotorPos._Value[i])) > fDistances)
                    return false;
            }

            return true;
        }

        public void PlusMove(EMotorPos ePos, MotorPosDef cShiftMotorPos)
        {
            if (cShiftMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cShiftMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return;

                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i] + cShiftMotorPos._Value[i],
                    _MotorPosArray[(int)ePos]._ESpeedType);
            }
        }

        public void PlusMove(EMotorPos ePos, MotorPosDef cShiftMotorPos, ESPEED_TYPE eSpeed)
        {
            if (cShiftMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cShiftMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return;

                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i] + cShiftMotorPos._Value[i],
                   eSpeed);
            }
        }

        public void SetCurrentPos(EMotorPos ePos)
        {
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                _MotorPosArray[(int)ePos]._Value[i] = G.Comm.MtnCtrl.GetPos(_MotorPosArray[(int)ePos].GetAxis(i), 0);
            }
        }

        public void Set(EMotorPos ePos, double[] value)
        {
            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                _MotorPosArray[(int)ePos]._Value[i] = value[i];
            }
        }

        public void PlusMove(EMotorPos ePos, double[] fValue)
        {
            if (_MotorPosArray[(int)ePos].GetAxisNum() != fValue.Length)
                return;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i] + fValue[i],
                    _MotorPosArray[(int)ePos]._ESpeedType);
            }
        }

        public void PlusMove(EMotorPos ePos, double[] fValue, ESPEED_TYPE ESpeed)
        {
            if (_MotorPosArray[(int)ePos].GetAxisNum() != fValue.Length)
                return;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                G.Comm.MtnCtrl.AbsMv(
                    _MotorPosArray[(int)ePos].GetAxis(i),
                    _MotorPosArray[(int)ePos]._Value[i] + fValue[i],
                    ESpeed);
            }
        }

        public void Plus(EMotorPos ePos, MotorPosDef cShiftMotorPos)
        {
            if (cShiftMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cShiftMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return;

                _MotorPosArray[(int)ePos]._Value[i] += cShiftMotorPos._Value[i];
            }
        }

        public void CopyTo(EMotorPos ePos, MotorPosDef cDstMotorPos)
        {
            if (cDstMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return;

            cDstMotorPos._ESpeedType = _MotorPosArray[(int)ePos]._ESpeedType;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cDstMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return;

                cDstMotorPos._Value[i] = _MotorPosArray[(int)ePos]._Value[i];
            }
        }

        public void CopyFrom(EMotorPos ePos, MotorPosDef cSrcMotorPos)
        {
            if (cSrcMotorPos.GetAxisNum() != _MotorPosArray[(int)ePos].GetAxisNum())
                return;

            _MotorPosArray[(int)ePos]._ESpeedType = cSrcMotorPos._ESpeedType;

            for (int i = 0; i < _MotorPosArray[(int)ePos].GetAxisNum(); i++)
            {
                if (cSrcMotorPos.GetAxis(i) != _MotorPosArray[(int)ePos].GetAxis(i))
                    return;

                _MotorPosArray[(int)ePos]._Value[i] = cSrcMotorPos._Value[i];
            }
        }

        public void Save()
        {
            IniFile wriFileInfo = new IniFile(_FolderPath + "\\MotorPos.ini", false);
            String sSection = "MotorPosition";
            string str, str1;
            for (int i = 0; i < _MotorPosArray.Count(); i++)
            {
                str = string.Join(",", _MotorPosArray[i]._Value);
                str1 = string.Join(",", _MotorPosArray[i]._ESpeedType);
                String sKeyFront = ((EMotorPos)i).ToString() + "_";
                wriFileInfo.WriteStr(sSection, sKeyFront + "MotorPos", str + "," + str1);
            }
            wriFileInfo.FileClose();
            wriFileInfo.Dispose();
        }


        public void Load()
        {
            if (!File.Exists(_FolderPath + "\\MotorPos.ini"))
                return;

            String[] MotorPos;
            MotorPos = new string[_MotorPosArray.Count()];

            IniFile cReaFileInfo = new IniFile(_FolderPath + "\\MotorPos.ini", true);
            String sSection = "MotorPosition";
            string str, str1;
            for (int i = 0; i < _MotorPosArray.Count(); i++)
            {
                str = string.Join(",", _MotorPosArray[i]._Value);
                str1 = string.Join(",", _MotorPosArray[i]._ESpeedType);
                String sKeyFront = ((EMotorPos)i).ToString() + "_";
                MotorPos[i] = cReaFileInfo.ReadStr(sSection, sKeyFront + "MotorPos", str + "," + str1);

                String[] split = MotorPos[i].Split(',');
                for (int j = 0; j < _MotorPosArray[i]._Value.Count(); j++)
                {
                    _MotorPosArray[i]._Value[j] = double.Parse(split[j]);
                }
                for (int j = 0; j < (int)ESPEED_TYPE.SPEED_COUNT; j++)
                {
                    if (split[split.Count() - 1] == ((ESPEED_TYPE)j).ToString())
                        _MotorPosArray[i]._ESpeedType = ((ESPEED_TYPE)j);
                }
            }

            cReaFileInfo.FileClose();
            cReaFileInfo.Dispose();
        }
    }
}