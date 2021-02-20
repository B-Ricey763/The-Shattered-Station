using Godot;
using Godot.Collections;
using System;

public class Globals : Node
{   
    [Signal]
    public delegate void AsteroidDestroyed();

    public int Highscore {get; private set;}

    private string _saveFileName = "user://shattered_station_game.save";

    private int _asteroidsDestroyed = 0;

    public int AsteroidsDestroyed 
    {
        get => _asteroidsDestroyed; 
        set {
            _asteroidsDestroyed = value;

            // Send a signal with the current value
            EmitSignal("AsteroidDestroyed", _asteroidsDestroyed);
        }
    }

    public void Save()
    {
        UpdateHighscore();

        File saveGame = new File();
        saveGame.Open(_saveFileName, File.ModeFlags.WriteRead);
        
        saveGame.Store32((uint)Highscore);
        saveGame.Close();

        GD.Print("Saved!");
    }

    public void UpdateHighscore()
    {
        // Change the highscore to the current if we need it
        if (AsteroidsDestroyed > Highscore)
            Highscore = AsteroidsDestroyed;
    }

    public void Load()
    {
        File saveGame = new File();

        if (!saveGame.FileExists(_saveFileName))
        {
            GD.Print("Warning: No save file found!");
            return;
        }

        saveGame.Open(_saveFileName, File.ModeFlags.Read);

        // Super jank type conversion
        Highscore = (int)saveGame.Get32();

        saveGame.Close();

        GD.Print($"Loaded Game! Highscore is {Highscore}");
    }

    public override void _Notification(int what)
    {
        base._Notification(what);
        // Save the game when the user tries to quit
        if (what == MainLoop.NotificationWmQuitRequest)
            Save();
    }

}
