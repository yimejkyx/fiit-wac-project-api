#!/bin/bash
docker build -t patients-portal-api:latest .
docker run -p 8080:8080 patients-portal-api:latest
