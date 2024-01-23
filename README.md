# Git Diff Analyzer

This github action will look at th differences from the current branch towards the branch determined by $GITHUB_REF and highlight based on the configuration if significant changes have taken place.

It will analyze all the file changes and do the following:

- Compare the added & removed lines for each change that occurred in each file and determine the Levenshtein distance between the two versions
- Validate the extension of the file:
  - files that are part of the `allowed-file-types` will be checked for the `ld-threshold` and have `impact-level`: `considerable` if above the threshold
  - other files will automatically count as `impact-level`: `considerable`
- When there is no file with `impact-level`: `considerable`, it is deemed `low`

The impact level is returned as output, so it can be used in your workflow, e.g. to determine if a PR can automatically be approved.

## Inputs

## `ld-threshold`

**Required** The maximum Levenshtein distance allowed for the changes to be considered as low impact. Default `10`.

## `allowed-file-types`

**Required** The types of files that are analyzed. Default `md`.

## Outputs

## `impact-level`

The level of impact on the commit, this can be:

- low : this means all analyzed files are below the set threshold
- considerable : this means either files were changed that do not match `allowed-file-types`, or the changes in at least one of the analyzed files is above the threshold.

## Example usage

```yaml
jobs:
  test_job:
    runs-on: ubuntu-latest
    name: Test the action
    steps:
      - name: Checkout
        uses: actions/checkout@v4
      - name: Diff Analyzer
        uses: raimondb/git-diff-analyzer@v0.9
        id: diff-analyze
        with:
          ld-threshold: 8
          allowed-file-types: 'md'
      # Use the output from the previous step
      - name: Show the output
        run: echo "The change was impact-level ${{ steps.diff-analyze.outputs.impact-level }}"
```
