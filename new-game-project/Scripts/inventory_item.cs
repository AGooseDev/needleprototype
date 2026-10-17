using Godot;
using System;

[GlobalClass]
public partial class inventory_item : Resource
{
	[Export] public ITEM_ID id;
	[Export] public Texture2D invicon;
    [Export] public bool is_needle;
    [Export] public float cc;
	[Export] public string desc = "";
	[Export] public string path = "";
	[Export] public int pid;
}

public enum ITEM_ID
{
	adrenaline_needle,
	COMT,

	NitricOxide,

	Risperidone,
	Kenadine,
	COMAD
}
