using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RocketInterface : MonoBehaviour 
{
    public Text lifeLabel;
    public Text shotsLabel;
    public Text fragsLabel;
    public Text scoreLabel;

    public void UpdateLife(int life)
    {
        StartCoroutine(UpdateLifeAnimated(life));
    }

    public void UpdateDPS(float rate)
    {
        shotsLabel.text = (1.0f / rate).ToString("#.#") + " DPS";
    }

    public void UpdateFrag(int frags)
    {
        fragsLabel.text = frags.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.text = score.ToString();
    }

    IEnumerator UpdateLifeAnimated(int until)
    {
        lifeLabel.text = lifeLabel.text.Replace('%', ' ');
        int cur = int.Parse(lifeLabel.text);
        while (cur != until)
        {
            if (until < cur)
                cur -= (int)Mathf.Ceil(Mathf.Abs(cur - until) * 0.5f);
            else if (until > cur)
                cur += (int)Mathf.Ceil(Mathf.Abs(cur - until) * 0.5f);

            lifeLabel.text = cur.ToString() + "%";

            yield return new WaitForSeconds(0.01f);
        }
    }
}
