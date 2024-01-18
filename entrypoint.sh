#! /bin/bash

# This script is used to run the analyzer on a set of files.

# Create a git diff for the current branch linked to the main branch
# Remove unneeded whitepace for easier parsing
git diff --name-only --minimal main > /Input/diff.txt

# Run the analyzer on the diff
dotnet git-diff-analyzer.dll -f /Input/diff.txt

echo "impact-level=$?" >> $GITHUB_OUTPUT
