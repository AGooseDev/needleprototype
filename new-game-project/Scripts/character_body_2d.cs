using Godot;
using Godot.Collections;
using System.Collections.Generic;


[GlobalClass]
public partial class character_body_2d : CharacterBody2D
{
	//i dont like public and private, idk, idk why every var
	//cant just be public, if you dont want to use it in other classes
	//then just dont fucking use it, you can only access it by
	//typing the classname anyway, if anyone uses private i will actually
	//send a pipe bomb to their mailbox.
	public static float SPEED = 80.0f;

	public int Txtarrlen;
	public static Label Txtlbl;
	public static bool can_shoot = true;

	public static bool zpr = false;

	public static float blood_toxicity = 0.08f;
	public static float heartrate = 74;
	public static float bloodpressure = 100.0f;
	public static float physicalhealth = 99.999f;
	public static float head_integrity = 100.00f;
	public static float sanity = 100.0f;

	//aparently 2d arrays are defined with a single comma in the brackets for some fkn reason
	//dont know why it couldent have just been a number
	//anyway DONT DELETE IT
	public static int[,] cam_clamp_var = {{0,0},{320,180}};
	[Export] public int[] min = [0,0];
	[Export] public int[] max = [320,180];
	public static CanvasLayer cv;

	public static int effect = -1;
	public static int adrenalinetimer = 10*60;

	public float charge_time = 0f;
	public float max_charge_time = 1.4f;
	public float min_needle_speed = 10f;
	public float max_needle_speed = 400f;

	public int page = 0;

	public static int dir = 0;

	//i dont even think i ever used this
	public static character_body_2d Instance;

	[Export(PropertyHint.MultilineText, "")] public Godot.Collections.Array<string> Txtarr0 = new Godot.Collections.Array<string> { "a" };
	public float Txtmth;

	public static int state = 0;
	
	Camera2D cm;
	[Export] public int state_override = 0;
	[Export] public inventory inv;
	public static int invcount = 4;

	//menu shit
	public static int px = 0;
	public static int py = 0;
	public static int pmax = 2;
	public static int pval = 0;
	public static int[] current_menu = {0,0};
	public static int equipped = 0;
	public static Godot.Vector2 gpos;

	public static int spr = 0;


	public int Pickup(inventory_item id)
	{
		// First, try to stack it.
		for (int i = 0; i < invcount; i++)
		{
			if (inv.inv[i] == id)
			{
				menu.count[i]++;
				return 0;
			}
		}

		// No existing stack, so find an empty slot.
		for (int i = 0; i < invcount; i++)
		{
			if (inv.inv[i] == null)
			{
				inv.inv[i] = id;
				menu.count[i] = 1;
				return 0;
			}
		}

		// Inventory full.
		return -1;
	}

	public override void _Ready()
	{
		state = state_override;
		//again i dont even think i ever used this
		Instance = this;	
		cv = GetNode<CanvasLayer>("Camera2D/CanvasLayer");
		cm = GetNode<Camera2D>("Camera2D");
		Txtlbl = GetNode<Label>("Camera2D/CanvasLayer/Control/Label");
		Txtarrlen = Txtlbl.GetTotalCharacterCount();
		Txtmth = Txtlbl.VisibleRatio / Txtarrlen;

		Txtlbl.VisibleRatio = 0;
		Pickup(GD.Load<inventory_item>("res://tres/adrenaline_needle.tres"));
	}

