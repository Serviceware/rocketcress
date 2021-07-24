using System;

#if SLIM
using MaSch.Test.Assertion;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Rocketcress.Core.Assertion
{
    internal struct AssertOperation : IAssertOperation
    {
        private bool _throw;

        public AssertOperation(bool @throw)
        {
            _throw = @throw;
        }

        public bool That(Action<Assert> assertAction)
        {
            try
            {
                var assert = _throw ? Assert.Instance : Assert.NonThrowInstance;
                assertAction(assert);
                return true;
            }
            catch (AssertFailedException)
            {
                if (_throw)
                    throw;
                return false;
            }
        }

        public IAssertWithResult<T> WithResult<T>(T resultOnSuccess, T resultOnFailure)
        {
            return new AssertWithResult<T>(_throw, resultOnSuccess, resultOnFailure);
        }
    }
}
