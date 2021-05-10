using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTimeout : MonoBehaviour
{
    public bool IsActive = false;
    
    void OnEnable()
    {
        //Start the coroutine
        //disable self after 5 seconds if not already performing active        
        if (IsActive == false)
        { 
            StartCoroutine(DisableSelf(5.0f));
        }
    }

    IEnumerator DisableSelf(float waitTime)
    {
        //Debug.Log($"DisableSelf: {this.gameObject}\n");
        //Wait for `waitTime` seconds
        yield return new WaitForSeconds(waitTime);
        //Deactivate
        this.gameObject.SetActive(false);
        IsActive = false;
    }    
}