using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FileStreamLibrary;
using nsAlignAlgorithm;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models.Abstract;
using static VisionLibrary.Process;

namespace VisionLibrary
{

    public enum ECorner
    {
        Corner1,
        Corner2,

        Count
    }

    /// <summary>色域</summary>
    public enum ColorGamut
    {
        Blue,
        Green,
        Red,

        Count
    }

    /// <summary>判定物件</summary>
    public enum ItemsType
    {
        None = -1,

        /// <summary>Mylar</summary>
        Mylar,
        /// <summary>棧板</summary>
        Pallet,
        /// <summary>PCB</summary>
        PCB,
        /// <summary>淺盤</summary>
        Tray
    }

    public enum EVisionErrorCode
    {
        CornerNotFound,
        MatchFail,
        SizeError,
        CircleNotFound,
    }

    public enum ECCDQuadrant
    {
        Quadrant1,
        Quadrant2,
        Quadrant3,
        Quadrant4,
    }

    public enum EAlignIndex
    {
        Align1,
        Align2,
        Align3,
        Count
    }

    public enum EAlignType
    {
        Match,
        FindCorner,
        BlobByPoint,
        FindCornerByTriangle,
        MatchBox,
        Yolo,
        ObjectCenterByPoint,
    }

    public enum EAlignPositionType
    {
        PanelSize,
        MatchPosition,
    }



    public class VisionManagerDef : IDisposable
    {
        public bool ComponentsReady;
        public CameraCollectionDef CameraCollection;
        public CornerDetectorDef[] CornerDetector;
        public SaveImageDef SaveImg;
        public Image<Bgr, byte>[] _ImgDisplay;

        private cAlignAlgorithmDef[] _AlignAlgorithm;
        private Point[][] _OrgImagCorner;
        public PointF[][] ResultCorner { get; private set; }
        public double[][] ResultCornerDeg { get; private set; }
        public LineSegment2D[][][] ResultCornerLines { get; private set; }
        private ECamera[][] _Camera;

        public PointF[][] _CCDCenterPoint { private set; get; }
        public PointF[] _PanelCenterPoint { private set; get; }
        private PointF[] _RobotCenter;
        private double[] _PanelXLength;
        private double[] _PanelYLength;

        #region blob parameter
        private int[][] _BlobThreshold;
        private int[][] _BlobMinArea;
        private int[][] _BlobMaxArea;
        private double[][] _BlobMinCircularity;
        #endregion

        #region Match Parameter
        public MatchDef[][] Match;
        #endregion

        #region Corner Parameter
        private double[][] _CornerXv;
        private double[][] _CornerYv;
        private int[][] _CornerHoughMinLineGap;
        private int[][] _CornerHoughLinesMinLen;
        private int[][] _CornerHoughLinesThrehold;
        private int[][] _CornerAntiBrightNoise;
        private CornerDetectorDef.DetectionDirection[][] _CornerDirection;
        #endregion

        #region Find corner by triangle
        private double[][] _FCBTLineLengthThreshold;
        private int[][] _FCBTThreshold;
        private int[][] _FCBTCornerMaskSize;
        private int[][] _FCBTUseBlur;
        private Process.CornerSide[][] _FCBTCornerSide;
        #endregion

        #region MatchBox
        private Process.CornerSide[][] _MatchBoxCornerSides;
        private int[][] _MatchBoxSearchHalfW;
        private int[][] _MatchBoxSearchHalfH;
        #endregion

        #region Yolo
        YoloScorer<CornerModel>[][] _CornerModel;
        #endregion

        private Thread[] _Task;
        private bool[] _AlignSuccess;
        private double[] _AlignTolerance;
        public string[] ImageFormula;
        private string _SystemDirPath;
        private Rectangle[][] _CornerROILimit;
        private int[] _SearchHalfWidth;
        private int[] _SearchHalfHeight;

        private double[][] _CCDAngle;
        private double[] _ShiftX;
        private double[] _ShiftY;
        private double[] _ShiftAngle;
        private bool[] _DoAlign;
        private EAlignType[] _AlignType;
        private EAlignPositionType[] _AlignPositionTypes;
        private ECCDQuadrant[][] _CCDQuadrant;
        private bool[] _saveImage;
   
        public EVisionErrorCode[] ErrorCode { get; private set; }

