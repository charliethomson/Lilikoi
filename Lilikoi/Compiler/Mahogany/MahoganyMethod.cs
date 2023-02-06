﻿//       ========================
//       Lilikoi::MahoganyMethod.cs
//       (c) 2023. Distributed under the MIT License
// 
// ->    Created: 22.12.2022
// ->    Bumped: 06.02.2023
//       ========================
#region

using System.Linq.Expressions;
using System.Reflection;

using Lilikoi.Compiler.Mahogany.Generator;
using Lilikoi.Context;

#endregion

namespace Lilikoi.Compiler.Mahogany;

public class MahoganyMethod
{
	public List<ParameterExpression> Temporaries = new();

	public LabelTarget HaltTarget { get; set; }

	/// <summary>
	///     A list of parameters that are to be injected into the method, indexed by the parameter they
	///     will be filling.
	/// </summary>
	public Dictionary<ParameterInfo, ParameterExpression> MethodInjects { get; } = new();

	public Dictionary<string, ParameterExpression> NamedVariables { get; set; } = new();

	public Dictionary<Type, ParameterExpression> Wildcards { get; set; } = new();

	protected List<Expression> Body { get; } = new();

	protected List<Expression> Unordered { get; } = new();

	public ParameterExpression AsHoistedVariable(Expression input)
	{
		var value = CommonGenerator.ToVariable(input, out var variable, $"hoist{Unordered.Count}");

		Unordered.Add(value);
		Temporaries.Add(variable);

		return variable;
	}

	public Expression AsVariable(Expression input, out ParameterExpression variable)
	{
		var value = CommonGenerator.ToVariable(input, out variable, $"var{Temporaries.Count}");

		Temporaries.Add(variable);

		return value;
	}

	public ParameterExpression Named(string name)
	{
		if (!NamedVariables.ContainsKey(name))
			throw new Exception($"named veriable '{name}' not defined.");

		return NamedVariables[name];
	}

	public void Append(Expression block)
	{
		Body.Add(block);
	}

	public LambdaExpression Lambda()
	{
		var func = typeof(Func<,,>).MakeGenericType(Host, Input, Result);
		var internalVariables = new[]
		{
			//Named(MahoganyConstants.HOST_VAR), Named(MahoganyConstants.INPUT_VAR),
			Named(MahoganyConstants.OUTPUT_VAR)
		};

		var parameters = new[]
		{
			Named(MahoganyConstants.HOST_VAR),
			Named(MahoganyConstants.INPUT_VAR)
		};

		var internalBody = Expression.Block(Expression.Block(Unordered), Expression.Block(Body));

		var lambdaBody = Expression.Block(
			internalVariables,
			Expression.Block(Temporaries.ToArray(), internalBody),
			Expression.Return(HaltTarget, Named(MahoganyConstants.OUTPUT_VAR)),
			Expression.Label(HaltTarget, Named(MahoganyConstants.OUTPUT_VAR))
			);

		return Expression.Lambda(lambdaBody, "LilikoiContainer", parameters);
	}

	#region Containerized

	/// <summary>
	///     Parameters to the containerized method.
	/// </summary>
	public List<Type> Parameters { get; set; }

	/// <summary>
	///     The return value of the containerized method
	/// </summary>
	public Type Return { get; set; }

	/// <summary>
	///     The class that will be injected to accomodate the container.
	/// </summary>
	public Type Host { get; set; }

	public MethodInfo Entry { get; set; }

	public Mount Mount { get; set; }

	#endregion

	#region Container

	/// <summary>
	///     The type provided to the container
	/// </summary>
	public Type Input { get; set; }

	/// <summary>
	///     The return type of the container
	/// </summary>
	public Type Result { get; set; }

	#endregion
}