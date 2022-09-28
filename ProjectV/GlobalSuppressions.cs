// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Scope = "namespaceanddescendants", Target = "~N:ProjectV")]
[assembly: SuppressMessage("Major Bug", "S1751:Loops with at most one iteration should be refactored", Scope = "type", Target = "~T:ProjectV.PVConfig")]
