#! /bin/bash

docker run -it -v $(pwd):/github/workspace -e GITHUB_OUTPUT=/github/workspace/testoutput/output.txt git-diff-image 15 "md cpp"