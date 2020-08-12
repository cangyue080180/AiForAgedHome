#!/usr/bin/python3
import requests
import json
from ObjectJsonHelper import *


# 获取数据记录
def get_items(url):
    response = requests.get(url)
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
    requests.put(url+f"/{update_item_id}", json_str, headers=header)


# 删除数据记录,谨慎使用，数据库设置为删除记录时会自动删除相关的所有记录！！！
def delete_item(url, delete_item_id):
    response = requests.delete(url + f"/{delete_item_id}")