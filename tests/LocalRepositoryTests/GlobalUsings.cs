global using FluentAssertions;
global using FluentAssertions.Execution;
global using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
