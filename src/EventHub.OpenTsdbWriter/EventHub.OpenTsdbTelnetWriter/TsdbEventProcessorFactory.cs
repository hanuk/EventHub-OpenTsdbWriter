/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using Utility.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.OpenTsdbTelnetWriter
{
    public class TsdbEventProcessorFactory : IEventProcessorFactory,IInitializer
    {
        private dynamic _settings; 
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            IEventProcessor tsdbEventProcessor = TypeC.TypeContainer.Instance.GetInstance<IEventProcessor>("OpenTSDB");
            IInitializer initializer = tsdbEventProcessor as IInitializer;
            if (initializer != null)
            {
                initializer.Initialize(_settings);
            }
            return tsdbEventProcessor; 
            
        }

        public void Initialize(dynamic settings)
        {
            this._settings = settings; 
        }
    }
}