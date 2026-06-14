# Security

## Supported Versions

BadMelon is currently in a rewrite phase. Supported versions will be defined once the first release is available.

## Reporting A Vulnerability

Please do not open public issues for suspected security vulnerabilities.

Until a dedicated security contact is chosen, report vulnerabilities privately to the repository owner.

## Security Expectations

- Use JWT authentication for API access.
- Enforce authorization on every family-scoped resource.
- Store passwords using strong one-way hashing through established platform libraries.
- Validate input on the server.
- Keep secrets out of source control.
- Avoid rendering untrusted recipe content as raw HTML.
- Review dependencies regularly.

