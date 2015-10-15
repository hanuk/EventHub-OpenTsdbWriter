#EvenHub-OpenTsdbWriter#
This library uses telnet interface to write EventHub streams to OpenTsdb.
*EventHub.DataPump.ConsoleRunner: this project hosts the entrypoint for testing purpose. This code can easily be put in WorkerRole or some kind of a background process for running on Azure.
*EventHub.OpenTsdbTelentWriter: this project contains EventHub host factory and processor components, telenet client to OpenTSDB and event hub to DataPoint translator.
*Utility.Common:common utility implementaitons like DynamicDictionary, congiruration utitliy and other cross-cutting components
*Utility.Core: contains the contracts used across the projects. 