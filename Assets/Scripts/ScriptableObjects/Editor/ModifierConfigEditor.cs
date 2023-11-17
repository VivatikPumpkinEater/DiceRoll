using System.Linq;
using UnityEditor;

[CustomEditor(typeof(ModifierConfig))]
public class ModifierConfigEditor : Editor
{
    private ModifierConfig _config;

    private void OnEnable()
    {
        _config = (ModifierConfig)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheckDuplicate();
    }

    private void CheckDuplicate()
    {
        var duplicateItems = _config.Models
            .GroupBy(x => x.Name)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateItems.Any())
            EditorGUILayout.HelpBox("There's a duplicate", MessageType.Warning);
    }
}