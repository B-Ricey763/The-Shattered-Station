using Godot;
using System;

public class Asteroid : RigidBody2D
{
    [Signal]
    public delegate void HealthChanged();

    [Export]
    public float TargetingSpeed = 10.0f;
    [Export]
    public float Damage = 10.0f;
    [Export]
    public float MaxHealth = 10.0f;

    private float _health;

    public float Health {
        get => _health;
        set {
            _health = Mathf.Clamp(value, 0.0f, MaxHealth);
            if (_health == 0.0f)
                QueueFree();
            EmitSignal(nameof(HealthChanged), _health);
        }
    }

    public Node2D Target {get; set;}

    private Random _rand = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Health = MaxHealth;
        Target = GetNode<Node2D>("/root/Main/Station");
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (IsInstanceValid(Target))
            state.LinearVelocity = (Target.GlobalPosition - GlobalPosition).Normalized() * TargetingSpeed;
    }
}
