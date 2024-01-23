#! /bin/bash

mkdir -p ./testoutput
echo "" > ./testoutput/output.txt

docker run -it -v $(pwd):/github/workspace --workdir /github/workspace -e GITHUB_OUTPUT=/github/workspace/testoutput/output.txt -e GITHUB_WORKSPACE=/github/workspace -e GITHUB_REF=main git-diff-image 15 "md cpp"