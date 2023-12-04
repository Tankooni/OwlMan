extends CanvasLayer

@export_file("*.tscn") var next_scene_path: String
@export var parameters: Dictionary

func _ready():
	ResourceLoader.load_threaded_request(next_scene_path)

func _process(delta):
	if ResourceLoader.load_threaded_get_status(next_scene_path) == ResourceLoader.THREAD_LOAD_LOADED:
		set_process(false)
		await get_tree().create_timer(1).timeout
		var new_scene: PackedScene = ResourceLoader.load_threaded_get(next_scene_path)
		var new_node = new_scene.instantiate()
		new_node.parameters = parameters
		var current_scene = get_tree().current_scene
		get_tree().get_root().add_child(new_node)
		get_tree().current_scene = new_node
		current_scene.queue_free()
