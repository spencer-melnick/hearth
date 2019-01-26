using Godot;
using System;

using AdventureGame;

public class PushableBlock : KinematicBody2D, IPushable
{
    [Export]
    public float PushSpeed = 40.0f;

    public override void _Ready()
    {
        
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }

    void IPushable.TryPush(Vector2 normal, float delta)
    {
        MoveAndCollide(-normal * delta * PushSpeed);
    }
}
