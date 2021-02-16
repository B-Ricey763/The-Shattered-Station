using Godot;
using System;
using System.Collections.Generic;

public class Station : Area2D
{   
    [Signal]
    public delegate void HealthChanged();
    [Export]
    public float MaxHealth = 100.0f;
    [Export]
    public float Power = 1.0f;

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

    public override void _PhysicsProcess(float delta)
    {
        PowerTowers();
    }

    private void PowerTowers()
    {
        float availablePower = Power;
        foreach (ITower tower in _towers)
        {
            if (availablePower - tower.PowerUsage > 0)
            {
                availablePower -= tower.PowerUsage;
                tower.IsPowered = true;
            }
            else
            {
                tower.IsPowered = false;
            }
        }
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
        if (body is ITower tower)
            AddTower(tower);
    }

    public void BodyExitedPower(Node body)
    {
        if (body is ITower tower)
            RemoveTower(tower);
    }

    private void AddTower(ITower tower)
    {
        // The body is a tower, so we know it needs power
        _towers.Add(tower);

        // THIS IS TERRIBLE NEVER DO THIS AGAIN
        Tether powerTether = (tower as Node2D).GetNode<Tether>("Tether");
        // MY EYES ARE BLEEDING SOMEONE HELP ME
        powerTether.Link(tower as Node2D, this);
    }

    private void RemoveTower(ITower tower)
    {
        tower.IsPowered = false;
        _towers.Remove(tower);

        Tether powerTether = (tower as Node2D).GetNode<Tether>("Tether");
        powerTether.Unlink();
    }
}
