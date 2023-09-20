using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Emgu.CV;
using System.Threading;
using Emgu.CV.CvEnum;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace nsUI
{
    public partial class ZoomAndPanWindow : UserControl
    {
        //zoom parm//
        private int _RatioIndex = 0;
        private int _LastRatioIndex = 0;
        private readonly float[] _Ratio = new float[7];

        //image//
        private string _InfoString = string.Empty;
        private readonly object _Lock = new object();
        private UMat _SrcImage, _FitWindowImage, _DstImage;
        private int _FitWindowImageWidth, _FitWindowImageHeight;
        private float _ImgMagnificationX;
        private float _ImgMagnificationY;

        //private bool _isGettingWindowImage;
        private bool _IsCreatedMaskAndWaferRect = false;
        private bool _IsCreatedCommonRect = false;
        private bool _IsShowedWarning = false;

        private AutoResetEvent _GetWindowImageEvent = new AutoResetEvent(false);

        private ZoomAndPanWindowLearnPair _Mask;
        private ZoomAndPanWindowLearnPair _Wafer;
        private ZoomAndPanWindowLearnPair _Common;

        public MatchPosition WaferMp;
        public MatchPosition MaskMp;
        public MatchPosition CommonMp;
        public Rectangle PanelRectangle;
        public Rectangle MatchRectangle;
        public List<MatchPosition> OcrInfoMp;
        public List<MatchPosition> EdgeInfoMp;

        //point//
        private bool _IsMouseDown = false;
        private Point _AftZoomRoiLeftUpPt = new Point(0, 0);
        private Point _OldPt = new Point(0, 0);
        private Point _PtMouseMove = new Point(0, 0);
        private Point _PtMousedown = new Point(0, 0);
        private Point _PtMouseWheel = new Point(0, 0);

        //switch//
        public bool CanZoom;
        public bool CanRectMaskAndWafer;
        public bool CanRectCommonRoi;
        public bool CanRectLineScanArea;
        public bool CanTrackPattern;
        public bool CanShowOcrInfo;
        public bool CanCenterLine;
        public bool CanShowInfo;
        public bool CanDrawPanelRectangle;
        public bool CanShowEdgeInfo;

        public enum Zoom { ZoomIn, ZoomOut };

        public ZoomAndPanWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            _Ratio[0] = 1;
            for (int i = 1; i < _Ratio.Length; i++) _Ratio[i] = _Ratio[i - 1] * 1.5f;
        }

        private void SKZoomAndPanWindow_MouseDown(object sender, MouseEventArgs e)
        {
            _IsMouseDown = true;
            _PtMousedown = e.Location;
            if (CanRectCommonRoi) _Common.PatternRoiMouseDown(e);
            if (CanRectMaskAndWafer)
            {
                _Mask.PatternRoiMouseDown(e);
                _Wafer.PatternRoiMouseDown(e);
            }
        }

        private void SKZoomAndPanWindow_MouseUp(object sender, MouseEventArgs e)
        {
            _IsMouseDown = false;
            _OldPt = _AftZoomRoiLeftUpPt;
            if (CanRectCommonRoi)
                _Common.PatternRoiMouseUp();
            if (CanRectMaskAndWafer)
            {
                _Mask.PatternRoiMouseUp();
                _Wafer.PatternRoiMouseUp();
            }
        }

        private void SKZoomAndPanWindow_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void SKZoomAndPanWindow_MouseMove(object sender, MouseEventArgs e)
        {
            _PtMouseMove = e.Location;
            if (!_IsCreatedMaskAndWaferRect) return;
            if (!_IsCreatedCommonRect) return;

            if (CanRectCommonRoi)
            {
                _Common.PatternRoiMouseMove(e, _AftZoomRoiLeftUpPt, _FitWindowImageWidth, _FitWindowImageHeight, _Ratio[_RatioIndex]);
                if (_Common.IsAdjustStatus) return;
                if (_Common.IsInRoi == false) Cursor = Cursors.Arrow;
            }

            if (CanRectMaskAndWafer)
            {
                if (!_Mask.IsAdjustStatus)
                    _Wafer.PatternRoiMouseMove(e, _AftZoomRoiLeftUpPt, _FitWindowImageWidth, _FitWindowImageHeight, _Ratio[_RatioIndex]);
                if (!_Wafer.IsAdjustStatus)
                    _Mask.PatternRoiMouseMove(e, _AftZoomRoiLeftUpPt, _FitWindowImageWidth, _FitWindowImageHeight, _Ratio[_RatioIndex]);
                if (_Mask.IsAdjustStatus || _Wafer.IsAdjustStatus) return;
                if (_Mask.IsInRoi == false && _Wafer.IsInRoi == false) Cursor = Cursors.Arrow;
            }
            if (_IsMouseDown)
            {
                _AftZoomRoiLeftUpPt.X = _PtMousedown.X - e.Location.X + _OldPt.X;
                _AftZoomRoiLeftUpPt.Y = _PtMousedown.Y - e.Location.Y + _OldPt.Y;
                PreventRectFail(ref _AftZoomRoiLeftUpPt);
            }
        }

        private void SKZoomAndPanWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (CanZoom)
            {
                if (e.Delta > 0 && _RatioIndex < _Ratio.Length - 1) _RatioIndex++;
                if (e.Delta < 0 && _RatioIndex > 0) _RatioIndex--;
                if (_LastRatioIndex == _RatioIndex) return;

                _AftZoomRoiLeftUpPt.X = (int)((e.X + _AftZoomRoiLeftUpPt.X) * _Ratio[_RatioIndex] / _Ratio[_LastRatioIndex]) - e.X;
                _AftZoomRoiLeftUpPt.Y = (int)((e.Y + _AftZoomRoiLeftUpPt.Y) * _Ratio[_RatioIndex] / _Ratio[_LastRatioIndex]) - e.Y;
                _OldPt = _AftZoomRoiLeftUpPt;

                if (_RatioIndex == 0)   //reset oldpt
                {
                    _OldPt.X = 0;
                    _OldPt.Y = 0;
                }

                _LastRatioIndex = _RatioIndex;
                PreventRectFail(ref _AftZoomRoiLeftUpPt);
            }
        }

        public void ZoomMin()
        {
            _RatioIndex = 0;
            _AftZoomRoiLeftUpPt.X = 0;
            _AftZoomRoiLeftUpPt.Y = 0;
            _OldPt = _AftZoomRoiLeftUpPt;
            _LastRatioIndex = _RatioIndex;
        }

        //avoid roi area not in image plane 
        private void PreventRectFail(ref Point aftZoomRoiLeftUp)
        {
            if (aftZoomRoiLeftUp.X <= 0)
                aftZoomRoiLeftUp.X = 0;

            if (aftZoomRoiLeftUp.Y <= 0)
                aftZoomRoiLeftUp.Y = 0;

            if (aftZoomRoiLeftUp.X + ClientRectangle.Width >= (int)(_FitWindowImageWidth * _Ratio[_RatioIndex]))
                aftZoomRoiLeftUp.X = (int)(_FitWindowImageWidth * _Ratio[_RatioIndex]) - ClientRectangle.Width;

            if (aftZoomRoiLeftUp.Y + ClientRectangle.Height >= (int)(_FitWindowImageHeight * _Ratio[_RatioIndex]))
                aftZoomRoiLeftUp.Y = (int)(_FitWindowImageHeight * _Ratio[_RatioIndex]) - ClientRectangle.Height;
        }

        private void ImageInput(Mat image)
        {
            try
            {
                if (_SrcImage == null) _SrcImage = new UMat();
                image.CopyTo(_SrcImage);
                _FitWindowImage = new UMat();
                CvInvoke.Resize(_SrcImage, _FitWindowImage, ClientSize, 0, 0, Emgu.CV.CvEnum.Inter.Linear); //to fit window
                _GetWindowImageEvent.Set();
                _FitWindowImageWidth = _FitWindowImage.Cols;
                _FitWindowImageHeight = _FitWindowImage.Rows;

                _ImgMagnificationX = (float)_FitWindowImageWidth / (float)image.Width;
                _ImgMagnificationY = (float)_FitWindowImageHeight / (float)image.Height;

                if (Math.Abs(_ImgMagnificationX - _ImgMagnificationY) > 0.05F)
                {
                    if (!_IsShowedWarning)
                        MessageBox.Show("Input image size is not compatible with the window", "SKZoomAndPanWindow", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _IsShowedWarning = true;
                }
                if (!_IsCreatedMaskAndWaferRect)
                {
                    _Mask = new ZoomAndPanWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.6), (int)(ClientSize.Height * 0.4), (int)(ClientSize.Width / 5), (int)(ClientSize.Width / 5)), this);
                    _Wafer = new ZoomAndPanWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.2), (int)(ClientSize.Height * 0.45), (int)(ClientSize.Width / 10), (int)(ClientSize.Width / 10)), this);
                    _IsCreatedMaskAndWaferRect = true;
                }
                if (!_IsCreatedCommonRect)
                {
                    _Common = new ZoomAndPanWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.4), (int)(ClientSize.Height * 0.4), (int)(ClientSize.Width / 5), (int)(ClientSize.Width / 5)), this);
                    _IsCreatedCommonRect = true;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DrawImage()
        {
            try
            {
                if (!_FitWindowImage.IsEmpty)
                {

                    UMat roi = new UMat();
                    int failTimes = 0;
                roiCheckPoint:   //to check if ZoomRatio changed after resize before DstImage = new Mat(roi, s);
                    CvInvoke.Resize(_FitWindowImage, roi, new Size(0, 0), _Ratio[_RatioIndex], _Ratio[_RatioIndex], Emgu.CV.CvEnum.Inter.Linear);
                    Rectangle s = new Rectangle(_AftZoomRoiLeftUpPt, ClientSize);
                    if (0 <= s.X && 0 <= s.Width && s.X + s.Width <= roi.Cols
                        && 0 <= s.Y && 0 <= s.Height && s.Y + s.Height <= roi.Rows)
                    {
                        _DstImage = new UMat(roi, s);
                        if (_DstImage.NumberOfChannels == 1)
                            CvInvoke.CvtColor(_DstImage, _DstImage, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);
                        if (CanZoom) //draw zoom led
                        {
                            for (int i = 0; i < _Ratio.Length; i++)
                            {
                                CvInvoke.Rectangle(_DstImage, new Rectangle(10, 10 + 15 * i, 15, 8),
                                new Emgu.CV.Structure.MCvScalar(0, 125, 0), 2);
                            }
                            CvInvoke.Rectangle(_DstImage, new Rectangle(10, 10 + 15 * _RatioIndex, 15, 8),
                                new Emgu.CV.Structure.MCvScalar(0, 200, 0), -1);
                        }
                        if (CanRectMaskAndWafer)
                        {
                            _DstImage = _Mask.DrawPatternRectangle(_DstImage, _Ratio[_RatioIndex], _AftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                            _DstImage = _Wafer.DrawPatternRectangle(_DstImage, _Ratio[_RatioIndex], _AftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                            if (CanRectLineScanArea)
                            {
                                CvInvoke.PutText(_DstImage, "Vertical", new Point((int)_Mask.PatternRoi.X - 10, (int)_Mask.PatternRoi.Y - 10), FontFace.HersheyComplexSmall, 1, new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                CvInvoke.PutText(_DstImage, "Horizontal", new Point((int)_Wafer.PatternRoi.X - 10, (int)_Wafer.PatternRoi.Y - 10), FontFace.HersheyComplexSmall, 1, new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                            }
                        }
                        if (CanRectCommonRoi)
                        {
                            _DstImage = _Common.DrawPatternRectangle(_DstImage, _Ratio[_RatioIndex], _AftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                        }
                        if (CanCenterLine)
                        {
                            CvInvoke.Line(_DstImage, new Point(_DstImage.Cols / 2, 0), new Point(_DstImage.Cols / 2, _DstImage.Rows), new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                            CvInvoke.Line(_DstImage, new Point(0, _DstImage.Rows / 2), new Point(_DstImage.Cols, _DstImage.Rows / 2), new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                        }
                        if (CanTrackPattern)
                        {
                            if (CommonMp != null)
                            {
                                Point commonPt = Point.Round(new PointF(CommonMp.X * _ImgMagnificationX * _Ratio[_RatioIndex], CommonMp.Y * _ImgMagnificationY * _Ratio[_RatioIndex]));
                                if (s.Contains(commonPt))
                                {
                                    CvInvoke.Line(_DstImage, new Point(commonPt.X - s.X, commonPt.Y - s.Y - 30), new Point(commonPt.X - s.X, commonPt.Y - s.Y + 30)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                                    CvInvoke.Line(_DstImage, new Point(commonPt.X - s.X - 30, commonPt.Y - s.Y), new Point(commonPt.X - s.X + 30, commonPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                                }
                            }
                            if (MaskMp != null && WaferMp != null)
                            {
                                Point maskPt = Point.Round(new PointF(MaskMp.X * _ImgMagnificationX * _Ratio[_RatioIndex], MaskMp.Y * _ImgMagnificationY * _Ratio[_RatioIndex]));
                                Point waferPt = Point.Round(new PointF(WaferMp.X * _ImgMagnificationX * _Ratio[_RatioIndex], WaferMp.Y * _ImgMagnificationY * _Ratio[_RatioIndex]));
                                if (s.Contains(maskPt) || s.Contains(waferPt))
                                {
                                    //CvInvoke.Line(DstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y - 30) , new Point(maskPt.X - s.X, maskPt.Y - s.Y + 30)
                                    //    ,new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    //CvInvoke.Line(DstImage, new Point(maskPt.X - s.X - 30, maskPt.Y - s.Y), new Point(maskPt.X - s.X + 30, maskPt.Y - s.Y)
                                    //    , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_DstImage, new Point(maskPt.X - s.X - 60, maskPt.Y - s.Y), new Point(maskPt.X - s.X - 30, maskPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_DstImage, new Point(maskPt.X - s.X + 30, maskPt.Y - s.Y), new Point(maskPt.X - s.X + 60, maskPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_DstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y - 60), new Point(maskPt.X - s.X, maskPt.Y - s.Y - 30)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_DstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y + 30), new Point(maskPt.X - s.X, maskPt.Y - s.Y + 60)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_DstImage, new Point(waferPt.X - s.X, waferPt.Y - s.Y - 30), new Point(waferPt.X - s.X, waferPt.Y - s.Y + 30)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                                    CvInvoke.Line(_DstImage, new Point(waferPt.X - s.X - 30, waferPt.Y - s.Y), new Point(waferPt.X - s.X + 30, waferPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                                }
                            }
                        }
                        if (CanShowOcrInfo)
                        {
                            if (OcrInfoMp.Count > 0)
                            {
                                for (int i = 0; i < OcrInfoMp.Count; i++)
                                {
                                    if (OcrInfoMp[i] != null)
                                    {
                                        Rectangle ocrRect = new Rectangle((int)OcrInfoMp[i].X, (int)OcrInfoMp[i].Y, (int)OcrInfoMp[i].TemplateSize.Width, (int)OcrInfoMp[i].TemplateSize.Height);
                                        ocrRect.X = (int)(ocrRect.X * _ImgMagnificationX * _Ratio[_RatioIndex] - s.X);
                                        ocrRect.Y = (int)(ocrRect.Y * _ImgMagnificationY * _Ratio[_RatioIndex] - s.Y);
                                        ocrRect.Width = (int)(ocrRect.Width * _ImgMagnificationX * _Ratio[_RatioIndex]);
                                        ocrRect.Height = (int)(ocrRect.Height * _ImgMagnificationY * _Ratio[_RatioIndex]);

                                        CvInvoke.PutText(_DstImage, OcrInfoMp[i].Char.ToString(), new Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.5)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 1 * _Ratio[_RatioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                        CvInvoke.Rectangle(_DstImage, ocrRect, new Emgu.CV.Structure.MCvScalar(0), 1, Emgu.CV.CvEnum.LineType.FourConnected);
                                        if (i % 2 == 1)
                                            CvInvoke.PutText(_DstImage, OcrInfoMp[i].Score.ToString("f2"), new Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.1)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5 * _Ratio[_RatioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                        else
                                            CvInvoke.PutText(_DstImage, OcrInfoMp[i].Score.ToString("f2"), new Point((int)ocrRect.X, (int)(ocrRect.Y + ocrRect.Height * 1.3)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5 * _Ratio[_RatioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                    }
                                }
                            }
                        }
                        if (CanShowEdgeInfo)
                        {
                            if (EdgeInfoMp != null)
                            {
                                if (EdgeInfoMp.Count > 0)
                                {
                                    for (int i = 0; i < EdgeInfoMp.Count; i++)
                                    {
                                        if (EdgeInfoMp[i] != null)
                                        {
                                            Point edgeRect = Point.Round(new PointF(EdgeInfoMp[i].X * _ImgMagnificationX * _Ratio[_RatioIndex], EdgeInfoMp[i].Y * _ImgMagnificationY * _Ratio[_RatioIndex]));
                                            if (s.Contains(edgeRect))
                                            {
                                                CvInvoke.Circle(_DstImage, new Point(edgeRect.X - s.X, edgeRect.Y - s.Y), 1, new Emgu.CV.Structure.MCvScalar(255, 0, 0), -1);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (CanShowInfo)
                        {
                            CvInvoke.PutText(_DstImage, _InfoString, new Point(10, 30), Emgu.CV.CvEnum.FontFace.HersheySimplex, 1, new Emgu.CV.Structure.MCvScalar(0, 0, 255), 2);
                        }
                        if (CanDrawPanelRectangle)
                        {
                            if (PanelRectangle != null)
                            {
                                Rectangle temp = PanelRectangle;
                                temp.X = (int)(PanelRectangle.X * _ImgMagnificationX * _Ratio[_RatioIndex] - s.X);
                                temp.Y = (int)(PanelRectangle.Y * _ImgMagnificationY * _Ratio[_RatioIndex] - s.Y);
                                temp.Width = (int)(PanelRectangle.Width * _ImgMagnificationX * _Ratio[_RatioIndex]);
                                temp.Height = (int)(PanelRectangle.Height * _ImgMagnificationY * _Ratio[_RatioIndex]);

                                CvInvoke.Rectangle(_DstImage, temp, new Emgu.CV.Structure.MCvScalar(0, 0, 255), 1);
                            }
                            if (MatchRectangle != null)
                            {
                                Rectangle temp = MatchRectangle;
                                temp.X = (int)(MatchRectangle.X * _ImgMagnificationX * _Ratio[_RatioIndex] - s.X);
                                temp.Y = (int)(MatchRectangle.Y * _ImgMagnificationY * _Ratio[_RatioIndex] - s.Y);
                                temp.Width = (int)(MatchRectangle.Width * _ImgMagnificationX * _Ratio[_RatioIndex]);
                                temp.Height = (int)(MatchRectangle.Height * _ImgMagnificationY * _Ratio[_RatioIndex]);

                                CvInvoke.Rectangle(_DstImage, temp, new Emgu.CV.Structure.MCvScalar(255, 128, 200), 1);
                            }
                        }
                    }
                    else
                    {
                        failTimes++;
                        if (failTimes > 3) return;
                        goto roiCheckPoint;
                    }

                    roi.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UMat ShowOcrInfo(UMat img)
        {
            return img;
        }

        Graphics G;
        bool CreateGraphic = false;
        private void DisplayImage()
        {
            try
            {
                if (!_FitWindowImage.IsEmpty)
                {
                    //drawImage();
                    Bitmap showImg = _DstImage.ToBitmap();

                    if (!CreateGraphic)
                    {
                        G = this.CreateGraphics();
                        G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        G.InterpolationMode = InterpolationMode.NearestNeighbor;
                        G.PageUnit = GraphicsUnit.Pixel;
                        CreateGraphic = true;
                    }
                    G.DrawImage(showImg, ClientRectangle.Location);

                    //if (_isGettingWindowImage)
                    //{
                    //    if (_gettingWindowImage != null) _gettingWindowImage.Dispose();
                    //    _gettingWindowImage = _dstImage.Clone();
                    //    _isGettingWindowImage = false;
                    //}

                    //showImg.Dispose();
                    //SrcImage.Dispose();
                    //FitWindowImage.Dispose();
                    //DstImage.Dispose();
                    //G.Dispose();
                    GC.Collect();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetImage(Mat image)
        {
            if (image.Width < 1 || image.Height < 1) return;
            ImageInput(image);
            DrawImage();
            DisplayImage();
        }

        public Rectangle GetMaskRectangle()
        {
            RectangleF correctRectF = new RectangleF(_Mask.PatternRoi.X / _ImgMagnificationX, _Mask.PatternRoi.Y / _ImgMagnificationY
                , _Mask.PatternRoi.Width / _ImgMagnificationX, _Mask.PatternRoi.Height / _ImgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
        }
        public Rectangle GetWaferRectangle()
        {
            RectangleF correctRectF = new RectangleF(_Wafer.PatternRoi.X / _ImgMagnificationX, _Wafer.PatternRoi.Y / _ImgMagnificationY
                , _Wafer.PatternRoi.Width / _ImgMagnificationX, _Wafer.PatternRoi.Height / _ImgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
        }
        public Rectangle GetCommonRectangle()
        {
            RectangleF correctRectF = new RectangleF(_Common.PatternRoi.X / _ImgMagnificationX, _Common.PatternRoi.Y / _ImgMagnificationY
                , _Common.PatternRoi.Width / _ImgMagnificationX, _Common.PatternRoi.Height / _ImgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
        }

        /*
        public Mat GetSrcImage()
        {
            lock (Lock)
            {
                IsGettingSrcImage = true;
                while (true)
                {
                    if (IsGettingSrcImage == false)
                        return GettingSrcImage;
                    Thread.Sleep(10);
                }
            }
        }
        */
        public Mat GetWindowImage()
        {
            lock (_Lock)
            {
                //_isGettingWindowImage = true;
                //while (true)
                //{
                //    if (_isGettingWindowImage == false)
                //        return _gettingWindowImage.GetMat(AccessType.Fast);
                //    Thread.Sleep(10);
                //}
                if (!_GetWindowImageEvent.WaitOne(1000))
                    throw new Exception("Get window image fail.");
                else
                    return _FitWindowImage.GetMat(AccessType.Fast);
            }
        }

        public void SetInfoString(string info)
        {
            _InfoString = info;
        }

        public Point ConverUiLocationToOriginalLocation(Point UiLocation)
        {
            if (_ImgMagnificationX != 0 && _ImgMagnificationY != 0)
            {
                Point OriginalLocation = new Point
                {
                    X = (int)Math.Round((_AftZoomRoiLeftUpPt.X + UiLocation.X) / _Ratio[_RatioIndex] / _ImgMagnificationX),
                    Y = (int)Math.Round((_AftZoomRoiLeftUpPt.Y + UiLocation.Y) / _Ratio[_RatioIndex] / _ImgMagnificationY)
                };
                if (OriginalLocation.X >= _SrcImage.Cols) OriginalLocation.X = _SrcImage.Cols - 1;
                if (OriginalLocation.Y >= _SrcImage.Rows) OriginalLocation.Y = _SrcImage.Rows - 1;
                return OriginalLocation;
            }

            return new Point(0, 0);
        }

        public void ManualZoom(Zoom zoom)
        {
            if (CanZoom)
            {
                if (zoom == Zoom.ZoomIn && _RatioIndex < _Ratio.Length - 1) _RatioIndex++;
                if (zoom == Zoom.ZoomOut && _RatioIndex > 0) _RatioIndex--;

                if (_LastRatioIndex == _RatioIndex) return;

                _AftZoomRoiLeftUpPt.X = (int)((this.Width / 2 + _AftZoomRoiLeftUpPt.X) * _Ratio[_RatioIndex] / _Ratio[_LastRatioIndex]) - this.Width / 2;
                _AftZoomRoiLeftUpPt.Y = (int)((this.Height / 2 + _AftZoomRoiLeftUpPt.Y) * _Ratio[_RatioIndex] / _Ratio[_LastRatioIndex]) - this.Height / 2;
                _OldPt = _AftZoomRoiLeftUpPt;

                if (_RatioIndex == 0)   //reset oldpt
                {
                    _OldPt.X = 0;
                    _OldPt.Y = 0;
                }

                _LastRatioIndex = _RatioIndex;
                PreventRectFail(ref _AftZoomRoiLeftUpPt);
            }
        }

        public ZoomInfo GetZoomInfo()
        {
            ZoomInfo zi = new ZoomInfo
            {
                ZoomRatio = _Ratio[_RatioIndex],
                AftFitMagnificationX = _ImgMagnificationX,
                AftFitMagnificationY = _ImgMagnificationY
            };
            return zi;
        }

        public void ChangeCommonRectLocation(Rectangle r)
        {
            RectangleF rF;
            rF = new RectangleF((float)(r.X * _ImgMagnificationX), (float)(r.Y * _ImgMagnificationY), (float)(r.Width * _ImgMagnificationX), (float)(r.Height * _ImgMagnificationY));
            _Common.PatternRoi = rF;

        }

        public void MaskMove(int x, int y)
        {
            _Mask.PatternRoi.X += x / _Ratio[_RatioIndex];
            _Mask.PatternRoi.Y += y / _Ratio[_RatioIndex];

            _Mask.PatternOldRoi.X += x / _Ratio[_RatioIndex];
            _Mask.PatternOldRoi.Y += y / _Ratio[_RatioIndex];
        }

        public void WaferMove(int x, int y)
        {
            _Wafer.PatternRoi.X += x / _Ratio[_RatioIndex];
            _Wafer.PatternRoi.Y += y / _Ratio[_RatioIndex];

            _Wafer.PatternOldRoi.X += x / _Ratio[_RatioIndex];
            _Wafer.PatternOldRoi.Y += y / _Ratio[_RatioIndex];
        }

        public void CommonMove(int x, int y)
        {
            _Common.PatternRoi.X += x / _Ratio[_RatioIndex];
            _Common.PatternRoi.Y += y / _Ratio[_RatioIndex];

            _Common.PatternOldRoi.X += x / _Ratio[_RatioIndex];
            _Common.PatternOldRoi.Y += y / _Ratio[_RatioIndex];
        }

        public void SetCommonOriginalLocation(Rectangle rect)
        {
            _Common.PatternRoi.X = rect.X * _ImgMagnificationX;
            _Common.PatternRoi.Y = rect.Y * _ImgMagnificationY;
            _Common.PatternRoi.Width = rect.Width * _ImgMagnificationX;
            _Common.PatternRoi.Height = rect.Height * _ImgMagnificationY;

            _Common.PatternOldRoi = _Common.PatternRoi;
        }

        public void CommonOriginalMove(float x, float y)
        {
            _Common.PatternRoi.X += x * _ImgMagnificationX;
            _Common.PatternRoi.Y += y * _ImgMagnificationY;

            _Common.PatternOldRoi.X += x * _ImgMagnificationX;
            _Common.PatternOldRoi.Y += y * _ImgMagnificationY;
        }

        public void MaskOriginalMove(float x, float y)
        {
            _Mask.PatternRoi.X += x * _ImgMagnificationX;
            _Mask.PatternRoi.Y += y * _ImgMagnificationY;

            _Mask.PatternOldRoi.X += x * _ImgMagnificationX;
            _Mask.PatternOldRoi.Y += y * _ImgMagnificationY;
        }

        public void WaferOriginalMove(float x, float y)
        {
            _Wafer.PatternRoi.X += x * _ImgMagnificationX;
            _Wafer.PatternRoi.Y += y * _ImgMagnificationY;

            _Wafer.PatternOldRoi.X += x * _ImgMagnificationX;
            _Wafer.PatternOldRoi.Y += y * _ImgMagnificationY;
        }
    }

    public class ZoomInfo
    {
        public float ZoomRatio;
        public float AftFitMagnificationX;
        public float AftFitMagnificationY;
    }
}
