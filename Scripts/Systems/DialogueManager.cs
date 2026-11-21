namespace app.Systems;

using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using app.Models;

public partial class DialogueManager : Node
{
    public static DialogueManager Instance { get; private set; }

    public event Action<string,string> OnDialogueLineChanged;
    public event Action OnDialogueEnded;

    private readonly Dictionary<string, NpcData> _npcDatabase = new();
    private NpcData _activeNpc;
    private int _currentLineIndex = -1;

    public bool IsDialogueActive => _activeNpc != null;

    public override void _Ready()
    {
        Instance = this;
        LoadNpcData("res://Data/npcs.json");
    }

    private void LoadNpcData(string filePath)
    {
        if( !FileAccess.FileExists(filePath) ){
            GD.PushError($"NPC data file not found at: {filePath}");
            return;
        }

        string json = FileAccess.GetFileAsString(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var npcList = JsonSerializer.Deserialize<List<NpcData>>(json, options);

        if (npcList == null) return;

        _npcDatabase.Clear();
        foreach ( var npc in npcList )
        {
            _npcDatabase[npc.Id] = npc;
        }

        GD.Print($"Loaded {_npcDatabase.Count} NPCs from JSON.");
    }

    public NpcData GetNpcData(string npcId)
    {
        return _npcDatabase.TryGetValue(npcId, out var data) ? data : null;
    }

    public void StartDialogue(string npcId)
    {
        var data = GetNpcData(npcId);
        if( data == null || data.Dialogues.Count == 0)
        {
            GD.PushWarning($"No dialogue found for NPC ID: {npcId}");
            return;
        }

        _activeNpc = data;
        _currentLineIndex = 0;
        OnDialogueLineChanged?.Invoke(_activeNpc.Name, _activeNpc.Dialogues[_currentLineIndex]);
    }

    public void AdvanceDialogue()
    {
        if( !IsDialogueActive ) return;

        _currentLineIndex++;
        if(_currentLineIndex < _activeNpc.Dialogues.Count)
        {
            OnDialogueLineChanged?.Invoke(_activeNpc.Name, _activeNpc.Dialogues[_currentLineIndex]);
        }
        else
        {
            _activeNpc = null;
            _currentLineIndex = -1;
            OnDialogueEnded?.Invoke();
        }
    }
}