/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Utility.Common;
using Utility.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHub.OpenTsdbTelnetWriter
{
    public class TsdbEventProcessor : IEventProcessor, IInitializer
    {
        private ILogger _logger;
        private int _totalMessages = 0;
        private Stopwatch _checkpointStopWatch;
        private TsdbTelnetClient _tsdbTelnetClient;
        private IConverter<EventData, DataPoint> _tsdbConverter;
        private bool _debug = false;
        private bool _checkpointingOn = true; 
        public int TotalMessages
        {
            get
            {
                return this._totalMessages;
            }
        }
        public void Initialize(dynamic settings)
        {
            FieldValidator.CheckMissingFields(settings, new string[] { "OpenTsdbConfigFile", "TsdbConverter", "Debug", "Logger" });
            this._tsdbConverter = settings.TsdbConverter;

            //CheckpointOn is to be set false for debugging purpose when you don't care about repeat processing of EventHub records
            //set CheckpointOn to true to save the offset to avoid duplicate processing of EventHub records
            if (settings.CheckpointOn != null)
            {
                _checkpointingOn = settings.CheckpointOn;
            }

            string configFileName = settings.OpenTsdbConfigFile;
            if (configFileName == null)
            {
                throw new ApplicationException("missing OpenTsdbConfig file");
            }
            var config = ConfigUtility.ReadConfig < Dictionary<string, string>>(configFileName);
            string openTsdbHostName = config["OpenTsdbHost"];
            int openTsdbPort = int.Parse(config["OpenTsdbPort"]);
            this._debug = settings.Debug; 
            this._tsdbTelnetClient = new TsdbTelnetClient(openTsdbHostName, openTsdbPort, true);
            _logger = settings.Logger; 
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            WriteLog("{0} > Close called for processor with PartitionId '{1}' and Owner: {2} with reason '{3}'.", DateTime.Now.ToString(), context.Lease.PartitionId, context.Lease.Owner ?? string.Empty, reason);
            if (_checkpointingOn)
            {
                return context.CheckpointAsync();
            }
            else
            {
                return Task.FromResult<object>(null);
            }
        }

        public Task OpenAsync(PartitionContext context)
        {
            this._checkpointStopWatch = new Stopwatch();
            this._checkpointStopWatch.Start();

            WriteLog("{0} > Processor Initializing for PartitionId '{1}' and Owner: {2}.", DateTime.Now.ToString(), context.Lease.PartitionId, context.Lease.Owner ?? string.Empty);
            return Task.FromResult<object>(null);
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            try
            {
                foreach (EventData message in messages)
                {
                    if (message == null )
                    {
                        continue;
                    }
                    DataPoint dataPoint = _tsdbConverter.Convert(message);
                    //ignore old data in the eventhub sent due to formatting error
                    if (dataPoint.Name.IndexOf("=") < 0)
                    {
                        //write to OpenTSDB
                        this._tsdbTelnetClient.Write(dataPoint);
                        // increase the message counter
                        Interlocked.Increment(ref this._totalMessages);

                        if (this._debug)
                        {
                            Debug.WriteLine("{0} > received message: {1} at partition {2}, owner: {3}, offset: {4}", DateTime.Now.ToString(), Encoding.UTF8.GetString(message.GetBytes()), context.Lease.PartitionId, context.Lease.Owner, message.Offset);
                        }
                    }

                    if (_checkpointingOn)
                    {
                        if (_checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(1))
                        {
                            lock (this)
                            {
                                _checkpointStopWatch.Reset();
                                return context.CheckpointAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Task.FromResult<object>(null);
        }
        public void WriteLog(string formatString, params object[] fields)
        {
            string logMsg = string.Format(formatString, fields);
            if (_debug)
            {
                Debug.WriteLine(logMsg);
            }

            if (_logger != null)
            {
                _logger.Write(logMsg);
            }
        }
    }
}