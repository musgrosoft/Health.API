# Health.API

This is an API that I use to import Health data into a SQL Server database.

Data is imported from Withings (Weight, % Fat Body Mass, and Blood Pressure), from Fitbit (Resting Heart Rate and Sleep Data), and from Google Sheets (Exercise data).

I then use a Grafana dashboard to display the data.

I use Appveyor to deploy to Azure.

[![Build status](https://ci.appveyor.com/api/projects/status/7i66d4150g49q2rk?svg=true)](https://ci.appveyor.com/project/TimMusgrove/health-api-fuiws)