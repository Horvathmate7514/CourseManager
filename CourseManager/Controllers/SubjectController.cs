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

        [HttpGet("api/subjects/{id}")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectByIdAsync(id);
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

        [HttpPut("api/subjects/{id}")]
        public async Task<IActionResult> UpdateSubject(int id, UpdateSubjectDto dto)
        {
            try
            {
                var subject = await _subjectService.UpdateSubjectAsync(id, dto);
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects/{id}/deactivate")]
        public async Task<IActionResult> DeactivateSubject(int id)
        {
            try
            {
                await _subjectService.DeactivateSubjectAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("api/subjects/{id}/reactivate")]
        public async Task<IActionResult> ReactivateSubject(int id)
        {
            try
            {
                await _subjectService.ReactivateSubjectAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
