using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.vo
{
    class StatRecord
    {
        //班级名称
        string clazz;
        //总人数
        int totalCount;
        //平均分
        float meanScore;
        //高分线
        public float highScoreLine;
        //低分线
        public float lowScoreLine;
        //高分人数
        int highScoreCount;
        //低分人数
        int lowScoreCount;
        //高分学生列表
        public List<Student> highStudentList;
        //低分学生列表
        public List<Student> lowStudentList;

        public StatRecord(string clazz, int totalCount, float meanScore, List<Student> highStudentList, List<Student> lowStudentList,float highLine,float lowLine)
        {
            this.clazz = clazz;
            this.totalCount = totalCount;
            this.meanScore = meanScore;
            this.highScoreCount = highStudentList.Count;
            this.lowScoreCount = lowStudentList.Count;
            this.highStudentList = highStudentList;
            this.lowStudentList = lowStudentList;
            this.highScoreLine = highLine;
            this.lowScoreLine = lowLine;
        }

        public string getClazz()
        {
            return this.clazz;
        }

        public int getTotalCount()
        {
            return this.totalCount;
        }

        public float getMeanScore()
        {
            return this.meanScore;
        }

        public int getHighScoreCount()
        {
            return this.highScoreCount;
        }

        public int getLowScoreCount()
        {
            return this.lowScoreCount;
        }
    }
}
