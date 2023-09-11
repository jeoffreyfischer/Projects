using Microsoft.EntityFrameworkCore;
using projects_api.Data;
using projects_api.Data.Entities;
using projects_api.Models.Project;

namespace projects_api.Services;
public class ProjectService
{
    private readonly ProjectDbContext _context;

    public ProjectService(ProjectDbContext context)
    {
        _context = context;
    }

    private void ValidateProject(ProjectInfoDTO projectInfoDTO)
    {
        if (projectInfoDTO == null)
        {
            throw new ArgumentNullException(nameof(projectInfoDTO), "Project cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(projectInfoDTO.Name))
        {
            throw new ArgumentException("Project name cannot be empty or whitespace.");
        }
        if (string.IsNullOrWhiteSpace(projectInfoDTO.Author))
        {
            throw new ArgumentException("Project author cannot be empty or whitespace.");
        }
        if (string.IsNullOrWhiteSpace(projectInfoDTO.Description))
        {
            throw new ArgumentException("Project description cannot be empty or whitespace.");
        }
        if (projectInfoDTO.SizeInBytes < 0)
        {
            throw new ArgumentException("Project size cannot be negative.");
        }
    }

    public async Task<List<ProjectDisplayDTO>> GetAll(CancellationToken cancellationToken)
    {
        var ProjectDisplayDTO = await _context.Projects.Select(i => new ProjectDisplayDTO
        {
            Id = i.Id,
            Name = i.Name,
            Author = i.Author,
            SizeInBytes = i.SizeInBytes,
            IsCompleted = i.IsCompleted
        }).ToListAsync(cancellationToken);

        return ProjectDisplayDTO;
    }

    public async Task<ProjectInfoDTO> GetById(int id, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(id, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException($"Project with Id {id} not found.");
        }

        var ProjectInfoDTO = new ProjectInfoDTO
        {
            Id = project.Id,
            Name = project.Name,
            Author = project.Author,
            Description = project.Description,
            SizeInBytes = project.SizeInBytes,
            IsCompleted = project.IsCompleted
        };

        return ProjectInfoDTO;
    }

    public async Task Add(ProjectInfoDTO projectInfoDTO, CancellationToken cancellationToken)
    {
        ValidateProject(projectInfoDTO);

        var project = new Project
        {
            Name = projectInfoDTO.Name.Trim(),
            Author = projectInfoDTO.Author.Trim(),
            Description = projectInfoDTO.Description.Trim(),
            SizeInBytes = projectInfoDTO.SizeInBytes,
            IsCompleted = projectInfoDTO.IsCompleted
        };

        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(ProjectInfoDTO projectInfoDTO, CancellationToken cancellationToken)
    {
        ValidateProject(projectInfoDTO);

        var project = await _context.Projects.FindAsync(projectInfoDTO.Id, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException($"Project with Id {projectInfoDTO.Id} not found.");
        }
        project.Name = projectInfoDTO.Name.Trim();
        project.Author = projectInfoDTO.Author.Trim();
        project.Description = projectInfoDTO.Description.Trim();
        project.SizeInBytes = projectInfoDTO.SizeInBytes;
        project.IsCompleted = projectInfoDTO.IsCompleted;

        await _context.SaveChangesAsync(cancellationToken);

    }

    public async Task Delete(int id, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(id, cancellationToken);
        if (project == null)
        {
            throw new NotFoundException($"Project with Id {id} not found.");
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProjectInfoDTO>> SearchByName(string searchTerm, CancellationToken cancellationToken)
    {
        var cleanedSearchTerm = searchTerm.Trim().ToLower();
        var projects = await _context.Projects
            .Where(i => i.Name.Contains(cleanedSearchTerm) || i.Description.Contains(cleanedSearchTerm))
            .Select(i => new ProjectInfoDTO
            {
                Id = i.Id,
                Name = i.Name,
                Author = i.Author,
                Description = i.Description,
                SizeInBytes = i.SizeInBytes,
                IsCompleted = i.IsCompleted
            })
            .ToListAsync(cancellationToken);

        if (projects.Count == 0)
        {
            throw new NotFoundException($"Project with string '{searchTerm}' not found.");
        }

        return projects;
    }

}