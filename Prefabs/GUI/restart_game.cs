using Godot;
using System;

public class restart_game : Node2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        
    }

public override void _UnhandledInput(InputEvent @event)
{
    if (@event is InputEventKey eventKey)
        if (eventKey.Pressed)// && eventKey.Scancode == (int)KeyList.Escape)
            GetTree().ChangeScene("res://Prefabs/GUI/title_screen.tscn");
}
}
