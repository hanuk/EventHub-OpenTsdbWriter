/*  
Copyright (c) Microsoft.  All rights reserved.  Licensed under the MIT License.  See LICENSE in the root of the repository for license information 
*/
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Common
{
	/// <summary>
	/// Adapted from MSDN samples; if a field is not found, null will be returned
	/// </summary>
	public class DynamicDictionary : DynamicObject
	{
		Dictionary<string, object> _innerDictionary = new Dictionary<string, object>();
		public int Count
		{
			get { return _innerDictionary.Count; }
		}
		/// <summary>
		/// Always returns true to avoid RuntimeBinderException
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			//string name = binder.Name.ToLower();
			_innerDictionary.TryGetValue(binder.Name, out result);
			//ignore the return value as we don't want RuntimeBinderException if an item is not found
			return true;
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			_innerDictionary[binder.Name] = value;
			return true;
		}

		public Dictionary<string, object> InnerDictionary
		{ get { return _innerDictionary; } }
	}
}
