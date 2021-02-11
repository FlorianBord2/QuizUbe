class Quiz:
	def __init__(self, data):
		self.quiz_histo = {
			'date': data['date'],
			'time': data['time'],
			'uuid': data['uuid'],
			'userScore':data['userScore'],
			'nbQuestion': data['nbQuestion']
			}

	def get_histo(self):
		return self.quiz_histo