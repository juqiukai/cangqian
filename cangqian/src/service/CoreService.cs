using cangqian.src.comm;
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
    class CoreService
    {
        public static StatResult calc(DataTable dt) 
        {
            //1、设置查询条件
            CheckCellCondition condition0 = new CheckCellCondition(0, null, "*");
            CheckCellCondition condition1 = new CheckCellCondition(1, null, "*");
            CheckCellCondition condition2 = new CheckCellCondition(2, null, "*");
            CheckCellCondition condition3 = new CheckCellCondition(3, null, "*", true);
            List<CheckCellCondition> queryList = new List<CheckCellCondition>();
            queryList.Add(condition0);
            queryList.Add(condition1);
            queryList.Add(condition2);
            queryList.Add(condition3);

            //2、获取到正确的行记录
            DataRowCollection rows = dt.Rows;
            List<DataRow> nrows =  DataRowManager.findAllRightRows(rows,queryList);
            if (null == nrows || nrows.Count == 0)
            {
                return null;
            }
            DataTable rightDataTable = DataTableManager.newDataTable(dt.Columns, nrows);

            //3、排序 根据score由高到底排序
            List<Student> studentList = StudentManager.rows2StudentList(nrows);

            //4、统计平均值
            StatResult result = new StatResult();
            float totalScore = 0;
            int uv = 0;
            foreach(Student stu in studentList) {
                totalScore += stu.getScore();
                uv += 1;
            }

           return null;
        }

        /***
        * 计算高分人数，低分人数，平均分
        * 
        * */
        public static StatRecord coreCalc(List<Student> studentList, float highScore, float lowScore, string defaultName)
        {
            float meanScore = CoreService.calcMeanScore(studentList);
            List<Student> highStudentList = CoreService.calcScoreRange(studentList, highScore, 101);
            List<Student> lowStudentList = CoreService.calcScoreRange(studentList, 0, lowScore);
            string name = studentList[0].getClazz();
            if (!StringUtils.isBlank(defaultName))
            {
                name = defaultName;
            }
            StatRecord record = new StatRecord(name, studentList.Count, meanScore, highStudentList, lowStudentList,highScore,lowScore);
            return record;
        }


        /***
         * 
         * 计算平均分
         * */
        public static float calcMeanScore(List<Student> list)
        {
            float totalScore = 0f;
            int totalUv = 0;
            foreach (Student stu in list)
            {
                totalUv++;
                totalScore += stu.getScore();
            }
            return totalScore / totalUv;
        }

        /**
         * 根据分数区间，计算学生列表
         * 
         * */
        public static List<Student> calcScoreRange(List<Student> studentList,float beginScore,float endScore)
        {
            List<Student> list = new List<Student>();
            foreach (Student stu in studentList)
            {
                float score = stu.getScore();
                if (score >= beginScore && score < endScore)
                {
                    list.Add(stu);
                }
            }
            return list;
        }

        /***
         * 
         * 计算划分段
         * */
        public static float calcSegScore(List<Student> studentList, float rate)
        {
            int totalUv = studentList.Count;
            float realUv = totalUv * rate;
            int uv = (int)(totalUv * rate);
            if (uv < realUv)
            {
                uv = uv + 1;
            }

            float seqScore = 10000; //分割的分数
            int upUv = 0;
            int downUv = 0;
            int i = 0;
            foreach (Student student in studentList)
            {
                i++;
                float curr_score = student.getScore();
                if (i < uv)
                {
                    if (curr_score < seqScore)
                    {
                        seqScore = curr_score;
                        upUv = 1;
                        continue;
                    }
                    if (curr_score == seqScore)
                    {
                        upUv++;
                        continue;
                    }
                }
                if (i == uv)
                {
                    if (curr_score < seqScore)
                    {
                        seqScore = curr_score;
                        upUv = 0;
                        continue;
                    }
                    if (curr_score == seqScore)
                    {
                        continue;
                    }
                }
                if (i < uv)
                {
                    if (curr_score < seqScore)
                    {
                        downUv = 0;
                        break;
                    }
                    if (curr_score == seqScore)
                    {
                        downUv++;
                        continue;
                    }
                }
            }
            if (upUv == 0 && uv / totalUv > rate)
            {
                return seqScore + 0.0001f;
            }
            if (upUv >= downUv)
            {
                return seqScore - 0.0001f;
            }
            else
            {
                return seqScore + 0.0001f;
            }
        }
    }
}
