/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.OpenTsdbTelnetWriter
{
    public interface ITsdbWriter
    {
        void Write(DataPoint dataPoint);
        void WriteList(List<DataPoint> dataPoints);
    }
}