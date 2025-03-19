// Use "Internal signal" by default. 
USE_OSC = false;

// Effect and layer.
layerIndexOrName = "PING";
effectName = "Loop In Space timed";

// Project FPS: would be great to have an input from Mlillumiin for this.
fps = 60.

// Interval at which we update our clock.
interval = 1000. / fps;

// Final clock to increment according to 'speed' and at given interval.
clock = [0., 0.];
speed = [0., 0.];

onInternalSignalEvent = function(event)
{
    if (USE_OSC == false) {
        if (event.fromDataTrack == "INTERNAL_SPEED_X") {
            speed[0] = parseFloat(event.values[0]) / 1000. * -1.;
        }
        
        if (event.fromDataTrack == "INTERNAL_SPEED_Y") {
            speed[1] = parseFloat(event.values[0]) / 1000. * -1.;
        }
    }
}

// FIXME: Doesn't work in 'Continuous Mode'.
onOSCEvent = function(event)
{
    if (USE_OSC == true) {
        if (event.address == '/renaud/list_x') {
            speed[0] = parseFloat(event.values[0]) / 1000. * -1.;
        }
            
         if (event.address == '/renaud/list_y') {
            speed[1] = parseFloat(event.values[0]) / 1000. * -1.;
        }
    }
}


setInterval(function () {
    clock[0] += (interval * speed[0]);
    clock[1] += (interval * speed[1]);
    Millumin.setEffectProperty(layerIndexOrName, effectName, "clock", clock);
}, interval);
