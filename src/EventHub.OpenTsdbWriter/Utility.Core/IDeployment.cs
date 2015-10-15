/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    interface IDeployment
    {
        void AddReceiver(IProcessor receiver);
        void AddFilter(string name, IFilter filter, int order);
        void AddFilter(string name, IFilter filter);
        void AddSender(IProcessor sender);
    }
}
