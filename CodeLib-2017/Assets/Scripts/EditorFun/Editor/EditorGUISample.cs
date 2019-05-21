using UnityEditor;

public class EditorGUISample
{
    bool isFoldout = true;         //是否显示折页内容

    private void GUIFuns()
    {
        //垂直组
        EditorGUILayout.BeginVertical();
        //设置标题
        EditorGUILayout.LabelField("Title", "K InspectorTest");
        //设置坐标
        //k.Point = EditorGUILayout.Vector3Field("Point", k.Point);
        //设置枚举
        //k.kEnum = (KEnum)EditorGUILayout.EnumPopup("K Enum", k.kEnum);
        //结束垂直组
        EditorGUILayout.EndVertical();



        //标签折页箭头
        isFoldout = EditorGUILayout.Foldout(isFoldout, "K Struct");
        if (isFoldout)
        {
            if (Selection.activeTransform)  //访问编辑器中的选择, 只返回一个场景中被选中的游戏对象变换
            {
                EditorGUILayout.BeginVertical();

                //水平组1
                EditorGUILayout.BeginHorizontal();
                //标签的项
                EditorGUILayout.PrefixLabel("   value1");
                //k.value1 = EditorGUILayout.Slider(k.value1, 0, 1);
                EditorGUILayout.EndHorizontal();

                //水平组2
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("   value2");
                //k.value2 = EditorGUILayout.IntSlider(k.value2, 0, 100);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.TextField("TextField", "content");
    }
}

//public class KInspectorTest : MonoBehaviour
//{
//    public Vector3 Point;
//    public KEnum kEnum;
//    public float value1;
//    public int value2;
//}

//public enum KEnum
//{
//    k1,
//    k2
//}
