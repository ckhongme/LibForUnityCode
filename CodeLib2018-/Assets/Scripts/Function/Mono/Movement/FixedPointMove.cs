using UnityEngine;
using System.Collections;


//Let this object to move toward the points one by one
public class FixedPointMove : MonoBehaviour
{ 
    [Tooltip("the Object that contains all wayPoints(include itself)")]
    public Transform wayPointObj;   
    public float moveSpeed = 2.5f;

    private int index;
    private Transform[] wayPoints;

    void Start()
    {
        index = 0;
        wayPoints = wayPointObj.GetComponentsInChildren<Transform>(true);
    }

    void Update()
    {
        MoveRotate(wayPoints[index].position);
    }

    void MoveRotate(Vector3 target)
    {
        Rotation(target);
        Movement(target);
    }

    void Rotation(Vector3 targetPos)
    {
        Quaternion quaterAngle = Quaternion.Lerp(transform.rotation,
                                                Quaternion.LookRotation(targetPos - transform.position),
                                                Time.deltaTime * 3);
        transform.eulerAngles = new Vector3(0, quaterAngle.eulerAngles.y, 0);
    }

    void Movement(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
        if (transform.position == targetPos)
        {
            index += index < wayPoints.Length ? 1:0;
            index %= wayPoints.Length;
        }
    }
}
