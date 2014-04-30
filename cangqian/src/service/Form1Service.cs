using cangqian.src.comm;
using cangqian.src.manager;
using cangqian.src.tools;
using cangqian.src.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.service
{
    class Form1Service
    {
        public static string[] columnNames = new string[] { "班级", "学号", "姓名", "score", "*" };
        /**
         * 加载excel数据
         * */
        public static DataTable loadExcelData(string filePath)
        {

            return ExcelTools.dumpXml2DataTable(filePath, columnNames);
        }

        /***
         * 统计计算
         * */
        public static DataTable[] calc(DataRowCollection rows)
        {
            //1、设置查询条件
            List<CheckCellCondition> condtionList = getConditionList();
            //2、获取验证通过的记录列表
            List<DataRow> rightRows = DataRowManager.findAllRightRows(rows, condtionList);
            //3、封装对象
            List<Student> studentList = StudentManager.rows2StudentList(rightRows);
            //4、根据分数由高到低排序
            studentList.Sort();
            //5、计算所有人的一个平均分，高分段和低分段,高分段人数，低分段人数
            float highScore = CoreService.calcSegScore(studentList,0.2f);
            float lowScore = CoreService.calcSegScore(studentList,0.8f);
            StatRecord mainRecord = CoreService.coreCalc(studentList, highScore, lowScore, "全年级");
            //6、获取不同班级人员列表
            List<List<Student>> groupList = groupByClazz(studentList);
            List<StatRecord> statRecordList = coreCalcGroup(groupList,highScore,lowScore);
            //7、获取高、低分明细table
            DataTable[] tablearr = renderScoreTable(statRecordList);
            DataTable highDataTable = tablearr[0];
            DataTable lowDataTable = tablearr[1];
            //8、获取渲染最终统计结果的table
            statRecordList.Add(mainRecord);
            DataTable mainStatTable = renderMainResultTable(statRecordList);
            return new DataTable[] { mainStatTable, highDataTable, lowDataTable };
        }

        private static DataTable[] renderScoreTable(List<StatRecord> statRecordList)
        {
            DataTable highDt = DataTableManager.newDataTable(new string[] { "班级", "学号", "姓名", "分数" });
            DataTable lowDt = DataTableManager.newDataTable(new string[] { "班级", "学号", "姓名", "分数" });
            foreach (StatRecord record in statRecordList)
            {
                List<Student> highList = record.highStudentList;
                foreach (Student stu in highList)
                {
                    DataRow dr = highDt.NewRow();
                    dr[0] = stu.clazz;
                    dr[1] = stu.number;
                    dr[2] = stu.name;
                    dr[3] = stu.score;
                    highDt.Rows.Add(dr);
                }

                List<Student> lowList = record.lowStudentList;
                foreach (Student stu in lowList)
                {
                    DataRow dr = lowDt.NewRow();
                    dr[0] = stu.clazz;
                    dr[1] = stu.number;
                    dr[2] = stu.name;
                    dr[3] = stu.score;
                    lowDt.Rows.Add(dr);
                }
            }
            return new DataTable[] { highDt,lowDt};
        }

        private static DataTable renderMainResultTable(List<StatRecord> statRecordList)
        {
            DataTable dt = DataTableManager.newDataTable(new string[] { "班级", "总人数", "平均分", "高分线","高分人数","高分率", "低分线","低分人数","低分率" });
            foreach (StatRecord record in statRecordList)
            {
                DataRow dr = dt.NewRow();
                dr[0] = record.getClazz();
                dr[1] = record.getTotalCount();
                dr[2] = Convert.ToDecimal(record.getMeanScore()).ToString("0.00");
                dr[3] = record.highScoreLine+0.0001f;
                dr[4] = record.getHighScoreCount();
                float highRate = record.getHighScoreCount() * 1000* 0.1f/ record.getTotalCount();
                dr[5] = Convert.ToDouble(highRate).ToString("0.00")+"%";
                dr[6] = record.lowScoreLine+0.0001f;
                dr[7] = record.getLowScoreCount();
                float lowRate = record.getLowScoreCount() * 1000 * 0.1f/record.getTotalCount();
                dr[8] = Convert.ToDouble(lowRate).ToString("0.00")+"%";
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /**
         * 计算不同班级的高分人数，低分人数以及平均分
         * 
         * */
        private static List<StatRecord> coreCalcGroup(List<List<Student>> groupList, float highScore, float lowScore)
        {
            List<StatRecord> retlist = new List<StatRecord>();
            foreach(List<Student> list in groupList) {
                retlist.Add(CoreService.coreCalc(list,highScore,lowScore,null));
            }
            return retlist;
        }

        /**
         * 根据班级对学生进行分组
         * 
         * */
        public static List<List<Student>> groupByClazz(List<Student> studentList) 
        {
            Dictionary<string, List<Student>> dict = new Dictionary<string,List<Student>>();
            foreach(Student stu in studentList) {
                string clazz = stu.getClazz();
                List<Student> list = null;
                try
                {
                    list = dict[clazz];
                }
                catch (Exception ex) { }
                if (null == list)
                {
                    list = new List<Student>();
                    dict.Add(clazz, list);
                }
                list.Add(stu);
            }
            List<List<Student>> retList = new List<List<Student>>();
            foreach (string key in dict.Keys)
            {
                retList.Add(dict[key]);
            }
            return retList;
        }

        /**
         * 设置过滤条件
         * */
        private static List<CheckCellCondition> getConditionList()
        {
            List<CheckCellCondition> conditionList = new List<CheckCellCondition>();
            CheckCellCondition condition0 = new CheckCellCondition(0, null, "*");
            CheckCellCondition condition1 = new CheckCellCondition(1, null, "*");
            CheckCellCondition condition2 = new CheckCellCondition(2, null, "*");
            CheckCellCondition condition3 = new CheckCellCondition(3, "float", "*", true);
            CheckCellCondition condition4 = new CheckCellCondition(4, null, "*", true);
            conditionList.Add(condition0);
            conditionList.Add(condition1);
            conditionList.Add(condition2);
            conditionList.Add(condition3);
            conditionList.Add(condition4);
            return conditionList;
        }

        public static DataTable getCheckFailedDataTable(DataRowCollection rows)
        {
            List<CheckCellCondition> conditonList = getConditionList();
            List<DataRow> errRows = DataRowManager.findAllErrorRows(rows, conditonList);
            return DataTableManager.newDataTable(columnNames, errRows);
        }
    }
}
