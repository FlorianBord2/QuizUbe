from flask import Flask
from createQuiz import create_quiz
from ourYoutube import yapi as ya
import json

app = Flask(__name__)

cq = create_quiz()
yapi = ya()


@app.route('/search_channel/<q>')
def search_channel(q):
    res = yapi.youtube_search_channel(q)
    res = json.dumps(res)
    return res


@app.route('/get_quiz/<channelId>')
def get_quiz(channelId):
    res = cq.create(channelId)
    return str(res)
