namespace app.Entities;

using Godot;
using app.Interfaces;
using app.Systems;

public partial class Player : Character
{
    private Area2D _interationZone;
    
    public override void _Ready()
    {
        _interationZone = GetNode<Area2D>("InteractionZone");
    }

    public override void _PhysicsProcess(double delta)
    {
        if ( DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive )
        {
            ApplyMovement(Vector2.Zero);
            return;
        }

        Vector2 inputDirection = Input.GetVector("Move_Left", "Move_Right", "Move_Up", "Move_Down");
        ApplyMovement(inputDirection);
        UpdateInteractionZoneDirection();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionPressed("Interact"))
        {
            if( DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
            }
            else
            {
                TryInteract();
            }
        }
    }

    private void UpdateInteractionZoneDirection()
    {
        if(FacingDirection != Vector2.Zero)
        {
            _interationZone.Rotation = FacingDirection.Angle();
        }
    }

    private void TryInteract()
    {
        foreach ( Node2D body in _interationZone.GetOverlappingBodies() )
        {
            if( body is IInteractable interactable)
            {
                interactable.Interact(this);
                return;
            }
        }
    }
}