import pyrebase
import requests
import json

class fb:

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

        self.firebase = pyrebase.initialize_app(config)
        self.auth = self.firebase.auth()
        self.db = self.firebase.database()
    
    #User management

    def register(self, email, password):
        try:
            return(self.auth.create_user_with_email_and_password(email, password))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error
    
    def login(self, email, password):
        try:
            return(self.auth.sign_in_with_email_and_password(email, password))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error

    def verify_mail(self, userIdToken):
        try:
            return(self.auth.send_email_verification(userIdToken))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error
    
    def reset_password(self, email):
        try:
            return(self.auth.send_password_reset_email(email))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error
    
    def get_account_info(self, userIdToken):
        try:
            return(self.auth.get_account_info(userIdToken))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error
    
    def refresh_token(self, refreshToken):
        try:
            return(self.auth.refresh(refreshToken))
        except requests.exceptions.HTTPError as e:
            error_json = e.args[1]
            error = json.loads(error_json)['error']
            return error

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


def main():
    firebase =fb()
    #firebase.create_user('yolo@yopmail.com', '123456789')
    user = firebase.login('avogawo-3902@yopmail.com', '123456789')
    #print(user)
    print(firebase.get_account_info(user['idToken']))
    #print(firebase.verify_mail(user['idToken']))


if __name__ == '__main__':
    main()