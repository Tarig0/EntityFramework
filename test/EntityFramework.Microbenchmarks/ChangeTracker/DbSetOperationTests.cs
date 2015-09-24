// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
=======
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Core.Models.Orders;
using EntityFramework.Microbenchmarks.Models.Orders;
using Xunit;

namespace EntityFramework.Microbenchmarks.ChangeTracker
{
<<<<<<< HEAD
    public class DbSetOperationTests
    {
        private static readonly string _connectionString 
            = $@"Server={TestConfig.Instance.DataSource};Database=Perf_ChangeTracker_DbSetOperation;Integrated Security=True;MultipleActiveResultSets=true;";

        [Fact]
        public void Add()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_Add",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = new Customer[1000];
                                for (var i = 0; i < customers.Length; i++)
                                {
                                    customers[i] = new Customer { Name = "Customer " + i };
                                }

                                using (harness.StartCollection())
                                {
                                    foreach (var customer in customers)
                                    {
                                        context.Customers.Add(customer);
                                    }
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void AddCollection()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_AddCollection",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = new Customer[1000];
                                for (var i = 0; i < customers.Length; i++)
                                {
                                    customers[i] = new Customer { Name = "Customer " + i };
                                }

                                using (harness.StartCollection())
                                {
                                    context.Customers.AddRange(customers);
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Attach()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_Attach",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = GetAllCustomersFromDatabase();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    foreach (var customer in customers)
                                    {
                                        context.Customers.Attach(customer);
                                    }
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void AttachCollection()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_AttachCollection",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = GetAllCustomersFromDatabase();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    context.Customers.AttachRange(customers);
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Remove()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_Remove",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = context.Customers.ToArray();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    foreach (var customer in customers)
                                    {
                                        context.Customers.Remove(customer);
                                    }
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void RemoveCollection()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_RemoveCollection",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = context.Customers.ToArray();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    context.Customers.RemoveRange(customers);
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Update()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_Update",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = GetAllCustomersFromDatabase();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    foreach (var customer in customers)
                                    {
                                        context.Customers.Update(customer);
                                    }
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void UpdateCollection()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_DbSetOperation_UpdateCollection",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = GetAllCustomersFromDatabase();
                                Assert.Equal(1000, customers.Length);

                                using (harness.StartCollection())
                                {
                                    context.Customers.UpdateRange(customers);
                                }
                            }
                        }
                }.RunTest();
        }

        private static Customer[] GetAllCustomersFromDatabase()
        {
            using (var context = new OrdersContext(_connectionString))
=======
    public class DbSetOperationTests : IClassFixture<DbSetOperationTests.DbSetOperationFixture>
    {
        private readonly DbSetOperationFixture _fixture;

        public DbSetOperationTests(DbSetOperationFixture fixture)
        {
            _fixture = fixture;
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void Add(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = new Customer[1000];
                for (var i = 0; i < customers.Length; i++)
                {
                    customers[i] = new Customer { Name = "Customer " + i };
                }

                using (collector.StartCollection())
                {
                    foreach (var customer in customers)
                    {
                        context.Customers.Add(customer);
                    }
                }
            }
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void AddCollection(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = new Customer[1000];
                for (var i = 0; i < customers.Length; i++)
                {
                    customers[i] = new Customer { Name = "Customer " + i };
                }

                using (collector.StartCollection())
                {
                    context.Customers.AddRange(customers);
                }
            }

        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void Attach(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = GetAllCustomersFromDatabase();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    foreach (var customer in customers)
                    {
                        context.Customers.Attach(customer);
                    }
                }
            }

        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void AttachCollection(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = GetAllCustomersFromDatabase();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    context.Customers.AttachRange(customers);
                }
            }
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void Remove(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = context.Customers.ToArray();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    foreach (var customer in customers)
                    {
                        context.Customers.Remove(customer);
                    }
                }
            }
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void RemoveCollection(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = context.Customers.ToArray();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    context.Customers.RemoveRange(customers);
                }
            }
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void Update(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = GetAllCustomersFromDatabase();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    foreach (var customer in customers)
                    {
                        context.Customers.Update(customer);
                    }
                }
            }
        }

        [Benchmark]
        [BenchmarkVariation("AutoDetectChanges On", true)]
        [BenchmarkVariation("AutoDetectChanges Off", false)]
        public void UpdateCollection(MetricCollector collector, bool autoDetectChanges)
        {
            using (var context = _fixture.CreateContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;

                var customers = GetAllCustomersFromDatabase();
                Assert.Equal(1000, customers.Length);

                using (collector.StartCollection())
                {
                    context.Customers.UpdateRange(customers);
                }
            }
        }

        private Customer[] GetAllCustomersFromDatabase()
        {
            using (var context = _fixture.CreateContext())
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            {
                return context.Customers.ToArray();
            }
        }

<<<<<<< HEAD
        private static void EnsureDatabaseSetup()
        {
            new OrdersSeedData().EnsureCreated(
                _connectionString,
                productCount: 0,
                customerCount: 1000,
                ordersPerCustomer: 0,
                linesPerOrder: 0);
=======
        public class DbSetOperationFixture : OrdersFixture
        {
            public DbSetOperationFixture()
                : base("Perf_ChangeTracker_DbSetOperation", 0, 1000, 0, 0)
            { }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
