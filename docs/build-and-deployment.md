# Build and deployment

## Build pipeline

### Local build

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build --no-restore

# Run tests
dotnet test --no-build --verbosity normal
```

### CI pipeline (`.github/workflows/dotnet.yml`)

**Trigger:** Push or PR to `master` branch

**Jobs:**
1. **Build** (ubuntu-latest)
   - Checkout (actions/checkout@v4)
   - Setup .NET 10.0.x (actions/setup-dotnet@v4)
   - Restore dependencies (`dotnet restore`)
   - Build (`dotnet build --no-restore`)
   - Test (`dotnet test --no-build --verbosity normal`)

**No deployment** in this workflow — only validation.

## Release pipeline

Two workflows triggered on **GitHub Release published**:

### NuGet.org release (`.github/workflows/nuget-release.yml`)

**Permissions:** `contents: read`, `id-token: write` (for OIDC NuGet auth)

**Steps:**
1. Checkout with full history (`fetch-depth: 0`)
2. Setup .NET 10.0.x
3. Extract version from tag (strip `v` prefix)
4. Restore dependencies
5. Pack Release build (`dotnet pack -c Release --output ./nupkg -p:Version=<version>`)
6. NuGet login via OIDC (`NuGet/login@v1`)
7. Push to NuGet.org (`dotnet nuget push ... --source https://api.nuget.org/v3/index.json --skip-duplicate`)

### GitHub Release upload (`.github/workflows/github-release.yml`)

**Permissions:** `contents: write`

**Steps:**
1. Checkout with full history
2. Setup .NET 10.0.x (uses v5 of setup-dotnet)
3. Extract version from tag
4. Restore dependencies
5. Pack Release build
6. Calculate SHA256 checksum of `.nupkg`
7. Upload package to GitHub Release (`gh release upload ... --clobber`)
8. Append SHA256 to release notes via GitHub API

## Versioning

- **Source of truth:** `<Version>` in `NuciCLI.Arguments.csproj`
- **Release tag format:** `v<version>` (e.g., `v1.0.3`)
- **Workflow extraction:** Strips leading `v` from tag name
- **Scheme:** Semantic versioning (major.minor.patch)

## Artifacts

| Artifact | Produced by | Published to |
|----------|-------------|--------------|
| `NuciCLI.Arguments.<version>.nupkg` | `dotnet pack` | NuGet.org, GitHub Releases |

## Environments

| Environment | Purpose | Configuration |
|-------------|---------|---------------|
| Development | Local development | `dotnet build` (Debug) |
| CI | Validation | `dotnet build --no-restore` (Release) |
| Release | Package creation | `dotnet pack -c Release` |

No staging/production deployment — library is consumed via NuGet.

## Dependencies

### Build-time

- .NET 10.0 SDK
- NuGet packages (restored via `dotnet restore`)

### Runtime

- .NET 10.0 runtime (or compatible)
- `NuciExtensions` 6.0.2 (transitive)

## Rollback

- **NuGet.org:** Unlist package version via NuGet.org UI or `dotnet nuget delete`
- **GitHub Releases:** Delete release and re-create, or edit release assets

No automated rollback — manual process.

## Security

- NuGet publish uses OIDC token (no API key stored in repo)
- GitHub Release uses `GITHUB_TOKEN` (scoped to repo)
- No secrets in workflow files
- Dependency versions pinned in `.csproj`