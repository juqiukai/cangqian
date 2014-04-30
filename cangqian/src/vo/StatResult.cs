using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.vo
{
    class StatResult
    {
        //总用户数
        public int m_uv;
        //总分
        public float m_totalScore;

        public float getMeanCount()
        {
            if (0 == m_uv)
            {
                return 0;
            }
            return m_totalScore / m_uv;
        }
    }
}
