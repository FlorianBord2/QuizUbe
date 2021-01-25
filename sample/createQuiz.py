from ourYoutube import yapi
import json
from random import randrange

class create_quiz:

    def __init__(self):

        self.youtubeApi = yapi()
        self.videoFeed = []
        self.quiz = []

    def getVideoFeed(self, channelId):
         self.videoFeed = self.youtubeApi.yotube_getvideofeed(channelId)

    def create(self, channelId):
        self.getVideoFeed(channelId)
        self.quizGeneration()
        res = json.dumps(self.quiz)
        return res

    def quizGeneration(self):
        self.questionMostView()
        self.questionMostComment()

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
            "title":"La quels de ces videos a le plus de vue ?",
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
        self.quiz.append(question)
    
    #Create a Most commented question
    def questionMostComment(self):
        question = {
            "type":"mostComment",
            "title":"La quels de ces videos a le plus de commentaires ?",
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
        self.quiz.append(question)


def main():
    cq = create_quiz()
    cq.getVideoFeed('UCpLfaNS0J7iPLZhZ_5t8hCA')
    cq.quizGeneration()

if __name__ == '__main__':
    main()