        public VisionManagerDef(string sSystemDirPath)
        {
            _SystemDirPath = sSystemDirPath;
            if (!Directory.Exists(_SystemDirPath))
                Directory.CreateDirectory(_SystemDirPath);

            try
            {
                _ImgDisplay = new Image<Bgr, byte>[(int)ECamera.Count];
                ComponentsReady = true;
                ErrorCode = new EVisionErrorCode[(int)EAlignIndex.Count];
                ResultCorner = new PointF[(int)EAlignIndex.Count][];
                ResultCornerDeg = new double[(int)EAlignIndex.Count][];
                ResultCornerLines = new LineSegment2D[(int)EAlignIndex.Count][][];
                Match = new MatchDef[(int)EAlignIndex.Count][];
                _OrgImagCorner = new Point[(int)EAlignIndex.Count][];
                _Camera = new ECamera[(int)EAlignIndex.Count][];
                _CornerROILimit = new Rectangle[(int)EAlignIndex.Count][];
                _CCDQuadrant = new ECCDQuadrant[(int)EAlignIndex.Count][];
                _saveImage = new bool[(int)EAlignIndex.Count];
                _CornerDirection = new CornerDetectorDef.DetectionDirection[(int)EAlignIndex.Count][];
                _CCDCenterPoint = new PointF[(int)EAlignIndex.Count][];

                _PanelCenterPoint = new PointF[(int)EAlignIndex.Count];
                _RobotCenter = new PointF[(int)EAlignIndex.Count];
                _PanelXLength = new double[(int)EAlignIndex.Count];
                _PanelYLength = new double[(int)EAlignIndex.Count];

                _AlignAlgorithm = new cAlignAlgorithmDef[(int)EAlignIndex.Count];
                CornerDetector = new CornerDetectorDef[(int)EAlignIndex.Count];
                SaveImg = new SaveImageDef();

                _Task = new Thread[(int)EAlignIndex.Count];
                ImageFormula = new string[(int)EAlignIndex.Count];

                _AlignSuccess = new bool[(int)EAlignIndex.Count];
                _AlignTolerance = new double[(int)EAlignIndex.Count];
                _DoAlign = new bool[(int)EAlignIndex.Count];
                _ShiftX = new double[(int)EAlignIndex.Count];
                _ShiftY = new double[(int)EAlignIndex.Count];
                _ShiftAngle = new double[(int)EAlignIndex.Count];
                _AlignType = new EAlignType[(int)EAlignIndex.Count];
                _AlignPositionTypes = new EAlignPositionType[(int)EAlignIndex.Count];
                _SearchHalfWidth = new int[(int)EAlignIndex.Count];
                _SearchHalfHeight = new int[(int)EAlignIndex.Count];
                _CCDAngle = new double[(int)EAlignIndex.Count][];

                _BlobThreshold = new int[(int)EAlignIndex.Count][];
                _BlobMinArea = new int[(int)EAlignIndex.Count][];
                _BlobMaxArea = new int[(int)EAlignIndex.Count][];
                _BlobMinCircularity = new double[(int)EAlignIndex.Count][];

                _CornerXv = new double[(int)EAlignIndex.Count][];
                _CornerYv = new double[(int)EAlignIndex.Count][];
                _CornerHoughMinLineGap = new int[(int)EAlignIndex.Count][];
                _CornerHoughLinesMinLen = new int[(int)EAlignIndex.Count][];
                _CornerHoughLinesThrehold = new int[(int)EAlignIndex.Count][];
                _CornerAntiBrightNoise = new int[(int)EAlignIndex.Count][];

                _FCBTLineLengthThreshold = new double[(int)EAlignIndex.Count][];
                _FCBTThreshold = new int[(int)EAlignIndex.Count][];
                _FCBTCornerMaskSize = new int[(int)EAlignIndex.Count][];
                _FCBTUseBlur = new int[(int)EAlignIndex.Count][];
                _FCBTCornerSide = new Process.CornerSide[(int)EAlignIndex.Count][];
                _MatchBoxSearchHalfW = new int[(int)EAlignIndex.Count][];
                _MatchBoxSearchHalfH = new int[(int)EAlignIndex.Count][];
                _CornerModel = new YoloScorer<CornerModel>[(int)EAlignIndex.Count][];

                for (int i = 0; i < (int)EAlignIndex.Count; i++)
                {
                    ResultCorner[i] = new PointF[(int)ECorner.Count];
                    ResultCornerDeg[i] = new double[(int)ECorner.Count];
                    ResultCornerLines[i] = new LineSegment2D[(int)ECorner.Count][];

                    Match[i] = new MatchDef[(int)ECorner.Count];
                    _OrgImagCorner[i] = new Point[(int)ECorner.Count];
                    _Camera[i] = new ECamera[(int)ECorner.Count];
                    _CornerROILimit[i] = new Rectangle[(int)ECorner.Count];
                    _CCDQuadrant[i] = new ECCDQuadrant[(int)ECorner.Count];
                    _CornerDirection[i] = new CornerDetectorDef.DetectionDirection[(int)ECorner.Count];
                    _CCDCenterPoint[i] = new PointF[(int)ECorner.Count];
                    _CCDAngle[i] = new double[(int)ECorner.Count];

                    #region blob parameter
                    _BlobThreshold[i] = new int[(int)ECorner.Count];
                    _BlobMinArea[i] = new int[(int)ECorner.Count];
                    _BlobMaxArea[i] = new int[(int)ECorner.Count];
                    _BlobMinCircularity[i] = new double[(int)ECorner.Count];
                    #endregion

                    #region Corner Parameter
                    _CornerXv[i] = new double[(int)ECorner.Count];
                    _CornerYv[i] = new double[(int)ECorner.Count];
                    _CornerHoughMinLineGap[i] = new int[(int)ECorner.Count];
                    _CornerHoughLinesMinLen[i] = new int[(int)ECorner.Count];
                    _CornerHoughLinesThrehold[i] = new int[(int)ECorner.Count];
                    _CornerAntiBrightNoise[i] = new int[(int)ECorner.Count];
                    #endregion

                    _FCBTLineLengthThreshold[i] = new double[(int)ECorner.Count];
                    _FCBTThreshold[i] = new int[(int)ECorner.Count];
                    _FCBTCornerMaskSize[i] = new int[(int)ECorner.Count];
                    _FCBTUseBlur[i] = new int[(int)ECorner.Count];
                    _FCBTCornerSide[i] = new Process.CornerSide[(int)ECorner.Count];
                    _MatchBoxSearchHalfW[i] = new int[(int)ECorner.Count];
                    _MatchBoxSearchHalfH[i] = new int[(int)ECorner.Count];
                    _CornerModel[i] = new YoloScorer<CornerModel>[(int)ECorner.Count];

                    for (int j = 0; j < (int)ECorner.Count; j++)
                    {
                        ResultCorner[i][j] = new PointF();
                        ResultCornerDeg[i][j] = 0;
                        ResultCornerLines[i][j] = new LineSegment2D[2];
                        Match[i][j] = new MatchDef();
                        _OrgImagCorner[i][j] = new Point();
                        _Camera[i][j] = new ECamera();
                        _CornerROILimit[i][j] = new Rectangle();
                        _CCDQuadrant[i][j] = new ECCDQuadrant();
                        _CornerDirection[i][j] = new CornerDetectorDef.DetectionDirection();
                        _CCDCenterPoint[i][j] = new PointF();
                        _CCDAngle[i][j] = 0;

                        #region blob parameter
                        _BlobThreshold[i][j] = 0;
                        _BlobMinArea[i][j] = 0;
                        _BlobMaxArea[i][j] = 0;
                        _BlobMinCircularity[i][j] = 0;
                        #endregion

                        #region Corner Parameter
                        _CornerXv[i][j] = 0;
                        _CornerYv[i][j] = 0;
                        _CornerHoughMinLineGap[i][j] = 0;
                        _CornerHoughLinesMinLen[i][j] = 0;
                        _CornerHoughLinesThrehold[i][j] = 0;
                        _CornerAntiBrightNoise[i][j] = 0;
                        #endregion

                        _FCBTLineLengthThreshold[i][j] = 0;
                        _FCBTThreshold[i][j] = 0;
                        _FCBTCornerMaskSize[i][j] = 0;
                        _FCBTUseBlur[i][j] = 0;
                        _FCBTCornerSide[i][j] = 0;
                        _MatchBoxSearchHalfW[i][j] = 0;
                        _MatchBoxSearchHalfH[i][j] = 0;
                        _CornerModel[i][j] = new YoloScorer<CornerModel>();
                    }

                    _AlignAlgorithm[i] = new cAlignAlgorithmDef();
                    CornerDetector[i] = new CornerDetectorDef();
                    _AlignSuccess[i] = false;
                    _AlignTolerance[i] = 10;

                    _ShiftX[i] = 0;
                    _ShiftY[i] = 0;
                    _ShiftAngle[i] = 0;

                    _DoAlign[i] = false;

                    ImageFormula[i] = "G";

                    _Task[i] = new Thread(AlignTask);
                    _Task[i].IsBackground = true;
                    _Task[i].Priority = ThreadPriority.Normal;
                    _Task[i].Start(i);
                }

                CameraCollection = new CameraCollectionDef(_SystemDirPath);
                String sErrorCode = String.Empty;
                ReadSetting();
                UpdateAlignParameter();
                if (!CameraCollection.CreateSucces(ref sErrorCode))
                {
                    ComponentsReady = false;
                    MessageBox.Show(sErrorCode);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void TaskAlign(EAlignIndex Index)
        {
            _DoAlign[(int)Index] = true;
        }

        public bool AlignDone(EAlignIndex Index)
        {
            return !_DoAlign[(int)Index];
        }

        public bool AlignSucess(EAlignIndex Index)
        {
            return _AlignSuccess[(int)Index];
        }

        private void AlignTask(Object para)
        {
            while (true)
            {
                if (_DoAlign[Convert.ToInt32(para)])
                {
                    try
                    {
                        _AlignSuccess[Convert.ToInt32(para)] = Align(
                            (EAlignIndex)Convert.ToInt32(para),
                            ref _ShiftX[Convert.ToInt32(para)],
                            ref _ShiftY[Convert.ToInt32(para)],
                            ref _ShiftAngle[Convert.ToInt32(para)]);

                        _DoAlign[Convert.ToInt32(para)] = false;
                    }
                    catch(Exception ex)
                    {
                        CommonLibrary.AlarmTextDisplay.Add((int)CommonLibrary.AlarmCode.Alarm, CommonLibrary.AlarmType.Alarm, ex.ToString());
                    }
                }

                Thread.Sleep(1);
            }
        }

        public double GetShiftX(EAlignIndex Index)
        {
            return _ShiftX[(int)Index];
        }

        public double GetShiftY(EAlignIndex Index)
        {
            return _ShiftY[(int)Index];
        }

        public double GetShiftAngle(EAlignIndex Index)
        {
            return _ShiftAngle[(int)Index];
        }

        private void CreateSetting()
        {
            IniFile ini = new IniFile(_SystemDirPath + "\\Vision.ini", false);

            for (int i = 0; i < (int)EAlignIndex.Count; i++)
            {
                for (int j = 0; j < (int)ECorner.Count; j++)
                {
                    ini.WriteInt("EAlignIndex" + i.ToString(), "ECAMERA" + j.ToString(), i * 2 + j);
                    _Camera[i][j] = (ECamera)ini.ReadInt("EAlignIndex" + i.ToString(), "ECAMERA" + j.ToString(), i * 2 + j);

                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerROILimitX" + j.ToString(), 0);
                    _CornerROILimit[i][j].X = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitX" + j.ToString(), 0);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerROILimitY" + j.ToString(), 0);
                    _CornerROILimit[i][j].Y = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitY" + j.ToString(), 0);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerROILimitW" + j.ToString(), 1024);
                    _CornerROILimit[i][j].Width = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitW" + j.ToString(), 1024);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerROILimitH" + j.ToString(), 768);
                    _CornerROILimit[i][j].Height = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitH" + j.ToString(), 768);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CCDQuadrant" + j.ToString(), 0);
                    _CCDQuadrant[i][j] = (ECCDQuadrant)ini.ReadInt("EAlignIndex" + i.ToString(), "CCDQuadrant" + j.ToString(), 0);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "DetectionDirection" + j.ToString(), 0);
                    _CornerDirection[i][j] = (CornerDetectorDef.DetectionDirection)ini.ReadInt("EAlignIndex" + i.ToString(), "CornerDetectionDirection" + j.ToString(), 0);
                    ini.WriteFloat("EAlignIndex" + i.ToString(), "CCDCenterPointX" + j.ToString(), (float)0);
                    _CCDCenterPoint[i][j].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "CCDCenterPointX" + j.ToString(), 0);
                    ini.WriteFloat("EAlignIndex" + i.ToString(), "CCDCenterPointY" + j.ToString(), (float)0);
                    _CCDCenterPoint[i][j].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "CCDCenterPointY" + j.ToString(), 0);
                    Match[i][j].Load(GetMatchPath((EAlignIndex)i, (ECorner)j));

