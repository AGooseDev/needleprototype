using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;


[GlobalClass]
public partial class character_body_2d : CharacterBody2D
{
	//i dont like public and private, idk, idk why every var
	//cant just be public, if you dont want to use it in other classes
	//then just dont fucking use it, you can only access it by
	//typing the classname anyway, if anyone uses private i will actually
	//send a pipe bomb to their mailbox.
	public static float SPEED = 80.0f;
	public static bool[] IsDashing = {false,false};
	public static int pshflag = 0;
	public static bool ismoving = false;
	public static float hitr = 0.35f;
	//fk you
	public int Txtarrlen;
	public static Godot.Vector2 kbv = new Godot.Vector2(0,0);
	public static bool[] FLAGS = [false,false,false,false,false,false,false,false];

	//fk you
	public static Label Txtlbl;
	public static bool can_shoot = true;
	public static bool zpr = false;
	public static float blood_toxicity = 0.08f;
	public static float heartrate = 74;
	public static float bloodpressure = 100.0f;
	public static float physicalhealth = 99.999f;
	public static float head_integrity = 100.00f;
	public static float sanity = 100.0f;
	public static int[,] cam_clamp_var = {{0,0},{320,180}};
	[Export] public int[] min = [0,0];
	[Export] public int[] max = [320,180];
	public static CanvasLayer cv;
	public static int effect = -1;
	public static int previouseffect = -1;
	public static int adrenalinetimer = 10*60;
	public static int COMTtimer = 10*60;
	public static float charge_time = 0f;
	public static float max_charge_time = 1.4f;
	public static float min_needle_speed = 10f;
	public static float max_needle_speed = 400f;
	public static int page = 0;
	public static int dir = 0;
	public static character_body_2d Instance;
	
	//FK YOU
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


	//static count x99999

	


	
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

//ok but why?
private void DeferredSceneChange()
{
    GetTree().ChangeSceneToFile("res://Scenes/fallen.tscn");
}
	public override void _Ready()
	{
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		GetNode<Timer>("DASHTIMER").Timeout += OnDashTimeout;
		//GetNode<CollisionShape2D>("CollisionShape2D").SafeMargin
		state = state_override;
		Instance = this;	
		cv = GetNode<CanvasLayer>("Camera2D/CanvasLayer");
		cm = GetNode<Camera2D>("Camera2D");
		//cm.MakeCurrent();
		Txtlbl = GetNode<Label>("Camera2D/CanvasLayer/Control/Label");
		Txtarrlen = Txtlbl.GetTotalCharacterCount();
		Txtmth = Txtlbl.VisibleRatio / Txtarrlen;

		Txtlbl.VisibleRatio = 0;
		Pickup(GD.Load<inventory_item>("res://tres/adrenaline_needle.tres"));
		//FLAGS[0] = true;
	}

