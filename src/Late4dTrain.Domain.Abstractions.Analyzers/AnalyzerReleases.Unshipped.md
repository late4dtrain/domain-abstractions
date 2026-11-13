# Unshipped analyzer releases

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
L4DA001 | DomainModeling | Warning | Value object state should be immutable.
L4DA002 | DomainModeling | Warning | Domain event state should be immutable.
L4DA003 | DomainModeling | Warning | Entity identity should be read-only.
L4DA004 | DomainModeling | Warning | Aggregate collections should not expose mutable collection types.
L4DA005 | DomainModeling | Warning | IValueObject implementations should supply value equality.
