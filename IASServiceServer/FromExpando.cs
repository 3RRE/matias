using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;

namespace IASServiceServer
{
    public class FromExpando : KnownSourceInjection<ExpandoObject>
    {
        protected override void Inject(ExpandoObject source, object target)
        {
            var d = source as IDictionary<string, object>;
            if (d == null) return;
            var props = target.GetProps();
            foreach (var o in d)
            {
                var tp = props.FirstOrDefault( x=> x.Name == o.Key);
                if (tp == null) continue;
                tp.SetValue(target, o.Value);
            }
        }
    }
}
