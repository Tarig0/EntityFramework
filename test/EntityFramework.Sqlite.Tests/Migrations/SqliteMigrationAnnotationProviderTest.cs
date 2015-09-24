// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Conventions.Internal;
<<<<<<< HEAD
using Microsoft.Data.Entity.Sqlite.Metadata;
using Xunit;

namespace Microsoft.Data.Entity.Sqlite.Migrations
=======
using Microsoft.Data.Entity.Metadata.Internal;
using Xunit;

namespace Microsoft.Data.Entity.Migrations
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
{
    public class SqliteMigrationAnnotationProviderTest
    {
        private readonly ModelBuilder _modelBuilder;
<<<<<<< HEAD
        private readonly SqliteMigrationAnnotationProvider _provider;
=======
        private readonly SqliteMigrationsAnnotationProvider _provider;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

        private readonly Annotation _autoincrement = new Annotation(SqliteAnnotationNames.Prefix + SqliteAnnotationNames.Autoincrement, true);

        public SqliteMigrationAnnotationProviderTest()
        {
            _modelBuilder = new ModelBuilder(new CoreConventionSetBuilder().CreateConventionSet(), new Model());
<<<<<<< HEAD
            _provider = new SqliteMigrationAnnotationProvider();
        }

        [Theory]
        [InlineData(StoreGeneratedPattern.Computed, false)]
        [InlineData(StoreGeneratedPattern.Identity, true)]
        [InlineData(StoreGeneratedPattern.None, false)]
        public void Adds_Autoincrement_by_store_generated_pattern(StoreGeneratedPattern pattern, bool addsAnnotation)
        {
            _modelBuilder.Entity<Entity>(b => { b.Property(e => e.Prop).StoreGeneratedPattern(pattern); });
            var property = _modelBuilder.Model.GetEntityType(typeof(Entity)).GetProperty("Prop");

            var annotation = _provider.For(property);
            if (addsAnnotation)
            {
                Assert.Contains(annotation, a => a.Name == _autoincrement.Name && (bool)a.Value);
            }
            else
            {
                Assert.DoesNotContain(annotation, a => a.Name == _autoincrement.Name && (bool)a.Value);
            }
=======
            _provider = new SqliteMigrationsAnnotationProvider();
        }

        [Fact]
        public void Adds_Autoincrement_for_OnAdd_integer_property()
        {
            var property = _modelBuilder.Entity<Entity>().Property(e => e.IntProp).ValueGeneratedOnAdd().Metadata;

            Assert.Contains(_provider.For(property), a => a.Name == _autoincrement.Name && (bool)a.Value);
        }

        [Fact]
        public void Does_not_add_Autoincrement_for_OnAddOrUpdate_integer_property()
        {
            var property = _modelBuilder.Entity<Entity>().Property(e => e.IntProp).ValueGeneratedOnAddOrUpdate().Metadata;

            Assert.DoesNotContain(_provider.For(property), a => a.Name == _autoincrement.Name);
        }

        [Fact]
        public void Does_not_add_Autoincrement_for_Never_value_generated_integer_property()
        {
            var property = _modelBuilder.Entity<Entity>().Property(e => e.IntProp).ValueGeneratedNever().Metadata;

            Assert.DoesNotContain(_provider.For(property), a => a.Name == _autoincrement.Name);
        }

        [Fact]
        public void Does_not_add_Autoincrement_for_default_integer_property()
        {
            var property = _modelBuilder.Entity<Entity>().Property(e => e.IntProp).Metadata;

            Assert.DoesNotContain(_provider.For(property), a => a.Name == _autoincrement.Name);
        }

        [Fact]
        public void Does_not_add_Autoincrement_for_non_integer_OnAdd_property()
        {
            var property = _modelBuilder.Entity<Entity>().Property(e => e.StringProp).ValueGeneratedOnAdd().Metadata;

            Assert.DoesNotContain(_provider.For(property), a => a.Name == _autoincrement.Name);
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }

        private class Entity
        {
            public int Id { get; set; }
<<<<<<< HEAD
            public long Prop { get; set; }
=======
            public long IntProp { get; set; }
            public string StringProp { get; set; }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
