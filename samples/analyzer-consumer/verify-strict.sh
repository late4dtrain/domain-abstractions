#!/usr/bin/env bash
set -euo pipefail

project="samples/analyzer-consumer/AnalyzerConsumer.csproj"
violation="samples/analyzer-consumer/StrictViolation.cs"
output="$(mktemp)"
trap 'rm -f "$violation" "$output"' EXIT

cp samples/analyzer-consumer/StrictViolation.cs.template "$violation"
if dotnet build "$project" --no-restore >"$output" 2>&1; then
    cat "$output"
    echo "Expected strict analyzer diagnostics, but the consumer build succeeded." >&2
    exit 1
fi

if ! grep -q "error L4DA001" "$output"; then
    cat "$output"
    echo "Consumer failed, but not for the expected L4DA001 diagnostic." >&2
    exit 1
fi

cat "$output"
