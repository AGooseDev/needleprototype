using Godot;
using System;

public partial class Rope : Node2D
{
	public RigidBody2D current_anchor;
	public RigidBody2D previous_anchor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		current_anchor = GetNode<RigidBody2D>("RigidBody2D2");
		previous_anchor = current_anchor;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetNode<RigidBody2D>("RigidBody2D").GlobalPosition = GetGlobalMousePosition();
		
	}
}
