using Microsoft.Extensions.Logging;

namespace TornBlackMarket.Common.Util
{
    public static class GenericsUtil
    {
        public static object? CallStaticMethod<T>(string methodName, ILogger logger, params object[] args) where T : class
        {
            var type = typeof(T);
            var methodInfo = type.GetMethod(methodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            if (methodInfo != null)
            {
                return methodInfo.Invoke(null, args);
            }
            else
            {
                logger.LogError("Attempt to call method {MethodName} on class {ClassName} failed. Method not found", methodName, type.Name);
                throw new InvalidOperationException($"Method {type.Name}.{methodName} not found");
            }
        }
    }
}
