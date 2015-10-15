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
	public class FieldValidator
	{
		/// <summary>
		/// A collection of Name-Value pairs
		/// </summary>
		/// <param name="fieldCollection"></param>
		/// <param name="fieldNames"></param>
		public static void CheckMissingFields(dynamic fieldCollection, string[] fieldNames)
		{
			List<string> errors = new List<string>();
			foreach (var fieldName in fieldNames)
			{
				object fieldValue = null;
				try
				{
					fieldValue = fieldCollection.InnerDictionary[fieldName];
				}
				catch(KeyNotFoundException)
				{
					fieldValue = null; 
				}
				if (fieldValue == null)
				{
					errors.Add(string.Format("{0} is missing", fieldName));
				}
			}

			if (errors.Count > 0)
			{
				ValidationException ex = new ValidationException("Invalid settings");
				ex.Errors = errors;
				throw ex;
			}
		}
	}
}
