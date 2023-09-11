using Microsoft.AspNetCore.Mvc;
using projects_api.Models.Project;
using projects_api.Services;

namespace project_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {

        private readonly ILogger<ProjectController> _logger;

        private readonly ProjectService _projectService;

        public ProjectController(ILogger<ProjectController> logger, ProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProjectDisplayDTO>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var projects = await _projectService.GetAll(cancellationToken);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ProjectInfoDTO>> Get(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var project = await _projectService.Get(id, cancellationToken);
                return Ok(project);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "NotFoundException occurred.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Create(ProjectInfoDTO projectInfoDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                await _projectService.Add(projectInfoDTO, cancellationToken);
                return CreatedAtAction(nameof(Get), new { id = projectInfoDTO.Id }, projectInfoDTO);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException occurred.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Update(int id, ProjectInfoDTO projectInfoDTO, CancellationToken cancellationToken = default)
        {
            if (id != projectInfoDTO.Id)
            {
                return BadRequest("The provided Id does not match the project's Id.");
            }

            try
            {
                await _projectService.Update(projectInfoDTO, cancellationToken);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "ArgumentException occurred.");
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "NotFoundException occurred.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _projectService.Delete(id, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "NotFoundException occurred.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<ProjectInfoDTO>>> Search([FromQuery] string searchTerm, CancellationToken cancellationToken = default)
        {
            try
            {
                var projects = await _projectService.SearchByName(searchTerm, cancellationToken);
                return Ok(projects);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "NotFoundException occurred.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }

}