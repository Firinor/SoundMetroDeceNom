using System;

namespace FirInstanceCell
{
    public class InstanceCell<T>
    {
        private T value;
        public void SetValue(T value)
        {
            if (value == null)
                throw new Exception("You cannot pass a null value!");

            this.value = value;
        }
        public T GetValue()
        {
            if (value == null)
                throw new Exception("At the moment, the value has not yet been assigned!");

            return value;
        }

        public bool isValueNull()
        {
            return value == null;
        }
    }
}