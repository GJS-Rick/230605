using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using VisionLibrary;
using CommonLibrary;
using System.Threading;
using System.Collections.Generic;

namespace nsUI
{
    public partial class FmAreaCCD : Form
    {
        public enum EDisplayStatus
        {
            Show,
            LearnMatchCorner1,
            LearnMatchCorner2,
            LearnMatchMask1,
            LearnMatchMask2,
        }
        public enum ScanMode
        {
            UpToDown,
            DownToUp,
            LeftToRight,
            RightToLeft
        }

        private Image<Gray, byte>[] _ImgChanel;
        private Image<Bgr, byte>[] _ImgDisplay;

        private EDisplayStatus _DisplayStatus;
        private Rectangle _tROI;
        private int _LearnMatchingCornerCount;

        private readonly object _DisplayLocker = new object();
        private Thread _DisplayThread;

        private Mat _LoadImage;
        private bool _IsLive;
        private bool _ThreadEnd;

        private int _LearnStep;

        private PointF _LearnMatchP1;
        private PointF _LearnMatchP2;
        private int _tickCount;

        public FmAreaCCD()
        {

            InitializeComponent();

            SetClickEvnet();

            _ImgDisplay = new Image<Bgr, byte>[(int)ECamera.Count];
            for (int i = 0; i < (int)ECamera.Count; i++)
                _ImgDisplay[i] = new Image<Bgr, byte>(G.Vision.CameraCollection.GetWidth((ECamera)i), G.Vision.CameraCollection.GetHeight((ECamera)i));

            cbxCCDIndex.Items.Clear();
            for (int i = 0; i < (int)ECamera.Count; i++)
                cbxCCDIndex.Items.Add(((ECamera)i).ToString());

            comboBoxAlignIndex.Items.Clear();
            for (int i = 0; i < (int)EAlignIndex.Count; i++)
                comboBoxAlignIndex.Items.Add(((EAlignIndex)i).ToString());

            ZoomWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            ZoomWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);

            for (int i = 0; i < (int)ECamera.Count; i++)
                G.Vision.CameraCollection.SetExposureByDefault((ECamera)i);

            BtnCAMProperties.Visible = false;

            BtnRectCorner.Enabled = true;
            BtnMakeMask.Enabled = false;
            BtnCancelRect.Enabled = true;
            BtnLearnCorner.Enabled = false;
            ZoomWindow.CanTrackPattern = true;
            ZoomWindow.CanZoom = true;
            ZoomWindow.CanShowInfo = true;
            ZoomWindow.CanDrawPanelRectangle = true;
            _IsLive = false;
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
                    G.Vision.CameraCollection.CopyImage((ECamera)cameraIndex, _ImgDisplay[cameraIndex]);

                    int alignIndex = -1;
                    Invoke((MethodInvoker)delegate () { alignIndex = comboBoxAlignIndex.SelectedIndex; });
                    int cornerIndex = -1;
                    Invoke((MethodInvoker)delegate () { cornerIndex = comboBoxCorner.SelectedIndex; });
                    ZoomWindow.PanelRectangle = G.Vision.GetPanelRectangleOnImage(
                        (EAlignIndex)alignIndex,
                        (ECorner)cornerIndex);
                    if (_ImgDisplay[cameraIndex] != null)
                    {
                        Image<Bgr, byte> image = _ImgDisplay[cameraIndex].Clone();
                        ZoomWindow.SetImage(image.Mat);
                    }

