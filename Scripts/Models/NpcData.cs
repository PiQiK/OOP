namespace app.Models;

using System.Collections.Generic;

public class NpcData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Dialogues { get; set; } = new();
}