using Godot;
using Godot.Collections;


[GlobalClass]
public partial class item_data : Godot.Resource
{
	[Export] public string Name = "";
	[Export] public double Ml = 5.1;
	[Export] public double Weight = 0.1;
}
