extends Node

signal on_collide(CollisionObject2D)

export(NodePath) onready var node
export(NodePath) onready var target

export(int) var attack_freq = 240
var frames_until_attack = attack_freq

var animation_node

var boolette = preload("res://DamagingThings/boolette.tscn")

func _ready():
	set_physics_process(true)
	
	node = get_node(node)
	target = get_node(target)
	
	# Fix me later
	if has_node("AnimatedSprite"):
		animation_node = $AnimatedSprite
	elif $Worm:
		animation_node = $Worm

func _physics_process(delta):
	if animation_node:
		var direction = (target as Node2D).position.direction_to((node as Node2D).position)
		
		if direction.x > 0:
			animation_node.set_flip_h(false)
		elif direction.x < 0:
			animation_node.set_flip_h(true)
			
		# I was gonna do this somewhere else but gamejam time jam AHHHHHHH
		if frames_until_attack <= 0:
			frames_until_attack = rand_range(0, 120) + attack_freq
			
			animation_node.play("attack")
			animation_node.connect("animation_finished", animation_node, "play", ["idle"], CONNECT_ONESHOT)

			var bullet = boolette.instance()
			bullet.direction = direction.normalized()
			
		else:
			frames_until_attack -= 1
