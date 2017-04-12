using DynamicAccess.Test.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicAccess.Test
{
    /// <summary>
    /// This is a test class for ReflectionMemberAccessorTest and is intended
    /// to contain all ReflectionMemberAccessorTest Unit Tests
    /// </summary>
    [TestClass]
    public class ReflectionMemberAccessorTest
    {
        private object testObject;
        private string testMemberName;

        [TestInitialize]
        public void OnTestStartup()
        {
            testObject = new Man()
            {
                Weight = 50.0
            };
            testMemberName = "Weight";
        }

        [TestMethod]
        public void GetValueByRelfectionTest()
        {
            var target = new ReflectionMemberAccessor();
            var actual = target.GetValue(testObject, testMemberName);
            Assert.AreEqual(50.0, actual);
        }

        [TestMethod]
        public void GetValueByDelegatedreflectionTest()
        {
            var acessor = new DelegatedExpressionMemberAccessor();
            var actual = acessor.GetValue(testObject, testMemberName);
            Assert.AreEqual(50.0, actual);
        }

        [TestMethod]
        public void GetValueByDynamicMethodTest()
        {
            var accessor = new DynamicMethodMemberAccessor();
            accessor.SetValue(testObject, testMemberName, 60.0);
            var actual = (double)accessor.GetValue(testObject, testMemberName);
            Assert.AreEqual(60.0, actual);
            accessor.SetValue(testObject, testMemberName, 50.0);
        }

        [TestMethod]
        public void GetValueByObjectInterfaceTest()
        {
            var actual = (testObject as IMemberAccessor).GetValue(testObject, testMemberName);
            Assert.AreEqual(50.0, actual);
        }

        [TestMethod]
        public void PerformanceTest()
        {
            var runTime = 10000000;
            var ra = new ReflectionMemberAccessor();
            var ea = new DelegatedExpressionMemberAccessor();
            var da = new DelegatedReflectionMemberAccessor();
            var ma = new DynamicMethodMemberAccessor();
            var ga = new DynamicMethod<Man>();

            //预热
            ea.SetValue(testObject, testMemberName, ea.GetValue(testObject, testMemberName));
            ma.SetValue(testObject, testMemberName, ma.GetValue(testObject, testMemberName));
            ra.SetValue(testObject, testMemberName, ra.GetValue(testObject, testMemberName));
            ga.SetValue(testObject, testMemberName, ga.GetValue(testObject, testMemberName));
            da.SetValue(testObject, testMemberName, da.GetValue(testObject, testMemberName));

            new TimeProfiler(() => (testObject as Man).Weight = (testObject as Man).Weight, "直接调用").Run(runTime);
            new TimeProfiler(() => ra.SetValue(testObject, testMemberName, ra.GetValue(testObject, testMemberName)), "反射调用").Run(runTime);
            new TimeProfiler(() => ea.SetValue(testObject, testMemberName, ea.GetValue(testObject, testMemberName)), "Expression委托调用").Run(runTime);
            new TimeProfiler(() => da.SetValue(testObject, testMemberName, da.GetValue(testObject, testMemberName)), "CreateDelegate委托调用").Run(runTime);
            new TimeProfiler(() => (testObject as IMemberAccessor).SetValue(testObject, testMemberName, (testObject as IMemberAccessor).GetValue(testObject, testMemberName)), "接口Switch调用").Run(runTime);
            new TimeProfiler(() => ma.SetValue(testObject, testMemberName, ma.GetValue(testObject, testMemberName)), "动态生成函数调用").Run(runTime);
            new TimeProfiler(() => ga.SetValue(testObject, testMemberName, ga.GetValue(testObject, testMemberName)), "泛型动态生成函数调用").Run(runTime);
            
        }
    }
}
