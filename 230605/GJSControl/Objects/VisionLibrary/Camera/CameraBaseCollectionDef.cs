using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using FileStreamLibrary;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using VisionLibrary;
using VisionLibrary.Camera;

namespace VisionLibrary
{
    public enum ECamera
    {
        Camera1,
        Camera2,
        Count
    }
    public enum ECameraType
    {
        PointGrey,
        Logitech,
        Basler,
        SenTech,

        Count,
        None
    }

    public class CameraCollectionDef
    {
        private enum ECameraBasicKey
        {
            Type,
            SerialNumberOrMonikerString,
            Resolution,
            ExposureTime,
            ExposureTimeMin,
            ExposureTimeMax,
            ImageW,
            ImageH,
            ImageFPS,
            YInverse,
            XInverse,
            XYSwap,
            ImageVerticalFlip,
            ImageHorizontalFlip,
            ChessboardRow,
            ChessboardColumn,
            Calibrate,

            Count
        }

        public ECameraType[] CameraType;
        readonly double[] _Resolution;
        readonly String[] _SerialNumberOrMonikerString;
        readonly bool[] _XYSwap;          // XY 先反向再交換
        readonly bool[] _XInverse;            // XY 先反向再交換 => 指交換前的X是否反向
        readonly bool[] _YInverse;            // XY 先反向再交換 => 指交換前的Y是否反向
        readonly bool[] _ImageVerticalFlip;
        readonly bool[] _ImageHorizontalFlip;
        private double[] _DefaultExposureTime;
        private int[] _ExposureTimeMax;
        private int[] _ExposureTimeMin;
        private int[] _ExposureTimeStep;
        private int[] _ImageW;
        private int[] _ImageH;
        private int[] _ImageFPS;
        readonly String _FolderPath;
        readonly String _FilePath;
        private String _ErrorCode;
        private bool[] _EnableCalibrate;
        private int[] _ChessboardRow;
        private int[] _ChessboardColumn;
        private Mat[] _CameraMatrix;
        private Mat[] _DistCoeffs;
        readonly CameraBaseDef[] _CameraBase;
        private Thread _task;

        private int CameraCount { get; set; }

        public CameraCollectionDef(String sFolderPath)
        {
            try
            {
                _FolderPath = sFolderPath;
                _FilePath = _FolderPath + "\\Camera.ini";
                _ErrorCode = "";

                _XYSwap = new bool[(int)ECamera.Count];
                _XInverse = new bool[(int)ECamera.Count];
                _YInverse = new bool[(int)ECamera.Count];
                _ImageVerticalFlip = new bool[(int)ECamera.Count];
                _ImageHorizontalFlip = new bool[(int)ECamera.Count];
                _DefaultExposureTime = new double[((int)ECamera.Count)];
                _ExposureTimeMax = new int[((int)ECamera.Count)];
                _ExposureTimeMin = new int[((int)ECamera.Count)];
                _ExposureTimeStep = new int[((int)ECamera.Count)];
                _ImageW = new int[((int)ECamera.Count)];
                _ImageH = new int[((int)ECamera.Count)];
                _ImageFPS = new int[((int)ECamera.Count)];
                _EnableCalibrate = new bool[(int)ECamera.Count];
                _ChessboardRow = new int[(int)ECamera.Count];
                _ChessboardColumn = new int[(int)ECamera.Count];

                _Resolution = new double[((int)ECamera.Count)];
                _SerialNumberOrMonikerString = new string[((int)ECamera.Count)];
                CameraType = new ECameraType[(int)ECamera.Count];

                _CameraMatrix = new Mat[(int)ECamera.Count];
                _DistCoeffs = new Mat[(int)ECamera.Count];

                if (File.Exists(_FilePath))
                    ReadCameraParameter();
                else
                    CretaeCameraFile();

                if ((int)ECamera.Count > 0)
                {
                    CameraCount = LogitechDef.GetCameraQuantities() + BaslerDef.GetCameraQuantities();// + PointGreyDef.GetCameraQuantities() 
                    if (CameraCount < (int)ECamera.Count)
                    {
                        _ErrorCode = "Not enough cameras";
                        return;
                    }
                }

                _CameraBase = new CameraBaseDef[(int)ECamera.Count];
                AssignCamera();

                _task = new Thread(CheckCameraAlive);
                _task.IsBackground = true;
                _task.Priority = ThreadPriority.Lowest;
                _task.Start();
            }
            catch (Exception e)
            {
                _ErrorCode = e.ToString();
            }
        }

