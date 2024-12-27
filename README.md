# Torn Black Market
The original intent behind this abandoned project was to create a tool for advertising bazaars and negotiating trades for the PBBG called [Torn](https://torn.com).
I had higher priority items come up and I needed to address them.

In its current form, it consists of a web API and two console applications. The web API (TornBlackMarket.Server) was planned to be accessed by a Blazor frontend. 
The first console application (TornBlackMarket.Migrations) was for performing database upgrades using FluentMigrator. The second console application
(TornBlackMarket.Periodic) was intended to be something could be lauched periodically with command line arguments specifying jobs to run.
