extends Control


## Called when the node enters the scene tree for the first time.
#func _ready():
#	pass # Replace with function body.
#
#
## Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_start_button_pressed():
	Functions.load_screen_to_scene("res://scenes/world_select.tscn", {"test": "f"})
