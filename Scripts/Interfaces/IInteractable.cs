namespace app.Interfaces;

using app.Entities;

public interface IInteractable
{
    string PromptText { get; }
    void Interact(Player player);
}