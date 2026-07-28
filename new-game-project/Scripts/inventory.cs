using Godot;
using System;

[GlobalClass]
public partial class inventory : Resource
{
    [Export] public Godot.Collections.Array<inventory_item> inv = new();
}