	public override void _PhysicsProcess(double _delta)
	{
		//GetNode<Label>("Camera2D/CanvasLayer2/Label").Text = equipped.ToString();
		cam_clamp_var[0,0] = min[0];
		cam_clamp_var[0,1] = min[1];
		cam_clamp_var[1,0] = max[0];
		cam_clamp_var[1,1] = max[1];
		if (character_body_2d.cv.Visible == false)
		{
			character_body_2d.can_shoot = true;
		}
		else
		{
			character_body_2d.can_shoot = false;
			charge_time = 0f;
		}

		gpos = GlobalPosition;

		switch (state)
		{
			//yes, i did not use enums to name the states. now shut up.
			//(i should have used enums when i started this ngl)
			case 0:

				switch (effect)
				{
					case -2:
						
						break;

					case -1:
						GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Visible = false;
						GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect").Visible = false;
						break;

					case 0:
						GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Visible = true;
						GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect").Visible = true;
						if (adrenalinetimer >= 0)
						{
							adrenalinetimer--;
							head_integrity -= 0.005f;
							bloodpressure += 0.002f;
						}
						else
						{
							effect = -1;
							adrenalinetimer = 180;
							
						}
						break;
				}

				GetNode<CanvasLayer>("Camera2D/CanvasLayer3").Visible = false;

				if (Input.IsActionPressed("C"))
				{
					state = 3;
					break;
				}

				//yes the progress bar upgrade is snappy n shit but im too tired to even try fixing that rn
				GetNode<TextureProgressBar>("Camera2D/CanvasLayer2/PB").MaxValue = 100;
				GetNode<TextureProgressBar>("Camera2D/CanvasLayer2/PB").Value = charge_time * 100 / max_charge_time;
				
				if (inv.inv[equipped] != null)
				{
					if (can_shoot && (inv.inv[equipped].is_needle == true))
					{
						if (Input.IsActionPressed("Z"))
						{
							charge_time = Mathf.Min(charge_time + (float)_delta, max_charge_time);
						}

						if (Input.IsActionJustPressed("S"))
						{
							
							if (menu.count[equipped] > 0)
							{
								effect = inv.inv[equipped].pid;
								menu.count[equipped]--;
								if (menu.count[equipped] <= 0)
								{
									inv.inv[equipped] = null;
									menu.count[equipped] = 0;
								}
							}
						}

						if (Input.IsActionJustReleased("Z") && (charge_time > 0.0f)) 
						{
							if ((menu.count[pval]-1) <= 0)
							{
								inv.inv[pval] = null;
								menu.count[pval] = 0;
							}
							else if (menu.count[pval] > 0)
							{
								menu.count[pval]--;
							}
							var scene = GD.Load<PackedScene>("res://Scenes/shoot_needle_adrenaline.tscn");
							var instance = scene.Instantiate<Sprite2D>();

							instance.GlobalPosition = GlobalPosition;

							float charge_ratio = charge_time / max_charge_time;
							float needle_speed = Mathf.Lerp(min_needle_speed,max_needle_speed,charge_ratio);

							instance.Set("spd", needle_speed);

							GetTree().CurrentScene.AddChild(instance);

							charge_time = 0f;
						}
					}
				}
				if (Input.IsActionJustReleased("ui_down"))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idledown");
				}
				if (Input.IsActionJustReleased("ui_up"))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleup");
				}
				if (Input.IsActionJustReleased("ui_left"))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleleft");
				}
				if (Input.IsActionJustReleased("ui_right"))
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleright");
				}
				var horizontal_direction = Input.GetAxis("ui_left", "ui_right");
				//i just need to find out what dimensional plane lesbians are on and how to access it
				//ADDENDUM: its like 5 pm rn and i have no idea why tf i typed that
				var vertical_direction = Input.GetAxis("ui_up", "ui_down");
				if (vertical_direction != 0)
				{
					GetNode<Node2D>("Area2D").Rotation = 0;
					Velocity = new Vector2(Velocity.X, vertical_direction * SPEED);
					
					if (vertical_direction == -1)
					{
						dir = 0;
						if (horizontal_direction == 0) GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("up");
					}
					else if (vertical_direction == 1)
					{
						dir = 1;
						if (horizontal_direction == 0) GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("down");
					}
				}
				else
				{
					Velocity = new Vector2(
						Velocity.X,
						Mathf.MoveToward(Velocity.X, 0, SPEED)
					);
				}

				
				if (horizontal_direction != 0)
				{
					GetNode<Node2D>("Area2D").RotationDegrees = 90;
					Velocity = new Vector2(horizontal_direction * SPEED, Velocity.Y);
				
					if (horizontal_direction == -1)
					{
						dir = 2;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("left");
					}
					else if (horizontal_direction == 1)
					{
						dir = 3;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("right");
					}
				}
				else
				{
					Velocity = new Vector2(
						Mathf.MoveToward(Velocity.Y, 0, SPEED),
						Velocity.Y
					);
				}


				MoveAndSlide();

				break;

			case 1:
				if (Input.IsActionJustReleased("Z"))
				{
					zpr = false;
				}
				//most nice looking C# code in the history of the world here
				Txtlbl.Text = Txtarr0[page];
				Txtarrlen = Txtlbl.GetTotalCharacterCount();
				Txtmth = 1.0f / Txtarrlen;
				GetNode<CanvasLayer>("Camera2D/CanvasLayer").Visible = true;
				if (page == 0 && zpr == false && Input.IsActionJustPressed("Z"))
				{
					Txtlbl.VisibleRatio = 1;
				}
				if (!(Txtlbl.VisibleRatio >= 1))
				{
					Txtlbl.VisibleRatio += Txtmth / 2;

					if (Input.IsActionJustPressed("Z") && page != 0)
					{
						Txtlbl.VisibleRatio = 1;
					}
				}
				else if (Txtlbl.VisibleRatio >= 1)
				{
					if (Input.IsActionJustPressed("Z"))
					{
						if (page + 1 < Txtarr0.Count) {page++;}
						else {state = 0; page = 0; GetNode<CanvasLayer>("Camera2D/CanvasLayer").Visible = false;}
						Input.ActionRelease("Z"); Input.FlushBufferedEvents();
						Txtlbl.VisibleRatio = 0;
					}
				}
				break;
			
			case 2:
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = false;
				break;

			case 3:
				GetNode<CanvasLayer>("Camera2D/CanvasLayer3").Visible = true;
				if (GetNode<CanvasLayer>("Camera2D/CanvasLayer3").Visible == true)
				{
					var p = GetNode<TextureRect>("Camera2D/CanvasLayer3/TextureRect");
					Godot.Vector2 t = new Vector2(px, py);

					p.Position = t;

					if (current_menu[0] == 0)
					{
						if (Input.IsActionJustPressed("X"))
						{
							state = 0;
						}

						pmax = 2;
						if (Input.IsActionJustPressed("ui_up")) pval--;
						else if (Input.IsActionJustPressed("ui_down")) pval++;
						if (pval < 0) pval = pmax;
						else if (pval > pmax) pval = 0;
						GetNode<Label>("Camera2D/CanvasLayer3/Label2").Text = "";

						
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt").Visible = false;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt2").Visible = false;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt3").Visible = false;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt4").Visible = false;
						GetNode<Sprite2D>("Camera2D/CanvasLayer3/Sprite0005").Visible = false;

						if (Input.IsActionJustPressed("Z"))
						{
							if (pval == 0) 
							{
								current_menu[0] = 1;
								zpr = true;
							}
							else if (pval == 1)
							{
								current_menu[0] = 2;
							}
						}
						
						px = 10;
						py = 12+(15*pval);
					}
					else if (current_menu[0] == 1)
					{
						GetNode<Sprite2D>("Camera2D/CanvasLayer3/Sprite0005").Position = new Vector2(91+(36*equipped), GetNode<Sprite2D>("Camera2D/CanvasLayer3/Sprite0005").Position.Y);
						pmax = 3;
						if (Input.IsActionJustPressed("ui_left")) pval--;
						else if (Input.IsActionJustPressed("ui_right")) pval++;

						if (pval < 0) pval = pmax;
						else if (pval > pmax) pval = 0;
						pmax = invcount - 1;
						px = 61+(36*pval);
						py = 14;
						
						if (Input.IsActionJustPressed("C"))
						{
							//drop items one at a time
							if ((menu.count[pval]-1) <= 0)
							{
								inv.inv[pval] = null;
								menu.count[pval] = 0;
							}
							else if (menu.count[pval] > 0)
							{
								menu.count[pval]--;
							}
						}
						if (Input.IsActionJustPressed("Z") && zpr == false)
						{
							//make the current selected slot to be equipped
							equipped = pval;
						}
						if (Input.IsActionJustReleased("Z"))
						{
							zpr = false;
						}
						if (Input.IsActionJustPressed("X"))
						{
							current_menu[0] = 0;
							pval = 0;
						}

						if (Input.IsActionJustPressed("ui_up")) pval--;
						else if (Input.IsActionJustPressed("ui_down")) pval++;

						if (pval < 0) pval = pmax;
						else if (pval > pmax) pval = 0;

						if (inv.inv[pval] != null)
						{
							GetNode<Label>("Camera2D/CanvasLayer3/Label2").Text = inv.inv[pval].desc;
						}
						else 
						{
							GetNode<Label>("Camera2D/CanvasLayer3/Label2").Text = "";
						}

						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt2").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt3").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt4").Visible = true;
						GetNode<Sprite2D>("Camera2D/CanvasLayer3/Sprite0005").Visible = true;
						GetNode<Label>("Camera2D/CanvasLayer3/slt/Label4").Text = menu.count[0].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt2/Label5").Text = menu.count[1].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt3/Label5").Text = menu.count[2].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt4/Label4").Text = menu.count[3].ToString();
						if (inv.inv[0] != null) 
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt/TextureRect").Texture = inv.inv[0].invicon;
						}
						else
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt/TextureRect").Texture = null;
						}
						if (inv.inv[1] != null) 
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt2/TextureRect").Texture = inv.inv[1].invicon;
						}
						else
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt2/TextureRect").Texture = null;
						}
						if (inv.inv[2] != null) 
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt3/TextureRect").Texture = inv.inv[2].invicon;
						}
						else
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt3/TextureRect").Texture = null;
						}
						if (inv.inv[3] != null) 
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt4/TextureRect").Texture = inv.inv[3].invicon;
						}
						else
						{
							GetNode<TextureRect>("Camera2D/CanvasLayer3/slt4/TextureRect").Texture = null;
						}
						
					}
					else if (current_menu[0] == 2)
					{
						GetNode<Control>("Camera2D/CanvasLayer3/Control").Visible = true;
						if (Input.IsActionJustPressed("X"))
						{
							GetNode<Control>("Camera2D/CanvasLayer3/Control").Visible = false;
							current_menu[0] = 0;
							pval = 1;
						}
					}
				}
				break;
		}

				
	}
			
		
}
