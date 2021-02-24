import sys
import json
sys.path.insert(1, '../Database')

from flask import Flask, request, Response
from firebase import Firebase as fb
from user import User as ue

app = Flask(__name__)

f = fb()
f.is_running()
auth = f.get_auth()