using Godot;
using System;
using System.Net.Sockets;

public partial class Shoptest : Node2D
{
	public static int state = 0;
	public static int[] menu = [0,0];
	public static int sv = 0;
	public static int selm = 0;
	public static int itemchoice = 0;

	public static bool exist = false;
	
	public static bool z = false;
	public static bool x = false;
	public Godot.Collections.Array<string> txtarr0 = new Godot.Collections.Array<string> { "Yeah Im broke chief."};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		exist = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (state)
		{
			case 0:
				character_body_2d.state = 10;
				GetNode<AnimationPlayer>("CanvasLayer/AnimationPlayer").Play("new_animation");
				state = 1;
				break;
			
			case 1:
				if (menu[0] == 0)
				{
					if (Input.IsActionJustReleased("X")) x = false;
					selm = 2;
					if (Input.IsActionJustPressed("ui_up")) sv--;
					else if (Input.IsActionJustPressed("ui_down")) sv++;
					if (sv < 0) sv = selm;
					if (sv > selm) sv = 0;

					switch (sv)
					{
						case 0:
							GetNode<Sprite2D>("CanvasLayer/Pointer").GlobalPosition = new Vector2(12, 139);
							if (Input.IsActionJustPressed("Z") && z == false)
							{
								menu = [1,0];
								sv = 0;
								z = true;
								GetNode<Control>("CanvasLayer/Control").Visible = true;
							}
							break;
						case 1:
							if (Input.IsActionJustPressed("Z") && z == false)
							{
								menu = [1,0];
								sv = 0;
								z = true;
								GetNode<Control>("CanvasLayer/Control").Visible = true;
							}
							GetNode<Sprite2D>("CanvasLayer/Pointer").GlobalPosition = new Vector2(12,154);
							break;
						case 2:
							if (Input.IsActionJustPressed("Z") && z == false)
							{
								menu = [1,0];
								sv = 0;
								z = true;
								GetNode<Control>("CanvasLayer/Control").Visible = true;
							}
							GetNode<Sprite2D>("CanvasLayer/Pointer").GlobalPosition = new Vector2(12,169);
							break;
					}
				}
				if (menu[0] == 1 && menu[1] == 0)
				{
					selm = 6;
					if (Input.IsActionJustPressed("ui_up")) sv--;
					else if (Input.IsActionJustPressed("ui_down")) sv++;
					if (sv < 0) sv = selm;
					if (sv > selm) sv = 0;
					if (Input.IsActionJustReleased("Z"))
					{
						z = false;
					}
					if (Input.IsActionJustPressed("X") && x == false)
					{
						x = true;
						sv = 0;
						menu[0] = 0;
						menu[1] = 0;
						GetNode<Control>("CanvasLayer/Control").Visible = false;
					}
					switch (sv)
					{
						case 0:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* It can accela-heal you physicaly! WARNING: MESSES W- [Scratched..]";
							break;
						
						case 1:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* Well I know what adrenaline is.";
							break;

						case 2:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* A Tungsten needle tip. Pokey. I wonder why the other needles dont have this?";
							break;
						
						case 3:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* ...";
							break;
						
						case 4:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* This is......... uhh....... \n  ...\n  hmmmm.............";
							break;

						case 5:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* Holy crap I can do some damage with this...";
							break;

						case 6:
							GetNode<Label>("CanvasLayer/Control/ColorRect/Label").Text = "* Looks heavy but it should help...";
							break;
					}
					
					GetNode<Sprite2D>("CanvasLayer/Pointer").GlobalPosition = new Vector2(28,17 + (16*sv));

					if (Input.IsActionJustPressed("Z") && z == false)
					{
						GetNode<ColorRect>("CanvasLayer/Control/ColorRect").Visible = false;
						GetNode<NinePatchRect>("CanvasLayer/NinePatchRect2").Visible = true;
						z = true;
						itemchoice = sv;
						GetNode<Control>("CanvasLayer/Control/NinePatchRect3").Visible = true;
						menu = [2,0];
						sv = 0;
					}
					if (Input.IsActionJustReleased("X")) x = false;
				}
				if (menu[0] == 2 && menu[1] == 0)
				{
					switch (itemchoice)
					{
						case 0:
							GetNode<Label>("CanvasLayer/Control/NinePatchRect3/Label").Text = "1. \n adrenaline 8 \n molonol 1 \n \n 2. \n Thorium 54 \n \n \n 3. \n Molonol 7 \n \n \n 4. \n journal page";
							break;
						case 1:
							GetNode<Label>("CanvasLayer/Control/NinePatchRect3/Label").Text = "1. \n Thorium 4 \n needlestick \n \n 2. \n Thorium 10 \n \n \n 3. \n COMT \n \n \n 4. \n big ass cube";
							break;
						case 2:
							GetNode<Label>("CanvasLayer/Control/NinePatchRect3/Label").Text = "1. \n Thorium 4 \n needlestick \n \n 2. \n Thorium 10 \n \n \n 3. \n COMT \n \n \n 4. \n big ass cube";
							break;
						case 4:
							GetNode<Label>("CanvasLayer/Control/NinePatchRect3/Label").Text = "1. \n Thorium 18 \n \n \n 2. \n Thorium 18 \n \n \n 3. \n Thorium 18 \n \n \n 4. \n Thorium 18";
							break;
					}
					selm = 3;
					if (Input.IsActionJustPressed("ui_up")) sv--;
					else if (Input.IsActionJustPressed("ui_down")) sv++;
					if (sv < 0) sv = selm;
					if (sv > selm) sv = 0;
					GetNode<NinePatchRect>("CanvasLayer/NinePatchRect2").GlobalPosition = new Godot.Vector2(212,9+(sv*40));
					if (Input.IsActionJustReleased("Z")) z = false;

					if (Input.IsActionJustPressed("Z") && z == false)
					{
						z = true;
						character_body_2d.zpr = true;
						menu = [2,1];
						//return; //idk why but a return function with no return value looks scary
								//like it shouldnt yk? its a return keyword but like idk it just looks so strange sometimes
								//then again i AM running low on caffine so maybe ill pick up some after codin this.
					}

					if (Input.IsActionJustPressed("X") && x == false)
					{
						GetNode<NinePatchRect>("CanvasLayer/NinePatchRect2").Visible = false;
						x = true;
						sv = 0;
						menu[0] = 1;
						menu[1] = 0;
						GetNode<NinePatchRect>("CanvasLayer/Control/NinePatchRect3").Visible = false;
						GetNode<ColorRect>("CanvasLayer/Control/ColorRect").Visible = true;
					}
					if (Input.IsActionJustReleased("X")) x = false;
				}
				if (menu[0] == 2 && menu[1] == 1)
				{
					character_body_2d.Instance.Txtarr0 = txtarr0;
					character_body_2d.state = 1;
					
					menu = [2,2];
					//return;
				}
				if (menu[0] == 2 && menu[1] == 2)
				{
					
				}

				break;

		}
		
		
	}
}
