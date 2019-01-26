using Godot;
using System;

public class Kindling : Node2D, IInteractive
{
    private Area2D _interactiveArea;

    [Export]
    public bool CanInteract = true;

    public override void _Ready()
    {
        _interactiveArea = GetNode("Area2D") as Area2D;
    }

   public void TryInteract(BasicCharacter character)
   {
       if (CanInteract)
       {
           CanInteract = false;
           GetParent().RemoveChild(this);
           character.GetCarryAttachPoint().AddChild(this);
           SetPosition(new Vector2());
       }
   }

   public Area2D GetInteractiveArea()
   {
       return _interactiveArea;
   }
}
