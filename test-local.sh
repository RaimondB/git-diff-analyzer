#! /bin/bash

docker run -it -v $(pwd):/github/workspace --workdir /github/workspace -e GITHUB_OUTPUT=/github/workspace/testoutput/output.txt git-diff-image 15 "md cpp"