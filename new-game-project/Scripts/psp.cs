using Godot;
using System;

public partial class psp : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public override void _PhysicsProcess(double delta)
	{
		
		ZAsRelative = false;
		var e = GetNode<CharacterBody2D>("../../Player");
		if (GlobalPosition.Y >= e.GlobalPosition.Y)
		{
			
			ZIndex = e.ZIndex + 1;
		}
		else if (GlobalPosition.Y < e.GlobalPosition.Y)
		{
			ZIndex = e.ZIndex - 1;
		}
		
		MoveAndSlide();
	}
}
