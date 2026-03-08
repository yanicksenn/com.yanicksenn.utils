# Feature add-enumerable-union-extension

Extend `IEnumerable<T>` with a method that allows appending a dynamic amount of additional elements, functioning similarly to `Union` or `Concat`.

# Implementation Plan

1. Analyze `Runtime/Extensions/EnumerableExtensions.cs` to understand the current structure and namespace.
2. Add a new extension method, e.g., `public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] elements)` or `Union<T>(this IEnumerable<T> source, params T[] elements)` to `EnumerableExtensions.cs`. Wait, if we use `Union`, we probably should filter duplicates since that's what `Union` implies, or maybe the user meant `Append` or `Concat`. I will name it `Union` since the user mentioned "Union but with dynamic amount of additional elements", though `params T[]` is key here. 
3. Create tests in `Tests/Runtime/Extensions/EnumerableExtensionsTests.cs` (or similar) to ensure the newly added method handles standard arrays, empty arrays, nulls, and duplicate filtering correctly as `Union` does.

# Testing Strategy

- Provide unit tests validating the correct behavior of the new `Union` or `Concat` extension using `params T[]`.
- Verify duplicates are handled correctly (i.e. if it's called `Union`, duplicates between `source` and `elements` are omitted, and duplicates within `elements` are omitted).

# Tasks

* Extend Enumerable Extensions
* [x] Analyze `EnumerableExtensions.cs`
* [x] Implement extension method with `params T[]` in `EnumerableExtensions.cs`
* [x] Add unit tests for the new extension method
* [x] Run tests to validate behavior