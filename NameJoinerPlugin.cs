using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Orchestration;

public class NameJoinerPlugin 
{
    [SKFunction("Joins a first and last name together")]
    [SKFunctionContextParameter(Name = "firstname", Description = "Informal name you use")]
    [SKFunctionContextParameter(Name = "lastname", Description = "More formal name you use")]
    public async Task<string> FullNamer(SKContext context)
    {
        return context["firstname"] + " " + context["lastname"] + " - This is my full name from C#";
    }
}