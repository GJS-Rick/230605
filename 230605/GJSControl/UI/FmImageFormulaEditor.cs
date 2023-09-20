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
using Emgu.CV;
using Emgu.CV.Structure;

namespace nsUI
{
    public partial class FmImageFormulaEditor : Form
    {
        FileStreamLibrary.IniFile _Ini;
        List<string> _FormulaNames = new List<string>();
        List<string> _Formulas = new List<string>();
        VisionLibrary.VisionManagerDef _Vision;

        DataTable _FormulaTable = new DataTable();
        string _IniPath = nsUI.FmMain.GetSystemDirPath() + "\\Formula.ini";

        Mat _SelectedSample = new Mat();
        Mat _SelectedTemplate = new Mat();

        Mat _Sample = new Mat();
        public FmImageFormulaEditor(VisionLibrary.VisionManagerDef vision)
        {
            InitializeComponent();
            buttonTestImageWithFormula.Enabled = false;
            _Ini = new FileStreamLibrary.IniFile(_IniPath, true);
            _Vision = vision;
            //更新DataFridView
            _FormulaNames.AddRange(_Ini.GetKey("Formula"));

            for (int i = 0; i < _FormulaNames.Count; i++)
                _Formulas.Add(_Ini.ReadStr("Formula", _FormulaNames[i], ""));

            _FormulaTable.Columns.Add("Name", typeof(string));
            _FormulaTable.Columns.Add("Formula", typeof(string));
            for (int i = 0; i < _Formulas.Count; i++)
                _FormulaTable.Rows.Add(_FormulaNames[i], _Formulas[i]);
            dataGridViewFormula.DataSource = _FormulaTable;

            //更新ComboBox
            comboBoxSample.Items.Clear();
            for (int i = 0; i < (int)VisionLibrary.ECamera.Count; i++)
                comboBoxSample.Items.Add(((VisionLibrary.ECamera)i).ToString());

            
            _Ini.FileClose();

        }
        private bool CheckFormulaFormat(string formula)
        {
            char[] allowedChar = new char[] { 
                'B', 'G', 'R', 'b', 'g', 'r', '+', '-', '*', '/', '(', ')',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            bool result = true;

            for(int i = 0; i < 255; i++)
            {
                char c = (char)i;
                if (allowedChar.Contains(c))
                    continue;
                if (formula.Contains(c))
                    result = false;
            }
            return result;
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAddFormula_Click(object sender, EventArgs e)
        {
            if (textBoxFormula.Text == string.Empty || 
                textBoxFormulaName.Text == string.Empty || 
                _FormulaNames.Contains(textBoxFormulaName.Text) ||
                !CheckFormulaFormat(textBoxFormula.Text))
            {
                MessageBox.Show("Formula format error.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Mat mat = new Mat(100,100,Emgu.CV.CvEnum.DepthType.Cv8S, 3);
                mat.SetTo(new MCvScalar(0, 0, 0));
                VisionLibrary.Process.ImageArithmetic(mat, textBoxFormula.Text, out mat);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                
            _Ini = new FileStreamLibrary.IniFile(_IniPath, false);
            _FormulaTable.Rows.Add(textBoxFormulaName.Text, textBoxFormula.Text);
            _FormulaNames.Add(textBoxFormulaName.Text);
            _Formulas.Add(textBoxFormula.Text);
            _Ini.WriteStr("Formula", textBoxFormulaName.Text, textBoxFormula.Text);
            _Ini.FileClose();
                
        }

        private void buttonDeleteFormula_Click(object sender, EventArgs e)
        {
            int index;
            if(dataGridViewFormula.SelectedRows.Count > 0)
                index = dataGridViewFormula.SelectedRows[0].Index;
            else
            {
                MessageBox.Show("Please select a formula first.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                File.Create(_IniPath, 1024).Dispose();

                _FormulaTable.Rows.RemoveAt(index);
                _FormulaNames.RemoveAt(index);
                _Formulas.RemoveAt(index);
                _Ini = new FileStreamLibrary.IniFile(_IniPath, false);
                for (int i = 0; i < _FormulaNames.Count; i++)
                    _Ini.WriteStr("Formula", _FormulaNames[i], _Formulas[i]);
                _Ini.FileClose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonTestImageWithFormula_Click(object sender, EventArgs e)
        {
            string formula;
            if (checkBoxUseSelectedFormula.Checked)
            {
                int index = dataGridViewFormula.SelectedRows[0].Index;
                formula = _Formulas[index];
                if (dataGridViewFormula.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a formula first.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
                formula = textBoxFormula.Text;

         
          

            try
            {
                Mat mat = new Mat();
                VisionLibrary.Process.ImageArithmetic(_Sample, formula, out mat);
                pictureBox1.Image = mat.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Image<Bgr, byte> image;
            //if (radioButtonSample.Checked)
            //{
            //    image = new Image<Bgr, byte>(
            //        _Vision.CameraCollection.GetWidth((VisionLibrary.ECAMERA)comboBoxSample.SelectedIndex),
            //        _Vision.CameraCollection.GetHeight((VisionLibrary.ECAMERA)comboBoxSample.SelectedIndex));
            //    _Vision.CameraCollection.CopyImage((VisionLibrary.ECAMERA)comboBoxSample.SelectedIndex, image);


            //    //VisionLibrary.VisionManagerDef.ImageArithmetic(image.Mat, _Formulas[index], out _SelectedSample);
            //    pictureBox1.Image = _SelectedSample.ToBitmap();
            //}

            //if (radioButtonTemplate.Checked)
            //{
            //    image = new Image<Bgr, byte>(
            //       _Vision.MatchCollection.GetMatch((VisionLibrary.EMATCH)comboBoxSample.SelectedIndex).GetWidth(),
            //       _Vision.MatchCollection.GetMatch((VisionLibrary.EMATCH)comboBoxSample.SelectedIndex).GetHeight());

            //    //image = _Vision.MatchCollection.GetMatch((VisionLibrary.EMATCH)comboBoxSample.SelectedIndex).GetImage();

            //    //image = _Vision.MatchCollection.GetMatch((VisionLibrary.EMATCH)comboBoxSample.SelectedIndex).GetImage();
            //}

        }

        private void buttonOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;

            try
            {
                _Sample = CvInvoke.Imread(filePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            buttonTestImageWithFormula.Enabled = true;
            pictureBox1.Image = _Sample.ToBitmap();
        }
    }
}
