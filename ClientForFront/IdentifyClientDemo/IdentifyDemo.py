#!/usr/bin/python3
import Conf
from DataModel import *
import HttpHelper

'''此文件是数据访问接口的测试样例程序，提供简单的接口访问方法演示，核心是http的使用'''


# demo
# 获取房间信息
def get_rooms_demo():
    rooms = HttpHelper.get_items(Conf.Urls.RoomInfoUrl)
    for room in rooms:
        print(room.name)


# 新建一个房间
def create_new_room_demo():
    room = RoomInfo(id=0, name='测试', roomSize=25)
    result = HttpHelper.create_item(Conf.Urls.RoomInfoUrl, room)
    print(result)


# 更改房间名称
def update_room_name_demo():
    rooms = HttpHelper.get_items(Conf.Urls.RoomInfoUrl)
    for room in rooms:
        if room.name == '测试':
            # 将room的信息复制给new_room，但是将name值修改为‘测试1’
            new_room = RoomInfo(id=room.id, name='测试1', roomSize=room.roomSize, agesInfos=room.agesInfos, cameraInfos=room.cameraInfos)
            HttpHelper.update_item(Conf.Urls.RoomInfoUrl, room.id, new_room)
            break


# 删除房间
def delete_room_demo():
    HttpHelper.delete_item(Conf.Urls.RoomInfoUrl, 1)


get_rooms_demo()
