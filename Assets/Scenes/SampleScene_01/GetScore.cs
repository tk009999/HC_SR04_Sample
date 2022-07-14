using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetScore : MonoBehaviour
{
    int i = 0;

    public UnityEvent<Vector3, float> OnScoreGet = new UnityEvent<Vector3, float>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        i++;

        //Debug.Log(i);

        Vector3 pos = other.gameObject.transform.position;

        OnScoreGet?.Invoke(pos, i);
    }
}
