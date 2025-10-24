using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunicularController : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();
    public Vector3 offsetFromNodes;
    public float travelSpeed = 0.1f;

    private void Start()
    {
        StartCoroutine(MoveFunicular());
    }

    IEnumerator MoveFunicular()
    {
        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(nodes[0].position, nodes[1].position, t) + offsetFromNodes;
            t += Time.deltaTime * travelSpeed;
            yield return null;
        }
    }
}
