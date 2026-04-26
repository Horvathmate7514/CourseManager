using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services.Dtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credit { get; set; }
        public bool isActive { get; set; }
    }


    public class CreateSubjectDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int Credit { get; set; }
    }

        public class UpdateSubjectDto
        {
            public string? Name { get; set; }
            public string? Code { get; set; }
            public int? Credit { get; set; }
            public bool? isActive { get; set; }
    }
}
