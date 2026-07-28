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
	public static float SPEED = 90.0f;

	public int Txtarrlen;
	public static Label Txtlbl;
	public static bool can_shoot = true;

	public static float blood_toxicity = 0.08f;

	//aparently 2d arrays are defined with a single comma in the brackets for some fkn reason
	//dont know why it couldent have just been a number
	//anyway DONT DELETE IT
	public static int[,] cam_clamp_var = {{0,0},{320,180}};
	[Export] public int[] min = [0,0];
	[Export] public int[] max = [320,180];
	public static CanvasLayer cv;

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

	public static Godot.Vector2 gpos;

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
	}

	public override void _PhysicsProcess(double _delta)
	{
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

				//yes the progress bar upgrade is snappy n shit but im too tired to even try fixing that rn
				GetNode<TextureProgressBar>("Camera2D/CanvasLayer2/PB").MaxValue = 100;
				GetNode<TextureProgressBar>("Camera2D/CanvasLayer2/PB").Value = charge_time * 100 / max_charge_time;
				
				if (can_shoot)
				{
					if (Input.IsActionPressed("Z"))
					{
						charge_time = Mathf.Min(charge_time + (float)_delta, max_charge_time);
					}

					if (Input.IsActionJustReleased("Z") && (charge_time > 0.0f)) 
					{
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

				//i just need to find out what dimensional plane lesbians are on and how to access it
				var vertical_direction = Input.GetAxis("ui_up", "ui_down");
				if (vertical_direction != 0)
				{
					GetNode<Node2D>("Area2D").Rotation = 0;
					Velocity = new Vector2(Velocity.X, vertical_direction * SPEED);
					
					if (vertical_direction == -1)
					{
						dir = 0;
					}
					else if (vertical_direction == 1)
					{
						dir = 1;
					}
				}
				else
				{
					Velocity = new Vector2(
						Velocity.X,
						Mathf.MoveToward(Velocity.X, 0, SPEED)
					);
				}

				var horizontal_direction = Input.GetAxis("ui_left", "ui_right");
				if (horizontal_direction != 0)
				{
					GetNode<Node2D>("Area2D").RotationDegrees = 90;
					Velocity = new Vector2(horizontal_direction * SPEED, Velocity.Y);
				
					if (horizontal_direction == -1)
					{
						dir = 2;
					}
					else if (horizontal_direction == 1)
					{
						dir = 3;
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
				//most nice looking C# code in the history of the world here
				Txtlbl.Text = Txtarr0[page];
				Txtarrlen = Txtlbl.GetTotalCharacterCount();
				Txtmth = 1.0f / Txtarrlen;
				GetNode<CanvasLayer>("Camera2D/CanvasLayer").Visible = true;
				if (!(Txtlbl.VisibleRatio >= 1))
				{
					Txtlbl.VisibleRatio += Txtmth / 2;
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
				//you just god deathed. dieed. died. yeah thats the one.
				//people say i cant take what i dish out.
				break;
		}

				
	}
			
		
}
