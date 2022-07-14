using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene_02 : MonoBehaviour
{
    public SceneEnvironment env;
    public ArduiConnection Connection;
    public Text dataText;
    public float Speed;
    public Image image;
    private Sprite mySprite;

    public int width = 2;
    public int height = 2;

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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Color32[] resetColorArray = texture.GetPixels32();

            for (int i = 0; i < resetColorArray.Length; i++)
            {
                resetColorArray[i] = new Color32(255, 255, 255, 0);
            }

            texture.SetPixels32(resetColorArray);
            texture.Apply();

            image.sprite = null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Painting");

            // Apply all SetPixel calls
            texture.Apply();

            // Display on Sprite Renderer
            //sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
            //sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

            mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

            image.sprite = mySprite;

            image.SetNativeSize();

            pixelX = 0;

            pixelY = 0;

            keepTime = 0f;
        }

        if (!env.IsGameStart)
        {
            return;
        }

        float upValue = 0;

        if (!string.IsNullOrEmpty(value))
        {
            //float.TryParse(sp.ReadLine(), out m); //將資料轉成float

            value.TryParseJSON<Data>(out data);

            if (data != null)
            {
                //Debug.Log($"Distance {data.distance}");

                lerpValue = Mathf.Lerp(lerpValue, Mathf.Clamp(data.distance, 0f, 30), Time.deltaTime * Speed);

                upValue = lerpValue * 0.1f * 0.3f;

                Debug.Log(upValue);

                upValue = Mathf.Clamp(upValue, 0f, 1f);

                //for (int x = 0; x < width; x++)
                //{
                //    for (int y = 0; y < height; y++)
                //    {
                //        texture.SetPixel(x, y, new Color(1, 1, 1, 1));
                //    }
                //}                
            }
        }

        if (pixelX < width && keepTime > 1 / 16)
        {
            keepTime = 0f;
            pixelY = 0;
            pixelX += 1;
        }

        if (pixelY < height)
        {
            pixelY = 30 - Mathf.RoundToInt(lerpValue);
        }

        dataText.text = $"{pixelX}, {pixelY}";

        texture.SetPixel(pixelX, pixelY, new Color(1, 0, 0, 1));

        keepTime += Time.deltaTime;
    }

    // Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
    Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
