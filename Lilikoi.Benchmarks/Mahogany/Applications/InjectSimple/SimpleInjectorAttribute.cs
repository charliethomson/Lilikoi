﻿//       ========================
//       Lilikoi.Benchmarks::HellInjectorAttribute.cs
//       Distributed under the MIT License.
//
// ->    Created: 22.12.2022
// ->    Bumped: 22.12.2022
//
// ->    Purpose:
//
//
//       ========================
using Lilikoi.Core.Attributes.Typed;

namespace Lilikoi.Benchmarks.Mahogany.Applications.InjectSimple;

public class SimpleInjectorAttribute : LkTypedInjectionAttribute<Simple>
{
	public override Simple Inject()
	{
		return Simple.Create();
	}
}