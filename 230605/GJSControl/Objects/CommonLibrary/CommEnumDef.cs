using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    #region Axis
    public enum EAXIS_NAME
    {
       Robot,
       DrillsAlign_Servo,
       DrillsAlign_Step,
       InputCV,
       OutputCV,
       CV_Align,

        Count,
    }
    #endregion

    #region DIO
    public enum EDI_TYPE
    {

        //常設
        #region//0X
        /// <summary>0X00 急停開關</summary>
        EmgStop,
        /// <summary>0X01 重置鈕</summary>
        BtnReset,
        /// <summary>0X02 啟動鈕 </summary>
        BtnStart,
        /// <summary>0X03 停止鈕</summary>
        BtnStop,
        /// <summary>0X04_入氣壓力偵測</summary>
        AirPressure_Main,
        /// <summary>0X05_入口滾輪入料光電</summary>
        Sensor_ImportCV_PanelIn,
        /// <summary>0X06_入口滾輪在籍光電1</summary>
        Sensor_ImportCV_PanelInPosition_1,
        /// <summary>0X07_入口滾輪在籍光電2</summary>
        Sensor_ImportCV_PanelInPosition_2,
        /// <summary>0X08_出口滾輪入料光電</summary>
        Sensor_ExportCV_PanelIn,
        /// <summary>0X09_出口滾滾輪在籍光電1</summary>
        Sensor_ExportCV_PanelInPosition_1,
        /// <summary>0X10_出口滾滾輪在籍光電2</summary>
        Sensor_ExportCV_PanelInPosition_2,
        /// <summary>0X11_出口滾輪出料光電</summary>
        Senesor_ExportCV_PanelOut,
        /// <summary>0X12_預留</summary>
        None_0X12,
        /// <summary>0X13_預留</summary>
        None_0X13,
        /// <summary>0X14 出口滾輪整版前限光素子</summary>
        Sensor_ExportCV_Align_LSP,
        /// <summary>0X15 出口滾輪整版後限光素子</summary>
        Sensor_ExportCV_Align_LSN,
        #endregion



        /// <summary>1X00 置料有料上極限光電(NC)</summary>
        Sensor_Lifts_PanelLimit,
        /// <summary>1X01 置料有料上定位光電(NC)</summary>
        Sensor_Lifts_InPosition,
        /// <summary>1X02 置料上極限開關</summary>
        LMS_Lifts_Top,
        /// <summary>1X03 置料上限光素子</summary>
        Sensor_Lifts_Top,
        /// <summary>1X04 置料下限光素子</summary>
        Sensor_Lifts_Down,
        /// <summary>1X05 置料下極限開關</summary>
        LMS_Lifts_Down,
        /// <summary>1X06 棧板在籍偵測</summary>
        Sensor_Pallet_Inposition,
        /// <summary>1X07 棧板缺料光電(預留)</summary>
        Sensor_Pallet_PanelEmpty,
        /// <summary>1X08 預留</summary>
        None_1X08,
        /// <summary>1X09 手臂前限光素子</summary>
        Sensor_Robot_Lsp,
        /// <summary>1X10 手臂原點光素子</summary>
        Sensor_Robot_Home,
        /// <summary>1X11 手臂換刀位光素子</summary>
        Sensor_Robot_ChengeDrills,
        /// <summary>1X12 手臂後限光素子</summary>
        Sensor_Robot_Lsn,
        /// <summary>1X13 預留</summary>
        None_1X13,
        /// <summary>1X14 取板上限磁簧</summary>
        Sensor_TakePanelCylinder_Top,
        /// <summary>1X15 取板下限磁簧</summary>
        Sensor_TakePanelCylinder_Down,



        /// <summary>2X00 頂板氣缸上限磁簧</summary>
        Sensor_PressPanelCylinder_Top,
        /// <summary>2X01 頂板氣缸下限磁簧</summary>
        Sensor_PressPanelCylinder_Down,
        /// <summary>2X02 取板吸盤到位光素子/summary>
        Sensor_TakePanel_InPosition,
        /// <summary>2X03 取板吸盤過壓光素子/summary>
        Sensor_TakePanel_Overpress,
        /// <summary>2X04 取板真空負壓偵測/summary>
        Sensor_TakePanel_Vacuum,
        /// <summary>2X05 放板氣缸上限磁簧/summary>
        Sensor_PutPanelCylinder_Top,
        /// <summary>2X06 放板氣缸下限磁簧/summary>
        Sensor_PutPanelCylinder_Down,
        /// <summary>2X07 放板吸盤到位光素子/summary>
        Sensor_PutPanel_InPosition,
        /// <summary>2X08 放板吸盤過壓光素子/summary>
        Sensor_PutPanel_Overpress,
        /// <summary>2X09 鑽孔真空負壓偵測/summary>
        Sensor_PutPanel_Vacuum,
        /// <summary>2X10 操作側左門左限/summary>
        Door_Operate_LL,
        /// <summary>2X11 操作側左門右限/summary>
        Door_Operate_LR,
        /// <summary>2X12 操作側右門左限/summary>
        Door_Operate_RL,
        /// <summary>2X13 操作側右門右限/summary>
        Door_Operate_RR,
        /// <summary>2X14 反操作側左門左限/summary>
        Door_OppositeOperate_LL,
        /// <summary>2X15 反操作側左門右限/summary>
        Door_OppositeOperate_LR,



        /// <summary>3X00 反操作側右門左限/summary>
        Door_OppositeOperate_RL,
        /// <summary>3X01 反操作側右門右限/summary>
        Door_OppositeOperate_RR,
        /// <summary>3X02 舉升上升按鈕/summary>
        Btn_Lifts_Up,
        /// <summary>3X03 舉升下降按鈕/summary>
        Btn_Lifts_Down,
        /// <summary>3X04 取板破真空按鈕/summary>
        Btn_TakePanel_BreakVacuum,
        /// <summary>3X05 放板破真空按鈕/summary>
        Btn_PutPanel_BreakVacuum,
        /// <summary>3X06 整板伺服正極限/summary>
        Sensor_AlignServo_Lsp,
        /// <summary>3X07 整板伺服負極限/summary>
        Sensor_AlignServo_Lsn,
        /// <summary>3X08 整板伺服原點/summary>
        Sensor_AlignServo_Home,
        /// <summary>3X09 整板步進正極限/summary>
        Sensor_AlignStep_Lsp,
        /// <summary>3X10 整板步進負極限/summary>
        Sensor_AlignStep_Lsn,
        /// <summary>3X11 整板步進原點/summary>
        Sensor_AlignStep_Home,
        /// <summary>3X12 預留</summary>
        None_3X12,
        /// <summary>3X13 預留</summary>
        None_3X13,
        /// <summary>3X14 預留</summary>
        None_3X14,
        /// <summary>3X15 預留</summary>
        None_3X15,


        /// <summary>4X00 A1側夾持氣缸上限/summary>
        Sensor_A1ClampCylinder_Top,
        /// <summary>4X01 A1側夾持氣缸下限/summary>
        Sensor_A1ClampCylinder_Down,
        /// <summary>4X02 A1側夾持蓋板確認/summary>
        Sensor_A1DrillsCoverCheck,
        /// <summary>4X03 A1側鑽孔上升氣缸上限/summary>
        Sensor_A1DrillsCylinder_Top,
        /// <summary>4X04 A1側鑽孔上升氣缸下限/summary>
        Sensor_A1DrillsCylinder_Down,
        /// <summary>4X05 A1側鑽孔上升氣缸擋塊確認/summary>
        Sensor_A1DrillsCylinder_StopperCheck,
        /// <summary>4X06 A2側夾持氣缸上限/summary>
        Sensor_A2ClampCylinder_Top,
        /// <summary>4X07 A2側夾持氣缸下限/summary>
        Sensor_A2ClampCylinder_Down,
        /// <summary>4X08 A2側夾持蓋板確認/summary>
        Sensor_A2DrillsCoverCheck,
        /// <summary>4X09 A2側鑽孔上升氣缸上限/summary>
        Sensor_A2DrillsCylinder_Top,
        /// <summary>4X10 A2側鑽孔上升氣缸下限/summary>
        Sensor_A2DrillsCylinder_Down,
        /// <summary>4X11 A2側鑽孔上升氣缸擋塊確認/summary>
        Sensor_A2DrillsCylinder_StopperCheck,
        /// <summary>4X12 預留</summary>
        None_4X12,
        /// <summary>4X13 預留</summary>
        None_4X13,
        /// <summary>4X14 預留</summary>
        None_4X14,
        /// <summary>4X15 預留</summary>
        None_4X15,


        /// <summary>5X00 預留</summary>
        None_5X00,
        /// <summary>5X01 預留</summary>
        None_5X01,
        /// <summary>5X02  鑽孔在籍光電 </summary>
        Seneor_DrillsPanelInPosition,
        /// <summary>5X03  欠逆相保護 </summary>
        APR_Check,
        /// <summary>5X04 預留</summary>
        None_5X04,
        /// <summary>5X05 預留</summary>
        None_5X05,
        /// <summary>5X06 預留</summary>
        None_5X06,
        /// <summary>5X07 預留</summary>
        None_5X07,
        /// <summary>5X08 舉升馬達異常</summary>
        Alarm_LiftsMotor,
        /// <summary>5X09 手臂步進驅動器異常</summary>
        Alarm_RobotStepDriver,
        /// <summary>5X10 整板伺服驅動器異常</summary>
        Alarm_AlignServoDriver,
        /// <summary>5X11 預留</summary>
        None_5X11,
        /// <summary>5X12 A1定位光電</summary>
        Sensor_A1_PanelInPosition,
        /// <summary>5X13 A2定位光電</summary>
        Sensor_A2_PanelInPosition,
        /// <summary>5X14 外部連線輸入2</summary>
        ExternalConnect_In2,
        /// <summary>5X15 外部連線輸入1</summary>
        ExternalConnect_In1,


        DI_COUNT
    }
    public enum EDO_TYPE
    {
        //常設
        #region//0Y
        /// <summary>0Y00_塔燈-紅燈</summary>
        StackLight_Red,
        /// <summary>0Y01_塔燈-綠燈</summary>
        StackLight_Green,
        /// <summary>0Y02_塔燈-橙燈</summary>
        StackLight_Yellow,
        /// <summary>0Y03_塔燈-蜂鳴器</summary>
        StackLight_Buzzer,
        /// <summary>0Y04 重置鈕燈</summary>
        LED_BtnReset,
        /// <summary>0Y05 啟動鈕燈</summary>
        LED_BtnStart,
        /// <summary>0Y06 停止鈕燈</summary>
        LED_BtnStop,
        /// <summary>0Y07 舉升上升燈</summary>
        LED_BtnLiftsUp,
        /// <summary>0Y08 舉升下降燈</summary>
        LED_BtnLiftsDown,
        /// <summary>0Y09 取板破真空燈</summary>
        LED_BtnTakePanelBreakVac,
        /// <summary>0Y10 放板破真空燈</summary>
        LED_BtnPutPanelBreakVac,
        /// <summary>0Y11 預留</summary>
        None_OY11,
        /// <summary>0Y12 預留</summary>
        None_OY12,
        /// <summary>0Y13 預留</summary>
        None_OY13,
        /// <summary>0Y14 預留</summary>
        None_OY14,
        /// <summary>0Y15 預留</summary>
        None_OY15,

        #endregion

        /// <summary>1Y00 舉升上升</summary>
        LiftsUp,
        /// <summary>1Y01 舉升下降</summary>
        LiftsDown,
        /// <summary>1Y02 取板真空ON</summary>
        TakePanelVacOn,
        /// <summary>1Y03 取板真空OFF</summary>
        TakePanelVacOff,
        /// <summary>1Y04 取板真空Break</summary>
        TakePanelBreakVac,
        /// <summary>1Y05 取板振動缸_1</summary>
        TakePanelVibration_1,
        /// <summary>1Y06 取板振動缸_2</summary>
        TakePanelVibration_2,
        /// <summary>1Y07 取板壓料氣缸下降</summary>
        TakePanel_PressPanel_Down,
        /// <summary>1Y08 取板真空ON</summary>
        PutPanelVacOn,
        /// <summary>1Y09 取板真空OFF</summary>
        PutPanelVacOff,
        /// <summary>1Y10 取板真空Break</summary>
        PutPanelBreakVac,
        /// <summary>1Y11 預留</summary>
        None_1Y11,
        /// <summary>1Y12 預留</summary>
        None_1Y12,
        /// <summary>1Y13 預留</summary>
        None_1Y13,
        /// <summary>1Y14 A1鑽孔馬達啟動</summary>
        DrillMotorOn_A1,
        /// <summary>1Y15 A2鑽孔馬達啟動</summary>
        DrillMotorOn_A2,


        /// <summary>2Y00 A1&A2側夾持氣缸上升</summary>
        ClampCylinderOn,
        /// <summary>2Y01 預留</summary>
        None_2Y01,
        /// <summary>2Y02 A1&A2側鑽孔氣缸上升</summary>
        DrillsCylinderOn,
        /// <summary>2Y03 預留</summary>
        None_2Y03,
        /// <summary>2Y04 取板吸盤氣缸上升</summary>
        TakePanelCylinderUp,
        /// <summary>2Y05 取板吸盤氣缸下降</summary>
        TakePanelCylinderDown,
        /// <summary>2Y06 放板吸盤氣缸上升</summary>
        PutPanelCylinderUp,
        /// <summary>2Y07 放板吸盤氣缸下降</summary>
        PutPanelCylinderDown,
        /// <summary>2Y08 手臂馬達煞車</summary>
        Robot_Brake,
        /// <summary>2Y09 預留</summary>
        None_2Y09,
        /// <summary>2Y10 預留</summary>
        None_2Y10,
        /// <summary>2Y11 SDR1 ServoON</summary>
        SDR1ServoOn,
        /// <summary>2Y12 鑽頭吹屑</summary>
        DrillsBlowAir,
        /// <summary>2Y13 伺服馬達剎車</summary>
        AlignServoBrake,
        /// <summary>2Y14 外部連線輸出1</summary>
        External_Connect_Out1,
        /// <summary>2Y15 外部連線輸出2</summary>
        External_Connect_Out2,


        DO_COUNT
    }
    #endregion

    #region PanelStatus
    public enum EPanelStatusDouble
    {
        Count
    }

    public enum EPanelStatusInt
    {
        Count
    }

    public enum EPanelStatusString
    {
        Count
    }
    #endregion

    #region StationStatus
    public enum EStation
    {
        Count
    }
    #endregion

    #region TBotPosition
    public enum ETBotPosition
    {
        Count
    }
    #endregion

    #region Robot and SCARA
    /// <summary>Robot</summary>
    public enum ERobotPosition
    {
        Count
    }
   
    /// <summary>Scara</summary>
    public enum EScaraPosition
    {
        Standby,
        J2Back,
        TakeDown,
        TakeTop,
        TakeTopTemp,
        PutTop,
        PutDown,
        Count,
    }
    #endregion

    #region Alarm
    public enum AlarmCode
    {
        #region Basic
        Success,
        Alarm,
        LotEnd,//作業結束
        Warning,
        Fail,
        Alarm_RobotOnDangerZone,
        Alarm_EMOPressed,
        Alarm_AirPressureNotEnough,
        Alarm_SafetyDoorOpen,
        Alarm_TrolleyLeaves,
        Alarm_ComportOpenFail,
        Alarm_NoInital,
        Alarm_AxisError,//軸異常
        Alarm_IOError,//IO異常
        Alarm_ScaraError,//scara異常
        Alarm_CommonObjError,//Common異常
        Alarm_FormatError,
        Alarm_Timeout,
        FmFunctionOver20Btn,//功能按鈕超過20個

        Machine_MotionCardError,
        Machine_DIOCardError,
        Machine_CameraError,
        Machine_RobotConnectionError,
        Machine_ParmError,
        NoCOMPortCanUse,//無可用串口
        #endregion

        

        Count
    }
    #endregion

    #region DataTransfer
    public enum EDataTransferName
    {
        ExampleStation_01,
        ExampleStation_02,

        Count
    }
    #endregion

    #region Mod模組
    #region 氣缸模組
    /// <summary>氣缸名稱</summary>
    public enum ECylName
    {
        Cyl_1,

        Count
    }
    #endregion
    #region 門板模組
    /// <summary>門板名稱</summary>
    public enum EDoorPos
    {

        Count
    }
    #endregion
    #region 升降台模組
    /// <summary>升降台名稱</summary>
    public enum ELifts
    {

        Count
    }
    #endregion
    #region 台車模組
    /// <summary>台車名稱</summary>
    public enum ETrolley
    {

        Count
    }
    #endregion
    #region 真空吸盤模組
    /// <summary>吸盤名稱</summary>
    public enum EVacSuckerName
    {
        SCARA,

        Count
    }
    #endregion
    #region 震動送料機模組
    /// <summary>震動送料機名稱</summary>
    public enum EVibBow
    {

        Count
    }
    #endregion
    #endregion

    #region 常用動作
    /// <summary>變頻器動作</summary>
    public enum EInverterAction
    {
        /// <summary>DeltaVFD主速20-01</summary>
        FastSpeed,
        /// <summary>DeltaVFD慢速04-00</summary>
        SlowSpeed
    }
    /// <summary>取板模式</summary>
    public enum ETakeBoardMode
    {
        SingleBoard,
        DoubleBoard
    }
    /// <summary>投收板順序</summary>
    public enum ETakePutMode
    {
        Order,
        PassMylar
    }
    /// <summary>板子位置</summary>
    public enum EBoardPos
    {
        Left,
        Middle,
        Right
    }
    /// <summary>設備機型</summary>
    public enum EMechanicalModel
    {
        All,

        Basic,

        Load_Pin,
        UnLoad_Pin,

        None,
        Count
    }
    /// <summary>CV位置</summary>
    public enum ECVPos
    {
        Export,

        Count
    }
    #endregion
}

