extends AnimatedSprite

# handles animation control based on state of entity

onready var node = get_parent()
var previous_pos = Vector2(0, 0)

func try_play(animation : String):
	if animation in self.frames.get_animation_names():
		self.play(animation)

func _ready():
	set_process(true)

func _process(delta):
	var vel = (previous_pos - node.position) * delta
	previous_pos = node.position

	# direction
	if vel.x > 0:
		self.set_flip_h(false)
	elif vel.x < 0:
		self.set_flip_h(true)
	
	# move animation
	if vel.x == 0:
		try_play("idle")
	else:
		try_play("run")

	# fall animation
	if vel.y > 0:
		try_play("fall")