        private void CheckCameraAlive()
        {
            while (true)
            {
                for (int i = 0; i < (int)ECamera.Count; i++)
                {
                    Thread.Sleep(10);
                    if (_CameraBase[i] == null)
                    {
                        CommonLibrary.AlarmTextDisplay.Add((int)CommonLibrary.AlarmCode.Machine_CameraError, CommonLibrary.AlarmType.Alarm);
                        break;
                    }
                    if (!_CameraBase[i].IsAlive())
                    {
                        _CameraBase[i].Dispose();
                        if (CameraType[i] == ECameraType.Logitech)
                            _CameraBase[i] = new LogitechDef(_SerialNumberOrMonikerString[i], _Resolution[i]);
                        else if (CameraType[i] == ECameraType.Basler)
                            _CameraBase[i] = new BaslerDef(_SerialNumberOrMonikerString[i], _Resolution[i]);

                        Thread.Sleep(500);
                        if (!_CameraBase[i].IsAlive())
                        {
                            CommonLibrary.AlarmTextDisplay.Add((int)CommonLibrary.AlarmCode.Machine_CameraError, CommonLibrary.AlarmType.Alarm);
                            //throw new Exception("Camera" + ((ECAMERA)i).ToString() + " Error");
                        }
                    }
                }

                Thread.Sleep(30);
            }
        }

        public void AssignCamera()
        {
            for (int i = 0; i < CameraCount; i++)
            {
                if (i + 1 > _SerialNumberOrMonikerString.Length)
                    break;

                else if (CameraType[i] == ECameraType.Logitech)
                    _CameraBase[i] = new LogitechDef(_SerialNumberOrMonikerString[i], _Resolution[i]);

                else if (CameraType[i] == ECameraType.Basler)
                    _CameraBase[i] = new BaslerDef(_SerialNumberOrMonikerString[i], _Resolution[i]);

                string errorCode = string.Empty;
                if (!_CameraBase[i].IniSuccess(ref errorCode))
                    _CameraBase[i] = null;
            }
        }

