using Basilisk.Autoloads;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;

namespace Basilisk.Interactions;

public abstract partial class BaseExaminable: Node2D, IExaminable
{
    [Export] public string ConceptUniqueId { get; private set; } = null!;
    [Export] public string Conclusion { get; private set; } = string.Empty;
    [Export] public string ClueId { get; private set; } = string.Empty;
    
    // Is this examinable corrupted? If so, it will add to the insanity meter when examined.
    [Export] public bool IsCorrupted { get; private set; }
    [Export] public CollisionShape2D Shape { get; private set; } = null!;
    private Line2D _outline = null!;
    private Node2D Parent => GetParent<Node2D>().GetParent<Node2D>();
    public bool IsTopMost => Parent.ZIndex > 0;
	
    private const string INPUT_EVENT = "input_event";
    protected const string MOUSE_DOWN = "mouse_down";
    protected readonly ILog Log = new GDLog(nameof(SimpleExaminable));
    protected const string IS_NOT_CLUE_TEXT = "is not part of a clue";
    protected const string IS_PART_OF_CLUE_TEXT = "is part of clue with id {0}";
    protected PlayerToolState PlayerToolState => this.Autoload<PlayerToolState>();
    protected ShortTermMemoryManager ShortTermMemoryManager => this.Autoload<ShortTermMemoryManager>();

    private bool IsInMemory => ShortTermMemoryManager.IsInMemory(ConceptUniqueId);
    
    public override void _Ready()
    {
        _outline = new Line2D();
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

        Connect(INPUT_EVENT, new Callable(this, nameof(OnExamined)));
        PostReady();
    }
    
    /// <summary>
    /// Called right after the base _ready method
    /// </summary>
    public virtual void PostReady()
    {
        
    }

    public abstract void OnExamined(Node viewport, InputEvent @event, int shapeIndx);

    public override void _Process(double delta)
    {
        _outline.Visible = IsInMemory;
        PostProcess(delta);
    }
    
    /// <summary>
    /// Override this if you need to do something in the _process method
    /// </summary>
    /// <param name="delta"></param>
    public virtual void PostProcess(double delta)
    {
        
    }
}