import pyrebase
import requests
import json
import sys

class Firebase:
    def __init__(self):
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
            self.running = True
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

    def verify_email(self, user_id_token):
        try:
            return self.auth.send_email_verification(user_id_token)
        except requests.exceptions.HTTPError as e:
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
            print("Can't verify email address: {}".format(e), sys.stderr)
            return self.http_error(e)

    #Data management

    def save_quiz(self, quiz):
        print(data['userLocalId'])
        quiz_histo = quiz.get_histo()
        self.db.child(data['userLocalId']).child('quizHisto').push(quiz_histo)
        self.db.child(data['userLocalId']).child('quiz').child(data['uuid']).set(data['quiz'])

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