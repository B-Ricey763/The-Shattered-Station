using Godot;
using System;
using System.Collections.Generic;

public class Station : Area2D
{   
    [Signal]
    public delegate void HealthChanged();
    [Export]
    public float MaxHealth = 100.0f;

    private List<ITower> _towers = new List<ITower>();

    private float _health;

    // Use a custom setter so health is within bounds
    public float Health {
        get => _health;
        set {
            _health = Mathf.Clamp(value, 0.0f, MaxHealth);
            EmitSignal(nameof(HealthChanged), _health);
        }
    }

    private Area2D _powerRange;

    public override void _Ready()
    {   
        _powerRange = GetNode<Area2D>("PowerRange");
        // Give our starting health
        Health = MaxHealth;
    }

    private void BodyEntered(Node body)
    {
        if (body is Asteroid asteroid)
        {
            // We have been hit! Damage
            Health -= asteroid.Damage;
            // Delete the asteroid
            asteroid.QueueFree();
        }
    }

    public void BodyEnteredPower(Node body)
    {
        if (body.IsInGroup("tower"))
            AddTower(body as RigidBody2D);
    }

    public void BodyExitedPower(Node body)
    {
        if (body.IsInGroup("tower"))
            RemoveTower(body);
    }

    private void AddTower(RigidBody2D tower)
    {
        // The body is a tower, so we know it needs power
        _towers.Add(tower);

        // THIS IS TERRIBLE NEVER DO THIS AGAIN
        Tether powerTether = tower.GetNode<Tether>("Tether");
        // MY EYES ARE BLEEDING SOMEONE HELP ME
        powerTether.Link(tower as Node2D, this);
    }

    private void RemoveTower(RigidBody2D tower)
    {
        _towers.Remove(tower);

        Tether powerTether = (tower as Node2D).GetNode<Tether>("Tether");
        powerTether.Unlink();
    }
}
