# Contributing to HeroKit

Thank you for helping. This guide is short. Please read it before you open a PR.

## Prerequisites

- .NET 10 SDK.
- .NET 8 runtime. You need it to run the net8 tests.

## Build and test

```
dotnet build -c Release
dotnet test -c Release
```

## Conventions

- One issue per PR.
- Put `Closes #n` at the start of the PR body.
- Every fix and feature needs tests.
- Public APIs need XML docs.
- Parsing and formatting must be culture-invariant. Take a culture as a parameter if the caller must choose one.

## Public API tracking

HeroKit tracks its public API in text files (Roslyn public API analyzer). The analyzer is being added in a separate PR.

- Add every new public API to `HeroKit/PublicAPI.Unshipped.txt`.
- At release, move those entries to `HeroKit/PublicAPI.Shipped.txt`.

## Obsolete before remove

Do not remove a public API in a 1.x release. Mark it with `[Obsolete]` and name the replacement in the message. Remove it in the next major version.

## Releases

1. Bump `<Version>` in `HeroKit/HeroKit.csproj`.
2. Update `CHANGELOG.md`. Move the Unreleased entries under the new version.
3. Move the `PublicAPI.Unshipped.txt` entries to `PublicAPI.Shipped.txt`.
4. Merge the PR.
5. Publish a GitHub release. Name it and tag it with the version, for example `1.0.3`.

The `nuget-publish.yml` workflow then pushes the package to NuGet with Trusted Publishing.
