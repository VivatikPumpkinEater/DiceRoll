using System.Linq;
using UnityEditor;

[CustomEditor(typeof(VFXPoolConfig))]
public class VFXPoolConfigEditor : Editor
{
    private VFXPoolConfig _config;

    private void OnEnable()
    {
        _config = (VFXPoolConfig)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CheckDuplicate();
    }

    private void CheckDuplicate()
    {
        var duplicateItems = _config.Models
            .GroupBy(x => x.Type)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateItems.Any())
            EditorGUILayout.HelpBox("There's a duplicate", MessageType.Warning);
    }
}