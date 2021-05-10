var MQTTPlugin = {
   $Devices: null,
   $Client: null,
   NewClient__deps: ['Client'],
   NewClient: function (host, port, cname) {
      _Client = new Paho.MQTT.Client(host,port,cname);
      return _Client
   },
   Subscribe__deps: ['Client','Devices'],
   Subscribe: function () {      
      //console.log("Connected");
      Object.keys(_Devices).forEach(function(key) {
         //console.log(_Devices[key] + ": subscribing to topic: "+ key);
         _Client.subscribe(key);      
      });
   },
   MQTTConnect__deps: ['Subscribe','NewClient','Client', 'Devices', 'OnConnectionLost', 'OnMessageArrived'],
   MQTTConnect: function (h, port, u, pwd) {
      var host = UTF8ToString(h);
      var user = UTF8ToString(u);
      var password = UTF8ToString(pwd);
      // TODO: Make dynamic and to config
      _Devices = {
        // doors
        "hass/contact_frontdoor/48/0/Any": "door-1",
         // lights
        "hass/porch/37/0/currentValue": "light-1"
      }
      //console.log("connecting to "+ host +" "+ port);
      var x=Math.floor(Math.random() * 10000);
      var cname="orderform-"+x;
      _Client = _NewClient(host, port, cname);
      var options = {
         useSSL:true,
         timeout: 3,
         userName: user,
         password: password,
         onSuccess: _Subscribe
      };
      _Client.onConnectionLost = _OnConnectionLost;
      _Client.onMessageArrived = _OnMessageArrived;
      _Client.connect(options);
   },
   OnConnectionLost: function (responseObject) {
     console.log("onConnectionLost:" + responseObject.errorMessage);
   },
   OnMessageArrived__deps: ['Devices'],
   OnMessageArrived: function (message) {
     unityGame.SendMessage(_Devices[message.destinationName], 'MQTTEvent', message.payloadString);
   }
};
mergeInto(LibraryManager.library, MQTTPlugin);