#!/bin/sh
# List of commands to host the database locally

firebase login # Log in through the web browser
firebase projects:create # Create the required project
firebase projects:list # Get the projects list and IDs
firebase init
firebase deploy