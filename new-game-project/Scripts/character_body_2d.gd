extends CharacterBody2D;

const SPEED = 160.0;

@onready var txtarrlen = $Camera2D/CanvasLayer/Control/Label.get_total_character_count();
@onready var txtlbl = $Camera2D/CanvasLayer/Control/Label;
@onready var txtmth = txtlbl.visible_ratio / txtarrlen;

func _ready() -> void:
	txtlbl.visible_ratio = 0;

func _physics_process(_delta: float) -> void:
	if (!(txtlbl.visible_ratio >= 1)):
		txtlbl.visible_ratio += txtmth / 2;
	else:
		txtlbl.visible_ratio = 1;

	var vertical_direction := Input.get_axis("ui_up", "ui_down");
	if vertical_direction:
		$Area2D.rotation = 0;
		velocity.y = vertical_direction * SPEED;
	else:
		velocity.y = move_toward(velocity.y, 0, SPEED);

	var horizontal_direction := Input.get_axis("ui_left", "ui_right");
	if horizontal_direction:
		$Area2D.rotation_degrees = 90;
		velocity.x = horizontal_direction * SPEED;
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED);

	move_and_slide();
