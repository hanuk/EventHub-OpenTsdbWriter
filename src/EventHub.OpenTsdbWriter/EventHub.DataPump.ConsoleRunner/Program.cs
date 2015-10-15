/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Utility.Common;
using Utility.Core;
using EventHub.OpenTsdbTelnetWriter;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using TypeC;

namespace EventHub.DataPump.CopnsoleRunner
{
    class Program
    {
        private static dynamic _settings;

        static void Main(string[] args)
        {
            TypeContainer tc = GetTypeContainerFromCode();
            Initialize(tc);
            Run(tc);
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
        private static TypeContainer GetTypeContainerFromCode()
        {
            TypeContainer tc = TypeContainer.Instance;
            tc.Register<IEventProcessor, TsdbEventProcessor>("OpenTSDB");
            tc.Register<IConverter<EventData,DataPoint>, EventDataToTsdbConverter> ("OpenTSDB");
            tc.Register<ILogger, ConsoleLogger>();
            return tc;
        }

        private static void Initialize(TypeContainer tc)
        {
            _settings = new DynamicDictionary();
            _settings.OpenTsdbConfigFile = "OpenTsdbConfig.json";
            _settings.EventHubConfigFile = "EventHubConfig.json";
            _settings.Logger = tc.GetInstance<ILogger>();
            _settings.TsdbConverter = tc.GetInstance<IConverter<EventData, DataPoint>>("OpenTSDB");
            _settings.Debug = true;
        }
        private async static void Run(TypeContainer tc)
        {
            string eventHubConfigFile = _settings.EventHubConfigFile;
            var eventHubConfig = ConfigUtility.ReadConfig<Dictionary<string, string>>(eventHubConfigFile);
            string eventHubName = eventHubConfig["EventHubName"];
            string eventHubConnectionString = eventHubConfig["EventHubConnectionString"];
            string azureStorageConnectionString = eventHubConfig["AzureStorageConnectionString"];
            string eventProcessorHostName = "IoTEventHubTSDBWriter";
            try
            {
               EventProcessorHost host = new EventProcessorHost(
                           eventProcessorHostName,
                           eventHubName,
                           EventHubConsumerGroup.DefaultGroupName,
                           eventHubConnectionString,
                           azureStorageConnectionString,
                           eventHubName.ToLowerInvariant());

                IEventProcessorFactory factory = new TsdbEventProcessorFactory();
                IInitializer factoryInitializer = factory as IInitializer;
                factoryInitializer.Initialize(_settings);
                var options = new EventProcessorOptions();
                options.ExceptionReceived += Options_ExceptionReceived; 
                await host.RegisterEventProcessorFactoryAsync(factory, options);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void Options_ExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            Debug.WriteLine(string.Format("Received exception, action: {0}, message： {1}.", e.Action, e.Exception.Message));
        }
    }
}