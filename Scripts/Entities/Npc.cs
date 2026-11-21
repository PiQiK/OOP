namespace app.Entities;

using Godot;
using app.Interfaces;
using app.Systems;

public partial class Npc : Character, IInteractable
{
    [Export] public string NpcId { get; set; } = string.Empty;

    public string PromptText => $"Talk to {(string.IsNullOrWhiteSpace(DisplayName) ? "NPC" : DisplayName)}";

    public override void _Ready()
    {
        var data = DialogueManager.Instance?.GetNpcData(NpcId);
        if ( data != null )
        {
            SetDisplayName(data.Name);
        }
    }

    public void Interact(Player player)
    {
        DialogueManager.Instance?.StartDialogue(NpcId);
    }
}