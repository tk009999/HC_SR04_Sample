using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SampleScene_03 : MonoBehaviour
{
    public RectTransform target;
    public RectTransform launch;
    public SceneEnvironment env;
    public ArduiConnection Connection;
    public float Speed;

    Sprite mySprite;
    Data data;
    float lerpValue;

    //private SpriteRenderer sr;

    private void OnEnable()
    {
        Connection.OnSerialPortDataSent.AddListener(OnSerialPortDataSent);
    }

    private void OnDisable()
    {
        Connection.OnSerialPortDataSent.RemoveListener(OnSerialPortDataSent);
    }

    int pixelX = 0;
    int pixelY = 0;
    float keepTime = 0f;

    void OnSerialPortDataSent(string value)
    {
        //Debug.Log("value=" + value);

        if (!env.IsGameStart)
        {
            return;
        }

        if (!string.IsNullOrEmpty(value))
        {
            //float.TryParse(sp.ReadLine(), out m); //將資料轉成float

            value.TryParseJSON<Data>(out data);

            if (data != null)
            {
                //Debug.Log($"Distance {data.distance}");

                lerpValue = Mathf.Lerp(lerpValue, Mathf.Clamp(data.distance, 0f, 30), Time.deltaTime * Speed);

                //upValue = lerpValue * 0.1f * 0.3f;

                //Debug.Log(upValue);

                //upValue = Mathf.Clamp(upValue, 0f, 1f);

                //for (int x = 0; x < width; x++)
                //{
                //    for (int y = 0; y < height; y++)
                //    {
                //        texture.SetPixel(x, y, new Color(1, 1, 1, 1));
                //    }
                //}                
            }
        }

        Debug.Log($"{pixelX}, {pixelY}");

        keepTime += Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        target.position = Input.mousePosition;
        Vector3 to = target.position;
        Vector3 from = launch.position;
        from.z = 0f;

        Vector3 dir = to - from;
        dir = dir.normalized;
        Debug.Log(dir);

        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.y);

        Debug.Log(angle);

        Debug.Log(dir);

        launch.localEulerAngles = Vector3.back * angle;
    }
}
