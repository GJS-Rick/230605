using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace nsSequence
{
    public enum EStation
    {
        Loader,
        UpperMover,
        Pin,
        Tape,
        Unloader,
        Count
    }

    public class StationStatusDef : IDisposable
    {
        private string[] _ID;
        private bool[] _PanelEnable;
        private bool _pinned;
        private int _LayerCount;
        private int _LayerLimit;
        public StationStatusDef()
        {
            _pinned = false;
            _ID = new string [(int)EStation.Count];
            _PanelEnable = new bool [(int)EStation.Count];
            _LayerLimit = 3;
            _LayerCount = 0;
            for (int i = 0; i < (int)EStation.Count; i++)
            {
                _ID[i] = "";
                _PanelEnable[i] = false;
            }
        }

        public void Dispose()
        {
        }

        public void Clear()
        {
            for (int i = 0; i < (int)EStation.Count; i++)
            {
                _ID[i] = "";
                _PanelEnable[i] = false;
            }
            _LayerCount = 0;
        }

        public void SetLayerLimit(int n)
        {
            _LayerLimit = n;
        }

        public string[] GetID(EStation station)
        {
            return _ID[(int)station].Split(',');
        }

        public void Remove(EStation station)
        {
            _PanelEnable[(int)EStation.Loader] = false;
            _ID[(int)EStation.Loader] = "";
        }

        public void Import(string ID)
        {
           
            if (!_PanelEnable[(int)EStation.Loader])
            {
                _PanelEnable[(int)EStation.Loader] = true;
                _ID[(int)EStation.Loader] = ID;
            }
        }

        public void SetPinned()
        {
            _pinned = true;
        }

        public bool Full(EStation station)
        {
            if (station == EStation.Pin)
            {
                if (_LayerCount < _LayerLimit)
                    return false;
            }
            else
            {
                if (!_PanelEnable[(int)station])
                    return false;
            }

            return true;
        }
        public void ToNext(EStation station)
        {
            if(station == EStation.Unloader)
            {
                _PanelEnable[(int)station] = false;
                _ID[(int)station] = "";
                return;

            }

            else if (station == EStation.UpperMover)
            {
                if (_PanelEnable[(int)station] && !Full(EStation.Pin))
                {
                    _PanelEnable[(int)station + 1] = true;
                    _ID[(int)station + 1] += _ID[(int)station] + ",";
                    _LayerCount++;

                    _PanelEnable[(int)station] = false;
                    _ID[(int)station] = "";

                    return;
                }
            }
          
            if (_PanelEnable[(int)station] && !_PanelEnable[(int)station + 1])
            {
                _PanelEnable[(int)station + 1] = true;
                _ID[(int)station + 1] = _ID[(int)station];

                _PanelEnable[(int)station] = false;
                _ID[(int)station] = "";

                if (station == EStation.Pin)
                {
                    _pinned = false;
                    _LayerCount = 0;
                }
            }
        }
    }
}
