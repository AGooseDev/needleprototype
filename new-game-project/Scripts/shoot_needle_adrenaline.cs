using Godot;
using System;
using System.ComponentModel;

//shit
public partial class shoot_needle_adrenaline : CharacterBody2D
{
	//im genuinly considering making my variables straight up emojis
	[Export] public float spd = 300.0f;
	[Export] public int effect = 0;
	public override void _Ready()
	{
		//this is utterly dumb
		if (character_body_2d.dir == 0) {RotationDegrees = 270;}
		else if (character_body_2d.dir == 1) {RotationDegrees = 90;}
		else if (character_body_2d.dir == 2) {RotationDegrees = 180;}
		else if (character_body_2d.dir == 3) {RotationDegrees = 0;}
	}
	public override void _Process(double delta)
	{	
		//temp shit to delete it cause im too tired
		//to make it delete when it hits a static body at the moment
		//i think its like 3:30 am rn
		if (Position.X == 1500 || Position.Y == 1500) QueueFree();

		//i do not like this.
		Position += Vector2.Right.Rotated(Mathf.DegToRad(RotationDegrees)) * spd * (float)delta;
	}
}
