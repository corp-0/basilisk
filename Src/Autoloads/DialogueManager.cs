using System;
using System.Collections.Generic;
using Basilisk.Models;
using Basilisk.Ui;
using Chickensoft.GoDotLog;
using Godot;
using MonoCustomResourceRegistry;
using Tomlyn;

namespace Basilisk.Autoloads;

[RegisteredType(nameof(DialogueManager))]
public partial class DialogueManager: Node
{
    private string DialoguesPath => "res://GameData/Dialogues/";
    private ILog _log = new GDLog(nameof(DialogueManager));
    public List<DialogueModel> Dialogues { get; } = new();
    private Queue<DialogueModel> DisplayingDialogues { get; } = new();
    public event Action<DialogueModel>? DialoguedDequeued;

    public override void _Ready()
    {
        _log.Print($"Initialising {nameof(DialogueManager)}...");
        InitialiseDialoguesDatabase();
        _log.Print($"Loaded {Dialogues.Count} dialogues");
    }

    public void QueueDialogue(string id)
    {
        if (!TryFindById(id, out var dialogue))
        {
            _log.Err($"Failed to find dialogue with id: {id}");
            return;
        }

        DisplayingDialogues.Enqueue(dialogue!);
    }

    public void StartDialogue()
    {
        if (DisplayingDialogues.Count > 0)
        {
            var dialogue = DisplayingDialogues.Dequeue();
            DialoguedDequeued?.Invoke(dialogue);
        }
    }

    private bool TryFindById(string id, out DialogueModel? dialogue)
    {
        dialogue = Dialogues.Find(d => d.UniqueId == id);
        return dialogue != null;
    }

    private static string EvaluateSpeaker(string fileName, string? serialiazedSpaker)
    {
        if (serialiazedSpaker != null)
        {
            return serialiazedSpaker;
        }

        return fileName.Equals("player_dialogues.toml") ? "player" : "system";
    }

    private void InitialiseDialoguesDatabase()
    {
        var dir = DirAccess.Open(DialoguesPath);
        var definitionFiles = dir.GetFiles();
        _log.Print($"Found {definitionFiles.Length} files in {DialoguesPath}");
        foreach (var file in definitionFiles)
        {
            var content = FileAccess.GetFileAsString(DialoguesPath.PathJoin(file));
            
            if (Toml.TryToModel<DialoguesFileModel>(content, out var collection, out var diagnostics))
            {
                _log.Print($"Loaded file: {file}");
                foreach (var dialogue in collection.Dialogues)
                {
                    dialogue.Speaker = EvaluateSpeaker(file, dialogue.Speaker);
                    Dialogues.Add(dialogue);
                }
                continue;
            }
            
            _log.Err($"Failed to load definition file: {file}");
            var errors = diagnostics.GetEnumerator();
            while (errors.MoveNext())
            {
                _log.Err(errors.Current.ToString());
            }

            errors.Dispose();
        }
    }
}