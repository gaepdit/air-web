# NuGet Package maintenance

The following NuGet packages have been added directly to work around vulnerable dependencies in other packages.

- `System.Security.Cryptography.Xml` 10.0.10 was added to `AppServices.Core` to avoid a vulnerable version 
  referenced in `Microsoft.Identity.Web`.

- `System.Security.Cryptography.Xml` 10.0.10 was added to `EfRepositoryTests` to avoid a vulnerable version 
  referenced in `EfCore.TestSupport`.

- `SQLitePCLRaw.lib.e_sqlite3` 2.1.12 was added to `EfRepositoryTests` to avoid a vulnerable version 
  referenced in `EfCore.TestSupport` (via `Microsoft.EntityFrameworkCore.Sqlite`).

The following packages have been pinned to work around issues with later versions:

- `NUnit3TestAdapter` was pinned to version 6.2.0 because 6.3.0 [broke compatibility with the test logger](https://github.com/nunit/nunit3-vs-adapter/issues/1505).
