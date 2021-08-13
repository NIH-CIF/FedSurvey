If a new office outside of OSC were to adopt this software, there are various routes to this with different levels of effort.
The easiest route would be to host a separate instance of the software with a separate database.
To set up a database, this software comes with database migrations.
Once a connection string to your database has been established, you can connect
a local copy of the source code to that database.
I did this by setting my `appsettings.json` connection string to the connection
string to the production server.
Then, in the Visual Studio Package Manager Console, you can run
"Update-Database".
This will run all migrations to get your database ready to go.

The next step will be to turn on login for other offices outside of OSC.
Check out the Login documentation page for more details, but
if you know the `physicaldeliveryofficename` of members of your office,
you can replace the OSC string with that.
Otherwise, you can remove that check entirely, but note that
then anybody across the NIH could log in to the system and
view your data.

The final step, possibly even after running the instance and uploading all of your data, is to customize
the dashboard.
If you do not want any of the quick views, you could drop the `Views` table entirely.
Otherwise, check out the Dashboard documentation page for a more technical explanation
of customizing these views along with an example SQL script.

Both of the steps required above are not ideal, and only exist because
of the time crunch of 10 weeks resulting in having to cut a few corners in
order to get the software working most ideal for OSC.

**Better Way of the Future**

A better way of the future could be to store all office data in a single database.
Permissions would then become a challenge, though.
The `physicaldeliveryofficename` variable attached to each user account would likely
need to be used in order to establish who can view what data.
This would absolutely require new programming, but then it could save the same server
being run by multiple offices.