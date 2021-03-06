using Godot;
using System;

public class Menu : Control
{
    [Export]
    public PackedScene Main;
    [Export]
    public NodePath HighscorePath;

    private Label _highscore;
    private Globals _globals;

    public override void _Ready()
    {
        _highscore = GetNode<Label>(HighscorePath);
        _globals = GetNode<Globals>("/root/Globals");

        _globals.Load();

        _highscore.Text = $"Highscore: {_globals.Highscore}";
    }

    public void OnPlay()
    {
        GetTree().ChangeSceneTo(Main);
    }

    public void OnSettings()
    {

    }

    public void OnQuit()
    {
        GetTree().Quit();
    }
}
