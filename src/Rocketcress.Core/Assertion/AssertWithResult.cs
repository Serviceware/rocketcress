using System;

#if SLIM
using MaSch.Test.Assertion;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.Core.Assertion
{
    internal struct AssertWithResult<T> : IAssertWithResult<T>
    {
        private readonly bool _throw;
        private readonly T _resultOnSuccess;
        private readonly T _resultOnFailure;

        public AssertWithResult(bool @throw, T resultOnSuccess, T resultOnFailure)
        {
            _throw = @throw;
            _resultOnSuccess = resultOnSuccess;
            _resultOnFailure = resultOnFailure;
        }

        public T That(Action<Assert> assertAction)
        {
            try
            {
                var assert = _throw ? Assert.Instance : Assert.NonThrowInstance;
                assertAction(assert);
                return _resultOnSuccess;
            }
            catch (AssertFailedException)
            {
                if (_throw)
                    throw;
                return _resultOnFailure;
            }
        }
    }
}
