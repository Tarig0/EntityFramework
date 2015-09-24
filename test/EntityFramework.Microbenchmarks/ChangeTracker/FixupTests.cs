// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System;
=======
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Microbenchmarks.Core;
using EntityFramework.Microbenchmarks.Core.Models.Orders;
using EntityFramework.Microbenchmarks.Models.Orders;
using Xunit;

namespace EntityFramework.Microbenchmarks.ChangeTracker
{
<<<<<<< HEAD
    public class FixupTests
    {
        private static readonly string _connectionString 
            = $@"Server={TestConfig.Instance.DataSource};Database=Perf_ChangeTracker_Fixup;Integrated Security=True;MultipleActiveResultSets=true;";

        [Fact]
        public void AddChildren()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_AddChildren",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = context.Customers.ToList();
                                Assert.Equal(1000, customers.Count);

                                foreach (var customer in customers)
                                {
                                    var order = new Order { CustomerId = customer.CustomerId };

                                    using (harness.StartCollection())
                                    {
                                        context.Orders.Add(order);
                                    }

                                    Assert.Same(order, order.Customer.Orders.Single());
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void AddParents()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_AddParents",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = new List<Customer>();
                                for (var i = 0; i < 1000; i++)
                                {
                                    customers.Add(new Customer { CustomerId = i + 1 });
                                    context.Orders.Add(new Order { CustomerId = i + 1 });
                                }

                                foreach (var customer in customers)
                                {
                                    using (harness.StartCollection())
                                    {
                                        context.Customers.Add(customer);
                                    }

                                    Assert.Same(customer, customer.Orders.Single().Customer);
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void AttachChildren()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_AttachChildren",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            List<Order> orders;
                            using (var context = new OrdersContext(_connectionString))
                            {
                                orders = context.Orders.ToList();
                            }

                            using (var context = new OrdersContext(_connectionString))
                            {
                                var customers = context.Customers.ToList();
                                Assert.Equal(1000, orders.Count);
                                Assert.Equal(1000, customers.Count);

                                foreach (var order in orders)
                                {
                                    using (harness.StartCollection())
                                    {
                                        context.Orders.Attach(order);
                                    }

                                    Assert.Same(order, order.Customer.Orders.Single());
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void AttachParents()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_AttachParents",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            List<Customer> customers;
                            using (var context = new OrdersContext(_connectionString))
                            {
                                customers = context.Customers.ToList();
                            }

                            using (var context = new OrdersContext(_connectionString))
                            {
                                var orders = context.Orders.ToList();
                                Assert.Equal(1000, orders.Count);
                                Assert.Equal(1000, customers.Count);

                                foreach (var customer in customers)
                                {
                                    using (harness.StartCollection())
                                    {
                                        context.Customers.Attach(customer);
                                    }

                                    Assert.Same(customer, customer.Orders.Single().Customer);
                                }
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void QueryChildren()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_QueryChildren",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                context.Customers.ToList();

                                harness.StartCollection();
                                var orders = context.Orders.ToList();
                                harness.StopCollection();

                                Assert.Equal(1000, context.ChangeTracker.Entries<Customer>().Count());
                                Assert.Equal(1000, context.ChangeTracker.Entries<Order>().Count());
                                Assert.All(orders, o => Assert.NotNull(o.Customer));
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void QueryParents()
        {
            new TestDefinition
                {
                    TestName = "ChangeTracker_Fixup_QueryParents",
                    IterationCount = 10,
                    WarmupCount = 5,
                    Setup = EnsureDatabaseSetup,
                    Run = harness =>
                        {
                            using (var context = new OrdersContext(_connectionString))
                            {
                                context.Orders.ToList();

                                harness.StartCollection();
                                var customers = context.Customers.ToList();
                                harness.StopCollection();

                                Assert.Equal(1000, context.ChangeTracker.Entries<Customer>().Count());
                                Assert.Equal(1000, context.ChangeTracker.Entries<Order>().Count());
                                Assert.All(customers, c => Assert.Equal(1, c.Orders.Count));
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
                ordersPerCustomer: 1,
                linesPerOrder: 1);
=======
    public class FixupTests : IClassFixture<FixupTests.FixupFixture>
    {
        private readonly FixupFixture _fixture;

        public FixupTests(FixupFixture fixture)
        {
            _fixture = fixture;
        }

        [Benchmark(Iterations = 10)]
        public void AddChildren(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
            {
                var customers = context.Customers.ToList();
                Assert.Equal(100, customers.Count);

                foreach (var customer in customers)
                {
                    var order = new Order { CustomerId = customer.CustomerId };

                    using (collector.StartCollection())
                    {
                        context.Orders.Add(order);
                    }

                    Assert.Same(order, order.Customer.Orders.Single());
                }
            }
        }

        [Benchmark(Iterations = 10)]
        public void AddParents(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
            {
                var customers = new List<Customer>();
                for (var i = 0; i < 100; i++)
                {
                    customers.Add(new Customer { CustomerId = i + 1 });
                    context.Orders.Add(new Order { CustomerId = i + 1 });
                }

                foreach (var customer in customers)
                {
                    using (collector.StartCollection())
                    {
                        context.Customers.Add(customer);
                    }

                    Assert.Same(customer, customer.Orders.Single().Customer);
                }
            }
        }

        [Benchmark(Iterations = 10)]
        public void AttachChildren(MetricCollector collector)
        {
            List<Order> orders;
            using (var context = _fixture.CreateContext())
            {
                orders = context.Orders.ToList();
            }

            using (var context = _fixture.CreateContext())
            {
                var customers = context.Customers.ToList();
                Assert.Equal(100, orders.Count);
                Assert.Equal(100, customers.Count);

                foreach (var order in orders)
                {
                    using (collector.StartCollection())
                    {
                        context.Orders.Attach(order);
                    }

                    Assert.Same(order, order.Customer.Orders.Single());
                }
            }
        }

        [Benchmark(Iterations = 10)]
        public void AttachParents(MetricCollector collector)
        {
            List<Customer> customers;
            using (var context = _fixture.CreateContext())
            {
                customers = context.Customers.ToList();
            }

            using (var context = _fixture.CreateContext())
            {
                var orders = context.Orders.ToList();
                Assert.Equal(100, orders.Count);
                Assert.Equal(100, customers.Count);

                foreach (var customer in customers)
                {
                    using (collector.StartCollection())
                    {
                        context.Customers.Attach(customer);
                    }

                    Assert.Same(customer, customer.Orders.Single().Customer);
                }
            }
        }

        [Benchmark(Iterations = 10)]
        public void QueryChildren(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
            {
                context.Customers.ToList();

                collector.StartCollection();
                var orders = context.Orders.ToList();
                collector.StopCollection();

                Assert.Equal(100, context.ChangeTracker.Entries<Customer>().Count());
                Assert.Equal(100, context.ChangeTracker.Entries<Order>().Count());
                Assert.All(orders, o => Assert.NotNull(o.Customer));
            }
        }

        [Benchmark(Iterations = 10)]
        public void QueryParents(MetricCollector collector)
        {
            using (var context = _fixture.CreateContext())
            {
                context.Orders.ToList();

                collector.StartCollection();
                var customers = context.Customers.ToList();
                collector.StopCollection();

                Assert.Equal(100, context.ChangeTracker.Entries<Customer>().Count());
                Assert.Equal(100, context.ChangeTracker.Entries<Order>().Count());
                Assert.All(customers, c => Assert.Equal(1, c.Orders.Count));
            }
        }

        public class FixupFixture : OrdersFixture
        {
            public FixupFixture()
                : base("Perf_ChangeTracker_Fixup", 100, 100, 1, 1)
            { }
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
        }
    }
}
