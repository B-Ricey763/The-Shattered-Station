using Godot;
using System;

public class PauseMenu : Control
{
    [Export]
    public PackedScene Main;

    private Globals _globals;
    private MarginContainer _mc;

    public override void _Ready()
    {
        _globals = GetNode<Globals>("/root/Globals");
        _mc = GetNode<MarginContainer>("MarginContainer");
    }   

    public void TogglePause()
    {
        bool paused = !GetTree().Paused;
        GetTree().Paused = paused;
        _mc.Visible = paused;  
    }

    public void OnPlay()
    {
        TogglePause();
        GetTree().ChangeSceneTo(Main);
    }

    public void OnMenu()
    {
        TogglePause();
        GetTree().ChangeScene("res://scenes/Menu.tscn");
    }

    public void OnQuit()
    {
        GetTree().Quit();
    }
}
