using Godot;
using System;

public class SnowParticles : Particles2D
{
    private BasicCharacter _mainCharacter;

    public override void _Ready()
    {
        var characters = GetTree().GetNodesInGroup("main_character");

        if (characters != null)
        {
            _mainCharacter = characters[0] as BasicCharacter;
        }   
    }

   public override void _Process(float delta)
   {
       SetGlobalPosition(_mainCharacter.GetGlobalPosition());
   }
}
