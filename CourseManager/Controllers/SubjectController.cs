using CourseManager.Services;
using CourseManager.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;


        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [HttpGet("api/subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjectsAsync();
            return Ok(subjects);
        }

        [HttpGet("api/subjects/{subjectId}")]
        public async Task<IActionResult> GetSubjectById(int subjectId)
        {
            try
            {
                var subject = await _subjectService.GetSubjectByIdAsync(subjectId);
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects")]
        public async Task<IActionResult> CreateSubject(CreateSubjectDto dto)
        {
            try
            {
                var subject = await _subjectService.CreateSubjectAsync(dto);
                return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/subjects/{subjectId}")]
        public async Task<IActionResult> UpdateSubject(int subjectId, UpdateSubjectDto dto)
        {
            try
            {
                var subject = await _subjectService.UpdateSubjectAsync(subjectId, dto);
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects/{subjectId}/deactivate")]
        public async Task<IActionResult> DeactivateSubject(int subjectId)
        {
            try
            {
                await _subjectService.DeactivateSubjectAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects/{subjectId}/reactivate")]
        public async Task<IActionResult> ReactivateSubject(int subjectId)
        {
            try
            {
                await _subjectService.ReactivateSubjectAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost("api/subjects/{subjectId}/unregister")]
        public async Task<IActionResult> UnRegisterFromSubject(int subjectId, CourseUnregisterDto dto)
        {
            try
            {
                await _subjectService.UnRegisterFromSubjectAsync(subjectId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects/{subjectId}/register")]
        public async Task<IActionResult> RegisterForCourseAsync(int subjectId, CourseRegisterDto dto)
        {
            try
            {
                await _subjectService.RegisterForCourseAsync(subjectId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/api/subjects/{subjectId}/students")]
        public async Task<IActionResult> GetStudentsForSubject(int subjectId, string semester)
        {
            try
            {
                var students = await _subjectService.GetStudentsForSubjectAsync(subjectId, semester);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }


        }

    }
}
