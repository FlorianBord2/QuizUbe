from flask import Flask
from flask import request
from createQuiz import create_quiz
from ourYoutube import yapi as ya
import json

app = Flask(__name__)

cq = create_quiz()
yapi = ya()


@app.route('/search_channel', methods=['GET'])
def search_channel():
    if request.method == 'GET':
        res = yapi.youtube_search_channel(request.args['q'])
        res = json.dumps(res)
        return res
    else:  
        return 'Wrong method'

@app.route('/get_quiz', methods=['GET'])
def get_quiz():
    if request.method == 'GET':
        res = cq.create(request.args['channelId'])
        return res
    else:
        return 'Wrong method'
