using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemObject)), CanEditMultipleObjects]
public class PropertyHolderEditor : Editor {

    public SerializedProperty
        name_Prop,
        itemType_Prop,
        costGems_Prop,
        amount_Prop,
        assetImage_Prop,
        items_Prop,
        shopImage_Prop;

    void OnEnable() {
        // Setup the SerializedProperties
        name_Prop = serializedObject.FindProperty("name");
        itemType_Prop = serializedObject.FindProperty("itemType");
        costGems_Prop = serializedObject.FindProperty("costGems");
        amount_Prop = serializedObject.FindProperty("amount");
        assetImage_Prop = serializedObject.FindProperty("assetImage");
        shopImage_Prop = serializedObject.FindProperty("shopImage");
        items_Prop = serializedObject.FindProperty("items");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(name_Prop, new GUIContent("name"));
        EditorGUILayout.PropertyField(itemType_Prop);

        ItemObject.ItemType st = (ItemObject.ItemType)itemType_Prop.enumValueIndex;

        switch (st) {
            case ItemObject.ItemType.Ball:
                EditorGUILayout.PropertyField(costGems_Prop, new GUIContent("costGems"));
                EditorGUILayout.PropertyField(assetImage_Prop, new GUIContent("assetImage"));
                EditorGUILayout.PropertyField(shopImage_Prop, new GUIContent("shopImage"));

                break;
            case ItemObject.ItemType.Currency:
                EditorGUILayout.PropertyField(amount_Prop, new GUIContent("amount"));

                break;
            case ItemObject.ItemType.Booster:
                EditorGUILayout.PropertyField(amount_Prop, new GUIContent("amount"));
                EditorGUILayout.PropertyField(costGems_Prop, new GUIContent("costGems"));
                EditorGUILayout.PropertyField(shopImage_Prop, new GUIContent("shopImage"));

                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
 