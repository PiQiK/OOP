namespace app.Entities;

using Godot;

public abstract partial class Character : CharacterBody2D
{
    [Export] public string DisplayName { get; set; } = string.Empty;
    [Export] public float MoveSpeed { get; set; } = 150.0f;

    public Vector2 FacingDirection { get; protected set; } = Vector2.Down;

    public void SetDisplayName(string newName)
    {
        if(!string.IsNullOrWhiteSpace(newName))
        {
            DisplayName = newName.Trim();
        }
    }

    protected void ApplyMovement(Vector2 inputDirection)
    {
        if(inputDirection != Vector2.Zero)
        {
            FacingDirection = inputDirection.Normalized();
            Velocity = FacingDirection * MoveSpeed;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
        
        MoveAndSlide();
    }
}