import sys
sys.path.insert(1, '../Database')

from flask import Flask
from flask import request
from flask import Response
from createQuiz import create_quiz as create_quiz_class
from ourYoutube import yapi as ya
from firebase import Firebase as fb
from user import User as ue

import json

app = Flask(__name__)


yapi = ya()
f = fb()
f.is_running()
auth = f.get_auth()

@app.route('/quiz/search_channel', methods=['GET'])
def search_channel():
    if request.method == 'GET':
        res = yapi.youtube_search_channel(request.args['q'])
        res = json.dumps(res)
        return res
    else:  
        return 'Wrong method'

@app.route('/quiz/create_quiz', methods=['GET'])
def create_quiz():
    if request.method == 'GET':
        cq = create_quiz_class()
        res = cq.create(request.args['channelId'])
        return res
    else:
        return 'Wrong method'

@app.route('/quiz/save_quiz', methods=['POST'])
def save_quiz():
    if request.method == 'POST':
        try:
            quiz = request.json
            f.save_quiz(quiz)
            return 'ok'
        except:
            return Response('Error', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

@app.route('/quiz/get_quiz_histo', methods=['GET'])
def get_quiz_histo():
    if request.method == 'GET':
        try:
            userLocalId = request.headers['userLocalId']
            return f.get_quiz_histo(userLocalId)   
        except:
            return Response('Bad parameter, make sure you have "userLocalId" param in your header', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

@app.route('/quiz/get_quiz', methods=['GET'])
def get_quiz():
    if request.method == 'GET':
        quizuid = request.headers['quizUuid']
        userLocalId = request.headers['userLocalId']
        return f.get_quiz(quizuid, userLocalId)
        # except:
        #     return Response('Bad parameter, make sure you have "quizUuid" and "userLocalId" param in your header', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

#Gestion des users

@app.route('/users/register')
def register():
    if request.method == 'GET':
        try:
            password = request.headers['password']
            email = request.headers['email']    
        except:
            return Response('Bad parameter, make sure you have "password" and "email" param in your header', status=400, mimetype='application/json')
        user = ue(request.headers['email'], request.headers['password'])
        return user.register(auth)
    else:
        return 'Wrong method'

@app.route('/users/login')
def login():
    if request.method == 'GET':
        try:
            password = request.headers['password']
            email = request.headers['email']    
        except:
            return Response('Bad parameter, make sure you have "password" and "email" param in your header', status=400, mimetype='application/json')
        user = ue(request.headers['email'], request.headers['password'])
        return user.login(auth)
    else:
        return 'Wrong method'

@app.route('/users/reset_password')
def reset_password():
    if request.method == 'GET':
        try:
            email = request.headers['email']    
        except:
            return Response('Bad parameter, make sure you have "email" param in your header', status=400, mimetype='application/json')
        user = ue(request.headers['email'], '*********')
        return user.reset_password(auth)
    else:
        return 'Wrong method'

@app.route('/users/verify_mail')
def verify_mail():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" param in your header', status=400, mimetype='application/json')
        return f.verify_email(userIdToken)
    else:
        return 'Wrong method'

@app.route('/users/get_account_info')
def get_account_info():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" param in your header', status=400, mimetype='application/json')
        userIdToken = request.headers['userIdToken']
        return f.get_user_info(userIdToken)
    else:
        return 'Wrong method'
    
@app.route('/users/refresh_token')
def refresh_token():
    if request.method == 'GET':
        try:
            refreshToken = request.headers['refreshToken']    
        except:
            return Response('Bad parameter, make sure you have "refreshToken" param in your header', status=400, mimetype='application/json')
        refreshToken = request.headers['refreshToken']
        return f.refresh_token(refreshToken)
    else:
        return 'Wrong method'