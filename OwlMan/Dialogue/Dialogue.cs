using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Atmo2.Movements.PlayerStates;
using System.Net.NetworkInformation;
using System.Diagnostics;

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
	public bool IsReadyToClose { get; set; } = false;
	
	public DialogueEntry CurrentInfo;
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

	public Button[] GetButtons()
	{
		Button Button1 = this.GetNode<Button>("Control/Options/Option1");
		Button Button2 = this.GetNode<Button>("Control/Options/Option2");
		Button Button3 = this.GetNode<Button>("Control/Options/Option3");

		Button[] buttons = new Button[] {Button1, Button2, Button3};

		return buttons;
	}

	public void OnOption1Pressed()
	{
		GD.Print("PUSHED BUTTON 1");
		ParseJSON(GetButtons()[0].Text);
		SwitchToText();
	}

	public void OnOption2Pressed()
	{
		GD.Print("PUSHED BUTTON 2");
		ParseJSON(GetButtons()[1].Text);
		SwitchToText();
	}

	public void OnOption3Pressed()
	{
		GD.Print("PUSHED BUTTON 3");
		ParseJSON(GetButtons()[2].Text);
		SwitchToText();
	}		

	public void ClearOptions()
	{
		GetButtons()[0].Text = "";
		GetButtons()[1].Text = "";
		GetButtons()[2].Text = "";
	}

	public void CleanOptions()
	{
		if (GetButtons()[0].Text == "")
		{
			GetButtons()[0].Visible = false;
		}
		if (GetButtons()[1].Text == "")
		{
			GetButtons()[1].Visible = false;
		}
		if (GetButtons()[2].Text == "")
		{
			GetButtons()[2].Visible = false;
		}
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

	public void SetVisibleIndicator(bool status)
	{
		AnimatedSprite2D Indicator = this.GetNode<AnimatedSprite2D>("Control/PanelBG/NextIndicator");
		if(status == true)
		{
			Indicator.Visible = true;
		}
		else
		{
			Indicator.Visible = false;
		}
	}

	public void SetVisiblePortrait(bool status)
	{
		TextureRect PortraitRect = this.GetNode<TextureRect>("Control/Portrait");
		if(status == true)
		{
			PortraitRect.Visible = true;
		}
		else
		{
			PortraitRect.Visible = false;
		}
	}

	public void SetVisibleNameBox(bool status)
	{
		Panel NameBox = this.GetNode<Panel>("Control/PanelBG/NameBox");
		if(status == true)
		{
			NameBox.Visible = true;
		}
		else
		{
			NameBox.Visible = false;
		}
	}

	public void SetVisibleOptions(bool status)
	{
		GridContainer Options = this.GetNode<GridContainer>("Control/Options");
		if (status == true)
		{
			Options.Visible = true;
			GetButtons()[0].Visible = true;
			GetButtons()[1].Visible = true;
			GetButtons()[2].Visible = true;
		}
		else
		{
			Options.Visible = false;
		}
	}

	public void SetVisibleText(bool status)
	{
		RichTextLabel Text = this.GetNode<RichTextLabel>("Control/PanelBG/Text");
		if (status == true)
		{
			Text.Visible = true;
		}
		else
		{
			Text.Visible = false;
		}		
	}

	public void FindEndDialog(DialogueEntry entry, string IDLabel)
	{
		if(entry.Function == "end_dialog")
		{
			SetVisibleIndicator(false);
            IsReadyToClose = true;
			
		}
		else
		{
			SetVisibleIndicator(true);
			IsReadyToClose = false;
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

		// GD.Print("DIALOGUEDATA");
		// GD.Print(dialogueData.Dialogues);

		DialogueEntry entry = GetDialogueEntryByID(IDLabel);
		CurrentInfo = entry;

		if (entry != null)
		{
			UpdateDialogueBox(entry);
		}

	}
	public void UpdateDialogueBox(DialogueEntry entry)
	{
		GD.Print(entry.Text);
		ChangeMainLabel(entry.Text);
		ChangeCharacterLabel(entry.Character);
		ChangePortrait(entry.Character, entry.Pose);
		FindEndDialog(entry, entry.ID);
		CleanUpScreen(entry);
	}

	public void ContinueDialogueBox()
	{
		if (CurrentInfo.Options != "")
		{
			SetUpOptions(CurrentInfo);
		}
		else
		{
			GD.Print("Need to find next line of code");
		}
	}


	public void CleanUpScreen(DialogueEntry entry)
	{
		if (entry.Character == string.Empty)
		{
			SetVisiblePortrait(false);
			SetVisibleNameBox(false);
		}

	}

	public void ChangeMainLabel(string newText)
	{
		// Assuming you have a RichTextLabel node in your scene with the name "RichTextLabel"
		RichTextLabel MainLabel = this.GetNode<RichTextLabel>("Control/PanelBG/Text");

		if (MainLabel != null)
		{
			MainLabel.Text = newText;
		}
		else
		{
			GD.Print("Main Label not found. Make sure the node path is correct.");
		}
	}

	public void ChangeCharacterLabel(string NewName)
	{
		RichTextLabel CharacterLabel = this.GetNode<RichTextLabel>("Control/PanelBG/NameBox/Character");

		if(NewName != null)
		{
			CharacterLabel.Text = NewName;
		}
		else
		{
			GD.Print("Character Name not found");
		}
	}

	public void ChangePortrait(string Character, string Pose)
	{
		TextureRect PortraitRect = this.GetNode<TextureRect>("Control/Portrait");
		string ImagePath = "res://Dialogue/Portraits/OwlMan/" + Pose + ".png";
		GD.Print(ImagePath);

		if (System.IO.File.Exists(ImagePath) && Pose != null)
		{
			Texture2D PortraitTexture = GD.Load<Texture2D>(ImagePath);
			PortraitRect.Texture = PortraitTexture;
		}
		else
		{
			GD.Print("There is no image portrait path for : " + ImagePath);
		}
	}

	static string[] ParseOptions(string input)
	{
		string[] items = input.Split(';');

		// Trim whitespace from each item
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = items[i].Trim();
        }

        return items;
	}
	
	public void SetUpOptions(DialogueEntry entry)
	{
		if (entry.Options != "")
		{
			SetVisibleOptions(true);
			SetVisibleText(false);
			ClearOptions();

			Button[] buttons = GetButtons();
			string[] items = ParseOptions(entry.Options);

			for (int i = 0; i < items.Length; i++)
			{
				buttons[i].Text = items[i];

				// Check if the item is empty
				if (string.IsNullOrEmpty(items[i]))
				{
					buttons[i].Visible = false;
				}
				else
				{
					buttons[i].Visible = true;
				}
			}

			CleanOptions();
		}
	}

	public void SwitchToText()
	{
		SetVisibleOptions(false);
		SetVisibleText(true);
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
