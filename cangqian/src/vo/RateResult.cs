using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.vo
{
    class RateResult
    {
        float rate;
        float score;
        int uv;
        public RateResult(float rate,float score,int uv)
        {
            this.rate = rate;
            this.score = score;
            this.uv = uv;
        }

        public float getRate()
        {
            return rate;
        }

    }
}
