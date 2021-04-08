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
        print(data['userLocalId'])
        quiz_histo = {
            'date': data['date'],
            'time': data['time'],
            'uuid': data['uuid'],
            'userScore':data['userScore'],
            'nbQuestion': data['nbQuestion']
            }
        self.db.child(data['userLocalId']).child('quizHisto').push(quiz_histo)
        self.db.child(data['userLocalId']).child('quiz').child(data['uuid']).set(data['quiz'])
        #SaveStats
        self.saveStats(data['userLocalId'], data['userScore'])

    def saveStats(self, userLocalId, userScore):
        users = self.db.child('users').get()
        username = None
        for user in users.each ():
            if user.val() == userLocalId:
                username = user.key()
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
        score = self.db.child("score").order_by_value().get().val()
        quiz = self.db.child("nb_quiz").order_by_value().get().val()
        res = {"score" : score, "quiz" : quiz}
        return res

    def get_quiz_histo(self, userLocalId):
        return self.db.child(userLocalId).child('quizHisto').get().val()
    
    def get_quiz(self, quizUuid, userLocalId):
        res = {"quiz":self.db.child(userLocalId).child('quiz').child(quizUuid).get().val()}
        return res

#test main

def main():
    firebase =fb()
    #firebase.create_user('yolo@yopmail.com', '123456789')
    user = firebase.login('avogawo-3902@yopmail.com', '123456789')
    #print(user)
    print(firebase.get_account_info(user['idToken']))
    #print(firebase.verify_mail(user['idToken']))