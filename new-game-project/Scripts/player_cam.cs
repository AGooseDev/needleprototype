using Godot;
using System;

public partial class player_cam : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LimitLeft = character_body_2d.cam_clamp_var[0,0];
		LimitTop = character_body_2d.cam_clamp_var[0,1];
		LimitRight = character_body_2d.cam_clamp_var[1,0];
		LimitBottom = character_body_2d.cam_clamp_var[1,1];
		
	}
}
