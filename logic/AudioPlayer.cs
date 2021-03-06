using Godot;
using System;

public class AudioPlayer : Node2D
{
    public bool ShouldLoop = false;

    private AudioStreamPlayer _audio;
    private Globals _globals;

    public override void _Ready()
    {
       _audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer"); 
       _globals = GetNode<Globals>("/root/Globals");
    }

    public void PlaySound(AudioStream stream)
    {
        _audio.Stream = stream;
        _audio.Play();
    }

    public void OnFinished()
    {
        if (ShouldLoop)
        {
            _audio.Play();
            return;
        }
        _globals.AudioPlayers.Remove(this);
        _audio.Stop();
        QueueFree();
    }

}   
