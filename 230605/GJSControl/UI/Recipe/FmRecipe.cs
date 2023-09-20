using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileStreamLibrary;
using CommonLibrary;
using VisionLibrary;
using Microsoft.VisualBasic;
using GJSControl.UI;
using Emgu.CV.Structure;

namespace nsUI
{
    public partial class FmRecipe : Form
    {
        private int _DGVTableChangeRows;
        private string _RecipeName;

        public FmRecipe()
        {
            InitializeComponent();
            vSetClickEvnet();
        }

        private void ShowNumKeyboard(object sender, EventArgs e)
        {
            FmNumKeyboard k = new FmNumKeyboard((NumericUpDown)sender);
            k.StartPosition = FormStartPosition.CenterScreen;
            k.ShowDialog();
        }

        #region 依數值是否改變而變化控件顏色
        private void CtlColorByValue(NumericUpDown eCtl, double dValue)
        {
            if ((double)eCtl.Value != dValue)
                eCtl.BackColor = Color.LightPink;
            else
                eCtl.BackColor = SystemColors.Window;
        }
        private void CtlColorByValue(CheckBox eCtl, bool bValue)
        {
            if (eCtl.Checked != bValue)
                eCtl.BackColor = Color.LightPink;
            else
                eCtl.BackColor = SystemColors.Window;
        }
        private void CtlColorByValue(ComboBox eCtl, int iValue)
        {
            if (eCtl.SelectedIndex != iValue)
                eCtl.BackColor = Color.LightPink;
            else
                eCtl.BackColor = SystemColors.Window;
        }
        #endregion

        #region 上Pin A1 A2
        private void ShowRBtn()
        {
            RBtn_A1.Visible = true;
            RBtn_A2.Visible = true;
        }
        private void HideRBtn()
        {
            RBtn_A1.Visible = false;
            RBtn_A2.Visible = false;
        }
        #endregion

