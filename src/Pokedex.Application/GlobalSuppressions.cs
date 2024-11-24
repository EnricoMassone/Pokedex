// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
  "Performance",
  "CA1848:Use the LoggerMessage delegates",
  Justification = "This code is not performance critical",
  Scope = "module")]

[assembly: SuppressMessage(
  "Design",
  "CA1062:Validate arguments of public methods",
  Justification = "Implicit casts should not throw exceptions",
  Scope = "member",
  Target = "~M:Pokedex.Application.Abstractions.TranslatedDescription.op_Implicit(Pokedex.Application.Abstractions.TranslatedDescription)~System.String")]
