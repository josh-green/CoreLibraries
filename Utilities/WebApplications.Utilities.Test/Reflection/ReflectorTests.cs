﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Testing;
using WebApplications.Utilities.Relection;

namespace WebApplications.Utilities.Test.Reflection
{
    [TestClass]
    public class ReflectorTests : TestBase
    {
        /// <summary>
        /// A class with complicated overloads illustrating classic failure cases.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        private class ComplexOverloads<T>
        {
            public void A() {}
            public void A(string a) { }
            public void A(ref string a) { }
            public void A(string a, string b = null) { }
            public void A(string a, out string b)
            {
                b = null;
            }

            public void A<TException>(string a) { }
            public void A<T1, T2>(string a) { }

            public static explicit operator int(ComplexOverloads<T> a)
            {
                return 1;
            }
            public static explicit operator Int16(ComplexOverloads<T> a)
            {
                return 1;
            }
        }

        [TestMethod]
        public void Reflector_CanDisambiguateMembers()
        {
            ExtendedType et = ExtendedType.Get(typeof (ComplexOverloads<>));

            Methods methods = et.GetMethods("A");

            Assert.IsNotNull(methods);
            Assert.AreEqual(7, methods.Overloads.Count());

            Method method1 = methods.GetOverload(typeof(void));
            Assert.IsTrue(method1.Info.ContainsGenericParameters);
            Assert.IsNotNull(method1);

            Method method2 = methods.GetOverload(typeof(string), typeof(void));
            Assert.IsNotNull(method2);
            Assert.AreNotEqual(method1, method2);

            Method method3 = methods.GetOverload(typeof(string).MakeByRefType(), typeof(void));
            Assert.IsNotNull(method3);
            Assert.AreNotEqual(method3, method1);
            Assert.AreNotEqual(method3, method2);

            Method method4 = methods.GetOverload(typeof(string), typeof(string), typeof(void));
            Assert.IsNotNull(method4);
            Assert.AreNotEqual(method4, method1);
            Assert.AreNotEqual(method4, method2);
            Assert.AreNotEqual(method4, method3);

            Method method5 = methods.GetOverload(typeof(string), typeof(string).MakeByRefType(), typeof(void));
            Assert.IsNotNull(method5);
            Assert.AreNotEqual(method5, method1);
            Assert.AreNotEqual(method5, method2);
            Assert.AreNotEqual(method5, method3);
            Assert.AreNotEqual(method5, method4);

            Method method6 = methods.GetOverload(1, typeof(string), typeof(void));
            Assert.IsNotNull(method6);
            Assert.AreNotEqual(method6, method1);
            Assert.AreNotEqual(method6, method2);
            Assert.AreNotEqual(method6, method3);
            Assert.AreNotEqual(method6, method4);
            Assert.AreNotEqual(method6, method5);

            Method method7 = methods.GetOverload(2, typeof(string), typeof(void));
            Assert.IsNotNull(method7);
            Assert.AreNotEqual(method7, method1);
            Assert.AreNotEqual(method7, method2);
            Assert.AreNotEqual(method7, method3);
            Assert.AreNotEqual(method7, method4);
            Assert.AreNotEqual(method7, method5);
            Assert.AreNotEqual(method7, method6);
        }

        [TestMethod]
        public void Reflector_CanGetConcreteMethods()
        {
            // This time we get a concrete type.
            Method method = ExtendedType<ComplexOverloads<int>>.GetMethod("A", typeof (void));
            Assert.IsNotNull(method);
            // This method has no generic arguments and the type is concrete.
            Assert.IsFalse(method.Info.ContainsGenericParameters);

            Method method2 = ExtendedType<ComplexOverloads<int>>.GetMethod("A", 1, typeof(string), typeof(void));
            Assert.IsNotNull(method2);
            // This method has generic arguments and so it does contain generic parameters even though type is concrete.
            Assert.IsTrue(method2.Info.ContainsGenericParameters);
        }
    }
}
