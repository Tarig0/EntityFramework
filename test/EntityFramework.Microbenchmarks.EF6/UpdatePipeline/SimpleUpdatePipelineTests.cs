// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
=======
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Core.Models.Orders;
using EntityFramework.Microbenchmarks.EF6.Models.Orders;
using Xunit;

namespace EntityFramework.Microbenchmarks.EF6.UpdatePipeline
{
<<<<<<< HEAD
    public class SimpleUpdatePipelineTests
    {
        private static readonly string _connectionString 
            = $@"Server={TestConfig.Instance.DataSource};Database=Perf_UpdatePipeline_Simple_EF6;Integrated Security=True;MultipleActiveResultSets=true;";

        [Fact]
        public void Insert()
        {
            new TestDefinition
                {
                    TestName = "UpdatePipeline_Simple_Insert_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = Insert,
                    Setup = EnsureDatabaseSetup
                }.RunTest();
        }

        private static void Insert(TestHarness harness)
        {
            using (var context = new OrdersContext(_connectionString))
=======
    public class SimpleUpdatePipelineTests : IClassFixture<SimpleUpdatePipelineTests.SimpleUpdatePipelineFixture>
    {
        private readonly SimpleUpdatePipelineFixture _fixture;

        public SimpleUpdatePipelineTests(SimpleUpdatePipelineFixture fixture)
        {
            _fixture = fixture;
        }

        [Benchmark]
        public void Insert(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            {
                using (context.Database.BeginTransaction())
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        context.Customers.Add(new Customer { Name = "New Customer " + i });
                    }

<<<<<<< HEAD
                    harness.StartCollection();
                    var records = context.SaveChanges();
                    harness.StopCollection();
=======
                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                    Assert.Equal(1000, records);
                }
            }
        }

<<<<<<< HEAD
        [Fact]
        public void Update()
        {
            new TestDefinition
                {
                    TestName = "UpdatePipeline_Simple_Update_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = Update,
                    Setup = EnsureDatabaseSetup
                }.RunTest();
        }

        private static void Update(TestHarness harness)
        {
            using (var context = new OrdersContext(_connectionString))
=======
        [Benchmark]
        public void Update(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            {
                using (context.Database.BeginTransaction())
                {
                    foreach (var customer in context.Customers)
                    {
                        customer.Name += " Modified";
                    }

<<<<<<< HEAD
                    harness.StartCollection();
                    var records = context.SaveChanges();
                    harness.StopCollection();
=======
                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                    Assert.Equal(1000, records);
                }
            }
        }

<<<<<<< HEAD
        [Fact]
        public void Delete()
        {
            new TestDefinition
                {
                    TestName = "UpdatePipeline_Simple_Delete_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = Delete,
                    Setup = EnsureDatabaseSetup
                }.RunTest();
        }

        private static void Delete(TestHarness harness)
        {
            using (var context = new OrdersContext(_connectionString))
=======
        [Benchmark]
        public void Delete(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            {
                using (context.Database.BeginTransaction())
                {
                    foreach (var customer in context.Customers)
                    {
                        context.Customers.Remove(customer);
                    }

<<<<<<< HEAD
                    harness.StartCollection();
                    var records = context.SaveChanges();
                    harness.StopCollection();
=======
                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                    Assert.Equal(1000, records);
                }
            }
        }

<<<<<<< HEAD
        [Fact]
        public void Mixed()
        {
            new TestDefinition
                {
                    TestName = "UpdatePipeline_Simple_Mixed_EF6",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = Mixed,
                    Setup = EnsureDatabaseSetup
                }.RunTest();
        }

        private static void Mixed(TestHarness harness)
        {
            using (var context = new OrdersContext(_connectionString))
=======
        [Benchmark]
        public void Mixed(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
            {
                using (context.Database.BeginTransaction())
                {
                    var customers = context.Customers.ToArray();

                    for (var i = 0; i < 333; i++)
                    {
                        context.Customers.Add(new Customer { Name = "New Customer " + i });
                    }

                    for (var i = 0; i < 1000; i += 3)
                    {
                        context.Customers.Remove(customers[i]);
                    }

                    for (var i = 1; i < 1000; i += 3)
                    {
                        customers[i].Name += " Modified";
                    }

<<<<<<< HEAD
                    harness.StartCollection();
                    var records = context.SaveChanges();
                    harness.StopCollection();
=======
                    collector.StartCollection();
                    var records = context.SaveChanges();
                    collector.StopCollection();
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

                    Assert.Equal(1000, records);
                }
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
        public class SimpleUpdatePipelineFixture : OrdersFixture
        {
            public SimpleUpdatePipelineFixture()
                : base("Perf_UpdatePipeline_Simple_EF6", 0, 1000, 0, 0)
            { }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
