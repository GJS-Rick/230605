using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStreamLibrary;


namespace CommonLibrary
{
    
    public class PanelStatusDef : IDisposable
    {

        private double[] _ValueDouble;
        private int[] _ValueInt;
        private string[] _ValueString;

        // <summary>
        /// 預設建構子
        /// </summary>
        public PanelStatusDef()
        {
            InitializeComponet();
        }
        // <summary>
        /// 複製建構子
        /// </summary>
        public PanelStatusDef(PanelStatusDef Old)
        {
            _ValueDouble = new double[(int)EPanelStatusDouble.Count];
            for (int i = 0; i < (int)EPanelStatusDouble.Count; i++)
            {
                _ValueDouble[i] = Old._ValueDouble[i];
            }

            _ValueInt = new int[(int)EPanelStatusInt.Count];
            for (int i = 0; i < (int)EPanelStatusInt.Count; i++)
            {
                _ValueInt[i] = Old._ValueInt[i];
            }

            _ValueString = new String[(int)EPanelStatusString.Count];
            for (int i = 0; i < (int)EPanelStatusString.Count; i++)
            {
                _ValueString[i] = Old._ValueString[i];
            }
        }

        public void InitializeComponet()
        {
            _ValueDouble = new double[(int)EPanelStatusDouble.Count];
            _ValueInt = new int[(int)EPanelStatusInt.Count];
            _ValueString = new String[(int)EPanelStatusString.Count];
            for (int i = 0; i < (int)EPanelStatusString.Count; i++)
            {
                _ValueString[i] = "";
            }
        }

        public string GetValue(EPanelStatusString Index)
        {
            return _ValueString[(int)Index];
        }

        public int GetValue(EPanelStatusInt Index)
        {
            return _ValueInt[(int)Index];
        }

        public double GetValue(EPanelStatusDouble Index)
        {
            return _ValueDouble[(int)Index];
        }


        public void SetValue(EPanelStatusString Index, string Value)
        {
            _ValueString[(int)Index] = Value;
        }

        public void SetValue(EPanelStatusInt Index, int Value)
        {
            _ValueInt[(int)Index] = Value;
        }

        public void SetValue(EPanelStatusDouble Index, double Value)
        {
            _ValueDouble[(int)Index]= Value;
        }


        public void Dispose()
        {

        }
    }

    public class StationStatusDef : IDisposable
    {

        public bool[] Empty { private set; get; }
        private List<PanelStatusDef>[] _Station;

        public StationStatusDef()
        {
            Empty = new bool[(int)EStation.Count];
            _Station = new List<PanelStatusDef>[(int)EStation.Count];
            for (int i = 0; i < (int)EStation.Count; i++)
                _Station[i] = new List<PanelStatusDef>();
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// 增加一片panel到 current station
        /// </summary>
        public void Add(EStation CurrentStation, PanelStatusDef Panel)
        {
            
            if ((int)CurrentStation >= (int)EStation.Count)
            {
                return;
            }
            Empty[(int)CurrentStation] = false;

            _Station[(int)CurrentStation].Add(new PanelStatusDef(Panel));
        }

        /// <summary>
        /// 移動整個 current station到 Next station
        /// </summary>
        public void ToNext(EStation CurrentStation, EStation Next)
        {
            if (Empty[(int)CurrentStation])
                return;

           
            _Station[(int)Next].Clear();
            for (int i = 0; i < _Station[(int)CurrentStation].Count; i++)
                _Station[(int)Next].Add(new PanelStatusDef(_Station[(int)CurrentStation][i]));
            Empty[(int)Next] = false;
            Remove(CurrentStation);
        }

        /// <summary>
        /// 移除整個 current station
        /// </summary>
        public void Remove(EStation CurrentStation)
        {
            Empty[(int)CurrentStation] = true;
            _Station[(int)CurrentStation].Clear();
        }

        

        /// <summary>
        /// 搬移current station最上層 panel 到 Next station上層
        /// </summary>
        public void Stack(EStation CurrentStation, EStation Next)
        {
            if (Empty[(int)CurrentStation])
                return;


            if (_Station[(int)CurrentStation].Count > 0)
            {
                int last = _Station[(int)CurrentStation].Count - 1;
                _Station[(int)Next].Add(new PanelStatusDef(_Station[(int)CurrentStation][last]));
                Empty[(int)Next] = false;
            }

            if (_Station[(int)CurrentStation].Count == 0)
                Empty[(int)CurrentStation] = true;
        }

        /// <summary>
        /// 取得CurrentStation內容(new 一份新的List給外面)
        /// </summary>
        public List<PanelStatusDef> GetStation(EStation CurrentStation)
        {
            List<PanelStatusDef> station = new List<PanelStatusDef>();

            if (Empty[(int)CurrentStation])
                return station;

            for (int i = 0; i < _Station[(int)CurrentStation].Count; i++)
                station.Add(new PanelStatusDef(_Station[(int)CurrentStation][i]));

            return station;
        }
    }

    
}
