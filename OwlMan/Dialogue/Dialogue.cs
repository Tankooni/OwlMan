using Godot;
using System;

public partial class Dialogue : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Overlord.DialogueScripts = this;
		PlayIndicator();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void PlayIndicator()
	{	
		AnimatedSprite2D AnimatedSprite = this.GetNode<AnimatedSprite2D>("Control/PanelBG/NextIndicator");
		AnimatedSprite.Play("idle");
	}

	

	public void SetVisible(bool status)
	{
		if (status == true)
		{
			Overlord.Dialogue.Visible = true;
		}
		else
		{
			Overlord.Dialogue.Visible = false;
		}
	}

	public void ParseJSON(string IDLabel)
	{
		GD.Print("Going to parse JSON label for :" + IDLabel);
	}

}
