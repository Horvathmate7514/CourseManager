using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Entities
{
    public class Subject
    {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }

            public int Credit { get; set; }

            public bool isActive { get; set; } = true;



        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
