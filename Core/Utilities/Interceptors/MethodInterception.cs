using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception:MethodInterceptionBaseAttribute
    {
        //Invocation  : Business method(Add,GetList)
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation,Exception e) { }
        protected virtual void OnSuccess(IInvocation invocation) { }

        public override void Intercept(IInvocation invocation)
        {
            var IsSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                IsSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (IsSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }

    }
}
