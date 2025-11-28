# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [src\Sudoku\Sudoku.csproj](#srcsudokusudokucsproj)
  - [test\Sudoku.Test\Sudoku.Test.csproj](#testsudokutestsudokutestcsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 2 | All require upgrade |
| Total NuGet Packages | 4 | All compatible |
| Total Code Files | 22 |  |
| Total Code Files with Incidents | 2 |  |
| Total Lines of Code | 2427 |  |
| Total Number of Issues | 2 |  |
| Estimated LOC to modify | 0+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [src\Sudoku\Sudoku.csproj](#srcsudokusudokucsproj) | net8.0 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [test\Sudoku.Test\Sudoku.Test.csproj](#testsudokutestsudokutestcsproj) | net8.0 | 🟢 Low | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 4 | 100.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 0 | 0.0% |
| ***Total NuGet Packages*** | ***4*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1956 |  |
| ***Total APIs Analyzed*** | ***1956*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| coverlet.collector | 6.0.0 |  | [Sudoku.Test.csproj](#testsudokutestsudokutestcsproj) | ✅Compatible |
| Microsoft.NET.Test.Sdk | 17.8.0 |  | [Sudoku.Test.csproj](#testsudokutestsudokutestcsproj) | ✅Compatible |
| xunit | 2.5.3 |  | [Sudoku.Test.csproj](#testsudokutestsudokutestcsproj) | ✅Compatible |
| xunit.runner.visualstudio | 2.5.3 |  | [Sudoku.Test.csproj](#testsudokutestsudokutestcsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;Sudoku.csproj</b><br/><small>net8.0</small>"]
    P2["<b>📦&nbsp;Sudoku.Test.csproj</b><br/><small>net8.0</small>"]
    P2 --> P1
    click P1 "#srcsudokusudokucsproj"
    click P2 "#testsudokutestsudokutestcsproj"

```

## Project Details

<a id="srcsudokusudokucsproj"></a>
### src\Sudoku\Sudoku.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 14
- **Number of Files with Incidents**: 1
- **Lines of Code**: 1367
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P2["<b>📦&nbsp;Sudoku.Test.csproj</b><br/><small>net8.0</small>"]
        click P2 "#testsudokutestsudokutestcsproj"
    end
    subgraph current["Sudoku.csproj"]
        MAIN["<b>📦&nbsp;Sudoku.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#srcsudokusudokucsproj"
    end
    P2 --> MAIN

```

#### Project Technologies and Features

<a id="testsudokutestsudokutestcsproj"></a>
### test\Sudoku.Test\Sudoku.Test.csproj

#### Project Info

- **Current Target Framework:** net8.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 10
- **Number of Files with Incidents**: 1
- **Lines of Code**: 1060
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Sudoku.Test.csproj"]
        MAIN["<b>📦&nbsp;Sudoku.Test.csproj</b><br/><small>net8.0</small>"]
        click MAIN "#testsudokutestsudokutestcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;Sudoku.csproj</b><br/><small>net8.0</small>"]
        click P1 "#srcsudokusudokucsproj"
    end
    MAIN --> P1

```

#### Project Technologies and Features

