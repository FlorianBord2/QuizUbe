import pyrebase
import requests
import json
import sys

def noquote(s):
    return s
pyrebase.pyrebase.quote = noquote


class Firebase:
    def __init__(self):
        self.running = True
        config = {
            "apiKey": "AIzaSyAY123nmo2Yfa5D4JUsqkIMMZQixL8c_5o",
            "authDomain": "quizube.firebaseapp.com",
            "databaseURL": "https://quizube-default-rtdb.europe-west1.firebasedatabase.app",
            "projectId": "quizube",
            "storageBucket": "quizube.appspot.com",
            "messagingSenderId": "118956939660",
            "appId": "1:118956939660:web:e9861eafa9d7a1804a9ea9"
        }
        try:
            self.firebase = pyrebase.initialize_app(config)
            self.auth = self.firebase.auth()
            self.db = self.firebase.database()
        except Exception as e:
            self.running = e

    # Databse tools

    @staticmethod
    def http_error(exception):
        error_json = exception.args[1]
        error = json.loads(error_json)['error']
        return error

    def is_running(self):
        if self.running == True:
            return True
        raise self.running

    def get_auth(self):
        return self.auth

    def get_db(self):
        return self.db

    # User tools
    def findName(self, userIdToken):
        users = self.db.child('users').get()
        username = None
        for user in users.each ():
            if user.val() == userIdToken:
                username = user.key()
        return username

    def chekc_username(self, username):
        users = self.db.child("users").child(username).get().val()
        print(users)
        if users == None:
            return 'ok'
        else:
            return "taken"

    def verify_email(self, user_id_token):
        try:
            return self.auth.send_email_verification(user_id_token)
        except requests.exceptions.HTTPError as e:
            self.userIdToken = ""
            print("Can't verify email address: {}".format(e), sys.stderr)
            return self.http_error(e)

    def get_user_info(self, user_id_token):
        try:
            return self.auth.get_account_info(user_id_token)
        except requests.exceptions.HTTPError as e:
            print("Can't retrieve acccount info: {}".format(e), sys.stderr)
            return self.http_error(e)
    
    def refresh_token(self, refresh_token):
        try:
            return self.auth.refresh(refresh_token)
        except requests.exceptions.HTTPError as e:
            refresh_token = ""
            print("Can't verify email address: {}".format(e), sys.stderr)
            return self.http_error(e)

    def friendList(self, userIdToken):
        plist = self.db.child(userIdToken).child('friends').get().val()
        if plist == None:
            return {}
        users = []
        myformat = {"name" : "",
            "userIdToken": ""}
        for each in plist:
            myformat['name'] = each
            myformat['userIdToken'] = plist[each]
            users.append(myformat)
        return json.dumps(users)

    def addFriend(self, userIdToken, friendUsername):
        f_idToken = self.db.child("users").child(friendUsername).get().val()
        print(f_idToken)
        if (f_idToken == None):
            return "-1"
        users = self.db.child('users').get()
        username = None
        for user in users.each ():
            if user.val() == userIdToken:
                username = user.key()
        print(username)
        self.db.child(f_idToken).child('pending_friend').child(username).set(userIdToken)
        return "1"

    def acceptFriend(self, userIdToken, friendUsername):
        f_idToken = self.db.child(userIdToken).child('pending_friend').child(friendUsername).get().val()
        if (f_idToken == None):
            return('')
        self.db.child(userIdToken).child('pending_friend').child(friendUsername).remove()
        self.db.child(userIdToken).child('friends').child(friendUsername).set(f_idToken)
        users = self.db.child('users').get()
        username = None
        for user in users.each ():
            if user.val() == userIdToken:
                username = user.key()
        self.db.child(f_idToken).child('friends').child(username).set(userIdToken)
        return 'Friend added'
    
    def refuseFriend(self, userIdToken, friendUsername):
        f_idToken = self.db.child(userIdToken).child('pending_friend').child(friendUsername).get().val()
        if (f_idToken == None):
            return('')
        self.db.child(userIdToken).child('pending_friend').child(friendUsername).remove()
        return 'Friend refuse'
    
    def getPendingList(self, userIdToken):
        plist  = self.db.child(userIdToken).child('pending_friend').get().val()
        if plist == None:
            return {}
        users = []
        myformat = {"name" : "",
            "userIdToken": ""}
        for each in plist:
            myformat['name'] = each
            myformat['userIdToken'] = plist[each]
            users.append(myformat)
        return json.dumps(users)

    #Data management

    def save_quiz(self, data):
        print(data)
        quiz_histo = {
            'channelName' : data['channelName'],
            'channelUrl': data['channelUrl'],
            'date': data['date'],
            'time': data['time'],
            'uuid': data['uuid'],
            'userScore':data['userScore'],
            'nbQuestion': data['nbQuestion'],
            'defis':False
            }
        self.db.child(data['userLocalId']).child('quizHisto').push(quiz_histo)
        self.db.child(data['userLocalId']).child('quiz').child(data['uuid']).set(data['quiz'])
        #SaveStatsuserLocalId
        self.saveStats(data['userLocalId'], data['userScore'])

    def saveStats(self, userLocalId, userScore):
        
        score = self.db.child("score").child(username).get().val()
        if score == None:
            self.db.child("score").child(username).set(userScore)
        else:
            self.db.child("score").child(username).set(userScore + score)
        
        nbQuiz = self.db.child("nb_quiz").child(username).get().val()
        if nbQuiz == None:
            self.db.child("nb_quiz").child(username).set(1)
        else:
            self.db.child("nb_quiz").child(username).set(nbQuiz + 1)
    
    def getLeaders(self):
        score = self.db.child("score").get().val()
        quiz = self.db.child("nb_quiz").get().val()
        res = []
        
        for each in score:
            myformat = {"name" : "",
            "score":0,
            "quiz":0}
            myformat['name'] = each
            myformat['score'] = score[each]
            myformat['quiz'] = quiz[each]
            res.append(myformat)
        return json.dumps(res)

    def get_quiz_histo(self, userLocalId):
        quiz_histo  = self.db.child(userLocalId).child('quizHisto').get().val()
        print(quiz_histo)
        res = []
        for each in quiz_histo:
            print(each)
            res.append(quiz_histo[each])
        return json.dumps(res)
    
    def get_quiz(self, quizUuid, userLocalId):
        res = {"quiz":self.db.child(userLocalId).child('quiz').child(quizUuid).get().val()}
        return res
    
    #On en est la
    #Il faut encore voir comment sauvegarder les data defis
    def add_defis(self, data, userLocalId, cibleId):
        username = self.findName(userLocalId)
        res = {'fromName':username,
        'fromId': userLocalId,
        'quiz_uuid': data['uuid'],
        'channelName' : data['channelName'],
        'channelUrl': data['channelUrl']
        }
        self.db.child(cibleId).child("pendingDefis").child(data['uuid']).set(res)

        quiz_histo = {
            'channelName' : data['channelName'],
            'channelUrl': data['channelUrl'],
            'date': data['date'],
            'time': data['time'],
            'uuid': data['uuid'],
            'userScore':data['userScore'],
            'userScore2':"None",
            'nbQuestion': data['nbQuestion'],
            'defis':True,
            'from':userLocalId,
            'to':cibleId,
            'done':False
            }
        self.db.child(userLocalId).child('quizHisto').child(data['uuid']).set(quiz_histo)
        self.db.child(userLocalId).child('quiz').child(data['uuid']).set(data['quiz'])
        return json.dumps("ok")
    
    def get_defis(self, userLocalId):
        pending = self.db.child(userLocalId).child('pendingDefis').get().val()
        res  = []
        ##ADD ERROR CASE NO PE?DING
        if pending == None:
            return json.dumps('[]')
        for each in pending:
            print(each)
            res.append(pending[each])
        return json.dumps(res)
    
    def get_defis_quiz(self, fromId, uuid):
        quiz = self.db.child(fromId).child('quiz').child(uuid).get().val()
        histo = self.db.child(fromId).child('quizHisto').child(uuid).get().val()
        i = 0
        while i < histo['nbQuestion']:
            quiz[i]['userResponse'] = "None"
            i = i + 1
        res = {'histo': histo,
        'quiz': quiz}
        return json.dumps(res)
    
    def save_defis(self, userLocalId, quiz):
        print(userLocalId)
        histo = quiz['histo']
        print(histo)
        quiz = quiz['quiz']
        #update quizHisto chez histo[from] pour le UUID uuid
        histo['done'] = True
        self.db.child(histo['from']).child('quizHisto').child(histo['uuid']).update(histo)
        #save le quiz avec metadata chez userLocalId
        self.db.child(userLocalId).child('quizHisto').child(histo['uuid']).set(histo)
        #save l'histo chez userlocalId
        self.db.child(userLocalId).child('quiz').child(histo['uuid']).set(quiz)
        #remove le quiz des pending
        self.db.child(userLocalId).child('pendingDefis').child(histo['uuid']).remove()
        return 'ok'



#test main

#def main():
    #firebase =fb()
    #firebase.create_user('yolo@yopmail.com', '123456789')
    #user = firebase.login('avogawo-3902@yopmail.com', '123456789')
    #print(user)
    #print(firebase.get_account_info(user['idToken']))
    #print(firebase.verify_mail(user['idToken']))