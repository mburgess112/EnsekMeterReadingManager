# Ensek Meter Reading Manager
A simple API for uploading meter readings from CSV files.

## Getting Started
This solution requires Visual Studio 2019 16.8 and .Net SDK 5.0.
Start the service by pressing F5 or running from the Build menu.
This will deploy the database, start the API and open a browser with a Swagger page.

There are some files in the LocalTesting folder to assist with development:
1. ImportTestingAccounts.sql is a SQL script that can be run against a fresh database install. This will create a number of test accounts to upload readings for.
2. Test_Accouts.ods was converted from the given CSV file, and contains a formula for putting together the one-off import script above.
3. Meter_Reading.csv is the unaltered sample file given in the task, and can be used for testing with Swagger.