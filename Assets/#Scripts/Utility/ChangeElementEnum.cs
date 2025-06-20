/*
 ファイル名：ChangeElementEnum.cs
 概　　　要：配列名を指定のenumに変更
 作　成　者：20CU0213　小林大輝
 */

/*
 更新履歴：
    2022/09/11  [20cu0213 小林大輝]　以下のサイトを参考に作成
        https://goropocha.hatenablog.com/entry/2021/02/11/232617
*/


using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Showing an array with Enum as keys in the property inspector. (Supported children)
/// </summary>
public class EnumIndexAttribute : PropertyAttribute
{
    private string[] _names;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="enumType"></param>
    public EnumIndexAttribute(Type enumType) => _names = Enum.GetNames(enumType);

#if UNITY_EDITOR
    /// <summary>
    /// Show inspector
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumIndexAttribute))]
    private class Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var names = ((EnumIndexAttribute)attribute)._names;
            // propertyPath returns something like hogehoge.Array.data[0]
            // so get the index from there.
            var index = int.Parse(property.propertyPath.Split('[', ']').Where(c => !string.IsNullOrEmpty(c)).Last());
            if (index < names.Length) label.text = names[index];
            EditorGUI.PropertyField(rect, property, label, includeChildren: true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }
    }
#endif
}