from firebase_admin import credentials, initialize_app, storage
# Init firebase with your credentials
cred = credentials.Certificate("quizube-epitech-546df9bc873a.json")
initialize_app(cred, {'storageBucket': 'quizube-epitech.appspot.com'})

# Put your local file path 
fileName = "icelake.png"
bucket = storage.bucket()
blob = bucket.blob(fileName)
blob.upload_from_filename(fileName)

# Opt : if you want to make public access from the URL
blob.make_public()

print("your file url", blob.public_url)