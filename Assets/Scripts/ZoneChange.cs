using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneChange : MonoBehaviour
{
    public bool dark;
    public float transitionDuration = 1f;
    private float baseEnvIntensity;
    private float baseReflIntensity;
    private float baseFogDens;
    private float val;
    // Start is called before the first frame update
    void Start()
    {
        baseEnvIntensity = RenderSettings.ambientIntensity;
        baseReflIntensity = RenderSettings.reflectionIntensity;
        baseFogDens = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (dark)
            //{
            //    RenderSettings.ambientIntensity = 0f;
            //    RenderSettings.reflectionIntensity = 0f;
            //    RenderSettings.fogDensity = 0f;
            //}
            //else
            //{
            //    RenderSettings.ambientIntensity = baseEnvIntensity;
            //    RenderSettings.reflectionIntensity = baseReflIntensity;
            //    RenderSettings.fogDensity = baseFogDens;
            //}

            if (!dark && RenderSettings.ambientIntensity >= 0.9f)
            {
                
            }
            else if (dark && RenderSettings.ambientIntensity <= 0.1f)
            {
                
            }
            else
            {
                StartCoroutine(Timer(!dark));
            }
            
        }
        
    }

    IEnumerator Timer(bool zone)
    {
        float elapsed = 0f;

        if (zone) //light
        {
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                val = Mathf.Clamp01(elapsed / transitionDuration);

                RenderSettings.ambientIntensity = Mathf.Lerp(0f, baseEnvIntensity, val);
                RenderSettings.reflectionIntensity = Mathf.Lerp(0f, baseReflIntensity, val);
                RenderSettings.fogDensity = Mathf.Lerp(0f, baseFogDens, val);
                print("reeeee");
                yield return null;
            }
        }
        else //dark
        {
            elapsed = transitionDuration;
            while (elapsed > 0f)
            {
                elapsed -= Time.deltaTime;
                val = Mathf.Clamp01(elapsed / transitionDuration);

                RenderSettings.ambientIntensity = Mathf.Lerp(0f, baseEnvIntensity, val);
                RenderSettings.reflectionIntensity = Mathf.Lerp(0f, baseReflIntensity, val);
                RenderSettings.fogDensity = Mathf.Lerp(0f, baseFogDens, val);
                print("reeeee");
                yield return null;
            }
        }
        
        
    }
}