        private void ReadCameraParameter()
        {
            IniFile cIni = new IniFile(_FilePath, true);

            for (int i = 0; i < (int)ECamera.Count; i++)
            {
                String sSection = ((ECamera)i).ToString();
                if (cIni.ReadStr(sSection, ECameraBasicKey.Type.ToString(), "") == ECameraType.Logitech.ToString())
                    CameraType[i] = ECameraType.Logitech;
                if (cIni.ReadStr(sSection, ECameraBasicKey.Type.ToString(), "") == ECameraType.PointGrey.ToString())
                    CameraType[i] = ECameraType.PointGrey;
                if (cIni.ReadStr(sSection, ECameraBasicKey.Type.ToString(), "") == ECameraType.Basler.ToString())
                    CameraType[i] = ECameraType.Basler;
                if (cIni.ReadStr(sSection, ECameraBasicKey.Type.ToString(), "") == ECameraType.SenTech.ToString())
                    CameraType[i] = ECameraType.SenTech;

                _SerialNumberOrMonikerString[i] = cIni.ReadStr(sSection, ECameraBasicKey.SerialNumberOrMonikerString.ToString(), "");
                _Resolution[i] = cIni.ReadDouble(sSection, ECameraBasicKey.Resolution.ToString(), 0);
                _DefaultExposureTime[i] = cIni.ReadDouble(sSection, ECameraBasicKey.ExposureTime.ToString(), 0);
                _ExposureTimeMin[i] = cIni.ReadInt(sSection, ECameraBasicKey.ExposureTimeMin.ToString(), -10);
                _ExposureTimeMax[i] = cIni.ReadInt(sSection, ECameraBasicKey.ExposureTimeMax.ToString(), 0);
                _ImageW[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageW.ToString(), 1280);
                _ImageH[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageH.ToString(), 960);
                _ImageFPS[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageFPS.ToString(), 60);
                _XYSwap[i] = cIni.ReadBool(sSection, ECameraBasicKey.XYSwap.ToString(), false);
                _XInverse[i] = cIni.ReadBool(sSection, ECameraBasicKey.XInverse.ToString(), false);
                _YInverse[i] = cIni.ReadBool(sSection, ECameraBasicKey.YInverse.ToString(), false);
                _ImageVerticalFlip[i] = cIni.ReadBool(sSection, ECameraBasicKey.ImageVerticalFlip.ToString(), false);
                _ImageHorizontalFlip[i] = cIni.ReadBool(sSection, ECameraBasicKey.ImageHorizontalFlip.ToString(), false);
                _ChessboardRow[i] = cIni.ReadInt(sSection, ECameraBasicKey.ChessboardRow.ToString(), 9);
                _ChessboardColumn[i] = cIni.ReadInt(sSection, ECameraBasicKey.ChessboardColumn.ToString(), 6);
                _EnableCalibrate[i] = cIni.ReadBool(sSection, ECameraBasicKey.Calibrate.ToString(), false);

                if (_EnableCalibrate[i])
                {
                    Calibrate(i, _FolderPath + "\\" + ((ECamera)i).ToString() + "Chessboard.bmp");
                }
            }

            cIni.FileClose();
            cIni.Dispose();
        }
        private void CretaeCameraFile()
        {
            IniFile cIni = new IniFile(_FilePath, false);

            for (int i = 0; i < (int)ECamera.Count; i++)
            {
                String sSection = ((ECamera)i).ToString();

                cIni.WriteStr(sSection, ECameraBasicKey.Type.ToString(), ECameraType.Logitech.ToString());
                CameraType[i] = ECameraType.Logitech;
                cIni.WriteStr(sSection, ECameraBasicKey.SerialNumberOrMonikerString.ToString(), "COM" + (i + 10).ToString("00"));
                _SerialNumberOrMonikerString[i] = "COM" + (i + 10).ToString("00");
                cIni.WriteDouble(sSection, ECameraBasicKey.Resolution.ToString(), 1.0);
                _Resolution[i] = 1.0;
                cIni.WriteDouble(sSection, ECameraBasicKey.ExposureTime.ToString(), 0.0);
                _DefaultExposureTime[i] = 0.0;
                cIni.WriteInt(sSection, ECameraBasicKey.ExposureTimeMin.ToString(), -10);
                _ExposureTimeMin[i] = -10;
                cIni.WriteInt(sSection, ECameraBasicKey.ExposureTimeMax.ToString(), 0);
                _ExposureTimeMax[i] = 0;
                cIni.WriteInt(sSection, ECameraBasicKey.ImageW.ToString(), 1280);
                _ImageW[i] = 1280;
                cIni.WriteInt(sSection, ECameraBasicKey.ImageH.ToString(), 960);
                _ImageH[i] = 960;
                cIni.WriteInt(sSection, ECameraBasicKey.ImageFPS.ToString(), 60);
                _ImageFPS[i] = 60;
                cIni.WriteBool(sSection, ECameraBasicKey.XYSwap.ToString(), false);
                _XYSwap[i] = false;
                cIni.WriteBool(sSection, ECameraBasicKey.XInverse.ToString(), false);
                _XInverse[i] = false;
                cIni.WriteBool(sSection, ECameraBasicKey.YInverse.ToString(), false);
                _YInverse[i] = false;
                cIni.WriteBool(sSection, ECameraBasicKey.ImageVerticalFlip.ToString(), false);
                _ImageVerticalFlip[i] = false;
                cIni.WriteBool(sSection, ECameraBasicKey.ImageHorizontalFlip.ToString(), false);
                _ImageHorizontalFlip[i] = false;
                cIni.WriteInt(sSection, ECameraBasicKey.ChessboardRow.ToString(), 9);
                _ChessboardRow[i] = 9;
                cIni.WriteInt(sSection, ECameraBasicKey.ChessboardColumn.ToString(), 6);
                _ChessboardColumn[i] = 6;
                cIni.WriteBool(sSection, ECameraBasicKey.Calibrate.ToString(), false);
                _EnableCalibrate[i] = false;

                if (_EnableCalibrate[i])
                {
                    Calibrate(i, _FolderPath + "\\" + ((ECamera)i).ToString() + "Chessboard.bmp");
                }
            }

            cIni.FileClose();
            cIni.Dispose();
        }
        public void Reload()
        {
            IniFile cIni = new IniFile(_FilePath, true);

            for (int i = 0; i < (int)ECamera.Count; i++)
            {
                String sSection = ((ECamera)i).ToString();

                _Resolution[i] = cIni.ReadDouble(sSection, ECameraBasicKey.Resolution.ToString(), 0);
                _ExposureTimeMin[i] = cIni.ReadInt(sSection, ECameraBasicKey.ExposureTimeMin.ToString(), -10);
                _ExposureTimeMax[i] = cIni.ReadInt(sSection, ECameraBasicKey.ExposureTimeMax.ToString(), 0);
                _ImageW[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageW.ToString(), 1280);
                _ImageH[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageH.ToString(), 960);
                _ImageFPS[i] = cIni.ReadInt(sSection, ECameraBasicKey.ImageFPS.ToString(), 60);
                _XYSwap[i] = cIni.ReadBool(sSection, ECameraBasicKey.XYSwap.ToString(), false);
                _XInverse[i] = cIni.ReadBool(sSection, ECameraBasicKey.XInverse.ToString(), false);
                _YInverse[i] = cIni.ReadBool(sSection, ECameraBasicKey.YInverse.ToString(), false);
                _ImageVerticalFlip[i] = cIni.ReadBool(sSection, ECameraBasicKey.ImageVerticalFlip.ToString(), false);
                _ImageHorizontalFlip[i] = cIni.ReadBool(sSection, ECameraBasicKey.ImageHorizontalFlip.ToString(), false);
                _ChessboardRow[i] = cIni.ReadInt(sSection, ECameraBasicKey.ChessboardRow.ToString(), 9);
                _ChessboardColumn[i] = cIni.ReadInt(sSection, ECameraBasicKey.ChessboardColumn.ToString(), 6);
                _EnableCalibrate[i] = cIni.ReadBool(sSection, ECameraBasicKey.Calibrate.ToString(), false);

                if (_EnableCalibrate[i])
                {
                    Calibrate(i, _FolderPath + "\\" + ((ECamera)i).ToString() + "Chessboard.bmp");
                }
            }

            cIni.FileClose();
            cIni.Dispose();
        }
        public void SaveExposure(ECamera eCamera, double eExposureTime)
        {
            if ((int)eCamera >= 0)
            {
                _DefaultExposureTime[(int)eCamera] = eExposureTime;

                IniFile cIni = new IniFile(_FilePath, false);

                String sSection = eCamera.ToString();

                cIni.WriteDouble(sSection, ECameraBasicKey.ExposureTime.ToString(), _DefaultExposureTime[(int)eCamera]);

                cIni.FileClose();
                cIni.Dispose();
            }
        }

