// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System.Diagnostics;
using System.Linq.Expressions;
using JetBrains.Annotations;
=======
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata.Internal;
using Microsoft.Data.Entity.Query.Expressions;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Utilities;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
<<<<<<< HEAD
=======
using Remotion.Linq.Clauses.StreamedData;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

namespace Microsoft.Data.Entity.Query.ExpressionVisitors
{
    public class RelationalProjectionExpressionVisitor : ProjectionExpressionVisitor
    {
<<<<<<< HEAD
        private readonly IQuerySource _querySource;

        private readonly SqlTranslatingExpressionVisitor _sqlTranslatingExpressionVisitor;

        private bool _requiresClientEval;

        public RelationalProjectionExpressionVisitor(
=======
        private readonly ISqlTranslatingExpressionVisitorFactory _sqlTranslatingExpressionVisitorFactory;
        private readonly IEntityMaterializerSource _entityMaterializerSource;
        private readonly RelationalQueryModelVisitor _queryModelVisitor;
        private readonly IQuerySource _querySource;

        public RelationalProjectionExpressionVisitor(
            [NotNull] ISqlTranslatingExpressionVisitorFactory sqlTranslatingExpressionVisitorFactory,
            [NotNull] IEntityMaterializerSource entityMaterializerSource,
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            [NotNull] RelationalQueryModelVisitor queryModelVisitor,
            [NotNull] IQuerySource querySource)
            : base(Check.NotNull(queryModelVisitor, nameof(queryModelVisitor)))
        {
<<<<<<< HEAD
            _querySource = querySource;

            _sqlTranslatingExpressionVisitor
                = new SqlTranslatingExpressionVisitor(queryModelVisitor);
=======
            Check.NotNull(sqlTranslatingExpressionVisitorFactory, nameof(sqlTranslatingExpressionVisitorFactory));
            Check.NotNull(entityMaterializerSource, nameof(entityMaterializerSource));
            Check.NotNull(querySource, nameof(querySource));

            _sqlTranslatingExpressionVisitorFactory = sqlTranslatingExpressionVisitorFactory;
            _entityMaterializerSource = entityMaterializerSource;
            _queryModelVisitor = queryModelVisitor;
            _querySource = querySource;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }

        private new RelationalQueryModelVisitor QueryModelVisitor
            => (RelationalQueryModelVisitor)base.QueryModelVisitor;

<<<<<<< HEAD
        public virtual bool RequiresClientEval => _requiresClientEval;

        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
=======
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            Check.NotNull(methodCallExpression, nameof(methodCallExpression));

>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            if (methodCallExpression.Method.IsGenericMethod)
            {
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition();

                if (ReferenceEquals(methodInfo, EntityQueryModelVisitor.PropertyMethodInfo))
                {
                    var newArg0 = Visit(methodCallExpression.Arguments[0]);

                    if (newArg0 != methodCallExpression.Arguments[0])
                    {
                        return Expression.Call(
                            methodCallExpression.Method,
                            newArg0,
                            methodCallExpression.Arguments[1]);
                    }

                    return methodCallExpression;
                }
            }

            return base.VisitMethodCall(methodCallExpression);
        }

<<<<<<< HEAD
        public override Expression Visit(Expression expression)
        {
            if (expression != null
                && !(expression is ConstantExpression))
            {
                var sqlExpression
                    = _sqlTranslatingExpressionVisitor.Visit(expression);
=======
        protected override Expression VisitNew(NewExpression newExpression)
        {
            Check.NotNull(newExpression, nameof(newExpression));

            var newNewExpression = base.VisitNew(newExpression);

            var selectExpression = QueryModelVisitor.TryGetQuery(_querySource);

            if (selectExpression != null)
            {
                for (var i = 0; i < newExpression.Arguments.Count; i++)
                {
                    var aliasExpression
                        = selectExpression.Projection
                            .OfType<AliasExpression>()
                            .SingleOrDefault(ae => ae.SourceExpression == newExpression.Arguments[i]);

                    if (aliasExpression != null)
                    {
                        aliasExpression.SourceMember
                            = newExpression.Members?[i]
                              ?? (newExpression.Arguments[i] as MemberExpression)?.Member;
                    }
                }
            }

            return newNewExpression;
        }

        public override Expression Visit(Expression expression)
        {
            var selectExpression = QueryModelVisitor.TryGetQuery(_querySource);

            if (expression != null
                && !(expression is ConstantExpression)
                && selectExpression != null)
            {
                var sqlExpression
                    = _sqlTranslatingExpressionVisitorFactory
                        .Create(QueryModelVisitor, selectExpression, inProjection: true)
                        .Visit(expression);
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                if (sqlExpression == null)
                {
                    if (!(expression is QuerySourceReferenceExpression))
                    {
<<<<<<< HEAD
                        _requiresClientEval = true;
=======
                        QueryModelVisitor.RequiresClientProjection = true;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
                    }
                }
                else
                {
<<<<<<< HEAD
                    var selectExpression
                        = QueryModelVisitor.TryGetQuery(_querySource);

                    Debug.Assert(selectExpression != null);

                    if (!(expression is NewExpression))
                    {
=======
                    if (!(expression is NewExpression))
                    {
                        AliasExpression aliasExpression;

                        int index;

>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
                        if (!(expression is QuerySourceReferenceExpression))
                        {
                            var columnExpression = sqlExpression.TryGetColumnExpression();

                            if (columnExpression != null)
                            {
<<<<<<< HEAD
                                selectExpression.AddToProjection(columnExpression);
=======
                                index = selectExpression.AddToProjection(sqlExpression);

                                aliasExpression = selectExpression.Projection[index] as AliasExpression;

                                if (aliasExpression != null)
                                {
                                    aliasExpression.SourceExpression = expression;
                                }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                                return expression;
                            }
                        }

<<<<<<< HEAD
                        var index = selectExpression.AddToProjection(sqlExpression);

                        return
                            QueryModelVisitor.BindReadValueMethod(
                                expression.Type,
                                QueryResultScope.GetResult(
                                    EntityQueryModelVisitor.QueryResultScopeParameter,
                                    _querySource,
                                    typeof(ValueBuffer)),
                                index);
=======
                        if (!(sqlExpression is ConstantExpression))
                        {
                            index = selectExpression.AddToProjection(sqlExpression);

                            aliasExpression = selectExpression.Projection[index] as AliasExpression;

                            if (aliasExpression != null)
                            {
                                aliasExpression.SourceExpression = expression;
                            }

                            var valueBufferExpression
                                = QueryResultScope.GetResult(
                                    EntityQueryModelVisitor.QueryResultScopeParameter,
                                    _querySource,
                                    typeof(ValueBuffer));

                            var readValueExpression
                                = _entityMaterializerSource
                                    .CreateReadValueCallExpression(valueBufferExpression, index);

                            var outputDataInfo
                                = (expression as SubQueryExpression)?.QueryModel
                                    .GetOutputDataInfo();

                            if (outputDataInfo is StreamedScalarValueInfo)
                            {
                                // Compensate for possible nulls
                                readValueExpression
                                    = Expression.Coalesce(
                                        readValueExpression,
                                        Expression.Default(expression.Type));
                            }

                            return Expression.Convert(readValueExpression, expression.Type);
                        }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
                    }
                }
            }

            return base.Visit(expression);
        }
    }
}
