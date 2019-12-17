using Godot;
using System;

public class OneOffSounds : AudioStreamPlayer2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Connect("finished", this, "OnAudioFinish");
        Play(0);
    }

    public void OnAudioFinish()
    {
        QueueFree();
    }
}
