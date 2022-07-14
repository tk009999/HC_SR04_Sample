using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyToStart : MonoBehaviour
{
    public SceneEnvironment env;

    public GameObject Text;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
        env.IsGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            env.IsGameStart = true;

            Text.gameObject.SetActive(false);

            Debug.Log("TTTT");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            env.IsGameStart = false;

            Text.gameObject.SetActive(true);
        }
    }
}
