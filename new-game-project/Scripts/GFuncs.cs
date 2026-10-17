using Godot;
using System;
using System.Threading.Tasks;

public partial class GFuncs : Node
{
	public async void Hitstop(double dr)
	{
		Engine.TimeScale = 0.0f;
		await ToSignal(GetTree().CreateTimer(dr, true, false, true), SceneTreeTimer.SignalName.Timeout);
		Engine.TimeScale = 1.0f;
	}
}
