extends Node

enum DamageType {
	Boolette,
	Swashh
}

enum Alignment {
	Player,
	Enemy
}

export(bool) var apply_to_parent = false

#export(int) var damage = 1
#export(DamageType) var damage_type = DamageType.Boolette
#export(Alignment) var alignment = Alignment.Enemy

var node

export(Vector2) var direction

func _ready():
	if apply_to_parent:
		node = get_parent()
	else:
		node = self
		
	if direction:
		$Movement.direction = direction

func _physics_process(delta):
	pass

func on_area_enter(collider):
	# Tell the other entity it collided and kill ourselves
	if not collider.is_in_group("enemy"):
		if collider.has_method("OnDamage"):
			collider.call("OnDamage", self)
		elif collider.has_method("on_damage"):
			collider.call("on_damage", self)
		self.queue_free()