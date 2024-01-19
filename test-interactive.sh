#! /bin/bash

docker run -it -v $(pwd):/github/workspace -e GITHUB_OUTPUT=/github/workspace/testoutput/output.txt --entrypoint /bin/sh git-diff-image