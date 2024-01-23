#!/bin/sh -l

# This script is used to run the analyzer on a set of files.

# Set the working directory to the root of the repository
#cd /github/workspace

echo "Working directory: $(pwd)"

echo "Github Workspace: $GITHUB_WORKSPACE"

# Configure the workspace as safe so it will allow to execute git commands properly
# Otherwise, the git commands will fail with a "fatal: not a git repository" error
# https://stackoverflow.com/questions/73485958/how-to-correct-git-reporting-detected-dubious-ownership-in-repository-withou
git config --global --add safe.directory $GITHUB_WORKSPACE

# Show the current branch
git status

# Create a git diff for the current branch linked to the main branch
# Remove unneeded whitepace for easier parsing
git diff --minimal $GITHUB_REF > /tmp/diff.txt

# Show diff file for debugging
#echo "Diff file start ------------------------------"	
#cat /tmp/diff.txt
#echo "Diff file end ------------------------------"	

# Run the analyzer on the diff
/App/GitDiffAnalyzer -f /tmp/diff.txt -t $1 -e $2 -o $GITHUB_OUTPUT 
#>> $GITHUB_STEP_SUMMARY
