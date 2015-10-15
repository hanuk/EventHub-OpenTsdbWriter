﻿/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface IMessage
    {
        string GetId();
        string GetValue(string fieldName);
        void Add(string fieldName, string fieldValue);
        void Add(string fieldName, string fieldValue, bool isId);
        string Serialize();
    }
}
