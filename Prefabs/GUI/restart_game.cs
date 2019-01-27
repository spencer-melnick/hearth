using Godot;
using System;

public class restart_game : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
	
	public bool CanContinue = false;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }
	
	public void EnableContinue()
	{
		CanContinue = true;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (CanContinue)
		{
		    if (@event is InputEventKey eventKey)
			{
		        if (eventKey.Pressed)// && eventKey.Scancode == (int)KeyList.Escape)
		        {
		            FadeOut();
		        }
			}
			else if (@event is InputEventJoypadButton eventButton)
			{
				if (eventButton.Pressed)
				{
					FadeOut();
				}
			}
		}
	}
	
	public void FadeOut()
	{
		CanContinue = false;
		(GetNode("AnimationPlayer") as AnimationPlayer).Play("fade_out");
	}
	
	public void Restart()
	{
		(GetNode("/root/Globals") as Globals)?.Restart();
	    GetTree().ChangeScene("res://Scenes/dungeon_test_1.tscn");
	}
}

