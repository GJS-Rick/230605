# 功能指令名稱與使用範例。

## VacSucker.cs
主要使用方法VacSuckerDef(string sFilePath = "C:\\Automation\\Mod.ini")
需先設定吸盤元件名稱於EVacSuckerName
```C#
public enum EVacSuckerName
    {
        SCARA,

        Count
    }
```
設定吸盤元件名稱後再透過UI設定對應的吸盤元件DIO並保存即可
### 動作編輯中使用方法說明
震動缸
|方法|描述|
|---|---|
|SuckerDone | 確認震動次數是否完成 |
|ShockTimeReset | 震動間隔時間初始化 |
|Up | 震動缸上升 |
|Down | 震動缸下降 |
|GetSuckerAction | 取得震動缸目前動作 |
|GetPreSuckerAction | 取得震動缸上一步動作 |

真空
|方法|描述||
|---|---|---|
|CheckVac | 確認真空 | 依據方法Retract、Extension自動判定|
|GetVacName | 取得氣缸到位感測器名稱 ||
|BtnOff_Start | 真空解除按鈕開始 | 針對吸盤上真空解除按鈕使用|
|BtnOff_Check | 確認真空解除按鈕計時是否到達 | 針對吸盤上真空解除按鈕使用|
|On | 真空開 ||
|Off | 真空關 ||
|Break | 真空關並吹氣 | return string|
|GetVacAction | 取得真空目前動作 ||
|GetPreVacAction | 取得真空上一步動作 ||

## Cylinder.cs
主要使用方法CylinderDef(string sFilePath = "C:\\Automation\\Mod.ini")
需先設定氣缸元件名稱於ECylName
```C#
public enum ECylName
    {
        /// <summary>入料移載夾爪1</summary>
        Import_Jaws1,
        /// <summary>入料移載夾爪2</summary>
        Import_Jaws2,
        Count
    }
```
設定氣缸元件名稱後再透過UI設定對應的氣缸元件DIO並保存即可
### 動作編輯中使用方法說明
|方法|描述||
|---|---|---|
|CheckIO | 確認氣缸是否到位 | 依據方法Retract、Extension自動判定|
|Retract | 氣缸伸出 | 指定某一氣缸伸出|
|Extension | 氣缸縮回 | 指定某一氣缸縮回|
|GetIOName | 取得氣缸到位感測器名稱 | return string|
|GetCYLStatus | 取得氣缸目前動作 ||
|GetPreCYLStatus | 取得氣缸上一步動作 ||

## Door.cs
主要使用方法DoorDef(string sFilePath = "C:\\Automation\\Mod.ini")
需先設定單片門板名稱於EDoorPos
```C#
public enum EDoorPos
    {
        FrontDoor,
        Count
    }
```
設定單片門板名稱後再透過UI設定對應的門板DIO與自動上鎖時間並保存即可
### 動作編輯中使用方法說明
|方法|描述||
|---|---|---|
|Unlock | 解鎖門板 | 可解鎖所有門板或某一側門板|
|Lock | 上鎖門板 | 可上鎖所有門板或某一側門板|
|SingleUnlock | 解鎖單門板 ||
|SingleLock | 上鎖單門板 ||
|AutoLock | 自動計時並上鎖門板 ||
|DoorSafty | 確認所有門板是否關上 | 不一定上鎖|

## CycleTime.cs
主要使用方法CycleTime()
預定宣告於CommonManagerDef.cs
```C#
public CycleTime Cycle = new CycleTime();
```

CycleTime名稱自行定義，結束時名稱必須相同，否則不會停止計時。

週期計時啟動與停止範例：
```C#
public override void AutoExecute()
{
    if (_Pause && _MoveStep.GetStep() != _MoveStep.GetPreStep())
        return;

    switch ((EAuto)_AutoStep.GetStep())
    {
        case EAuto.Adj:
            if (_AutoStep.FirstRun())
            {
                G.Common.Cycle.Start("Adj");
                Move(EMoveType.Adj);
            }
            if (MoveDone())
            {
                G.Common.Cycle.Stop("Adj");
                _AutoStep.SetStep(EAuto.WaitSCARASingle);
            }
            break;
    }
}
```

週期計時全部停止時機範例(按下停止按鈕時)：
```C#
public void UserSetStatus(ERunStatus eStatus)
{
    switch (eStatus)
    {
        case ERunStatus.Stop:
            G.Common.Cycle.Stop();
            break;
    }
}
```

### 動作編輯中使用方法說明
|方法|描述||
|---|---|---|
|Start | 啟動計時 | 需建立週期名稱|
|Stop | 停止計時 | 名稱需與建立的週期名稱相同才會停止計時，不給指定名稱則停止全部週期計時|
|GetAllName | 取得所有週期名稱 | 需Start後才會有名稱|

## Trolley.cs
主要使用方法TrolleyDef(_SystemDirPath + "\\Mod.ini")
需先設定台車名稱於ETrolley
```C#
public enum ETrolley
    {
        Trolley,

        Count
    }
```
設定台車名稱後再透過UI設定對應的台車感測器、氣缸與自動上鎖時間並保存即可
### 動作編輯中使用方法說明
|方法|描述||
|---|---|---|
|Unlock | 台車解鎖 | 可解鎖指定台車|
|Lock | 台車上鎖 | 上鎖指定台車|
|IsInPlace | 確認台車是否到位 ||
|IsLock | 確認台車是否上鎖 ||
|AutoLock | 自動計時並上鎖台車 ||