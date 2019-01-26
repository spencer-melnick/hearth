using Godot;
using System;

using AdventureGame;
public class BasicCharacter : KinematicBody2D
{
    [Export]
	public float WalkSpeed = 80.0f;

	[Export]
	public float FreezeSpeed = 10.0f;

	private Globals _globals;
	
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
		_globals = GetNode("/root/Globals") as Globals;
    }

	public override void _Process(float delta)
	{
		CheckHeat(delta);

		if (Input.IsActionPressed("character_primary_interact"))
		{
			_checkInteractions();
		}
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

	private void _checkInteractions()
	{
		var interactables = GetTree().GetNodesInGroup("interactive");

		foreach (Node node in interactables)
		{
			IInteractive interactable = node as IInteractive;

			if (interactable != null)
			{
				Area2D interactiveArea = interactable.GetInteractiveArea();

				if (interactiveArea.OverlapsBody(this))
				{
					interactable.TryInteract(this);
				}
			}
		}
	}

	private void CheckHeat(float delta)
	{
		var fireplaces = GetTree().GetNodesInGroup("fireplace");

		// Defines how fast (in % of FreezeSpeed) the player's health will decay.
		// Take the smallest value - freeze factor is the percent distance of the player
		// from the inner radius of the fire to the outer radius.
		// Valid between -1.0f and 1.0f.
		// A value of less than 0.0f indicates that the player is being healed
		// in which case, the max heal speed will be set according to that fireplace's
		// heal speed.
		float minFreezeFactor = 1.0f;

		// Heal
		float maxHealSpeed = 0.0f;

		foreach (Node node in fireplaces)
		{
			Fireplace fireplace = node as Fireplace;

			if (fireplace != null)
			{
				float distance = fireplace.GlobalPosition.DistanceTo(GlobalPosition);
				
				// Note - multiple fires healing the player at once could be weird!
				// Not currently handled
				if (fireplace.HealEnabled && distance < fireplace.HealInnerRadius)
				{
					minFreezeFactor = -1.0f;
					maxHealSpeed = fireplace.HealSpeed;
					break;
				}
				else if (fireplace.HealEnabled && distance < fireplace.HealOuterRadius)
				{
					float freezeFactor = -(distance - fireplace.HealInnerRadius)
										  / (fireplace.HealOuterRadius - fireplace.HealInnerRadius);
				
					if (freezeFactor < minFreezeFactor)
					{
						minFreezeFactor = freezeFactor;
						maxHealSpeed = fireplace.HealSpeed;
					}
				}
				else if (distance < fireplace.HeatInnerRadius)
				{
					if (minFreezeFactor > 0.0f)
					{
						minFreezeFactor = 0.0f;
					}
				}
				else if (distance < fireplace.HeatOuterRadius)
				{
					float freezeFactor = (distance - fireplace.HeatInnerRadius)
										  / (fireplace.HeatOuterRadius - fireplace.HeatInnerRadius);
				
					if (freezeFactor < minFreezeFactor)
					{
						minFreezeFactor = freezeFactor;
					}
				}
			}
		}

		if (minFreezeFactor > 0.0f)
		{
			_globals.PlayerHealth -= FreezeSpeed * minFreezeFactor * delta;
		}
		else
		{
			_globals.PlayerHealth -= maxHealSpeed * minFreezeFactor * delta;
		}
	}

	public Node2D GetCarryAttachPoint()
	{
		return GetNode("carry_attach_point") as Node2D;
	}
}
