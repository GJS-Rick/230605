using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using nsMsgFm;
using VisionLibrary;
using FileStreamLibrary;
using CommonLibrary;
using System.Threading;
using System.Runtime.InteropServices;
using ZoomWindow;
using Emgu.CV.Stitching;

namespace nsUI
{
    public partial class FmSetManualAlignPosition : Form
    {
        public enum EDisplayStatus
        {
            Show,
            SetPositon1,
            SetPositon2,
            LearnMatchCorner1,
            LearnMatchCorner2,
            LearnMatchMask1,
            LearnMatchMask2,
            LearnCenter,
            CalculateCCDResolution,
            CalculateRobotRadius,
            CalculateCCDAngle,
        }
        public enum ScanMode
		{
            UpToDown,
            DownToUp,
            LeftToRight,
            RightToLeft
        }

        private Image<Bgr, byte>[] _ImgDisplay;
        private Image<Bgr, byte>[] _ImageEdge;

        private readonly VisionManagerDef _VisionManager;
        private readonly FileManagerDef _FileStreamManager;
        private readonly CommonManagerDef _CommonManager;

        private EDisplayStatus _DisplayStatus;
        private MatchPosition[] _NowPosition;
        private MatchPosition[] _TempPosition;

        private readonly object _DisplayLocker = new object();

        private Thread _DisplayThread;

        private Mat _LoadImage;
        private bool _IsLive;
        private bool _ThreadEnd;



        public FmSetManualAlignPosition(CommonManagerDef cCommonManager, VisionManagerDef cVisionManagerDef, FileManagerDef cFileStreamManager)
        {
            _VisionManager = cVisionManagerDef;
            _FileStreamManager = cFileStreamManager;
            _CommonManager = cCommonManager;
            InitializeComponent();

            SetClickEvnet();

            _ImgDisplay = new Image<Bgr, byte>[(int)ECAMERA.Count];
            for (int i = 0; i < (int)ECAMERA.Count; i++)
                _ImgDisplay[i] = new Image<Bgr, byte>(_VisionManager.CameraCollection.GetWidth((ECAMERA)i), _VisionManager.CameraCollection.GetHeight((ECAMERA)i));

			cbxCCDIndex.Items.Clear();
			for (int i = 0; i < (int)ECAMERA.Count; i++)
				cbxCCDIndex.Items.Add(((ECAMERA)i).ToString());

            _NowPosition = new MatchPosition[(int)ECAMERA.Count];
            for (int i = 0; i < (int)ECAMERA.Count; i++)
                _NowPosition[i] = new MatchPosition();

            _TempPosition = new MatchPosition[(int)ECAMERA.Count];
            for (int i = 0; i < (int)ECAMERA.Count; i++)
                _TempPosition[i] = new MatchPosition();

            ZoomWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            ZoomWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);

