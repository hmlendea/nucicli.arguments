# Security Policy

This policy outlines the security practices and procedures for reporting vulnerabilities in **NuciCLI.Arguments**.

## 📑 Table of Contents

- [Supported Versions](#-supported-versions)
- [Reporting a Vulnerability](#-reporting-a-vulnerability)
- [Scope](#-scope)
- [Disclosure Policy](#-disclosure-policy)
- [Automated Security Scanning](#-automated-security-scanning)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | GitHub Releases | ✅ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/nucicli.arguments/security/advisories)
- Contact the maintainers directly at **security@nucicli.com**

Include the following details in your report:
- A description of the vulnerability.
- Steps to reproduce the issue.
- The version of NuciCLI.Arguments you are using.
- Any relevant code snippets or logs.

We will acknowledge your report within **24 hours** and provide updates on the progress of the fix.

## 📌 Scope

The original maintainers are not responsible for changes, modifications, or security issues introduced by downstream forks.

The subsequent report categories are in scope for this repository:
- Code execution vulnerabilities.
- Security misconfigurations.
- Dependency vulnerabilities.

The subsequent categories are out of scope unless explicitly stated to the contrary:
- Vulnerabilities in third-party libraries not directly maintained by this project.
- Physical access attacks.
- Social engineering attacks.

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.

## 🔍 Automated Security Scanning

This repository runs automated security scans on every push and pull request to `master`, and on a weekly schedule. The scan includes:
- **Vulnerable NuGet packages** Reported with the complete transitive dependency graph through `dotnet list package --vulnerable --include-transitive`.
- **CodeQL static analysis** Performed on the C# source with the `github/codeql-action` workflow and uploaded as SARIF results.

Results are published to the [Security](https://github.com/hmlendea/nucicli.arguments/security) tab and the [Actions](https://github.com/hmlendea/nucicli.arguments/actions/workflows/security.yml) workflow page.