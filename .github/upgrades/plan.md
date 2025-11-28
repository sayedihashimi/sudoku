# .NET Upgrade Plan (Sudoku) - Target net10.0

## 1. Executive Summary
- Scenario: Upgrade all projects in the Sudoku solution from net8.0 to net10.0.
- Scope: 2 projects require upgrade.
  - `src/Sudoku/Sudoku.csproj` (ClassLibrary, SDK-style, net8.0)
  - `test/Sudoku.Test/Sudoku.Test.csproj` (DotNetCoreApp, SDK-style, net8.0)
- Target State: Both projects target `net10.0`. Packages remain compatible per assessment.

- Selected Strategy: Big Bang Strategy – All projects upgraded simultaneously in single operation.
  - Rationale:
    - Small solution (2 projects)
    - Simple dependency graph: tests depend on library; no circular deps
    - Assessment shows packages compatible with net10.0; no suggested updates

- Complexity Assessment: Low
  - 2 projects, SDK-style, LOC ~2,427; no API incompatibilities detected, no package upgrade requirements.

- Critical Issues: None reported. No security vulnerabilities found.

- Recommended Approach: Big Bang Strategy due to low complexity and simple dependency chain.

## 2. Migration Strategy

### 2.1 Approach Selection
- Chosen Strategy: Big Bang Strategy.
- Strategy Rationale: Minimal projects, homogeneous TFMs (net8.0 → net10.0), clear dependency structure.
- Strategy-Specific Considerations:
  - Atomic operation for framework and package updates across all projects.
  - Single commit preferred for framework and package updates.
  - Build and fix compilation errors in the same unified pass.

### 2.2 Dependency-Based Ordering
- Dependency graph:
  - `Sudoku.Test.csproj` → depends on `Sudoku.csproj`.
- Ordering consideration (contextual only; execution remains atomic):
  - Leaf: `Sudoku.csproj`
  - Root: `Sudoku.Test.csproj`
- No circular dependencies.

### 2.3 Parallel vs Sequential Execution
- Under Big Bang, all updates are performed simultaneously. Build verification occurs for entire solution at once.

## 3. Detailed Dependency Analysis

### 3.1 Dependency Graph Summary
- Projects:
  - `src/Sudoku/Sudoku.csproj` (leaf)
  - `test/Sudoku.Test/Sudoku.Test.csproj` (depends on `Sudoku.csproj`)

### 3.2 Project Groupings
- Phase 0: Preparation
  - Validate .NET 10 SDK availability; update `global.json` if present.
- Phase 1: Atomic Upgrade (all projects simultaneously)
- Phase 2: Tests and validation

## 4. Project-by-Project Migration Plans

### Project: src/Sudoku/Sudoku.csproj

**Current State**
- Dependencies: none
- Dependants: `test/Sudoku.Test/Sudoku.Test.csproj`
- Package Count: 0 (no external NuGet packages listed)
- LOC: 1367

**Target State**
- Target Framework: net10.0
- Updated Packages: N/A

**Migration Steps**
1. Prerequisites
   - Ensure .NET 10 SDK installed and CI images support net10.0.
2. Framework Update
   - Update `TargetFramework` in `src/Sudoku/Sudoku.csproj` from `net8.0` to `net10.0`.
3. Package Updates
   - None required per assessment.
4. Expected Breaking Changes
   - None flagged. Validate for potential minor API changes after build.
5. Code Modifications
   - Address any obsolete warnings surfaced after retargeting.
6. Testing Strategy
   - Build with no warnings/errors; rely on test project for functional validation.
7. Validation Checklist
   - [ ] Dependencies resolve correctly
   - [ ] Builds without errors
   - [ ] Builds without warnings
   - [ ] No security warnings

---

### Project: test/Sudoku.Test/Sudoku.Test.csproj

**Current State**
- Dependencies: `src/Sudoku/Sudoku.csproj`
- Dependants: none
- Packages: `coverlet.collector (6.0.0)`, `Microsoft.NET.Test.Sdk (17.8.0)`, `xunit (2.5.3)`, `xunit.runner.visualstudio (2.5.3)`
- LOC: 1060

**Target State**
- Target Framework: net10.0
- Updated Packages: None required per assessment (all marked compatible).

