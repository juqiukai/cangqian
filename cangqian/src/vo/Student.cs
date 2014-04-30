using cangqian.src.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.vo
{
    class Student : IEquatable<Student>, IComparable<Student>
    {
        //学号
        public long number;

        //姓名
        public string name;

        //分数
        public float score;

        //班级
        public string clazz;

        //是否合格 true：合格 false:不合格
        public bool isQualified = true;

        private DataRow dataRow;

        public Student(DataRow dataRow)
        {
            this.clazz = (string)dataRow[0];
            this.number = long.Parse((string)dataRow[1]);
            this.name = (string)dataRow[2];
            this.score = float.Parse((string)dataRow[3]);
            try
            {
                string sign = (string)dataRow[4];
                if ("*".Equals(sign.Trim()))
                {
                    this.isQualified = false;
                }
            }
            catch (Exception ex) { }
            this.dataRow = dataRow;
        }

        public Student(long num,string nam,float scor)
        {
            this.number = num;
            this.name = nam;
            this.score = scor;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Student objAsStudent = obj as Student;
            if (objAsStudent == null) return false;
            else return Equals(obj);
        }

        public int CompareTo(Student compareStudent)
        {
            if (compareStudent.score > this.score) return 1;
            if (compareStudent.score < this.score) return -1;
            if (compareStudent.number < this.number) return 1;
            if (compareStudent.number > this.score) return -1;
            if (compareStudent.clazz.GetHashCode() > this.clazz.GetHashCode()) return 1;
            if (compareStudent.clazz.GetHashCode() < this.clazz.GetHashCode()) return -1;
            if (compareStudent.name.GetHashCode() > this.name.GetHashCode()) return 1;
            if (compareStudent.name.GetHashCode() < this.name.GetHashCode()) return -1;
            return 0;
        }

        public override int GetHashCode()
        {
            return GetHashCode();
        }

        public bool Equals(Student compareStudent)
        {
            if (compareStudent == null) return false;
            return compareStudent.score == this.score 
                && compareStudent.number == this.number 
                && compareStudent.name.Equals(this.name);
        }

        public DataRow getDataRow()
        {
            return this.dataRow;
        }

        public string getClazz()
        {
            return this.clazz;
        }

        public float getScore()
        {
            return this.score;
        }
    }
}
