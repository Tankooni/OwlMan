using Godot;
using System;

public partial class ShakeCamera : Camera2D
{
	public Node2D FollowNode;

	private double _duration = 0.0f;
	private double _period_in_ms = 0.0f;
	private double _amplitude = 0.0f;
	private double _timer = 0.0f;
	private double _last_shook_timer = 0;
	private double _previous_x = 0.0f;
	private double _previous_y = 0.0f;
	private Vector2 _last_offset = new Vector2(0, 0);

	//private Vector2 _last_window_offset;

	private RandomNumberGenerator rand = new RandomNumberGenerator();

    public override void _EnterTree()
    {
        base._EnterTree();
		Overlord.MainCamera = this;
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(FollowNode == null)
			return;
		UpdatePosition();

		//CAMERA SHAKING

		//Only shake when there's shake time remaining.
		if (_timer == 0) {
			//_last_window_offset = OS.WindowPosition;
			return;
		}

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
			var new_offset = new Vector2((float)x_component, (float)y_component);
			this.Offset -= (_last_offset + new_offset);
			//OS.WindowPosition += (_last_offset + new_offset);
		}
		//Reset the offset when we're done shaking.
		_timer = _timer - delta;
		_last_offset = new Vector2(0, 0);// new_offset;
		if (_timer <= 0)
		{
			_timer = 0;
			this.Offset = new Vector2(0, 0); // -= _last_offset;
			//OS.WindowPosition = this._last_window_offset;
			//Engine.TimeScale = 1f;
		}
	}

	/// <summary>
	/// Kick off a new screenshake effect.
	/// </summary>
	/// <param name="duration"></param>
	/// <param name="frequency"></param>
	/// <param name="amplitude"></param>
	public void Shake(float duration, float frequency, float amplitude)
	{
		//Initialize variables.
		_duration = duration;
		_timer = duration;

		_period_in_ms = 1 / frequency;
		_amplitude = amplitude;

		_previous_x = rand.RandfRange(-1, 1);

		_previous_y = rand.RandfRange(-1, 1);
		//Reset previous offset, if any.
		this.Offset -= _last_offset;

		_last_offset = new Vector2(0, 0);
	}

	public void UpdatePosition()
	{
		var centerX = FollowNode.Position.X;
		var centerY = FollowNode.Position.Y;

		centerX = Mathf.Clamp(centerX, Overlord.ViewportSize.X / 2f, Overlord.LevelBoundsX.Y - Overlord.ViewportSize.X / 2f);
		centerY = Mathf.Clamp(centerY, Overlord.ViewportSize.Y / 2f, Overlord.LevelBoundsY.Y - Overlord.ViewportSize.Y / 2f);

		this.Position = new Vector2(centerX, centerY);
	}

	public void SetFollow(NodePath nodePath)
	{
		var node = (Node2D)GetNode(nodePath);
		if (node == null)
		{
			ClearFollow();
			return;
		}
		FollowNode = node;
		UpdatePosition();
	}

	public void ClearFollow()
	{
		FollowNode = null;
	}
}
