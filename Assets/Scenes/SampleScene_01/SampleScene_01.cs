using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene_01 : MonoBehaviour
{
    public SceneEnvironment env;
    public ArduiConnection Connection;

    public Transform Target;
    public int TargetSpeed;
    public Text ScoreText;
    public int Speed;
    public int Count;
    public ObjectPool Pool;
    public GetScore getScore;
    public ParticleSystem EFX;

    List<Transform> topFraises = new List<Transform>();

    List<Transform> bottomFraises = new List<Transform>();

    float[] speeds;
    Transform[] fraiseT;

    float lerpValue;

    Data data;

    private void OnEnable()
    {
        Connection.OnSerialPortDataSent.AddListener(OnSerialPortDataSent);
    }

    private void OnDisable()
    {
        Connection.OnSerialPortDataSent.RemoveListener(OnSerialPortDataSent);
    }

    // Start is called before the first frame update
    void Start()
    {
        getScore.OnScoreGet.AddListener(OnScoreGet);
        speeds = new float[Count];
        fraiseT = new Transform[Count];

        for (int i = 0; i < Count; i++)
        {
            fraiseT[i] = Pool.Get().transform;
            Vector3 VTop = (Vector3.right * 7.5f) + (Vector3.up * Random.Range(-5, 5));
            fraiseT[i].transform.position = VTop;
            speeds[i] = Speed;
        }
    }

    float Score = 0f;
    Coroutine coroutine;
    private void OnScoreGet(Vector3 pos, float value)
    {
        if (!env.IsGameStart)
        {
            return;
        }

        Instantiate(EFX, pos, Quaternion.identity);

        coroutine = StartCoroutine(YieldScoreGet(value));
    }

    IEnumerator YieldScoreGet(float value)
    {
        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime)
        {
            Score = Mathf.Lerp(Score, value, i);

            //Debug.Log(Score);

            ScoreText.text = Score.ToString("0");

            yield return null;
        }

        StopCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (!env.IsGameStart)
        {
            return;
        }

        for (int i = 0; i < Count; i++)
        {
            if (fraiseT[i] == null)
            {
                return;
            }


            if (CheckVisibility(fraiseT[i]))
            {
                fraiseT[i].position += Vector3.left * Time.deltaTime * speeds[i];
            }
            else
            {
                Pool.Return(fraiseT[i].gameObject);
                fraiseT[i] = Pool.Get().transform;
                Vector3 VTop = (Vector3.right * 7.5f) + (Vector3.up * Random.Range(-2.5f, 2.5f));
                fraiseT[i].transform.position = VTop;
                speeds[i] = Speed;
            }
        }
    }

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

            Vector3 targetPos = Target.localPosition;

            if (data != null)
            {
                //Debug.Log($"Distance {data.distance}");

                lerpValue = Mathf.Lerp(lerpValue, data.distance, Time.deltaTime * TargetSpeed);

                float upValue = lerpValue * 0.1f;

                upValue = Mathf.Clamp(upValue, 0f, 9f);

                targetPos = (transform.up * upValue) + Vector3.up;
            }

            Target.localPosition = Vector3.Lerp(Target.localPosition, targetPos, Time.deltaTime * TargetSpeed);
        }
    }


    bool CheckVisibility(Transform t)
    {
        if (t.gameObject.activeInHierarchy && t.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
        {
            //Visible
            return true;
        }
        else
        {
            //NotVisible
            return false;
        }
    }
}
