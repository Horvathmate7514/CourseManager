using CourseManager.Services;
using CourseManager.Services.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto)
        {
            try
            {
                var course = await _courseService.CreateCourseAsync(dto);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCoursesById(int courseId)
        {
            var courses = await _courseService.GetCourseByIdAsync(courseId);
            return Ok(courses);
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, UpdateCourseDto dto)
        {
            try
            {
                var course = await _courseService.UpdateCourseAsync(courseId, dto);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            try
            {
                await _courseService.DeleteCourseAsync(courseId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        public async Task<IActionResult> ChangeCourse(CourseChangeDto dto)
        {
            try
            {
                await _courseService.ChangeCourseAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}