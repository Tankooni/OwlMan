extends MarginContainer

var scene_pip = preload("res://owlman_sprite.tscn")

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
			health_pips.remove_child(health_pips.get_child(current_health - _i - 1))
	else:
		for i in range(health - current_health):
			var pip = scene_pip.instance()
			pip.scale *= 0.6
			pip.offset.y -= 5
			pip.centered = false
			
			# Because hboxes don't work? (probably because this isn't a UI element. ho llew)
			pip.offset.x += (i + current_health) * 55
			pip.offset.x -= 20

			health_pips.add_child(pip)

func on_animation_changed(animation : String):
	for i in health_pips.get_child_count():
		var pip = health_pips.get_child(i)
		pip.play(animation)