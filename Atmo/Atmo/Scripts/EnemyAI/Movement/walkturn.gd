extends Node

signal on_collide(CollisionObject2D)

enum WalkingDirection {
	Left = 0,
	Right = 1
}

export(NodePath) onready var node = get_node(node)
export(WalkingDirection) var direction = WalkingDirection.Left
export(int) var speed = 200
export(int) var gravity = 18;
#export(NodePath) onready var node = get_node(node)

var vel_y = 0

func _ready():
	set_physics_process(true)
	
	# if the node we are bound to is a blank node, try to use it's parent
	if node.get_class() == "Node":
		node = get_parent()

func _physics_process(delta):
	var cur_direction = -1 if int(direction) == int(WalkingDirection.Left) else 1
	
	# update gravity
	vel_y += gravity
	
	# check if we would fall
	if node.test_move(node.transform, Vector2(0, vel_y)) and not node.test_move(node.transform, Vector2(speed * 5 * cur_direction, vel_y)):
		direction = !direction

	# do the move
	var vel = Vector2(speed, vel_y)
	if int(direction) == int(WalkingDirection.Left):
		vel.x *= -1

	var collision = node.move_and_slide(vel)

	# emit collision if there is one
	if node is CollisionObject2D:
		for i in (node as CollisionObject2D).get_slide_count():
			emit_signal("on_collide", node.get_slide_collision(i))
	
	# reverse direction
	if abs(collision.x) < speed:
		direction = !direction

	# clear gravity if needed
	if collision.y == 0:
		vel_y = 0
