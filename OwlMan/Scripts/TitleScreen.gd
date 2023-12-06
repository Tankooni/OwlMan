extends Control

@export var level_select_packed: PackedScene = load("res://scenes/world_select/world_select.tscn")


func _on_start_button_pressed():
	print("Start Button Pressed!")
	var parameters: Dictionary = {"test": "f"}
	Functions.load_screen_to_scene("res://scenes/world_select/world_select.tscn", parameters)


func _on_quit_button_pressed():
	get_tree().quit()


func _on_options_button_pressed():
	print("Options Button Pressed")
