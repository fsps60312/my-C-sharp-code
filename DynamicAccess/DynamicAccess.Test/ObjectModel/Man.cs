using System;

namespace DynamicAccess.Test.ObjectModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Man : IMemberAccessor
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public decimal Salary { get; set; }

        public bool Married { get; set; }

        public object GetValue(object instance, string memberName)
        {
            var man = instance as Man;
            if (man != null)
            {
                switch (memberName)
                {
                    case "Name": return man.Name;
                    case "Age": return man.Age;
                    case "Birthday": return man.Birthday;
                    case "Weight": return man.Weight;
                    case "Height": return man.Height;
                    case "Salary": return man.Salary;
                    case "Married": return man.Married;
                    default:
                        return null;
                }
            }
            else
                throw new InvalidProgramException();
        }

        public void SetValue(object instance, string memberName, object newValue)
        {
            var man = instance as Man;
            if (man != null)
            {
                switch (memberName)
                {
                    case "Name": man.Name = newValue as string; break;
                    case "Age": man.Age = Convert.ToInt32(newValue); break;
                    case "Birthday": man.Birthday = Convert.ToDateTime(newValue); break;
                    case "Weight": man.Weight = Convert.ToDouble(newValue); break;
                    case "Height": man.Height = Convert.ToDouble(newValue); break;
                    case "Salary": man.Salary = Convert.ToDecimal(newValue); break;
                    case "Married": man.Married = Convert.ToBoolean(newValue); break;
                }
            }
            else
                throw new InvalidProgramException();
        }
    }
}