namespace FileStreamLibrary
{
    #region 設備參數
    /// <summary>設備參數名稱(Double)</summary>
    public enum EMachineDouble
    {
        ForkPanelHeight,
        PCBHoleDisTolerance,
        WoodHoleDisTolerance,
        FoolProofHoleYDistance,
        PCBHoleDiameter,
        WoodBoardHoleDiameter,
        CCDGrabDelayTime,

        TapeAboveForwardDistance,
        TapeAboveDistance,
        TapeCutDistance,
        TapeDownDistance,
        TapeBelowDistance,

        TapeA2AboveForwardDistance,
        TapeA2AboveDistance,
        TapeA2CutDistance,
        TapeA2DownDistance,
        TapeA2BelowDistance,

        TapeInspectDistance,
        TapeInspectDelayTime,
        TapingDelayTime,
        TapeCutDelayTime,
        RotateShift,
        PinPutDelayTime,
        AluminumShakeTime,
        PinDownDelayTime,
        VacOffDelayTime,
        CCDRetryDelayTime,
        AfterRotateYShift,







        ShockTimes,
        ShockTime,

        LRackAngle,

        Count
    }
    /// <summary>設備參數名稱(Int)</summary>
    public enum EMachineInt
    {
        PassPCBTimes,
       
        


        ///<summary>保養逾時警報</summary>
        MaintainOvertimeAlarm,
        Count
    }
    /// <summary>設備參數名稱(String)</summary>
    public enum EMachineString
    {

