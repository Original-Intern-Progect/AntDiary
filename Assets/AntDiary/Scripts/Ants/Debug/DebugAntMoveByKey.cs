using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAntMoveByKey : MonoBehaviour
{
    public GameObject targetObj;
    public bool isHoldingFood;

    private Vector3 targetPos;
    private Vector3 nowPos;

    // Start is called before the first frame update
    void Start()
    {
        nowPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.1f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.1f, 0, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

        targetPos = transform.position;
    }
}
