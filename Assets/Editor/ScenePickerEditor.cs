using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelsDataBase), true)]
public class LevelsDataBaseEditor : Editor
{
    private SerializedProperty _levels;

    private void OnEnable()
    {
        // do this only once here
        var ldb = target as LevelsDataBase;
        _levels = serializedObject.FindProperty(nameof(ldb.LevelsPaths));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var ldb = target as LevelsDataBase;
        PropertyDraw(ldb.TitleScenePath, nameof(ldb.TitleScenePath));
        PropertyDraw(ldb.LoadingScenePath, nameof(ldb.LoadingScenePath));


        //Ofcourse you also want to change the list size here
        _levels.arraySize = EditorGUILayout.IntField(nameof(ldb.LevelsPaths) + " size", _levels.arraySize);
        EditorGUI.indentLevel++;
        for (int i = 0; i < _levels.arraySize; i++)
        {
            var level = _levels.GetArrayElementAtIndex(i);
            PropertyDraw(level.stringValue, "Level " + i, level);
        }
        EditorGUI.indentLevel--;

        // Note: You also forgot to add this
        serializedObject.ApplyModifiedProperties();
    }

    private void PropertyDraw(string path, string propertyName, SerializedProperty sp = null)
    {
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);


        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField(propertyName, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            if (sp == null)
            {
                var scenePathProperty = serializedObject.FindProperty(propertyName);
                scenePathProperty.stringValue = newPath;
            } else
            {
                sp.stringValue = newPath;
            }
        }
    }
}