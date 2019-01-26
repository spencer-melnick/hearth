using Godot;
using System;

using AdventureGame;
public class BasicCharacter : KinematicBody2D
{
    [Export]
	public float WalkSpeed = 80.0f;
	
	private struct AxisMapping
	{
		public string ActionName;
		public Vector2 Axis;
		
		public AxisMapping(string actionName, Vector2 axis)
		{
			ActionName = actionName;
			Axis = axis;
		}
	}
	
	private readonly AxisMapping[] _axisMappings = {
		new AxisMapping("character_move_left", new Vector2(-1.0f, 0.0f)),
		new AxisMapping("character_move_right", new Vector2(1.0f, 0.0f)),
		new AxisMapping("character_move_up", new Vector2(0.0f, -1.0f)),
		new AxisMapping("character_move_down", new Vector2(0.0f, 1.0f))
	};

    public override void _Ready()
    {

    }

	public override void _PhysicsProcess(float delta) {
		Vector2 axisInput = new Vector2(0.0f, 0.0f);

		foreach (AxisMapping axisMapping in _axisMappings)
		{
			if (Input.IsActionPressed(axisMapping.ActionName)) {
				axisInput += axisMapping.Axis;
			}
		}

		axisInput = axisInput.Normalized();
		Vector2 velocity = axisInput * WalkSpeed;
		KinematicCollision2D collision = MoveAndCollide(velocity * delta);

		if (collision != null)
		{
			Node2D collider = collision.Collider as Node2D;
			if (collider != null && collider.IsInGroup("pushable")) {
				IPushable pushedObject = collider as IPushable;
				pushedObject?.TryPush(collision.Normal, delta);
			}

			velocity = velocity.Slide(collision.Normal);
			MoveAndCollide(velocity * delta);
		}
	}
}
