using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Interactions;

[RegisteredType(nameof(SimpleExaminable), baseType:nameof(Area2D))]
public partial class SimpleExaminable : Area2D
{
	[Export] public string ConceptUniqueId { get; private set; } = null!;
	[Export] public string Conclusion { get; private set; } = string.Empty;
	[Export] public string ClueId { get; private set; } = string.Empty;
	[Export] public CollisionShape2D Shape { get; private set; } = null!;
	private Line2D _outline = null!;
	
	private const string INPUT_EVENT = "input_event";
	private const string MOUSE_DOWN = "mouse_down";
	private readonly ILog _log = new GDLog(nameof(SimpleExaminable));
	private string _isNotClueText = "is not part of a clue";
	private string _isPartOfClueText = "is part of clue with id {0}";
	private PlayerToolState PlayerToolState => this.Autoload<PlayerToolState>();
	private ShortTermMemoryManager ShortTermMemoryManager => this.Autoload<ShortTermMemoryManager>();

	private bool IsInMemory => ShortTermMemoryManager.IsInMemory(ConceptUniqueId);
	
	public override void _Ready()
	{
		_outline = new Line2D();
		_outline.ZIndex = 100;
		_outline.ZAsRelative = true;
		_outline.Position = Shape.Position;
		AddChild(_outline);
    
		// Assuming a rectangular shape
		Rect2 shapeRect = Shape.Shape.GetRect();
		Vector2[] points = new Vector2[5];
		points[0] = shapeRect.Position;
		points[1] = new Vector2(shapeRect.Position.X + shapeRect.Size.X, shapeRect.Position.Y);
		points[2] = new Vector2(shapeRect.Position.X + shapeRect.Size.X, shapeRect.Position.Y + shapeRect.Size.Y);
		points[3] = new Vector2(shapeRect.Position.X, shapeRect.Position.Y + shapeRect.Size.Y);
		points[4] = shapeRect.Position; // Close the loop
		_outline.DefaultColor = new Color(0, 1, 0); // green color
		_outline.Width = 3; // line width
		_outline.Points = points;

		Connect(INPUT_EVENT, new Callable(this, nameof(OnInputEvent)));
	}

	public void OnInputEvent(Node viewport, InputEvent @event, int shapeIndx)
	{
		if (PlayerToolState.CurrentTool != PlayerTool.MagnifyingGlass) return;
		
		if (@event.IsActionPressed(MOUSE_DOWN))
		{
			_log.Print($"Sending examined concept to short term memory: {Conclusion}");
			_log.Print($"Concept {(string.IsNullOrEmpty(ClueId) ? _isNotClueText: string.Format(_isPartOfClueText, ClueId))}");
			MemorisedConceptModel concept = new()
			{
				ConceptUniqueId = ConceptUniqueId,
				Conclusion = Conclusion,
				ClueId = ClueId
			};
			ShortTermMemoryManager.Add(concept);
		}
	}

	public override void _Process(double delta)
	{
		_outline.Visible = IsInMemory;
	}
}