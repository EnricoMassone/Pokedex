﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
  "Reliability",
  "CA2007:Consider calling ConfigureAwait on the awaited task",
  Justification = "There is no SynchronizationContext in ASP.NET core. See https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html",
  Scope = "module")]

[assembly: SuppressMessage(
  "Design",
  "CA1031:Do not catch general exception types",
  Justification = "Here we want to catch any possible error",
  Scope = "member",
  Target = "~M:Pokedex.Api.Program.Main(System.String[])~System.Int32")]
