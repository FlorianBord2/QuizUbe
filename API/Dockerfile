FROM tiangolo/meinheld-gunicorn-flask:python3.8

WORKDIR /api

ENV STATIC_URL /static
ENV STATIC_PATH ./app/static

EXPOSE 65000

RUN python3 -m pip install --upgrade pip && \
python3 -m pip install -r requirements.txt