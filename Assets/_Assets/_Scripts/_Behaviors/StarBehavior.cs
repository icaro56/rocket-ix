using UnityEngine;
using System.Collections;

public class StarBehavior : MonoBehaviour {
	float time = 0;
	bool expanding = true;
    float start = 0;
    float end = 0.5f;
    float ticks = 1/4;

    bool incandecente = false;

	// Troquei o update pelo fixedUpdate
	void FixedUpdate () 
	{
        if (incandecente)
        {
            if (expanding)
            {
                time += Time.fixedDeltaTime * ticks;
                if (time >= end) expanding = false;
            }
            else
            {
                time -= Time.fixedDeltaTime * ticks;
                if (time <= start) expanding = true;
            }
            GetComponent<Renderer>().material.SetFloat("incandecencia", time);
        }
	}

    public void SetIncadecenciaRange(float init, float finish, float duration)
    {
        start = init;
        end = finish;
        ticks = 1/duration;
    }

    public void Randomize(int faixa)
    {
        switch (faixa)
        {
            case 1: Randomize1(); break;
            case 2: Randomize2(); break;
            case 3: Randomize3(); break;
        }
    }

    void Randomize1()
    {
        float grayFactor = Random.Range(0.3f, 0.6f);
        GetComponent<Renderer>().material.SetColor("MainColor", new Color(grayFactor, grayFactor, grayFactor));

        float size = Random.Range(1.3f, 1.6f);
        transform.localScale = new Vector3(size, size, size);
        SetIncadecenciaRange(0, Random.Range(0.1f, 0.4f), Random.Range(5, 15));
        incandecente = true;
    }

    void Randomize2()
    {
        Color[] colors = new Color[] { 	new Color(1.0f, 1.0f, 1.0f), 
                                        new Color(0.8f, 0.8f, 0.8f), 
                                        new Color(0.6f, 0.6f, 0.6f), 
                                        new Color(0.5f, 0.5f, 0.5f),
                                        
                                        new Color(0.5f, 1.0f, 1.0f), 
                                        new Color(0.4f, 0.8f, 0.8f), 
                                        new Color(0.3f, 0.6f, 0.6f), 
                                        new Color(0.2f, 0.5f, 0.5f),
                                        
                                        /*new Color(1.0f, 0.5f, 1.0f), 
                                        new Color(0.8f, 0.4f, 0.8f), 
                                        new Color(0.6f, 0.3f, 0.6f), 
                                        new Color(0.5f, 0.2f, 0.5f)*/};

        GetComponent<Renderer>().material.SetColor("MainColor", colors[Random.Range(0,8)]);

        float size = Random.Range(1.7f, 2.0f);
        transform.localScale = new Vector3(size, size, size);
        SetIncadecenciaRange(0, Random.Range(0.2f, 0.6f), Random.Range(4, 8));
        incandecente = true;
        transform.Rotate(Vector3.forward, Random.Range(0, 180));
    }

    void Randomize3()
    {
        Color[] colors = new Color[] { 	new Color(1.0f, 1.0f, 0.3f), 
                                        new Color(0.8f, 0.8f, 0.2f), 
                                        new Color(0.6f, 0.6f, 0.1f), 
                                        new Color(0.5f, 0.5f, 0.05f),
                                        
                                        new Color(0.3f, 1.0f, 1.0f), 
                                        new Color(0.2f, 0.8f, 0.8f), 
                                        new Color(0.1f, 0.6f, 0.6f), 
                                        new Color(0.05f, 0.5f, 0.5f),
                                        
                                        /*new Color(1.0f, 0.3f, 1.0f), 
                                        new Color(0.8f, 0.2f, 0.8f), 
                                        new Color(0.6f, 0.1f, 0.6f), 
                                        new Color(0.5f, 0.05f, 0.5f)*/};

        GetComponent<Renderer>().material.SetColor("MainColor", colors[Random.Range(0, 8)]);

        float size = Random.Range(1.8f, 2.2f);
        transform.localScale = new Vector3(size, size, size);
        SetIncadecenciaRange(0, Random.Range(0.4f, 1.0f), Random.Range(4, 8));
        incandecente = true;
        transform.Rotate(Vector3.forward, Random.Range(0, 180));
    }
}
