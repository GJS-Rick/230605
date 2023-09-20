using System;

namespace nsUI
{
    partial class FmRecipe
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnModify = new System.Windows.Forms.Button();
            this.DGVTable = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.LblPartNo = new System.Windows.Forms.Label();
            this.LblRecipeName = new System.Windows.Forms.Label();
            this.LblRecipeTitle = new System.Windows.Forms.Label();
            this.TLP_1 = new System.Windows.Forms.TableLayoutPanel();
            this.TLP_2 = new System.Windows.Forms.TableLayoutPanel();
            this.TabCtl_Recipe = new System.Windows.Forms.TabControl();
            this.Board = new System.Windows.Forms.TabPage();
            this.TabCtl_BasicSet = new System.Windows.Forms.TabControl();
            this.Basic_Board = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic = new System.Windows.Forms.TableLayoutPanel();
            this.LblBoardWidth = new System.Windows.Forms.Label();
            this.LblBoardHigh = new System.Windows.Forms.Label();
            this.NumUD_BoardWidth = new System.Windows.Forms.NumericUpDown();
            this.LblBoardThickness = new System.Windows.Forms.Label();
            this.NumUD_BoardHigh = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardThickness = new System.Windows.Forms.NumericUpDown();
            this.Basic_Board_Standard = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic_Standard = new System.Windows.Forms.TableLayoutPanel();
            this.LblBoardHigh_Standard = new System.Windows.Forms.Label();
            this.NumUD_BoardWidth_Standard = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardHigh_Standard = new System.Windows.Forms.NumericUpDown();
            this.LblBoardWidth_Standard = new System.Windows.Forms.Label();
            this.LblBoardHighMax = new System.Windows.Forms.Label();
            this.LblBoardWidthMax = new System.Windows.Forms.Label();
            this.LblBoardHighMin = new System.Windows.Forms.Label();
            this.LblBoardWidthMin = new System.Windows.Forms.Label();
            this.NumUD_BoardHighMax = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardWidthMax = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardHighMin = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardWidthMin = new System.Windows.Forms.NumericUpDown();
            this.Basic_LoadPin = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic_LoadPin = new System.Windows.Forms.TableLayoutPanel();
            this.Cmbox_BoardStackingMethod = new System.Windows.Forms.ComboBox();
            this.LblBoardStackingMethod = new System.Windows.Forms.Label();
            this.NumUD_BoardHoleDis_L = new System.Windows.Forms.NumericUpDown();
            this.LblBoardHoleDis_L = new System.Windows.Forms.Label();
            this.LblBoardFoolHoleDis_A2 = new System.Windows.Forms.Label();
            this.NumUD_BoardFoolHoleDis_A2 = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardFoolHoleDis_A1 = new System.Windows.Forms.NumericUpDown();
            this.LblBoardFoolHoleDis_A1 = new System.Windows.Forms.Label();
            this.LblBoardFoolHoleDis = new System.Windows.Forms.Label();
            this.NumUD_BoardFoolHoleDis = new System.Windows.Forms.NumericUpDown();
            this.ChBox_CheckFoolHole = new System.Windows.Forms.CheckBox();
            this.Basic_LoadPin_Standard = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic_LoadPin_Standard = new System.Windows.Forms.TableLayoutPanel();
            this.NumUD_BoardHoleDis_Standard_L = new System.Windows.Forms.NumericUpDown();
            this.LblBoardHoleDis_Standard_L = new System.Windows.Forms.Label();
            this.LblBoardFoolHoleDis_Standard_A2 = new System.Windows.Forms.Label();
            this.NumUD_BoardFoolHoleDis_Standard_A2 = new System.Windows.Forms.NumericUpDown();
            this.NumUD_BoardFoolHoleDis_Standard_A1 = new System.Windows.Forms.NumericUpDown();
            this.LblBoardFoolHoleDis_Standatd_A1 = new System.Windows.Forms.Label();
            this.LblBoardFoolHoleDis_Standard = new System.Windows.Forms.Label();
            this.NumUD_BoardFoolHoleDis_Standard = new System.Windows.Forms.NumericUpDown();
            this.Basic_UnLoadPin = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic_UnLoadPin = new System.Windows.Forms.TableLayoutPanel();
            this.NumUD_BoardHoleDis_UnL = new System.Windows.Forms.NumericUpDown();
            this.LblBoardHoleDis_UnL = new System.Windows.Forms.Label();
            this.Basic_UnLoadPin_Standard = new System.Windows.Forms.TabPage();
            this.TabLP_BoardBasic_UnLoadPin_Standard = new System.Windows.Forms.TableLayoutPanel();
            this.NumUD_BoardHoleDis_Standard_UnL = new System.Windows.Forms.NumericUpDown();
            this.LblBoardHoleDis_Standard_UnL = new System.Windows.Forms.Label();
            this.SubPanel_Board_Basic = new System.Windows.Forms.Panel();
            this.RBtn_A2 = new System.Windows.Forms.RadioButton();
            this.RBtn_A1 = new System.Windows.Forms.RadioButton();
            this.SubPanel_Dir = new System.Windows.Forms.Panel();
            this.Other = new System.Windows.Forms.TabPage();
            this.TabCtl_OtherSet = new System.Windows.Forms.TabControl();
            this.Other_Basic = new System.Windows.Forms.TabPage();
            this.TabLP_Other_Normal = new System.Windows.Forms.TableLayoutPanel();
            this.NumUD_ExportTimeMin = new System.Windows.Forms.NumericUpDown();
            this.LblExportTimeMin = new System.Windows.Forms.Label();
            this.Other_LoadPin = new System.Windows.Forms.TabPage();
            this.TabLP_Other_LoadPin = new System.Windows.Forms.TableLayoutPanel();
            this.LblAlBoardThickness = new System.Windows.Forms.Label();
            this.NumUD_AlBoardThickness = new System.Windows.Forms.NumericUpDown();
            this.LblUnilateBoardThickness = new System.Windows.Forms.Label();
            this.NumUD_UnilateBoardThickness = new System.Windows.Forms.NumericUpDown();
            this.LblTolerance = new System.Windows.Forms.Label();
            this.NumUD_Tolerance = new System.Windows.Forms.NumericUpDown();
            this.Lbl_PCBNumBySet_L = new System.Windows.Forms.Label();
            this.NumUD_PCBNumBySet_L = new System.Windows.Forms.NumericUpDown();
            this.Other_UnLoadPin = new System.Windows.Forms.TabPage();
            this.TabLP_Other_UnLoadPin = new System.Windows.Forms.TableLayoutPanel();
            this.Lbl_PCBNumBySet_UnL = new System.Windows.Forms.Label();
            this.NumUD_PCBNumBySet_UnL = new System.Windows.Forms.NumericUpDown();
            this.RecipeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DGVTable)).BeginInit();
            this.TLP_1.SuspendLayout();
            this.TLP_2.SuspendLayout();
            this.TabCtl_Recipe.SuspendLayout();
            this.Board.SuspendLayout();
            this.TabCtl_BasicSet.SuspendLayout();
            this.Basic_Board.SuspendLayout();
            this.TabLP_BoardBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardThickness)).BeginInit();
            this.Basic_Board_Standard.SuspendLayout();
            this.TabLP_BoardBasic_Standard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidth_Standard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHigh_Standard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHighMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidthMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHighMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidthMin)).BeginInit();
            this.Basic_LoadPin.SuspendLayout();
            this.TabLP_BoardBasic_LoadPin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_A2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_A1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis)).BeginInit();
            this.Basic_LoadPin_Standard.SuspendLayout();
            this.TabLP_BoardBasic_LoadPin_Standard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_Standard_L)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard_A2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard_A1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard)).BeginInit();
            this.Basic_UnLoadPin.SuspendLayout();
            this.TabLP_BoardBasic_UnLoadPin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_UnL)).BeginInit();
            this.Basic_UnLoadPin_Standard.SuspendLayout();
            this.TabLP_BoardBasic_UnLoadPin_Standard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_Standard_UnL)).BeginInit();
            this.SubPanel_Board_Basic.SuspendLayout();
            this.Other.SuspendLayout();
            this.TabCtl_OtherSet.SuspendLayout();
            this.Other_Basic.SuspendLayout();
            this.TabLP_Other_Normal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_ExportTimeMin)).BeginInit();
            this.Other_LoadPin.SuspendLayout();
            this.TabLP_Other_LoadPin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_AlBoardThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_UnilateBoardThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Tolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PCBNumBySet_L)).BeginInit();
            this.Other_UnLoadPin.SuspendLayout();
            this.TabLP_Other_UnLoadPin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PCBNumBySet_UnL)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnLoad
            // 
            this.BtnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLoad.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnLoad.Location = new System.Drawing.Point(3, 3);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(152, 30);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "載入";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDelete.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnDelete.Location = new System.Drawing.Point(477, 3);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(155, 30);
            this.BtnDelete.TabIndex = 1;
            this.BtnDelete.Text = "刪除";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnModify
            // 
            this.BtnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnModify.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnModify.Location = new System.Drawing.Point(319, 3);
            this.BtnModify.Name = "BtnModify";
            this.BtnModify.Size = new System.Drawing.Size(152, 30);
            this.BtnModify.TabIndex = 3;
            this.BtnModify.Text = "修改";
            this.BtnModify.UseVisualStyleBackColor = true;
            this.BtnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // DGVTable
            // 
            this.DGVTable.AllowUserToAddRows = false;
            this.DGVTable.AllowUserToResizeColumns = false;
            this.DGVTable.AllowUserToResizeRows = false;
            this.DGVTable.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGVTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.Column1,
            this.Column2});
            this.DGVTable.Location = new System.Drawing.Point(0, 30);
            this.DGVTable.Margin = new System.Windows.Forms.Padding(2);
            this.DGVTable.MultiSelect = false;
            this.DGVTable.Name = "DGVTable";
            this.DGVTable.ReadOnly = true;
            this.DGVTable.RowHeadersWidth = 24;
            this.DGVTable.RowTemplate.Height = 24;
            this.DGVTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGVTable.Size = new System.Drawing.Size(635, 110);
            this.DGVTable.TabIndex = 4;
            this.DGVTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGVTable_CellClick);
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.HeaderText = "料號";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "日期";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "建立者";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 120;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAdd.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnAdd.Location = new System.Drawing.Point(161, 3);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(152, 30);
            this.BtnAdd.TabIndex = 5;
            this.BtnAdd.Text = "新增";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // LblPartNo
            // 
            this.LblPartNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LblPartNo.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblPartNo.Location = new System.Drawing.Point(101, 1);
            this.LblPartNo.Margin = new System.Windows.Forms.Padding(1);
            this.LblPartNo.Name = "LblPartNo";
            this.LblPartNo.Size = new System.Drawing.Size(98, 24);
            this.LblPartNo.TabIndex = 6;
            this.LblPartNo.Text = "料號 : ";
            this.LblPartNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblRecipeName
            // 
            this.LblRecipeName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LblRecipeName.AutoSize = true;
            this.LblRecipeName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LblRecipeName.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblRecipeName.Location = new System.Drawing.Point(201, 1);
            this.LblRecipeName.Margin = new System.Windows.Forms.Padding(1);
            this.LblRecipeName.Name = "LblRecipeName";
            this.LblRecipeName.Size = new System.Drawing.Size(198, 24);
            this.LblRecipeName.TabIndex = 7;
            this.LblRecipeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblRecipeTitle
            // 
            this.LblRecipeTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LblRecipeTitle.AutoSize = true;
            this.LblRecipeTitle.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblRecipeTitle.Location = new System.Drawing.Point(1, 1);
            this.LblRecipeTitle.Margin = new System.Windows.Forms.Padding(1);
            this.LblRecipeTitle.Name = "LblRecipeTitle";
            this.LblRecipeTitle.Size = new System.Drawing.Size(98, 24);
            this.LblRecipeTitle.TabIndex = 72;
            this.LblRecipeTitle.Text = "配方";
            this.LblRecipeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TLP_1
            // 
            this.TLP_1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TLP_1.ColumnCount = 3;
            this.TLP_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.TLP_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.TLP_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_1.Controls.Add(this.LblRecipeTitle, 0, 0);
            this.TLP_1.Controls.Add(this.LblPartNo, 1, 0);
            this.TLP_1.Controls.Add(this.LblRecipeName, 2, 0);
            this.TLP_1.Location = new System.Drawing.Point(0, 2);
            this.TLP_1.Margin = new System.Windows.Forms.Padding(1);
            this.TLP_1.Name = "TLP_1";
            this.TLP_1.RowCount = 1;
            this.TLP_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_1.Size = new System.Drawing.Size(400, 26);
            this.TLP_1.TabIndex = 73;
            // 
            // TLP_2
            // 
            this.TLP_2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TLP_2.ColumnCount = 4;
            this.TLP_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TLP_2.Controls.Add(this.BtnLoad, 0, 0);
            this.TLP_2.Controls.Add(this.BtnModify, 2, 0);
            this.TLP_2.Controls.Add(this.BtnAdd, 1, 0);
            this.TLP_2.Controls.Add(this.BtnDelete, 3, 0);
            this.TLP_2.Location = new System.Drawing.Point(0, 420);
            this.TLP_2.Margin = new System.Windows.Forms.Padding(1);
            this.TLP_2.Name = "TLP_2";
            this.TLP_2.RowCount = 1;
            this.TLP_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP_2.Size = new System.Drawing.Size(635, 36);
            this.TLP_2.TabIndex = 74;
            // 
            // TabCtl_Recipe
            // 
            this.TabCtl_Recipe.Controls.Add(this.Board);
            this.TabCtl_Recipe.Controls.Add(this.Other);
            this.TabCtl_Recipe.Location = new System.Drawing.Point(0, 140);
            this.TabCtl_Recipe.Name = "TabCtl_Recipe";
            this.TabCtl_Recipe.SelectedIndex = 0;
            this.TabCtl_Recipe.Size = new System.Drawing.Size(635, 280);
            this.TabCtl_Recipe.TabIndex = 75;
            // 
            // Board
            // 
            this.Board.Controls.Add(this.TabCtl_BasicSet);
            this.Board.Controls.Add(this.SubPanel_Board_Basic);
            this.Board.Location = new System.Drawing.Point(4, 28);
            this.Board.Name = "Board";
            this.Board.Padding = new System.Windows.Forms.Padding(3);
            this.Board.Size = new System.Drawing.Size(627, 248);
            this.Board.TabIndex = 0;
            this.Board.Text = "PCB板材";
            this.Board.UseVisualStyleBackColor = true;
            // 
            // TabCtl_BasicSet
            // 
            this.TabCtl_BasicSet.Controls.Add(this.Basic_Board);
            this.TabCtl_BasicSet.Controls.Add(this.Basic_Board_Standard);
            this.TabCtl_BasicSet.Controls.Add(this.Basic_LoadPin);
            this.TabCtl_BasicSet.Controls.Add(this.Basic_LoadPin_Standard);
            this.TabCtl_BasicSet.Controls.Add(this.Basic_UnLoadPin);
            this.TabCtl_BasicSet.Controls.Add(this.Basic_UnLoadPin_Standard);
            this.TabCtl_BasicSet.Location = new System.Drawing.Point(4, 4);
            this.TabCtl_BasicSet.Name = "TabCtl_BasicSet";
            this.TabCtl_BasicSet.SelectedIndex = 0;
            this.TabCtl_BasicSet.Size = new System.Drawing.Size(305, 241);
            this.TabCtl_BasicSet.TabIndex = 76;
            this.TabCtl_BasicSet.SelectedIndexChanged += new System.EventHandler(this.TabCtl_BasicSet_Selected);
            // 
            // Basic_Board
            // 
            this.Basic_Board.Controls.Add(this.TabLP_BoardBasic);
            this.Basic_Board.Location = new System.Drawing.Point(4, 28);
            this.Basic_Board.Name = "Basic_Board";
            this.Basic_Board.Padding = new System.Windows.Forms.Padding(3);
            this.Basic_Board.Size = new System.Drawing.Size(297, 209);
            this.Basic_Board.TabIndex = 0;
            this.Basic_Board.Text = "基本設置";
            this.Basic_Board.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic
            // 
            this.TabLP_BoardBasic.ColumnCount = 2;
            this.TabLP_BoardBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic.Controls.Add(this.LblBoardWidth, 0, 1);
            this.TabLP_BoardBasic.Controls.Add(this.LblBoardHigh, 0, 0);
            this.TabLP_BoardBasic.Controls.Add(this.NumUD_BoardWidth, 1, 1);
            this.TabLP_BoardBasic.Controls.Add(this.LblBoardThickness, 0, 2);
            this.TabLP_BoardBasic.Controls.Add(this.NumUD_BoardHigh, 1, 0);
            this.TabLP_BoardBasic.Controls.Add(this.NumUD_BoardThickness, 1, 2);
            this.TabLP_BoardBasic.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic.Name = "TabLP_BoardBasic";
            this.TabLP_BoardBasic.RowCount = 6;
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic.TabIndex = 77;
            // 
            // LblBoardWidth
            // 
            this.LblBoardWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardWidth.AutoSize = true;
            this.LblBoardWidth.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardWidth.Location = new System.Drawing.Point(1, 34);
            this.LblBoardWidth.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardWidth.Name = "LblBoardWidth";
            this.LblBoardWidth.Size = new System.Drawing.Size(164, 31);
            this.LblBoardWidth.TabIndex = 78;
            this.LblBoardWidth.Text = "板寬(mm)：";
            this.LblBoardWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardHigh
            // 
            this.LblBoardHigh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHigh.AutoSize = true;
            this.LblBoardHigh.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHigh.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHigh.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHigh.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHigh.Name = "LblBoardHigh";
            this.LblBoardHigh.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHigh.TabIndex = 77;
            this.LblBoardHigh.Text = "板長(mm)：";
            this.LblBoardHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardWidth
            // 
            this.NumUD_BoardWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardWidth.DecimalPlaces = 1;
            this.NumUD_BoardWidth.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardWidth.Location = new System.Drawing.Point(167, 35);
            this.NumUD_BoardWidth.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardWidth.Maximum = new decimal(new int[] {
            813,
            0,
            0,
            0});
            this.NumUD_BoardWidth.Minimum = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardWidth.Name = "NumUD_BoardWidth";
            this.NumUD_BoardWidth.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardWidth.TabIndex = 81;
            this.NumUD_BoardWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardWidth.Value = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardWidth.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardThickness
            // 
            this.LblBoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardThickness.AutoSize = true;
            this.LblBoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardThickness.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardThickness.Location = new System.Drawing.Point(1, 67);
            this.LblBoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardThickness.Name = "LblBoardThickness";
            this.LblBoardThickness.Size = new System.Drawing.Size(164, 31);
            this.LblBoardThickness.TabIndex = 76;
            this.LblBoardThickness.Text = "板厚(mm)：";
            this.LblBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardHigh
            // 
            this.NumUD_BoardHigh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHigh.DecimalPlaces = 1;
            this.NumUD_BoardHigh.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHigh.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHigh.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHigh.Maximum = new decimal(new int[] {
            813,
            0,
            0,
            0});
            this.NumUD_BoardHigh.Minimum = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardHigh.Name = "NumUD_BoardHigh";
            this.NumUD_BoardHigh.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHigh.TabIndex = 79;
            this.NumUD_BoardHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHigh.Value = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardHigh.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardThickness
            // 
            this.NumUD_BoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardThickness.DecimalPlaces = 2;
            this.NumUD_BoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardThickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardThickness.Location = new System.Drawing.Point(167, 68);
            this.NumUD_BoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardThickness.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumUD_BoardThickness.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NumUD_BoardThickness.Name = "NumUD_BoardThickness";
            this.NumUD_BoardThickness.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardThickness.TabIndex = 76;
            this.NumUD_BoardThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardThickness.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardThickness.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // Basic_Board_Standard
            // 
            this.Basic_Board_Standard.Controls.Add(this.TabLP_BoardBasic_Standard);
            this.Basic_Board_Standard.Location = new System.Drawing.Point(4, 28);
            this.Basic_Board_Standard.Name = "Basic_Board_Standard";
            this.Basic_Board_Standard.Size = new System.Drawing.Size(297, 209);
            this.Basic_Board_Standard.TabIndex = 3;
            this.Basic_Board_Standard.Text = "樣板尺寸";
            this.Basic_Board_Standard.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic_Standard
            // 
            this.TabLP_BoardBasic_Standard.ColumnCount = 2;
            this.TabLP_BoardBasic_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardHigh_Standard, 0, 0);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardWidth_Standard, 1, 1);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardHigh_Standard, 1, 0);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardWidth_Standard, 0, 1);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardHighMax, 0, 2);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardWidthMax, 0, 3);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardHighMin, 0, 4);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.LblBoardWidthMin, 0, 5);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardHighMax, 1, 2);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardWidthMax, 1, 3);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardHighMin, 1, 4);
            this.TabLP_BoardBasic_Standard.Controls.Add(this.NumUD_BoardWidthMin, 1, 5);
            this.TabLP_BoardBasic_Standard.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic_Standard.Name = "TabLP_BoardBasic_Standard";
            this.TabLP_BoardBasic_Standard.RowCount = 6;
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic_Standard.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic_Standard.TabIndex = 78;
            // 
            // LblBoardHigh_Standard
            // 
            this.LblBoardHigh_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHigh_Standard.AutoSize = true;
            this.LblBoardHigh_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHigh_Standard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHigh_Standard.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHigh_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHigh_Standard.Name = "LblBoardHigh_Standard";
            this.LblBoardHigh_Standard.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHigh_Standard.TabIndex = 77;
            this.LblBoardHigh_Standard.Text = "板長(mm)：";
            this.LblBoardHigh_Standard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardWidth_Standard
            // 
            this.NumUD_BoardWidth_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardWidth_Standard.DecimalPlaces = 1;
            this.NumUD_BoardWidth_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardWidth_Standard.Location = new System.Drawing.Point(167, 35);
            this.NumUD_BoardWidth_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardWidth_Standard.Maximum = new decimal(new int[] {
            813,
            0,
            0,
            0});
            this.NumUD_BoardWidth_Standard.Minimum = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardWidth_Standard.Name = "NumUD_BoardWidth_Standard";
            this.NumUD_BoardWidth_Standard.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardWidth_Standard.TabIndex = 81;
            this.NumUD_BoardWidth_Standard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardWidth_Standard.Value = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardWidth_Standard.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardHigh_Standard
            // 
            this.NumUD_BoardHigh_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHigh_Standard.DecimalPlaces = 1;
            this.NumUD_BoardHigh_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHigh_Standard.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHigh_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHigh_Standard.Maximum = new decimal(new int[] {
            813,
            0,
            0,
            0});
            this.NumUD_BoardHigh_Standard.Minimum = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardHigh_Standard.Name = "NumUD_BoardHigh_Standard";
            this.NumUD_BoardHigh_Standard.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHigh_Standard.TabIndex = 79;
            this.NumUD_BoardHigh_Standard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHigh_Standard.Value = new decimal(new int[] {
            543,
            0,
            0,
            0});
            this.NumUD_BoardHigh_Standard.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardWidth_Standard
            // 
            this.LblBoardWidth_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardWidth_Standard.AutoSize = true;
            this.LblBoardWidth_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardWidth_Standard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardWidth_Standard.Location = new System.Drawing.Point(1, 34);
            this.LblBoardWidth_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardWidth_Standard.Name = "LblBoardWidth_Standard";
            this.LblBoardWidth_Standard.Size = new System.Drawing.Size(164, 31);
            this.LblBoardWidth_Standard.TabIndex = 78;
            this.LblBoardWidth_Standard.Text = "板寬(mm)：";
            this.LblBoardWidth_Standard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardHighMax
            // 
            this.LblBoardHighMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHighMax.AutoSize = true;
            this.LblBoardHighMax.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblBoardHighMax.ForeColor = System.Drawing.Color.Blue;
            this.LblBoardHighMax.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHighMax.Location = new System.Drawing.Point(1, 67);
            this.LblBoardHighMax.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHighMax.Name = "LblBoardHighMax";
            this.LblBoardHighMax.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHighMax.TabIndex = 77;
            this.LblBoardHighMax.Text = "最大板長(mm)：";
            this.LblBoardHighMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardWidthMax
            // 
            this.LblBoardWidthMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardWidthMax.AutoSize = true;
            this.LblBoardWidthMax.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblBoardWidthMax.ForeColor = System.Drawing.Color.Blue;
            this.LblBoardWidthMax.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardWidthMax.Location = new System.Drawing.Point(1, 100);
            this.LblBoardWidthMax.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardWidthMax.Name = "LblBoardWidthMax";
            this.LblBoardWidthMax.Size = new System.Drawing.Size(164, 31);
            this.LblBoardWidthMax.TabIndex = 78;
            this.LblBoardWidthMax.Text = "最大板寬(mm)：";
            this.LblBoardWidthMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardHighMin
            // 
            this.LblBoardHighMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHighMin.AutoSize = true;
            this.LblBoardHighMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblBoardHighMin.ForeColor = System.Drawing.Color.Blue;
            this.LblBoardHighMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHighMin.Location = new System.Drawing.Point(1, 133);
            this.LblBoardHighMin.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHighMin.Name = "LblBoardHighMin";
            this.LblBoardHighMin.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHighMin.TabIndex = 77;
            this.LblBoardHighMin.Text = "最小板長(mm)：";
            this.LblBoardHighMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardWidthMin
            // 
            this.LblBoardWidthMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardWidthMin.AutoSize = true;
            this.LblBoardWidthMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LblBoardWidthMin.ForeColor = System.Drawing.Color.Blue;
            this.LblBoardWidthMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardWidthMin.Location = new System.Drawing.Point(1, 166);
            this.LblBoardWidthMin.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardWidthMin.Name = "LblBoardWidthMin";
            this.LblBoardWidthMin.Size = new System.Drawing.Size(164, 33);
            this.LblBoardWidthMin.TabIndex = 78;
            this.LblBoardWidthMin.Text = "最小板寬(mm)：";
            this.LblBoardWidthMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardHighMax
            // 
            this.NumUD_BoardHighMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHighMax.DecimalPlaces = 1;
            this.NumUD_BoardHighMax.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.NumUD_BoardHighMax.ForeColor = System.Drawing.Color.Blue;
            this.NumUD_BoardHighMax.Location = new System.Drawing.Point(167, 68);
            this.NumUD_BoardHighMax.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHighMax.Maximum = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardHighMax.Minimum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardHighMax.Name = "NumUD_BoardHighMax";
            this.NumUD_BoardHighMax.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHighMax.TabIndex = 79;
            this.NumUD_BoardHighMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHighMax.Value = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardHighMax.ValueChanged += new System.EventHandler(this.BoardSizeRangeValueChanged);
            this.NumUD_BoardHighMax.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardWidthMax
            // 
            this.NumUD_BoardWidthMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardWidthMax.DecimalPlaces = 1;
            this.NumUD_BoardWidthMax.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.NumUD_BoardWidthMax.ForeColor = System.Drawing.Color.Blue;
            this.NumUD_BoardWidthMax.Location = new System.Drawing.Point(167, 101);
            this.NumUD_BoardWidthMax.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardWidthMax.Maximum = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardWidthMax.Minimum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardWidthMax.Name = "NumUD_BoardWidthMax";
            this.NumUD_BoardWidthMax.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardWidthMax.TabIndex = 81;
            this.NumUD_BoardWidthMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardWidthMax.Value = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardWidthMax.ValueChanged += new System.EventHandler(this.BoardSizeRangeValueChanged);
            this.NumUD_BoardWidthMax.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardHighMin
            // 
            this.NumUD_BoardHighMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHighMin.DecimalPlaces = 1;
            this.NumUD_BoardHighMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.NumUD_BoardHighMin.ForeColor = System.Drawing.Color.Blue;
            this.NumUD_BoardHighMin.Location = new System.Drawing.Point(167, 134);
            this.NumUD_BoardHighMin.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHighMin.Maximum = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardHighMin.Minimum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardHighMin.Name = "NumUD_BoardHighMin";
            this.NumUD_BoardHighMin.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHighMin.TabIndex = 79;
            this.NumUD_BoardHighMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHighMin.Value = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardHighMin.ValueChanged += new System.EventHandler(this.BoardSizeRangeValueChanged);
            this.NumUD_BoardHighMin.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardWidthMin
            // 
            this.NumUD_BoardWidthMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardWidthMin.DecimalPlaces = 1;
            this.NumUD_BoardWidthMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.NumUD_BoardWidthMin.ForeColor = System.Drawing.Color.Blue;
            this.NumUD_BoardWidthMin.Location = new System.Drawing.Point(167, 168);
            this.NumUD_BoardWidthMin.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardWidthMin.Maximum = new decimal(new int[] {
            1016,
            0,
            0,
            0});
            this.NumUD_BoardWidthMin.Minimum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardWidthMin.Name = "NumUD_BoardWidthMin";
            this.NumUD_BoardWidthMin.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardWidthMin.TabIndex = 81;
            this.NumUD_BoardWidthMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardWidthMin.Value = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.NumUD_BoardWidthMin.ValueChanged += new System.EventHandler(this.BoardSizeRangeValueChanged);
            this.NumUD_BoardWidthMin.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // Basic_LoadPin
            // 
            this.Basic_LoadPin.Controls.Add(this.TabLP_BoardBasic_LoadPin);
            this.Basic_LoadPin.Location = new System.Drawing.Point(4, 28);
            this.Basic_LoadPin.Name = "Basic_LoadPin";
            this.Basic_LoadPin.Size = new System.Drawing.Size(297, 209);
            this.Basic_LoadPin.TabIndex = 1;
            this.Basic_LoadPin.Text = "上Pin";
            this.Basic_LoadPin.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic_LoadPin
            // 
            this.TabLP_BoardBasic_LoadPin.ColumnCount = 2;
            this.TabLP_BoardBasic_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.Cmbox_BoardStackingMethod, 1, 4);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.LblBoardStackingMethod, 0, 4);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.NumUD_BoardHoleDis_L, 1, 0);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.LblBoardHoleDis_L, 0, 0);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.LblBoardFoolHoleDis_A2, 0, 3);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.NumUD_BoardFoolHoleDis_A2, 1, 3);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.NumUD_BoardFoolHoleDis_A1, 1, 2);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.LblBoardFoolHoleDis_A1, 0, 2);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.LblBoardFoolHoleDis, 0, 1);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.NumUD_BoardFoolHoleDis, 1, 1);
            this.TabLP_BoardBasic_LoadPin.Controls.Add(this.ChBox_CheckFoolHole, 1, 5);
            this.TabLP_BoardBasic_LoadPin.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic_LoadPin.Name = "TabLP_BoardBasic_LoadPin";
            this.TabLP_BoardBasic_LoadPin.RowCount = 6;
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic_LoadPin.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic_LoadPin.TabIndex = 78;
            // 
            // Cmbox_BoardStackingMethod
            // 
            this.Cmbox_BoardStackingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Cmbox_BoardStackingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cmbox_BoardStackingMethod.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.Cmbox_BoardStackingMethod.FormattingEnabled = true;
            this.Cmbox_BoardStackingMethod.Location = new System.Drawing.Point(169, 135);
            this.Cmbox_BoardStackingMethod.Name = "Cmbox_BoardStackingMethod";
            this.Cmbox_BoardStackingMethod.Size = new System.Drawing.Size(116, 28);
            this.Cmbox_BoardStackingMethod.TabIndex = 76;
            this.Cmbox_BoardStackingMethod.SelectedIndexChanged += new System.EventHandler(this.Cbx_BoardStackingMethod_SelectedIndexChanged);
            // 
            // LblBoardStackingMethod
            // 
            this.LblBoardStackingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardStackingMethod.AutoSize = true;
            this.LblBoardStackingMethod.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardStackingMethod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardStackingMethod.Location = new System.Drawing.Point(1, 133);
            this.LblBoardStackingMethod.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardStackingMethod.Name = "LblBoardStackingMethod";
            this.LblBoardStackingMethod.Size = new System.Drawing.Size(164, 31);
            this.LblBoardStackingMethod.TabIndex = 77;
            this.LblBoardStackingMethod.Text = "疊板方式：";
            this.LblBoardStackingMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardHoleDis_L
            // 
            this.NumUD_BoardHoleDis_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHoleDis_L.DecimalPlaces = 1;
            this.NumUD_BoardHoleDis_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHoleDis_L.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHoleDis_L.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHoleDis_L.Maximum = new decimal(new int[] {
            503,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_L.Minimum = new decimal(new int[] {
            478,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_L.Name = "NumUD_BoardHoleDis_L";
            this.NumUD_BoardHoleDis_L.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHoleDis_L.TabIndex = 82;
            this.NumUD_BoardHoleDis_L.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHoleDis_L.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_L.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardHoleDis_L
            // 
            this.LblBoardHoleDis_L.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHoleDis_L.AutoSize = true;
            this.LblBoardHoleDis_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHoleDis_L.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHoleDis_L.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHoleDis_L.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHoleDis_L.Name = "LblBoardHoleDis_L";
            this.LblBoardHoleDis_L.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHoleDis_L.TabIndex = 77;
            this.LblBoardHoleDis_L.Text = "孔距(mm)：";
            this.LblBoardHoleDis_L.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardFoolHoleDis_A2
            // 
            this.LblBoardFoolHoleDis_A2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis_A2.AutoSize = true;
            this.LblBoardFoolHoleDis_A2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis_A2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis_A2.Location = new System.Drawing.Point(1, 100);
            this.LblBoardFoolHoleDis_A2.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis_A2.Name = "LblBoardFoolHoleDis_A2";
            this.LblBoardFoolHoleDis_A2.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis_A2.TabIndex = 76;
            this.LblBoardFoolHoleDis_A2.Text = "A2側邊距(mm)：";
            this.LblBoardFoolHoleDis_A2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardFoolHoleDis_A2
            // 
            this.NumUD_BoardFoolHoleDis_A2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis_A2.DecimalPlaces = 2;
            this.NumUD_BoardFoolHoleDis_A2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis_A2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_A2.Location = new System.Drawing.Point(167, 101);
            this.NumUD_BoardFoolHoleDis_A2.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis_A2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis_A2.Name = "NumUD_BoardFoolHoleDis_A2";
            this.NumUD_BoardFoolHoleDis_A2.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis_A2.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis_A2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis_A2.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_A2.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardFoolHoleDis_A1
            // 
            this.NumUD_BoardFoolHoleDis_A1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis_A1.DecimalPlaces = 2;
            this.NumUD_BoardFoolHoleDis_A1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis_A1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_A1.Location = new System.Drawing.Point(167, 68);
            this.NumUD_BoardFoolHoleDis_A1.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis_A1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis_A1.Name = "NumUD_BoardFoolHoleDis_A1";
            this.NumUD_BoardFoolHoleDis_A1.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis_A1.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis_A1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis_A1.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_A1.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardFoolHoleDis_A1
            // 
            this.LblBoardFoolHoleDis_A1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis_A1.AutoSize = true;
            this.LblBoardFoolHoleDis_A1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis_A1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis_A1.Location = new System.Drawing.Point(1, 67);
            this.LblBoardFoolHoleDis_A1.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis_A1.Name = "LblBoardFoolHoleDis_A1";
            this.LblBoardFoolHoleDis_A1.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis_A1.TabIndex = 76;
            this.LblBoardFoolHoleDis_A1.Text = "A1側邊距(mm)：";
            this.LblBoardFoolHoleDis_A1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardFoolHoleDis
            // 
            this.LblBoardFoolHoleDis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis.AutoSize = true;
            this.LblBoardFoolHoleDis.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis.Location = new System.Drawing.Point(1, 34);
            this.LblBoardFoolHoleDis.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis.Name = "LblBoardFoolHoleDis";
            this.LblBoardFoolHoleDis.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis.TabIndex = 76;
            this.LblBoardFoolHoleDis.Text = "防呆孔(mm)：";
            this.LblBoardFoolHoleDis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardFoolHoleDis
            // 
            this.NumUD_BoardFoolHoleDis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis.DecimalPlaces = 1;
            this.NumUD_BoardFoolHoleDis.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis.Location = new System.Drawing.Point(167, 35);
            this.NumUD_BoardFoolHoleDis.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis.Name = "NumUD_BoardFoolHoleDis";
            this.NumUD_BoardFoolHoleDis.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // ChBox_CheckFoolHole
            // 
            this.ChBox_CheckFoolHole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ChBox_CheckFoolHole.AutoSize = true;
            this.ChBox_CheckFoolHole.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.ChBox_CheckFoolHole.Location = new System.Drawing.Point(167, 170);
            this.ChBox_CheckFoolHole.Margin = new System.Windows.Forms.Padding(1);
            this.ChBox_CheckFoolHole.Name = "ChBox_CheckFoolHole";
            this.ChBox_CheckFoolHole.Size = new System.Drawing.Size(120, 24);
            this.ChBox_CheckFoolHole.TabIndex = 76;
            this.ChBox_CheckFoolHole.Text = "防呆孔檢測";
            this.ChBox_CheckFoolHole.UseVisualStyleBackColor = true;
            // 
            // Basic_LoadPin_Standard
            // 
            this.Basic_LoadPin_Standard.Controls.Add(this.TabLP_BoardBasic_LoadPin_Standard);
            this.Basic_LoadPin_Standard.Location = new System.Drawing.Point(4, 28);
            this.Basic_LoadPin_Standard.Name = "Basic_LoadPin_Standard";
            this.Basic_LoadPin_Standard.Size = new System.Drawing.Size(297, 209);
            this.Basic_LoadPin_Standard.TabIndex = 5;
            this.Basic_LoadPin_Standard.Text = "上Pin樣板";
            this.Basic_LoadPin_Standard.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic_LoadPin_Standard
            // 
            this.TabLP_BoardBasic_LoadPin_Standard.ColumnCount = 2;
            this.TabLP_BoardBasic_LoadPin_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic_LoadPin_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.NumUD_BoardHoleDis_Standard_L, 1, 0);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.LblBoardHoleDis_Standard_L, 0, 0);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.LblBoardFoolHoleDis_Standard_A2, 0, 3);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.NumUD_BoardFoolHoleDis_Standard_A2, 1, 3);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.NumUD_BoardFoolHoleDis_Standard_A1, 1, 2);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.LblBoardFoolHoleDis_Standatd_A1, 0, 2);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.LblBoardFoolHoleDis_Standard, 0, 1);
            this.TabLP_BoardBasic_LoadPin_Standard.Controls.Add(this.NumUD_BoardFoolHoleDis_Standard, 1, 1);
            this.TabLP_BoardBasic_LoadPin_Standard.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic_LoadPin_Standard.Name = "TabLP_BoardBasic_LoadPin_Standard";
            this.TabLP_BoardBasic_LoadPin_Standard.RowCount = 6;
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_LoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic_LoadPin_Standard.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic_LoadPin_Standard.TabIndex = 79;
            // 
            // NumUD_BoardHoleDis_Standard_L
            // 
            this.NumUD_BoardHoleDis_Standard_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHoleDis_Standard_L.DecimalPlaces = 1;
            this.NumUD_BoardHoleDis_Standard_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHoleDis_Standard_L.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHoleDis_Standard_L.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHoleDis_Standard_L.Maximum = new decimal(new int[] {
            503,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_L.Minimum = new decimal(new int[] {
            478,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_L.Name = "NumUD_BoardHoleDis_Standard_L";
            this.NumUD_BoardHoleDis_Standard_L.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHoleDis_Standard_L.TabIndex = 82;
            this.NumUD_BoardHoleDis_Standard_L.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHoleDis_Standard_L.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_L.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardHoleDis_Standard_L
            // 
            this.LblBoardHoleDis_Standard_L.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHoleDis_Standard_L.AutoSize = true;
            this.LblBoardHoleDis_Standard_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHoleDis_Standard_L.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHoleDis_Standard_L.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHoleDis_Standard_L.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHoleDis_Standard_L.Name = "LblBoardHoleDis_Standard_L";
            this.LblBoardHoleDis_Standard_L.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHoleDis_Standard_L.TabIndex = 77;
            this.LblBoardHoleDis_Standard_L.Text = "孔距(mm)：";
            this.LblBoardHoleDis_Standard_L.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardFoolHoleDis_Standard_A2
            // 
            this.LblBoardFoolHoleDis_Standard_A2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis_Standard_A2.AutoSize = true;
            this.LblBoardFoolHoleDis_Standard_A2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis_Standard_A2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis_Standard_A2.Location = new System.Drawing.Point(1, 100);
            this.LblBoardFoolHoleDis_Standard_A2.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis_Standard_A2.Name = "LblBoardFoolHoleDis_Standard_A2";
            this.LblBoardFoolHoleDis_Standard_A2.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis_Standard_A2.TabIndex = 76;
            this.LblBoardFoolHoleDis_Standard_A2.Text = "A2側邊距(mm)：";
            this.LblBoardFoolHoleDis_Standard_A2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardFoolHoleDis_Standard_A2
            // 
            this.NumUD_BoardFoolHoleDis_Standard_A2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis_Standard_A2.DecimalPlaces = 2;
            this.NumUD_BoardFoolHoleDis_Standard_A2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis_Standard_A2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard_A2.Location = new System.Drawing.Point(167, 101);
            this.NumUD_BoardFoolHoleDis_Standard_A2.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis_Standard_A2.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis_Standard_A2.Name = "NumUD_BoardFoolHoleDis_Standard_A2";
            this.NumUD_BoardFoolHoleDis_Standard_A2.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis_Standard_A2.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis_Standard_A2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis_Standard_A2.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard_A2.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // NumUD_BoardFoolHoleDis_Standard_A1
            // 
            this.NumUD_BoardFoolHoleDis_Standard_A1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis_Standard_A1.DecimalPlaces = 2;
            this.NumUD_BoardFoolHoleDis_Standard_A1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis_Standard_A1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard_A1.Location = new System.Drawing.Point(167, 68);
            this.NumUD_BoardFoolHoleDis_Standard_A1.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis_Standard_A1.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis_Standard_A1.Name = "NumUD_BoardFoolHoleDis_Standard_A1";
            this.NumUD_BoardFoolHoleDis_Standard_A1.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis_Standard_A1.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis_Standard_A1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis_Standard_A1.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard_A1.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardFoolHoleDis_Standatd_A1
            // 
            this.LblBoardFoolHoleDis_Standatd_A1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis_Standatd_A1.AutoSize = true;
            this.LblBoardFoolHoleDis_Standatd_A1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis_Standatd_A1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis_Standatd_A1.Location = new System.Drawing.Point(1, 67);
            this.LblBoardFoolHoleDis_Standatd_A1.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis_Standatd_A1.Name = "LblBoardFoolHoleDis_Standatd_A1";
            this.LblBoardFoolHoleDis_Standatd_A1.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis_Standatd_A1.TabIndex = 76;
            this.LblBoardFoolHoleDis_Standatd_A1.Text = "A1側邊距(mm)：";
            this.LblBoardFoolHoleDis_Standatd_A1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblBoardFoolHoleDis_Standard
            // 
            this.LblBoardFoolHoleDis_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardFoolHoleDis_Standard.AutoSize = true;
            this.LblBoardFoolHoleDis_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardFoolHoleDis_Standard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardFoolHoleDis_Standard.Location = new System.Drawing.Point(1, 34);
            this.LblBoardFoolHoleDis_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardFoolHoleDis_Standard.Name = "LblBoardFoolHoleDis_Standard";
            this.LblBoardFoolHoleDis_Standard.Size = new System.Drawing.Size(164, 31);
            this.LblBoardFoolHoleDis_Standard.TabIndex = 76;
            this.LblBoardFoolHoleDis_Standard.Text = "防呆孔(mm)：";
            this.LblBoardFoolHoleDis_Standard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_BoardFoolHoleDis_Standard
            // 
            this.NumUD_BoardFoolHoleDis_Standard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardFoolHoleDis_Standard.DecimalPlaces = 1;
            this.NumUD_BoardFoolHoleDis_Standard.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardFoolHoleDis_Standard.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard.Location = new System.Drawing.Point(167, 35);
            this.NumUD_BoardFoolHoleDis_Standard.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardFoolHoleDis_Standard.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardFoolHoleDis_Standard.Name = "NumUD_BoardFoolHoleDis_Standard";
            this.NumUD_BoardFoolHoleDis_Standard.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardFoolHoleDis_Standard.TabIndex = 76;
            this.NumUD_BoardFoolHoleDis_Standard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardFoolHoleDis_Standard.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.NumUD_BoardFoolHoleDis_Standard.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // Basic_UnLoadPin
            // 
            this.Basic_UnLoadPin.Controls.Add(this.TabLP_BoardBasic_UnLoadPin);
            this.Basic_UnLoadPin.Location = new System.Drawing.Point(4, 28);
            this.Basic_UnLoadPin.Name = "Basic_UnLoadPin";
            this.Basic_UnLoadPin.Size = new System.Drawing.Size(297, 209);
            this.Basic_UnLoadPin.TabIndex = 2;
            this.Basic_UnLoadPin.Text = "退Pin";
            this.Basic_UnLoadPin.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic_UnLoadPin
            // 
            this.TabLP_BoardBasic_UnLoadPin.ColumnCount = 2;
            this.TabLP_BoardBasic_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic_UnLoadPin.Controls.Add(this.NumUD_BoardHoleDis_UnL, 1, 0);
            this.TabLP_BoardBasic_UnLoadPin.Controls.Add(this.LblBoardHoleDis_UnL, 0, 0);
            this.TabLP_BoardBasic_UnLoadPin.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic_UnLoadPin.Name = "TabLP_BoardBasic_UnLoadPin";
            this.TabLP_BoardBasic_UnLoadPin.RowCount = 6;
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic_UnLoadPin.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic_UnLoadPin.TabIndex = 79;
            // 
            // NumUD_BoardHoleDis_UnL
            // 
            this.NumUD_BoardHoleDis_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHoleDis_UnL.DecimalPlaces = 1;
            this.NumUD_BoardHoleDis_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHoleDis_UnL.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHoleDis_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHoleDis_UnL.Maximum = new decimal(new int[] {
            503,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_UnL.Minimum = new decimal(new int[] {
            478,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_UnL.Name = "NumUD_BoardHoleDis_UnL";
            this.NumUD_BoardHoleDis_UnL.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHoleDis_UnL.TabIndex = 81;
            this.NumUD_BoardHoleDis_UnL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHoleDis_UnL.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_UnL.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardHoleDis_UnL
            // 
            this.LblBoardHoleDis_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHoleDis_UnL.AutoSize = true;
            this.LblBoardHoleDis_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHoleDis_UnL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHoleDis_UnL.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHoleDis_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHoleDis_UnL.Name = "LblBoardHoleDis_UnL";
            this.LblBoardHoleDis_UnL.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHoleDis_UnL.TabIndex = 76;
            this.LblBoardHoleDis_UnL.Text = "孔距(mm)：";
            this.LblBoardHoleDis_UnL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Basic_UnLoadPin_Standard
            // 
            this.Basic_UnLoadPin_Standard.Controls.Add(this.TabLP_BoardBasic_UnLoadPin_Standard);
            this.Basic_UnLoadPin_Standard.Location = new System.Drawing.Point(4, 28);
            this.Basic_UnLoadPin_Standard.Name = "Basic_UnLoadPin_Standard";
            this.Basic_UnLoadPin_Standard.Size = new System.Drawing.Size(297, 209);
            this.Basic_UnLoadPin_Standard.TabIndex = 4;
            this.Basic_UnLoadPin_Standard.Text = "退Pin樣板";
            this.Basic_UnLoadPin_Standard.UseVisualStyleBackColor = true;
            // 
            // TabLP_BoardBasic_UnLoadPin_Standard
            // 
            this.TabLP_BoardBasic_UnLoadPin_Standard.ColumnCount = 2;
            this.TabLP_BoardBasic_UnLoadPin_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.Controls.Add(this.NumUD_BoardHoleDis_Standard_UnL, 1, 0);
            this.TabLP_BoardBasic_UnLoadPin_Standard.Controls.Add(this.LblBoardHoleDis_Standard_UnL, 0, 0);
            this.TabLP_BoardBasic_UnLoadPin_Standard.Location = new System.Drawing.Point(4, 4);
            this.TabLP_BoardBasic_UnLoadPin_Standard.Name = "TabLP_BoardBasic_UnLoadPin_Standard";
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowCount = 6;
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TabLP_BoardBasic_UnLoadPin_Standard.Size = new System.Drawing.Size(288, 200);
            this.TabLP_BoardBasic_UnLoadPin_Standard.TabIndex = 80;
            // 
            // NumUD_BoardHoleDis_Standard_UnL
            // 
            this.NumUD_BoardHoleDis_Standard_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_BoardHoleDis_Standard_UnL.DecimalPlaces = 1;
            this.NumUD_BoardHoleDis_Standard_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_BoardHoleDis_Standard_UnL.Location = new System.Drawing.Point(167, 2);
            this.NumUD_BoardHoleDis_Standard_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_BoardHoleDis_Standard_UnL.Maximum = new decimal(new int[] {
            503,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_UnL.Minimum = new decimal(new int[] {
            478,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_UnL.Name = "NumUD_BoardHoleDis_Standard_UnL";
            this.NumUD_BoardHoleDis_Standard_UnL.Size = new System.Drawing.Size(120, 28);
            this.NumUD_BoardHoleDis_Standard_UnL.TabIndex = 81;
            this.NumUD_BoardHoleDis_Standard_UnL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_BoardHoleDis_Standard_UnL.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.NumUD_BoardHoleDis_Standard_UnL.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblBoardHoleDis_Standard_UnL
            // 
            this.LblBoardHoleDis_Standard_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblBoardHoleDis_Standard_UnL.AutoSize = true;
            this.LblBoardHoleDis_Standard_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblBoardHoleDis_Standard_UnL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblBoardHoleDis_Standard_UnL.Location = new System.Drawing.Point(1, 1);
            this.LblBoardHoleDis_Standard_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.LblBoardHoleDis_Standard_UnL.Name = "LblBoardHoleDis_Standard_UnL";
            this.LblBoardHoleDis_Standard_UnL.Size = new System.Drawing.Size(164, 31);
            this.LblBoardHoleDis_Standard_UnL.TabIndex = 76;
            this.LblBoardHoleDis_Standard_UnL.Text = "孔距(mm)：";
            this.LblBoardHoleDis_Standard_UnL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SubPanel_Board_Basic
            // 
            this.SubPanel_Board_Basic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SubPanel_Board_Basic.BackColor = System.Drawing.Color.Transparent;
            this.SubPanel_Board_Basic.BackgroundImage = global::GJSControl.Properties.Resources.BasicBoard;
            this.SubPanel_Board_Basic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SubPanel_Board_Basic.Controls.Add(this.RBtn_A2);
            this.SubPanel_Board_Basic.Controls.Add(this.RBtn_A1);
            this.SubPanel_Board_Basic.Controls.Add(this.SubPanel_Dir);
            this.SubPanel_Board_Basic.Location = new System.Drawing.Point(311, 36);
            this.SubPanel_Board_Basic.Margin = new System.Windows.Forms.Padding(1);
            this.SubPanel_Board_Basic.Name = "SubPanel_Board_Basic";
            this.SubPanel_Board_Basic.Size = new System.Drawing.Size(312, 201);
            this.SubPanel_Board_Basic.TabIndex = 76;
            // 
            // RBtn_A2
            // 
            this.RBtn_A2.AutoSize = true;
            this.RBtn_A2.Location = new System.Drawing.Point(262, 32);
            this.RBtn_A2.Name = "RBtn_A2";
            this.RBtn_A2.Size = new System.Drawing.Size(46, 23);
            this.RBtn_A2.TabIndex = 77;
            this.RBtn_A2.TabStop = true;
            this.RBtn_A2.Text = "A2";
            this.RBtn_A2.UseVisualStyleBackColor = true;
            // 
            // RBtn_A1
            // 
            this.RBtn_A1.AutoSize = true;
            this.RBtn_A1.Location = new System.Drawing.Point(262, 175);
            this.RBtn_A1.Name = "RBtn_A1";
            this.RBtn_A1.Size = new System.Drawing.Size(46, 23);
            this.RBtn_A1.TabIndex = 76;
            this.RBtn_A1.TabStop = true;
            this.RBtn_A1.Text = "A1";
            this.RBtn_A1.UseVisualStyleBackColor = true;
            // 
            // SubPanel_Dir
            // 
            this.SubPanel_Dir.BackgroundImage = global::GJSControl.Properties.Resources.RightArrow;
            this.SubPanel_Dir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SubPanel_Dir.Location = new System.Drawing.Point(4, 4);
            this.SubPanel_Dir.Name = "SubPanel_Dir";
            this.SubPanel_Dir.Size = new System.Drawing.Size(34, 34);
            this.SubPanel_Dir.TabIndex = 77;
            // 
            // Other
            // 
            this.Other.Controls.Add(this.TabCtl_OtherSet);
            this.Other.Location = new System.Drawing.Point(4, 28);
            this.Other.Name = "Other";
            this.Other.Size = new System.Drawing.Size(627, 248);
            this.Other.TabIndex = 1;
            this.Other.Text = "其他";
            this.Other.UseVisualStyleBackColor = true;
            // 
            // TabCtl_OtherSet
            // 
            this.TabCtl_OtherSet.Controls.Add(this.Other_Basic);
            this.TabCtl_OtherSet.Controls.Add(this.Other_LoadPin);
            this.TabCtl_OtherSet.Controls.Add(this.Other_UnLoadPin);
            this.TabCtl_OtherSet.Location = new System.Drawing.Point(4, 4);
            this.TabCtl_OtherSet.Name = "TabCtl_OtherSet";
            this.TabCtl_OtherSet.SelectedIndex = 0;
            this.TabCtl_OtherSet.Size = new System.Drawing.Size(620, 241);
            this.TabCtl_OtherSet.TabIndex = 77;
            // 
            // Other_Basic
            // 
            this.Other_Basic.Controls.Add(this.TabLP_Other_Normal);
            this.Other_Basic.Location = new System.Drawing.Point(4, 28);
            this.Other_Basic.Name = "Other_Basic";
            this.Other_Basic.Padding = new System.Windows.Forms.Padding(3);
            this.Other_Basic.Size = new System.Drawing.Size(612, 209);
            this.Other_Basic.TabIndex = 0;
            this.Other_Basic.Text = "共用";
            this.Other_Basic.UseVisualStyleBackColor = true;
            // 
            // TabLP_Other_Normal
            // 
            this.TabLP_Other_Normal.ColumnCount = 4;
            this.TabLP_Other_Normal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_Normal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_Normal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_Normal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_Normal.Controls.Add(this.NumUD_ExportTimeMin, 1, 0);
            this.TabLP_Other_Normal.Controls.Add(this.LblExportTimeMin, 0, 0);
            this.TabLP_Other_Normal.Location = new System.Drawing.Point(4, 4);
            this.TabLP_Other_Normal.Name = "TabLP_Other_Normal";
            this.TabLP_Other_Normal.RowCount = 6;
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_Normal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_Normal.Size = new System.Drawing.Size(604, 200);
            this.TabLP_Other_Normal.TabIndex = 77;
            // 
            // NumUD_ExportTimeMin
            // 
            this.NumUD_ExportTimeMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_ExportTimeMin.DecimalPlaces = 1;
            this.NumUD_ExportTimeMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_ExportTimeMin.Location = new System.Drawing.Point(181, 2);
            this.NumUD_ExportTimeMin.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_ExportTimeMin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumUD_ExportTimeMin.Name = "NumUD_ExportTimeMin";
            this.NumUD_ExportTimeMin.Size = new System.Drawing.Size(120, 28);
            this.NumUD_ExportTimeMin.TabIndex = 76;
            this.NumUD_ExportTimeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_ExportTimeMin.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblExportTimeMin
            // 
            this.LblExportTimeMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblExportTimeMin.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblExportTimeMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblExportTimeMin.Location = new System.Drawing.Point(1, 1);
            this.LblExportTimeMin.Margin = new System.Windows.Forms.Padding(1);
            this.LblExportTimeMin.Name = "LblExportTimeMin";
            this.LblExportTimeMin.Size = new System.Drawing.Size(178, 31);
            this.LblExportTimeMin.TabIndex = 77;
            this.LblExportTimeMin.Text = "出板後延遲時間(秒)：";
            this.LblExportTimeMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Other_LoadPin
            // 
            this.Other_LoadPin.Controls.Add(this.TabLP_Other_LoadPin);
            this.Other_LoadPin.Location = new System.Drawing.Point(4, 28);
            this.Other_LoadPin.Name = "Other_LoadPin";
            this.Other_LoadPin.Size = new System.Drawing.Size(612, 209);
            this.Other_LoadPin.TabIndex = 1;
            this.Other_LoadPin.Text = "上Pin";
            this.Other_LoadPin.UseVisualStyleBackColor = true;
            // 
            // TabLP_Other_LoadPin
            // 
            this.TabLP_Other_LoadPin.ColumnCount = 4;
            this.TabLP_Other_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_LoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_LoadPin.Controls.Add(this.LblAlBoardThickness, 0, 0);
            this.TabLP_Other_LoadPin.Controls.Add(this.NumUD_AlBoardThickness, 1, 0);
            this.TabLP_Other_LoadPin.Controls.Add(this.LblUnilateBoardThickness, 0, 1);
            this.TabLP_Other_LoadPin.Controls.Add(this.NumUD_UnilateBoardThickness, 1, 1);
            this.TabLP_Other_LoadPin.Controls.Add(this.LblTolerance, 0, 2);
            this.TabLP_Other_LoadPin.Controls.Add(this.NumUD_Tolerance, 1, 2);
            this.TabLP_Other_LoadPin.Controls.Add(this.Lbl_PCBNumBySet_L, 0, 3);
            this.TabLP_Other_LoadPin.Controls.Add(this.NumUD_PCBNumBySet_L, 1, 3);
            this.TabLP_Other_LoadPin.Location = new System.Drawing.Point(4, 4);
            this.TabLP_Other_LoadPin.Name = "TabLP_Other_LoadPin";
            this.TabLP_Other_LoadPin.RowCount = 6;
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_LoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_LoadPin.Size = new System.Drawing.Size(604, 200);
            this.TabLP_Other_LoadPin.TabIndex = 78;
            // 
            // LblAlBoardThickness
            // 
            this.LblAlBoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblAlBoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblAlBoardThickness.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblAlBoardThickness.Location = new System.Drawing.Point(1, 1);
            this.LblAlBoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.LblAlBoardThickness.Name = "LblAlBoardThickness";
            this.LblAlBoardThickness.Size = new System.Drawing.Size(178, 31);
            this.LblAlBoardThickness.TabIndex = 78;
            this.LblAlBoardThickness.Text = "鋁板板厚(mm)：";
            this.LblAlBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_AlBoardThickness
            // 
            this.NumUD_AlBoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_AlBoardThickness.DecimalPlaces = 2;
            this.NumUD_AlBoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_AlBoardThickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_AlBoardThickness.Location = new System.Drawing.Point(181, 2);
            this.NumUD_AlBoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_AlBoardThickness.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumUD_AlBoardThickness.Name = "NumUD_AlBoardThickness";
            this.NumUD_AlBoardThickness.Size = new System.Drawing.Size(120, 28);
            this.NumUD_AlBoardThickness.TabIndex = 77;
            this.NumUD_AlBoardThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_AlBoardThickness.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblUnilateBoardThickness
            // 
            this.LblUnilateBoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblUnilateBoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblUnilateBoardThickness.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblUnilateBoardThickness.Location = new System.Drawing.Point(1, 34);
            this.LblUnilateBoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.LblUnilateBoardThickness.Name = "LblUnilateBoardThickness";
            this.LblUnilateBoardThickness.Size = new System.Drawing.Size(178, 31);
            this.LblUnilateBoardThickness.TabIndex = 78;
            this.LblUnilateBoardThickness.Text = "底板板厚(mm)：";
            this.LblUnilateBoardThickness.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_UnilateBoardThickness
            // 
            this.NumUD_UnilateBoardThickness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_UnilateBoardThickness.DecimalPlaces = 2;
            this.NumUD_UnilateBoardThickness.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_UnilateBoardThickness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_UnilateBoardThickness.Location = new System.Drawing.Point(181, 35);
            this.NumUD_UnilateBoardThickness.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_UnilateBoardThickness.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NumUD_UnilateBoardThickness.Name = "NumUD_UnilateBoardThickness";
            this.NumUD_UnilateBoardThickness.Size = new System.Drawing.Size(120, 28);
            this.NumUD_UnilateBoardThickness.TabIndex = 77;
            this.NumUD_UnilateBoardThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_UnilateBoardThickness.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // LblTolerance
            // 
            this.LblTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTolerance.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.LblTolerance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LblTolerance.Location = new System.Drawing.Point(1, 67);
            this.LblTolerance.Margin = new System.Windows.Forms.Padding(1);
            this.LblTolerance.Name = "LblTolerance";
            this.LblTolerance.Size = new System.Drawing.Size(178, 31);
            this.LblTolerance.TabIndex = 78;
            this.LblTolerance.Text = "脹縮值(mm)：";
            this.LblTolerance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_Tolerance
            // 
            this.NumUD_Tolerance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_Tolerance.DecimalPlaces = 2;
            this.NumUD_Tolerance.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_Tolerance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumUD_Tolerance.Location = new System.Drawing.Point(181, 68);
            this.NumUD_Tolerance.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_Tolerance.Name = "NumUD_Tolerance";
            this.NumUD_Tolerance.Size = new System.Drawing.Size(120, 28);
            this.NumUD_Tolerance.TabIndex = 77;
            this.NumUD_Tolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_Tolerance.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // Lbl_PCBNumBySet_L
            // 
            this.Lbl_PCBNumBySet_L.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_PCBNumBySet_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.Lbl_PCBNumBySet_L.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Lbl_PCBNumBySet_L.Location = new System.Drawing.Point(1, 100);
            this.Lbl_PCBNumBySet_L.Margin = new System.Windows.Forms.Padding(1);
            this.Lbl_PCBNumBySet_L.Name = "Lbl_PCBNumBySet_L";
            this.Lbl_PCBNumBySet_L.Size = new System.Drawing.Size(178, 31);
            this.Lbl_PCBNumBySet_L.TabIndex = 78;
            this.Lbl_PCBNumBySet_L.Text = "每疊PCB片數：";
            this.Lbl_PCBNumBySet_L.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_PCBNumBySet_L
            // 
            this.NumUD_PCBNumBySet_L.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_PCBNumBySet_L.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_PCBNumBySet_L.Location = new System.Drawing.Point(181, 101);
            this.NumUD_PCBNumBySet_L.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_PCBNumBySet_L.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumUD_PCBNumBySet_L.Name = "NumUD_PCBNumBySet_L";
            this.NumUD_PCBNumBySet_L.Size = new System.Drawing.Size(120, 28);
            this.NumUD_PCBNumBySet_L.TabIndex = 77;
            this.NumUD_PCBNumBySet_L.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_PCBNumBySet_L.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // Other_UnLoadPin
            // 
            this.Other_UnLoadPin.Controls.Add(this.TabLP_Other_UnLoadPin);
            this.Other_UnLoadPin.Location = new System.Drawing.Point(4, 28);
            this.Other_UnLoadPin.Name = "Other_UnLoadPin";
            this.Other_UnLoadPin.Size = new System.Drawing.Size(612, 209);
            this.Other_UnLoadPin.TabIndex = 2;
            this.Other_UnLoadPin.Text = "退Pin";
            this.Other_UnLoadPin.UseVisualStyleBackColor = true;
            // 
            // TabLP_Other_UnLoadPin
            // 
            this.TabLP_Other_UnLoadPin.ColumnCount = 4;
            this.TabLP_Other_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TabLP_Other_UnLoadPin.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TabLP_Other_UnLoadPin.Controls.Add(this.Lbl_PCBNumBySet_UnL, 0, 0);
            this.TabLP_Other_UnLoadPin.Controls.Add(this.NumUD_PCBNumBySet_UnL, 1, 0);
            this.TabLP_Other_UnLoadPin.Location = new System.Drawing.Point(4, 4);
            this.TabLP_Other_UnLoadPin.Name = "TabLP_Other_UnLoadPin";
            this.TabLP_Other_UnLoadPin.RowCount = 6;
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542F));
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708F));
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_UnLoadPin.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.TabLP_Other_UnLoadPin.Size = new System.Drawing.Size(604, 200);
            this.TabLP_Other_UnLoadPin.TabIndex = 79;
            // 
            // Lbl_PCBNumBySet_UnL
            // 
            this.Lbl_PCBNumBySet_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lbl_PCBNumBySet_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.Lbl_PCBNumBySet_UnL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Lbl_PCBNumBySet_UnL.Location = new System.Drawing.Point(1, 1);
            this.Lbl_PCBNumBySet_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.Lbl_PCBNumBySet_UnL.Name = "Lbl_PCBNumBySet_UnL";
            this.Lbl_PCBNumBySet_UnL.Size = new System.Drawing.Size(178, 31);
            this.Lbl_PCBNumBySet_UnL.TabIndex = 78;
            this.Lbl_PCBNumBySet_UnL.Text = "每疊PCB片數：";
            this.Lbl_PCBNumBySet_UnL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumUD_PCBNumBySet_UnL
            // 
            this.NumUD_PCBNumBySet_UnL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumUD_PCBNumBySet_UnL.Font = new System.Drawing.Font("Microsoft JhengHei UI", 12F);
            this.NumUD_PCBNumBySet_UnL.Location = new System.Drawing.Point(181, 2);
            this.NumUD_PCBNumBySet_UnL.Margin = new System.Windows.Forms.Padding(1);
            this.NumUD_PCBNumBySet_UnL.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumUD_PCBNumBySet_UnL.Name = "NumUD_PCBNumBySet_UnL";
            this.NumUD_PCBNumBySet_UnL.Size = new System.Drawing.Size(120, 28);
            this.NumUD_PCBNumBySet_UnL.TabIndex = 77;
            this.NumUD_PCBNumBySet_UnL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumUD_PCBNumBySet_UnL.Click += new System.EventHandler(this.ShowNumKeyboard);
            // 
            // RecipeTimer
            // 
            this.RecipeTimer.Tick += new System.EventHandler(this.RecipeTimer_Tick);
            // 
            // FmRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 456);
            this.ControlBox = false;
            this.Controls.Add(this.TabCtl_Recipe);
            this.Controls.Add(this.TLP_2);
            this.Controls.Add(this.TLP_1);
            this.Controls.Add(this.DGVTable);
            this.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "FmRecipe";
            this.Text = "Recipe";
            this.Load += new System.EventHandler(this.FmRecipe_Load);
            this.VisibleChanged += new System.EventHandler(this.FmRecipe_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.DGVTable)).EndInit();
            this.TLP_1.ResumeLayout(false);
            this.TLP_1.PerformLayout();
            this.TLP_2.ResumeLayout(false);
            this.TabCtl_Recipe.ResumeLayout(false);
            this.Board.ResumeLayout(false);
            this.TabCtl_BasicSet.ResumeLayout(false);
            this.Basic_Board.ResumeLayout(false);
            this.TabLP_BoardBasic.ResumeLayout(false);
            this.TabLP_BoardBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardThickness)).EndInit();
            this.Basic_Board_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_Standard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidth_Standard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHigh_Standard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHighMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidthMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHighMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardWidthMin)).EndInit();
            this.Basic_LoadPin.ResumeLayout(false);
            this.TabLP_BoardBasic_LoadPin.ResumeLayout(false);
            this.TabLP_BoardBasic_LoadPin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_A2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_A1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis)).EndInit();
            this.Basic_LoadPin_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_LoadPin_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_LoadPin_Standard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_Standard_L)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard_A2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard_A1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardFoolHoleDis_Standard)).EndInit();
            this.Basic_UnLoadPin.ResumeLayout(false);
            this.TabLP_BoardBasic_UnLoadPin.ResumeLayout(false);
            this.TabLP_BoardBasic_UnLoadPin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_UnL)).EndInit();
            this.Basic_UnLoadPin_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_UnLoadPin_Standard.ResumeLayout(false);
            this.TabLP_BoardBasic_UnLoadPin_Standard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_BoardHoleDis_Standard_UnL)).EndInit();
            this.SubPanel_Board_Basic.ResumeLayout(false);
            this.SubPanel_Board_Basic.PerformLayout();
            this.Other.ResumeLayout(false);
            this.TabCtl_OtherSet.ResumeLayout(false);
            this.Other_Basic.ResumeLayout(false);
            this.TabLP_Other_Normal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_ExportTimeMin)).EndInit();
            this.Other_LoadPin.ResumeLayout(false);
            this.TabLP_Other_LoadPin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_AlBoardThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_UnilateBoardThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_Tolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PCBNumBySet_L)).EndInit();
            this.Other_UnLoadPin.ResumeLayout(false);
            this.TabLP_Other_UnLoadPin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumUD_PCBNumBySet_UnL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnModify;
        private System.Windows.Forms.DataGridView DGVTable;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Label LblPartNo;
        private System.Windows.Forms.Label LblRecipeName;
        private System.Windows.Forms.Label LblRecipeTitle;
        private System.Windows.Forms.TableLayoutPanel TLP_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.TableLayoutPanel TLP_2;
        private System.Windows.Forms.TabControl TabCtl_Recipe;
        private System.Windows.Forms.TabPage Board;
        private System.Windows.Forms.Panel SubPanel_Board_Basic;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHigh;
        private System.Windows.Forms.NumericUpDown NumUD_BoardWidth;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic;
        private System.Windows.Forms.NumericUpDown NumUD_ExportTimeMin;
        private System.Windows.Forms.Label LblExportTimeMin;
        private System.Windows.Forms.Label LblBoardThickness;
        private System.Windows.Forms.NumericUpDown NumUD_BoardThickness;
        private System.Windows.Forms.TabPage Other;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHoleDis_UnL;
        private System.Windows.Forms.TabControl TabCtl_BasicSet;
        private System.Windows.Forms.TabPage Basic_Board;
        private System.Windows.Forms.TabPage Basic_LoadPin;
        private System.Windows.Forms.TabPage Basic_UnLoadPin;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic_LoadPin;
        private System.Windows.Forms.Label LblBoardFoolHoleDis;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic_UnLoadPin;
        private System.Windows.Forms.Label LblBoardHoleDis_UnL;
        private System.Windows.Forms.Label LblBoardHigh;
        private System.Windows.Forms.Label LblBoardWidth;
        private System.Windows.Forms.Panel SubPanel_Dir;
        private System.Windows.Forms.RadioButton RBtn_A1;
        private System.Windows.Forms.RadioButton RBtn_A2;
        private System.Windows.Forms.Label LblBoardFoolHoleDis_A1;
        private System.Windows.Forms.Label LblBoardFoolHoleDis_A2;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis_A1;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis_A2;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHoleDis_L;
        private System.Windows.Forms.Label LblBoardHoleDis_L;
        private System.Windows.Forms.Timer RecipeTimer;
        private System.Windows.Forms.CheckBox ChBox_CheckFoolHole;
        private System.Windows.Forms.Label LblBoardStackingMethod;
        private System.Windows.Forms.ComboBox Cmbox_BoardStackingMethod;
        private System.Windows.Forms.NumericUpDown NumUD_AlBoardThickness;
        private System.Windows.Forms.Label LblUnilateBoardThickness;
        private System.Windows.Forms.NumericUpDown NumUD_UnilateBoardThickness;
        private System.Windows.Forms.Label LblTolerance;
        private System.Windows.Forms.NumericUpDown NumUD_Tolerance;
        private System.Windows.Forms.TabControl TabCtl_OtherSet;
        private System.Windows.Forms.TabPage Other_Basic;
        private System.Windows.Forms.TableLayoutPanel TabLP_Other_Normal;
        private System.Windows.Forms.TabPage Other_LoadPin;
        private System.Windows.Forms.TableLayoutPanel TabLP_Other_LoadPin;
        private System.Windows.Forms.Label Lbl_PCBNumBySet_L;
        private System.Windows.Forms.NumericUpDown NumUD_PCBNumBySet_L;
        private System.Windows.Forms.TabPage Basic_Board_Standard;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic_Standard;
        private System.Windows.Forms.Label LblBoardWidth_Standard;
        private System.Windows.Forms.Label LblBoardHigh_Standard;
        private System.Windows.Forms.NumericUpDown NumUD_BoardWidth_Standard;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHigh_Standard;
        private System.Windows.Forms.TabPage Basic_UnLoadPin_Standard;
        private System.Windows.Forms.TabPage Basic_LoadPin_Standard;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic_LoadPin_Standard;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHoleDis_Standard_L;
        private System.Windows.Forms.Label LblBoardHoleDis_Standard_L;
        private System.Windows.Forms.Label LblBoardFoolHoleDis_Standard_A2;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis_Standard_A2;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis_Standard_A1;
        private System.Windows.Forms.Label LblBoardFoolHoleDis_Standatd_A1;
        private System.Windows.Forms.Label LblBoardFoolHoleDis_Standard;
        private System.Windows.Forms.NumericUpDown NumUD_BoardFoolHoleDis_Standard;
        private System.Windows.Forms.TableLayoutPanel TabLP_BoardBasic_UnLoadPin_Standard;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHoleDis_Standard_UnL;
        private System.Windows.Forms.Label LblBoardHoleDis_Standard_UnL;
        private System.Windows.Forms.TabPage Other_UnLoadPin;
        private System.Windows.Forms.TableLayoutPanel TabLP_Other_UnLoadPin;
        private System.Windows.Forms.Label Lbl_PCBNumBySet_UnL;
        private System.Windows.Forms.NumericUpDown NumUD_PCBNumBySet_UnL;
        private System.Windows.Forms.Label LblAlBoardThickness;
        private System.Windows.Forms.Label LblBoardHighMax;
        private System.Windows.Forms.Label LblBoardWidthMax;
        private System.Windows.Forms.Label LblBoardHighMin;
        private System.Windows.Forms.Label LblBoardWidthMin;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHighMax;
        private System.Windows.Forms.NumericUpDown NumUD_BoardWidthMax;
        private System.Windows.Forms.NumericUpDown NumUD_BoardHighMin;
        private System.Windows.Forms.NumericUpDown NumUD_BoardWidthMin;
    }
}