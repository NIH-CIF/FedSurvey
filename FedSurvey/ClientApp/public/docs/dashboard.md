The dashboard is the main page of the application.
It is entirely customizable outside of the History and Advanced views.
There is no non-technical way of customizing the tiles for now.
But a technical description of how it would be done is below.

There are two tables that relate to the dashboard:
`Views` and `ViewConfigs`.
`Views` contains a row for each tile that exists on the dashboard.
`ViewConfigs` contains a row for each argument that would be passed to
`ResultsDataTable` in the front end to generate that view.
This relates directly to how the tiles are formed: each tile is simply
setting values for the Advanced tab.
So, one `ViewConfig` row may have `sortingVariable` (across variable) as
the `VariableName` and `dataGroupName` as the `VariableValue`.
This will then be fed into `ResultsDataTable` to generate the table.

One easy way to generate a new custom view would be to create the table you want
to persist in the Advanced view and then observe with the React developer tools
what props are passed into the `ResultsDataTable`.
That will define your `ViewConfig` rows.

There is one special case for `ViewConfig`.
You can create a view that will always show the `N` latest years
by defining a variable as `$(latestExecutionNamesN)`.
So, to make a view use the latest year as the variable value,
the `VariableValue` would be set to `$(latestExecutionNames1)`.

An example of customizing the dashboard for another organization is based on OSC's
default dashboard configuration, and exists [here](https://github.com/NIH-CIF/FedSurvey/blob/main/FedSurvey/od-view-config.sql).