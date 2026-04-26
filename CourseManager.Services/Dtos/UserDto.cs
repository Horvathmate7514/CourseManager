using CourseManager.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Dtos
{
    public class UserDto
    {
    
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public UserRole Role { get; set; }
            public StudyType? StudyType { get; set; }
            public bool IsActive { get; set; }
        

    }

   public class RegisterUserDto
   {
        [Required]
        public string Name { get; set; } 

            [Required]
        [EmailAddress]
        public string Email { get; set; }
            public string Password { get; set; }
            public UserRole Role { get; set; }
            public StudyType? StudyType { get; set; }
   }

    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
