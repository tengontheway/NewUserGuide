using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class ShowFps : MonoBehaviour {

    public float frequency = 20f;

    private Text text;
    private float timer = 0f;
    private int count = 0;
    private float min = 60f;

    private StringBuilder sb;
    
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        sb = new StringBuilder();
        Application.targetFrameRate = 40;
	}
	
	// Update is called once per frame
	void Update () {

        sb.Remove(0, sb.Length);

        timer += Time.deltaTime * Time.timeScale;
        count++;

        if (count == frequency)
        {
            sb.Append(" Current FPS : ");
            float cur = frequency / timer;
            sb.Append(cur.ToString("#0.0"));
            sb.Append(", Min FPs : ");
            if (Time.frameCount > 50)
            {
                if (cur < min)
                {
                    min = cur;
                } 
            }
            sb.Append(min.ToString("#0.0"));
            text.text = sb.ToString();
            count = 0;
            timer = 0f;
        }
	}
}
