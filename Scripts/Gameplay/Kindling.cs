using Godot;
using System;

public class Kindling : Node2D, IInteractive, IFuel
{
    private Area2D _interactiveArea;

    [Export]
    public bool CanInteract = true;

    [Export]
    public bool TooltipEnabled = false;

    [Export]
    public NodePath TooltipPath;

    [Export]
    public float FuelAmount = 20.0f; // fire strength++

    public float GetFuelAmount()
    {
        return FuelAmount;
    }

    public override void _Ready()
    {
        _interactiveArea = GetNode("Area2D") as Area2D;
    }

   public void TryInteract(BasicCharacter character)
   {
       if (CanInteract)
       {
            if (TooltipEnabled)
            {
               InteractTooltip tooltip = GetNode(TooltipPath) as InteractTooltip;

               tooltip?.FadeOut();
            }

            CanInteract = false;
            character.PickupObject(this);
       }
   }

   public Area2D GetInteractiveArea()
   {
       return _interactiveArea;
   }
}
