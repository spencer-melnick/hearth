using Godot;
using System;

public class FirstFireplace : Fireplace, IInteractive
{
    private Area2D _area;
    private InteractTooltip _tooltip;
    private Globals _globals;
    private BasicCharacter _mainCharacter;


    public override void _Ready()
    {
        _area = GetNode("Area2D") as Area2D;
        _tooltip = GetNode("interact_tooltip") as InteractTooltip;
        _globals = GetNode("/root/Globals") as Globals;

        var characters = GetTree().GetNodesInGroup("main_character");

        if (characters != null)
        {
            _mainCharacter = characters[0] as BasicCharacter;
        }   
    }

    public void TryInteract(BasicCharacter character)
    {
        if (character.FuelHeld)
        {
            GD.Print("Player put stuff into the fire!");
            character.DestroyHeldObject();
            _tooltip.FadeOut();
        }
    }

    public Area2D GetInteractiveArea()
    {
        return _area;
    }

    public override void _Process(float delta)
    {
        _tooltip.Enabled = _mainCharacter.FuelHeld;

        if (_mainCharacter.FuelHeld)
        {
            _tooltip.FadeIn();
        }
    }
}