        private void BtnModify_Click(object sender, EventArgs e)
        {
            if (DGVTable.SelectedRows.Count == 1)
            {
                string name = DGVTable.SelectedRows[0].Cells[0].Value.ToString();
                if (MessageBox.Show("確定修改:" + name, "修改料號", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    #region Basic
                    #region PCB長
                    if (!double.TryParse(NumUD_BoardHigh.Value.ToString(), out double BoardH))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region PCB樣板長
                    if (!double.TryParse(NumUD_BoardHigh_Standard.Value.ToString(), out double BoardSH))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region PCB寬
                    if (!double.TryParse(NumUD_BoardWidth.Value.ToString(), out double BoardW))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region PCB樣板寬
                    if (!double.TryParse(NumUD_BoardWidth_Standard.Value.ToString(), out double BoardSW))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region PCB板厚
                    if (!double.TryParse(NumUD_BoardThickness.Value.ToString(), out double BoardThickness))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region PCB最大最小尺寸
                    if (!double.TryParse(NumUD_BoardHighMax.Value.ToString(), out double BoardHMax))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardWidthMax.Value.ToString(), out double BoardWMax))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardHighMin.Value.ToString(), out double BoardHMin))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardWidthMin.Value.ToString(), out double BoardWMin))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region Pin孔距離
                    if (!double.TryParse(NumUD_BoardHoleDis_L.Value.ToString(), out double BoardHoleDis_L))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardHoleDis_UnL.Value.ToString(), out double BoardHoleDis_UnL))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region Pin孔樣板距離
                    if (!double.TryParse(NumUD_BoardHoleDis_Standard_L.Value.ToString(), out double BoardHoleDis_SL))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardHoleDis_Standard_UnL.Value.ToString(), out double BoardHoleDis_SUnL))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 防呆孔距離
                    if (!double.TryParse(NumUD_BoardFoolHoleDis.Value.ToString(), out double FoolHoldPin))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardFoolHoleDis_Standard.Value.ToString(), out double FoolHoldPinS))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region Pin孔與板側邊距
                    if (!double.TryParse(NumUD_BoardFoolHoleDis_A1.Value.ToString(), out double A1Dis))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardFoolHoleDis_A2.Value.ToString(), out double A2Dis))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region Pin孔與板側邊距(樣板)
                    if (!double.TryParse(NumUD_BoardFoolHoleDis_Standard_A1.Value.ToString(), out double A1SDis))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!double.TryParse(NumUD_BoardFoolHoleDis_Standard_A2.Value.ToString(), out double A2SDis))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 疊板方式
                    if (!int.TryParse(Cmbox_BoardStackingMethod.SelectedIndex.ToString(), out int BoardStackingMethod))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 啟用防呆孔檢測
                    int CheckFoolHoleEnable = 0;
                    if (ChBox_CheckFoolHole.Checked)
                        CheckFoolHoleEnable = 1;
                    #endregion
                    #region 最小出板時間
                    if (!double.TryParse(NumUD_ExportTimeMin.Value.ToString(), out double ExportTimeMin))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 鋁板厚度
                    if (!double.TryParse(NumUD_AlBoardThickness.Value.ToString(), out double AlBoardThickness))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 底板厚度
                    if (!double.TryParse(NumUD_UnilateBoardThickness.Value.ToString(), out double UnilateBoardThickness))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 脹縮值
                    if (!double.TryParse(NumUD_Tolerance.Value.ToString(), out double BoardTolerance))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 每疊PCB數
                    if (!int.TryParse(NumUD_PCBNumBySet_L.Value.ToString(), out int PCBNumBySet_L))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    if (!int.TryParse(NumUD_PCBNumBySet_UnL.Value.ToString(), out int PCBNumBySet_UnL))
                    {
                        AlarmTextDisplay.Add((int)AlarmCode.Machine_ParmError, AlarmType.Alarm);
                        return;
                    }
                    #endregion
                    #region 基準側
                    int PinHoldBasic = 0;
                    if (RBtn_A1.Checked && !RBtn_A2.Checked)
                        PinHoldBasic = 1;
                    else if (!RBtn_A1.Checked && RBtn_A2.Checked)
                        PinHoldBasic = 2;
                    #endregion

                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHeight, BoardH, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHeight_Standard, BoardSH, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardWidth, BoardW, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardWidth_Standard, BoardSW, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHeightMax, BoardHMax, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardWidthMax, BoardWMax, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHeightMin, BoardHMin, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardWidthMin, BoardWMin, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardThickness, BoardThickness, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHoleDistance_L, BoardHoleDis_L, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHoleDistance_L_Standard, BoardHoleDis_SL, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHoleDistance_UnL, BoardHoleDis_UnL, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardHoleDistance_UnL_Standard, BoardHoleDis_SUnL, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance, FoolHoldPin, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance_Standard, FoolHoldPinS, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance_A1, A1Dis, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance_A2, A2Dis, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance_A1_Standard, A1SDis, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardFoolHoleDistance_A2_Standard, A2SDis, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeInt.BoardStackingMethod, BoardStackingMethod, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeInt.BoardFoolHoleEnable, CheckFoolHoleEnable, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.ExportTime_Min, ExportTimeMin, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.AlBoardThickness, AlBoardThickness, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.UnilateBoardThickness, UnilateBoardThickness, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeDouble.BoardTolerance, BoardTolerance, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeInt.PCBNumBySet_L, PCBNumBySet_L, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeInt.PCBNumBySet_UnL, PCBNumBySet_UnL, G.Comm.Login.GetCurrentName());
                    G.FS.RecipeCollection.SetRecipeContent(name, ERecipeInt.PinHoldBasic, PinHoldBasic, G.Comm.Login.GetCurrentName());
                    #endregion
                }

                vRefreshUI();
            }
            else
                MessageBox.Show("請選擇料號");
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("新增Recipe", "新增", "Default", 50, 50);

            if (MessageBox.Show("確定新增:" + input, "增加料號", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                G.FS.RecipeCollection.Add(input, G.Comm.Login.GetCurrentName());
                vRefreshUI();
            }
        }

        private void FmRecipe_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                vRefreshUI();
            }
        }

        private void vRefreshUI()
        {
            DGVTable.Rows.Clear();
            DGVTable.RowCount = G.FS.RecipeCollection.GetRecipeNum();
            String[] sNames = G.FS.RecipeCollection.GetRecipeNames();

            for (int i = 0; i < DGVTable.RowCount; i++)
            {
                DGVTable.Rows[i].Cells[0].Value = sNames[i];
                string user = "";
                string date = "";

                G.FS.RecipeCollection.GetRecipeInfo(sNames[i], ref date, ref user);
                DGVTable.Rows[i].Cells[1].Value = date;
                DGVTable.Rows[i].Cells[2].Value = user;
            }

            String sName = "";
            String sNotice = "";
            G.FS.RecipeCollection.GetRecipeInfo(ref sName, ref sNotice);
            LblRecipeName.Text = sName;

            DGVTable.CurrentCell = null;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (DGVTable.CurrentRow == null || DGVTable.CurrentRow.Index < 0 || DGVTable.CurrentRow.Index >= G.FS.RecipeCollection.GetRecipeNum())
                return;

            String sErrorCode = "";
            if (!G.FS.RecipeCollection.Load(DGVTable.Rows[DGVTable.CurrentRow.Index].Cells[0].Value.ToString(), ref sErrorCode))
            {
                vRefreshUI();
                return;
            }

            vRefreshUI();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DGVTable.CurrentRow == null || DGVTable.CurrentRow.Index < 0 || DGVTable.CurrentRow.Index >= G.FS.RecipeCollection.GetRecipeNum())
                return;

            String sErrorCode = "";
            if (!G.FS.RecipeCollection.Delete(DGVTable.Rows[DGVTable.CurrentRow.Index].Cells[0].Value.ToString(), ref sErrorCode))
            {
                vRefreshUI();
                return;
            }

            vRefreshUI();
        }

        private void DGVTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            _DGVTableChangeRows = e.RowIndex;
            _RecipeName = DGVTable.Rows[e.RowIndex].Cells[0].Value.ToString();

            if (String.IsNullOrEmpty(_RecipeName))
                return;

            #region Basic
            ResetBoardSizeCtlMaxMinValue();
            #region PCB長
            double boardH = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeight);
            if ((decimal)boardH < NumUD_BoardHigh.Minimum)
                NumUD_BoardHigh.Value = NumUD_BoardHigh.Minimum;
            else if ((decimal)boardH > NumUD_BoardHigh.Maximum)
                NumUD_BoardHigh.Value = NumUD_BoardHigh.Maximum;
            else
                NumUD_BoardHigh.Value = (decimal)boardH;
            #endregion
            #region PCB樣板長
            double boardSH = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeight_Standard);
            if ((decimal)boardSH < NumUD_BoardHigh_Standard.Minimum)
                NumUD_BoardHigh_Standard.Value = NumUD_BoardHigh_Standard.Minimum;
            else if ((decimal)boardSH > NumUD_BoardHigh_Standard.Maximum)
                NumUD_BoardHigh_Standard.Value = NumUD_BoardHigh_Standard.Maximum;
            else
                NumUD_BoardHigh_Standard.Value = (decimal)boardSH;
            #endregion
            #region PCB寬
            double boardW = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidth);
            if ((decimal)boardW < NumUD_BoardWidth.Minimum)
                NumUD_BoardWidth.Value = NumUD_BoardWidth.Minimum;
            else if ((decimal)boardW > NumUD_BoardWidth.Maximum)
                NumUD_BoardWidth.Value = NumUD_BoardWidth.Maximum;
            else
                NumUD_BoardWidth.Value = (decimal)boardW;
            #endregion
            #region PCB樣板寬
            double boardSW = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidth_Standard);
            if ((decimal)boardSW < NumUD_BoardWidth_Standard.Minimum)
                NumUD_BoardWidth_Standard.Value = NumUD_BoardWidth_Standard.Minimum;
            else if ((decimal)boardSW > NumUD_BoardWidth_Standard.Maximum)
                NumUD_BoardWidth_Standard.Value = NumUD_BoardWidth_Standard.Maximum;
            else
                NumUD_BoardWidth_Standard.Value = (decimal)boardSW;
            #endregion
            #region PCB最大最小板尺寸
            double boardHMax = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMax);
            if ((decimal)boardHMax < NumUD_BoardHighMax.Minimum)
                NumUD_BoardHighMax.Value = NumUD_BoardHighMax.Minimum;
            else if ((decimal)boardHMax > NumUD_BoardHighMax.Maximum)
                NumUD_BoardHighMax.Value = NumUD_BoardHighMax.Maximum;
            else
                NumUD_BoardHighMax.Value = (decimal)boardHMax;
            double boardWMax = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMax);
            if ((decimal)boardWMax < NumUD_BoardWidthMax.Minimum)
                NumUD_BoardWidthMax.Value = NumUD_BoardWidthMax.Minimum;
            else if ((decimal)boardWMax > NumUD_BoardWidthMax.Maximum)
                NumUD_BoardWidthMax.Value = NumUD_BoardWidthMax.Maximum;
            else
                NumUD_BoardWidthMax.Value = (decimal)boardWMax;
            double boardHMin = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMin);
            if ((decimal)boardHMin < NumUD_BoardHighMin.Minimum)
                NumUD_BoardHighMin.Value = NumUD_BoardHighMin.Minimum;
            else if ((decimal)boardHMin > NumUD_BoardHighMin.Maximum)
                NumUD_BoardHighMin.Value = NumUD_BoardHighMin.Maximum;
            else
                NumUD_BoardHighMin.Value = (decimal)boardHMin;
            double boardWMin = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMin);
            if ((decimal)boardWMin < NumUD_BoardWidthMin.Minimum)
                NumUD_BoardWidthMin.Value = NumUD_BoardWidthMin.Minimum;
            else if ((decimal)boardWMin > NumUD_BoardWidthMin.Maximum)
                NumUD_BoardWidthMin.Value = NumUD_BoardWidthMin.Maximum;
            else
                NumUD_BoardWidthMin.Value = (decimal)boardWMin;
            #endregion
            ResetBoardOtherCtlMaxMinValue();
            #region PCB板厚
            double BoardThickness = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardThickness);
            if ((decimal)BoardThickness < NumUD_BoardThickness.Minimum)
                NumUD_BoardThickness.Value = NumUD_BoardThickness.Minimum;
            else if ((decimal)BoardThickness > NumUD_BoardThickness.Maximum)
                NumUD_BoardThickness.Value = NumUD_BoardThickness.Maximum;
            else
                NumUD_BoardThickness.Value = (decimal)BoardThickness;
            #endregion
            #region Pin孔距離
            double PinDis_L = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_L);
            if ((decimal)PinDis_L < NumUD_BoardHoleDis_L.Minimum)
                NumUD_BoardHoleDis_L.Value = NumUD_BoardHoleDis_L.Minimum;
            else if ((decimal)PinDis_L > NumUD_BoardHoleDis_L.Maximum)
                NumUD_BoardHoleDis_L.Value = NumUD_BoardHoleDis_L.Maximum;
            else
                NumUD_BoardHoleDis_L.Value = (decimal)PinDis_L;
            double PinDis_UnL = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_UnL);
            if ((decimal)PinDis_UnL < NumUD_BoardHoleDis_UnL.Minimum)
                NumUD_BoardHoleDis_UnL.Value = NumUD_BoardHoleDis_UnL.Minimum;
            else if ((decimal)PinDis_UnL > NumUD_BoardHoleDis_UnL.Maximum)
                NumUD_BoardHoleDis_UnL.Value = NumUD_BoardHoleDis_UnL.Maximum;
            else
                NumUD_BoardHoleDis_UnL.Value = (decimal)PinDis_UnL;
            #endregion
            #region Pin孔樣板距離
            double PinSDis_L = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_L_Standard);
            if ((decimal)PinSDis_L < NumUD_BoardHoleDis_Standard_L.Minimum)
                NumUD_BoardHoleDis_Standard_L.Value = NumUD_BoardHoleDis_Standard_L.Minimum;
            else if ((decimal)PinSDis_L > NumUD_BoardHoleDis_Standard_L.Maximum)
                NumUD_BoardHoleDis_Standard_L.Value = NumUD_BoardHoleDis_Standard_L.Maximum;
            else
                NumUD_BoardHoleDis_Standard_L.Value = (decimal)PinSDis_L;
            double PinSDis_UnL = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_UnL_Standard);
            if ((decimal)PinSDis_UnL < NumUD_BoardHoleDis_Standard_UnL.Minimum)
                NumUD_BoardHoleDis_Standard_UnL.Value = NumUD_BoardHoleDis_Standard_UnL.Minimum;
            else if ((decimal)PinSDis_UnL > NumUD_BoardHoleDis_Standard_UnL.Maximum)
                NumUD_BoardHoleDis_Standard_UnL.Value = NumUD_BoardHoleDis_Standard_UnL.Maximum;
            else
                NumUD_BoardHoleDis_Standard_UnL.Value = (decimal)PinSDis_UnL;
            #endregion
            #region 防呆孔距離
            double FoolHoldPin = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance);
            if ((decimal)FoolHoldPin < NumUD_BoardFoolHoleDis.Minimum)
                NumUD_BoardFoolHoleDis.Value = NumUD_BoardFoolHoleDis.Minimum;
            else if ((decimal)FoolHoldPin > NumUD_BoardFoolHoleDis.Maximum)
                NumUD_BoardFoolHoleDis.Value = NumUD_BoardFoolHoleDis.Maximum;
            else
                NumUD_BoardFoolHoleDis.Value = (decimal)FoolHoldPin;
            double FoolHoldPinS = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_Standard);
            if ((decimal)FoolHoldPinS < NumUD_BoardFoolHoleDis_Standard.Minimum)
                NumUD_BoardFoolHoleDis_Standard.Value = NumUD_BoardFoolHoleDis_Standard.Minimum;
            else if ((decimal)FoolHoldPinS > NumUD_BoardFoolHoleDis_Standard.Maximum)
                NumUD_BoardFoolHoleDis_Standard.Value = NumUD_BoardFoolHoleDis_Standard.Maximum;
            else
                NumUD_BoardFoolHoleDis_Standard.Value = (decimal)FoolHoldPinS;
            #endregion
            #region Pin孔與板側邊距
            double A1Dis = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A1);
            if ((decimal)A1Dis < NumUD_BoardFoolHoleDis_A1.Minimum)
                NumUD_BoardFoolHoleDis_A1.Value = NumUD_BoardFoolHoleDis_A1.Minimum;
            else if ((decimal)A1Dis > NumUD_BoardFoolHoleDis_A1.Maximum)
                NumUD_BoardFoolHoleDis_A1.Value = NumUD_BoardFoolHoleDis_A1.Maximum;
            else
                NumUD_BoardFoolHoleDis_A1.Value = (decimal)A1Dis;
            double A2Dis = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2);
            if ((decimal)A2Dis < NumUD_BoardFoolHoleDis_A2.Minimum)
                NumUD_BoardFoolHoleDis_A2.Value = NumUD_BoardFoolHoleDis_A2.Minimum;
            else if ((decimal)A2Dis > NumUD_BoardFoolHoleDis_A2.Maximum)
                NumUD_BoardFoolHoleDis_A2.Value = NumUD_BoardFoolHoleDis_A2.Maximum;
            else
                NumUD_BoardFoolHoleDis_A2.Value = (decimal)A2Dis;
            #endregion
            #region Pin孔與板側邊距(樣板)
            double A1SDis = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A1_Standard);
            if ((decimal)A1SDis < NumUD_BoardFoolHoleDis_Standard_A1.Minimum)
                NumUD_BoardFoolHoleDis_Standard_A1.Value = NumUD_BoardFoolHoleDis_Standard_A1.Minimum;
            else if ((decimal)A1SDis > NumUD_BoardFoolHoleDis_Standard_A1.Maximum)
                NumUD_BoardFoolHoleDis_Standard_A1.Value = NumUD_BoardFoolHoleDis_Standard_A1.Maximum;
            else
                NumUD_BoardFoolHoleDis_Standard_A1.Value = (decimal)A1SDis;
            double A2SDis = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2_Standard);
            if ((decimal)A2SDis < NumUD_BoardFoolHoleDis_Standard_A2.Minimum)
                NumUD_BoardFoolHoleDis_Standard_A2.Value = NumUD_BoardFoolHoleDis_Standard_A2.Minimum;
            else if ((decimal)A2SDis > NumUD_BoardFoolHoleDis_Standard_A2.Maximum)
                NumUD_BoardFoolHoleDis_Standard_A2.Value = NumUD_BoardFoolHoleDis_Standard_A2.Maximum;
            else
                NumUD_BoardFoolHoleDis_Standard_A2.Value = (decimal)A2SDis;
            #endregion
            #region 疊板方式
            int BoardStackingMethod = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.BoardStackingMethod);
            if (Cmbox_BoardStackingMethod.Items.Count > 0)
            {
                if (BoardStackingMethod >= 0 && BoardStackingMethod < Cmbox_BoardStackingMethod.Items.Count)
                    Cmbox_BoardStackingMethod.SelectedIndex = BoardStackingMethod;
                else
                    Cmbox_BoardStackingMethod.SelectedIndex = 0;
            }
            #endregion
            #region 啟用防呆孔檢測
            ChBox_CheckFoolHole.Checked = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.BoardFoolHoleEnable) > 0;
            #endregion
            #region 最小出板時間
            double ExportTime_Min = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.ExportTime_Min);
            if ((decimal)ExportTime_Min < NumUD_ExportTimeMin.Minimum)
                NumUD_ExportTimeMin.Value = NumUD_ExportTimeMin.Minimum;
            else if ((decimal)ExportTime_Min > NumUD_ExportTimeMin.Maximum)
                NumUD_ExportTimeMin.Value = NumUD_ExportTimeMin.Maximum;
            else
                NumUD_ExportTimeMin.Value = (decimal)ExportTime_Min;
            #endregion
            #region 鋁板厚度
            double AlBoardThickness = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.AlBoardThickness);
            if ((decimal)AlBoardThickness < NumUD_AlBoardThickness.Minimum)
                NumUD_AlBoardThickness.Value = NumUD_AlBoardThickness.Minimum;
            else if ((decimal)AlBoardThickness > NumUD_AlBoardThickness.Maximum)
                NumUD_AlBoardThickness.Value = NumUD_AlBoardThickness.Maximum;
            else
                NumUD_AlBoardThickness.Value = (decimal)AlBoardThickness;
            #endregion
            #region 底板厚度
            double UnilateBoardThickness = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.UnilateBoardThickness);
            if ((decimal)UnilateBoardThickness < NumUD_UnilateBoardThickness.Minimum)
                NumUD_UnilateBoardThickness.Value = NumUD_UnilateBoardThickness.Minimum;
            else if ((decimal)UnilateBoardThickness > NumUD_UnilateBoardThickness.Maximum)
                NumUD_UnilateBoardThickness.Value = NumUD_UnilateBoardThickness.Maximum;
            else
                NumUD_UnilateBoardThickness.Value = (decimal)UnilateBoardThickness;
            #endregion
            #region 脹縮值
            double BoardTolerance = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardTolerance);
            if ((decimal)BoardTolerance < NumUD_Tolerance.Minimum)
                NumUD_Tolerance.Value = NumUD_Tolerance.Minimum;
            else if ((decimal)BoardTolerance > NumUD_Tolerance.Maximum)
                NumUD_Tolerance.Value = NumUD_Tolerance.Maximum;
            else
                NumUD_Tolerance.Value = (decimal)BoardTolerance;
            #endregion
            #region 每疊PCB數
            double PCBNumBySet_L = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PCBNumBySet_L);
            if ((decimal)PCBNumBySet_L < NumUD_PCBNumBySet_L.Minimum)
                NumUD_PCBNumBySet_L.Value = NumUD_PCBNumBySet_L.Minimum;
            else if ((decimal)PCBNumBySet_L > NumUD_PCBNumBySet_L.Maximum)
                NumUD_PCBNumBySet_L.Value = NumUD_PCBNumBySet_L.Maximum;
            else
                NumUD_PCBNumBySet_L.Value = (decimal)PCBNumBySet_L;

            double PCBNumBySet_UnL = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PCBNumBySet_UnL);
            if ((decimal)PCBNumBySet_UnL < NumUD_PCBNumBySet_UnL.Minimum)
                NumUD_PCBNumBySet_UnL.Value = NumUD_PCBNumBySet_UnL.Minimum;
            else if ((decimal)PCBNumBySet_UnL > NumUD_PCBNumBySet_UnL.Maximum)
                NumUD_PCBNumBySet_UnL.Value = NumUD_PCBNumBySet_UnL.Maximum;
            else
                NumUD_PCBNumBySet_UnL.Value = (decimal)PCBNumBySet_UnL;
            #endregion
            #region 基準側
            int PinHoldBasic = G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PinHoldBasic);
            if (PinHoldBasic <= 0)
            {
                RBtn_A1.Checked = true;
                RBtn_A2.Checked = false;
            }
            else
            {
                RBtn_A1.Checked = false;
                RBtn_A2.Checked = true;
            }
            #endregion
            #endregion
        }

        /// <summary>設定板尺寸最大最小值</summary>
        private void ResetBoardSizeCtlMaxMinValue()
        {
            decimal _BoardHMax = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMax);
            decimal _BoardHMin = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMin);
            decimal _BoardWMax = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMax);
            decimal _BoardWMin = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMin);

            NumUD_BoardHigh.Maximum = _BoardHMax;
            NumUD_BoardHigh.Minimum = _BoardHMin;
            NumUD_BoardWidth.Maximum = _BoardWMax;
            NumUD_BoardWidth.Minimum = _BoardWMin;

            NumUD_BoardHigh_Standard.Maximum = _BoardHMax;
            NumUD_BoardHigh_Standard.Minimum = _BoardHMin;
            NumUD_BoardWidth_Standard.Maximum = _BoardWMax;
            NumUD_BoardWidth_Standard.Minimum = _BoardWMin;
        }
        /// <summary>設定板其他控件最大最小值</summary>
        private void ResetBoardOtherCtlMaxMinValue()
        {
            decimal _BoardH = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeight);
            decimal _BoardW = (decimal)G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidth);

            NumUD_BoardHoleDis_L.Maximum = _BoardW;
            NumUD_BoardHoleDis_L.Minimum = _BoardW - 40;
            NumUD_BoardHoleDis_Standard_L.Maximum = _BoardW;
            NumUD_BoardHoleDis_Standard_L.Minimum = _BoardW - 40;

            NumUD_BoardHoleDis_UnL.Maximum = _BoardW;
            NumUD_BoardHoleDis_UnL.Minimum = _BoardW - 40;
            NumUD_BoardHoleDis_Standard_UnL.Maximum = _BoardW;
            NumUD_BoardHoleDis_Standard_UnL.Minimum = _BoardW - 40;

            NumUD_BoardFoolHoleDis.Maximum = _BoardH / 2;
            NumUD_BoardFoolHoleDis.Minimum = 0;
            NumUD_BoardFoolHoleDis_Standard.Maximum = _BoardH / 2;
            NumUD_BoardFoolHoleDis_Standard.Minimum = 0;

            NumUD_BoardFoolHoleDis_A1.Maximum = _BoardW / 2;
            NumUD_BoardFoolHoleDis_A1.Minimum = 0;
            NumUD_BoardFoolHoleDis_Standard_A1.Maximum = _BoardW / 2;
            NumUD_BoardFoolHoleDis_Standard_A1.Minimum = 0;

            NumUD_BoardFoolHoleDis_A2.Maximum = _BoardW / 2;
            NumUD_BoardFoolHoleDis_A2.Minimum = 0;
            NumUD_BoardFoolHoleDis_Standard_A2.Maximum = _BoardW / 2;
            NumUD_BoardFoolHoleDis_Standard_A2.Minimum = 0;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            LogDef.Add(
                ELogFileName.Operate,
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ((Button)sender).Name.ToString() + " Click");
        }

        private void vSetClickEvnet()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                    ctrl.Click += new EventHandler(Btn_Click);
            }
        }

        private void FmRecipe_Load(object sender, EventArgs e)
        {
            #region 初始化
            this.Font = new Font("微軟正黑體", 11);
            DGVTable.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            for (int i = 0; i < DGVTable.ColumnCount; i++)
                DGVTable.Columns[i].Resizable = DataGridViewTriState.False;

            _DGVTableChangeRows = -1;
            _RecipeName = "";

            //更新上Pin疊料方式清單
            Cmbox_BoardStackingMethod.Items.Clear();
            Cmbox_BoardStackingMethod.Items.Add("正交疊板");
            Cmbox_BoardStackingMethod.Items.Add("扇形疊板");
            Cmbox_BoardStackingMethod.SelectedIndex = 0;
            //設定出料方向圖示
            SubPanel_Dir.BackgroundImage = GJSControl.Properties.Resources.LeftArrow;
            //設定起始板圖示
            SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.BasicBoard;
            //設定基本板頁面
            TabCtl_BasicSet.SelectedIndex = 0;

            HideRBtn();
            #endregion

            //this.Other.Parent = this.TabCtl_Recipe;//顯示
            //this.Other.Parent = null;//隱藏

            switch (G.Comm.MechanicalModel)
            {
                case EMechanicalModel.All:
                    #region TabCtl_Recipe
                    this.Board.Parent = this.TabCtl_Recipe;
                    this.Other.Parent = this.TabCtl_Recipe;
                    #endregion

                    #region TabCtl_BasicSet
                    this.Basic_Board.Parent = this.TabCtl_BasicSet;
                    this.Basic_LoadPin.Parent = this.TabCtl_BasicSet;
                    this.Basic_UnLoadPin.Parent = this.TabCtl_BasicSet;
                    #endregion

                    #region TabCtl_OtherSet
                    this.Other_Basic.Parent = this.TabCtl_OtherSet;
                    this.Other_LoadPin.Parent = this.TabCtl_OtherSet;
                    this.Other_UnLoadPin.Parent = this.TabCtl_OtherSet;
                    #endregion
                    break;
                case EMechanicalModel.Basic:
                    #region TabCtl_Recipe
                    this.Board.Parent = this.TabCtl_Recipe;
                    this.Other.Parent = this.TabCtl_Recipe;
                    #endregion

                    #region TabCtl_BasicSet
                    this.Basic_Board.Parent = this.TabCtl_BasicSet;
                    this.Basic_LoadPin.Parent = null;
                    this.Basic_UnLoadPin.Parent = null;
                    #endregion

                    #region TabCtl_OtherSet
                    this.Other_Basic.Parent = this.TabCtl_OtherSet;
                    this.Other_LoadPin.Parent = null;
                    this.Other_UnLoadPin.Parent = null;
                    #endregion
                    break;
                case EMechanicalModel.Load_Pin:
                    #region TabCtl_Recipe
                    this.Board.Parent = this.TabCtl_Recipe;
                    this.Other.Parent = this.TabCtl_Recipe;
                    #endregion

                    #region TabCtl_BasicSet
                    this.Basic_Board.Parent = this.TabCtl_BasicSet;
                    this.Basic_LoadPin.Parent = this.TabCtl_BasicSet;
                    this.Basic_UnLoadPin.Parent = null;
                    #endregion

                    #region TabCtl_OtherSet
                    this.Other_Basic.Parent = this.TabCtl_OtherSet;
                    this.Other_LoadPin.Parent = this.TabCtl_OtherSet;
                    this.Other_UnLoadPin.Parent = null;
                    #endregion
                    break;
                case EMechanicalModel.UnLoad_Pin:
                    #region TabCtl_Recipe
                    this.Board.Parent = this.TabCtl_Recipe;
                    this.Other.Parent = this.TabCtl_Recipe;
                    #endregion

                    #region TabCtl_BasicSet
                    this.Basic_Board.Parent = this.TabCtl_BasicSet;
                    this.Basic_LoadPin.Parent = null;
                    this.Basic_UnLoadPin.Parent = this.TabCtl_BasicSet;
                    #endregion

                    #region TabCtl_OtherSet
                    this.Other_Basic.Parent = this.TabCtl_OtherSet;
                    this.Other_LoadPin.Parent = null;
                    this.Other_UnLoadPin.Parent = this.TabCtl_OtherSet;
                    #endregion
                    break;
            }

            RecipeTimer.Interval = 100;
            RecipeTimer.Enabled = true;
        }

        private void TabCtl_BasicSet_Selected(object sender, EventArgs e)
        {
            switch (TabCtl_BasicSet.SelectedTab.Name)
            {
                case "Basic_Board":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.BasicBoard;
                    HideRBtn();
                    break;
                case "Basic_Board_Standard":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.BasicBoard;
                    HideRBtn();
                    break;
                case "Basic_LoadPin_Standard":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.Board_LodePin;
                    ShowRBtn();
                    break;
                case "Basic_LoadPin":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.Board_LodePin;
                    ShowRBtn();
                    break;
                case "Basic_UnLoadPin_Standard":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.Board_UnLodePin;
                    HideRBtn();
                    break;
                case "Basic_UnLoadPin":
                    SubPanel_Board_Basic.BackgroundImage = GJSControl.Properties.Resources.Board_UnLodePin;
                    HideRBtn();
                    break;
                default:
                    break;
            }
        }

        private void RecipeTimer_Tick(object sender, EventArgs e)
        {
            TabCtl_Recipe.Enabled = DGVTable.CurrentCell != null;

            #region Basic
            #region CtlColor
            if (_DGVTableChangeRows >= 0 && !String.IsNullOrEmpty(_RecipeName))
            {
                #region PCB板材Tab
                #region PCB
                CtlColorByValue(NumUD_BoardHigh, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeight));
                CtlColorByValue(NumUD_BoardWidth, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidth));
                CtlColorByValue(NumUD_BoardThickness, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardThickness));
                #endregion
                #region PCB樣板
                CtlColorByValue(NumUD_BoardHigh_Standard, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeight_Standard));
                CtlColorByValue(NumUD_BoardWidth_Standard, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidth_Standard));
                CtlColorByValue(NumUD_BoardHighMax, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMax));
                CtlColorByValue(NumUD_BoardWidthMax, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMax));
                CtlColorByValue(NumUD_BoardHighMin, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHeightMin));
                CtlColorByValue(NumUD_BoardWidthMin, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardWidthMin));
                #endregion
                #region Pin孔孔距
                CtlColorByValue(NumUD_BoardHoleDis_L, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_L));
                CtlColorByValue(NumUD_BoardHoleDis_UnL, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_UnL));
                CtlColorByValue(NumUD_BoardHoleDis_Standard_L, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_L_Standard));
                CtlColorByValue(NumUD_BoardHoleDis_Standard_UnL, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardHoleDistance_UnL_Standard));
                #endregion
                #region 防呆孔
                CtlColorByValue(NumUD_BoardFoolHoleDis, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance));
                CtlColorByValue(NumUD_BoardFoolHoleDis_Standard, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_Standard));
                #endregion
                #region A1側距離
                CtlColorByValue(NumUD_BoardFoolHoleDis_A1, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A1));
                CtlColorByValue(NumUD_BoardFoolHoleDis_Standard_A1, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A1_Standard));
                #endregion
                #region A2側距離
                CtlColorByValue(NumUD_BoardFoolHoleDis_A2, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2));
                CtlColorByValue(NumUD_BoardFoolHoleDis_Standard_A2, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2_Standard));
                #endregion
                #region A2側距離
                CtlColorByValue(NumUD_BoardFoolHoleDis_A2, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2));
                CtlColorByValue(NumUD_BoardFoolHoleDis_Standard_A2, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardFoolHoleDistance_A2_Standard));
                #endregion
                #region 疊板型式
                CtlColorByValue(Cmbox_BoardStackingMethod, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.BoardStackingMethod));
                #endregion
                #region 防呆孔
                CtlColorByValue(ChBox_CheckFoolHole, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.BoardFoolHoleEnable) > 0);
                #endregion
                #endregion
                #region 其他Tab
                #region 出板最小時間
                CtlColorByValue(NumUD_ExportTimeMin, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.ExportTime_Min));
                #endregion
                #region 鋁板板厚
                CtlColorByValue(NumUD_AlBoardThickness, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.AlBoardThickness));
                #endregion
                #region 底板板板厚
                CtlColorByValue(NumUD_UnilateBoardThickness, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.UnilateBoardThickness));
                #endregion
                #region 脹縮值
                CtlColorByValue(NumUD_Tolerance, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeDouble.BoardTolerance));
                #endregion
                #region 每疊PCB數
                CtlColorByValue(NumUD_PCBNumBySet_L, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PCBNumBySet_L));
                CtlColorByValue(NumUD_PCBNumBySet_UnL, G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PCBNumBySet_UnL));
                #endregion
                #endregion

                if (RBtn_A1.Checked && !RBtn_A2.Checked)
                {
                    if (G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PinHoldBasic) == 1)
                    {
                        RBtn_A1.BackColor = SystemColors.Window;
                        RBtn_A2.BackColor = SystemColors.Window;
                    }
                    else
                    {
                        RBtn_A1.BackColor = Color.LightPink;
                        RBtn_A2.BackColor = Color.LightPink;
                    }
                }
                else if (!RBtn_A1.Checked && RBtn_A2.Checked)
                {
                    if (G.FS.RecipeCollection.GetRecipeValue(_RecipeName, ERecipeInt.PinHoldBasic) == 2)
                    {
                        RBtn_A1.BackColor = SystemColors.Window;
                        RBtn_A2.BackColor = SystemColors.Window;
                    }
                    else
                    {
                        RBtn_A1.BackColor = Color.LightPink;
                        RBtn_A2.BackColor = Color.LightPink;
                    }
                }
            }
            #endregion
            #endregion

            #region Renew TabCtl_Recipe TabPage
            if ((int)G.Comm.Login.GetLevel() >= (int)ELoginLevel.Developer)
            {
                switch (G.Comm.MechanicalModel)
                {
                    case EMechanicalModel.All:
                        #region Basic
                        #region TabCtl_BasicSet
                        this.Basic_Board_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_LoadPin_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_UnLoadPin_Standard.Parent = this.TabCtl_BasicSet;
                        #endregion
                        #endregion
                        break;
                    case EMechanicalModel.Basic:
                        #region Basic
                        #region TabCtl_BasicSet
                        this.Basic_Board_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_LoadPin_Standard.Parent = null;
                        this.Basic_UnLoadPin_Standard.Parent = null;
                        #endregion
                        #endregion
                        break;
                    case EMechanicalModel.Load_Pin:
                        #region Basic
                        #region TabCtl_BasicSet
                        this.Basic_Board_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_LoadPin_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_UnLoadPin_Standard.Parent = null;
                        #endregion
                        #endregion
                        break;
                    case EMechanicalModel.UnLoad_Pin:
                        #region Basic
                        #region TabCtl_BasicSet
                        this.Basic_Board_Standard.Parent = this.TabCtl_BasicSet;
                        this.Basic_LoadPin_Standard.Parent = null;
                        this.Basic_UnLoadPin_Standard.Parent = this.TabCtl_BasicSet;
                        #endregion
                        #endregion
                        break;
                }
            }
            else
            {
                #region Basic
                #region TabCtl_BasicSet
                this.Basic_Board_Standard.Parent = null;
                this.Basic_LoadPin_Standard.Parent = null;
                this.Basic_UnLoadPin_Standard.Parent = null;
                #endregion
                #endregion
            }
            #endregion
        }

        private void Cbx_BoardStackingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BoardSizeRangeValueChanged(object sender, EventArgs e)
        {
            ResetBoardSizeCtlMaxMinValue();
            ResetBoardOtherCtlMaxMinValue();
        }
    }
}