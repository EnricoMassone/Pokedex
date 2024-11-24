// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
  "Naming",
  "CA1707:Identifiers should not contain underscores",
  Justification = "I use underscore as a separator in unit test names",
  Scope = "module")]

[assembly: SuppressMessage(
  "Design",
  "CA1062:Validate arguments of public methods",
  Justification = "We can skip validation because this argument is provided by Autofixture",
  Scope = "member",
  Target = "~M:Pokedex.Application.UnitTests.Pokemons.GetPokemonQueryHandlerTests.Handle_Returns_Response_Based_On_Pokemon_Returned_By_Repository(Pokedex.Domain.Pokemons.Name,Pokedex.Domain.Pokemons.Pokemon,System.Threading.CancellationToken)~System.Threading.Tasks.Task")]

[assembly: SuppressMessage(
  "Design",
  "CA1062:Validate arguments of public methods",
  Justification = "We can skip validation because this argument is provided by Autofixture",
  Scope = "type",
  Target = "~T:Pokedex.Application.UnitTests.Translations.Strategies.YodaPokemonDescriptionTranslationStrategyTests")]

[assembly: SuppressMessage(
  "Performance",
  "CA1848:Use the LoggerMessage delegates",
  Justification = "This project does not use LoggerMessage delegates",
  Scope = "module")]
