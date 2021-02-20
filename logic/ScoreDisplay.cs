using Godot;
using System;

public class ScoreDisplay : Label
{

    private Globals _globals;

    public override void _Ready()
    {
        _globals = GetNode<Globals>("/root/Globals");
        _globals.Connect("AsteroidDestroyed", this, "UpdateDisplay");

        UpdateDisplay(_globals.AsteroidsDestroyed);
    }

    public void UpdateDisplay(int score)
    {
        Text = $"Score: {score}";
    }

}
