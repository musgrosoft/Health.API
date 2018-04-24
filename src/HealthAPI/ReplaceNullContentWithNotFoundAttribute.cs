//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.AspNet.OData;
//
//namespace HealthAPI
//{
//    internal class ReplaceNullContentWithNotFoundAttribute : EnableQueryAttribute
//    {
//        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
//        {
//            base.OnActionExecuted(actionExecutedContext);
//
//            HttpResponseMessage httpResponseMessage = actionExecutedContext.Response;
//            if (httpResponseMessage.IsSuccessStatusCode && IsContentMissingValue(httpResponseMessage))
//            {
//                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotFound);
//            }
//        }
//
//        private static bool IsContentMissingValue(HttpResponseMessage httpResponseMessage)
//        {
//            var objectContent = httpResponseMessage.Content as ObjectContent;
//            if (objectContent == null)
//            {
//                return false;
//            }
//
//            var type = GetType(objectContent);
//            return type == typeof(SingleResult<>) && objectContent.Value == null;
//        }
//
//        private static Type GetType(ObjectContent objectContent)
//        {
//            var type = objectContent.ObjectType;
//            if (type.IsGenericType && !type.IsGenericTypeDefinition)
//            {
//                var genericTypeDefinition = type.GetGenericTypeDefinition();
//                type = genericTypeDefinition;
//            }
//            return type;
//        }
//    }
//}
