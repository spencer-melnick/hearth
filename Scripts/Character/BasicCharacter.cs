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

	private bool _fuelHeld = false;

	private Node2D _carriedObject = null;

	private Particles2D _healthParticles;

	public Node2D HeldObject
	{
		get
		{
			return _carriedObject;
		}
	}

	private Node2D _carryAttachPoint;

	public bool FuelHeld
	{
		get
		{
			return _fuelHeld;
		}
	}
	
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
		_carryAttachPoint = GetNode("carry_attach_point") as Node2D;
		_healthParticles = GetNode("health_particles") as Particles2D;
    }

	public override void _Process(float delta)
	{
		CheckHeat(delta);

		if (Input.IsActionJustPressed("character_primary_interact"))
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
		float heatRate = -FreezeSpeed;

		foreach (Node node in fireplaces)
		{
			Fireplace fireplace = node as Fireplace;

			if (fireplace != null)
			{
				float distance = fireplace.GlobalPosition.DistanceTo(GlobalPosition);
				
				if (distance < fireplace.HeatInnerRadius)
				{
					heatRate += fireplace.HeatSpeed;
				}
				else if (distance < fireplace.HeatOuterRadius)
				{
					heatRate += fireplace.HeatSpeed * (fireplace.HeatOuterRadius - distance)
								/ (fireplace.HeatOuterRadius - fireplace.HeatInnerRadius);
				
				}
			}
		}

		_healthParticles.Emitting = false;
		_globals.PlayerHealth += heatRate * delta;
			
		if (_globals.PlayerHealth < 100.0f && heatRate > 0.0f)
		{
			_healthParticles.Emitting = true;
		}
	}

	public void PickupObject(Node2D node)
	{
		if (_carriedObject == null)
		{
			if (node.IsInGroup("fuel"))
			{
				_fuelHeld = true;
			}
			
			_carriedObject = node;
			node.GetParent().RemoveChild(node);
			_carryAttachPoint.AddChild(node);
			node.SetPosition(new Vector2());
		}
	}

	public void DestroyHeldObject()
	{
		_carryAttachPoint.RemoveChild(_carriedObject);
		_carriedObject.QueueFree();

		if (_fuelHeld)
		{
			_fuelHeld = false;
		}
	}
}
