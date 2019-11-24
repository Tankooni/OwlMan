extends MarginContainer

var scene_pip = preload("res://JulepSprite.tscn")

onready var health_pips : HBoxContainer = $StatPanel/Health/HealthPips

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func on_set_health(health : int):
	health = int(clamp(0, health, 3))
	print(health_pips)
	var current_health = health_pips.get_child_count()
	
	if current_health == health:
		return
	elif current_health > health:
		for _i in range(current_health - health):
			health_pips.remove_child(health_pips.get_child(0))
	else:
		for _i in range(health - current_health):
			var pip = scene_pip.instance()
			pip.scale *= 0.6
			pip.offset.y -= 10
			pip.centered = false
			health_pips.add_child(pip)