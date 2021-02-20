using Godot;
using System;
using System.Collections.Generic;

public class Turret : Tower
{
    [Export]
    public readonly float AttackSpeed = 0.5f;
    [Export]
    public readonly float Damage = 2.0f;

    private List<Asteroid> _targets = new List<Asteroid>();
    private float _elapsedTime = 0.0f;
    private Area2D _detection;
    private Laser _laser;

    public override void _Ready()
    {
        base._Ready();

        _detection = GetNode<Area2D>("Detection");
        _laser = GetNode<Laser>("Laser");

        // Do an inital scan of our area for asteriods that start in it (for safety)
        foreach (Node n in _detection.GetOverlappingBodies())
        {
            if (n is Asteroid asteroid)
                _targets.Add(asteroid);
        }
    }

    public override void _PhysicsProcess(float delta)
    {

        if (_targets.Count > 0)
            AttackTarget(delta);
    }

    /// <summary> Looks at the asteroid and fires a laser </summary>
    private void AttackTarget(float delta)
    {
        Asteroid currentTarget = _targets[0];

        _elapsedTime += delta;
        if (IsPowered) {
            if (_elapsedTime > AttackSpeed)
            {
                // Reset our time variable
                _elapsedTime = 0;

                Node2D hit = _laser.Fire();
                
                if (hit is Asteroid asteroid)
                    asteroid.GetNode<Health>("Health").Value -= Damage;       
            } 
            // TODO: Make it not lookat, but based on physics
            LookAt(currentTarget.GlobalPosition);
        }
    }

    public void BodyEntered(Node body)
    {
        if (body is Asteroid asteroid)
            _targets.Add(asteroid);
    }

    public void BodyExited(Node body)
    {
        if (body is Asteroid asteroid && _targets.Contains(asteroid))
            _targets.Remove(asteroid);
    }
    
}
