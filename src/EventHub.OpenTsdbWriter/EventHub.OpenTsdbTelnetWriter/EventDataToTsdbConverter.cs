/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Utility.Core;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.OpenTsdbTelnetWriter
{
    public class EventDataToTsdbConverter : IConverter<EventData, DataPoint>
    {
        public DataPoint Convert(EventData inObj)
        {
            string str = Encoding.UTF8.GetString(inObj.GetBytes());
            Message msg = Message.Deserialize(str);
            DataPoint dataPoint = new DataPoint();
            dataPoint.Name = msg.GetValue(msg.KeyName);
            dataPoint.Value = Double.Parse(msg.GetValue("TagValue"));
            dataPoint.Timestamp = DateTime.Parse(msg.GetValue("Timestamp"));
            dataPoint.Tags.Add("Quality", msg.GetValue("TagQuality"));
            dataPoint.Tags.Add("Type", msg.GetValue("TagType"));
            return dataPoint;
        }
    }
}