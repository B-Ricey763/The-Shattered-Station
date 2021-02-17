using Godot;
using System;
using System.Collections.Generic;

public class Station : Area2D
{   
    private Health _health;

    public override void _Ready()
    {   
        _health = GetNode<Health>("Health");
    }

    public void BodyEntered(Node body)
    {
        if (body is Asteroid asteroid)
        {
            // We have been hit! Damage
            _health.Value -= asteroid.Damage;
            // Delete the asteroid
            asteroid.QueueFree();
        }
    }
}
