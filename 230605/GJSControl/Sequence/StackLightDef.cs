using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace nsSequence
{
    internal class StackLightDef
    {
        private enum LightType
        {
            /// <summary>捷惠</summary>
            JHMCO,
            /// <summary>高加盛</summary>
            GJSTCO,
            Other
        }

        public enum LightStatus
        {
            Stop,
            Stop_Alarm,
            Stop_Warning,

            Manual,
            Manual_Alarm,
            Manual_Warning,

            Auto,
            Auto_Alarm,
            Auto_Warning,

            BeforeInitial,
            BeforeInitial_Alarm,
            BeforeInitial_Warning,

            Initial,
            Initial_Alarm,
            Initial_Warning,

            Pause,
            Pause_Alarm,
            Pause_Warning,

            Teach,
            Teach_Alarm,
            Teach_Warning,

            End,
            End_Alarm,
            End_Warning,

            GoStandby,
            GoStandby_Alarm,
            GoStandby_Warning,

            Alarm,
            Warning,
            None,
        }
        private Thread RunningThread;
        private bool _IsBlink;
        private bool _End;
        private int _BlinkInterval = 1000;
        private LightType _LightType;
        private CommonLibrary.ERunStatus _Status;
        LightStatus _LightStatus = LightStatus.Stop;

        public StackLightDef()
        {
            _LightType = LightType.JHMCO;
            RunningThread = new Thread(Execute);
            RunningThread.Priority = ThreadPriority.Lowest;
            RunningThread.Start();
        }

        public void SetStatus(CommonLibrary.ERunStatus status, CommonLibrary.AlarmType alarmType, bool isTimeout)
        {
            if (status != _Status)
                _IsBlink = true;

            //Analyze machine status
            switch (status)
            {
                case CommonLibrary.ERunStatus.BeforeInitial:
                    if (alarmType == CommonLibrary.AlarmType.Warning)
                        _LightStatus = LightStatus.BeforeInitial_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.BeforeInitial_Alarm;
                    else
                        _LightStatus = LightStatus.BeforeInitial;
                    break;

                case CommonLibrary.ERunStatus.Stop:
                    if (alarmType == CommonLibrary.AlarmType.Warning)
                        _LightStatus = LightStatus.Stop_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.Stop_Alarm;
                    else
                        _LightStatus = LightStatus.Stop;
                    break;

                case CommonLibrary.ERunStatus.Auto:
                    if (alarmType == CommonLibrary.AlarmType.Warning || isTimeout)
                        _LightStatus = LightStatus.Auto_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.Auto_Alarm;
                    else
                        _LightStatus = LightStatus.Auto;
                    break;

                case CommonLibrary.ERunStatus.End:
                    if (alarmType == CommonLibrary.AlarmType.Warning || isTimeout)
                        _LightStatus = LightStatus.End_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.End_Alarm;
                    else
                        _LightStatus = LightStatus.End;
                    break;

                case CommonLibrary.ERunStatus.Initial:
                    if (alarmType == CommonLibrary.AlarmType.Warning)
                        _LightStatus = LightStatus.Initial_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.Initial_Alarm;
                    else
                        _LightStatus = LightStatus.Initial;
                    break;

                case CommonLibrary.ERunStatus.Pause:
                    if (alarmType == CommonLibrary.AlarmType.Warning || isTimeout)
                        _LightStatus = LightStatus.Pause_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.Pause_Alarm;
                    else
                        _LightStatus = LightStatus.Pause;
                    break;

                case CommonLibrary.ERunStatus.Teach:
                    if (alarmType == CommonLibrary.AlarmType.Warning || isTimeout)
                        _LightStatus = LightStatus.Teach_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.Teach_Alarm;
                    else
                        _LightStatus = LightStatus.Teach;
                    break;

                case CommonLibrary.ERunStatus.GoStandby:
                    if (alarmType == CommonLibrary.AlarmType.Warning)
                        _LightStatus = LightStatus.GoStandby_Warning;
                    else if (alarmType == CommonLibrary.AlarmType.Alarm)
                        _LightStatus = LightStatus.GoStandby_Alarm;
                    else
                        _LightStatus = LightStatus.GoStandby;
                    break;

                case CommonLibrary.ERunStatus.Alarm:
                    _LightStatus = LightStatus.Alarm;
                    break;

                case CommonLibrary.ERunStatus.Manual:
                    _LightStatus = LightStatus.Manual;
                    break;
            }
            _Status = status;
        }

        private void SetStackLight(bool red, bool green, bool yellow, bool buzzer, bool alarmSpeaker, bool warningSpeaker)
        {
            G.Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_Red, red);
            G.Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_Green, green);
            G.Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_Yellow, yellow);
            G.Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_Buzzer, buzzer && !G.Comm.VolumeMute);
            //_Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_AlarmSpeaker, alarmSpeaker);
            //_Comm.IOCtrl.SetDO(CommonLibrary.EDO_TYPE.StackLight_WarningSpeaker, warningSpeaker);
        }

        public void Execute()
        {
            while (!_End)
            {
                switch (_LightStatus)
                {
                    case LightStatus.Stop:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, true, false, false, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, true, true, false, false, false);
                                break;
                            default:
                                SetStackLight(false, true, true, false, false, false);
                                break;
                        }
                        break;
                    case LightStatus.Stop_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Stop_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, true, true, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, true, true, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Manual:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                            default:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Manual_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Manual_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, true, _IsBlink, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, true, _IsBlink, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Auto:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                            default:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Auto_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Auto_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, true, false, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, true, false, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.BeforeInitial:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, false, true, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, false, true, false, true, false);
                                break;
                            default:
                                SetStackLight(false, false, true, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.BeforeInitial_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.BeforeInitial_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, false, _IsBlink, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, false, _IsBlink, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Initial:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, _IsBlink, _IsBlink, false, true, false);
                                break;
                            default:
                                SetStackLight(false, _IsBlink, _IsBlink, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Initial_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Initial_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(!_IsBlink, _IsBlink, _IsBlink, !_IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(!_IsBlink, _IsBlink, _IsBlink, !_IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Pause:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, _IsBlink, false, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, _IsBlink, false, false, true, false);
                                break;
                            default:
                                SetStackLight(false, _IsBlink, false, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Pause_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Pause_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, _IsBlink, false, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, _IsBlink, false, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Teach:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                            default:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Teach_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Teach_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, true, _IsBlink, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, true, _IsBlink, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.End:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, _IsBlink, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                            default:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.End_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.End_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, true, false, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, true, false, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.GoStandby:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(false, true, false, false, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(false, _IsBlink, _IsBlink, false, true, false);
                                break;
                            default:
                                SetStackLight(false, _IsBlink, _IsBlink, false, true, false);
                                break;
                        }
                        break;
                    case LightStatus.GoStandby_Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.GoStandby_Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(!_IsBlink, _IsBlink, _IsBlink, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(!_IsBlink, _IsBlink, _IsBlink, _IsBlink, false, true);
                                break;
                        }
                        break;
                    case LightStatus.Alarm:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                            default:
                                SetStackLight(true, false, false, _IsBlink, true, false);
                                break;
                        }
                        break;
                    case LightStatus.Warning:
                        switch (_LightType)
                        {
                            case LightType.JHMCO:
                                SetStackLight(true, true, false, _IsBlink, false, true);
                                break;
                            case LightType.GJSTCO:
                                SetStackLight(_IsBlink, false, false, _IsBlink, false, true);
                                break;
                            default:
                                SetStackLight(_IsBlink, false, false, _IsBlink, false, true);
                                break;
                        }
                        break;
                }
                Thread.Sleep(_BlinkInterval);
                _IsBlink = !_IsBlink;
            }
        }

        public virtual void Dispose()
        {
            _End = true;

            while (RunningThread != null && RunningThread.IsAlive)
            {
                Thread.Sleep(5);
            }
        }
    }
}