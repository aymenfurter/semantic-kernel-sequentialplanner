using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;


var kernelConfig = new KernelConfig();
// Read from env OPEN_AI_KEY
var openAIKey = Environment.GetEnvironmentVariable("OPEN_AI_KEY");
kernelConfig.AddOpenAIChatCompletionService("gpt-3.5-turbo", openAIKey);

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Warning)
        .AddConsole()
        .AddDebug();
});

IKernel kernel = new KernelBuilder().WithLogger(loggerFactory.CreateLogger<IKernel>()).WithConfiguration(kernelConfig).Build();

// note: using skills from the repo
var skillsDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "plugins");
kernel.ImportSemanticSkillFromDirectory(skillsDirectory, "SloganPlugin");
kernel.ImportSkill(new NameJoinerPlugin(), "NameJoinerPlugin");

var context = new ContextVariables();
context.Set("input", "kitten");
context.Set("firstname", "Sam");
context.Set("lastname", "Appdev");

// var result = await kernel.RunAsync(context, csharpPlugin["FullNamer"]);

var planner = new SequentialPlanner(kernel);

var goal = "My name is Brad Pit. Join my name, then add a matching slogan after my name.";

var plan = await planner.CreatePlanAsync(goal);

SKContext result = await plan.InvokeAsync();

Console.WriteLine(result);

