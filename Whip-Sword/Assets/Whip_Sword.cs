using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip_Sword : MonoBehaviour
{
    [SerializeField] bool whip = false;

    void Start()
    {
        for (int i = 0; i < GetComponentsInChildren<DistanceJoint2D>().Length; i++)
        {
            GetComponentsInChildren<DistanceJoint2D>()[GetComponentsInChildren<DistanceJoint2D>().Length - i -1].GetComponent<Rigidbody2D>().mass = 1 * (i * 10);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(toggleWhip());
        }
    }

    IEnumerator toggleWhip()
    {

        for (int i = 0; i < GetComponentsInChildren<DistanceJoint2D>().Length; i++)
        {
            GetComponentsInChildren<DistanceJoint2D>()[i].distance = !whip ? 0.5f : 0.005f;
            yield return new WaitForSeconds(0.15f);
        }

        for (int i = 0; i < GetComponentsInChildren<DistanceJoint2D>().Length; i++)
        {
            GetComponentsInChildren<DistanceJoint2D>()[i].GetComponent<Rigidbody2D>().gravityScale = !whip ? 1 : 0;
            GetComponentsInChildren<DistanceJoint2D>()[i].GetComponent<Rigidbody2D>().freezeRotation = whip;
            if (whip)
            {
                GetComponentsInChildren<DistanceJoint2D>()[i].transform.localEulerAngles = Vector3.zero;
            }
        }

        whip = !whip;
        yield return null;
    }
}
