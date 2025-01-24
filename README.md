# Torn Black Market
The Torn Black Market was initially planned to be a web API to facilitate trading through the bazaar system for a PBBG called [Torn](https://torn.com).
The motivation for starting this project was that the game's developer made a change such that bazaar are no longer visible from the item market. It was designed
to take advantage of the game's web API and the meat of this project's authentication was handled by the game. The JWT issued by this web API was essentially
meant to be a cache so that I would not need to issue needless requests for a user's information.

In its current form, it consists of a web API and two console applications. The web API (TornBlackMarket.Server) project was planned to be accessed by a frontend. 
The first console application (TornBlackMarket.Migrations) was for performing database upgrades using FluentMigrator. I used FluentMigrator as I chose to use Dapper
for database access to SQL Server database. The second console application (TornBlackMarket.Periodic) was intended to be something could be lauched periodically with 
command line arguments specifying jobs to run.

This project is considered to be abandoned due to higher priority items and that it was a duplication of effort by several other members of the game's community.
