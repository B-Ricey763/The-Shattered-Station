using Godot;
using System;

public class Asteroid : RigidBody2D
{
    [Export]
    public float TargetingSpeed = 10.0f;
    [Export]
    public float Damage = 10.0f;

    public Node2D Target {get; set;}

    private Random _rand = new Random();

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        if (IsInstanceValid(Target))
            state.LinearVelocity = (Target.GlobalPosition - GlobalPosition).Normalized() * TargetingSpeed;
    }

    public void OnBodyEntered(Node hit)
    {
        if (hit.IsInGroup("damageable"))
            GiveDamage(hit);
    }

    public void GiveDamage(Node hit)
    {
        // Make whatever we hit take damage
        Health health = hit.GetNode<Health>("Health");
        health.Value -= Damage;

        // Kill ouselves
        QueueFree();
    }

    public void OnDied()
    {
        QueueFree();
    }
}
