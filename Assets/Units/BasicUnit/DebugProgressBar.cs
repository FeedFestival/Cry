using UnityEngine;
using System.Collections;

public class DebugProgressBar : MonoBehaviour
{
    public bool debuging;

    BasicAtack BA;

    public GameObject bar;
    public GameObject Endbar;

    public Material impact;
    public Material complete;

    // Duration
    float endDuration = 0.5f;
    float smoothness = 0.01f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.
    //Color currentColor = Color.white; // This is the state of the color in the current interpolation.

    Vector3 DebugStartValue;
    Vector3 DebugEndValue;

    // Step atak
    Vector3 StepAtackStartValue;
    Vector3 StepAtackEndValue;

    void Start()
    {
        DebugStartValue = new Vector3(0.5f, 0.5f, 0);
        DebugEndValue = new Vector3(0.5f, 0.5f, 1.45f);
        ResetCooldown();
    }

    bool once = true;
    // Update is called once per frame
    void Update()
    {
        /*  
         * For Test !
         */
        if (Input.GetKeyDown(KeyCode.V) && once)
        {
            StartCoolDown();
            once = false;
        }
    }

    public void ResetCooldown()
    {
        once = true;
        bar.transform.localScale = DebugStartValue;
        bar.SetActive(false);
        Endbar.SetActive(false);

        if (BA)
            BA.AtackEnded();
    }
    public void StartCoolDown(float firstDuration = 1f, float secondDuration = 1f, BasicAtack ba = null)
    {
        if (ba)
        {
            BA = ba;
        }

        bar.SetActive(true);
        Endbar.SetActive(true);

        /*
         * If the second duration its 0 then we only wait for cooldown once.
         */
        endDuration = secondDuration;

        // Calculate the step Atack
        StepAtackStartValue = BA.gameObject.transform.position;
        StepAtackEndValue = BA.gameObject.transform.position + BA.gameObject.transform.forward;

        StartCoroutine(LerpAtackCoolDown(firstDuration));
    }
    IEnumerator LerpAtackCoolDown(float Duration , bool endCoolDown = false)
    {
        if (endCoolDown || endDuration == 0)
            bar.GetComponent<MeshRenderer>().material = complete;
        else
            bar.GetComponent<MeshRenderer>().material = impact;

        var i = 0.0f;
        var rate = 1.0f / Duration;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;

            if(debuging)
                bar.transform.localScale = Vector3.Lerp(DebugStartValue, DebugEndValue, i);

            //BA.gameObject.transform.position = Vector3.Lerp(StepAtackStartValue, StepAtackEndValue,i);

            yield return new WaitForSeconds(smoothness);
        }
        if (endCoolDown || endDuration == 0)
            ResetCooldown();
        else
            StartCoroutine(LerpAtackCoolDown(endDuration, true));
    }
}
