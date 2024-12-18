﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "I want to name this type Error", Scope = "type", Target = "~T:Pokedex.Domain.Abstractions.Error")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "I want to name this type Option", Scope = "type", Target = "~T:Pokedex.Domain.Abstractions.Option`1")]
[assembly: SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "I want to name this type Option", Scope = "type", Target = "~T:Pokedex.Domain.Abstractions.Option")]
[assembly: SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "I want to use operator overload here", Scope = "member", Target = "~M:Pokedex.Domain.Abstractions.Option`1.op_Implicit(`0)~Pokedex.Domain.Abstractions.Option{`0}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Implicit conversion operator should never throw exceptions", Scope = "member", Target = "~M:Pokedex.Domain.Pokemons.Name.op_Implicit(Pokedex.Domain.Pokemons.Name)~System.String")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Implicit conversion operator should never throw exceptions", Scope = "member", Target = "~M:Pokedex.Domain.Abstractions.Text.op_Implicit(Pokedex.Domain.Abstractions.Text)~System.String")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Implicit casts should not throw exceptions", Scope = "member", Target = "~M:Pokedex.Domain.Abstractions.TranslatedDescription.op_Implicit(Pokedex.Domain.Abstractions.TranslatedDescription)~System.String")]

[assembly: SuppressMessage(
  "Naming",
  "CA1716:Identifiers should not match keywords",
  Justification = "I want to name this type Text",
  Scope = "type",
  Target = "~T:Pokedex.Domain.Abstractions.Text")]
