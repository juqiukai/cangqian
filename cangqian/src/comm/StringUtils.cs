using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.comm
{
    class StringUtils
    {
        public static bool isBlank(object s)
        {
            if (null == s || "".Equals(((string)s).Trim())) return true;
            return false;
        }

        public static bool isBlank(string s)
        {
            if (null == s || "".Equals(s.Trim())) return true;
            return false;
        }
    }
}
