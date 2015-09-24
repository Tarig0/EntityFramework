// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
using System.Data.Entity;
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.EF6.Models.Orders;
using Xunit;

namespace EntityFramework.Microbenchmarks.Query
{
    public class SimpleQueryTests
    {
        private static readonly string _connectionString 
            = $@"Server={TestConfig.Instance.DataSource};Database=Perf_Query_Simple_EF6;Integrated Security=True;MultipleActiveResultSets=true;";

        [Fact]
        public void LoadAll()
        {
            new TestDefinition
                {
                    TestName = "Query_Simple_LoadAll_EF6",
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
                    TestName = "Query_Simple_Where_EF6",
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
                    TestName = "Query_Simple_OrderBy_EF6",
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
                    TestName = "Query_Simple_Count_EF6",
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
                    TestName = "Query_Simple_SkipTake_EF6",
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
                    TestName = "Query_Simple_GroupBy_EF6",
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
                    TestName = "Query_Simple_Include_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
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
                    TestName = "Query_Simple_Projection_EF6",
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
                    TestName = "Query_Simple_ProjectionAcrossNavigation_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                harness.StartCollection();
                                var result = context.Orders.Select(o => new
                                    {
                                        CustomerName = o.Customer.Name,
                                        OrderDate = o.Date
                                    })
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
                    TestName = "Query_Simple_NoTracking_EF6",
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
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.EF6.Models.Orders;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace EntityFramework.Microbenchmarks.EF6.Query
{
    public class SimpleQueryTests : IClassFixture<SimpleQueryTests.SimpleQueryFixture>
    {
        private readonly SimpleQueryFixture _fixture;

        public SimpleQueryTests(SimpleQueryFixture fixture)
        {
            _fixture = fixture;
        }

        [Benchmark(Iterations = 1, WarmupIterations = 0)]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void LoadAll(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
                    .ApplyTracking(tracking);
                
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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching);

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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
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

        [Benchmark]
        [BenchmarkVariation("Tracking On", true, true)]
        [BenchmarkVariation("Tracking Off", false, true)]
        [BenchmarkVariation("Tracking On (No Query Cache)", true, false)]
        [BenchmarkVariation("Tracking Off (No Query Cache)", false, false)]
        public void Include(MetricCollector collector, bool tracking, bool caching)
        {
            using (var context = _fixture.CreateContext())
            {
                var query = context.Customers
                    .ApplyCaching(caching)
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
            using (var context = _fixture.CreateContext())
            {
                var query = context.Products
                    .ApplyCaching(caching)
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
            using (var context = _fixture.CreateContext())
            {
                // TODO Use navigation for projection when supported (#325)
                var query = context.Orders
                    .ApplyCaching(caching)
                    .Join(
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
            public SimpleQueryFixture()
                : base("Perf_Query_Simple_EF6", 1000, 1000, 2, 2)
            { }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
