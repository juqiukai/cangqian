using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.comm
{
    class BaseResult
    {
        int code = 200;
        object data;
        string msg = "suc";

        public BaseResult(int code,string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public BaseResult(object data)
        {
            this.data = data;
        }

        public static BaseResult newSucBaseResult(object data)
        {
            return new BaseResult(data);
        }

        public static BaseResult newFailBaseResult(int code,string msg)
        {
            return new BaseResult(code,msg);
        }

        public bool isSuccess()
        {
            return this.code == 200;
        }

    }
}
