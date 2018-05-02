using UnityEngine;

public class QuestObjective
{
    private UI_ObjectiveText _ui_objectiveText;
    private UI_ObjectiveIcon _ui_objectiveIcon;

    public QuestObjective()
    {
        _ui_objectiveText = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveText>();
        if (_ui_objectiveText == null) Debug.LogError("WARNING: UI_ObjectiveText reference missing from QuestObjective");

        _ui_objectiveIcon = GlobalObjectManager.Instance.HUDCanvas.GetComponentInChildren<UI_ObjectiveIcon>();
        if (_ui_objectiveIcon == null) Debug.LogError("WARNING: UI_ObjectiveIcon reference missing from QuestObjective");
    }

    public void UpdateObjectiveText(string text = "", bool popup = true)
    {
        _ui_objectiveText.UpdateText(text);
        if (popup) _ui_objectiveText.PopUp();
    }

    public void PopUpText()
    {
        _ui_objectiveText.PopUp();
    }

    public void UpdateObjectiveIcon(GameObject target, bool popup = true)
    {
        _ui_objectiveIcon.objectiveObject = target;
        if (popup) _ui_objectiveIcon.PopUp();
    }

    public void PopUpIcon()
    {
        _ui_objectiveIcon.PopUp();
    }
}
