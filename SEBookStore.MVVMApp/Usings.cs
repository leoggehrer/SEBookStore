//@CodeCopy

#if IDINT_ON
global using IdType = System.Int32;
#elif IDLONG_ON
global using IdType = System.Int64;
#elif IDGUID_ON
global using IdType = System.Guid;
#else
global using IdType = System.Int32;
#endif

global using Common = SEBookStore.Common;
global using CommonContracts = SEBookStore.Common.Contracts;
global using CommonModels = SEBookStore.Common.Models;
global using CommonModules = SEBookStore.Common.Modules;
global using SEBookStore.Common.Extensions;

