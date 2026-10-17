using Godot;
using System;

public partial class FPSLABEL : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		double e = Engine.GetFramesPerSecond();
		SetText(e.ToString());
	}
}
