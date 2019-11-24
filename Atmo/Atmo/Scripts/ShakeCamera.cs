using Godot;
using System;

public class ShakeCamera : Camera2D
{
	float _duration = 0.0f;
	float _period_in_ms = 0.0f;
	float _amplitude = 0.0f;
	float _timer = 0.0f;
	float _last_shook_timer = 0;
	float _previous_x = 0.0f;
	float _previous_y = 0.0f;
	Vector2 _last_offset = new Vector2(0, 0);

	RandomNumberGenerator rand = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		//SetProcess(true);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//Only shake when there's shake time remaining.
		if (_timer == 0)
			return;
		//Only shake on certain frames.
		_last_shook_timer = _last_shook_timer + delta;
		//Be mathematically correct in the face of lag; usually only happens once.
		while (_last_shook_timer >= _period_in_ms)
		{
			_last_shook_timer = _last_shook_timer - _period_in_ms;

			//Lerp between [amplitude] and 0.0 intensity based on remaining shake time.
			var intensity = _amplitude * (1 - ((_duration - _timer) / _duration));
			// Noise calculation logic from http://jonny.morrill.me/blog/view/14
			var new_x = rand.RandfRange(-1, 1);
			var x_component = intensity * (_previous_x + (delta * (new_x - _previous_x)));
			var new_y = rand.RandfRange(-1, 1);
			var y_component = intensity * (_previous_y + (delta * (new_y - _previous_y)));
			_previous_x = new_x;
			_previous_y = new_y;
			// Track how much we've moved the offset, as opposed to other effects.
			var new_offset = new Vector2(x_component, y_component);
			SetOffset(GetOffset() - _last_offset + new_offset);
			_last_offset = new_offset;
		}
		//Reset the offset when we're done shaking.
		_timer = _timer - delta;
		if (_timer <= 0)
		{
			_timer = 0;
			SetOffset(GetOffset() - _last_offset);
		}
	}

	//Kick off a new screenshake effect.
	public void Shake(float duration, float frequency, float amplitude)
	{
		GD.Print("Shaking ", duration, " " , frequency, " ", amplitude);
		//Initialize variables.
		_duration = duration;
		_timer = duration;

		_period_in_ms = 1 / frequency;
		_amplitude = amplitude;

		_previous_x = rand.RandfRange(-1, 1);

		_previous_y = rand.RandfRange(-1, 1);
		//Reset previous offset, if any.
		SetOffset(GetOffset() - _last_offset);

		_last_offset = new Vector2(0, 0);
	}
}
