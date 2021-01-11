#
#  IoT_thermometer.py
#
# Created by Yisheng Li on 2021/1/7.
#

import http.client
import urllib.parse
import requests
from time import sleep

from sense_hat import SenseHat
from smbus2 import SMBus
from mlx90614 import MLX90614


deviceID = "0435efaae76c81"

w = (255, 255, 255)
g = (0, 255, 0)
o = (255, 165, 0)
r = (255, 0, 0)


def show_color(c):
    sense = SenseHat()
    image = [
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c,
    c,c,c,c,c,c,c,c]
    sense.set_pixels(image)


def check_onOff(device_ID):
    r = requests.post("http://ivmsp.us-east-1.elasticbeanstalk.com/Home/incomingVisitorDetect", params ={'deviceID': device_ID})
    on_off = r.json()["result"] == "true"
    return on_off

def send_temp(device_ID, temp):
    r = requests.post("http://ivmsp.us-east-1.elasticbeanstalk.com/Home/visitorTemperatureUpdate", params ={'deviceID': device_ID,'temperature': temp})
    result = r.json()["result"]
    print(result)
    return result

def take_current_temp():
    valid_temps = []
    lastOne = -1
    bus = SMBus(1)
    sensor = MLX90614(bus, address=0x5A)
    
    for i in range(20):
        print(i)
        
        if i == lastOne:
            print(valid_temps)
            return max(valid_temps)
        
        temp = round(sensor.get_object_1(),1)
        
        if temp > 36 and temp < 41:
            # when t in valid range, take a few temps more to make it accurate
            if lastOne == -1:
                lastOne = i + 4 
            valid_temps.append(temp)
        
    
        print("Object Temperature :", temp)
        sleep(0.5)

    bus.close()
    return -1


def start():
    sense = SenseHat()
    sense.set_rotation(90)
    while True:
        show_color(w)
        if check_onOff(deviceID):
            show_color(g)
            t = take_current_temp()
            print("t is:", t)
            
            if t > 36 and t <= 37.1:
                t_color = g
            elif t > 37.1 and t <= 38:
                t_color = o
            elif t > 38:
                t_color = r
            else:
                t_color = w
            
            
            t_str = str(t)
            sense.show_message(t_str, text_colour = t_color, scroll_speed = 0.12)
            sense.show_message(t_str, text_colour = t_color, scroll_speed = 0.12)
            send_temp(deviceID,t_str) 
   
    
if __name__ == '__main__':
    start()

