from googleapiclient.discovery import build
from googleapiclient.errors import HttpError

DEVELOPER_KEY = 'AIzaSyDxoSNT5StMt0-lgDw4IZyQiV3eBrTLsMc'
YOUTUBE_API_SERVICE_NAME = 'youtube'
YOUTUBE_API_VERSION = 'v3'

class options:

    q=''
    search_type=''
    max_results=5

class yapi:

    def __init__(self):

        self.youtube = build(YOUTUBE_API_SERVICE_NAME, YOUTUBE_API_VERSION, developerKey=DEVELOPER_KEY)

    def yotube_getvideofeed(self, channelID):

        search_response = self.youtube.search().list(
            q=options.q,
            part='id,snippet',
            type='video',
            channelId=channelID,
            maxResults=30
        ).execute()

        videos = []
        for search_result in search_response.get('items', []):
            if search_result['id']['kind'] == 'youtube#video':
                req = self.youtube.videos().list(part="statistics", id=search_result['id']['videoId'])
                response = req.execute()
                response = response.get('items',[])
                response = response[0]
                vid={}
                vid['url'] = search_result['snippet']['thumbnails']['medium']['url']
                vid['title'] = search_result['snippet']['title']
                vid['desc'] = search_result['snippet']['description']
                vid['view'] = response['statistics']['viewCount']
                vid['like'] = response['statistics']['likeCount']
                vid['dislike'] = response['statistics']['dislikeCount']
                vid['comment'] = response['statistics']['commentCount']
                videos.append(vid)
        
        return (videos)

    def youtube_search_channel(self, looking_for):
       
        option=options()
        option.q=looking_for
        option.max_results=5

        search_response = self.youtube.search().list(
            q=option.q,
            part='id,snippet',
            type='channel',
            maxResults=option.max_results
        ).execute()

        channel = []

        for search_result in search_response.get('items', []):
            if search_result['id']['kind'] == 'youtube#channel':
                chan={}
                chan['title'] = search_result['snippet']['title']
                chan['channelId'] = search_result['id']['channelId']
                chan['url'] = search_result['snippet']['thumbnails']['medium']['url']
                #print(chan)
                channel.append(chan)
    
        return channel

#test main

def main():
    api = yapi()

    

    print(api.youtube_search_channel('Clippex rotmg'))
    #print(api.yotube_getvideofeed('UCpLfaNS0J7iPLZhZ_5t8hCA'))

if __name__ == '__main__':
    main()