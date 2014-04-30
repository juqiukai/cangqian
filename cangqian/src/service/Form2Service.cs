using cangqian.src.manager;
using cangqian.src.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.service
{
    class Form2Service
    {
        public static DataTable[] compare(DataRowCollection rows1, DataRowCollection rows2)
        {
            //1、参数检查
            if (null == rows1 || rows1.Count == 0 || null == rows2 || rows2.Count == 0)
            {
                return null;
            }
            //2、将rows转换为map对象，key为学号
            Dictionary<long, Student> dict1 = rows2Dict(rows1);
            Dictionary<long, Student> dict2 = rows2Dict(rows2);

            //3、分离出rows1,rows2中合格/不合格的人员列表
            List<Student>[] larr = filt(dict1,dict2);
            List<Student> qualified1List = larr[0];
            List<Student> unqualified1List = larr[1];
            larr = filt(dict2, dict1);
            List<Student> qualified2List = larr[0];
            List<Student> unqualified2List = larr[1];

            //4、排序
            qualified1List.Sort();
            qualified2List.Sort();

            //5、分别计算合格人员列表的平均分，高分人数，低分人数
            float high1Score = CoreService.calcSegScore(qualified1List, 0.2f);
            float low1Score = CoreService.calcSegScore(qualified1List, 0.8f);
            StatRecord statRecord1 = CoreService.coreCalc(qualified1List, high1Score, low1Score, null);

            float high2Score = CoreService.calcSegScore(qualified2List, 0.2f);
            float low2Score = CoreService.calcSegScore(qualified2List, 0.8f);
            StatRecord statRecord2 = CoreService.coreCalc(qualified2List, high2Score, low2Score, null);
            
            //6、获取不同DataTable
            DataTable unqulifiedTable1 = renderUnqualifiedTable(unqualified1List);
            DataTable unqulifiedTable2 = renderUnqualifiedTable(unqualified2List);
            DataTable statTable = renderStatTable(statRecord1,statRecord2);

            return new DataTable[] { unqulifiedTable1, unqulifiedTable2, statTable };
        }

        private static DataTable renderUnqualifiedTable(List<Student> unqualifiedList)
        {
            DataTable dt = DataTableManager.newDataTable(Form1Service.columnNames);
            foreach (Student stu in unqualifiedList)
            {
                DataRow dr = dt.NewRow();
                dr[0] = stu.clazz;
                dr[1] = stu.number;
                dr[2] = stu.name;
                dr[3] = stu.score;
                dt.Rows.Add(dr);
            }
            return dt;
        }


        private static DataTable renderStatTable(StatRecord statRecord1, StatRecord statRecord2)
        {
            DataTable dt = DataTableManager.newDataTable(new string[] { "名称", "总人数", "平均分", "高分线","高分人数","高分率", "低分线","低分人数","低分率" });
            float highRate = statRecord1.getHighScoreCount()*1000*0.1f/statRecord1.getTotalCount();
            float lowRate = statRecord1.getLowScoreCount()*1000*0.1f/statRecord1.getTotalCount();
            DataRow dr = dt.NewRow();
            dr[0] = "上次统计结果";
            dr[1] = statRecord1.getTotalCount();
            dr[2] = Convert.ToDecimal(statRecord1.getMeanScore()).ToString("0.00");
            dr[3] = Convert.ToDecimal(statRecord1.highScoreLine + 0.0001f).ToString("0.00") ;
            dr[4] = statRecord1.getHighScoreCount();
            dr[5] = Convert.ToDecimal(highRate).ToString("0.00")+"%";
            dr[6] = Convert.ToDecimal(statRecord1.lowScoreLine + 0.0001f).ToString("0.00");
            dr[7] = statRecord1.getLowScoreCount();
            dr[8] = Convert.ToDecimal(lowRate).ToString("0.00") + "%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            float highRate2 = statRecord2.getHighScoreCount()*100/statRecord2.getTotalCount();
            float lowRate2 = statRecord2.getLowScoreCount()*100/statRecord2.getTotalCount();
            dr[0] = "本次统计结果";
            dr[1] = statRecord2.getTotalCount();
            dr[2] = Convert.ToDecimal(statRecord2.getMeanScore()).ToString("0.00");
            dr[3] = Convert.ToDecimal(statRecord2.highScoreLine+0.0001f).ToString("0.00");
            dr[4] = statRecord2.getHighScoreCount();
            dr[5] = Convert.ToDecimal(highRate2).ToString("0.00") + "%";
            dr[6] = Convert.ToDecimal(statRecord2.lowScoreLine+0.0001f).ToString("0.00");
            dr[7] = statRecord2.getLowScoreCount();
            dr[8] = Convert.ToDecimal(lowRate2).ToString("0.00") + "%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "比较结果";
            dr[1] = statRecord2.getTotalCount();
            dr[2] = Convert.ToDecimal(statRecord2.getMeanScore()-statRecord1.getMeanScore()).ToString("0.00"); //平均分提升率
            dr[3] = statRecord2.highScoreLine - statRecord1.highScoreLine;
            dr[4] = statRecord2.getHighScoreCount() - statRecord1.getHighScoreCount();
            dr[5] = Convert.ToDecimal((highRate2 - highRate)).ToString("0.00") + "%"; //高分提高率
            dr[6] = statRecord2.lowScoreLine - statRecord1.lowScoreLine;
            dr[7] = statRecord2.getLowScoreCount() - statRecord1.getLowScoreCount();
            dr[8] = Convert.ToDecimal(lowRate2 - lowRate).ToString("0.00") + "%";
            dt.Rows.Add(dr);

            return dt;
        }

        /**
         * 记录集合转换为Map
         * */
        private static Dictionary<long, Student> rows2Dict(DataRowCollection rows)
        {
            List<Student> list = new List<Student>();
            foreach (DataRow row in rows)
            {
                Student stu = new Student(row);
                list.Add(stu);
            }
            Dictionary<long, Student> dict = new Dictionary<long, Student>();
            foreach (Student stu in list)
            {
                dict.Add(stu.number, stu);
            }
            return dict;
        }

        /***
         * 筛选出交集和补给，以前者（dict1）为准
         * 
         * */
        private static List<Student>[] filt(Dictionary<long, Student> dict1, Dictionary<long, Student> dict2)
        {
            List<Student> okList = new List<Student>();
            List<Student> failList = new List<Student>();
            foreach (var item in dict1)
            {
                long key = item.Key;
                Student stu = item.Value;
                if (!stu.isQualified)
                {
                    failList.Add(stu);
                    continue;
                }
                Student stu2;
                try
                {
                    stu2 = dict2[key];
                }
                catch (Exception ex)
                {
                    failList.Add(stu);
                    continue;
                }
                if (null == stu2 || !stu2.isQualified)
                {
                    failList.Add(stu);
                    continue;
                }
                okList.Add(stu);
            }
            return new List<Student>[] { okList, failList };
        }
    }
}
