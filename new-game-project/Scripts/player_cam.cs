using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class player_cam : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public static float random_strength = 30.0f;
	public static float shake_fade = 5.0f;

	//public static Random r = new Random();

	public static float shake_strength = 0.0f;
	public override void _Ready()
	{
	}
	
	public static void ApplyShake()
	{
		shake_strength = random_strength;
	}

	public static float RandFRange(float mn, float mx)
	{
		return mn + Random.Shared.NextSingle() * (mx - mn);
	}

	public static Godot.Vector2 RandomOffset()
	{
		return new Godot.Vector2(RandFRange(-shake_strength,shake_strength),RandFRange(-shake_strength,shake_strength));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (shake_strength > 0)
		{
			shake_strength = Mathf.Lerp(shake_strength,0,shake_fade * (float)delta);
			Offset = RandomOffset();
		}
		else
		{
			Offset = new Godot.Vector2(0,0);
		}
		//MakeCurrent();
		LimitLeft = character_body_2d.cam_clamp_var[0,0];
		LimitTop = character_body_2d.cam_clamp_var[0,1];
		LimitRight = character_body_2d.cam_clamp_var[1,0];
		LimitBottom = character_body_2d.cam_clamp_var[1,1];
		
	}
}
