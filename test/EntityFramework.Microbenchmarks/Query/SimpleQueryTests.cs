// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Models.Orders;
using Microsoft.Data.Entity;
=======
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Models.Orders;
using Microsoft.Data.Entity;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
using Xunit;

namespace EntityFramework.Microbenchmarks.Query
{
<<<<<<< HEAD
    public class SimpleQueryTests
    {
        private static readonly string _connectionString 
            = $@"Server={TestConfig.Instance.DataSource};Database=Perf_Query_Simple;Integrated Security=True;MultipleActiveResultSets=true;";

        [Fact]
        public void LoadAll()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_LoadAll",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.ToList();
                                harness.StopCollection();
                                Assert.Equal(1000, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Where()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_Where",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.Where(p => p.Retail < 15).ToList();
                                harness.StopCollection();
                                Assert.Equal(500, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void OrderBy()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_OrderBy",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.OrderBy(p => p.Retail).ToList();
                                harness.StopCollection();
                                Assert.Equal(1000, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Count()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_Count",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.Count();
                                harness.StopCollection();
                                Assert.Equal(1000, result);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void SkipTake()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_SkipTake",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products
                                    .OrderBy(p => p.ProductId)
                                    .Skip(500).Take(500)
                                    .ToList();

                                harness.StopCollection();
                                Assert.Equal(500, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void GroupBy()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_GroupBy",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products
                                    .GroupBy(p => p.Retail)
                                    .Select(g => new
                                        {
                                            Retail = g.Key,
                                            Products = g
                                        })
                                    .ToList();

                                harness.StopCollection();
                                Assert.Equal(10, result.Count);
                                Assert.All(result, g => Assert.Equal(100, g.Products.Count()));
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Include()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_Include",
                    // TODO Increase iteration count once perf issues addressed
                    IterationCount = 2,
                    WarmupCount = 1,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Customers.Include(c => c.Orders).ToList();
                                harness.StopCollection();
                                Assert.Equal(1000, result.Count);
                                Assert.Equal(2000, result.SelectMany(c => c.Orders).Count());
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Projection()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_Projection",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.Select(p => new { p.Name, p.Retail }).ToList();
                                harness.StopCollection();
                                Assert.Equal(1000, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void ProjectionAcrossNavigation()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_ProjectionAcrossNavigation",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                // TODO Use navigation for projection when supported (#325)
                                var result = context.Orders.Join(
                                    context.Customers,
                                    o => o.CustomerId,
                                    c => c.CustomerId,
                                    (o, c) => new { CustomerName = c.Name, OrderDate = o.Date })
                                    .ToList();

                                harness.StopCollection();
                                Assert.Equal(2000, result.Count);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void NoTracking()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_NoTracking",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Products.AsNoTracking().ToList();
                                harness.StopCollection();
                                Assert.Equal(1000, result.Count);
                            }
                        }
                }.RunTest();
        }

        private static void EnsureDatabaseSetup()
        {
            new OrdersSeedData().EnsureCreated(
                _connectionString,
                productCount: 1000,
                customerCount: 1000,
                ordersPerCustomer: 2,
                linesPerOrder: 2);
=======
    public class SimpleQueryTests : IClassFixture<SimpleQueryTests.SimpleQueryFixture>
    {
        private readonly SimpleQueryFixture _fixture;

        public SimpleQueryTests(SimpleQueryFixture fixture)
        {
            _fixture = fixture;
        }

        [Benchmark]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void LoadAll(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products.ApplyTracking(tracking);

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(1000, result.Count);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void Where(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products
                    .ApplyTracking(tracking)
                    .Where(p => p.Retail < 15);

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(500, result.Count);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void OrderBy(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products
                    .ApplyTracking(tracking)
                    .OrderBy(p => p.Retail);

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(1000, result.Count);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Default", true)]
        [BenchmarkVariation("No Query Cache", false)]
        public void Count(MetricCollector collector, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products;

                collector.StartCollection();
                var result = query.Count();
                collector.StopCollection();
                Assert.Equal(1000, result);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void SkipTake(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products
                    .ApplyTracking(tracking)
                    .OrderBy(p => p.ProductId)
                    .Skip(500).Take(500);

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(500, result.Count);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Default", true)]
        [BenchmarkVariation("No Query Cache", false)]
        public void GroupBy(MetricCollector collector, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products
                    .GroupBy(p => p.Retail)
                    .Select(g => new
                    {
                        Retail = g.Key,
                        Products = g
                    });

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(10, result.Count);
                Assert.All(result, g => Assert.Equal(100, g.Products.Count()));
            }
        }

        [Benchmark(Iterations = 2)]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void Include(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Customers
                    .ApplyTracking(tracking)
                    .Include(c => c.Orders);

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(1000, result.Count);
                Assert.Equal(2000, result.SelectMany(c => c.Orders).Count());
            }
        }

        [Benchmark]
        [BenchmarkVariation("Default", true)]
        [BenchmarkVariation("No Query Cache", false)]
        public void Projection(MetricCollector collector, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                var query = context.Products
                    .Select(p => new { p.Name, p.Retail });

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(1000, result.Count);
            }
        }

        [Benchmark]
        [BenchmarkVariation("Default", true)]
        [BenchmarkVariation("No Query Cache", false)]
        public void ProjectionAcrossNavigation(MetricCollector collector, bool caching)
        {
            using (var context = _fixture.CreateContext(queryCachingEnabled: caching))
            {
                // TODO Use navigation for projection when supported (#325)
                var query = context.Orders.Join(
                    context.Customers,
                    o => o.CustomerId,
                    c => c.CustomerId,
                    (o, c) => new { CustomerName = c.Name, OrderDate = o.Date });

                collector.StartCollection();
                var result = query.ToList();
                collector.StopCollection();
                Assert.Equal(2000, result.Count);
            }
        }

        public class SimpleQueryFixture : OrdersFixture
        {
            private readonly IServiceProvider _noQueryCacheServiceProvider;

            public SimpleQueryFixture()
                : base("Perf_Query_Simple", 1000, 1000, 2, 2)
            {
                var collection = new ServiceCollection();
                collection.AddEntityFramework().AddSqlServer();
                collection.AddSingleton<IMemoryCache, NonCachingMemoryCache>();
                _noQueryCacheServiceProvider = collection.BuildServiceProvider();
            }

            public OrdersContext CreateContext(bool disableBatching = false, bool queryCachingEnabled = true)
            {
                if (!queryCachingEnabled)
                {
                    return new OrdersContext(_noQueryCacheServiceProvider, ConnectionString, disableBatching);
                }

                return base.CreateContext(disableBatching);
            }

            private class NonCachingMemoryCache : IMemoryCache
            {
                public bool TryGetValue(object key, out object value)
                {
                    value = null;
                    return false;
                }

                public object Set(object key, object value, MemoryCacheEntryOptions options)
                {
                    return value;
                }

                public void Remove(object key)
                { }

                public IEntryLink CreateLinkingScope()
                {
                    throw new NotImplementedException();
                }

                public void Dispose()
                { }
            }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
