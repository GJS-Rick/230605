using System;
using System.Collections.Generic;
using CommonLibrary;
using FileStreamLibrary;

namespace nsFmMotion
{
    class cIOAnalysisDef
    {
        private MtnCtrlDef _MtnCtrl;
        private List<String> _ModuleNameLst;
        private List<List<int>> _DIIdxLst;
        private List<List<int>> _DOIdxLst;

        public cIOAnalysisDef(MtnCtrlDef cMtnCtrl)
        {
             _MtnCtrl = cMtnCtrl;
             _ModuleNameLst = new List<string>();
             _DIIdxLst = new List<List<int>>();
             _DOIdxLst = new List<List<int>>();

            vAnalysis();
        }

        ~cIOAnalysisDef()
        {
             _ModuleNameLst.Clear();
             _ModuleNameLst = null;

            for (int i = 0; i <  _DIIdxLst.Count; i++)
                 _DIIdxLst[i].Clear();
             _DIIdxLst.Clear();
             _DIIdxLst = null;

            for (int i = 0; i <  _DOIdxLst.Count; i++)
                 _DOIdxLst[i].Clear();
             _DOIdxLst.Clear();
             _DOIdxLst = null;
        }

        private void vAnalysis()
        {
            // analysis 
            for (int i = 0; i < (int) MachineModules.Count ; i++)
            {
                List<int> nIdxDILst = new List<int>();
                List<int> nIdxDOLst = new List<int>();
                 _ModuleNameLst.Add(((MachineModules)i).ToString());
                 _DIIdxLst.Add(nIdxDILst);
                 _DOIdxLst.Add(nIdxDOLst);
            }

            // analysis DI Index
            for (int i = 0; i <  _ModuleNameLst.Count; i++)
            {
                for (int j = 0; j < (int) EDI_TYPE.DI_COUNT; j++)
                {
                    String sModuleName =  _ModuleNameLst[i];
                    if (G.Comm.IOCtrl.GetDIModule((EDI_TYPE) j).ToString() == sModuleName)
                    {
                         _DIIdxLst[i].Add(j);
                    }
                }
            }

            // analysis DO Index
            for (int i = 0; i <  _ModuleNameLst.Count; i++)
            {
                for (int j = 0; j < (int)EDO_TYPE.DO_COUNT; j++)
                {
                    String sModuleName =  _ModuleNameLst[i];
                    if (G.Comm.IOCtrl.GetDOModule((EDO_TYPE)j).ToString() == sModuleName)
                    {
                         _DOIdxLst[i].Add(j);
                    }
                }
            }
        }


        public int nGetModuleNameNum()
        {
            return  _ModuleNameLst.Count;
        }

        public String sGetModuleName(MachineModules eModules)
        {
            return  _ModuleNameLst[(int) eModules];
        }

        public int nGetDINum(MachineModules eModules)
        {
            return  _DIIdxLst[(int)eModules].Count;
        }

        public int nGetDONum(MachineModules eModules)
        {
            return  _DOIdxLst[(int)eModules].Count;
        }

        public int nGetDIIdx(MachineModules eModules, int nIdx)
        {
            if (nIdx < 0 || nIdx >=  _DIIdxLst[(int)eModules].Count)
                return -1;

            return  _DIIdxLst[(int)eModules][nIdx];
        }

        public int nGetDOIdx(MachineModules eModules, int nIdx)
        {
            if (nIdx < 0 || nIdx >=  _DOIdxLst[(int)eModules].Count)
                return -1;

            return  _DOIdxLst[(int)eModules][nIdx];
        }
    }
}
