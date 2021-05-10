using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolType
{
    public bool value;
}

public class MQTTDevice : MonoBehaviour
{
    [Tooltip("Associated gameobject to trigger on animation on/off state")]
    public string triggerAnimation = "";

    [Tooltip("Reverse Boolean operation of state e.g. false = trigger because sometimes false = triggered")]
    public bool reverseTrigger = false;

    private GameObject triggerAnimationGameObject;
    private Animation triggerAnimationAnimation;
    
    public void Start()
    {
        if (triggerAnimation != "")
        {
            triggerAnimationGameObject = GameObject.Find(triggerAnimation);
            triggerAnimationAnimation = triggerAnimationGameObject.GetComponent<Animation>();
        }
    }

    public void MQTTEvent(string payload)
    {
        bool state = false;
        BoolType boolType = null;
        // TODO: This should be cleaned up to deal with more complex types
        try
        {                        
            boolType = JsonUtility.FromJson<BoolType>(payload);
            if (reverseTrigger) {
                state = !boolType.value;
            } else {
                state = boolType.value;
            }
        }
        catch (Exception e)
        {
            Debug.Log("error: " + e);
        }    

        //If gameobject has a light component, match state
        if (this.GetComponent<Light>()) {
            this.GetComponent<Light>().enabled = state;
        }
        // Do the same for any child light entities if they exist
        for(int i=0; i<this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.GetComponent<Light>()) {
                this.transform.GetChild(i).gameObject.GetComponent<Light>().enabled = state;
            }
        }
        // If an triggerAnimation is provided, activate it
        if (triggerAnimation != "")
        {
            AnimateObject(triggerAnimationAnimation, state);
        }
    }

    // Hack to target an animation named 'on' for true, 'off' for false.
    // TODO: kill this
    private void AnimateObject(Animation anim, bool state)
    {            
        string a = "off";
        if(state)
        {
            a = "on";
        }
        anim.Play(a);
    }    
}
