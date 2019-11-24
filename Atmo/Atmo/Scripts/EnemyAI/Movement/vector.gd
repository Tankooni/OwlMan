extends Node

signal on_collide(CollisionObject2D)

export(Vector2) var direction = Vector2(1, 0)
export(int) var speed = 500

onready var node = self

func _ready():
	set_physics_process(true)

	# if the node we are bound to is a blank node, try to use it's parent
	if node.get_class() == "Node":
		node = get_parent()

func _physics_process(delta):
	var collider = node.move_and_collide(direction.abs() * speed * delta)

	if collider and collider.collider:
		emit_signal("on_collide", collider.collider)
		