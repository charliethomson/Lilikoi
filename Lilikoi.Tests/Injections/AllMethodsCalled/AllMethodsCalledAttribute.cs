﻿//       ========================
//       Lilikoi.Tests::AllMethodsCalledAttribute.cs
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

using Lilikoi.Core.Attributes.Typed;

#endregion

namespace Lilikoi.Tests.Injections.AllMethodsCalled;

public class AllMethodsCalledAttribute : MkTypedInjectionAttribute<AllMethodsCalledInject>
{
	public AllMethodsCalledTest Test = AllMethodsCalledTest.Instance;

	public override AllMethodsCalledInject Inject()
	{
		Test.InjectCalled = true;

		return new AllMethodsCalledInject();
	}

	public override void Deject(AllMethodsCalledInject injected)
	{
		Test.DejectCalled = true;
	}
}