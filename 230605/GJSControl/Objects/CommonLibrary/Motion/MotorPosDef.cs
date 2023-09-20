using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class MotorPosDef : IDisposable
    {
        private EAXIS_NAME[] m_EAxisArray;
        public double[] _Value;
        public ESPEED_TYPE _ESpeedType;

        public MotorPosDef(EAXIS_NAME eAxisType)
        {
            m_EAxisArray = new EAXIS_NAME[1];
            _Value = new double[1];

            m_EAxisArray[0] = eAxisType;
            _Value[0] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(EAXIS_NAME eAxisType0, EAXIS_NAME eAxisType1)
        {
            m_EAxisArray = new EAXIS_NAME[2];
            _Value = new double[2];

            m_EAxisArray[0] = eAxisType0;
            m_EAxisArray[1] = eAxisType1;
            _Value[0] = 0;
            _Value[1] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(
            EAXIS_NAME eAxisType0,
            EAXIS_NAME eAxisType1,
            EAXIS_NAME eAxisType2)
        {
            m_EAxisArray = new EAXIS_NAME[3];
            _Value = new double[3];

            m_EAxisArray[0] = eAxisType0;
            m_EAxisArray[1] = eAxisType1;
            m_EAxisArray[2] = eAxisType2;
            _Value[0] = 0;
            _Value[1] = 0;
            _Value[2] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(
            EAXIS_NAME eAxisType0,
            EAXIS_NAME eAxisType1,
            EAXIS_NAME eAxisType2,
            EAXIS_NAME eAxisType3)
        {
            m_EAxisArray = new EAXIS_NAME[4];
            _Value = new double[4];

            m_EAxisArray[0] = eAxisType0;
            m_EAxisArray[1] = eAxisType1;
            m_EAxisArray[2] = eAxisType2;
            m_EAxisArray[3] = eAxisType3;
            _Value[0] = 0;
            _Value[1] = 0;
            _Value[2] = 0;
            _Value[3] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(
            EAXIS_NAME eAxisType0,
            EAXIS_NAME eAxisType1,
            EAXIS_NAME eAxisType2,
            EAXIS_NAME eAxisType3,
            EAXIS_NAME eAxisType4)
        {
            m_EAxisArray = new EAXIS_NAME[5];
            _Value = new double[5];

            m_EAxisArray[0] = eAxisType0;
            m_EAxisArray[1] = eAxisType1;
            m_EAxisArray[2] = eAxisType2;
            m_EAxisArray[3] = eAxisType3;
            m_EAxisArray[4] = eAxisType4;
            _Value[0] = 0;
            _Value[1] = 0;
            _Value[2] = 0;
            _Value[3] = 0;
            _Value[4] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(
            EAXIS_NAME eAxisType0,
            EAXIS_NAME eAxisType1,
            EAXIS_NAME eAxisType2,
            EAXIS_NAME eAxisType3,
            EAXIS_NAME eAxisType4,
            EAXIS_NAME eAxisType5)
        {
            m_EAxisArray = new EAXIS_NAME[6];
            _Value = new double[6];

            m_EAxisArray[0] = eAxisType0;
            m_EAxisArray[1] = eAxisType1;
            m_EAxisArray[2] = eAxisType2;
            m_EAxisArray[3] = eAxisType3;
            m_EAxisArray[4] = eAxisType4;
            m_EAxisArray[5] = eAxisType5;
            _Value[0] = 0;
            _Value[1] = 0;
            _Value[2] = 0;
            _Value[3] = 0;
            _Value[4] = 0;
            _Value[5] = 0;
            _ESpeedType = ESPEED_TYPE.Low;
        }

        public MotorPosDef(MotorPosDef cMotorPos)
        {
            m_EAxisArray = new EAXIS_NAME[cMotorPos.m_EAxisArray.Count()];
            _Value = new double[cMotorPos.m_EAxisArray.Count()];

            for (int i = 0; i < cMotorPos.m_EAxisArray.Count(); i++)
            {
                m_EAxisArray[i] = cMotorPos.m_EAxisArray[i];
                _Value[i] = cMotorPos._Value[i];
            }

            _ESpeedType = cMotorPos._ESpeedType;
        }

        public int GetAxisNum()
        {
            return m_EAxisArray.Count();
        }

        public EAXIS_NAME GetAxis(int nIndex)
        {
            if (nIndex < 0 || nIndex >= m_EAxisArray.Count())
                return EAXIS_NAME.Count;

            return m_EAxisArray[nIndex];
        }

        public void Dispose()
        {
            m_EAxisArray = null;
            _Value = null;
        }
    }
}
