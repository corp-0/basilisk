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
	private bool _selected;
	private const string INPUT_EVENT = "input_event";
	private const string MOUSE_DOWN = "mouse_down";
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
		if (PlayerToolState.CurrentTool != PlayerTool.Hand) return;
		
		if (@event.IsActionPressed(MOUSE_DOWN))
		{
			_selected = true;
			MakeTopMost();
			_mouseOffset = GetParent<Node2D>().GlobalPosition - GetGlobalMousePosition();
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
				Sprite.ZIndex++;
				Shape2D.ZIndex++;
			}
			else
			{
				d.Sprite.ZIndex = 0;
				d.Shape2D.ZIndex = 0;
			}
		}
	}
}