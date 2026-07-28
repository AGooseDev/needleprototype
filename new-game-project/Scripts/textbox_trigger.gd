extends Area2D

@export var sig = 0;
@export var text = "";

func _ready() -> void:
	pass;


#OK SO... when i first typed this out, i was stuck for 12 hours
#trying to figure oiut why the debug message would not print out...
#i was looking at the wrong fkn debug window.
func _process(delta: float) -> void:
	for i in get_overlapping_areas():
		if i.get_parent().name == "Player":
			print("islugfzw");