        Count
    }
    #endregion

    #region 配方參數
    public enum ERecipeDouble
    {
        #region 基本參數
        /// <summary>PCB最大板長</summary>
        BoardHeightMax,
        /// <summary>PCB最大板寬</summary>
        BoardWidthMax,
        /// <summary>PCB最小板長</summary>
        BoardHeightMin,
        /// <summary>PCB最小板寬</summary>
        BoardWidthMin,

        /// <summary>PCB寬</summary>
        BoardWidth,
        /// <summary>PCB樣板寬</summary>
        BoardWidth_Standard,
        /// <summary>PCB長</summary>
        BoardHeight,
        /// <summary>PCB樣板長</summary>
        BoardHeight_Standard,
        /// <summary>PCB板厚</summary>
        BoardThickness,

        /// <summary>上Pin孔孔距</summary>
        BoardHoleDistance_L,
        /// <summary>上Pin孔樣板孔距</summary>
        BoardHoleDistance_L_Standard,
        /// <summary>退Pin孔孔距</summary>
        BoardHoleDistance_UnL,
        /// <summary>退Pin孔樣板孔距</summary>
        BoardHoleDistance_UnL_Standard,

        /// <summary>防呆孔孔距</summary>
        BoardFoolHoleDistance,
        /// <summary>防呆孔樣板孔距</summary>
        BoardFoolHoleDistance_Standard,

