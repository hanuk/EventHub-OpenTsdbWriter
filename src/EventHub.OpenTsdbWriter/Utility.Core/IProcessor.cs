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
	public interface IProcessor
	{
		void ConfigureSettings(dynamic settings);
		void Initialize(dynamic settings);
		void Initialize();
		void Start();
		void Stop();
	}
}
