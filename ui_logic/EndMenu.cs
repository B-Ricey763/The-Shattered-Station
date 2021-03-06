using Godot;
using System;

public class EndMenu : Control
{
    [Export]
    public PackedScene Main;
    [Export]
    public NodePath HighscorePath;
    [Export]
    public NodePath ScorePath;

    private Label _highscore;
    private Label _score;
    private Globals _globals;

    public override void _Ready()
    {
        _highscore = GetNode<Label>(HighscorePath);
        _score = GetNode<Label>(ScorePath);
        _globals = GetNode<Globals>("/root/Globals");

        _globals.Load();

        _highscore.Text = $"Highscore: {_globals.Highscore}";
        _score.Text = $"Score: {_globals.AsteroidsDestroyed}";
    }

    public void OnPlay()
    {
        GetTree().ChangeSceneTo(Main);
    }

    public void OnMenu()
    {
        GetTree().ChangeScene("res://scenes/Menu.tscn");
    }

    public void OnQuit()
    {
        GetTree().Quit();
    }
}
