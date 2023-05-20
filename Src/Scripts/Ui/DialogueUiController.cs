using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotLog;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(DialogueUiController), baseType: nameof(Control))]
public partial class DialogueUiController: Control
{
    [Export] private TextureRect _backgroundImage = null!;
    [Export] private TextureRect _jumpingTriangle = null!;
    [Export] private Label _textLabel = null!;
    [Export] private AnimationPlayer _animationPlayer = null!;
    private ILog _log = new GDLog(nameof(DialogueUiController));
    public bool IsShowingDialogue { get; private set; }

    private DialogueManager DialogueManager => this.Autoload<DialogueManager>();

    public override void _Ready()
    {
        DialogueManager.DialoguedDequeued += ShowDialogue;
        _backgroundImage.Connect("gui_input", new Callable(this, nameof(OnSkipDialogue)));
        _animationPlayer.Connect("animation_finished", new Callable(this, nameof(OnAnimationFinished)));
    }
    
    public void ShowDialogue(DialogueModel dialogue)
    {
        _log.Print($"Showing dialogue: {dialogue.UniqueId}: {dialogue.Text}");
        IsShowingDialogue = true;
        _textLabel.Text = dialogue.Text;
        _animationPlayer.Play("show_dialogue_window");
    }
    
    public void HideDialogue()
    {
        _animationPlayer.PlayBackwards("show_dialogue_window");
    }

    public void OnAnimationFinished(string animName)
    {
        if (animName == "show_dialogue_window")
        {
            _jumpingTriangle.Show();
            _animationPlayer.Play("jumping_triangle");
        }

        if (IsShowingDialogue == false)
        {
            _jumpingTriangle.Hide();
        }
    }
    
    public void OnSkipDialogue(InputEvent @event)
    {
        //If pressed space bar or enter or clicked the texture rect
        if (@event.IsActionPressed("skip_dialogue"))
        {
            HideDialogue();
            IsShowingDialogue = false;
            DialogueManager?.StartDialogue();
        }
    }
}