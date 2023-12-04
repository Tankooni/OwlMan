extends Control

@onready var worlds: Array = [$WorldIcon, $WorldIcon2, $WorldIcon3, $WorldIcon4, $WorldIcon5]
@onready var current_level: String
var current_world: int = 0
#var functions = preload("res://Scripts/functions.gd").new()
@export var level_select_packed: PackedScene = load("res://scenes/GameScene.tscn")
#@onready var level_select_scene: LevelSelect = level_select_packed.instantiate()

# Function to get the value at a specific index
func getValueAtIndex(index: int) -> Control:
	if index >= 0 and index < worlds.size():
		return worlds[index]
	else:
		# Handle an out-of-bounds index or return a default value
		print("Index out of bounds!")
		return null  # You can choose a different default value or handle the error accordingly

# Called when the node enters the scene tree for the first time.
func _ready():
	$PlayerIcon.global_position = worlds[current_world].global_position
	
func _input(event):
	if event.is_action_pressed("left") and current_world > 0:
		current_world -= 1
		$PlayerIcon.global_position = worlds[current_world].global_position
		print($PlayerIcon.global_position)
		print("Left pressed")
		
	if event.is_action_pressed("right") and current_world < worlds.size() - 1:
		current_world += 1
		$PlayerIcon.global_position = worlds[current_world].global_position
		print($PlayerIcon.global_position)
		print("Right Pressed")
		
	if event.is_action_pressed("start"):
		print("Enter Pressed")
		Functions.load_screen_to_scene("res://scenes/GameScene.tscn", {"test": "f"})
#		var world_object = getValueAtIndex(current_world)
#		if world_object:
#			functions.load_screen_to_scene("res://scenes/loading.tscn")
