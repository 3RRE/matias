using System.IO;
using System.Web.Mvc;

namespace CapaPresentacion.Utilitarios
{
    public class RazorViewHelper
    {
        public string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
        {
            ViewEngineResult viewEngineResult;

            if (partial)
            {
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            }
            else
            {
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);
            }

            if (viewEngineResult == null)
            {
                throw new FileNotFoundException("View cannot be found.");
            }

            IView view = viewEngineResult.View;

            context.Controller.ViewData.Model = model;

            string result = null;

            using (StringWriter stringWriter = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, stringWriter);

                view.Render(viewContext, stringWriter);

                result = stringWriter.ToString();
            }

            return result;
        }
    }
}