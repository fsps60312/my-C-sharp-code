namespace DynamicAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionMemberAccessor : IMemberAccessor
    {
        public object GetValue(object instance, string memberName)
        {
            var propertyInfo = instance.GetType().GetProperty(memberName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(instance, null);
            }

            return null;
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            var propertyInfo = instance.GetType().GetProperty(memberName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, newValue, null);
            }
        }
    }
}