                    ini.WriteInt("EAlignIndex" + i.ToString(), "BlobThreshold" + j.ToString(), 150);
                    _BlobThreshold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobThreshold" + j.ToString(), 150);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "BlobMinArea" + j.ToString(), 500);
                    _BlobMinArea[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobMinArea" + j.ToString(), 200);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "BlobMaxArea" + j.ToString(), 3600);
                    _BlobMaxArea[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobMaxArea" + j.ToString(), 20000);
                    ini.WriteDouble("EAlignIndex" + i.ToString(), "BlobMinCircularity" + j.ToString(), (Double)0.3);
                    _BlobMinCircularity[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "BlobMinCircularity" + j.ToString(), 0.3);

                    ini.WriteDouble("EAlignIndex" + i.ToString(), "CornerXv", (Double)0.3);
                    _CornerXv[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "CornerXv" + j.ToString(), 0.3);
                    ini.WriteDouble("EAlignIndex" + i.ToString(), "CornerYv", (Double)0.3);
                    _CornerYv[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "CornerYv" + j.ToString(), 0.3);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerHoughMinLineGap" + j.ToString(), 5);
                    _CornerHoughMinLineGap[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughMinLineGap" + j.ToString(), 5);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerHoughLinesMinLen" + j.ToString(), 50);
                    _CornerHoughLinesMinLen[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughLinesMinLen" + j.ToString(), 50);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "HoughLinesThrehold" + j.ToString(), 50);
                    _CornerHoughLinesThrehold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughLinesThrehold" + j.ToString(), 50);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "CornerAntiBrightNoise" + j.ToString(), 200);
                    _CornerAntiBrightNoise[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerAntiBrightNoise" + j.ToString(), 200);

                    ini.WriteDouble("EAlignIndex" + i.ToString(), "FCBTLineLengthThreshold" + j.ToString(), 50);
                    _FCBTLineLengthThreshold[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "FCBTLineLengthThreshold" + j.ToString(), 50);
                    ini.WriteDouble("EAlignIndex" + i.ToString(), "FCBTThreshold" + j.ToString(), 200);
                    _FCBTThreshold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTThreshold" + j.ToString(), 200);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "FCBTCornerMaskSize" + j.ToString(), 0);
                    _FCBTCornerMaskSize[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTCornerMaskSize" + j.ToString(), 0);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "FCBTUseBlur" + j.ToString(), 0);
                    _FCBTUseBlur[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTUseBlur" + j.ToString(), 0);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "FCBTCornerSide" + j.ToString(), 0);
                    _FCBTCornerSide[i][j] = (Process.CornerSide)ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTCornerSide" + j.ToString(), 0);

                    ini.WriteInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfW" + j.ToString(), 150);
                    _MatchBoxSearchHalfW[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfW" + j.ToString(), 150);
                    ini.WriteInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfH" + j.ToString(), 150);
                    _MatchBoxSearchHalfH[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfH" + j.ToString(), 150);
                }

                ini.WriteInt("EAlignIndex" + i.ToString(), "EAlignType", 0);
                _AlignType[i] = (EAlignType)ini.ReadInt("EAlignIndex" + i.ToString(), "EAlignType", 0);
                ini.WriteInt("EAlignIndex" + i.ToString(), "EAlignPositionType", 0);
                _AlignPositionTypes[i] = (EAlignPositionType)ini.ReadInt("EAlignIndex" + i.ToString(), "EAlignPositionType", 0);

                ini.WriteInt("EAlignIndex" + i.ToString(), "SearchHalfW", 0);
                _SearchHalfWidth[i] = ini.ReadInt("EAlignIndex" + i.ToString(), "SearchHalfW", 0);
                ini.WriteInt("EAlignIndex" + i.ToString(), "SearchHalfH", 0);
                _SearchHalfHeight[i] = ini.ReadInt("EAlignIndex" + i.ToString(), "SearchHalfH", 0);

                ini.WriteDouble("EAlignIndex" + i.ToString(), "AlignTolerance", (Double)0);
                _AlignTolerance[i] = ini.ReadDouble("EAlignIndex" + i.ToString(), "AlignTolerance", 0);
                ini.WriteStr("EAlignIndex" + i.ToString(), "Formula", "G");
                ImageFormula[i] = ini.ReadStr("EAlignIndex" + i.ToString(), "Formula", "G");

                ini.WriteFloat("EAlignIndex" + i.ToString(), "PanelCenterPointX", (float)0);
                _PanelCenterPoint[i].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelCenterPointX", 0);
                ini.WriteFloat("EAlignIndex" + i.ToString(), "PanelCenterPointY", (float)0);
                _PanelCenterPoint[i].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelCenterPointY", 0);

                ini.WriteFloat("EAlignIndex" + i.ToString(), "RobotCenterX", (float)0);
                _RobotCenter[i].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "RobotCenterX", 0);
                ini.WriteFloat("EAlignIndex" + i.ToString(), "RobotCenterY", (float)0);
                _RobotCenter[i].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "RobotCenterY", 0);

                ini.WriteFloat("EAlignIndex" + i.ToString(), "PanelXLength", (float)0);
                _PanelXLength[i] = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelXLength", 0);
                ini.WriteFloat("EAlignIndex" + i.ToString(), "PanelYLength", (float)0);
                _PanelYLength[i] = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelYLength", 0);

                ini.WriteBool("EAlignIndex" + i.ToString(), "SaveImage", true);
                _saveImage[i] = ini.ReadBool("EAlignIndex" + i.ToString(), "SaveImage", true);
            }

            ini.FileClose();
            ini.Dispose();
        }
        public void ReadSetting()
        {
            if (File.Exists(_SystemDirPath + "\\Vision.ini"))
            {
                IniFile ini = new IniFile(_SystemDirPath + "\\Vision.ini", true);

                for (int i = 0; i < (int)EAlignIndex.Count; i++)
                {
                    for (int j = 0; j < (int)ECorner.Count; j++)
                    {
                        _Camera[i][j] = (ECamera)ini.ReadInt("EAlignIndex" + i.ToString(), "ECAMERA" + j.ToString(), i * 2 + j);

                        _CornerROILimit[i][j].X = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitX" + j.ToString(), 0);
                        _CornerROILimit[i][j].Y = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitY" + j.ToString(), 0);
                        _CornerROILimit[i][j].Width = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitW" + j.ToString(), 1024);
                        _CornerROILimit[i][j].Height = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerROILimitH" + j.ToString(), 768);
                        _CCDQuadrant[i][j] = (ECCDQuadrant)ini.ReadInt("EAlignIndex" + i.ToString(), "CCDQuadrant" + j.ToString(), 0);
                        _CornerDirection[i][j] = (CornerDetectorDef.DetectionDirection)ini.ReadInt("EAlignIndex" + i.ToString(), "CornerDirection" + j.ToString(), 0);
                        _CCDCenterPoint[i][j].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "CCDCenterPointX" + j.ToString(), 0);
                        _CCDCenterPoint[i][j].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "CCDCenterPointY" + j.ToString(), 0);
                        _CCDAngle[i][j] = ini.ReadFloat("EAlignIndex" + i.ToString(), "CCDAngle" + j.ToString(), 0);
                        _BlobThreshold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobThreshold" + j.ToString(), 180);
                        _BlobMinArea[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobMinArea" + j.ToString(), 200);
                        _BlobMaxArea[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "BlobMaxArea" + j.ToString(), 15000);
                        _BlobMinCircularity[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "BlobMinCircularity" + j.ToString(), 0.3);
                        _CornerXv[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "CornerXv" + j.ToString(), 0.3);
                        _CornerYv[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "CornerYv" + j.ToString(), 0.3);
                        _CornerHoughMinLineGap[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughMinLineGap" + j.ToString(), 5);
                        _CornerHoughLinesMinLen[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughLinesMinLen" + j.ToString(), 50);
                        _CornerHoughLinesThrehold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerHoughLinesThrehold" + j.ToString(), 50);
                        _CornerAntiBrightNoise[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "CornerAntiBrightNoise" + j.ToString(), 200);
                        _FCBTLineLengthThreshold[i][j] = ini.ReadDouble("EAlignIndex" + i.ToString(), "FCBTLineLengthThreshold" + j.ToString(), 50);
                        _FCBTThreshold[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTThreshold" + j.ToString(), 200);
                        _FCBTCornerMaskSize[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTCornerMaskSize" + j.ToString(), 0);
                        _FCBTUseBlur[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTUseBlur" + j.ToString(), 0);
                        _FCBTCornerSide[i][j] = (Process.CornerSide)ini.ReadInt("EAlignIndex" + i.ToString(), "FCBTCornerSide" + j.ToString(), 0);
                        _MatchBoxSearchHalfW[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfW" + j.ToString(), 150);
                        _MatchBoxSearchHalfH[i][j] = ini.ReadInt("EAlignIndex" + i.ToString(), "MatchBoxSearchHalfH" + j.ToString(), 150);

                        if (File.Exists(_SystemDirPath + "\\" + "Model" + "\\" + "EAlignIndex" + i.ToString() + "CornerModel" + j.ToString() + ".onnx"))
                            _CornerModel[i][j] = new YoloScorer<CornerModel>(_SystemDirPath + "\\" + "Model" + "\\" + "EAlignIndex" + i.ToString() + "CornerModel" + j.ToString() + ".onnx");
                        Match[i][j].Load(GetMatchPath((EAlignIndex)i, (ECorner)j));
                    }

                    _AlignType[i] = (EAlignType)ini.ReadInt("EAlignIndex" + i.ToString(), "EAlignType", 0);
                    _AlignPositionTypes[i] = (EAlignPositionType)ini.ReadInt("EAlignIndex" + i.ToString(), "EAlignPositionType", 0);

                    _SearchHalfWidth[i] = ini.ReadInt("EAlignIndex" + i.ToString(), "SearchHalfW", 0);
                    _SearchHalfHeight[i] = ini.ReadInt("EAlignIndex" + i.ToString(), "SearchHalfH", 0);

                    _AlignTolerance[i] = ini.ReadDouble("EAlignIndex" + i.ToString(), "AlignTolerance", 0);
                    ImageFormula[i] = ini.ReadStr("EAlignIndex" + i.ToString(), "Formula", "G");

                    _PanelCenterPoint[i].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelCenterPointX", 0);
                    _PanelCenterPoint[i].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelCenterPointY", 0);

                    _RobotCenter[i].X = ini.ReadFloat("EAlignIndex" + i.ToString(), "RobotCenterX", 0);
                    _RobotCenter[i].Y = ini.ReadFloat("EAlignIndex" + i.ToString(), "RobotCenterY", 0);

                    _PanelXLength[i] = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelXLength", 0);
                    _PanelYLength[i] = ini.ReadFloat("EAlignIndex" + i.ToString(), "PanelYLength", 0);

                    _saveImage[i] = ini.ReadBool("EAlignIndex" + i.ToString(), "SaveImage", true);
                }

                ini.FileClose();
                ini.Dispose();
            }
            else
                CreateSetting();
        }

        public void ReloadSetting()
        {
            ReadSetting();
        }

        private string GetMatchPath(EAlignIndex AlingIndex, ECorner Corner)
        {
            return _SystemDirPath + "\\Match\\" + "EAlignIndex" + ((int)AlingIndex).ToString() + "_Match" + ((int)Corner).ToString() + "\\";
        }

        public void UpdateAlignByPanelSize(EAlignIndex eAlignIndex)
        {
            int index = (int)eAlignIndex;
            RectangleF Panel = new RectangleF(
                _PanelCenterPoint[index].X - (float)_PanelXLength[index] / 2,
                _PanelCenterPoint[index].Y - (float)_PanelYLength[index] / 2,
                (float)_PanelXLength[index],
                (float)_PanelYLength[index]);

            PointF[] ccdPos = new PointF[(int)ECorner.Count];
            for (int j = 0; j < (int)ECorner.Count; j++)
            {
                ccdPos[j] = new PointF();
                switch (_CCDQuadrant[index][j])
                {
                    case ECCDQuadrant.Quadrant1:
                        ccdPos[j].X = Panel.X + (float)_PanelXLength[index];
                        ccdPos[j].Y = Panel.Y + (float)_PanelYLength[index];
                        break;

                    case ECCDQuadrant.Quadrant2:
                        ccdPos[j].X = Panel.X;
                        ccdPos[j].Y = Panel.Y + (float)_PanelYLength[index];
                        break;

                    case ECCDQuadrant.Quadrant3:
                        ccdPos[j].X = Panel.X;
                        ccdPos[j].Y = Panel.Y;
                        break;

                    case ECCDQuadrant.Quadrant4:
                        ccdPos[j].X = Panel.X + (float)_PanelXLength[index];
                        ccdPos[j].Y = Panel.Y;
                        break;
                }

                _OrgImagCorner[index][j] = GetImagePoint(_Camera[index][j], ccdPos[j], _CCDCenterPoint[index][j]);
            }

            _AlignAlgorithm[index].vSetAlignPos(
                ccdPos[(int)ECorner.Corner1].X,
                ccdPos[(int)ECorner.Corner1].Y,
                ccdPos[(int)ECorner.Corner2].X,
                ccdPos[(int)ECorner.Corner2].Y);

            _AlignAlgorithm[index].vSetCenterPos(
                _RobotCenter[index].X,
                _RobotCenter[index].Y);
        }
        public void UpdateAlignParameter()
        {
            for (int i = 0; i < (int)EAlignIndex.Count; i++)
            {
                if (_AlignPositionTypes[i] == EAlignPositionType.PanelSize)
                    UpdateAlignByPanelSize((EAlignIndex)i);
                else if (_AlignPositionTypes[i] == EAlignPositionType.MatchPosition)
                    UpdateAlignByMatchPosion((EAlignIndex)i);
            }
        }

        public void UpdateAlignByMatchPosion(
            EAlignIndex eAlignIndex)
        {
            PointF[] ccdPos = new PointF[(int)ECorner.Count];
            for (int j = 0; j < (int)ECorner.Count; j++)
            {

                ccdPos[j] = GetCartesianPoint(
                    _Camera[(int)eAlignIndex][j],
                    _CCDCenterPoint[(int)eAlignIndex][j],
                    Match[(int)eAlignIndex][j].GetLearnCenterPoint());

                _OrgImagCorner[(int)eAlignIndex][j] = Match[(int)eAlignIndex][j].GetLearnCenterPoint();
            }

            _AlignAlgorithm[(int)eAlignIndex].vSetAlignPos(
                ccdPos[(int)ECorner.Corner1].X,
                ccdPos[(int)ECorner.Corner1].Y,
                ccdPos[(int)ECorner.Corner2].X,
                ccdPos[(int)ECorner.Corner2].Y);

            _AlignAlgorithm[(int)eAlignIndex].vSetCenterPos(
                _RobotCenter[(int)eAlignIndex].X,
                _RobotCenter[(int)eAlignIndex].Y);
        }

        public Image<Bgr, byte> GetImage(ECamera eCAMERA)
        {
            return _ImgDisplay[(int)eCAMERA];
        }

        public void SetImage(ECamera eCAMERA, Image<Bgr, byte> image)
        {
            _ImgDisplay[(int)eCAMERA] = image;
        }

        public bool Align(
            EAlignIndex eAlignIndex,
            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            SaveImg.SetSave(_saveImage[(int)eAlignIndex]);

            if (_ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]] == null)
                _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]] = new Image<Bgr, byte>(CameraCollection.GetWidth(_Camera[(int)eAlignIndex][(int)ECorner.Corner1]), CameraCollection.GetHeight(_Camera[(int)eAlignIndex][(int)ECorner.Corner1]));

            _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]].ROI = Rectangle.Empty;
            CameraCollection.CopyImage(_Camera[(int)eAlignIndex][(int)ECorner.Corner1], _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]]);

            if (_ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]] == null)
                _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]] = new Image<Bgr, byte>(CameraCollection.GetWidth(_Camera[(int)eAlignIndex][(int)ECorner.Corner2]), CameraCollection.GetHeight(_Camera[(int)eAlignIndex][(int)ECorner.Corner2]));

            _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]].ROI = Rectangle.Empty;
            CameraCollection.CopyImage(_Camera[(int)eAlignIndex][(int)ECorner.Corner2], _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]]);
            
            Image<Gray, byte> srcImage1 = ImageArithmetic(_ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]], ImageFormula[(int)eAlignIndex]);
            SetLimitROI(eAlignIndex, _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1], srcImage1, _CornerROILimit[(int)eAlignIndex][(int)ECorner.Corner1], _SearchHalfWidth[(int)eAlignIndex], _SearchHalfHeight[(int)eAlignIndex]);

            Image<Gray, byte> srcImage2 = ImageArithmetic(_ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]], ImageFormula[(int)eAlignIndex]);
            SetLimitROI(eAlignIndex, _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2], srcImage2, _CornerROILimit[(int)eAlignIndex][(int)ECorner.Corner2], _SearchHalfWidth[(int)eAlignIndex], _SearchHalfHeight[(int)eAlignIndex]);

            if (_AlignType[(int)eAlignIndex] == EAlignType.Match)
            {
                Match[(int)eAlignIndex][(int)ECorner.Corner1].SetMatchCenterPos(_OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1]);
                Match[(int)eAlignIndex][(int)ECorner.Corner2].SetMatchCenterPos(_OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2]);

                return AlignByMatch(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    Match[(int)eAlignIndex][(int)ECorner.Corner1],
                    Match[(int)eAlignIndex][(int)ECorner.Corner2],
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }
            else if (_AlignType[(int)eAlignIndex] == EAlignType.FindCorner)
            {
                return AlignByCorner(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }
            else if (_AlignType[(int)eAlignIndex] == EAlignType.BlobByPoint)
            {
                return AlignByBlobAndPoint(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }
            else if (_AlignType[(int)eAlignIndex] == EAlignType.FindCornerByTriangle)
            {
                return AlignByCornerTriangle(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }
            else if (_AlignType[(int)eAlignIndex] == EAlignType.MatchBox)
            {
                return AlignByMatchBox(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    Match[(int)eAlignIndex][(int)ECorner.Corner1],
                    Match[(int)eAlignIndex][(int)ECorner.Corner2],
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }

            else if (_AlignType[(int)eAlignIndex] == EAlignType.Yolo)
            {
                return AlignByYolo(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner1]],
                    _ImgDisplay[(int)_Camera[(int)eAlignIndex][(int)ECorner.Corner2]],
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }
            else if (_AlignType[(int)eAlignIndex] == EAlignType.ObjectCenterByPoint)
            {
                return AlignByObjectCenterAndPoint(
                    eAlignIndex,
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner1],
                    _Camera[(int)eAlignIndex][(int)ECorner.Corner2],
                    srcImage1,
                    srcImage2,
                    _AlignTolerance[(int)eAlignIndex],
                    ref shiftX,
                    ref shiftY,
                    ref shiftAngle);
            }

            return false;
        }

        public bool AlignByMatch(
            EAlignIndex eAlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            Image<Gray, byte> srcImage1,
            Image<Gray, byte> srcImage2,
            MatchDef MATCH1,
            MatchDef MATCH2,
            double SizeTolerance,
            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();

            if (!MatchSuccess(eAlignIndex, srcImage1, srcImage2, MATCH1, MATCH2, (double)0, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                    !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                if (!MatchSuccess(eAlignIndex, srcImage1, srcImage2, MATCH1, MATCH2, (double)2.5, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                    !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
                {
                    if (!MatchSuccess(eAlignIndex, srcImage1, srcImage2, MATCH1, MATCH2, (double)-2.5, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                        !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
                    {
                        SaveImg.SaveImg(EImgDirPath.MatchNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                        SaveImg.SaveImg(EImgDirPath.MatchNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());
                        return false;
                    }
                }
            }
            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShift(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByCorner(
          EAlignIndex eAlignIndex,
          ECamera eCAMERA1,
          ECamera eCAMERA2,
          Image<Gray, byte> srcImage1,
          Image<Gray, byte> srcImage2,
          double SizeTolerance,

          ref double shiftX,
          ref double shiftY,
          ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();

            if (!FindCorner(eAlignIndex, srcImage1, srcImage2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());
                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShift(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByCornerTriangle(
            EAlignIndex eAlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            Image<Gray, byte> srcImage1,
            Image<Gray, byte> srcImage2,
            double SizeTolerance,
            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();

            if (!FindCornerByTriangle(eAlignIndex, srcImage1, srcImage2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());

                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShiftByPoint(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByBlobAndPoint(
            EAlignIndex eAlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            Image<Gray, byte> srcImage1,
            Image<Gray, byte> srcImage2,
            double SizeTolerance,

            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();

            if (!FindCircle(eAlignIndex, srcImage1, srcImage2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());

                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShiftByPoint(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByObjectCenterAndPoint(
            EAlignIndex eAlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            Image<Gray, byte> srcImage1,
            Image<Gray, byte> srcImage2,
            double SizeTolerance,

            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();

            if (!FindObjectCeneter(eAlignIndex, srcImage1, srcImage2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());

                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShiftByPoint(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByYolo(
           EAlignIndex eAlignIndex,
           ECamera eCAMERA1,
           ECamera eCAMERA2,
           Image<Bgr, byte> srcImage1,
           Image<Bgr, byte> srcImage2,
           double SizeTolerance,

           ref double shiftX,
           ref double shiftY,
           ref double shiftAngle)
        {
            PointF sP1 = new PointF();
            PointF sP2 = new PointF();

            if (!FindConerByYolo(eAlignIndex, srcImage1, srcImage2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, SizeTolerance))
            {
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.BlobAndPointNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());
                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShiftByPoint(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        public bool AlignByMatchBox(
            EAlignIndex eAlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            Image<Gray, byte> srcImage1,
            Image<Gray, byte> srcImage2,
            MatchDef match1,
            MatchDef match2,
            double alignTolerence,
            ref double shiftX,
            ref double shiftY,
            ref double shiftAngle)
        {
            PointF sP1 = new Point();
            PointF sP2 = new Point();
            PointF rP1 = new Point();
            PointF rP2 = new Point();


            Image<Gray, byte> resize_srcImage1 = new Image<Gray, byte>(srcImage1.Width, srcImage1.Height);
            Image<Gray, byte> resize_srcImage2 = new Image<Gray, byte>(srcImage1.Width, srcImage1.Height);
            float ratio1 = 0;
            float ratio2 = 0;
            for (float ratio = 0; ratio <= 1; ratio += 0.01F)
            {
                if (srcImage1.Width * ratio > match1.GetWidth() * 5 && srcImage1.Height * ratio > match1.GetHeight() * 5)
                {
                    resize_srcImage1 = srcImage1.Resize(ratio, Inter.Nearest);
                    ratio1 = ratio;
                    break;
                }
            }
            for (float ratio = 0; ratio <= 1; ratio += 0.01F)
            {
                if (srcImage2.Width * ratio > match2.GetWidth() * 5 && srcImage2.Height * ratio > match2.GetHeight() * 5)
                {
                    resize_srcImage2 = srcImage2.Resize(ratio, Inter.Nearest);
                    ratio2 = ratio;
                    break;
                }
            }

            if (!MatchSuccess(eAlignIndex, resize_srcImage1, resize_srcImage2, match1, match2, (double)0, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2))
            {
                if (!MatchSuccess(eAlignIndex, resize_srcImage1, resize_srcImage2, match1, match2, (double)15, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2))
                {
                    if (!MatchSuccess(eAlignIndex, resize_srcImage1, resize_srcImage2, match1, match2, (double)-15, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2))
                    {
                        SaveImg.SaveImg(EImgDirPath.MatchNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                        SaveImg.SaveImg(EImgDirPath.MatchNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());
                        return false;
                    }
                }
            }

            PointF P1 = ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1];
            PointF P2 = ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2];

            P1.X = P1.X / ratio1 + srcImage1.ROI.X;
            P1.Y = P1.Y / ratio1 + srcImage1.ROI.Y;
            P2.X = P2.X / ratio2 + srcImage2.ROI.X;
            P2.Y = P2.Y / ratio2 + srcImage2.ROI.Y;

            var fcbt_image1 = srcImage1.Clone();
            int x = (int)P1.X - _MatchBoxSearchHalfW[(int)eAlignIndex][(int)ECorner.Corner1];
            int y = (int)P1.Y - _MatchBoxSearchHalfH[(int)eAlignIndex][(int)ECorner.Corner1];
            int w = _MatchBoxSearchHalfW[(int)eAlignIndex][(int)ECorner.Corner1] * 2;
            int h = _MatchBoxSearchHalfH[(int)eAlignIndex][(int)ECorner.Corner1] * 2;
            fcbt_image1.ROI = new Rectangle(x, y, w, h);


            var fcbt_image2 = srcImage2.Clone();
            x = (int)P2.X - _MatchBoxSearchHalfW[(int)eAlignIndex][(int)ECorner.Corner2];
            y = (int)P2.Y - _MatchBoxSearchHalfH[(int)eAlignIndex][(int)ECorner.Corner2];
            w = _MatchBoxSearchHalfW[(int)eAlignIndex][(int)ECorner.Corner2] * 2;
            h = _MatchBoxSearchHalfH[(int)eAlignIndex][(int)ECorner.Corner2] * 2;
            fcbt_image2.ROI = new Rectangle(x, y, w, h);

          

            if (!FindCornerByTriangle(eAlignIndex, fcbt_image1, fcbt_image2, ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1], ref ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2], ref sP1, ref sP2) ||
                !AlignPositionInRange(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, alignTolerence))
            {
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA1], 1000, eCAMERA1.ToString() + "_" + eAlignIndex.ToString());
                SaveImg.SaveImg(EImgDirPath.CornerNG, _ImgDisplay[(int)eCAMERA2], 1000, eCAMERA2.ToString() + "_" + eAlignIndex.ToString());
                return false;
            }

            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, sP1, sP2, ref sX1, ref sY1, ref sX2, ref sY2);

            _AlignAlgorithm[(int)eAlignIndex].vGetShift(
               sX1,
               sY1,
               sX2,
               sY2,
               ref shiftX,
               ref shiftY,
               ref shiftAngle);

            return true;
        }

        private bool FindConerByYolo(
            EAlignIndex eAlignIndex,
            Image<Bgr, byte> srcImage1,
            Image<Bgr, byte> srcImage2,
            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            Point P1 = new Point();
            Point P2 = new Point();

            List<YoloPrediction> predictions1 = _CornerModel[(int)eAlignIndex][(int)ECorner.Corner1].Predict(srcImage1.AsBitmap());
            YoloPrediction prediction1 = new YoloPrediction();
            foreach (var item in predictions1)
            {
                double score = Math.Round(prediction1.Score, 2);
                if (item.Score > prediction1.Score)
                    prediction1 = item;
            }
            P1.X = srcImage1.ROI.X + (int)(prediction1.Rectangle.Left + 10 + 0.5);
            P1.Y = srcImage1.ROI.Y + (int)(prediction1.Rectangle.Bottom - 10 + 0.5);


            List<YoloPrediction> predictions2 = _CornerModel[(int)eAlignIndex][(int)ECorner.Corner2].Predict(srcImage2.AsBitmap());
            YoloPrediction prediction2 = new YoloPrediction();
            foreach (var item in predictions2)
            {
                double score = Math.Round(prediction2.Score, 2);
                if (item.Score > prediction2.Score)
                    prediction2 = item;
            }
            // if (predictions.Count < 1 || prediction.Label.Id != 2) //not LCorner
            //     return false;

            P2.X = srcImage2.ROI.X + (int)(prediction2.Rectangle.Right - 10 + 0.5);
            P2.Y = srcImage2.ROI.Y + (int)(prediction2.Rectangle.Bottom - 10 + 0.5);

            resultPoint1 = P1;
            resultPoint2 = P2;

            shiftPoint1 = new PointF(P1.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].X, P1.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].Y);
            shiftPoint2 = new PointF(P2.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].X, P2.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].Y);

            return true;
        }

        private bool FindCorner(
            EAlignIndex eAlignIndex,
            Image<Gray, byte> srcImage,
            Image<Gray, byte> srcImage2,
            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            Point P1 = new Point();
            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                P1 = CornerDetector[(int)eAlignIndex].GetCorner(srcImage.Mat,
                    _CornerDirection[(int)eAlignIndex][(int)ECorner.Corner1],
                    _CornerHoughLinesThrehold[(int)eAlignIndex][(int)ECorner.Corner1],
                    _CornerHoughLinesMinLen[(int)eAlignIndex][(int)ECorner.Corner1],
                    _CornerHoughMinLineGap[(int)eAlignIndex][(int)ECorner.Corner1],
                    _CornerXv[(int)eAlignIndex][(int)ECorner.Corner1],
                    _CornerYv[(int)eAlignIndex][(int)ECorner.Corner1]);
            P1.X += srcImage.ROI.X;
            P1.Y += srcImage.ROI.Y;

            Point P2 = new Point();
            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                P2 = CornerDetector[(int)eAlignIndex].GetCorner(srcImage2.Mat,
                     _CornerDirection[(int)eAlignIndex][(int)ECorner.Corner2],
                    _CornerHoughLinesThrehold[(int)eAlignIndex][(int)ECorner.Corner2],
                    _CornerHoughLinesMinLen[(int)eAlignIndex][(int)ECorner.Corner2],
                    _CornerHoughMinLineGap[(int)eAlignIndex][(int)ECorner.Corner2],
                    _CornerXv[(int)eAlignIndex][(int)ECorner.Corner2],
                    _CornerYv[(int)eAlignIndex][(int)ECorner.Corner2]);
            P2.X += srcImage2.ROI.X;
            P2.Y += srcImage2.ROI.Y;

            resultPoint1 = P1;
            resultPoint2 = P2;

            shiftPoint1 = new PointF(P1.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].X, P1.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].Y);
            shiftPoint2 = new PointF(P2.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].X, P2.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].Y);

            return true;
        }


        private bool FindCircle(
            EAlignIndex eAlignIndex,
            Image<Gray, byte> srcImage,
            Image<Gray, byte> srcImage2,

            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            PointF[] P1 = new PointF[0];
            PointF[] P2 = new PointF[0];
            float[] diameter1 = new float[0];
            float[] diameter2 = new float[0];

            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                BlobSearch(srcImage, _BlobThreshold[(int)eAlignIndex][(int)ECorner.Corner1], _BlobMinArea[(int)eAlignIndex][(int)ECorner.Corner1], _BlobMaxArea[(int)eAlignIndex][(int)ECorner.Corner1], (float)_BlobMinCircularity[(int)eAlignIndex][(int)ECorner.Corner1], out P1, out diameter1);

            if (srcImage2 != null && srcImage2.Width > 0 && srcImage2.Height > 0)
                BlobSearch(srcImage2, _BlobThreshold[(int)eAlignIndex][(int)ECorner.Corner2], _BlobMinArea[(int)eAlignIndex][(int)ECorner.Corner2], _BlobMaxArea[(int)eAlignIndex][(int)ECorner.Corner2], (float)_BlobMinCircularity[(int)eAlignIndex][(int)ECorner.Corner2], out P2, out diameter2);

            if (P1.Length < 1 || P2.Length < 1)
            {
                ErrorCode[(int)eAlignIndex] = EVisionErrorCode.CircleNotFound;
                return false;
            }

            int maxIndex1 = Array.IndexOf(diameter1, diameter1.Max());
            int maxIndex2 = Array.IndexOf(diameter2, diameter2.Max());

            if (diameter1[maxIndex1] < 1 || diameter2[maxIndex2] < 1)
            {
                ErrorCode[(int)eAlignIndex] = EVisionErrorCode.CircleNotFound;
                return false;
            }
            P1[maxIndex1].X += srcImage.ROI.X;
            P1[maxIndex1].Y += srcImage.ROI.Y;

            P2[maxIndex2].X += srcImage2.ROI.X;
            P2[maxIndex2].Y += srcImage2.ROI.Y;

            resultPoint1 = P1[maxIndex1];
            resultPoint2 = P2[maxIndex2];

            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                srcImage.ROI = Rectangle.Empty;

            if (srcImage2 != null && srcImage2.Width > 0 && srcImage2.Height > 0)
                srcImage2.ROI = Rectangle.Empty;


            shiftPoint1 = new PointF(P1[maxIndex1].X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].X, P1[maxIndex1].Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].Y);
            shiftPoint2 = new PointF(P2[maxIndex2].X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].X, P2[maxIndex2].Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].Y);

            return true;
        }


        private bool FindObjectCeneter(
            EAlignIndex eAlignIndex,
            Image<Gray, byte> srcImage,
            Image<Gray, byte> srcImage2,

            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            PointF[] P1 = new PointF[0];
            PointF[] P2 = new PointF[0];
            
            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                P1 = GetObjectCenter(srcImage, _BlobThreshold[(int)eAlignIndex][(int)ECorner.Corner1], _BlobMinArea[(int)eAlignIndex][(int)ECorner.Corner1], _BlobMaxArea[(int)eAlignIndex][(int)ECorner.Corner1], true);

            if (srcImage2 != null && srcImage2.Width > 0 && srcImage2.Height > 0)
                P2 = GetObjectCenter(srcImage2, _BlobThreshold[(int)eAlignIndex][(int)ECorner.Corner2], _BlobMinArea[(int)eAlignIndex][(int)ECorner.Corner2], _BlobMaxArea[(int)eAlignIndex][(int)ECorner.Corner2], true);


            if (P1.Length < 1 || P2.Length < 1)
            {
                ErrorCode[(int)eAlignIndex] = EVisionErrorCode.CircleNotFound;
                return false;
            }
            P1[P1.Length - 1].X += srcImage.ROI.X;
            P1[P1.Length - 1].Y += srcImage.ROI.Y;

            P2[P2.Length - 1].X += srcImage2.ROI.X;
            P2[P2.Length - 1].Y += srcImage2.ROI.Y;

            resultPoint1 = P1[P1.Length - 1];
            resultPoint2 = P2[P2.Length - 1];

            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                srcImage.ROI = Rectangle.Empty;

            if (srcImage2 != null && srcImage2.Width > 0 && srcImage2.Height > 0)
                srcImage2.ROI = Rectangle.Empty;


            shiftPoint1 = new PointF(P1[P1.Length - 1].X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].X, P1[P1.Length - 1].Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].Y);
            shiftPoint2 = new PointF(P2[P2.Length - 1].X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].X, P2[P2.Length - 1].Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].Y);

            return true;
        }
        private void SetLimitROI(EAlignIndex eAlignIndex, Point OrgImagCorner, Image<Gray, byte> srcImage, Rectangle CornerLimit, int SearchHalfWidth, int SearchHalfHeight)
        {
            if (SearchHalfWidth > 0 && SearchHalfHeight > 0)
            {

                // corner1 
                int OrgX = OrgImagCorner.X - SearchHalfWidth;
                int OrgY = OrgImagCorner.Y - SearchHalfHeight;

                int W = SearchHalfWidth * 2;
                int H = SearchHalfHeight * 2;
                //CornerLimit = Rectangle.Empty;
                if (CornerLimit != Rectangle.Empty)
                {
                    if (OrgX < CornerLimit.X)
                        OrgX = CornerLimit.X;
                    if (OrgY < CornerLimit.Y)
                        OrgY = CornerLimit.Y;
                    if (OrgX + W > CornerLimit.Width + CornerLimit.X)
                        W = CornerLimit.Width - OrgX + CornerLimit.X;
                    if (OrgY + H > CornerLimit.Height + CornerLimit.Y)
                        H = CornerLimit.Height - OrgY + CornerLimit.Y;
                }
                else
                {
                    if (OrgX < 0)
                        OrgX = 0;
                    if (OrgY < 0)
                        OrgY = 0;
                    if (OrgX + W > srcImage.Width)
                        W = srcImage.Width - OrgX;
                    if (OrgY + H > srcImage.Height)
                        H = srcImage.Height - OrgY;
                }

                if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
                    srcImage.ROI = new Rectangle(OrgX, OrgY, W, H);
            }
        }

        public Rectangle GetPanelRectangleOnImage(EAlignIndex AlignIndex, ECorner Corner)
        {
            int index = (int)AlignIndex;
            int cor = (int)Corner;
            RectangleF CCDRectangle = GetCartesianRetangle(_Camera[index][cor], _CCDCenterPoint[index][cor]);

            RectangleF Panel = new RectangleF(_PanelCenterPoint[index].X - (float)_PanelXLength[index] / 2, _PanelCenterPoint[index].Y - (float)_PanelYLength[index] / 2, (float)_PanelXLength[index], (float)_PanelYLength[index]);

            if (CCDRectangle.IntersectsWith(Panel))
            {
                RectangleF ccd1Intersect = RectangleF.Intersect(CCDRectangle, Panel);
                return GetImageRetangle(_Camera[index][cor], ccd1Intersect, _CCDCenterPoint[index][cor]);
            }

            return Rectangle.Empty;
        }

        private Point GetImagePoint(
                   ECamera eCAMERA,
                   PointF AlignPoint,
                   PointF CenterPoint)
        {
            int orgX = (int)Math.Round((AlignPoint.X - CenterPoint.X) / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);
            int orgY = (int)Math.Round((AlignPoint.Y - CenterPoint.Y) / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);

            if (CameraCollection.GetXYSwap(eCAMERA))
            {
                int temp = orgX;
                orgX = orgY;
                orgY = temp;
            }

            if (CameraCollection.GetXInverse(eCAMERA))
            {
                orgX = -(orgX);
            }

            if (CameraCollection.GetYInverse(eCAMERA))
            {
                orgY = -(orgY);
            }
            orgX += CameraCollection.GetWidth(eCAMERA) / 2;
            orgY += CameraCollection.GetHeight(eCAMERA) / 2;

            return new Point(orgX, orgY);
        }

        private Rectangle GetImageRetangle(
           ECamera eCAMERA,
           RectangleF Rectangle,
           PointF CenterPoint)
        {
            int w = (int)Math.Round(Rectangle.Width / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);
            int h = (int)Math.Round(Rectangle.Height / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);

            int orgX = (int)Math.Round((Rectangle.X - CenterPoint.X) / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);
            int orgY = (int)Math.Round((Rectangle.Y - CenterPoint.Y) / CameraCollection.GetResolution(eCAMERA), 0, MidpointRounding.AwayFromZero);

            if (CameraCollection.GetXYSwap(eCAMERA))
            {
                int temp = w;
                w = h;
                h = temp;

                temp = orgX;
                orgX = orgY;
                orgY = temp;
            }

            if (CameraCollection.GetXInverse(eCAMERA))
            {
                orgX = -(orgX + w);
            }

            if (CameraCollection.GetYInverse(eCAMERA))
            {
                orgY = -(orgY + h);
            }
            orgX += CameraCollection.GetWidth(eCAMERA) / 2;
            orgY += CameraCollection.GetHeight(eCAMERA) / 2;

            return new Rectangle(orgX, orgY, w, h);
        }


        public PointF GetCartesianPointByImageCenter(
            ECamera eCAMERA,
            Point ImagePos)
        {
            ImagePos.X -= CameraCollection.GetWidth(eCAMERA) / 2;
            ImagePos.Y -= CameraCollection.GetHeight(eCAMERA) / 2;

            if (CameraCollection.GetXInverse(eCAMERA))
                ImagePos.X *= -1;

            if (CameraCollection.GetYInverse(eCAMERA))
                ImagePos.Y *= -1;

            PointF Pos = new PointF();
            Pos.X = ImagePos.X;
            Pos.Y = ImagePos.Y;

            if (CameraCollection.GetXYSwap(eCAMERA))
            {
                Pos.X = ImagePos.Y;
                Pos.Y = ImagePos.X;
            }

            Pos.X = (float)(CameraCollection.GetResolution(eCAMERA) * Pos.X);
            Pos.Y = (float)(CameraCollection.GetResolution(eCAMERA) * Pos.Y);

            return Pos;
        }

        private PointF GetCartesianPoint(
            ECamera eCAMERA,
            PointF CCDCenterPoint,
            Point ImagePos)
        {
            ImagePos.X -= CameraCollection.GetWidth(eCAMERA) / 2;
            ImagePos.Y -= CameraCollection.GetHeight(eCAMERA) / 2;

            if (CameraCollection.GetXInverse(eCAMERA))
                ImagePos.X *= -1;

            if (CameraCollection.GetYInverse(eCAMERA))
                ImagePos.Y *= -1;

            PointF Pos = new PointF();
            Pos.X = ImagePos.X;
            Pos.Y = ImagePos.Y;

            if (CameraCollection.GetXYSwap(eCAMERA))
            {
                Pos.X = ImagePos.Y;
                Pos.Y = ImagePos.X;
            }

            Pos.X = (float)(CameraCollection.GetResolution(eCAMERA) * Pos.X) + (float)CCDCenterPoint.X;
            Pos.Y = (float)(CameraCollection.GetResolution(eCAMERA) * Pos.Y) + (float)CCDCenterPoint.Y;

            return Pos;
        }

        private RectangleF GetCartesianRetangle(
            ECamera eCAMERA,
            PointF CenterPoint)
        {
            float w = (float)CameraCollection.GetResolution(eCAMERA) * CameraCollection.GetWidth(eCAMERA);
            float h = (float)CameraCollection.GetResolution(eCAMERA) * CameraCollection.GetHeight(eCAMERA);

            if (CameraCollection.GetXYSwap(eCAMERA))
            {
                float temp = w;
                w = h;
                h = temp;
            }
            //float tempw = w;
            //if(CameraCollection.GetXInverse(eCAMERA))
            //{
            //    tempw = -w;
            //}

            //float temph = h;
            //if (CameraCollection.GetYInverse(eCAMERA))
            //{
            //    temph = -h;
            //}

            return new RectangleF((float)CenterPoint.X - w / 2, (float)CenterPoint.Y - h / 2, w, h);
        }

        private void GetCartesianCoordinates(
            EAlignIndex AlignIndex,
            ECamera eCAMERA1,
            ECamera eCAMERA2,
            PointF ShiftP1,
            PointF ShiftP2,
            ref double ShiftX1,
            ref double ShiftY1,
            ref double ShiftX2,
            ref double ShiftY2)
        {
            if (CameraCollection.GetXInverse(eCAMERA1))
            {
                ShiftP1.X *= -1;
            }
            if (CameraCollection.GetYInverse(eCAMERA1))
            {
                ShiftP1.Y *= -1;
            }

            if (CameraCollection.GetXInverse(eCAMERA2))
            {
                ShiftP2.X *= -1;
            }
            if (CameraCollection.GetYInverse(eCAMERA2))
            {
                ShiftP2.Y *= -1;
            }

            ShiftX1 = ShiftP1.X;
            ShiftY1 = ShiftP1.Y;

            ShiftX2 = ShiftP2.X;
            ShiftY2 = ShiftP2.Y;

            if (CameraCollection.GetXYSwap(eCAMERA1))
            {
                ShiftX1 = ShiftP1.Y;
                ShiftY1 = ShiftP1.X;
            }

            if (CameraCollection.GetXYSwap(eCAMERA2))
            {
                ShiftX2 = ShiftP2.Y;
                ShiftY2 = ShiftP2.X;
            }

            ShiftX1 *= CameraCollection.GetResolution(eCAMERA1);
            ShiftY1 *= CameraCollection.GetResolution(eCAMERA1);
            ShiftX2 *= CameraCollection.GetResolution(eCAMERA2);
            ShiftY2 *= CameraCollection.GetResolution(eCAMERA2);

            double theta1 = _CCDAngle[(int)AlignIndex][(int)ECorner.Corner1] * Math.PI / 180.0;
            double x1 = Math.Cos(theta1) * ShiftX1 + Math.Sin(theta1) * ShiftY1;
            double y1 = Math.Cos(theta1) * ShiftY1 + Math.Sin(theta1) * ShiftX1;

            double theta2 = _CCDAngle[(int)AlignIndex][(int)ECorner.Corner2] * Math.PI / 180.0;
            double x2 = Math.Cos(theta2) * ShiftX2 + Math.Sin(theta2) * ShiftY2;
            double y2 = Math.Cos(theta2) * ShiftY2 + Math.Sin(theta2) * ShiftX2;

            ShiftX1 = x1;
            ShiftY1 = y1;
            ShiftX2 = x2;
            ShiftY2 = y2;
        }

        private bool AlignPositionInRange(EAlignIndex eAlignIndex, ECamera eCAMERA1, ECamera eCAMERA2, PointF ShiftPoint1, PointF ShiftPoint2, double Tolerance)
        {
            double sX1 = 0;
            double sX2 = 0;
            double sY1 = 0;
            double sY2 = 0;

            GetCartesianCoordinates(eAlignIndex, eCAMERA1, eCAMERA2, ShiftPoint1, ShiftPoint2, ref sX1, ref sY1, ref sX2, ref sY2);

            double a1X = 0;
            double a1Y = 0;
            double a2X = 0;
            double a2Y = 0;

            _AlignAlgorithm[(int)eAlignIndex].vGetAlignPos(ref a1X, ref a1Y, ref a2X, ref a2Y);
            double fDistance = Math.Pow(Math.Pow(a1X - a2X, 2) + Math.Pow(a1Y - a2Y, 2), 0.5);

            double dX = a1X + sX1 - a2X - sX2;
            double dY = a1Y + sY1 - a2Y - sY2;
            if (Math.Abs(fDistance - Math.Pow(Math.Pow(dX, 2) + Math.Pow(dY, 2), 0.5)) > Tolerance)
            {
                ErrorCode[(int)eAlignIndex] = EVisionErrorCode.SizeError;
                return false;
            }

            return true;
        }

        private bool MatchSuccess(
            EAlignIndex eAlignIndex,
            Image<Gray, byte> srcImage,
            Image<Gray, byte> srcImage2,
            MatchDef MATCH1,
            MatchDef MATCH2,
            double angle,
            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            MATCH1.MatchByAngle(srcImage, angle);
            MATCH2.MatchByAngle(srcImage2, angle);

            if (!MATCH1.Success() || !MATCH2.Success())
            {
                resultPoint1.X = 0;
                resultPoint1.Y = 0;

                resultPoint2.X = 0;
                resultPoint2.Y = 0;

                ErrorCode[(int)eAlignIndex] = EVisionErrorCode.MatchFail;
                return false;
            }

            resultPoint1 = MATCH1.GetResultCenterPoint();
            resultPoint2 = MATCH2.GetResultCenterPoint();

            resultPoint1.X += srcImage.ROI.X;
            resultPoint1.Y += srcImage.ROI.Y;

            resultPoint2.X += srcImage2.ROI.X;
            resultPoint2.Y += srcImage2.ROI.Y;

            shiftPoint1 = new PointF(resultPoint1.X - MATCH1.GetLearnCenterPoint().X, resultPoint1.Y - MATCH1.GetLearnCenterPoint().Y);
            shiftPoint2 = new PointF(resultPoint2.X - MATCH2.GetLearnCenterPoint().X, resultPoint2.Y - MATCH2.GetLearnCenterPoint().Y);
            return true;
        }

        private bool FindCornerByTriangle(
            EAlignIndex eAlignIndex,
            Image<Gray, byte> srcImage,
            Image<Gray, byte> srcImage2,
            ref PointF resultPoint1,
            ref PointF resultPoint2,
            ref PointF shiftPoint1,
            ref PointF shiftPoint2)
        {
            PointF P1 = new PointF();
            if (srcImage != null && srcImage.Width > 0 && srcImage.Height > 0)
            {
                if (_FCBTUseBlur[(int)eAlignIndex][(int)ECorner.Corner1] > 0)
                {
                    CvInvoke.GaussianBlur(srcImage, srcImage, new Size(3, 3), 0);
                }
                CvInvoke.Threshold(srcImage, srcImage, _FCBTThreshold[(int)eAlignIndex][(int)ECorner.Corner1], 255, ThresholdType.BinaryInv);
                GetRotatedCornerByTriangle(srcImage.Mat, _FCBTCornerSide[(int)eAlignIndex][(int)ECorner.Corner1], _FCBTLineLengthThreshold[(int)eAlignIndex][(int)ECorner.Corner1], _FCBTCornerMaskSize[(int)eAlignIndex][(int)ECorner.Corner1], out P1, out double deg, out LineSegment2D[] line);
                for (int i = 0; i < line.Length; i++)
                {
                    line[i].P1 = new Point(line[i].P1.X + srcImage.ROI.X, line[i].P1.Y + srcImage.ROI.Y);
                    line[i].P2 = new Point(line[i].P2.X + srcImage.ROI.X, line[i].P2.Y + srcImage.ROI.Y);
                }
                P1.X += srcImage.ROI.X;
                P1.Y += srcImage.ROI.Y;
                ResultCorner[(int)eAlignIndex][(int)ECorner.Corner1] = P1;
                ResultCornerDeg[(int)eAlignIndex][(int)ECorner.Corner1] = deg;
                ResultCornerLines[(int)eAlignIndex][(int)ECorner.Corner1] = line;
            }



            PointF P2 = new PointF();
            if (srcImage2 != null && srcImage2.Width > 0 && srcImage2.Height > 0)
            {
                if (_FCBTUseBlur[(int)eAlignIndex][(int)ECorner.Corner2] > 0)
                {
                    CvInvoke.GaussianBlur(srcImage2, srcImage2, new Size(3, 3), 0);
                }
                CvInvoke.Threshold(srcImage2, srcImage2, _FCBTThreshold[(int)eAlignIndex][(int)ECorner.Corner2], 255, ThresholdType.BinaryInv);
                GetRotatedCornerByTriangle(srcImage2.Mat, _FCBTCornerSide[(int)eAlignIndex][(int)ECorner.Corner2], _FCBTLineLengthThreshold[(int)eAlignIndex][(int)ECorner.Corner2], _FCBTCornerMaskSize[(int)eAlignIndex][(int)ECorner.Corner2], out P2, out double deg, out LineSegment2D[] line);
                for (int i = 0; i < line.Length; i++)
                {
                    line[i].P1 = new Point(line[i].P1.X + srcImage2.ROI.X, line[i].P1.Y + srcImage2.ROI.Y);
                    line[i].P2 = new Point(line[i].P2.X + srcImage2.ROI.X, line[i].P2.Y + srcImage2.ROI.Y);
                }
                P2.X += srcImage2.ROI.X;
                P2.Y += srcImage2.ROI.Y;
                ResultCorner[(int)eAlignIndex][(int)ECorner.Corner2] = P2;
                ResultCornerDeg[(int)eAlignIndex][(int)ECorner.Corner2] = deg;
                ResultCornerLines[(int)eAlignIndex][(int)ECorner.Corner2] = line;
            }


            resultPoint1 = P1;
            resultPoint2 = P2;

            shiftPoint1 = new PointF(P1.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].X, P1.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner1].Y);
            shiftPoint2 = new PointF(P2.X - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].X, P2.Y - _OrgImagCorner[(int)eAlignIndex][(int)ECorner.Corner2].Y);

            return true;
        }



        public void LearnMatchModel(EAlignIndex AlignIndex, ECorner Corner, Image<Bgr, byte> SrcImage, float Score)
        {
            Rectangle roi = SrcImage.ROI;
            SrcImage.ROI = Rectangle.Empty;
            Image<Gray, byte> result = ImageArithmetic(SrcImage, ImageFormula[(int)AlignIndex]);
            result.ROI = roi;

            Match[(int)AlignIndex][(int)Corner].SetMatchModel(result, roi.Location, true);
            Match[(int)AlignIndex][(int)Corner].SetScore(Score);
            SaveMatchModel(AlignIndex, Corner);

            UpdateAlignParameter();
        }

        public void SaveMatchModel(EAlignIndex AlignIndex, ECorner Corner)
        {
            Match[(int)AlignIndex][(int)Corner].Save(GetMatchPath(AlignIndex, Corner));
        }

        public void SetPanelSize(EAlignIndex alignIndex, double xSize, double ySize)
        {
            _PanelXLength[(int)alignIndex] = xSize;
            _PanelYLength[(int)alignIndex] = ySize;

            IniFile ini = new IniFile(_SystemDirPath + "\\Vision.ini", false);
            ini.WriteDouble("EAlignIndex" + ((int)alignIndex).ToString(), "PanelXLength", _PanelXLength[(int)alignIndex]);
            ini.WriteDouble("EAlignIndex" + ((int)alignIndex).ToString(), "PanelYLength", _PanelYLength[(int)alignIndex]);
            ini.FileClose();
            ini.Dispose();
            UpdateAlignByPanelSize(alignIndex);
        }

        public void GetPanelSize(EAlignIndex alignIndex, out double xSize, out double ySize)
        {
            xSize = _PanelXLength[(int)alignIndex];
            ySize = _PanelYLength[(int)alignIndex];
            UpdateAlignByPanelSize(alignIndex);
        }

        public void Dispose()
        {
            for (int i = 0; i < (int)EAlignIndex.Count; i++)
            {
                _Task[i].Abort();
            }
            if (_AlignAlgorithm != null)
                _AlignAlgorithm = null;


            if (CameraCollection != null)
                CameraCollection.Dispose();

            for (int i = 0; i < (int)EAlignIndex.Count; i++)
            {
                if (CornerDetector[i] != null)
                    CornerDetector[i].Dispose();

                for (int j = 0; j < (int)ECorner.Count; j++)
                    if (Match[i][j] != null)
                        Match[i][j].Dispose();
            }
        }
    }
}



