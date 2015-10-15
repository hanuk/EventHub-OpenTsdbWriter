/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Common
{
    public class MessageFormatException:ApplicationException
    {
        public MessageFormatException(): base() { }
        public MessageFormatException(string message) : base(message) { }
        public MessageFormatException(string message, Exception e) : base(message, e) { }

    }
}
