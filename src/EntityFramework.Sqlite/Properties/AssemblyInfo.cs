// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using System.Resources;
<<<<<<< HEAD

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyMetadata("Serviceable", "True")]
=======
using Microsoft.Data.Entity.Infrastructure;

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: DesignTimeProviderServices(
    typeName: "Microsoft.Data.Entity.Sqlite.Design.ReverseEngineering.SqliteDesignTimeMetadataProviderFactory",
    assemblyName: "EntityFramework.Sqlite.Design")]
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
