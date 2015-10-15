/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Core
{
	public interface IMessageExchnage<TMsg>
	{
		void Write(TMsg inObj);
		TMsg Read();
		bool IsEmpty();
	}
}
