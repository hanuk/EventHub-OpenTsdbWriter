/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
	public class MemoryQueue<TMsg>:IMessageExchnage<TMsg>
	{
		private ConcurrentQueue<TMsg> _queue;
		public MemoryQueue()
		{
			_queue = new ConcurrentQueue<TMsg>();
		}
		#region IMessageExchnageNew<TMsg> Members
		public void Write(TMsg inObj)
		{
			_queue.Enqueue(inObj);
		}

		public TMsg Read()
		{
			TMsg outObj = default(TMsg);
			_queue.TryDequeue(out outObj);
			return outObj;
		}

		public bool IsEmpty()
		{
			if (_queue.Count == 0)
			{
				return true;
			}
			return false; 
		}

		#endregion
	}
}
