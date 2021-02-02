from flask import Flask
from flask import request
from flask import Response
from createQuiz import create_quiz
from ourYoutube import yapi as ya
from firebase import fb as fbapi

import json

app = Flask(__name__)

cq = create_quiz()
yapi = ya()
fb = fbapi()


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

#Gestion des users

@app.route('/users/register')
def register():
    if request.method == 'GET':
        try:
            password = request.headers['password']
            email = request.headers['email']    
        except:
            return Response('Bad parameter, make sure you have "password" and "email" param in your header', status=400, mimetype='application/json')
        password = request.headers['password']
        email = request.headers['email']
        return fb.register(email, password)
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
        password = request.headers['password']
        email = request.headers['email']
        return fb.login(email, password)
    else:
        return 'Wrong method'

@app.route('/users/reset_password')
def reset_password():
    if request.method == 'GET':
        try:
            email = request.headers['email']    
        except:
            return Response('Bad parameter, make sure you have "email" param in your header', status=400, mimetype='application/json')
        email = request.headers['email']
        return fb.reset_password(email)
    else:
        return 'Wrong method'

@app.route('/users/verify_mail')
def verify_mail():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" param in your header', status=400, mimetype='application/json')
        userIdToken = request.headers['userIdToken']
        return fb.verify_mail(userIdToken)
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
        return fb.get_account_info(userIdToken)
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
        return fb.refresh_token(refreshToken)
    else:
        return 'Wrong method'