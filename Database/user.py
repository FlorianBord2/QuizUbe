from requests.exceptions import HTTPError
import sys
import requests
import json
from firebase import Firebase as fb

class User:
	def __init__(self, email, password):
		self.email = email
		self.password = password
		self.first_name = ""
		self.last_name = ""
		self.username = ""
		self.picture = ""

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
	
	def save_email(self, email):
		self.email = email
	
	def save_password(self, password):
		self.password = password
	
	@staticmethod
	def to_json(self):
		return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)