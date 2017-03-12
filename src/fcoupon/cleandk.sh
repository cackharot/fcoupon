#!/bin/bash
set -xv
docker rm -v $(docker ps -a -q -f status=exited)