            BtnSetPosition.Enabled = false;
            BtnCancelChoose.Enabled = false;
            ZoomWindow.CanTrackPattern = true;
            ZoomWindow.CanShowInfo = true;
            ZoomWindow.CanDrawPanelRectangle = true;
            _IsLive = false;

        }
        private void FmSetPosition_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (_DisplayThread == null)
                {
                    _DisplayThread = new Thread(Display);
                    _DisplayThread.Start();
                }
                cbxCCDIndex.SelectedIndex = 0;
                _IsLive = true;
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                _IsLive = false;
            }
        }

        private void Display()
        {
            lock (_DisplayLocker)
            {
                while (!_ThreadEnd)
                {
                    if (!this.Visible || !_IsLive)
                    {
                        if (_LoadImage != null)
                        {
                            ZoomWindow.SetImage(_LoadImage);
                            Thread.Sleep(10);
                            continue;
                        }
                        Thread.Sleep(200);
                        continue;
                    }
                    int cameraIndex = -1;
                    Invoke((MethodInvoker)delegate () { cameraIndex = cbxCCDIndex.SelectedIndex; });
                    _VisionManager.CameraCollection.CopyImage((ECAMERA)cameraIndex, _ImgDisplay[cameraIndex]);
                    if (_ImgDisplay[cameraIndex].Height==0|| _ImgDisplay[cameraIndex].Width == 0)
                    {
                        if (cameraIndex == (int)ECAMERA.Camera1&& _VisionManager.GetImage1()!=null)
                        {
                            _ImgDisplay[cameraIndex] = _VisionManager.GetImage1();
                        }
                        else if(cameraIndex == (int)ECAMERA.Camera2 && _VisionManager.GetImage2() != null)
                        {
                            _ImgDisplay[cameraIndex] = _VisionManager.GetImage2();
                        }

                    }
                    if (_ImgDisplay[cameraIndex] != null)
                    {
                        ZoomWindow.SetImage(_ImgDisplay[cameraIndex].Clone().Mat);
                        ZoomWindow.CommonMp = _NowPosition[cameraIndex];
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private Rectangle GetPanelPosition(int cameraIndex)
        {
            double fWidth = Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelWidth)) / _VisionManager.CameraCollection.GetResolution((ECAMERA)cameraIndex);

            double fPanelCeneterX = Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelCenterY)) / _VisionManager.CameraCollection.GetResolution((ECAMERA)cameraIndex);
            double fPanelBottomY = -Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelBottomX)) / _VisionManager.CameraCollection.GetResolution((ECAMERA)cameraIndex);

            double fPanelCornerDis = (Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelCenterY)) + Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelWidth)) / 2 - Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.SecondCCDStartPosX))) / _VisionManager.CameraCollection.GetResolution((ECAMERA)cameraIndex);
            double fPanelBottomY2 = -(Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.PanelBottomX)) - Convert.ToDouble(_FileStreamManager.MachineData.GetFactorValue(EMachineData.SecondCCDStartPosY))) / _VisionManager.CameraCollection.GetResolution((ECAMERA)cameraIndex);
            if (cameraIndex == 0)
                return new Rectangle((int)(fPanelCeneterX - fWidth/2), (int)fPanelBottomY, 5000, (int)(fPanelCeneterX + fWidth / 2));
            else
                return new Rectangle((int)0, (int)(fPanelBottomY2), (int)(fPanelCornerDis), (int)5000);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            PointF Result1 = new PointF(_NowPosition[(int)ECAMERA.Camera1].X, _NowPosition[(int)ECAMERA.Camera1].Y);
            PointF Result2 = new PointF(_NowPosition[(int)ECAMERA.Camera2].X, _NowPosition[(int)ECAMERA.Camera2].Y);
            _FileStreamManager.SeqenceHandshake.SetLiftsData(LiftsData.ManualResult1, Result1);
            _FileStreamManager.SeqenceHandshake.SetLiftsData(LiftsData.ManualResult2, Result2);
            _FileStreamManager.SeqenceHandshake.SetLiftsHandshakeFlag(ELiftsHandshakeFlag.ManualAlign, true);
            Visible = false;
        }

        new private void MouseUp(object sender, MouseEventArgs e)
        {
            if (_ImgDisplay[cbxCCDIndex.SelectedIndex].Width < 1 || _ImgDisplay[cbxCCDIndex.SelectedIndex].Height < 1)
                return;
            Point stOriginalImageLocation = ZoomWindow.ConverUiLocationToOriginalLocation(e.Location);
            if (_DisplayStatus == EDisplayStatus.SetPositon1)
            {
                _NowPosition[cbxCCDIndex.SelectedIndex] = new MatchPosition { X = stOriginalImageLocation.X, Y = stOriginalImageLocation.Y };
                BtnSetPosition.Enabled = true;
            }
            else if (_DisplayStatus == EDisplayStatus.SetPositon2)
            {
                _NowPosition[cbxCCDIndex.SelectedIndex] = new MatchPosition { X = stOriginalImageLocation.X, Y = stOriginalImageLocation.Y };
                BtnSetPosition.Enabled = true;
            }
        }

        new private void MouseMove(object sender, MouseEventArgs e)
        {
            if (_ImgDisplay[cbxCCDIndex.SelectedIndex].Width < 1 || _ImgDisplay[cbxCCDIndex.SelectedIndex].Height < 1)
                return;
            Point OriginalLocation = ZoomWindow.ConverUiLocationToOriginalLocation(e.Location);
            byte RGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 0];
            byte GGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 1];
            byte BGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 2];
            statusStripInfo.Items[0].Text =
                "X:" + OriginalLocation.X.ToString("0000") + "," +
                "Y:" + OriginalLocation.Y.ToString("0000") + ", " +
                "R:" + (RGray).ToString("000") + ", " +
                "G:" + (GGray).ToString("000") + ", " +
                "B:" + (BGray).ToString("000");
        }

        private void BtnAlignTest_Click(object sender, EventArgs e)
        {
            if (cbxCCDIndex.SelectedIndex < 0 || cbxCCDIndex.SelectedIndex >= (int)ECAMERA.Count)
                return; ;

            //DrawMatch();
            //DrawMatchEdge();

        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _VisionManager.CameraCollection.CopyImage((ECAMERA)cbxCCDIndex.SelectedIndex, _ImgDisplay[cbxCCDIndex.SelectedIndex]);
                _ImgDisplay[cbxCCDIndex.SelectedIndex].Save(saveFileDialog1.FileName);
            }
        }

        /*
        private void DrawMatchEdge()
        {
            if (cbxCCDIndex.SelectedIndex < 0 || cbxCCDIndex.SelectedIndex >= (int)ECAMERA.Count)
                return; ;
            Image<Gray, byte> src = new Image<Gray, byte>(_ImgDisplay[cbxCCDIndex.SelectedIndex].Width, _ImgDisplay[cbxCCDIndex.SelectedIndex].Height);
            _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();
            _ImgChanel[1].CopyTo(src);
            Image<Gray, byte> Mask = new Image<Gray, byte>(src.Width, src.Height);
            src.CopyTo(Mask);
            Mask.SetValue(1);

            src.ROI = new Rectangle(1, 173, 1919, 907);
            List<Point> EdgePoint1 = new List<Point>() ;
            List<Point> EdgePoint2 = new List<Point>();

            EdgePoint1 = PCBImageProcessDef.EdgeDetect(
                src,
                _VisionManager.MatchCollection.GetMatch((EMATCH)cbxCCDIndex.SelectedIndex).GetThreshold()[0],
                _VisionManager.MatchCollection.GetMatch((EMATCH)cbxCCDIndex.SelectedIndex).GetDirectionX()
                );

            EdgePoint2 = PCBImageProcessDef.EdgeDetect(
                src,
                _VisionManager.MatchCollection.GetMatch((EMATCH)cbxCCDIndex.SelectedIndex).GetThreshold()[1],
                _VisionManager.MatchCollection.GetMatch((EMATCH)cbxCCDIndex.SelectedIndex).GetDirectionY()
                );

            ZoomWindow.EdgeInfoMp = new List<MatchPosition>();
            for (int i = 0; i < EdgePoint1.Count; i++)
			{
                Mask.Data[EdgePoint1[i].Y, EdgePoint1[i].X, 0] = 255;
                MatchPosition edgeMp = new MatchPosition { X = EdgePoint1[i].X, Y = EdgePoint1[i].Y };
                ZoomWindow.EdgeInfoMp.Add(edgeMp);
			}
			for (int i = 0; i < EdgePoint2.Count; i++)
			{
				Mask.Data[EdgePoint2[i].Y, EdgePoint2[i].X, 0] = 255;
                MatchPosition edgeMp = new MatchPosition { X = EdgePoint2[i].X, Y = EdgePoint2[i].Y };
                ZoomWindow.EdgeInfoMp.Add(edgeMp);
            }

            Mat element = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross,
					new Size(3, 3), new Point(-1, -1));
			CvInvoke.MorphologyEx(Mask, Mask, Emgu.CV.CvEnum.MorphOp.Dilate, element,
					new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0, 0, 0));
			_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Match(Mask);

			if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
            {
                MatchPosition cMp = new MatchPosition
                {
                    X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                    Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                };
                ZoomWindow.CommonMp = cMp;
            }
            else
            {
                MatchPosition cMp = new MatchPosition { X=-1, Y=-1 };
                ZoomWindow.CommonMp = cMp;
            }
        }
        */

        public Image<Bgr, byte>[] GetImageEdge()
        {
            return _ImageEdge;
        }
        public Image<Bgr, byte> GetCameraImage(ECAMERA eCAMERA)
		{
            //Image<>;
            _VisionManager.CameraCollection.CopyImage(eCAMERA, _ImgDisplay[(int)eCAMERA]);
            return _ImgDisplay[(int)eCAMERA].Clone();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            LogDef.Add(
                ELogFileName.Operate,
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ((Button)sender).Name.ToString() + " Click");
        }

        private void FmAreaCCD_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ThreadEnd = true;
        }
        private void SetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
            }
        }

        private void BtnChoosePosition_Click(object sender, EventArgs e)
        {
            if (cbxCCDIndex.SelectedIndex == 0)
            {
                _DisplayStatus = EDisplayStatus.SetPositon1;
                ZoomWindow.SetInfoString("Set right angle position1.");
                BtnCancelChoose.Enabled = true;
                BtnChoosePosition.Enabled = false;
                BtnSetPosition.Enabled = false;
                cbxCCDIndex.Enabled = false;
                BtnExit.Enabled = false;
            }

            if (cbxCCDIndex.SelectedIndex == 1)
            {
                _DisplayStatus = EDisplayStatus.SetPositon2;
                ZoomWindow.SetInfoString("Set right angle position2.");
                BtnCancelChoose.Enabled = true;
                BtnChoosePosition.Enabled = false;
                BtnSetPosition.Enabled = false;
                cbxCCDIndex.Enabled = false;
                BtnExit.Enabled = false;
            }
        }

        private void FmAreaCCD_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
            /*
            if (_DisplayStatus == EDisplayStatus.LearnCenter)
            {
                if (_LearnStep == 0)
                {

                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };

                        _LearnMatchP1.X = (int)cMp.X;
                        _LearnMatchP1.Y = (int)cMp.Y;

                        ZoomWindow.CommonMp = cMp;

                        _CommonManager.MtnCtrl._Scara.RelativeMove(Scara_mr_je_a_modbus.Axis.J3, 15, 10);

                        _LearnStep++;

                        _tickCount = Environment.TickCount;
                    }
                    else
                    {
                        _DisplayStatus = EDisplayStatus.Show;
                        _LearnStep = 0;
                    }



                }
                else if (_LearnStep == 1 && Environment.TickCount - _tickCount > 5000)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], -5);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };

                        _LearnMatchP2.X = (int)cMp.X;
                        _LearnMatchP2.Y = (int)cMp.Y;

                        ZoomWindow.CommonMp = cMp;

                        PointF RobotCenter = _VisionManager.GetRobotCenter(ECAMERA.Camera1, _LearnMatchP1, _LearnMatchP2, 15);
                        nsMsgFm.MsgFormDef.Add(
                            FrmMsg.EFORM_STYLE.FORM_SHOW,
                            FrmMsg.EBTN_STYLE.BTN_OK,
                            FrmMsg.EMSG_TYPE.MSG_MSG,
                            "CX:" + RobotCenter.X.ToString("0.000") + "CY:" + RobotCenter.Y.ToString("0.000"));

                    }

                    _DisplayStatus = EDisplayStatus.Show;
                    _LearnStep = 0;
                }
            }

            if (_DisplayStatus == EDisplayStatus.CalculateCCDResolution)
            {
                if (_LearnStep == 0)
                {

                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };
                        ZoomWindow.CommonMp = cMp;
                        _LearnMatchP1 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        _CommonManager.MtnCtrl._Scara.RelativeMove(Scara_mr_je_a_modbus.Axis.J2, 100, 10);

                        _LearnStep++;

                        _tickCount = Environment.TickCount;
                    }
                    else
                    {
                        _DisplayStatus = EDisplayStatus.Show;
                        _LearnStep = 0;
                    }



                }
                else if (_LearnStep == 1 && Environment.TickCount - _tickCount > 10000)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        _LearnMatchP2 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };
                        double Resolution = _VisionManager.CalculateCCDResolution(_LearnMatchP1, _LearnMatchP2, 100);
                        ZoomWindow.CommonMp = cMp;
                        nsMsgFm.MsgFormDef.Add(
                            FrmMsg.EFORM_STYLE.FORM_SHOW,
                            FrmMsg.EBTN_STYLE.BTN_OK,
                            FrmMsg.EMSG_TYPE.MSG_MSG,
                            "Resolution:" + Resolution.ToString("0.00000"));

                    }

                    _DisplayStatus = EDisplayStatus.Show;
                    _LearnStep = 0;
                }
            }
            if (_DisplayStatus == EDisplayStatus.CalculateRobotRadius)
            {
                if (_LearnStep == 0)
                {

                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {



                        _LearnMatchP1 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };

                        ZoomWindow.CommonMp = cMp;

                        _CommonManager.MtnCtrl._Scara.RelativeMove(Scara_mr_je_a_modbus.Axis.J3, -5, 10);

                        _LearnStep++;

                        _tickCount = Environment.TickCount;
                    }
                    else
                    {
                        _DisplayStatus = EDisplayStatus.Show;
                        _LearnStep = 0;
                    }
                }
                else if (_LearnStep == 1 && Environment.TickCount - _tickCount > 5000)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 5);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        _LearnMatchP2 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };

                        ZoomWindow.CommonMp = cMp;

                        _LearnStep++;

                        _tickCount = Environment.TickCount;
                        
                        _CommonManager.MtnCtrl._Scara.RelativeMove(Scara_mr_je_a_modbus.Axis.J3, 5, 10);
                        _CommonManager.MtnCtrl._Scara.RelativeMove(Scara_mr_je_a_modbus.Axis.J1, 5, 10);
                    }
                    else
                    {
                        _DisplayStatus = EDisplayStatus.Show;
                        _LearnStep = 0;
                    }
                }
                else if (_LearnStep == 2 && Environment.TickCount - _tickCount > 5000)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], -5);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        PointF LearnMatchP3 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };

                        ZoomWindow.CommonMp = cMp;

                        PointF robotThetaCenter = _VisionManager.GetRobotCenter((ECAMERA)cbxCCDIndex.SelectedIndex, _LearnMatchP1, _LearnMatchP2, -5);
                        PointF robotAlphaCenter = _VisionManager.GetRobotCenter((ECAMERA)cbxCCDIndex.SelectedIndex, _LearnMatchP1, LearnMatchP3, 5);


                        double r = Math.Sqrt((robotThetaCenter.X - robotAlphaCenter.X) * (robotThetaCenter.X - robotAlphaCenter.X) + (robotThetaCenter.Y - robotAlphaCenter.Y) * (robotThetaCenter.Y - robotAlphaCenter.Y));
                        nsMsgFm.MsgFormDef.Add(
                            FrmMsg.EFORM_STYLE.FORM_SHOW,
                            FrmMsg.EBTN_STYLE.BTN_OK,
                            FrmMsg.EMSG_TYPE.MSG_MSG,
                            "Base R:" + (r - _CommonManager.MtnCtrl._Scara.GetPosition(Scara_mr_je_a_modbus.Axis.J2)).ToString("0.000"));

                    }

                    _DisplayStatus = EDisplayStatus.Show;
                    _LearnStep = 0;
                }

            }

            if (_DisplayStatus == EDisplayStatus.CalculateCCDAngle)
            {
                if (_LearnStep == 0)
                {

                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };
                        ZoomWindow.CommonMp = cMp;
                        _LearnMatchP1 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        _CommonManager.MtnCtrl._Scara.RelativeMove(50, 0, 0, 0, 10);

                        _LearnStep++;
                        _tickCount = Environment.TickCount;
                    }
                    else
                    {
                        _DisplayStatus = EDisplayStatus.Show;
                        _LearnStep = 0;
                    }



                }
                else if (_LearnStep == 1 && Environment.TickCount - _tickCount > 5000)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

                    _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).MatchByAngle(_ImgChanel[0], 0);

                    if (_VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).Success())
                    {
                        _LearnMatchP2 = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint();

                        MatchPosition cMp = new MatchPosition
                        {
                            X = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().X,
                            Y = _VisionManager.MatchCollection.GetMatch((EMATCH)cbxMatchIndex.SelectedIndex).GetResultCenterPoint().Y
                        };
                        ZoomWindow.CommonMp = cMp;
                        double a = _VisionManager.GetRobotAngle(_LearnMatchP1, _LearnMatchP2, 50, 0);

                        nsMsgFm.MsgFormDef.Add(
                            FrmMsg.EFORM_STYLE.FORM_SHOW,
                            FrmMsg.EBTN_STYLE.BTN_OK,
                            FrmMsg.EMSG_TYPE.MSG_MSG,
                            "Angle:" + a.ToString("0.000"));

                    }

                    _DisplayStatus = EDisplayStatus.Show;
                    _LearnStep = 0;
                }
            }
            */
        }
        
        private void FmAreaCCD_Load(object sender, EventArgs e)
        {

        }

		private void BtnSetPosition_Click(object sender, EventArgs e)
		{
            _NowPosition.CopyTo(_TempPosition, 0);
            _DisplayStatus = EDisplayStatus.Show;
            ZoomWindow.SetInfoString(string.Empty);
            cbxCCDIndex.Enabled = true;
            BtnChoosePosition.Enabled = true;
            BtnExit.Enabled = true;
            BtnSetPosition.Enabled = false;
            BtnCancelChoose.Enabled = false;
        }

		private void BtnCancelChoose_Click(object sender, EventArgs e)
		{
            _TempPosition.CopyTo(_NowPosition, 0);
            _DisplayStatus = EDisplayStatus.Show;
            ZoomWindow.SetInfoString(string.Empty);
            cbxCCDIndex.Enabled = true;
            BtnChoosePosition.Enabled = true;
            BtnExit.Enabled = true;
            BtnSetPosition.Enabled = false;
            BtnCancelChoose.Enabled = false;
        }
	}
}
