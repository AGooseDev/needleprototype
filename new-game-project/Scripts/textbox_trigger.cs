using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Data;

[GlobalClass]
public partial class textbox_trigger : Godot.Area2D
{
	[Export] public int Sig = 0;
	[Export] public string Text = "";
	private bool locked = false;

	[Export(PropertyHint.MultilineText, "")] public Godot.Collections.Array<string> txtarr0 = new Godot.Collections.Array<string> { "a"};

	public override void _Ready()
	{

		ProcessPhysicsPriority = -10;
		

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
		
		bool zHeld = Input.IsActionPressed("Z");

		if (locked && !zHeld)
			locked = false;

		if (locked)
			return;

		foreach (Area2D i in GetOverlappingAreas())
		{
			if (i.GetParent().Name == "Player")
			{
				
				if (character_body_2d.state == 0 && zHeld)
				{					
					character_body_2d.Instance.Txtarr0 = txtarr0;
					character_body_2d.state = 1;

					locked = true;
				}
			}
		}
	}


}
