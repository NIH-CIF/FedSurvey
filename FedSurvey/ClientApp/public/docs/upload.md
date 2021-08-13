To perform an upload, go to the admin panel and click the red "Upload" button.
This will open up a dialog that prompts you to enter a file.
When you do, this file will be scanned to see if it is an acceptable format.
If it is, you will now be prompted to input a year.
None of the formats include the year they represent within them, so
this is required to make sure that the data is linked up with the right year.
If the upload is of the SurveyMonkey format, an organization name will also
need to be inputted.
If the organization does not exist, it will be created.
If it does exist, this new data will be linked to that organization.
Once each of these fields are inputted, press "Upload" to submit.
Once there is success, you may reset the dialog to upload a new file
or navigate into some of the common post-upload data operations.

There are three formats of files that can be uploaded.
These are detailed below.

**FEVS Format**

FEVS data is often supplied in a format of an Excel file (`.xlsx`).
It contains the headers, in order:

1. Sorting Level
1. Organization
1. Item
1. Item Text
1. Item Respondents N
1. ... columns for each of the possible responses

A screenshot of these headers with an example row is below.

![FEVS Format Headers](img/fevs-headers.PNG)

These columns are used as follows:

1. Sorting Level: This is ignored.
1. Organizaion: This is used to link the data to an organization in this system.
If it does not exist, it will be created.
1. Item: This is used to find the question number.  The "Q" will be pulled out of the field.
1. Item Text: This is used to possibly link the question to other questions in the past in this system.
If a question with this text does not exist, it will be created.
1. Item Respondents N: This is used to help to store percentages as number of people responding a certain way.
This ensures data accuracy as computed organizations are made.
1. Positive %, Neutral %, Negative %: Everything before the "%" is used to link the response to a response in the system.
These responses are deemed the responses that make up the percentages of the question.
1. Do Not Know/ No Basis to Judge N: Everything before the "N" is used to link the response to a response in the system.
This response is deemed a response that does not factor into the percentages of the question.

The worksheets that come in this format also come with multiple sheets within the larger file.
For example, the 2018 file in this format contains the sheets "Core Survey", "Telework", and
"Work Life".
While the system is built in a way that would support the other sheets, which have different
sets of responses, currently the system is locked in to only accepting sheets that are named
"Core Survey".
Other sheets will be ignored.

So, to use this file format, the filename must end with `.xslx` and the headers must exactly match
what is shown above.
Otherwise, the software will deem it too risky to try to upload.

**New Format**

In 2020, the FEVS data was supplied in a new format.
Because I have also found places where in the 2020 existed in the old format,
I have named this format the "new format".
I do not know if it will persist or not, and neither do my contacts in DPCPSI.
This format also uses an Excel file (.xlsx).
The format contains two relevant sheets within the larger file.
One has the following headers:

1. Item
1. Item Text
1. ... columns for various indices they exist within

A screenshot of these headers with an example row is below.

![New Format Index Headers](img/new-index-headers.PNG)

The other relevant sheets have the following headers:

1. Agency & Subagency Name
1. Level Code
1. Reporting Level
1. Response Count
1. ... columns for each question and response

A screenshot of these headers with an example row is below.

![New Format Data Headers](img/new-headers.PNG)

Together, the columns are used as follows:

1. Item and Item Text: These are used in order to link a question number with a question text.
The question text is what is used to link this question to a previous question in the system if one exists
or to create a new question if one does not exist.
1. Agency & Subagency Name: This is used to match the data to an organization in the system.
If one does not exist, it will be created.
1. Level Code: This is ignored.
1. Reporting Level: This is ignored.
1. Response Count: This is used to help to store percentages as number of people responding a certain way.
This ensures data accuracy as computed organizations are made.
1. Q1 Pos, Q1 Neu, Q1 Neg, Q2 Pos, Q2 Neu, Q2 Neg, ...: These form the percentage data in the file.
For each column, the material before the space is used to reference the first sheet in order to
determine to which question this data belongs.
The material after the space is used to determine which response this data is linked to.
Unlike the FEVS format, all responses are part of the percentage of the question.

The worksheets that come in this format also come with multiple sheets within the larger file.
For example, the 2020 file in this format contains the sheets "Core Q1-10, 12-38", "Core Perf Q11",
"COVID-19 Bckgrnd Q39, 41-42", and more.
While the system is built in a way that would support the other sheets, which have different
sets of responses, currently the system is locked in to only accepting sheets that are named
"Core Q1-10, 12-38".
Other sheets will be ignored.

So, to use this format, the filename must end with `.xslx` and each sheet in the file
must be either an item sheet (the first type referenced) or a data sheet (the second type referenced).
Otherwise, the software will deem it too risky to upload.

**SurveyMonkey Format**

If you seek to combine data from a survey that you run with FEVS data, SurveyMonkey's export
can be directly uploaded into the system.
The data should be exported into a file that ends with `.csv`.
Because the export from SurveyMonkey is a standardized process that should return
a file of similar structure every time, this format will not be documented to the extent
of the files above.

When creating a SurveyMonkey survey, if the questions and responses are to be lined up properly,
the question texts and response names need to match directly.

If there is a time when data is not provided to the office in a format described above - that is,
upload fails - the FEVS format should be used to translate data into a file that the software will
understand.
The FEVS format is the most logical and intepretable file format of the bunch.