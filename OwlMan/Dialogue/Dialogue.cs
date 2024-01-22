using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class DialogueEntry
{
	public string ID { get; set; }
	public string Character { get; set; }
	public string Pose { get; set; }
	public string Text { get; set; }
	public string Options { get; set; }
	public string EmitSignal { get; set; }
	public string WaitSeconds { get; set; }
	public string Function { get; set; }
	public string If { get; set; }
	public string Add { get; set; }
}

public class DialogueData
{
	public List<DialogueEntry> Dialogues { get; set; }
}

public partial class Dialogue : CanvasLayer
{

	private DialogueData dialogueData;
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
		GD.Print("Going to parse JSON label for : " + IDLabel);
		ReadJSONFile(IDLabel);
		GD.Print("Finished");	
	}


	public void ReadJSONFile(string IDLabel)
	{
		GD.Print("Starting READ function");
		// Specify the path to your JSON file
		string filePath = "res://Dialogue/Dialogue.json";

		//GD.Print(filePath);

		String Data = ObtainFileString(filePath);

		//GD.Print(Data);
		dialogueData = JsonSerializer.Deserialize<DialogueData>(Data);	

		GD.Print("DIALOGUEDATA");
		GD.Print(dialogueData.Dialogues);


		DialogueEntry entry = GetDialogueEntryByID(IDLabel);


// Use entry.infos to customize the dialog scene
		if (entry != null)
		{
			GD.Print(entry.Text);
		}

	}
	

	private string ObtainFileString(string path)
	{
		var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read); //Readonly
		string result = file.GetAsText();
		file.Close();
		return result;
	}

	private DialogueEntry GetDialogueEntryByID(string targetID)
	{
		return dialogueData.Dialogues.Find(entry => entry.ID == targetID);
	}

}
