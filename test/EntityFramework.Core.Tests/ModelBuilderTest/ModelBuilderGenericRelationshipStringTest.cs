// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Entity.Metadata.Builders;

// ReSharper disable once CheckNamespace
namespace Microsoft.Data.Entity.Tests
{
    public class ModelBuilderGenericRelationshipStringTest : ModelBuilderGenericTest
    {
        public class GenericOneToManyString : OneToManyTestBase
        {
            protected override TestModelBuilder CreateTestModelBuilder(ModelBuilder modelBuilder)
            {
                return new GenericStringTestModelBuilder(modelBuilder);
            }
        }

        public class GenericManyToOneString : ManyToOneTestBase
        {
            protected override TestModelBuilder CreateTestModelBuilder(ModelBuilder modelBuilder)
            {
                return new GenericStringTestModelBuilder(modelBuilder);
            }
        }

        public class GenericOneToOneString : OneToOneTestBase
        {
            protected override TestModelBuilder CreateTestModelBuilder(ModelBuilder modelBuilder)
            {
                return new GenericStringTestModelBuilder(modelBuilder);
            }
        }

        private class GenericStringTestModelBuilder : TestModelBuilder
        {
            public GenericStringTestModelBuilder(ModelBuilder modelBuilder)
                : base(modelBuilder)
            {
            }

            public override TestEntityTypeBuilder<TEntity> Entity<TEntity>()
                => new GenericStringTestEntityTypeBuilder<TEntity>(ModelBuilder.Entity<TEntity>());

            public override TestModelBuilder Entity<TEntity>(Action<TestEntityTypeBuilder<TEntity>> buildAction)
                => new GenericStringTestModelBuilder(ModelBuilder.Entity<TEntity>(entityTypeBuilder =>
                    buildAction(new GenericStringTestEntityTypeBuilder<TEntity>(entityTypeBuilder))));

            public override TestModelBuilder Ignore<TEntity>()
                => new GenericStringTestModelBuilder(ModelBuilder.Ignore<TEntity>());
        }

        private class GenericStringTestEntityTypeBuilder<TEntity> : GenericTestEntityTypeBuilder<TEntity>
            where TEntity : class
        {
            public GenericStringTestEntityTypeBuilder(EntityTypeBuilder<TEntity> entityTypeBuilder)
                : base(entityTypeBuilder)
            {
            }

            protected override TestEntityTypeBuilder<TEntity> Wrap(EntityTypeBuilder<TEntity> entityTypeBuilder)
                => new GenericStringTestEntityTypeBuilder<TEntity>(entityTypeBuilder);

            public override TestReferenceNavigationBuilder<TEntity, TRelatedEntity> Reference<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> reference = null)
                => new GenericStringTestReferenceNavigationBuilder<TEntity, TRelatedEntity>(EntityTypeBuilder.Reference(reference));

