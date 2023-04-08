# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.0] - 2023-04-08

### Added

- Add useful extensions for results.
- Add trace result for retryable and failure results.
- Add try extensions for results.

### Changed

- Simplify package structure.
- Rename result factories.
- Improve memory allocation of Resilience.
- Replace `Task` to `UniTask` in Resilience.

## [0.1.3] - 2023-03-28

### Added

- Add extension for UniTask.

## [0.1.2] - 2023-03-28

### Added

- Add extension for Newtonsoft.Json.

## [0.1.1] - 2023-03-20

### Changed

- Rename `RetryFactory.RetryWithWait` to `RetryFactory.RetryWithInterval`.

## [0.1.0] - 2023-03-19

### Added

- Add Result.
- Add UncertainResult.
- Add Resilience.