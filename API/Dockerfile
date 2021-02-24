FROM python:3.8-slim

EXPOSE 65000
EXPOSE 90

WORKDIR /requirements

COPY requirements.txt /requirements

RUN python3 -m pip install --upgrade pip && \
python3 -m pip install -r requirements.txt 

WORKDIR /app

ENTRYPOINT [ "/bin/bash" ]
