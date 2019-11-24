extends Node

signal on_collide(CollisionObject2D)

export(NodePath) onready var node
export(NodePath) onready var target

export(int) var speed = 500
export(int) var distance = 300

func _ready():
	set_physics_process(true)
	
	node = get_node(node)
	target = get_node(target)

func _physics_process(delta):
	var distance_to = (target as Node2D).position.distance_to((node as Node2D).position)
	var slerp_to = (target as Node2D).position.normalized().slerp((node as Node2D).position.normalized(), 0.1)
	var direction = (target as Node2D).position.direction_to((node as Node2D).position)
	
	# Try to stay distance away from target
	distance_to -= distance
	 
	node.move_and_slide(slerp_to * speed/12000.0 * distance_to / delta)

	if $AnimatedSprite:
		if direction.x > 0:
			$AnimatedSprite.set_flip_h(false)
		elif direction.x < 0:
			$AnimatedSprite.set_flip_h(true)

	#if collider:
	#	emit_signal("on_collide", collider.collider)
		