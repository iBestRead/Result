using iBestRead.Results;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// 转换iBestRead.Results.Result为Microsoft.AspNetCore.Mvc.ActionResult
        /// </summary>
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
            Result<T> result)
        {
            if (result.Status == ResultStatus.NotFound) return controller.NotFound();
            if (result.Status == ResultStatus.Invalid)
            {
                foreach (var error in result.ValidationErrors)
                {
                    controller.ModelState.AddModelError(error.Identifier, error.ErrorMessage);
                }
                return controller.BadRequest(controller.ModelState);
            }

            return controller.Ok(result.Value);
        }
    }
}