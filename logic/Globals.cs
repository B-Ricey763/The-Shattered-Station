using Godot;
using System;
using System.Collections.Generic;

public class Globals : Node
{   
    [Signal]
    public delegate void AsteroidDestroyed();

    public int Highscore {get; private set;}

    private string _saveFileName = "user://shattered_station_game.save";
    private PackedScene _audioPlayer;
    public List<AudioPlayer> AudioPlayers = new List<AudioPlayer>();

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

    public override void _Ready()
    {
        _audioPlayer = GD.Load<PackedScene>("res://scenes/AudioPlayer.tscn");
        PlaySound("res://Heartbeat - Godmode.ogg", true);

        Load();
    }

    public void PlaySound(string path, bool loop)
    {
        // TODO: check for valid path

        AudioStream sound = GD.Load<AudioStream>(path);

        AudioPlayer newAudio = (AudioPlayer) _audioPlayer.Instance();
        newAudio.ShouldLoop = loop;
        
        AddChild(newAudio);
        AudioPlayers.Add(newAudio);

        newAudio.PlaySound(sound);
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

    public void OnEndGame()
    {
        UpdateHighscore();

        GetTree().ChangeScene("res://scenes/EndMenu.tscn");
        
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
