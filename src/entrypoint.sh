#!/bin/sh -l

# This script is used to run the analyzer on a set of files.

# Check if all arguments are provided
if [ -z "$1" ]; then
    echo "Error: Arg(1) Threshold not provided."
    echo "Usage: ./entrypoint.sh <threshold> <excluded_files> <base_reference>"
    exit 1
fi

if [ -z "$2" ]; then
    echo "Error: Arg(2) Analyzed File Extensions not provided."
    echo "Usage: ./entrypoint.sh <threshold> <excluded_files> <base_reference>"
    exit 1
fi

if [ -z "$3" ]; then
    echo "Error: Arg(3) Base Reference for git diff not provided."
    echo "Usage: ./entrypoint.sh <threshold> <excluded_files> <base_reference>"
    exit 1
fi

# Set the working directory to the root of the repository
#cd /github/workspace

echo "Working directory: $(pwd)"

echo "Base reference for comparion: $3"

# Configure the workspace as safe so it will allow to execute git commands properly
# Otherwise, the git commands will fail with a "fatal: not a git repository" error
# https://stackoverflow.com/questions/73485958/how-to-correct-git-reporting-detected-dubious-ownership-in-repository-withou
git config --global --add safe.directory $GITHUB_WORKSPACE

# Make sure the base reference is available for comparison
git fetch origin $3

# Show the current branch
git status

# Create a git diff for the current branch linked to the main branch
# Remove unneeded whitepace for easier parsing
git diff --minimal $3 > /tmp/diff.txt

# Show diff file for debugging
#echo "Diff file start ------------------------------"	
#cat /tmp/diff.txt
#echo "Diff file end ------------------------------"	

# Run the analyzer on the diff
/App/GitDiffAnalyzer -f /tmp/diff.txt -t $1 -e $2 -o $GITHUB_OUTPUT
EXIT_CODE=$?
echo "Exit code: $EXIT_CODE"
exit $EXIT_CODE
#>> $GITHUB_STEP_SUMMARY
