namespace projects_api.Models.Project;
public class ProjectInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public decimal SizeInBytes { get; set; } = 0;
    public bool IsCompleted { get; set; }
}