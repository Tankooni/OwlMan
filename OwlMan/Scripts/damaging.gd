extends Node

enum DamageType {
	Boolette,
	Swashh
}

enum Alignment {
	Player,
	Enemy
}

var isDeflected = false

#export(int) var damage = 1
#export(DamageType) var damage_type = DamageType.Boolette
#export(Alignment) var alignment = Alignment.Enemy
@export var direction: Vector2

func _ready():
	if direction:
		$Movement.direction = direction

func _physics_process(_delta):
	pass

func on_area_enter(collider):
	# Tell the other entity it collided and kill ourselves
	if not isDeflected:
		if not collider.is_in_group("enemy"):
			if collider.has_method("OnDamage"):
				collider.call("OnDamage", self)
			elif collider.has_method("on_damage"):
				collider.call("on_damage", self)
			self.queue_free()
	else:
		if collider.is_in_group("enemy"):
			if collider.has_method("OnDamage"):
				collider.call("OnDamage", self)
			elif collider.has_method("on_damage"):
				collider.call("on_damage", self)
		self.queue_free()

func deflected():
	if not isDeflected:
		direction = -direction
		$Movement.direction = direction
		$Movement.speed = $Movement.speed * 2
		isDeflected = true
		Overlord.call("PlaySound", "Hit4", self.position)