public class CornerModel : YoloModel
{
    public override int Width { get; set; } = 640;
    public override int Height { get; set; } = 640;
    public override int Depth { get; set; } = 3;

    public override int Dimensions { get; set; } = 7; //Number of class + 5

    public override int[] Strides { get; set; } = new int[] { 8, 16, 32 };

    public override int[][][] Anchors { get; set; } = new int[][][]
    {
            new int[][] { new int[] { 010, 13 }, new int[] { 016, 030 }, new int[] { 033, 023 } },
            new int[][] { new int[] { 030, 61 }, new int[] { 062, 045 }, new int[] { 059, 119 } },
            new int[][] { new int[] { 116, 90 }, new int[] { 156, 198 }, new int[] { 373, 326 } }
    };

    public override int[] Shapes { get; set; } = new int[] { 80, 40, 20 };

    public override float Confidence { get; set; } = 0.25f;
    public override float MulConfidence { get; set; } = 0.25f;
    public override float Overlap { get; set; } = 0.45f;

    public override string[] Outputs { get; set; } = new[] { "output0" };

    public override List<YoloLabel> Labels { get; set; } = new List<YoloLabel>()
        {
            new YoloLabel { Id = 1, Name = "RCorner" },
            new YoloLabel { Id = 2, Name = "LCorner" },
        };

    public override bool UseDetect { get; set; } = true;

    public CornerModel()
    {

    }
}