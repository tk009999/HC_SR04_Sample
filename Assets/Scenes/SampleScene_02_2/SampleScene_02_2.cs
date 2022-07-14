using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DrawArea
{
    public List<int> width;
    public List<int> height;
}

public class SampleScene_02_2 : MonoBehaviour
{
    public SceneEnvironment env;
    public ArduiConnection Connection;
    public Text modeText;
    public Text dataText;
    public float Speed;
    public Image image;
    public Color DisplayColor1;
    public Color DisplayColor2;
    public Color DisplayColor3;

    public int width = 2;
    public int height = 2;

    DrawArea drawAreaV = new DrawArea();
    DrawArea drawAreaH = new DrawArea();
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

    enum Mode
    {
        None,
        Pause,
        Vertical,
        Horizontal
    }

    int pixelX = 0;
    int pixelY = 0;
    float keepTime = 0f;
    Mode mode = Mode.None;

    void OnSerialPortDataSent(string value)
    {
        //Debug.Log("value=" + value);

        if (Input.GetKeyDown(KeyCode.F1))
        {
            mode = Mode.Horizontal;
            drawAreaH.width = new List<int>();
            drawAreaH.height = new List<int>();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            mode = Mode.Vertical;
            drawAreaV.width = new List<int>();
            drawAreaV.height = new List<int>();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            mode = Mode.Pause;
        }

        if (Input.GetKeyDown(KeyCode.Space))
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

            int minHWidth = drawAreaH.width[0];
            int maxHWidth = drawAreaH.width[drawAreaH.width.Count - 1];

            int minHHeight = drawAreaH.height[0];
            int maxHHeight = drawAreaH.height[drawAreaH.height.Count - 1];

            Vector2 H_LD = new Vector2(minHWidth, 0);
            Vector2 H_RD = new Vector2(maxHWidth, 0);
            Vector2 H_LU = new Vector2(minHWidth, minHHeight);
            Vector2 H_RU = new Vector2(maxHWidth, maxHHeight);

            int minVWidth = drawAreaV.width[0];
            int maxVWidth = drawAreaV.width[drawAreaV.width.Count - 1];

            int minVHeight = drawAreaV.height[0];
            int maxVHeight = drawAreaV.height[drawAreaV.height.Count - 1];

            Vector2 V_LD = new Vector2(0, minVHeight);
            Vector2 V_RD = new Vector2(minVWidth, minVHeight);
            Vector2 V_LU = new Vector2(0, maxVHeight);
            Vector2 V_RU = new Vector2(maxVWidth, maxVHeight);

            Debug.Log($"V_RD => {V_RD}");

            Vector2 provit = new Vector2((int)H_LU.x, (int)V_RD.y);

            Debug.Log(provit);

            int w = (int)(H_RU.x - H_LU.x + H_LU.x);
            int h = (int)(V_RU.y - V_RD.y + V_RD.y);
            Vector2 size = new Vector2(w, h);

            Debug.Log(size);

            for (int x = (int)provit.x; x < (int)size.x; x++)
            {
                for (int y = (int)provit.y; y < (int)size.y; y++)
                {
                    //Debug.Log($"{x}, {y}");
                    texture.SetPixel(x, y, DisplayColor1);
                }
            }

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

            mode = Mode.None;
        }

        modeText.text = $"{mode}";

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

        if (mode == Mode.Horizontal)
        {
            if (pixelX < width && keepTime > 1 / 16)
            {
                keepTime = 0f;
                pixelY = 0;
                pixelX += 1;
            }

            if (pixelY < height)
            {
                pixelY = 30 - Mathf.RoundToInt(lerpValue);

                for (int i = 0; i < pixelY; i++)
                {
                    texture.SetPixel(pixelX, i, DisplayColor2);
                }
            }

            texture.SetPixel(pixelX, pixelY, DisplayColor3);

            if (pixelY >= 10 && pixelX > 0)
            {
                drawAreaH.width.Add(pixelX);
                drawAreaH.height.Add(pixelY);
            }
        }

        if (mode == Mode.Vertical)
        {
            if (pixelY < height && keepTime > 1 / 16)
            {
                keepTime = 0f;
                pixelX = 0;
                pixelY += 1;
            }

            if (pixelX < width)
            {
                pixelX = 30 - Mathf.RoundToInt(lerpValue);

                for (int i = 0; i < pixelX; i++)
                {
                    texture.SetPixel(i, pixelY, DisplayColor2);
                }
            }

            texture.SetPixel(pixelX, pixelY, DisplayColor3);

            if (pixelX >= 10 && pixelY > 0)
            {
                drawAreaV.width.Add(pixelX);
                drawAreaV.height.Add(pixelY);
            }
        }

        dataText.text = $"{pixelX}, {pixelY}";

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