        private void Calibrate(int nIndex, String sFilePath)
        {
            _CameraMatrix[nIndex] = new Mat(3, 3, Emgu.CV.CvEnum.DepthType.Cv32F, 1);
            _DistCoeffs[nIndex] = new Mat(1, 5, Emgu.CV.CvEnum.DepthType.Cv32F, 1);
            Mat cImage = CvInvoke.Imread(sFilePath, Emgu.CV.CvEnum.ImreadModes.Grayscale);

            System.Drawing.Size stPatternSize = new System.Drawing.Size(_ChessboardRow[nIndex], _ChessboardColumn[nIndex]);
            VectorOfPointF cCorners = new VectorOfPointF(stPatternSize.Width * stPatternSize.Height);

            bool find = CvInvoke.FindChessboardCorners(cImage, stPatternSize, cCorners);
            cImage.Dispose();

            if (find)
            {
                VectorOfVectorOfPointF cImagePointsList = new VectorOfVectorOfPointF();
                VectorOfVectorOfPoint3D32F cObjPointsList = new VectorOfVectorOfPoint3D32F();
                MCvPoint3D32f[] points = new MCvPoint3D32f[stPatternSize.Width * stPatternSize.Height];
                int loopIndex = 0;
                for (int i = 0; i < stPatternSize.Height; i++)
                {
                    for (int j = 0; j < stPatternSize.Width; j++)
                        points[loopIndex++] = new MCvPoint3D32f(j, i, 0);
                }

                cObjPointsList.Push(new VectorOfPoint3D32F(points));
                cImagePointsList.Push(cCorners);

                System.Drawing.PointF[][] imagePoints = cImagePointsList.ToArrayOfArray();
                MCvPoint3D32f[][] worldPoints = cObjPointsList.ToArrayOfArray();

                cObjPointsList.Dispose();
                cImagePointsList.Dispose();

                var termCriteria = new MCvTermCriteria(30, 0.1);

                int nImage = 1;
                Mat[] rotateMat = new Mat[nImage];
                for (int i = 0; i < nImage; i++)
                {
                    rotateMat[i] = new Mat(3, 3, DepthType.Cv32F, 1);
                }
                //平移矩阵T
                Mat[] transMat = new Mat[nImage];
                for (int i = 0; i < nImage; i++)
                {
                    transMat[i] = new Mat(3, 1, DepthType.Cv32F, 1);
                }
                CvInvoke.CalibrateCamera(
                    worldPoints,
                    imagePoints,

                    new System.Drawing.Size(_CameraBase[nIndex].GetWidth(), _CameraBase[nIndex].GetHeight()),
                    _CameraMatrix[nIndex],
                    _DistCoeffs[nIndex],
                    Emgu.CV.CvEnum.CalibType.RationalModel,
                    termCriteria,
                    out rotateMat, out transMat);
            }
        }

