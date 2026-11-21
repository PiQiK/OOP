namespace app.UI;

using Godot;
using app.Systems;

public partial class DialogueBox : CanvasLayer
{
    private PanelContainer _panel;
    private Label _nameLabel;
    private Label _dialogueLabel;

    public override void _Ready()
    {
        _panel = GetNode<PanelContainer>("PanelContainer");
        _nameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/NameLabel");
        _dialogueLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/DialogueLabel");

        _panel.Visible = false;

        if( DialogueManager.Instance != null )
        {
            DialogueManager.Instance.OnDialogueLineChanged += ShowLine;
            DialogueManager.Instance.OnDialogueEnded += HideBox;
        }
    }

    public override void _ExitTree()
    {
        if( DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueLineChanged -= ShowLine;
            DialogueManager.Instance.OnDialogueEnded -= HideBox;
        }
    }

    private void ShowLine(string speakerName, string line)
    {
        _nameLabel.Text = speakerName;
        _dialogueLabel.Text = line;
        _panel.Visible = true;
    }

    private void HideBox()
    {
        _panel.Visible = false;
    }
}