**Migration Steps**
1. Prerequisites
   - Ensure test SDK tooling supports net10.0 on build and CI.
2. Framework Update
   - Update `TargetFramework` in `test/Sudoku.Test/Sudoku.Test.csproj` from `net8.0` to `net10.0`.
3. Package Updates
   | Package | Current Version | Target Version | Reason |
   |---------|-----------------|----------------|--------|
   | coverlet.collector | 6.0.0 | (no change) | Compatible with net10.0 |
   | Microsoft.NET.Test.Sdk | 17.8.0 | (no change) | Compatible with net10.0 |
   | xunit | 2.5.3 | (no change) | Compatible with net10.0 |
   | xunit.runner.visualstudio | 2.5.3 | (no change) | Compatible with net10.0 |
4. Expected Breaking Changes
   - None flagged; verify test discovery/execution with net10.0.
5. Code Modifications
   - Adjust any test code if new nullable/SDK behaviors surface.
6. Testing Strategy
   - Run all unit tests.
   - Ensure coverage collection works with net10.0.
7. Validation Checklist
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] All unit tests pass
   - [ ] No security warnings

## 5. Risk Management

### 5.1 High-Risk Changes
- Overall risk: Low. Potential risk areas:
  - CI/CD pipeline compatibility with .NET 10 SDK.
  - Any latent API changes not detected by static assessment.

| Project | Risk | Mitigation |
|---------|------|------------|
| Sudoku.csproj | Low | Build solution and fix compile issues immediately in atomic pass |
| Sudoku.Test.csproj | Low | Validate test discovery/execution; update test SDK if needed |

### 5.3 Contingency Plans
- If CI images lack .NET 10 SDK: update pipeline images or use global.json to pin SDK.
- If test SDK/xUnit versions show runtime issues: bump to latest compatible minor versions for net10.0.

## 6. Testing and Validation Strategy

### 6.1 Phase-by-Phase Testing
- Phase 1 (Atomic Upgrade): Build entire solution, resolve any compile errors.
- Phase 2: Run all tests; validate coverage and test discovery.

### 6.2 Smoke Tests
- Confirm build success for entire solution.
- Run a subset of quick tests to confirm harness works.

### 6.3 Comprehensive Validation
- All unit tests pass.
- No new warnings or errors.
- Security scan clean.

## 7. Timeline and Effort Estimates

| Project | Complexity | Estimated Time | Dependencies | Risk Level |
|---------|------------|----------------|--------------|------------|
| Sudoku.csproj | Low | Short | None | Low |
| Sudoku.Test.csproj | Low | Short | Sudoku.csproj | Low |

## 8. Source Control Strategy

### 8.1 Strategy-Specific Guidance
- Big Bang: prefer a single commit for framework and package updates across all projects.

### 8.2 Branching Strategy
- Main upgrade branch: `upgrade-to-NET10` (created from `master`).

### 8.3 Commit Strategy
- Single atomic commit for TFM changes and necessary fixes.
- Commit message template: "Upgrade all projects to net10.0 (Big Bang): update TargetFrameworks, fix compile issues, verify tests".

### 8.4 Review and Merge Process
- Create PR from `upgrade-to-NET10` to `master`.
- Review checklist:
  - Builds succeed, tests pass, CI updated.
  - No new warnings/errors.
  - No security vulnerabilities.

## 9. Success Criteria

### 9.1 Strategy-Specific Success Criteria
- Atomic upgrade completed; solution builds with 0 errors.

### 10.2 Technical Success Criteria
- [ ] All projects migrated to net10.0
- [ ] All builds succeed without errors
- [ ] All builds succeed without warnings
- [ ] All automated tests pass
- [ ] Zero security vulnerabilities in dependencies

### 10.3 Quality Criteria
- [ ] Code quality maintained or improved
- [ ] Test coverage maintained or improved
- [ ] Documentation updated (README/CI notes)

### 10.4 Process Criteria
- [ ] Big Bang principles followed (single coordinated upgrade)
- [ ] Source control strategy followed with appropriate commits

## 10. Atomic Upgrade Task (Executor Reference)
- Update TargetFramework in all projects to `net10.0`.
- Restore dependencies.
- Build solution and fix all compilation errors.
- Run tests, ensure pass.
- Single commit for all changes.