        // <summary>A1側孔距孔距</summary>
        BoardFoolHoleDistance_A1,
        /// <summary>A1側孔距樣板孔距</summary>
        BoardFoolHoleDistance_A1_Standard,
        // <summary>A2側孔距孔距</summary>
        BoardFoolHoleDistance_A2,
        /// <summary>A2側孔距樣板孔距</summary>
        BoardFoolHoleDistance_A2_Standard,

        /// <summary>最小出板時間</summary>
        ExportTime_Min,
        /// <summary>鋁板板厚</summary>
        AlBoardThickness,
        /// <summary>尿素板板厚</summary>
        UnilateBoardThickness,
        /// <summary>脹縮值尿素板板厚</summary>
        BoardTolerance,
        #endregion

        Count,
    }

    public enum ERecipeInt
    {
        #region 基板參數
        /// <summary>疊板形式</summary>
        BoardStackingMethod,
        /// <summary>啟用防呆孔檢測</summary>
        BoardFoolHoleEnable,
        /// <summary>每疊PCB數(上Pin)</summary>
        PCBNumBySet_L,
        /// <summary>每疊PCB數(退Pin)</summary>
        PCBNumBySet_UnL,
        /// <summary>Pin孔基準(A1=1, A2=2)</summary>
        PinHoldBasic,
        #endregion

