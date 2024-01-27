// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S3241:Methods should not return values that are never used", Justification = "PInvoke 함수", Scope = "member", Target = "~M:SimpleVhd.Firmware.#cctor")]
