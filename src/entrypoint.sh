#!/bin/sh -l

# This script is used to run the analyzer on a set of files.

# Set the working directory to the root of the repository
#cd /github/workspace

echo "Working directory: $(pwd)"

echo "Github Workspace: $GITHUB_WORKSPACE"

ls -la

# Create the output directory
mkdir -p /github/workspace/testoutput

# Create a git diff for the current branch linked to the main branch
# Remove unneeded whitepace for easier parsing
git diff --minimal main > /tmp/diff.txt

## Show diff file for debugging
#cat /tmp/diff.txt

# Run the analyzer on the diff
/App/git-diff-analyzer -f /tmp/diff.txt -t $1 -e $2 -o $GITHUB_OUTPUT
