using AutoMapper;
using CourseManager.DataContext.Context;
using CourseManager.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services
{

    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAllSubjectsAsync();
        Task<SubjectDto> GetSubjectByIdAsync(int id);

        Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto);

        Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto dto);

        Task DeactivateSubjectAsync(int id);
        Task ReactivateSubjectAsync(int id);
    }
    public class SubjectService : ISubjectService
    {
        private readonly NeptunDbContext _context;
        private readonly IMapper _mapper;

        public SubjectService(NeptunDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto)
        {
            var subject = _mapper.Map<DataContext.Entities.Subject>(dto);
            subject.isActive = true;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto dto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            if (!subject.isActive) throw new Exception("Inaktív tantárgy nem szerkeszthető!");
            _mapper.Map(dto, subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task DeactivateSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.isActive = false;
            await _context.SaveChangesAsync();
        }


        public async Task ReactivateSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.isActive = true;
            await _context.SaveChangesAsync();
        }
    }
}
