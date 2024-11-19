namespace KamerConnect.Models;

public class Personality
{
    public string? School { get; set; }
    public string? Study { get; set; }
    public string? Description { get; set; }
    
    public Personality(string? school, string? study, string? description)
    {
        School = school;
        Study = study;
        Description = description;
    }
    
    public override string ToString()
    {
        return $@"
        School: {School}
        Study: {Study}
        Description: {Description}";
    }
}