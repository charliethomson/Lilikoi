﻿//       ========================
//       Lilikoi.Tests::AllMethodsCalledHost.cs
//       Distributed under the MIT License.
// 
// ->    Created: 22.12.2022
// ->    Bumped: 22.12.2022
// 
// ->    Purpose:
// 
// 
//       ========================
namespace Lilikoi.Tests.Injections.AllMethodsCalled;

public class AllMethodsCalledHost
{
	public AllMethodsCalledTest Test { get; set; }

	[AllMethodsCalled]
	public AllMethodsCalledInject Inject { get; set; }

	public object Entry()
	{
		Test.EntryCalled = true;

		Assert.IsNotNull(Inject);
		Assert.IsTrue(Inject.IsNotNull());

		return new object();
	}
}