        public ECameraType GetCameraType(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return ECameraType.None;
            return CameraType[(int)eCamera];
        }

        public void CopyImage(ECamera CCD, Image<Bgr, byte> cImage)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)CCD || _CameraBase[(int)CCD] == null)
                return;

            if (_EnableCalibrate[(int)CCD])
                _CameraBase[(int)CCD].UndistortImage(cImage, _CameraMatrix[(int)CCD], _DistCoeffs[(int)CCD]);
            else
                _CameraBase[(int)CCD].CopyImage(cImage);


            if (_ImageVerticalFlip[(int)CCD])
                CvInvoke.Flip(cImage, cImage, FlipType.Vertical);
            if (_ImageHorizontalFlip[(int)CCD])
                CvInvoke.Flip(cImage, cImage, FlipType.Horizontal);
        }

        public bool CreateSucces(ref String sErrorCode)
        {
            if (_ErrorCode.Length > 0)
            {
                sErrorCode = _ErrorCode;
                return false;
            }

            for (int i = 0; i < _CameraBase.Count(); i++)
                if (_CameraBase[i] != null && !_CameraBase[i].IniSuccess(ref sErrorCode))
                    return false;

            return true;
        }

        public double GetResolution(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return 0;

            return _Resolution[(int)eCamera];
        }

        public bool GetXYSwap(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return false;

            return _XYSwap[(int)eCamera];
        }

        public bool GetXInverse(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return false;

            return _XInverse[(int)eCamera];
        }

        public bool GetYInverse(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return false;

            return _YInverse[(int)eCamera];
        }

        public CameraBaseDef GetCamera(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return null;
            return _CameraBase[(int)eCamera];
        }

        public void Dispose()
        {
            if (_CameraBase != null)
            {
                for (int i = 0; i < _CameraBase.Length; i++)
                {
                    if (_CameraBase[i] != null)
                        _CameraBase[i].Dispose();

                    if (_DistCoeffs[i] != null)
                        _DistCoeffs[i].Dispose();

                    if (_CameraMatrix[i] != null)
                        _CameraMatrix[i].Dispose();
                }
            }
        }

        public int GetWidth(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return 0;

            return _CameraBase[(int)eCamera].GetWidth();
        }

        public int GetHeight(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return 0;
            return _CameraBase[(int)eCamera].GetHeight();
        }

        private CameraBaseDef.PropertyInfo GetExposureTimeRange(ECamera eCamera)
        {
            CameraBaseDef.PropertyInfo info = new CameraBaseDef.PropertyInfo();
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return info;

            return _CameraBase[(int)eCamera].GetExposureTimeRange();
        }
        public int GetExposureTimeRangeMax(ECamera eCamera, bool eFileValue = false)
        {
            if (eFileValue)
                return _ExposureTimeMax[(int)eCamera];
            else
                return GetExposureTimeRange(eCamera).iMax;
        }
        public int GetExposureTimeRangeMin(ECamera eCamera, bool eFileValue = false)
        {
            if (eFileValue)
                return _ExposureTimeMin[(int)eCamera];
            else
                return GetExposureTimeRange(eCamera).iMin;
        }
        public int GetExposureTimeRangeStep(ECamera eCamera)
        {
            _ExposureTimeStep[(int)eCamera] = GetExposureTimeRange(eCamera).iStep;
            return _ExposureTimeStep[(int)eCamera];
        }
        public int GetExposure(ECamera eCamera, bool Default = false)
        {
            if (!Default)
                return  (int)GetExposureTimeRange(eCamera).iValue;

            return (int)_DefaultExposureTime[(int)eCamera];
        }
        public void SetExposureByDefault(ECamera eCamera)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return;

            _CameraBase[(int)eCamera].SetExposureTime((int)_DefaultExposureTime[(int)eCamera]);
        }
        public void SetExposure(ECamera eCamera, double fExposure)
        {
            if (_CameraBase == null || _CameraBase.Length <= (int)eCamera || _CameraBase[(int)eCamera] == null)
                return;

            _CameraBase[(int)eCamera].SetExposureTime((int)fExposure);
        }
    }
}