using AutoMapper;
using CourseManager.DataContext.Dtos;
using CourseManager.DataContext.Entities;
using CourseManager.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<RegisterUserDto, User>();

            CreateMap<UpdateUserDto, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); ;


            CreateMap<Subject, SubjectDto>().ReverseMap();


            CreateMap<CreateSubjectDto, Subject>();

            CreateMap<UpdateSubjectDto, Subject>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Course, CourseDto>()
         .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src => src.Teachers.Select(t => t.Id).ToList()))
         .ForMember(dest => dest.SubjectCode, opt => opt.MapFrom(src => src.Subject.Code))
         .ReverseMap();

            CreateMap<CreateCourseDto, Course>();

            CreateMap<UpdateCourseDto, Course>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<ClassTimeDto, Schedule>().ReverseMap();

            CreateMap<Notification, NotificationLogDto>().ReverseMap();

        }
    }
}
