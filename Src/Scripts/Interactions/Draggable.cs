using System;
using System.Linq;
using Basilisk.Autoloads;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Interactions;

[RegisteredType(nameof(Draggable), baseType:nameof(Area2D))]
public partial class Draggable : Area2D
{
	[Export] public CollisionShape2D Shape2D { get; private set; } = null!;
	[Export] public Sprite2D Sprite { get; private set; } = null!;

	private Vector2 _mouseOffset;
	private readonly ILog _log = new GDLog(nameof(Draggable));
	public bool Selected { get; set; }
	private const string INPUT_EVENT = "input_event";
	private const string MOUSE_DOWN = "mouse_down";
	private Node2D Parent => GetParent<Node2D>();
	private PlayerToolState PlayerToolState => this.Autoload<PlayerToolState>();

	public override void _Ready()
	{
		Connect(INPUT_EVENT, new Callable(this, nameof(OnInputEvent)));
		Shape2D.Shape = new RectangleShape2D
		{
			Size = Sprite.Texture.GetSize()
		};
		AddToGroup("draggable");
	}
	
	
	public void OnInputEvent(Node viewport, InputEvent @event, int shapeIndx)
	{
		if (PlayerToolState.CurrentTool != PlayerTool.Hand || Selected == false) return;

		if (GetTree().GetNodesInGroup("draggable").Any(IsSelected)) return;

		if (@event.IsActionPressed(MOUSE_DOWN))
		{
			MakeTopMost();
			_mouseOffset = GetParent<Node2D>().GlobalPosition - GetGlobalMousePosition();
			_log.Print($"Draggable selected: {Selected}");
			return;
		}

		if (!@event.IsActionReleased(MOUSE_DOWN)) return;
		Selected = false;
		_log.Print($"Draggable selected: {Selected}");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Selected) return;
		var mousePos = GetGlobalMousePosition() + _mouseOffset;
		GetParent<Node2D>().GlobalPosition = mousePos;
	}

	private void MakeTopMost()
	{
		var draggables = GetTree().GetNodesInGroup("draggable");
		foreach (var draggable in draggables)
		{
			if (draggable is not Draggable d) continue;
			if (d == this)
			{
				Parent.ZIndex++;
				Parent.ZIndex++;
				d.Priority++;
			}
			else
			{
				d.Parent.ZIndex = 0;
				d.Parent.ZIndex = 0;
				d.Priority = 0;
			}
		}
	}

	private bool IsSelected(Node target)
	{
		return target is Draggable { Selected: true} && target != this;
	}
}