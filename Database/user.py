from requests.exceptions import HTTPError
import sys
import requests
import json
from firebase import Firebase as fb

class User:
	def __init__(self, email, password):
		self.email = email
		self.password = password
		self.id_token = ""
		self.refresh_token = ""
		self.first_name = ""
		self.last_name = ""
		self.username = ""
		self.picture = ""

	def get_id_token(self):
		return self.id_token
	
	def get_refresh_token(self):
		return self.refresh_token

	def register(self, auth):
		try:
			return auth.create_user_with_email_and_password(self.email, self.password)
		except requests.exceptions.HTTPError as e:
			print("Can't register: {}".format(e), sys.stderr)
			return fb.http_error(e)
	
	def login(self, auth):
		try:
			login = auth.sign_in_with_email_and_password(self.email, self.password)
			self.id_token = login['idToken']
			return login
		except requests.exceptions.HTTPError as e:
			print("Can't log in: {}".format(e), sys.stderr)
			return fb.http_error(e)
	
	def reset_password(self, auth):
		try:
			return auth.send_password_reset_email(self.email)
		except requests.exceptions.HTTPError as e:
			print("Can't reset password: {}".format(e), sys.stderr)
			return fb.http_error(e)
	
	def toJSON(self):
		return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)