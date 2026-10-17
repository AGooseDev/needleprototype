using Godot;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class TestEnemy : CharacterBody2D
{
	public int state = 0;
	public float speed = 60;
	public float hp = 40f;
	public float s = 140;
	public bool hf = false;
	public int dir = 0;
	public NavigationAgent2D nav;

	public Godot.Vector2 kbv = new Godot.Vector2(0,0);
    
    public override void _Ready()
    {
		nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
		nav.VelocityComputed += OnNavigationAgent2DVelocityComputed;
    	nav.AvoidanceEnabled = true;
        GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
		GetNode<Timer>("Timer2").Timeout += OnTtwo;
		GetNode<Timer>("Timer3").Timeout += OnTthree;
		GetNode<Timer>("Timer4").Timeout += OnT4;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").AnimationFinished += OnAnimationFinished;
		
    }
	Random r = new Random();
	public void OnTimerTimeout()
	{
		if (r.Next(-1,10) < 5)
		{
			Velocity = new Godot.Vector2(r.Next(-1,2)*speed,r.Next(-1,2)*speed);
		}
		else
		{
			if (state == 0) state = 1;
		}
	}

	public void OnTtwo()
	{
		GetNode<Timer>("Timer").Stop();
		state = 2;
	}
	public void OnT4()
	{
			GetNode<Timer>("Timer").Stop();
			state = 2;
			Velocity = Godot.Vector2.Zero;
			nav.Velocity = Godot.Vector2.Zero;
		
	}
	public void OnTthree()
	{
		if (!hf) {hf = true;}
		else if (hf) {hf = false;}
	}
	public void OnAnimationFinished()
	{
		if (state == 1)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
			state = 0;
		}
	}
	public async void Hitstop(double dr)
	{
		Engine.TimeScale = 0.0f;
		await ToSignal(GetTree().CreateTimer(dr, true, false, true), SceneTreeTimer.SignalName.Timeout);
		Engine.TimeScale = 1.0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		var e = GetNode<CharacterBody2D>("../Player");
		if (GlobalPosition.Y < e.GlobalPosition.Y)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").ZIndex = e.ZIndex - 1;
		}
		else
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").ZIndex = e.ZIndex + 1;
		}
		//OK LETS DO THIS!!!!!
		
		switch (state)
		{
			case -2:
				//buzz anim.!
				break;

			case -1:
				//ded
				break;

			case 0:
				if (GetNode<Timer>("Timer").IsStopped()) GetNode<Timer>("Timer").Start();
				GetNode<RayCast2D>("RayCast2D").RotationDegrees += 30f;
				if (GetNode<RayCast2D>("RayCast2D").IsColliding())
				{
					var c = GetNode<RayCast2D>("RayCast2D").GetCollider();
					if (c is character_body_2d)
					{
						//GD.Print("KILL");
						state = 2;
					}
				}
				GetNode<ShapeCast2D>("RayCast2D2").RotationDegrees += 11f;
				if (GetNode<ShapeCast2D>("RayCast2D2").IsColliding())
				{
					var c = GetNode<ShapeCast2D>("RayCast2D2").GetCollider(0);
					if (c is shoot_needle_adrenaline needle)
					{
						//GD.Print("KILL");
						if (needle.RotationDegrees == 180)
						{
							kbv = new Godot.Vector2(-1*s,0);
						}
						else if (needle.RotationDegrees == 0)
						{
							kbv = new Godot.Vector2(1*s,0);
						}
						else if (needle.RotationDegrees == 270)
						{
							kbv = new Godot.Vector2(0,-1*s);
						}
						else if (needle.RotationDegrees == 90)
						{
							kbv = new Godot.Vector2(0,1*s);
						}
						needle.QueueFree();
						Hitstop(0.15);

						state = 4;
					}
				}
				break;

			case 1:
				
				Velocity = new Godot.Vector2(0,0);
				
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("irritation");
				
				
				
				break;
			
			case 2:
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(1,1,1);
				GetNode<Timer>("Timer").Stop();
				//GetNode<RayCast2D>("RayCast2D").RotationDegrees += 30f;
				//nav = GetNode<NavigationAgent2D>("NavigationAgent2D");
				Godot.Vector2 pos = GlobalTransform.Origin;
				UpdateTargetPosition(character_body_2d.gpos, nav);
				Godot.Vector2 nvnp = nav.GetNextPathPosition();
				Godot.Vector2 direction = pos.DirectionTo(nvnp);
				nav.MaxSpeed = speed;
				nav.Velocity = direction * speed;
				//OnNavigationAgent2DVelocityComputed(Godot.Vector2 safeVelocity);
        //Velocity = Velocity.MoveToward(safeVelocity, 12.0f);
				
				
				GetNode<ShapeCast2D>("RayCast2D2").RotationDegrees += 21f;
				if (GetNode<ShapeCast2D>("RayCast2D2").IsColliding())
				{
					var c = GetNode<ShapeCast2D>("RayCast2D2").GetCollider(0);
					if (c is shoot_needle_adrenaline needle)
					{
						//GD.Print("KILL");
						if (needle.RotationDegrees == 180)
						{
							kbv = new Godot.Vector2(-1*s,0);
						}
						else if (needle.RotationDegrees == 0)
						{
							kbv = new Godot.Vector2(1*s,0);
						}
						else if (needle.RotationDegrees == 270)
						{
							kbv = new Godot.Vector2(0,-1*s);
						}
						else if (needle.RotationDegrees == 90)
						{
							kbv = new Godot.Vector2(0,1*s);
						}
						needle.QueueFree();
						Hitstop(0.15);

						state = 4;
					}
				}
				if (GetNode<RayCast2D>("RayCast2D3").IsColliding())
				{
					
					var c = GetNode<RayCast2D>("RayCast2D3").GetCollider();
					if (c is character_body_2d chr && character_body_2d.state == 0)
					{

						Hitstop(0.2);
						var sst = GD.Load<AudioStreamWav>("res://Assets/SND/hitHurt (2).wav");
						GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Stream = sst;
						GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
						if (dir == 2)
						{
							character_body_2d.kbv = new Godot.Vector2(-1*(character_body_2d.SPEED + 40),0);
							GetNode<Timer>("Timer4").Start();
							state = 6;
							kbv = new Godot.Vector2(1*(character_body_2d.SPEED + 40),0);
						}
						else if (dir == 3)
						{
							character_body_2d.kbv = new Godot.Vector2(1*(character_body_2d.SPEED + 40),0);
							GetNode<Timer>("Timer4").Start();
							state = 6;
							kbv = new Godot.Vector2(-1*(character_body_2d.SPEED + 40),0);
						}
						else if (dir == 0)
						{
							character_body_2d.kbv = new Godot.Vector2(0,-1*(character_body_2d.SPEED + 40));
							GetNode<Timer>("Timer4").Start();
							state = 6;
							kbv = new Godot.Vector2(0,1*(character_body_2d.SPEED + 40));
						}
						else if (dir == 1)
						{
							character_body_2d.kbv = new Godot.Vector2(0,1*(character_body_2d.SPEED + 40));
							state = 6;
							kbv = new Godot.Vector2(0,-1*(character_body_2d.SPEED + 40));
						}
						character_body_2d.state = 4;
						chr.Velocity = new Godot.Vector2(0,0);
						chr.GetNode<Timer>("Timer").Start();
						player_cam.random_strength = 0.52f;
						player_cam.ApplyShake();
					}
				}
				//AAAAAAAAAAAAAAAAAAAAAA
				GetNode<RayCast2D>("RayCast2D3").RotationDegrees = Mathf.PosMod(Mathf.RadToDeg(character_body_2d.gpos.AngleToPoint(GlobalPosition)),360) + 90;
				if (GetNode<RayCast2D>("RayCast2D3").RotationDegrees > 315 || GetNode<RayCast2D>("RayCast2D3").RotationDegrees <= 45)
				{
					dir = 1;
				}
				if (GetNode<RayCast2D>("RayCast2D3").RotationDegrees > 45 && GetNode<RayCast2D>("RayCast2D3").RotationDegrees <= 135)
				{
					dir = 2;
				}
				if (GetNode<RayCast2D>("RayCast2D3").RotationDegrees > 135 && GetNode<RayCast2D>("RayCast2D3").RotationDegrees <= 230)
				{
					dir = 0;
				}
				if (GetNode<RayCast2D>("RayCast2D3").RotationDegrees > 230 && GetNode<RayCast2D>("RayCast2D3").RotationDegrees <= 315)
				{
					dir = 3;
				}
				//attack
				break;

			case 3:
				//needleshoot.
				break;

			case 4:
				var st = GD.Load<AudioStreamWav>("res://Assets/SND/hitHurt (2).wav");
				GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Stream = st;
				GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
				GetNode<Timer>("Timer3").Start();
				//GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(156,169,255);
				//GetNode<Timer>("Timer").Stop();
				Velocity = new Godot.Vector2(0,0);
				if (GetNode<Timer>("Timer2").IsStopped()) GetNode<Timer>("Timer2").Start();
				GetNode<Timer>("Timer2").Start();
				
				state = 5;
				player_cam.random_strength = 0.4f;
				player_cam.ApplyShake();
				
				//GetNode<Timer>("Timer3").Start();
				//PREHIT
				break;
			case 5:
			
			//if ((Time.GetTicksMsec()/60)% 2 == 0) //the hell?
				//if (GetNode<Timer>("Timer3").IsStopped()) GetNode<Timer>("Timer3").Start();
					if ((Time.GetTicksMsec()/60) % 2 == 0 && Engine.TimeScale == 1.0f)
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D2").Play("new_animation");
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(0.61f,0.66f,1);
					}
					else
					{
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Modulate = new Color(1,1,1);
					}
					//GetNode<Timer>("Timer").Stop();
					Velocity = kbv;
					//HIT
				break;

			case 6: 
				if (GetNode<Timer>("Timer4").IsStopped()) GetNode<Timer>("Timer4").Start();
				Velocity = kbv;
				break;
		}

		MoveAndSlide();
	}
	private void UpdateTargetPosition(Godot.Vector2 target, NavigationAgent2D nav)
	{
		nav.TargetPosition = target;
	}
	private void OnNavigationAgent2DVelocityComputed(Godot.Vector2 safeVelocity)
    {
        if (state == 2) Velocity = Velocity.MoveToward(safeVelocity, 12.0f);
		//MoveAndSlide();
    }
}