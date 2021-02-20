using Godot;
using System;
using System.Collections.Generic;

public class Station : StaticBody2D
{   
    private Health _health;

    public override void _Ready()
    {   
        _health = GetNode<Health>("Health");
    }

}
