DatenMeister
============


Obsolete... 
use http://github.com/mbrenn/datenmeister-new


============

[![ghit.me](https://ghit.me/badge.svg?repo=mbrenn/datenmeister)](https://ghit.me/repo/mbrenn/datenmeister)

First of all: Thanks for using DatenMeister. 

DatenMeister is database connection application to store data, to visualize data and to analyze data. Key element of DatenMeister is the transparent use of existing database in a way that the original database is used directly and changes in DatenMeister will directly lead to changes in the original database. Reading and writing will be performed in the original database.

Access to these databases will be performed via database-providers which give access via an interface according to the MOF specification.

The DatenMeister library is provided with a demo application, called ProjektMeister. ProjektMeister can be used to manage simple projects, its persons, due dates and tasks.

Documentation 
-------------

Via OneDrive Word-Document: http://1drv.ms/1q1A7MW

Installation
------------

- git clone https://github.com/mbrenn/datenmeister.git
- Visual Studio Express 2013 for Windows Desktop: http://www.visualstudio.com/en-us/products/visual-studio-express-vs.aspx
- Execute "fetch_dependencies.bat"
- Open DatenMeister.sln in src folder
- Compile 
- Set ProjektMeister as default project
- Execute

The ProjektMeister project creates an executable of the example application. 
