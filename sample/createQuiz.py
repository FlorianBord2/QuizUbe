from ourYoutube import yapi
import json
from random import randrange
from datetime import datetime
import time
import uuid

class create_quiz:

    def __init__(self):

        self.youtubeApi = yapi()
        self.videoFeed = []
        now = datetime.now()
        self.quiz = {
            "userLocalId": "userLocalId",
            "date": now.strftime("%d/%m/%Y %H:%M:%S"),
            "time": time.time(),
            "uuid":str(uuid.uuid1()),
            "userScore":0,
            "nbQuestion":0,
            "quiz":[]
        }

    def getVideoFeed(self, channelId):
         self.videoFeed = self.youtubeApi.yotube_getvideofeed(channelId)

    def create(self, channelId):
        self.getVideoFeed(channelId)
        self.quizGeneration()
        self.quiz['nbQuestion'] = len(self.quiz['quiz'])
        res = json.dumps(self.quiz)
        return res

    def quizGeneration(self):
        self.questionMostView()
        self.questionMostComment()
        self.questionMostDislike()
        self.questionMostLike()

    #Choose x random video in videoFeed
    def chooseVideos(self, x):
        nbVideo = len(self.videoFeed)
        videoChoose= []
        while len(videoChoose)<x:
            nb = randrange(nbVideo)
            if nb not in videoChoose:
                videoChoose.append(nb)
        return videoChoose

    #Create a Most viewed question
    def questionMostView(self):
        question = {
            "type":"mostViewed",
            "videos":[],
            "response": 5,
            "userResponse":"None"
        }
        videoChoose = self.chooseVideos(4)
        i = 0
        mostView = 0
        validResponse = 0
        while i < 4:
            if int(self.videoFeed[videoChoose[i]]['view']) > mostView:
                validResponse = i
                mostView = int(self.videoFeed[videoChoose[i]]['view'])
            question['videos'].append(self.videoFeed[videoChoose[i]])
            i = i + 1    
        question['response'] = validResponse
        self.quiz['quiz'].append(question)
    
    #Create a Most commented question
    def questionMostComment(self):
        question = {
            "type":"mostComment",
            "videos":[],
            "response": 5,
            "userResponse":"None"
        }
        videoChoose = self.chooseVideos(4)
        i = 0
        mostComment = 0
        validResponse = 0
        while i < 4:
            if int(self.videoFeed[videoChoose[i]]['comment']) > mostComment:
                validResponse = i
                mostComment = int(self.videoFeed[videoChoose[i]]['comment'])
            question['videos'].append(self.videoFeed[videoChoose[i]])
            i = i + 1    
        question['response'] = validResponse
        self.quiz['quiz'].append(question)

    #Create a Most like question
    def questionMostLike(self):
        question = {
            "type":"mostLike",
            "videos":[],
            "response": 5,
            "userResponse":"None"
        }
        videoChoose = self.chooseVideos(4)
        i = 0
        mostLike = 0
        validResponse = 0
        while i < 4:
            if int(self.videoFeed[videoChoose[i]]['like']) > mostLike:
                validResponse = i
                mostLike = int(self.videoFeed[videoChoose[i]]['like'])
            question['videos'].append(self.videoFeed[videoChoose[i]])
            i = i + 1    
        question['response'] = validResponse
        self.quiz['quiz'].append(question)

    #Create a Most dislike question
    def questionMostDislike(self):
        question = {
            "type":"mostDisike",
            "videos":[],
            "response": 5,
            "userResponse":"None"
        }
        videoChoose = self.chooseVideos(4)
        i = 0
        mostDislike = 0
        validResponse = 0
        while i < 4:
            if int(self.videoFeed[videoChoose[i]]['dislike']) > mostDislike:
                validResponse = i
                mostLike = int(self.videoFeed[videoChoose[i]]['dislike'])
            question['videos'].append(self.videoFeed[videoChoose[i]])
            i = i + 1    
        question['response'] = validResponse
        self.quiz['quiz'].append(question)

def main():
    cq = create_quiz()
    cq.getVideoFeed('UCpLfaNS0J7iPLZhZ_5t8hCA')
    cq.quizGeneration()

if __name__ == '__main__':
    main()