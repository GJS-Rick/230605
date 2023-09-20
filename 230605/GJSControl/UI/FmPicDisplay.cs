using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLibrary;
using Emgu.CV;
using Emgu.CV.Structure;
using FileStreamLibrary;
using VisionLibrary;

namespace nsUI
{
    public enum EPicBox
    {
        A1,
        A2,
        B1,
        B2,
        Count
    }
    public partial class FmPicDisplay : Form
    {
        private Bitmap[] _BackBmp;
        private TrackBar[] _CCDExposureTrackBar;
        private PictureBox[] _PicBox;

        public FmPicDisplay()
        {
            InitializeComponent();
           
        }

        private void FmPicDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void FmNextPage_Load(object sender, EventArgs e)
        {
            

            _CCDExposureTrackBar = new TrackBar[] { ExposureTimeA1, ExposureTimeA2, ExposureTimeB1, ExposureTimeB2 };
            _PicBox = new PictureBox[] { picBxDisplayA1, picBxDisplayA2, picBxDisplayB1, picBxDisplayB2 };
            _BackBmp = new Bitmap[_PicBox.Length];
            for (int i = 0; i < _PicBox.Length; i++)
            {
                _BackBmp[i] = new Bitmap(_PicBox[i].Width, _PicBox[i].Height);
            }

            for (int i = 0; i < (int)_PicBox.Length; i++)
            {
                _PicBox[i].Visible = false;
                _CCDExposureTrackBar[i].Visible = false;
            }
            for (int i = 0; i < (int)ECamera.Count; i++)
            {
                _CCDExposureTrackBar[i].Visible = true;
                _PicBox[i].SizeMode = PictureBoxSizeMode.Zoom;
                _PicBox[i].Visible = true;
            }


            //PicPositionSizeSetting(5, 4, 35, 4, 2, _PicBox);

            
            
        }

        private void PicPositionSizeSetting(int StartX, int StartY, int DisX, int DisY, int BoxNum, PictureBox[] PicBox)
        {
            int _offset = 0;
            for (int i = 0; i < PicBox.Length; i++)
            {
                if (PicBox[i] == null)
                    return;

                PicBox[i].Width = 280;
                PicBox[i].Height = 222;

                if (!PicBox[i].Visible)
                    _offset = _offset - 1;

                PicBox[i].Left = StartX + ((i + _offset) % BoxNum) * (PicBox[i].Width + DisX);
                PicBox[i].Top = StartY + ((i + _offset) / BoxNum) * (PicBox[i].Height + DisY);
            }
        }

        

       

        public void DrawDisplayImg(EPicBox PicBox, Image<Bgr, byte> SrcImg)
        {
            // =========== CCD1 ======================================
            if (SrcImg.Width <= 0 || SrcImg.Height <= 0)
                return;

           
            int index = (int)PicBox;
            if (index > _PicBox.Length)
                return;

            int nW = _PicBox[index].Width;
            int nH = _PicBox[index].Height;
            double fMinScaleRatio = nH / (float)SrcImg.Height;
            if (nW / (float)SrcImg.Width < nH / (float)SrcImg.Height)
                fMinScaleRatio = nW / (float)SrcImg.Width;

            Image<Bgr, byte> cTempImg = SrcImg.Resize(fMinScaleRatio, Emgu.CV.CvEnum.Inter.Linear);

            if (_BackBmp[index] != null)
                _BackBmp[index].Dispose();
            _BackBmp[index] = cTempImg.ToBitmap();
            cTempImg.Dispose();
            _PicBox[index].CreateGraphics().DrawImageUnscaled(_BackBmp[index], 0, 0);
        }

        public void DrawCircle(EPicBox PicBox, Image<Bgr, byte> SrcImg,  PointF CirclePoint)
        {
            if (SrcImg.Width <= 0 || SrcImg.Height <= 0)
                return;

            CircleF circle1 = new CircleF(CirclePoint, 10);
        
            if (CirclePoint.X > 0 && CirclePoint.Y > 0)
                SrcImg.Draw(circle1, new Bgr(0, 0, 255), 3);
           
            DrawDisplayImg(PicBox, SrcImg);
        }

        public void DrawMultiCircle(EPicBox PicBox, Image<Bgr, byte> SrcImg, PointF[] CirclePoint)
        {
            if (SrcImg.Width <= 0 || SrcImg.Height <= 0)
                return;

            if (CirclePoint != null)
            {
                for (int i = 0; i < CirclePoint.Length; i++)
                {
                    CircleF circle = new CircleF(CirclePoint[i], 10);
                    SrcImg.Draw(circle, new Bgr(0, 0, 255), 3);
                }
            }

            DrawDisplayImg(PicBox, SrcImg);
        }
        
        public void UpdateImage()
        {
            for(int i = 0; i < _PicBox.Length; i++)
            {
                if(_PicBox[i].Visible)
                {
                    _PicBox[i].Image = _BackBmp[i];
                    _PicBox[i].Refresh();
                }
            }  
        }

        #region ExposureTime
        private void ExposureTime_Scroll(object sender, EventArgs e)
        {
            for(int i = 0; i < _CCDExposureTrackBar.Length; i++)
            {
                if(sender == _CCDExposureTrackBar[i])
                    G.Vision.CameraCollection.SetExposure((ECamera)i, (double)_CCDExposureTrackBar[i].Value);
            }
            
        }

        public void ResetScroll()
        {
            for (int i = 0; i < (int)ECamera.Count; i++)
            {
                _CCDExposureTrackBar[i].Value = G.Vision.CameraCollection.GetExposure((ECamera)i, true);
                G.Vision.CameraCollection.SetExposureByDefault((ECamera)i);
            }
        }
        #endregion
    }
}
