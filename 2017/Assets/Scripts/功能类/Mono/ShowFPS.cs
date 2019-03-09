using UnityEngine;
using System.Collections;
using System.Text;

public class ShowFPS : MonoBehaviour 
{
    public bool showFPS;
    public float f_UpdateInterval = 0.5F;
    public int frateRate = 60;

    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;
    private StringBuilder sb;

#if UNITY_EDITOR
    private bool isShowMore = true;
#elif UNITY_STANDALONE_WIN
    private bool isShowMore = false;
#endif

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        Application.targetFrameRate = frateRate;
        f_LastInterval = Time.realtimeSinceStartup;
        i_Frames = 0;
        sb = new StringBuilder();
    }

    void OnGUI() 
    {
        if (!showFPS) return;

        sb.Remove(0, sb.Length);
        sb.Append("FPS:\t").AppendLine(f_Fps.ToString("f2"));

        if (isShowMore)
        {
            sb.Append(tip);
            sb.Append("DC:\t").AppendLine(UnityEditor.UnityStats.drawCalls.ToString())
                .Append("Batch:\t").AppendLine(UnityEditor.UnityStats.batches.ToString())
                .Append("Tri:\t").AppendLine(UnityEditor.UnityStats.triangles.ToString())
                .Append("Ver:\t").AppendLine(UnityEditor.UnityStats.vertices.ToString());
        }

        GUI.color = Color.black;
		GUI.Label(new Rect(Screen.width - 150, 10, 200, 300), sb.ToString());
    }


    void Update() 
    {
        if (!showFPS) return; 

        ++i_Frames;
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval) 
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;
            f_LastInterval = Time.realtimeSinceStartup;
        }
    }

    #region Test
    private const string tip = @"
+ : Add HP
- : Sub HP
0 : Start Of Level1
1 : End of Level1
2 : Ready to fly
3 : Start of Level2
4 : End of Level2
5 : Game Over
G : Get Gun
M : Game Help
Space : Game Restart";
    #endregion
}