﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;
using Microsoft.Data.Entity.Query.ExpressionVisitors;
using Microsoft.Data.Entity.Query.Internal;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Query
{
    public class SqlServerQueryModelVisitorFactory : RelationalQueryModelVisitorFactory
    {
        public SqlServerQueryModelVisitorFactory(
            [NotNull] IModel model,
            [NotNull] IQueryOptimizer queryOptimizer,
            [NotNull] INavigationRewritingExpressionVisitorFactory navigationRewritingExpressionVisitorFactory,
            [NotNull] ISubQueryMemberPushDownExpressionVisitor subQueryMemberPushDownExpressionVisitor,
            [NotNull] IQuerySourceTracingExpressionVisitorFactory querySourceTracingExpressionVisitorFactory,
            [NotNull] IEntityResultFindingExpressionVisitorFactory entityResultFindingExpressionVisitorFactory,
            [NotNull] ITaskBlockingExpressionVisitor taskBlockingExpressionVisitor,
            [NotNull] IMemberAccessBindingExpressionVisitorFactory memberAccessBindingExpressionVisitorFactory,
            [NotNull] IOrderingExpressionVisitorFactory orderingExpressionVisitorFactory,
            [NotNull] IProjectionExpressionVisitorFactory projectionExpressionVisitorFactory,
            [NotNull] IEntityQueryableExpressionVisitorFactory entityQueryableExpressionVisitorFactory,
            [NotNull] IQueryAnnotationExtractor queryAnnotationExtractor,
            [NotNull] IResultOperatorHandler resultOperatorHandler,
            [NotNull] IEntityMaterializerSource entityMaterializerSource,
            [NotNull] IExpressionPrinter expressionPrinter,
            [NotNull] IRelationalMetadataExtensionProvider relationalMetadataExtensionProvider,
            [NotNull] IIncludeExpressionVisitorFactory includeExpressionVisitorFactory,
            [NotNull] ISqlTranslatingExpressionVisitorFactory sqlTranslatingExpressionVisitorFactory,
            [NotNull] ICompositePredicateExpressionVisitorFactory compositePredicateExpressionVisitorFactory,
            [NotNull] IQueryFlatteningExpressionVisitorFactory queryFlatteningExpressionVisitorFactory,
            [NotNull] IShapedQueryFindingExpressionVisitorFactory shapedQueryFindingExpressionVisitorFactory,
            [NotNull] IDbContextOptions options)
            : base(
                model,
                queryOptimizer,
                navigationRewritingExpressionVisitorFactory,
                subQueryMemberPushDownExpressionVisitor,
                querySourceTracingExpressionVisitorFactory,
                entityResultFindingExpressionVisitorFactory,
                taskBlockingExpressionVisitor,
                memberAccessBindingExpressionVisitorFactory,
                orderingExpressionVisitorFactory,
                projectionExpressionVisitorFactory,
                entityQueryableExpressionVisitorFactory,
                queryAnnotationExtractor,
                resultOperatorHandler,
                entityMaterializerSource,
                expressionPrinter,
                relationalMetadataExtensionProvider,
                includeExpressionVisitorFactory,
                sqlTranslatingExpressionVisitorFactory,
                compositePredicateExpressionVisitorFactory,
                queryFlatteningExpressionVisitorFactory,
                shapedQueryFindingExpressionVisitorFactory)
        {
            Check.NotNull(options, nameof(options));

            ContextOptions = options;
        }

        protected virtual IDbContextOptions ContextOptions { get; }

        public override EntityQueryModelVisitor Create(
            QueryCompilationContext queryCompilationContext,
            EntityQueryModelVisitor parentEntityQueryModelVisitor)
            =>
                new SqlServerQueryModelVisitor(
                    Model,
                    QueryOptimizer,
                    NavigationRewritingExpressionVisitorFactory,
                    SubQueryMemberPushDownExpressionVisitor,
                    QuerySourceTracingExpressionVisitorFactory,
                    EntityResultFindingExpressionVisitorFactory,
                    TaskBlockingExpressionVisitor,
                    MemberAccessBindingExpressionVisitorFactory,
                    OrderingExpressionVisitorFactory,
                    ProjectionExpressionVisitorFactory,
                    EntityQueryableExpressionVisitorFactory,
                    QueryAnnotationExtractor,
                    ResultOperatorHandler,
                    EntityMaterializerSource,
                    ExpressionPrinter,
                    RelationalMetadataExtensionProvider,
                    IncludeExpressionVisitorFactory,
                    SqlTranslatingExpressionVisitorFactory,
                    CompositePredicateExpressionVisitorFactory,
                    QueryFlatteningExpressionVisitorFactory,
                    ShapedQueryFindingExpressionVisitorFactory,
                    ContextOptions,
                    (RelationalQueryCompilationContext)Check.NotNull(queryCompilationContext, nameof(queryCompilationContext)),
                    (SqlServerQueryModelVisitor)parentEntityQueryModelVisitor);
    }
}
