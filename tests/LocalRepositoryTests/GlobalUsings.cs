global using AwesomeAssertions;
global using AwesomeAssertions.Execution;
global using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
