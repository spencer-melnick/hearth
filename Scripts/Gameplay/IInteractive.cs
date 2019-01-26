using Godot;
using System;

public interface IInteractive
{
    void TryInteract(BasicCharacter character);

    Area2D GetInteractiveArea();
}
