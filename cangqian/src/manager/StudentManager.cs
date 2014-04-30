using cangqian.src.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cangqian.src.manager
{
    class StudentManager
    {
        public static List<Student> rows2StudentList(List<DataRow> rows)
        {
            List<Student> list = new List<Student>();
            foreach (DataRow dataRow in rows)
            {
                Student student = new Student(dataRow);
                list.Add(student);
            }
            list.Sort();
            return list;
        }

        public static List<DataRow> studentList2Rows(List<Student> list)
        {
            List<DataRow> collection = new List<DataRow>();
            if (null == list || list.Count == 0) return collection;
            
            foreach (Student student in list)
            {
                DataRow dataRow = student.getDataRow();
                collection.Add(dataRow);
            }
            return collection;
        }
    }
}
