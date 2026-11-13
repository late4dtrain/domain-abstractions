# Feature workflow

Adapted from the sibling Route66 repository.

- For feature requests in Plan mode, create or update `.agents/plans/feat/<feature-name>.md` before presenting the plan. Include objectives, acceptance criteria, public API and target-framework decisions, implementation checklist, dependencies, verification, and open questions. Wait for approval before implementing library code.
- For an approved feature plan, read it before implementation. Check the branch and worktree, then create `feature/<feature-name>` from the current branch only when safe. If the branch already exists or switching risks existing changes, ask rather than overwrite or repurpose it. Verify acceptance criteria before marking tasks complete.
- Preserve the `netstandard2.0` public API unless the approved scope changes it. Call out changes to equality, hash codes, inheritance, generic constraints, and event collection ownership in the plan.
- Commit, push, or destructive Git operations require explicit user authorization.
