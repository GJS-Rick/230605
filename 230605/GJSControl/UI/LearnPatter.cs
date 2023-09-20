using FileStreamLibrary;
using VisionLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace nsUI
{
    public partial class LearnPatter : Form
    {
        Thread _CameraThread;
        bool _End;
        FileManagerDef _FileStream;
        VisionManagerDef _Vision;
        cUIManagerDef _UI;
        CommonLibrary.CommonManagerDef _Comm;
        Image<Bgr, byte> _Cam1;
        Image<Bgr, byte> _Cam2;

        Image<Bgr, byte> _Cam1ForLearn;
        Image<Bgr, byte> _Cam2ForLearn;
        Rectangle _TemplateRec1 = new Rectangle(0, 0, 0, 0);
        Rectangle _TemplateRec2 = new Rectangle(0, 0, 0, 0);

        public LearnPatter(FileManagerDef fileStream, VisionManagerDef vision, cUIManagerDef ui, CommonLibrary.CommonManagerDef comm)
        {
            InitializeComponent();
            _CameraThread = new Thread(Live);
            _CameraThread.Priority = ThreadPriority.BelowNormal;
            _CameraThread.Start();
            _FileStream = fileStream;
            _Vision = vision;
            _UI = ui;
            _Comm = comm;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Live()
        {
            while(!_End)
            {
                RectangleF r1 = _Vision.GetPanelRectangleOnImage(
                             ECAMERA.Camera1,
                             new PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD1CeneterY]),
                             new PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterY]),
                             (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelXLength],
                             (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelYLength]);

                RectangleF r2 =  _Vision.GetPanelRectangleOnImage(
                             ECAMERA.Camera2,
                             new PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.CCD2CeneterY]),
                             new PointF((float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterX], (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelCenterY]),
                             (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelXLength],
                             (float)_FileStream.MachineData.ValueDouble[(int)EMachineDouble.PanelYLength]);
                //_Cam1?.Dispose();
                //_Cam2?.Dispose();
                _Cam1 = new Image<Bgr, byte>(_Vision.CameraCollection.GetWidth(ECAMERA.Camera1), _Vision.CameraCollection.GetHeight(ECAMERA.Camera1));
                _Cam2 = new Image<Bgr, byte>(_Vision.CameraCollection.GetWidth(ECAMERA.Camera2), _Vision.CameraCollection.GetHeight(ECAMERA.Camera2));
               
                
                _Vision.CameraCollection.CopyImage(ECAMERA.Camera1, _Cam1);
                _Vision.CameraCollection.CopyImage(ECAMERA.Camera2, _Cam2);

                _Cam1ForLearn = _Cam1.Clone();
                _Cam2ForLearn = _Cam2.Clone();

                int templateW = _FileStream.MachineData.ValueInt[(int)EMachineInt.AutoLearnWidth];
                int templateH = _FileStream.MachineData.ValueInt[(int)EMachineInt.AutoLearnHeight];
                int shift = _FileStream.MachineData.ValueInt[(int)EMachineInt.AutoLearnInsideShift];

            
                _TemplateRec2 = new Rectangle(Rectangle.Round(r2).X + shift, Rectangle.Round(r2).Y + shift, templateW, templateH);
                _TemplateRec1 = new Rectangle(Rectangle.Round(r1).Left + shift, Rectangle.Round(r1).Bottom - templateH - shift, templateW, templateH);

                CvInvoke.Rectangle(_Cam1, Rectangle.Round(_TemplateRec1), new MCvScalar(255, 0, 0), 3);
                CvInvoke.Rectangle(_Cam2, Rectangle.Round(_TemplateRec2), new MCvScalar(255, 0, 0), 3);

                CvInvoke.Rectangle(_Cam1, Rectangle.Round(r1), new MCvScalar(0, 0, 255), 3);
                CvInvoke.Rectangle(_Cam2, Rectangle.Round(r2), new MCvScalar(0, 0, 255), 3);

                CvInvoke.Rotate(_Cam1, _Cam1, Emgu.CV.CvEnum.RotateFlags.Rotate90CounterClockwise);
                CvInvoke.Rotate(_Cam2, _Cam2, Emgu.CV.CvEnum.RotateFlags.Rotate90CounterClockwise);


                zoomAndPanWindow1.SetImage(_Cam1.Mat);
                zoomAndPanWindow2.SetImage(_Cam2.Mat);
                

                Thread.Sleep(50);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //_UI.frmAreaCCD.SavePatterForLearnPatternUI();


            _Vision.ReadSetting();

            _Cam1ForLearn.ROI = _TemplateRec1;
            _Vision.LearnMatchModel(EAlignIndex.Align1, 0, _Cam1ForLearn, 0.8F);
            _Cam1ForLearn.ROI = Rectangle.Empty;

            String sErrorCode = "";
            if (!_FileStream.RecipeCollection.SaveCurrentRecipe(_Comm.Login.GetCurrentName()))
            {
                MessageBox.Show(sErrorCode);
            }
            if (!_Vision.MatchCollection.Save(_FileStream.RecipeCollection.GetCurrentRecipePath()))
            {
                MessageBox.Show("Match File Save Error");
                return;
            }
            _Cam2ForLearn.ROI = _TemplateRec2;
            _Vision.LearnMatchModel(EAlignIndex.Align1, 1, _Cam2ForLearn, 0.8F);
            _Cam2ForLearn.ROI = Rectangle.Empty;

            sErrorCode = "";
            if (!_FileStream.RecipeCollection.SaveCurrentRecipe(_Comm.Login.GetCurrentName()))
            {
                MessageBox.Show(sErrorCode);
            }
            if (!_Vision.MatchCollection.Save(_FileStream.RecipeCollection.GetCurrentRecipePath()))
            {
                MessageBox.Show("Match File Save Error");
                return;
            }
            DialogResult = DialogResult.OK;
            _End = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _End = true;
            Close();
        }

        public virtual DialogResult ShowDialog(ref DialogResult result)
        {
            ShowDialog();
            result = DialogResult;
            return DialogResult;
        }
    }
}
