using System.Collections.Generic;
using Basilisk.Autoloads;
using Basilisk.Models;
using Chickensoft.GoDotNet;
using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(ShortMemoryUiController), baseType: nameof(Control))]
public partial class ShortMemoryUiController: Control
{
    private ShortTermMemoryManager ShortTermMemoryManager => this.Autoload<ShortTermMemoryManager>();
    [Export] private PackedScene _memoryItemScene = null!;
    [Export] private AnimationPlayer _animationPlayer = null!;
    [Export] private Panel _panel = null!;
    [Export] private VBoxContainer _memoriesContainer = null!;
    private bool _isShowing;

    public override void _Ready()
    {
        Hide();
        HideUi();
        ShortTermMemoryManager.MemoryChanged += RefreshMemories;
        ShortTermMemoryManager.FirstTimeRemembering += Show;
        _panel.Connect("gui_input", new Callable(this, nameof(OnPanelGuiInput)));
    }

    private void ClearMemories()
    {
        foreach (var node in _memoriesContainer.GetChildren())
        {
            var item = (MemoryItemUi)node;
            item.QueueFree();
        }
    }
    
    private void RefreshMemories(List<MemorisedConceptModel> memorisedConceptModels)
    {
        ClearMemories();
        foreach (var memory in memorisedConceptModels)
        {
            var memoryItem = _memoryItemScene.Instantiate<MemoryItemUi>();
            memoryItem.ConceptLabel.Text = memory.Conclusion;
            _memoriesContainer.AddChild(memoryItem);
        }
    }
    
    private void ShowUi()
    {
        _animationPlayer.Play("show_short_term_memories");
        _isShowing = true;
    }
    
    private void HideUi()
    {
        _animationPlayer.Play("hide_short_term_memories");
        _isShowing = false;
    }
    
    private void ToggleUi()
    {
        if (_isShowing)
        {
            HideUi();
        }
        else
        {
            ShowUi();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_memories"))
        {
            ToggleUi();
        }
    }
    
    public void OnPanelGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_memories"))
        {
            ToggleUi();
        }
    }
}