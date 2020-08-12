#!/usr/bin/python3
from enum import Enum


class AgesInfo:
    def __init__(self, id, name, contacterName, contacterPhone, nurseName, address, roomInfoId, roomINfo, poseInfos):
        self.id = id
        self.name = name
        self.contactername = contacterName
        self.contacterphone = contacterPhone
        self.nursename = nurseName
        self.address = address
        self.roominfoid = roomInfoId
        self.roominfo = roomINfo
        self.poseinfos = poseInfos


class CameraInfo:
    def __init__(self, id, factoryInfo, ipAddress, videoAddress, serverInfoId, serverInfo, roomInfoId, roomInfo):
        self.id = id
        self.factoryinfo = factoryInfo
        self.ipaddress = ipAddress
        self.videoaddress = videoAddress
        self.serverinfoid = serverInfoId
        self.serverinfo = serverInfo
        self.roominfoid = roomInfoId
        self.roominfo = roomInfo


class PoseInfo:
    def __init__(self, agesInfoId, agesInfo, date, timeStand, timeSit, timeLie, timeDown, timeOther, timeIn, isAlarm,
                 status):
        self.agesinfoid = agesInfoId
        self.agesinfo = agesInfo
        self.date = date  # formart is "2020_08_05T00:00:00"
        self.timestand = timeStand
        self.timesit = timeSit
        self.timelie = timeLie
        self.timedown = timeDown
        self.timeother = timeOther
        self.timein = timeIn
        self.isalarm = isAlarm
        self.status = status


class PoseStatus(Enum):
    Stand = 0
    Sit = 1
    Lie = 2
    Down = 3
    Other = 4


class RoomInfo:
    def __init__(self, id, name, roomSize, agesInfos=None, cameraInfos=None):
        self.id = id
        self.name = name
        self.roomsize = roomSize
        self.agesinfos = agesInfos
        self.camerainfos = cameraInfos


class ServerInfo:
    def __init__(self, id, name, factoryInfo, maxCameraCount, ip, cameraInfos):
        self.id = id
        self.name = name
        self.factoryinfo = factoryInfo
        self.maxcameracount = maxCameraCount
        self.ip = ip
        self.camerainfos = cameraInfos
