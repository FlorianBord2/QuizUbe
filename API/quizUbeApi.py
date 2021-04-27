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
        res = cq.create(request.headers['channelId'], request.headers['channelName'], request.headers['channelUrl'])
        return res
    else:
        return 'Wrong method'

@app.route('/quiz/save_quiz', methods=['POST'])
def save_quiz():
    if request.method == 'POST':
        #try:
        quiz = request.json
        f.save_quiz(quiz)
        return 'ok'
        #except:
        #    return Response('Error', status=400, mimetype='application/json')
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

@app.route('/quiz/defis', methods=['POST'])
def add_defis():
    if request.method == 'POST':
        userLocalId = request.headers['userLocalId']
        cibleName = request.headers['cibleName']
        cibleId = request.headers['cibleId']
        quiz = request.json
        return f.add_defis(quiz, userLocalId, cibleId)
        # except:
        #     return Response('Bad parameter, make sure you have "quizUuid" and "userLocalId" param in your header', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

@app.route('/quiz/get_defis_list', methods=['GET'])
def get_defis():
    if request.method == 'GET':
        userLocalId = request.headers['userLocalId']
        return f.get_defis(userLocalId)
        # except:
        #     return Response('Bad parameter, make sure you have "quizUuid" and "userLocalId" param in your header', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

@app.route('/quiz/get_defis_quiz', methods=['GET'])
def get_defis_quiz():
    if request.method == 'GET':
        fromId = request.headers['fromId']
        uuid = request.headers['uuid']
        return f.get_defis_quiz(fromId, uuid)
        # except:
        #     return Response('Bad parameter, make sure you have "quizUuid" and "userLocalId" param in your header', status=400, mimetype='application/json')
    else:
        return 'Wrong method'

@app.route('/quiz/save_defis', methods=['POST'])
def save_defis():
    if request.method == 'POST':
        userLocalId = request.headers['userLocalId']
        quiz = request.json
        return f.save_defis(userLocalId, quiz)
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
            username = request.headers['username']
            if f.chekc_username(username) == 'ok':
                dispo = True
            else:
                return Response('username taken', status=400, mimetype='application/json')
            user = ue(request.headers['email'], request.headers['password'])
            res = user.register(auth)
            print(res)
            f.db.child("users").child(username).set(res['localId'])
        except:
            return 'Error'
        return res
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

@app.route('/users/friend_list')
def friend_list():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" param in your header', status=400, mimetype='application/json')
        return f.friendList(userIdToken)
    else:
        return 'Wrong method'

@app.route('/users/add_friend')
def add_friend():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']
            friend_name = request.headers['friend_username']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" and "friend_username" param in your header', status=400, mimetype='application/json')
        return f.addFriend(userIdToken, friend_name)
    else:
        return 'Wrong method'

@app.route('/users/accept_friend')
def accept_friend():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']
            friend_name = request.headers['friend_username']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" and "friend_username" param in your header', status=400, mimetype='application/json')
        return f.acceptFriend(userIdToken, friend_name)
    else:
        return 'Wrong method'

@app.route('/users/refuse_friend')
def refuse_friend():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']
            friend_name = request.headers['friend_username']    
        except:
            return Response('Bad parameter, make sure you have "userIdToken" and "friend_username" param in your header', status=400, mimetype='application/json')
        return f.refuseFriend(userIdToken, friend_name)
    else:
        return 'Wrong method'

@app.route('/users/get_pending_list')
def get_pending_list():
    if request.method == 'GET':
        try:
            userIdToken = request.headers['userIdToken']
        except:
            return Response('Bad parameter, make sure you have "userIdToken" param in your header', status=400, mimetype='application/json')
        res = f.getPendingList(userIdToken)
        if res == None:
            return Response('{}', status=200, mimetype='application/json')
        else:
            return res
    else:
        return 'Wrong method'

@app.route('/users/leadersboard')
def get_leaders():
    if request.method == 'GET':
        res = f.getLeaders()
        if res == None:
            return Response('{}', status=200, mimetype='application/json')
        else:
            return res
    else:
        return 'Wrong method'