            public override TestCollectionNavigationBuilder<TEntity, TRelatedEntity> Collection<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> collection = null)
                => new GenericStringTestCollectionNavigationBuilder<TEntity, TRelatedEntity>(EntityTypeBuilder.Collection(collection));
        }

        private class GenericStringTestReferenceNavigationBuilder<TEntity, TRelatedEntity> : GenericTestReferenceNavigationBuilder<TEntity, TRelatedEntity>
            where TEntity : class
            where TRelatedEntity : class
        {
            public GenericStringTestReferenceNavigationBuilder(ReferenceNavigationBuilder<TEntity, TRelatedEntity> referenceNavigationBuilder)
                : base(referenceNavigationBuilder)
            {
            }

            public override TestReferenceCollectionBuilder<TRelatedEntity, TEntity> InverseCollection(Expression<Func<TRelatedEntity, IEnumerable<TEntity>>> collection = null)
                => new GenericStringTestReferenceCollectionBuilder<TRelatedEntity, TEntity>(ReferenceNavigationBuilder.InverseCollection(collection?.GetPropertyAccess().Name));

            public override TestReferenceReferenceBuilder<TEntity, TRelatedEntity> InverseReference(Expression<Func<TRelatedEntity, TEntity>> reference = null)
                => new GenericStringTestReferenceReferenceBuilder<TEntity, TRelatedEntity>(ReferenceNavigationBuilder.InverseReference(reference?.GetPropertyAccess().Name));
        }

        private class GenericStringTestCollectionNavigationBuilder<TEntity, TRelatedEntity> : GenericTestCollectionNavigationBuilder<TEntity, TRelatedEntity>
            where TEntity : class
            where TRelatedEntity : class
        {
            public GenericStringTestCollectionNavigationBuilder(CollectionNavigationBuilder<TEntity, TRelatedEntity> collectionNavigationBuilder)
                : base(collectionNavigationBuilder)
            {
            }

            public override TestReferenceCollectionBuilder<TEntity, TRelatedEntity> InverseReference(Expression<Func<TRelatedEntity, TEntity>> reference = null)
                => new GenericStringTestReferenceCollectionBuilder<TEntity, TRelatedEntity>(CollectionNavigationBuilder.InverseReference(reference?.GetPropertyAccess().Name));
        }

        private class GenericStringTestReferenceCollectionBuilder<TEntity, TRelatedEntity> : GenericTestReferenceCollectionBuilder<TEntity, TRelatedEntity>
            where TEntity : class
            where TRelatedEntity : class
        {
            public GenericStringTestReferenceCollectionBuilder(ReferenceCollectionBuilder<TEntity, TRelatedEntity> referenceCollectionBuilder)
                : base(referenceCollectionBuilder)
            {
            }

            protected override GenericTestReferenceCollectionBuilder<TEntity, TRelatedEntity> Wrap(ReferenceCollectionBuilder<TEntity, TRelatedEntity> referenceCollectionBuilder)
                => new GenericStringTestReferenceCollectionBuilder<TEntity, TRelatedEntity>(referenceCollectionBuilder);

            public override TestReferenceCollectionBuilder<TEntity, TRelatedEntity> ForeignKey(Expression<Func<TRelatedEntity, object>> foreignKeyExpression)
                => Wrap(ReferenceCollectionBuilder.ForeignKey(foreignKeyExpression.GetPropertyAccessList().Select(p => p.Name).ToArray()));

            public override TestReferenceCollectionBuilder<TEntity, TRelatedEntity> PrincipalKey(Expression<Func<TEntity, object>> keyExpression)
                => Wrap(ReferenceCollectionBuilder.PrincipalKey(keyExpression.GetPropertyAccessList().Select(p => p.Name).ToArray()));
        }

        private class GenericStringTestReferenceReferenceBuilder<TEntity, TRelatedEntity> : GenericTestReferenceReferenceBuilder<TEntity, TRelatedEntity>
            where TEntity : class
            where TRelatedEntity : class
        {
            public GenericStringTestReferenceReferenceBuilder(ReferenceReferenceBuilder<TEntity, TRelatedEntity> referenceReferenceBuilder)
                : base(referenceReferenceBuilder)
            {
            }

            protected override GenericTestReferenceReferenceBuilder<TEntity, TRelatedEntity> Wrap(ReferenceReferenceBuilder<TEntity, TRelatedEntity> referenceReferenceBuilder)
                => new GenericStringTestReferenceReferenceBuilder<TEntity, TRelatedEntity>(referenceReferenceBuilder);

            public override TestReferenceReferenceBuilder<TEntity, TRelatedEntity> ForeignKey<TDependentEntity>(Expression<Func<TDependentEntity, object>> foreignKeyExpression)
                => Wrap(ReferenceReferenceBuilder.ForeignKey(typeof(TDependentEntity).FullName, foreignKeyExpression.GetPropertyAccessList().Select(p => p.Name).ToArray()));

            public override TestReferenceReferenceBuilder<TEntity, TRelatedEntity> PrincipalKey<TPrincipalEntity>(Expression<Func<TPrincipalEntity, object>> keyExpression)
                => Wrap(ReferenceReferenceBuilder.PrincipalKey(typeof(TPrincipalEntity).FullName, keyExpression.GetPropertyAccessList().Select(p => p.Name).ToArray()));
        }
    }
}
