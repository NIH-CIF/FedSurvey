Logging in is required to view any survey data or perform any administrative actions.
It is done with a button that will appear in the top right of the application when you are not logged in.

To log in, use your computer username and password.
This system currently links into the NIH's accounts system,
so it will require a code change to work for other federal agencies.

Currently, login is restricted only to people within OSC.
To change this, [this line](https://github.com/NIH-CIF/FedSurvey/blob/main/FedSurvey/Services/AuthenticationService.cs#L53)
would need to be changed to allow `physicaldeliveryofficename` to be the name of the new office to allow login for.
The format seems to be the IC, followed by the next level down, and so on, as OSC is within DPCPSI which is within OD which is
within the NIH.
But I have not investigated the entire structure of this variable, so it is possible it is different in different situations.