using cangqian.src.comm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.vo
{
    class CheckCellCondition    //检查单元格的条件
    {
        int colIndex = 0;
        bool isAllowNull = false;   //默认值不允许为空
        string dataType = "string";
        string errorValue;  //错误值

        public CheckCellCondition(int index,string dataType,string errorValue)
        {
            this.colIndex = index;
            this.dataType = dataType;
            this.errorValue = errorValue;
        }

        public CheckCellCondition(int index, string dataType, string errorValue,bool isAllowNull)
        {
            this.colIndex = index;
            this.dataType = dataType;
            this.errorValue = errorValue;
            this.isAllowNull = isAllowNull;
        }

        public BaseResult check(DataRow dataRow)
        {
            //1、 非空校验
            object obj = dataRow[colIndex];
            if (this.isAllowNull && StringUtils.isBlank(obj)) return BaseResult.newSucBaseResult(null);
            if (!this.isAllowNull && StringUtils.isBlank(obj)) return BaseResult.newFailBaseResult(100, "current cell's values is null");
            string text = ((string)obj).Trim();

            //2、数据类型校验
            if (null != dataType && !"".Equals(text))
            {
                try 
                { 
                    if ("int".Equals(dataType))
                    {
                        int.Parse(text);
                    } 
                    else if("float".Equals(dataType)) {
                        float.Parse(text);
                    }
                }
                catch (Exception ex)
                {
                    return BaseResult.newFailBaseResult(101, "current cell value's datatype is no equal "+this.dataType);
                }
            }
            //3、值校验
            if (!StringUtils.isBlank(this.errorValue))
            {
                if (this.errorValue.Equals(text))
                {
                    return BaseResult.newFailBaseResult(102, "current cell's value is not all equals with " + this.errorValue);
                }
            }

            //4、验证通过
            return BaseResult.newSucBaseResult(null);
        }
    }
}
