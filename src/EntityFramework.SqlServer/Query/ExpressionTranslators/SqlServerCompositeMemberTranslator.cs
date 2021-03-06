﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Data.Entity.Query.ExpressionTranslators
{
    public class SqlServerCompositeMemberTranslator : RelationalCompositeMemberTranslator
    {
        public SqlServerCompositeMemberTranslator()
        {
            var sqlServerTranslators = new List<IMemberTranslator>
            {
                new StringLengthTranslator(),
                new DateTimeNowTranslator()
            };

            AddTranslators(sqlServerTranslators);
        }
    }
}
