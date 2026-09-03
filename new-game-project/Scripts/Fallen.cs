using Godot;
using System;

public partial class Fallen : Node2D
{
	public static bool s = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Active = true;
		GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation = "new_animation";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (s)
		{
			if (Input.IsActionJustPressed("ui_right") && GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation != "new_animation_2") s = false;
			if (Input.IsActionJustPressed("Z"))
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Active = true;
				GetNode<AnimationPlayer>("AnimationPlayer").CurrentAnimation = "new_animation_2";
			}
			GetNode<Sprite2D>("Pointer").Position = new Godot.Vector2(100f,143f);
		}
		else
		{
			if (Input.IsActionJustPressed("ui_left")) s = true;
			GetNode<Sprite2D>("Pointer").Position = new Godot.Vector2(170f,143f);
		}
	}
}
