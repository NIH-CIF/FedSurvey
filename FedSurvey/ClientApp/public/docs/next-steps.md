I have a rather large task list of items beyond the bare minimum that would be wonderful to implement if there is time.
I will include some of it below for whoever may later be interested in this project.
It is not exhaustive, as I would like to highlight managing dashboard views from outside the database as a wonderful additional feature.

- `QuestionMerge` in React UI: Remove merge candidate after a merge involving it has been done - do not wait for the page to be reset.
- `DataGroupsController#Merge` in API: When a data group merge happens, DataGroupLinks need to be adjusted too.
- `ResultsDataTable` in React UI: When there is no data getting into a `ResultsDataTable`, there should be some notification to the user.
- `ResultsDataTable` in React UI: Do not retrieve data when one locked variable is set and one unlocked variable is set - this breaks the table.
- Grouping questions together
  - Across React UI: Have QuestionGroup of id 1 be accessed by name rather than id.
  - API, UI: Allow editing of which questions are in QuestionGroups.  Allow creating.
  - React UI: Allow filtering `ResultsDataTable` by QuestionGroup.
  - `ResultsDataTable` in React UI: Optionally allow an Average row to appear to average out groups of questions.
- `Advanced` in React UI: List "Setup" in instructions as "Step 1" or "Step 2" depending on the step.
- `ResultsDataTable` in React UI: Examine some mechanism of doing a free-form vertical or horizontal sort.
- Consider ways of showing three dimensions on the table at the same time, such as an OrganizationYear across variable.
- `ResultsDataTable` in React UI (also expose in API): Show the total number of responses for a question optionally.
- React UI: Confirmation dialog for all destructive actions.
- Support and create question types other than "Core Survey".
  This would involve not allowing questions of different types to be in the same tables, as they cannot be compared.
  The database does support the fundamentals of this, though.
- Examine all edge cases and ensure errors do not happen (purposefully vague).
- Test suite for all API routes.
- Allow data table to scroll horizontally so that many years in the system does not cause issue.