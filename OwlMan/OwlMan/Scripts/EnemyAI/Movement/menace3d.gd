extends Node

signal on_collide(CollisionObject2D)

export(NodePath) onready var node
export(NodePath) onready var target

export(int) var speed = 500
export(int) var distance = 300

var activateRange = 1200

export(int) var attack_freq = 120
var frames_until_attack = 0

var animation_node

var boolette = preload("res://DamagingThings/boolette.tscn")

func _ready():
	set_physics_process(true)

	node = get_node(node)
	target = get_node(target)

	# Fix me later
	if has_node("AnimatedSprite"):
		animation_node = $AnimatedSprite

func _physics_process(delta):
	var distance_to = (node as Node2D).position.distance_to((target as Node2D).position)
	var direction = (node as Node2D).position.direction_to((target as Node2D).position) * distance

	var slerp_to = (node as Node2D).position.normalized().slerp((target as Node2D).position - direction, 0.1)

	# Try to stay distance away from target
	distance_to -= distance

	if distance_to <= activateRange:
		node.move_and_slide(slerp_to * speed/12000.0 * distance_to / delta)

		if frames_until_attack == 0 && distance <= 400:
			frames_until_attack = randi() % 60 + attack_freq
			animation_node.play("attack")
			animation_node.connect("animation_finished", animation_node, "play", ["idle"], CONNECT_ONESHOT)

			$BulletAudio.play(0)

			var bullet = boolette.instance()
			bullet.position = node.position
			bullet.direction = direction.normalized()
			get_tree().get_root().add_child(bullet)
		else:
			frames_until_attack = max(frames_until_attack - 1, 0)

	if $AnimatedSprite:
		if direction.x < 0:
			$AnimatedSprite.set_flip_h(false)
		elif direction.x >0:
			$AnimatedSprite.set_flip_h(true)


	#if collider:
	#	emit_signal("on_collide", collider.collider)
