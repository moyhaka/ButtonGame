using ButtonGrid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ButtonGrid.Controllers
{
    public class ButtonController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();
        Random r = new Random();
        const int GRID_SIZE = 25;
        public IActionResult Index()
        {
            if(buttons.Count < GRID_SIZE){
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    buttons.Add(new ButtonModel { Id = i, ButtonState = r.Next(4) });
                }
            }
            
            
            return View("Index", buttons);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            //from string to int
            int bt = int.Parse(buttonNumber);

            buttons.ElementAt(bt).ButtonState = (buttons.ElementAt(bt).ButtonState+1) % 4;

            return View("index", buttons);
        }

        public IActionResult ShowOneButton(int buttonNumber)
        {
            //add one to the button state, if > 4, then reset to 0
            buttons.ElementAt(buttonNumber).ButtonState = (buttons.ElementAt(buttonNumber).ButtonState + 1) %4;

            //1. render button and save it to a string
            string buttonString = RenderRazorViewToString(this, "ShowOneButton",buttons.ElementAt(buttonNumber));
            //2. Generate a win or loss string based on the satate of the buttons array
            bool victory = true;
            for(int i=0; i<buttons.Count; i++)
            {
                if (buttons.ElementAt(i).ButtonState != buttons.ElementAt(0).ButtonState)
                    victory = false;
            }

            String messageString;
            if (victory)
                messageString = "<p> Congratulations. All buttons are the same color</p>";
            else
                messageString = "<p>Not all buttons are the same color. See if you can make them match</p>";
            //3. Assemble a JSON string that has two parts (button string html an diwn loss message
            var package = new { part1 = buttonString, part2 = messageString };

            //4. Send the JSON result
            return Json(package);

            //5. In the site.js file, the data wil have to be interpreted as two pieces of data instead of one.

            return PartialView(buttons.ElementAt(buttonNumber));
        }

        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
