using System.Linq;
using System.Reflection;

namespace BadMelon.Data.Helpers
{
    public static class EntityCopier
    {
        public static void Copy<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            foreach (PropertyInfo property in typeof(TSource).GetProperties().Where(p => p.CanWrite))
            {
                if (property.Name != "ID")
                    property.SetValue(target, property.GetValue(source, null), null);
            }
        }
    }
}