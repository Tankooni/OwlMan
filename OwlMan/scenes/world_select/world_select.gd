extends Control

@onready var worlds: Array = [$WorldIcon, $WorldIcon2, $WorldIcon3, $WorldIcon4, $WorldIcon5]
var current_world: int = 0
# Called when the node enters the scene tree for the first time.

func _ready():
	$PlayerIcon.global_position = worlds[current_world].global_position


func _input(event):
	if event.is_action_pressed("left") and current_world > 0:
		current_world -= 1
		$PlayerIcon.global_position = worlds[current_world].global_positon
		print("Left pressed")
		
	if event.is_action_pressed("right") and current_world < worlds.size() - 1:
		current_world += 1
		$PlayerIcon.global_position = worlds[current_world].global_position
		print("Right Pressed")