	public void OnTimerTimeout()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(1,1,1);
		Velocity = new Godot.Vector2(0,0);
		if (state == 4) state = 0;
	}
	public void OnDashTimeout()
	{
		Velocity = new Godot.Vector2(0,0);
		IsDashing[0] = false;
		if (!IsDashing[0])
		{
			if (dir == 0)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleup");
			}
			else if (dir == 1)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idledown");
			}
			else if (dir == 2)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleleft");
			}
			else if (dir == 3)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleright");
			}
		}
		GetNode<Timer>("DASHTIMER").Stop();
	}

	public async void Hitstop(double dr)
	{
		Engine.TimeScale = 0.0f;
		await ToSignal(GetTree().CreateTimer(dr, true, false, true), SceneTreeTimer.SignalName.Timeout);
		Engine.TimeScale = 1.0f;
	}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionReleased("esc"))
		{
			if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
			}
			else
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			}
		}
    }

	public override void _PhysicsProcess(double _delta)
	{
		if (physicalhealth < 78)
		{
			SPEED = 70;
		}
		if (physicalhealth <= 0)
		{
			DeferredSceneChange(); //OK BUT WHY DI I NEED TO CHANGE THE SCENE IN A DIFFERENT METHOD/FUNCTION AAAAAAAAAAAAAAA
			return;
		}
		if (bloodpressure > 136)
		{
			
			physicalhealth -= 0.005f;
		}
		
		if (bloodpressure > 180)
		{
			sanity -= 0.05f;
			physicalhealth -= 0.05f;
		}
		else if (bloodpressure > 143)
		{
			Random r = new Random();
			SPEED = r.Next(60,90);
			sanity -= 0.008f;
		}
		if (bloodpressure < 60 && bloodpressure !< 50)
		{
			head_integrity -= 0.004f;
			sanity -= 0.01f;
			physicalhealth -= 0.025f;
			SPEED = 60;
		}
		else if (bloodpressure < 50)
		{
			head_integrity -= 0.008f;
			sanity -= 0.02f;
			physicalhealth -= 0.05f;
			SPEED = 40;
		}
		else if (bloodpressure < 30)
		{
			head_integrity -= 0.1f;
			sanity -= 0.1f;
			physicalhealth -= 0.1f;
			SPEED = 15;
		}

		if (heartrate > 130)
		{
			bloodpressure += 0.01f;
		}

		if (heartrate < 50)
		{
			bloodpressure -= 0.05f;
		}

		if (sanity < 60)
		{
			heartrate += 0.01f;
			bloodpressure += 0.005f;
			//physicalhealth -= 0.05f;
		}
		if (sanity < 30)
		{
			heartrate += 0.01f;
			bloodpressure += 0.005f;
			physicalhealth -= 0.05f;
		}

		if (blood_toxicity > 80)
		{
			sanity -= 0.01f;
			bloodpressure += 0.04f;
			physicalhealth -= 0.2f;
			blood_toxicity += 0.01f;
			head_integrity -= 0.1f;
		}
		else if (blood_toxicity > 50)
		{
			sanity -= 0.008f;
			bloodpressure += 0.01f;
			physicalhealth -= 0.1f;
			blood_toxicity += 0.001f;
			head_integrity -= 0.01f;
		}

		if (head_integrity < 50)
		{
			sanity -= 0.01f;
		}

		

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

		gpos = this.GlobalPosition;

		switch (state)
		{
			//yes, i did not use enums to name the states.  in hindsight i should have wrote the enums first
			//but like technically cant i jsut use a #define or something? like is there any memory benefit to using enums orrrrr.
			//idk ima google it
			case -1:
				//needle stab!
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("NEEDLESTAB");
				if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").Frame == 9)
				{
					
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
					FLAGS[0] = true;
					state = 0;
				}
				break;
			case 0:
				
				switch (effect)
				{
					case -2:
						break;

					case -1:
						GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Visible = false;
						GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect").Visible = false;
						if (head_integrity <= 45.00f) sanity -= 0.0002f;

						if (previouseffect == 3) {}
						break;

					case 0:
						GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Visible = true;
						GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect").Visible = true;
						if (adrenalinetimer >= 0)
						{
							adrenalinetimer--;
							head_integrity -= 0.01f;
							bloodpressure += 0.01f;
							heartrate += 0.06f;
							physicalhealth -= 0.01f;
							SPEED = 130;
							//heart rate changes should deteriorate physical health and bp
							//un-needed adrenaline affects HR, BP, toxicity i think and brain/physical health for hrs
							//COMT breaks down adrenaline
						}
						else
						{
							SPEED = 80;
							effect = -1;
							adrenalinetimer = 10*60;
							
						}
						break;
					
					case 2:
						if (physicalhealth < 99) physicalhealth++;
						break;

					case 3:
						if (COMTtimer >= 0)
						{
							GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect2").Visible = true;
							if (previouseffect == 0)
							{
								COMTtimer--;
								GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Modulate -= new Color(0,0,0,0.01f);
								if (head_integrity <= 100.00f) head_integrity += 0.006f;
								if (sanity <= 100.00f) sanity += 0.0065f;
								bloodpressure -= 0.008f;
								heartrate -= 0.006f;
							}
							else if (previouseffect == -1)
							{
								COMTtimer--;
								head_integrity -= 0.006f;
								sanity -= 0.004f;
								bloodpressure -= 0.01f;
								heartrate -= 0.008f;
							}
						}
						else
						{
							GetNode<Sprite2D>("Camera2D/DRUGEFFECTS/NewNoiseTexture2d").Modulate = new Color(1,1,1,1f);
							GetNode<ColorRect>("Camera2D/DRUGEFFECTS/ColorRect2").Visible = false;
							effect = -1;
							COMTtimer = 10*60;
							
						}
						
						break;
				}

				GetNode<CanvasLayer>("Camera2D/CanvasLayer3").Visible = false;

				if (Input.IsActionPressed("C"))
				{
					var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
					GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
					GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
					state = 3;
					break;
				}

				

				if (Input.IsActionPressed("shift"))
				{
					pshflag = 1;
				}
				if (Input.IsActionJustReleased("X"))
				{
					IsDashing[1] = false;
				}
				if (Input.IsActionPressed("X") && !IsDashing[1] && IsDashing[0] == false)
				{
					GetNode<Timer>("DASHTIMER").Start();
					IsDashing[0] = true;
					IsDashing[1] = true;
				}
				
				if (IsDashing[0])
				{
					if (dir == 0)
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("du");
						if (!GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").IsPlaying())
						{
							GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").GlobalPosition = GlobalPosition;
						}
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").Play("new_animation");
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").RotationDegrees = 270;
						Velocity = new Godot.Vector2(0,-1*SPEED * 5.6f);
					}
					else if (dir == 1)
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dd");
						if (!GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").IsPlaying())
						{
							GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").GlobalPosition = GlobalPosition;
						}
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").Play("new_animation");
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").RotationDegrees = 90;
						Velocity = new Godot.Vector2(0,1*SPEED * 5.6f);
					}
					else if (dir == 2)
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dl");
						if (!GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").IsPlaying())
						{
							GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").GlobalPosition = GlobalPosition;
						}
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").Play("new_animation");
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").RotationDegrees = 180;
						Velocity = new Godot.Vector2(-1*SPEED * 5.6f,0);
					}
					else if (dir == 3)
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dr");
						if (!GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").IsPlaying())
						{
							GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").GlobalPosition = GlobalPosition;
						}
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").Play("new_animation");
						GetNode<AnimatedSprite2D>("Node/AnimatedSprite2D2").RotationDegrees = 0;
						Velocity = new Godot.Vector2(1*SPEED * 5.6f,0);
					}
				}
				else if (!Input.IsActionPressed("shift"))
				{
					pshflag = 0;
				}

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

						if (FLAGS[0])
						{
							previouseffect = effect;
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
							FLAGS[0] = false;
						}

						if (Input.IsActionJustPressed("S"))
						{
							state = -1;
						}

						if (Input.IsActionJustReleased("Z") && (charge_time > 0.0f)) 
						{
							var scene = GD.Load<PackedScene>("res://Scenes/shoot_needle_adrenaline.tscn");
							var instance = scene.Instantiate<CharacterBody2D>();

							instance.GlobalPosition = GlobalPosition;

							float charge_ratio = charge_time / max_charge_time;
							float needle_speed = Mathf.Lerp(min_needle_speed,max_needle_speed,charge_ratio);

							instance.Set("spd", needle_speed);
							instance.Set("effect", inv.inv[equipped].pid);

							GetTree().CurrentScene.AddChild(instance);
							if ((menu.count[equipped]-1) <= 0)
							{
								inv.inv[equipped] = null;
								menu.count[equipped] = 0;
							}
							else if (menu.count[equipped] > 0)
							{
								menu.count[equipped]--;
							}
							

							charge_time = 0f;
						}
					}
				}
				if (IsDashing[0] == false)
				{
					if (Input.IsActionJustReleased("ui_down"))
					{
						ismoving = false;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idledown");
					}
					if (Input.IsActionJustReleased("ui_up"))
					{
						ismoving = false;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleup");
					}
					if (Input.IsActionJustReleased("ui_left"))
					{
						ismoving = false;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleleft");
					}
					if (Input.IsActionJustReleased("ui_right"))
					{
						ismoving = false;
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idleright");
					}
					var horizontal_direction = Input.GetAxis("ui_left", "ui_right");
					//i just need to find out what dimensional plane lesbians are on and how to access it
					
					var vertical_direction = Input.GetAxis("ui_up", "ui_down");
					if (vertical_direction != 0)
					{
						ismoving = true;
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
							Mathf.MoveToward(Velocity.Y, 0, SPEED)
						);
						
						
					}



					if (horizontal_direction != 0)
					{
						ismoving = true;
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
							Mathf.MoveToward(Velocity.X, 0, SPEED),
							Velocity.Y
						);
					}
				}

				MoveAndSlide();

				if (Input.IsActionJustPressed("Debug"))
				{
					{
						//var scene = ResourceLoader.Load<PackedScene>("res://Scenes/fallen.tscn").Instantiate();
						//GetTree().Root.AddChild(scene);
					}
					DeferredSceneChange(); //again no damn clue why i had to do this
				}

				break;

			case 1:
				if (Input.IsActionJustReleased("Z"))
				{
					zpr = false;
				}
				//haha bye bye naming conventions
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
						else if (Shoptest.exist == true && !(page + 1 < Txtarr0.Count))
						{
							state = 10;
							Shoptest.menu = [2,0];
							page = 0; 
							GetNode<CanvasLayer>("Camera2D/CanvasLayer").Visible = false;
						}
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
							IsDashing[1] = true;
							state = 0;
						}

						pmax = 2;
						if (Input.IsActionJustPressed("ui_up"))
						{
							pval--;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}
						else if (Input.IsActionJustPressed("ui_down"))
						{
							pval++;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}
						if (pval < 0) pval = pmax;
						else if (pval > pmax) pval = 0;
						GetNode<Label>("Camera2D/CanvasLayer3/Label2").Text = "";

						//spaghetti code
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
						if (Input.IsActionJustPressed("ui_up"))
						{
							pval--;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}
						else if (Input.IsActionJustPressed("ui_down"))
						{
							pval++;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}

						if (pval < 0) pval = pmax;
						else if (pval > pmax) pval = 0;
						pmax = invcount - 1;
						px = 61+(36*pval);
						py = 14;
						
						if (Input.IsActionJustPressed("C"))
						{
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
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
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/blipSelect.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
							//make the current selected slot to be equipped
							equipped = pval;
						}
						if (Input.IsActionJustReleased("Z"))
						{
							zpr = false;
						}
						if (Input.IsActionJustPressed("X"))
						{
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
							current_menu[0] = 0;
							pval = 0;
						}

						if (Input.IsActionJustPressed("ui_left"))
						{
							pval--;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}
						else if (Input.IsActionJustPressed("ui_right"))
						{
							pval++;
							var st = GD.Load<AudioStreamWav>("res://Assets/SND/click.wav");
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream = st;
							GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
						}

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

						//spaghetti
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt2").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt3").Visible = true;
						GetNode<NinePatchRect>("Camera2D/CanvasLayer3/slt4").Visible = true;
						GetNode<Sprite2D>("Camera2D/CanvasLayer3/Sprite0005").Visible = true;
						GetNode<Label>("Camera2D/CanvasLayer3/slt/Label4").Text = menu.count[0].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt2/Label5").Text = menu.count[1].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt3/Label5").Text = menu.count[2].ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/slt4/Label4").Text = menu.count[3].ToString();

						//mr spaghetti sphagetto
						//could i have used a for loop here?... yes
						//is it infinately better to use a forloop here?... also yes
						//am i a lazy sometimes and cant bother to rewrite something simpler?.....
						//               maybe.
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
						//sphagegeghetti
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label2").Text = blood_toxicity.ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label3").Text = bloodpressure.ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label4").Text = heartrate.ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label5").Text = physicalhealth.ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label6").Text = head_integrity.ToString();
						GetNode<Label>("Camera2D/CanvasLayer3/Control/Label7").Text = sanity.ToString();
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

			case 4:
				if ((Time.GetTicksMsec()/60) % 2 == 0 && Engine.TimeScale == 1.0f) 
				//no idea what the percentage symbol does here i just googled it cause my og condition didnt work
				//like ive litterelly never used this for some reason and i still dont know what it does

				//addendum: i now know what it does!
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(0.61f,0.66f,1);
					}
					else
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(1,1,1);
					}
					//GetNode<Timer>("Timer").Stop();
					Velocity = kbv;
					physicalhealth -= hitr;
					MoveAndSlide();
				break;
		}  		
	}		
}