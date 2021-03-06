using Godot;
using System;
using System.Collections.Generic;

public class Station : StaticBody2D
{   
    private Globals _globals;

    public override void _Ready()
    {   
        _globals = GetNode<Globals>("/root/Globals");
    }

    public void OnDied()
    {
        _globals.OnEndGame();
    }

}
