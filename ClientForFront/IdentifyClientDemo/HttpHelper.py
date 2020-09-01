#!/usr/bin/python3
import requests
import json
from ObjectJsonHelper import *


'''此文件是提供一些Http访问的常用方法，关于调试可以用PostmanCanary进行接口测试，
一般来说Http方法的返回结果是200多的话是执行成功，以400或者500开始的结果是有错误发生。'''


# 获取数据记录
def get_items(url):
    response = requests.get(url)
    items = json.loads(response.content, object_hook=model_decoder)
    return items


# 获取指定Id的数据记录信息
def get_item_by_id(url, select_id):
    response = requests.get(url+f"/{select_id}")
    item = json.loads(response.content, object_hook=model_decoder)
    # print(items)
    return item


# get带参数的url
# 例如 http://154.8.225.243/api/serverinfoes/GetServerInfo?ip=192.168.1.60 是获取ip=192.168.1.60的服务器记录
def get_item_by_param(url, param):
    response = requests.get(url+f"?{param}")
    items = json.loads(response.content, object_hook=model_decoder)
    # print(items)
    return items


# 新建数据记录
def create_item(url, obj):
    json_str = json.dumps(obj, cls=ModelEncoder)
    header = {'Content-Type': 'application/json'}
    return requests.post(url, json_str, headers=header)


# 更新数据记录
def update_item(url, update_item_id, obj):
    json_str = json.dumps(obj, cls=ModelEncoder)
    header = {'Content-Type': 'application/json'}
    return requests.put(url+f"/{update_item_id}", json_str, headers=header)


# 删除数据记录,谨慎使用，数据库设置为删除记录时会自动删除相关的所有记录！！！
def delete_item(url, delete_item_id):
    return requests.delete(url + f"/{delete_item_id}")
