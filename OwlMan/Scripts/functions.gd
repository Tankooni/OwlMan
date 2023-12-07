extends Node

var dialogue_file : String = "res://Dialogue/Test/dialogue_file.json"
var json_data : String

## Opens a loading screen to load the target scene from the resource path.
## target: The target scene resource path
## parameters: An optional set of parameters to pass to the new scene. Keep in mind the target scene needs to understand the parameters for them to work.
func load_screen_to_scene(target: String, parameters: Dictionary = {}) -> void:
	var loading_screen = preload("res://scenes/loading.tscn").instantiate() # Godot handles loading the loading screen upon game startup. We'll instance a new one here.
	loading_screen.next_scene_path = target # Assign the next scene to the loader so it knows what to load
	loading_screen.parameters = parameters # And the parameters to pass along
	get_tree().current_scene.add_child(loading_screen) # Add the loading screen to the current scene. Note that this does not replace the current scene, so if you want the current scene to stop doing things, you'll need to pause it.
	
func load_dialogue() -> Dictionary:
	var file = FileAccess.open(dialogue_file, FileAccess.READ)
	# Open the file for reading
	if file == OK:
		# Read the file content as a JSON string
		var file_content = file.get_as_text()
		
		# Parse the JSON string into a Dictionary
		var json_data = JSON.parse_string(file_content)
		json_data.open_buffer(file_content)
		
		if json_data.read() == OK:
			return json_data.get_node()
		else:
			print("Error: Failed to parse JSON.")
	else:
		print("Error: Failed to open file for reading.")
	
	return {}  # or handle the error in another appropriate way