        Count,
    }
    #endregion
}

namespace nsSequence
{
    #region Handshake
    /// <summary>HandShake Bool(用於內部訊號交握用，偵測到On時立即Off)</summary>
    public enum EHandshake
    {
        Lifts_TakeCylinder_CanTake,
        TakeCylinder_Lifts_TakeDone,
        Robot_TakeCylinder_CanTake,
        TakeCylinder_Robot_TakeDone,
        Robot_TakeCylinder_CanPut,
        TakeCylinder_Robot_PutDone,
        TakeCylinder_Drills_PutDone,
        PutCylinder_Drill_TakeDone,
        

        Drills_PutCylinder_CanTake,
        Robot_PutCylinder_CanTake,
        PutCylinder_Robot_TakeDone,

        Robot_PutCylinder_CanPut,
        OutputCV_PutCylinder_CanPut,

        PutCylinder_OutputCV_PutDone,
        PutCylinder_Robot_PutDone,

        ExportCV_ImportCV_CanPass,

        Putcylinder_Robot_InSafeZone,
        Takecylinder_Robot_InSafeZone,



        Count
    }
    /// <summary>HandShake Bool(用於內部訊號交握用，需另行設定On或Off才會變更)</summary>
    public enum EDataBool
    {
        LastPanel,

        Count
    }
    /// <summary>HandShake Int(用於內部訊號交握用)</summary>
    public enum EDataInt
    {
        TapeIndex,
        PinPanelCount,
        CCDPanelCount,

        Count
    }
    /// <summary>HandShake Double(用於內部訊號交握用)</summary>
    public enum EDataDouble
    {
        LRackMylarShiftX,
        LRackMylarShiftY,
        LRackMylarShiftZ,

        LRackPCBShiftX,
        LRackPCBShiftY,

        PalletShiftX,
        PalletShiftY,

        PanelTrayShiftX,
        PanelTrayShiftY,

        ShiftX,
        ShiftY,
        ShiftAngle,

        

        Count
    }
    #endregion
}

namespace VisionLibrary
{
    #region 存圖資料夾名稱
    public enum EImgDirPath
    {
        Original,
        ProcessImage,
        MatchPreprocessing,
        MatchNG,
        CornerNG,
        BlobAndPointNG,
        AlignByCornerBoxNG,
        CheckItem,
        AutoMatch,

        EdgeX,
        EdgeXThreshold,
        EdgeY,
        EdgeYThreshold,
        BeforeMerge,
        Paint,
        Src,
        Result,

        Threshold,
        Erode,
        Dilation,

        Other,
    }
    #endregion
}