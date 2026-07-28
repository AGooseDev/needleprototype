using Godot;
using System;
using System.Numerics;

public partial class enemy : CharacterBody2D
{
	public const float Speed = 100.0f;

	public Area2D hitcheck;

	public NavigationAgent2D nav;

	public override void _Ready()
	{
		nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		nav.VelocityComputed += OnNavigationAgent2DVelocityComputed;
    	nav.AvoidanceEnabled = true;
		hitcheck = GetNode<Area2D>("Area2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Godot.Vector2 pos = GlobalTransform.Origin;
		Godot.Vector2 nv = nav.GetNextPathPosition();
		Godot.Vector2 direction = pos.DirectionTo(nv);
		nav.MaxSpeed = Speed;
		nav.Velocity = direction * Speed;

		UpdateTargetPosition(character_body_2d.gpos);
		
		foreach (Area2D i in hitcheck.GetOverlappingAreas())
		{
			if (i.IsInGroup("adrenaline_needle"))
			{
				i.GetParent().QueueFree();
				this.QueueFree();
			}
		}
	}

	private void UpdateTargetPosition(Godot.Vector2 target)
	{
		nav.TargetPosition = target;
	}

	private void OnNavigationAgent2DVelocityComputed(Godot.Vector2 safeVelocity)
    {
        Velocity = Velocity.MoveToward(safeVelocity, 12.0f);
		MoveAndSlide();
    }
}
