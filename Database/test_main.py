from firebase import Firebase as fb
from user import User

def main():
	firebase = fb()
	firebase.is_running()
	auth = firebase.get_auth()

	user = User("test-001@gmail.com", "123456")
	user.register(auth)
	user.login(auth)
	firebase.verify_email(user)
	print(firebase.get_user_info(user))

if __name__ == '__main__':
	main()