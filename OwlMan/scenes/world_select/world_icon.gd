@tool
extends Control

@export var world_index: int = 1
@export_file("*.tscn") var next_scene_path: String
# Called when the node enters the scene tree for the first time.
func _ready():
	$Label.text = "World " + str(world_index)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Engine.is_editor_hint():
		$Label.text = "World " + str(world_index)
