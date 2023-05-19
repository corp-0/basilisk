using Godot;
using MonoCustomResourceRegistry;

namespace Basilisk.Ui;

[RegisteredType(nameof(MemoryItemUi), baseType: nameof(Panel))]
public partial class MemoryItemUi: Panel
{
    [Export] public Label ConceptLabel { get; private set; } = null!;
}