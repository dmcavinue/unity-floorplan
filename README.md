#### Unity Floorplan Example

Very basic demonstration of a webgl unity build that is capable of connecting to a MQTT broker via a websocket to subscribe and represent devices state in real time.
In my case, I have use both zigbee and zwave devices for my lights, motion, door and window sensors.  This demonstrates some of the basic logic but drops the sensitive stuff like my home floorplan.  I have included my simple blender door model with animations that can be called within unity.  I have it deployed and embedded directly in the home assistant UI, also displaying on my magic mirror when an events triggers, monitoring about 20 devices without issue. My personal setup is alot more complex but I wanted to drop something public facing in case its useful for anyone else.

**Disclaimer:** This is most definitely extremely rough around the edges, I'll be cleaning things up in shortly.

#### Notes
`./Assets/Plugins/WebGL/MQTT.cs`: this script is attached to the `mqtt` entity and configured with the url/port/credentials to connect to my broker over a websocket. 

`./Assets/Plugins/WebGL/MQTT.jslib`: This is what allows the javascript client to initiate the MQTT connection and subscribe to the necessary topics.  I have an important `TODO` here to relocate the devices list out of here and genrally allow for the abstracting these out to environmental variables. At the moment, the `_Devices` dictonary in this file is what ties gameobjects to topics. E.g.
```
_Devices = {
  "some/mqtt/topic": "some-gameobject-name",
  ...
}
```
When a message arrives for an objects given subscribed topic, The `MQTTEvent` function under `MQTTDevice` will be called.

`./Assets/Scripts/MQTTDevice.cs`: Thi script is associated with each gameobject we want to trigger. You can optionally provide the name of a gameobject to trigger animation for here, if one is provided, on trigger, that gameobjects `on` and `off` animations will be triggered on state `true` or `false`. I'll also clean this up eventually. In my setup, I have tweaked this to include Virtual Camera triggers too, to have my scene pan the game camera as events occur.

`./Assets/Scripts/SensorTimeout.cs`: More of a hack to disable things like lights on load without disabling the component.  I'm sure there is a better way to do this :shrug:.

#### Build Notes
This code also includes a webgl template that the sample code should be pointing at on build.  If you perform a webgl build to and output `./build`, you _should_ be able to perform a simple `docker-compose build` follow by `docker-compose up` to execute it.  Once its alive, it should be reachable locally at `http://localhost:8080`. 