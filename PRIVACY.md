# Privacy and Personal Data

This document describes how NuciCLI.Arguments at https://github.com/hmlendea/nucicli.arguments handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 📑 Table of Contents

- [What This Document Covers](#-what-this-document-covers)
- [Data We Handle](#-data-we-handle)
- [Processing and Use](#-processing-and-use)
- [Storage, Retention, and Deletion](#-storage-retention-and-deletion)
- [External Processing and Integrations](#-external-processing-and-integrations)
- [Document Changes](#-document-changes)
- [Contact](#-contact)

## 🔎 What This Document Covers

This document describes how NuciCLI.Arguments at https://github.com/hmlendea/nucicli.arguments handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 📥 Data We Handle

### Data Provided to the Application

- No personal data is requested by the library. The library parses command-line arguments provided by the consuming application.

### Data Generated or Collected by the Application

- No personal data is generated or collected automatically by the library.

### Data Received from Integrations

- No personal data is received from integrations or third parties.

## 🧭 Processing and Use

The library processes the data described above for these verified functions:
- Command-line argument parsing — Command-line tokens provided by the consuming application

## 🗄️ Storage, Retention, and Deletion

The library does not store any data. Parsed values are returned to the consuming application via the `ArgumentsCollection` object and are subject to that application's storage, retention, and deletion policies.

## 🔗 External Processing and Integrations

The library has no built-in external data transfer, telemetry, update checks, or third-party integrations.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🔄 Document Changes

Update this document when library data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nucicli.arguments/blob/main/PRIVACY.md.

## 📬 Contact

For questions about library data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nucicli.arguments/issues. Include the library version and a description of the data flow in question; do not send passwords, access tokens, or other secrets.