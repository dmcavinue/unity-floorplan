using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MQTT : MonoBehaviour
{
   [Tooltip("MQTT host")]
   public string host = "mqtt.example.com";

   [Tooltip("MQTT websocket port")]
   public int port = 8443;

   [Tooltip("MQTT username")]
   public string user = "mqtt";

   [Tooltip("MQTT password")]
   public string password = "password";

    [DllImport("__Internal")] public static extern void MQTTConnect(string host, int port, string user, string password);

#if UNITY_WEBGL && !UNITY_EDITOR 
   void Start()
   {
      MQTTConnect(host, port, user, password);
   }   
#endif
}
