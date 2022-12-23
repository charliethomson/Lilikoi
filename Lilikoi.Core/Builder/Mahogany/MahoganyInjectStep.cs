﻿//       ========================
//       Lilikoi.Core::MahoganyInjectStep.cs
//       Distributed under the MIT License.
// 
// ->    Created: 22.12.2022
// ->    Bumped: 22.12.2022
// 
// ->    Purpose:
// 
// 
//       ========================
#region

using System.Linq.Expressions;
using System.Reflection;

using Lilikoi.Core.Attributes.Builders;
using Lilikoi.Core.Generator;

#endregion

namespace Lilikoi.Core.Builder.Mahogany;

public class MahoganyInjectStep
{
	public MahoganyInjectStep(MahoganyMethod method, PropertyInfo propertyInfo, MkInjectionBuilderAttribute builder)
	{
		Method = method;
		PropertyInfo = propertyInfo;
		Builder = builder;
	}

	public MahoganyMethod Method { get; set; }

	public PropertyInfo PropertyInfo { get; set; }

	public MkInjectionBuilderAttribute Builder { get; set; }

	public (Expression, Expression) Generate()
	{
		var instance = InjectionGenerator.Builder(Builder);

		var entry =
			InjectionGenerator.InjectValueAsProperty(Method, instance, Method.Named(MahoganyConstants.HOST_VAR), PropertyInfo);

		var exit =
			InjectionGenerator.DejectValueAsProperty(instance, Method.Named(MahoganyConstants.HOST_VAR), PropertyInfo);

		return (entry, exit);
	}
}