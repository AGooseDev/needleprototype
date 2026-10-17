using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public partial class pushable : Godot.Area2D
{

	public override void _Ready()
	{
	}
	//OK SO... when i first typed this out, i was stuck for 12 hours
	//trying to figure out why the debug message would not print out...
	//i was looking at the wrong fkn debug window.
	//god i should have just stuck to gamemaker.........
	//ADDENDUM: nevermind i just figure out how to use shaders in under
	//10 minutes. if life were perfect all game engine shader systems would be
	//like godot's
	public override void _PhysicsProcess(double delta)
	{
		
		foreach (Area2D i in GetOverlappingAreas())
		{

			if (i.GetParent().Name == "Player" && i.Name == "psh" && character_body_2d.state == 0 && character_body_2d.pshflag == 1)
			{
				if (character_body_2d.dir == 0 && character_body_2d.ismoving == true)
				{
					this.GetParent<CharacterBody2D>().Velocity = new Godot.Vector2(0,-1 * character_body_2d.SPEED);
				}
				else if (character_body_2d.dir == 1 && character_body_2d.ismoving == true)
				{
					this.GetParent<CharacterBody2D>().Velocity = new Godot.Vector2(0,1 * character_body_2d.SPEED);
				}
				else if (character_body_2d.dir == 2 && character_body_2d.ismoving == true)
				{
					this.GetParent<CharacterBody2D>().Velocity = new Godot.Vector2(-1 * character_body_2d.SPEED,0);
				}
				else if (character_body_2d.dir == 3 && character_body_2d.ismoving == true)
				{
					this.GetParent<CharacterBody2D>().Velocity = new Godot.Vector2(1 * character_body_2d.SPEED,0);
				}
			}
			else if (i.GetParent().Name == "Player" && i.Name == "psh" && character_body_2d.state == 0 && character_body_2d.pshflag == 0)
			{
				this.GetParent<CharacterBody2D>().Velocity = new Godot.Vector2(0,0);
			}
		}
	}


}
