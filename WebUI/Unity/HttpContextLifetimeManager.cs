using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;

namespace WebUI.Unity
{
    public class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        private Guid _itemKey = Guid.NewGuid();

        public override object GetValue()
        {
            return HttpContext.Current.Items[_itemKey];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(_itemKey);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[_itemKey] = newValue;
        }

        public void Dispose()
        {
            RemoveValue();
        }

        public static void DisposeAllObjects()
        {
            foreach (var item in HttpContext.Current.Items)
            {
                if (typeof(IDisposable).IsAssignableFrom(item.GetType()))
                {
                    (item as IDisposable).Dispose();
                }
            }
        }
    }
}