                    Thread.Sleep(10);
                }
            }
        }

        new private void MouseUp(object sender, MouseEventArgs e)
        {
            if (_ImgDisplay[cbxCCDIndex.SelectedIndex].Width < 1 || _ImgDisplay[cbxCCDIndex.SelectedIndex].Height < 1)
                return;

            Point stOriginalImageLocation = ZoomWindow.ConverUiLocationToOriginalLocation(e.Location);
            if (_DisplayStatus == EDisplayStatus.LearnMatchCorner1 || _DisplayStatus == EDisplayStatus.LearnMatchCorner2 ||
                _DisplayStatus == EDisplayStatus.LearnMatchMask1 || _DisplayStatus == EDisplayStatus.LearnMatchMask2)
            {
                _LearnMatchingCornerCount++;
                if (_LearnMatchingCornerCount == 1)
                {
                    if (_DisplayStatus == EDisplayStatus.LearnMatchCorner1)
                        ZoomWindow.SetInfoString("Click corner1 roi right-down point.");
                    if (_DisplayStatus == EDisplayStatus.LearnMatchCorner2)
                        ZoomWindow.SetInfoString("Click corner2 roi right-down point.");
                    if (_DisplayStatus == EDisplayStatus.LearnMatchMask1)
                        ZoomWindow.SetInfoString("Click Mask roi right-down point.");
                    if (_DisplayStatus == EDisplayStatus.LearnMatchMask2)
                        ZoomWindow.SetInfoString("Click Mask roi right-down point.");
                    _tROI.X = stOriginalImageLocation.X;
                    _tROI.Y = stOriginalImageLocation.Y;
                }
                if (_LearnMatchingCornerCount == 2)
                {
                    _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();
                    _tROI.Width = Math.Abs(stOriginalImageLocation.X - _tROI.X);
                    _tROI.Height = Math.Abs(stOriginalImageLocation.Y - _tROI.Y);


                    ZoomWindow.SetCommonOriginalLocation(_tROI);

                    ZoomWindow.SetInfoString("Click learn button after adjusting roi");
                    ZoomWindow.CanRectCommonRoi = true;
                    BtnLearnCorner.Enabled = true;
                    BtnRectCorner.Enabled = false;
                }
            }
        }

        new private void MouseMove(object sender, MouseEventArgs e)
        {
            if (_ImgDisplay[cbxCCDIndex.SelectedIndex].Width < 1 || _ImgDisplay[cbxCCDIndex.SelectedIndex].Height < 1)
                return;
            Point OriginalLocation = ZoomWindow.ConverUiLocationToOriginalLocation(e.Location);
            if((new Rectangle(0,0, _ImgDisplay[cbxCCDIndex.SelectedIndex].Data.GetLength(1), _ImgDisplay[cbxCCDIndex.SelectedIndex].Data.GetLength(0))).Contains(OriginalLocation))
            {
                byte RGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 2];
                byte GGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 1];
                byte BGray = _ImgDisplay[cbxCCDIndex.SelectedIndex].Data[OriginalLocation.Y, OriginalLocation.X, 0];
                statusStripInfo.Items[0].Text =
               "X:" + OriginalLocation.X.ToString("0000") + "," +
               "Y:" + OriginalLocation.Y.ToString("0000") + ", " +
               "R:" + (RGray).ToString("000") + ", " +
               "G:" + (GGray).ToString("000") + ", " +
               "B:" + (BGray).ToString("000");
            }
        }
        private void BtnLive_Click(object sender, EventArgs e)
        {
            if (_IsLive)
            {
                _IsLive = false;
                BtnLive.Text = "Live";
            }
            else
            {
                _IsLive = true;
                BtnLive.Text = "Stop";
            }
        }

        private void FmAreaCCD_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (_DisplayThread == null)
                {
                    _DisplayThread = new Thread(Display);
                    _DisplayThread.Start();
                }

                cbxCCDIndex.SelectedIndex = 0;
                comboBoxAlignIndex.SelectedIndex = 0;
                comboBoxCorner.SelectedIndex = 0;
                _IsLive = true;
                BtnLive.Text = "Stop";
            }
            else
            {
                _IsLive = false;
            }
        }
        private void BtnAlignTest_Click(object sender, EventArgs e)
        {
            if (cbxCCDIndex.SelectedIndex < 0 || cbxCCDIndex.SelectedIndex >= (int)ECamera.Count)
                return;

            //G.Vision.SetAlignPos(
            //    EAlignIndex.Align1,
            //    new System.Drawing.PointF((float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterX], (float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterY]),
            //    new System.Drawing.PointF((float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterX], (float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterY]),
            //    new System.Drawing.PointF((float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterX], (float)G.FileStream.MachineData.ValueDouble[(int)EMachineDouble.RobotCenterY]));

            DrawAlignResult();
        }

        private void BtnSaveImage_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                G.Vision.CameraCollection.CopyImage((ECamera)cbxCCDIndex.SelectedIndex, _ImgDisplay[cbxCCDIndex.SelectedIndex]);
                _ImgDisplay[cbxCCDIndex.SelectedIndex].Save(saveFileDialog1.FileName);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _LoadImage = CvInvoke.Imread(openFileDialog1.FileName, ImreadModes.AnyColor);
                _ImgDisplay[cbxCCDIndex.SelectedIndex].Dispose();
                _ImgDisplay[cbxCCDIndex.SelectedIndex] = new Image<Bgr, byte>(openFileDialog1.FileName);
                
                G.Vision.SetImage((ECamera)cbxCCDIndex.SelectedIndex, _ImgDisplay[cbxCCDIndex.SelectedIndex]);
               
            }
            _IsLive = false;
            BtnLive.Text = "Live";
        }


        private void SaveCorner()
        {
            _ImgDisplay[cbxCCDIndex.SelectedIndex].ROI = ZoomWindow.GetCommonRectangle();
            G.Vision.LearnMatchModel((EAlignIndex)comboBoxAlignIndex.SelectedIndex, (ECorner)comboBoxCorner.SelectedIndex, _ImgDisplay[cbxCCDIndex.SelectedIndex], (float)numericUpDownScore.Value);
            _ImgDisplay[cbxCCDIndex.SelectedIndex].ROI = Rectangle.Empty;

            G.Vision.SaveMatchModel((EAlignIndex)comboBoxAlignIndex.SelectedIndex, (ECorner)comboBoxCorner.SelectedIndex);
        }

        private void DrawAlignResult()
        {
            double shiftX = 0;
            double shiftY = 0;
            double shiftAngle = 0;
            bool success = G.Vision.Align((EAlignIndex)comboBoxAlignIndex.SelectedIndex,
                ref shiftX,
                ref shiftY,
                ref shiftAngle);

            if (success)
                MessageBox.Show("shiftX:" + shiftX.ToString("F4") + "\nshiftY:" + shiftY.ToString("F4") + "\nshiftAngle:" + (-shiftAngle).ToString("F8") +
                    "\nC1:(" + G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][(int)ECorner.Corner1].X.ToString("F2") + "," + G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][(int)ECorner.Corner1].Y.ToString("F2") + ")\n" +
                    "C2:(" + G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][(int)ECorner.Corner2].X.ToString("F2") + "," + G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][(int)ECorner.Corner2].Y.ToString("F2"));

            MatchPosition cMp = new MatchPosition();
            cMp.X = (int)G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][comboBoxCorner.SelectedIndex].X;
            cMp.Y = (int)G.Vision.ResultCorner[comboBoxAlignIndex.SelectedIndex][comboBoxCorner.SelectedIndex].Y;
            ZoomWindow.CommonMp = cMp;
        }

        public Image<Bgr, byte> GetCameraImage(ECamera eCAMERA)
        {
            //Image<>;
            G.Vision.CameraCollection.CopyImage(eCAMERA, _ImgDisplay[(int)eCAMERA]);
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

        private void BtnSetExposure_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否設定亮度", "亮度", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                G.Vision.CameraCollection.SaveExposure((ECamera)cbxCCDIndex.SelectedIndex, (double)trackBarExposureTime.Value);
        }

        private void FmAreaCCD_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ThreadEnd = true;
            _DisplayThread.Abort();
        }


        private void CbxCCDIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                trackBarExposureTime.Maximum = G.Vision.CameraCollection.GetExposureTimeRangeMax((ECamera)cbxCCDIndex.SelectedIndex);
                trackBarExposureTime.Minimum = G.Vision.CameraCollection.GetExposureTimeRangeMin((ECamera)cbxCCDIndex.SelectedIndex);
                trackBarExposureTime.SmallChange = G.Vision.CameraCollection.GetExposureTimeRangeStep((ECamera)cbxCCDIndex.SelectedIndex);
                trackBarExposureTime.LargeChange = G.Vision.CameraCollection.GetExposureTimeRangeStep((ECamera)cbxCCDIndex.SelectedIndex);
                trackBarExposureTime.Value = G.Vision.CameraCollection.GetExposure((ECamera)cbxCCDIndex.SelectedIndex, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
            }
        }

        private void ButtonLearnCorner_Click(object sender, EventArgs e)
        {
            if (comboBoxCorner.SelectedIndex == 0)
            {
                _LearnMatchingCornerCount = 0;
                _DisplayStatus = EDisplayStatus.LearnMatchCorner1;
                ZoomWindow.SetInfoString("Click corner1 roi left-up point.");
                BtnRectCorner.Enabled = false;
                BtnLearnCorner.Enabled = false;
            }

            if (comboBoxCorner.SelectedIndex == 1)
            {
                _LearnMatchingCornerCount = 0;
                _DisplayStatus = EDisplayStatus.LearnMatchCorner2;
                ZoomWindow.SetInfoString("Click corner2 roi left-up point.");
                BtnRectCorner.Enabled = false;
                BtnLearnCorner.Enabled = false;
            }
        }

        private void WindowRoiRectangleControl(object sender, EventArgs e)
        {
            if ((Button)sender == BtnZoomIn)
                ZoomWindow.ManualZoom(ZoomAndPanWindow.Zoom.ZoomIn);
            if ((Button)sender == BtnZoomOut)
                ZoomWindow.ManualZoom(ZoomAndPanWindow.Zoom.ZoomOut);
            if ((Button)sender == BtnUp)
                ZoomWindow.CommonMove(0, -1);
            if ((Button)sender == BtnDown)
                ZoomWindow.CommonMove(0, 1);
            if ((Button)sender == BtnLeft)
                ZoomWindow.CommonMove(-1, 0);
            if ((Button)sender == BtnRight)
                ZoomWindow.CommonMove(1, 0);
        }

        private void ButtonCancelRect_Click(object sender, EventArgs e)
        {
            _DisplayStatus = EDisplayStatus.Show;
            _LearnMatchingCornerCount = 0;
            ZoomWindow.SetInfoString(string.Empty);
            ZoomWindow.CanRectCommonRoi = false;
            BtnRectCorner.Enabled = true;
            BtnLearnCorner.Enabled = false;
        }

        private void TrackBarExposureTime_Scroll(object sender, EventArgs e)
        {
            G.Vision.CameraCollection.SetExposure((ECamera)cbxCCDIndex.SelectedIndex, (double)trackBarExposureTime.Value);
        }

        private void FmAreaCCD_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FmAreaCCD_Load(object sender, EventArgs e)
        {
            this.Font = new Font("新細明體", 12);
        }

        private void buttonMakeMask_Click(object sender, EventArgs e)
        {
            if (G.Vision.Match[comboBoxAlignIndex.SelectedIndex][comboBoxCorner.SelectedIndex] == null)
                return;

            if (_LoadImage == null)
                _LoadImage = new Mat();
            G.Vision.Match[comboBoxAlignIndex.SelectedIndex][comboBoxCorner.SelectedIndex].GetImage().Mat.CopyTo(_LoadImage);

            _LearnMatchingCornerCount = 0;
            _DisplayStatus = EDisplayStatus.LearnMatchMask1;
            ZoomWindow.SetInfoString("Click Mask1 roi left-up point.");

            _IsLive = false;
            BtnLive.Text = "Live";
        }

        private void BtnAutoSetExposure_Click(object sender, EventArgs e)
        {
            _ImgChanel = _ImgDisplay[cbxCCDIndex.SelectedIndex].Split();

            Rectangle stRectModel1 = new Rectangle();
            Image<Gray, byte> cPreprocessingImg = Process.ImageArithmetic(_ImgDisplay[cbxCCDIndex.SelectedIndex], G.Vision.ImageFormula[comboBoxAlignIndex.SelectedIndex]);

            if (Process.GetAutoMatchModel(cPreprocessingImg, ref stRectModel1))
            {
                stRectModel1.X += cPreprocessingImg.ROI.X;
                stRectModel1.Y += cPreprocessingImg.ROI.Y;

                ZoomWindow.MatchRectangle = stRectModel1;

                double fScore = 0;
                if (!double.TryParse(numericUpDownScore.Value.ToString(), out fScore))
                {
                    MessageBox.Show("請輸入正確定位分數");
                    return;
                }

                if (MessageBox.Show("是否設定為定位影像?", "Auto Learn", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    _ImgDisplay[cbxCCDIndex.SelectedIndex].ROI = ZoomWindow.GetCommonRectangle();
                    G.Vision.LearnMatchModel((EAlignIndex)comboBoxAlignIndex.SelectedIndex, (ECorner)comboBoxCorner.SelectedIndex, _ImgDisplay[cbxCCDIndex.SelectedIndex], (float)numericUpDownScore.Value);
                    _ImgDisplay[cbxCCDIndex.SelectedIndex].ROI = Rectangle.Empty;

                    G.Vision.SaveMatchModel((EAlignIndex)comboBoxAlignIndex.SelectedIndex, (ECorner)comboBoxCorner.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("找不到合適定位點，請手動框選定位點");
                return;
            }
        }

        private void btnVisionReload_Click(object sender, EventArgs e)
        {
            G.Vision.CameraCollection.Reload();
            G.Vision.ReadSetting();
            G.Vision.UpdateAlignParameter();
        }

        private void BtnFormula_Click(object sender, EventArgs e)
        {
            FmImageFormulaEditor f = new FmImageFormulaEditor(G.Vision);
            f.ShowDialog();
        }

        private void BtnCAMProperties_Click(object sender, EventArgs e)
        {
            G.Vision.CameraCollection.GetCamera((ECamera)cbxCCDIndex.SelectedIndex).OpenPropertyPage();
            //int color = G.VisionManager.IdentifyColors(
            //	ECAMERA.Camera1,
            //	ECAMERA.Camera2,
            //	G.VisionManager.MatchCollection.GetMatch(EMATCH.Corner1).GetROI(),
            //	G.VisionManager.MatchCollection.GetMatch(EMATCH.Corner2).GetROI());//Test
        }

        private void BtnLearnCorner_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否儲存定位點影像?", "儲存", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;

            SaveCorner();

            _DisplayStatus = EDisplayStatus.Show;
            _LearnMatchingCornerCount = 0;
            ZoomWindow.SetInfoString(string.Empty);
            ZoomWindow.CanRectCommonRoi = false;
            BtnRectCorner.Enabled = true;
            BtnLearnCorner.Enabled = false;
        }
    }
}
