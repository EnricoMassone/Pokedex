// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "I want to name this type Error", Scope = "type", Target = "~T:Pokedex.Domain.Abstractions.Error")]
[assembly: SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "I want to use operator overload here", Scope = "member", Target = "~M:Pokedex.Domain.Abstractions.Result`1.op_Implicit(`0)~Pokedex.Domain.Abstractions.Result{`0}")]
[assembly: SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "I want to use operator overload here", Scope = "member", Target = "~M:Pokedex.Domain.Abstractions.Result`1.op_Implicit(Pokedex.Domain.Abstractions.Error)~Pokedex.Domain.Abstractions.Result{`0}")]
