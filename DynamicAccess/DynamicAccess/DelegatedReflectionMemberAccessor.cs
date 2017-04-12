using System;
using System.Collections.Generic;

namespace DynamicAccess
{
    public class DelegatedReflectionMemberAccessor : IMemberAccessor
    {
        private static Dictionary<string, INamedMemberAccessor> accessorCache = new Dictionary<string, INamedMemberAccessor>();

        public object GetValue(object instance, string memberName)
        {
            return  FindAccessor(instance, memberName).GetValue(instance);
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            FindAccessor(instance, memberName).SetValue(instance, newValue);
        }

        private INamedMemberAccessor FindAccessor(object instance, string memberName)
        {
            var type = instance.GetType();
            var key = type.FullName + memberName;
            INamedMemberAccessor accessor;
            accessorCache.TryGetValue(key, out accessor);
            if (accessor == null)
            {
                var propertyInfo = type.GetProperty(memberName);
                accessor = Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType), type, memberName) as INamedMemberAccessor;
                accessorCache.Add(key, accessor);
            }

            return accessor;
        }
    }

    internal interface INamedMemberAccessor
    {
        object GetValue(object instance);

        void SetValue(object instance, object newValue);
    }

    internal class PropertyAccessor<T, P> : INamedMemberAccessor
    {
        private Func<T, P> GetValueDelegate;
        private Action<T, P> SetValueDelegate;

        public PropertyAccessor(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                GetValueDelegate = (Func<T, P>)Delegate.CreateDelegate(typeof(Func<T, P>), propertyInfo.GetGetMethod());
                SetValueDelegate = (Action<T, P>)Delegate.CreateDelegate(typeof(Action<T, P>), propertyInfo.GetSetMethod());
            }
        }

        public object GetValue(object instance)
        {
            return GetValueDelegate((T)instance);
        }

        public void SetValue(object instance, object newValue)
        {
            SetValueDelegate((T)instance, (P)newValue);
        }
    }
}
