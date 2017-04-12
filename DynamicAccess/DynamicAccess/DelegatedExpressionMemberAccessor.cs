using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DynamicAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegatedExpressionMemberAccessor : IMemberAccessor
    {
        private Dictionary<string, Func<object, object>> getValueDelegates = new Dictionary<string, Func<object, object>>();
        private Dictionary<string, Action<object, object>> setValueDelegates = new Dictionary<string, Action<object, object>>();

        public object GetValue(object instance, string memberName)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            Func<object, object> getValueDelegate;
            getValueDelegates.TryGetValue(key, out getValueDelegate);
            if (getValueDelegate == null)
            {
                var info = type.GetProperty(memberName);
                var target = Expression.Parameter(typeof(object), "target");

                var getter = Expression.Lambda(typeof(Func<object, object>),
                    Expression.Convert(Expression.Property(Expression.Convert(target, type), info), typeof(object)),
                    target
                    );

                getValueDelegate = (Func<object, object>)getter.Compile();
                getValueDelegates.Add(key, getValueDelegate);
            }

            return getValueDelegate(instance);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            Action<object, object> setValueDelegate;
            setValueDelegates.TryGetValue(key, out setValueDelegate);
            if (setValueDelegate == null)
            {
                var info = type.GetProperty(memberName);
                var target = Expression.Parameter(typeof(object), "target");
                var value = Expression.Parameter(typeof(object), "value");

                var getter = Expression.Lambda(typeof(Action<object, object>),
                    Expression.Assign(Expression.Property(Expression.Convert(target, type), info), Expression.Convert(value, info.PropertyType)),
                    target, value
                    );

                setValueDelegate = (Action<object, object>)getter.Compile();
                setValueDelegates.Add(key, setValueDelegate);
            }

            setValueDelegate(instance, newValue);
        }
    }
}
