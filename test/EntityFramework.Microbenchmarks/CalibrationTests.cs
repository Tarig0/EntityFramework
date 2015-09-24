// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

<<<<<<< HEAD
using System.Threading;
using EntityFramework.Microbenchmarks.Core;
using Xunit;
=======
using System;
using System.Threading;
using EntityFramework.Microbenchmarks.Core;
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367

namespace EntityFramework.Microbenchmarks
{
    public class CalibrationTests
    {
<<<<<<< HEAD
        [Fact]
        public void Calibration_100ms()
        {
            new TestDefinition
                {
                    TestName = "Calibration_100ms",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = harness =>
                        {
                            using (harness.StartCollection())
                            {
                                Thread.Sleep(100);
                            }
                        }
                }.RunTest();
        }

        [Fact]
        public void Calibration_100ms_controlled()
        {
            new TestDefinition
                {
                    TestName = "Calibration_100ms_controlled",
                    IterationCount = 100,
                    WarmupCount = 5,
                    Run = harness =>
                        {
                            Thread.Sleep(100);
                            using (harness.StartCollection())
                            {
                                Thread.Sleep(100);
                            }
                        }
                }.RunTest();
        }
=======
        [Benchmark]
        public void Calibration_100ms(MetricCollector collector)
        {
            using (collector.StartCollection())
            {
                Thread.Sleep(100);
            }
        }

        [Benchmark]
        public void Calibration_100ms_controlled(MetricCollector collector)
        {

            Thread.Sleep(100);
            using (collector.StartCollection())
            {
                Thread.Sleep(100);
            }
        }

#if !DNXCORE50 && !DNX451
        [Benchmark]
        public void ColdStartSandbox_100ms(MetricCollector collector)
        {
            using (var sandbox = new ColdStartSandbox())
            {
                var testClass = sandbox.CreateInstance<ColdStartEnabledTests>();
                testClass.Sleep100ms(collector);
            }
        }

        private partial class ColdStartEnabledTests : MarshalByRefObject
        {
            public void Sleep100ms(MetricCollector collector)
            {
                using (collector.StartCollection())
                {
                    Thread.Sleep(100);
                }
            }
        }
#endif
>>>>>>> d2802fdaf35458e35a69f9573e57c592d43c6367
    }
}
