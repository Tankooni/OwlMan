extends Node

export(NodePath) onready var node
export(NodePath) onready var target

export(int) var attack_freq = 60
var frames_until_attack = 0

var animation_node

var boolette = preload("res:///prefab/Projectiles/Bullet.tscn")

export(String) var AttackSoundName

func _ready():
	set_physics_process(true)

	node = get_node(node)
	target = get_node(target)

	# Fix me later
	if has_node("AnimatedSprite"):
		animation_node = $AnimatedSprite
	elif $Worm:
		animation_node = $Worm

func _physics_process(_delta):
	return
	if animation_node:
		var direction = (node as Node2D).position.direction_to((target as Node2D).position)
		var distance = (node as Node2D).position.distance_to((target as Node2D).position)

		if direction.x < 0:
			animation_node.set_flip_h(false)
		elif direction.x > 0:
			animation_node.set_flip_h(true)

		# I was gonna do this somewhere else but gamejam time jam AHHHHHHH
		if frames_until_attack == 0 && distance <= 400:
			frames_until_attack = randi()%60 + attack_freq
			animation_node.play("attack")
			animation_node.connect("animation_finished", animation_node, "play", ["idle"], CONNECT_ONESHOT)

			Overlord.call("PlaySound", AttackSoundName, self.position)

			var bullet = boolette.instance()
			bullet.position = node.position
			bullet.direction = direction.normalized()
			get_tree().get_root().add_child(bullet)

		else:
			frames_until_attack = max(frames_until_attack - 1, 0)

func on_damage(damage):
	var _damage = damage
	self.queue_free()
