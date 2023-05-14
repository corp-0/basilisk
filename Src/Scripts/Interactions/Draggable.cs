using System;
using Chickensoft.GoDotLog;
using Godot;

namespace Basilisk.Interactions;

public partial class Draggable : Area2D, IComponent
{
	private CollisionShape2D? _shape2D;
	public Sprite2D Sprite { get; private set; } = null!;
	private Node2D _parent = null!;
	
	private Vector2 _mouseOffset;
	private readonly ILog _log = new GDLog(nameof(Draggable));
	private bool _selected;
	private const string INPUT_EVENT = "input_event";
	private const string MOUSE_DOWN = "mouse_down";

	public string Type => nameof(Draggable);
	
	public void Init(Node2D parent)
	{
		_log.Print($"Initializing DraggableComponent with parent {parent.Name}...");
		_parent = parent;
		Sprite = _parent.GetNodeOrNull<Sprite2D>("Sprite") ?? throw new NullReferenceException();
		Connect(INPUT_EVENT, new Callable(this, nameof(OnInputEvent)));
		var spriteSize = Sprite.Texture.GetSize();
		_shape2D = new CollisionShape2D
		{
			Shape = new RectangleShape2D
			{
				Size = spriteSize
			}
		};
		AddChild(_shape2D);
		AddToGroup("draggable");
	}

	public void OnInputEvent(Node viewport, InputEvent @event, int shapeIndx)
	{
		if (@event.IsActionPressed(MOUSE_DOWN))
		{
			_selected = true;
			MakeTopMost();
			_mouseOffset = _parent.GlobalPosition - GetGlobalMousePosition();
			_log.Print($"Draggable selected: {_selected}");
			return;
		}

		if (!@event.IsActionReleased(MOUSE_DOWN)) return;
		_selected = false;
		_log.Print($"Draggable selected: {_selected}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_selected) return;
		var mousePos = GetGlobalMousePosition() + _mouseOffset;
		_parent.GlobalPosition = mousePos;
		_log.Print($"Draggable position: {_parent.GlobalPosition}");
	}

	private void MakeTopMost()
	{
		var draggables = GetTree().GetNodesInGroup("draggable");
		foreach (var draggable in draggables)
		{
			if (draggable is not Draggable d) continue;
			if (d == this)
			{
				// Increase Z-index of the selected object
				Sprite.ZIndex++;
			}
			else
			{
				// Reset Z-index of all other objects
				d.Sprite.ZIndex = 0;
			}
		}
	}
}